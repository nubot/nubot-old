namespace NuBot.Adapters
{
    public class TextMessage : IMessage
    {
        public string ChannelId
        {
            get; set;
        }

        public string Content { get; set; }
    }
}
