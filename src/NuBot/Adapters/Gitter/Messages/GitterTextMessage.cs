using NuBot.Automation.Contexts;
using NuBot.Automation.Messages;

namespace NuBot.Adapters.Gitter.Messages
{
    internal sealed class GitterTextMessage : ITextMessage
    {
        public GitterTextMessage(IChannel channel, IUser user, string content)
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