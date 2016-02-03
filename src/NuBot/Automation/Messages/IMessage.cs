using NuBot.Automation.Contexts;

namespace NuBot.Automation.Messages
{
    public interface IMessage
    {
        IChannel Channel { get; } 

        IUser User { get; }
    }
}