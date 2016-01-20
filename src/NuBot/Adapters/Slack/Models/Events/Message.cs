using System.Runtime.Serialization;

namespace NuBot.Adapters.Slack.Models.Events
{
    [DataContract]
    internal class Message : Event
    {
        [DataMember(Name = "ts")]
        public string Timestamp { get; set; }

        [DataMember(Name = "channel")]
        public string ChannelId { get; set; }

        [DataMember(Name = "user")]
        public string UserId { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }

        [DataMember(Name = "subtype")]
        public string SubType { get; set; }
    }
}
