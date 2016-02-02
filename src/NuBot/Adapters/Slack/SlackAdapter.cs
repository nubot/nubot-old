using NuBot.Adapters.Slack.Models;
using NuBot.Adapters.Slack.Models.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Runtime.Serialization.Json;
using System.Threading;
using System.Threading.Tasks;
using NuBot.Automation.Messages;

namespace NuBot.Adapters.Slack
{
    public class SlackAdapter : Adapter
    {
        private static readonly string ChatPostMessageUrl = "https://slack.com/api/chat.postMessage";
        private static readonly string RtmStartUrl = "https://slack.com/api/rtm.start";

        private readonly string _accessToken;
        private readonly ArraySegment<byte> _buffer;
        private RtmStart _rtm;

        // State data
        private readonly IList<User> _teamMembers; 

        public SlackAdapter(string accessToken)
        {
            _accessToken = accessToken;
            _buffer = WebSocket.CreateClientBuffer(256, 256);

            // State
            _teamMembers = new List<User>();
        }

        public override string UserName => _rtm?.Self?.Name;

        public override async Task SetupAsync()
        {
            _rtm = await StartRtmAsync();

            // Add users to our local cache of team members
            foreach (var user in _rtm.Users)
            {
                _teamMembers.Add(user);
            }
        }

        public override async Task RunAsync(CancellationToken cancellationToken)
        {
            using (var ws = new ClientWebSocket())
            {
                await ws.ConnectAsync(_rtm.WebSocketUri, cancellationToken);

                while (!cancellationToken.IsCancellationRequested)
                {
                    using (var ms = new MemoryStream())
                    {
                        WebSocketReceiveResult result;

                        do
                        {
                            result = await ws.ReceiveAsync(_buffer, cancellationToken);
                            await ms.WriteAsync(_buffer.Array, _buffer.Offset, result.Count, cancellationToken);
                        } while (!result.EndOfMessage);

                        ms.Seek(0, SeekOrigin.Begin);
                        HandleEvent(ms);
                    }
                }
            }
        }

        public override async Task SendAsync(string channel, string message)
        {
            using (var client = new HttpClient())
            {
                var d = new Dictionary<string, string>
                {
                    { "token", _accessToken },
                    { "as_user", "True" },
                    { "channel", channel },
                    { "text", message }
                };

                await client.PostAsync(ChatPostMessageUrl, new FormUrlEncodedContent(d));
            }
        }

        private async Task<RtmStart> StartRtmAsync()
        {
            var serializer = new DataContractJsonSerializer(typeof(RtmStart));

            using (var client = new HttpClient())
            {
                var url = $"{RtmStartUrl}?token={_accessToken}";
                var stream = await client.GetStreamAsync(url);

                return serializer.ReadObject(stream) as RtmStart;
            }
        }

        private void HandleEvent(Stream stream)
        {
            var serializer = new DataContractJsonSerializer(typeof(Event));
            var ev = serializer.ReadObject(stream) as Event;

            if(ev == null)
            {
                // TODO: Handle case when we did not receive an object with
                // type attribute.
                return;
            }

            // Reset stream
            stream.Seek(0, SeekOrigin.Begin);

            switch(ev.Type)
            {
                case SlackConstants.Events.Message:
                    OnMessage(stream);
                    break;
                case SlackConstants.Events.TeamJoin:
                    OnTeamJoin(stream);
                    break;
            }
        }

        private void OnMessage(Stream stream)
        {
            var serializer = new DataContractJsonSerializer(typeof(Message));
            var message = serializer.ReadObject(stream) as Message;

            if(message == null)
            {
                // TODO: What happens when we cannot deserialize a message.
                return;
            }

            // Ignore any messages we sent ourselves.
            if(message.UserId == _rtm.Self.Id)
            {
                // TODO: Log.
                return;
            }

            switch(message.SubType)
            {
                case SlackConstants.MessageSubTypes.ChannelJoin:
                    var userId = message.UserId;
                    var user = _teamMembers.SingleOrDefault(u => u.Id == userId);

                    if (string.IsNullOrEmpty(user?.Name))
                    {
                        // TODO: Not a team member? What to do?
                        return;
                    }

                    Emit(new ChannelJoinMessage {ChannelId = message.ChannelId, UserId = userId, UserName = user.Name});
                    break;

                // Null or empty string means a regular message
                case null:
                case "":
                    // TODO: If this is a DM, we need to prepend the bot name.
                    var content = message.Text;

                    if(message.ChannelId.StartsWith("D"))
                    {
                        content = $"{_rtm.Self.Name} {content}";
                    }

                    Emit(new TextMessage { ChannelId = message.ChannelId, Content = content });
                    break;
            }
        }

        private void OnTeamJoin(Stream stream)
        {
            var serializer = new DataContractJsonSerializer(typeof(TeamJoin));
            var teamJoin = serializer.ReadObject(stream) as TeamJoin;

            if (teamJoin == null)
            {
                // TODO: What happens when we cannot deserialize a message.
                return;
            }

            _teamMembers.Add(teamJoin.User);
        }
    }
}
