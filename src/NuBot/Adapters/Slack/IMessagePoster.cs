using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NuBot.Adapters.Slack
{
    public interface IMessagePoster
    {
        Task SendAsync(string channelId, string message);
    }

    internal sealed class MessagePoster : IMessagePoster
    {
        private static readonly string ChatPostMessageUrl = "https://slack.com/api/chat.postMessage";
        private readonly string _token;

        public MessagePoster(string token)
        {
            if (token == null) throw new ArgumentNullException(nameof(token));
            _token = token;
        }

        public async Task SendAsync(string channelId, string message)
        {
            using (var client = new HttpClient())
            {
                var d = new Dictionary<string, string>
                {
                    {"token", _token},
                    {"as_user", "True"},
                    {"channel", channelId},
                    {"text", message}
                };

                await client.PostAsync(ChatPostMessageUrl, new FormUrlEncodedContent(d));
            }
        }
    }
}