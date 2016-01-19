using System.Runtime.Serialization;

namespace NuBot.Adapters.Slack.Models.Events
{
    [DataContract]
    internal class Event
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}
