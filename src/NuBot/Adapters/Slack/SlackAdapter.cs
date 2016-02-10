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
using NuBot.Adapters.Slack.Messages;
using NuBot.Automation;
using NuBot.Automation.Contexts;
using NuBot.Automation.Messages;

namespace NuBot.Adapters.Slack
{
    public class SlackAdapter : Adapter
    {
        private static readonly string RtmStartUrl = "https://slack.com/api/rtm.start";

        private readonly string _accessToken;
        private readonly ArraySegment<byte> _buffer;
        private RtmStart _rtm;

        // State data
        private readonly IList<Channel> _channels; 
        private readonly IList<User> _users; 

        public SlackAdapter(string accessToken)
        {
            _accessToken = accessToken;
            _buffer = WebSocket.CreateClientBuffer(256, 256);

            // State
            _channels = new List<Channel>();
            _users = new List<User>();
        }

        public override IEnumerable<IChannel> Channels => _channels.ToList();

        public override IEnumerable<IUser> Users => _users.ToList();

        public override string UserName => _rtm?.Self?.Name;

        public override async Task SetupAsync()
        {
            _rtm = await StartRtmAsync();

            // Create channels
            foreach (var channel in _rtm.Channels)
            {
                _channels.Add(new Channel(channel.Id, channel.Name, new MessagePoster(_accessToken)));
            }

            foreach (var group in _rtm.Groups)
            {
                _channels.Add(new Channel(group.Id, group.Name, new MessagePoster(_accessToken)));
            }

            foreach (var im in _rtm.Ims)
            {
                _channels.Add(new Channel(im.Id, $"IM with {im.UserId}", new MessagePoster(_accessToken)));
            }

            // Add users to our local cache of team members
            foreach (var user in _rtm.Users)
            {
                _users.Add(new User(user.Id, user.Name));
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

            switch (message.SubType)
            {
                case SlackConstants.MessageSubTypes.ChannelJoin:
                {
                    var userId = message.UserId;
                    var user = _users.SingleOrDefault(u => u.Id == userId);

                    if (string.IsNullOrEmpty(user?.Name))
                    {
                        // TODO: Not a team member? What to do?
                        return;
                    }

                    Emit<IChannelJoinMessage>(
                        new SlackChannelJoinMessage(
                            _channels.Single(c => c.Id == message.ChannelId),
                            _users.Single(u => u.Id == message.UserId)));
                    break;
                }

                case SlackConstants.MessageSubTypes.ChannelLeave:
                {
                    var userId = message.UserId;
                    var user = _users.SingleOrDefault(u => u.Id == userId);

                    if (string.IsNullOrEmpty(user?.Name))
                    {
                        // TODO: Not a team member? What to do?
                        return;
                    }

                    Emit<IChannelLeaveMessage>(
                        new SlackChannelLeaveMessage(
                            _channels.Single(c => c.Id == message.ChannelId),
                            _users.Single(u => u.Id == message.UserId)));
                    break;
                }

                // Null or empty string means a regular message
                case null:
                case "":
                    // TODO: If this is a DM, we need to prepend the bot name.
                    var content = message.Text;

                    if(message.ChannelId.StartsWith("D"))
                    {
                        content = $"{_rtm.Self.Name} {content}";
                    }

                    Emit<ITextMessage>(
                        new SlackTextMessage(
                            _channels.Single(c => c.Id == message.ChannelId),
                            _users.Single(u => u.Id == message.UserId),
                            content));
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

            _users.Add(new User(teamJoin.User.Id, teamJoin.User.Name));
        }
    }
}
