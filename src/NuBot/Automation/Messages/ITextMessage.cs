namespace NuBot.Automation.Messages
{
    public interface ITextMessage : IMessage
    {
        string Content { get; }
    }
}