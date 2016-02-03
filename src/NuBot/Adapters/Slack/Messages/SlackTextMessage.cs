using NuBot.Automation;
using NuBot.Automation.Contexts;
using NuBot.Automation.Messages;

namespace NuBot.Adapters.Slack.Messages
{
    internal sealed class SlackTextMessage : ITextMessage
    {
        public SlackTextMessage(IChannel channel, IUser user, string content)
        {
            Channel = channel;
            User = user;
            Content = content;
        }

        public IChannel Channel { get; }

        public string Content { get; }

        public IUser User { get; }
    }
}