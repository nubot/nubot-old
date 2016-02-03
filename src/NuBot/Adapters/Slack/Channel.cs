using System;
using System.Threading.Tasks;
using NuBot.Automation;
using NuBot.Automation.Contexts;

namespace NuBot.Adapters.Slack
{
    internal sealed class Channel : IChannel
    {
        private readonly IMessagePoster _messagePoster;

        public Channel(string id, string name, IMessagePoster messagePoster)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (messagePoster == null) throw new ArgumentNullException(nameof(messagePoster));

            _messagePoster = messagePoster;

            Id = id;
            Name = name;
        }

        public string Id { get; }

        public string Name { get; }

        public async Task RespondAsync(IUser user, string format, params object[] parameters)
        {
            await _messagePoster.SendAsync(Id, $"@{user.Name}: {string.Format(format, parameters)}");
        }

        public async Task SendAsync(string format, params object[] parameters)
        {
            await _messagePoster.SendAsync(Id, string.Format(format, parameters));
        }
    }
}