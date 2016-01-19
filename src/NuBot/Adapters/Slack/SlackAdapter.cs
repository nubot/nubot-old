using NuBot.Adapters.Slack.Models;
using NuBot.Adapters.Slack.Models.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.WebSockets;
using System.Runtime.Serialization.Json;
using System.Threading;
using System.Threading.Tasks;

namespace NuBot.Adapters.Slack
{
    public class SlackAdapter : Adapter
    {
        private static readonly string ChatPostMessageUrl = "https://slack.com/api/chat.postMessage";
        private static readonly string RtmStartUrl = "https://slack.com/api/rtm.start";

        private readonly string _accessToken;
        private RtmStart _rtm;

        public SlackAdapter(string accessToken)
        {
            _accessToken = accessToken;
        }

        public override string UserName
        {
            get { return _rtm?.Self?.Name; }
        }

        public async override Task SetupAsync()
        {
            _rtm = await StartRtmAsync();
        }

        public override async Task RunAsync(CancellationToken cancellationToken)
        {
            using (var ws = new ClientWebSocket())
            {
                await ws.ConnectAsync(_rtm.WebSocketUri, cancellationToken);

                var buffer = new byte[4096];
                var segment = new ArraySegment<byte>(buffer);

                while(!cancellationToken.IsCancellationRequested)
                {
                    var result = await ws.ReceiveAsync(segment, cancellationToken);
                    HandleEvent(segment.Array, 0, result.Count);
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

        private void HandleEvent(byte[] buffer, int index, int count)
        {
            var serializer = new DataContractJsonSerializer(typeof(Event));

            using (var stream = new MemoryStream(buffer, index, count))
            {
                var ev = serializer.ReadObject(stream) as Event;

                if(ev == null)
                {
                    // TODO: Handle case when we did not receive an object with
                    // type attribute.
                    return;
                }

                stream.Seek(0, SeekOrigin.Begin);

                switch(ev.Type)
                {
                    case "message":
                        OnMessage(stream);
                        break;
                }
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
                default:
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
    }
}
