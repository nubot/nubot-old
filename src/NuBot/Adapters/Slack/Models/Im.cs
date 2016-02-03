using System.Runtime.Serialization;

namespace NuBot.Adapters.Slack.Models
{
    [DataContract]
    public class Im
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "user")]
        public string UserId { get; set; }
    }
}