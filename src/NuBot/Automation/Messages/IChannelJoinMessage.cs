namespace NuBot.Automation.Messages
{
    public interface IChannelJoinMessage : IMessage
    {
        string UserId { get; }

        string UserName { get; }
    }
}