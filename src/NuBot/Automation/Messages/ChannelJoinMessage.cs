namespace NuBot.Automation.Messages
{
    public class ChannelJoinMessage : IChannelJoinMessage
    {
        public string ChannelId { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }
    }
}