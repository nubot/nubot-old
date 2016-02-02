namespace NuBot.Automation.Messages
{
    public class TextMessage : ITextMessage
    {
        public string ChannelId { get; set; }

        public string Content { get; set; }
    }
}
