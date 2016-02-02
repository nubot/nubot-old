using System.Runtime.Serialization;

namespace NuBot.Adapters.Slack.Models.Events
{
    [DataContract]
    internal sealed class TeamJoin : Event
    {
        [DataMember(Name = "user")]
        public User User { get; set; }
    }
}