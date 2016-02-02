using System.Runtime.Serialization;

namespace NuBot.Adapters.Slack.Models
{
    [DataContract]
    internal sealed class Channel
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "is_channel")]
        public bool IsChannel { get; set; }

        [DataMember(Name = "members")]
        public string[] Members { get; set; }
    }
}