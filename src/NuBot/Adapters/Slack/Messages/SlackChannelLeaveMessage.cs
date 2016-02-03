using NuBot.Automation.Contexts;
using NuBot.Automation.Messages;

namespace NuBot.Adapters.Slack.Messages
{
    internal sealed class SlackChannelLeaveMessage : IChannelLeaveMessage
    {
        public SlackChannelLeaveMessage(IChannel channel, IUser user)
        {
            Channel = channel;
            User = user;
        }

        public IChannel Channel { get; }

        public IUser User { get; }
    }
}