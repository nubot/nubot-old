using System.Runtime.Serialization;

namespace NuBot.Adapters.Slack.Models
{
    [DataContract]
    internal sealed class Group
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "is_group")]
        public bool IsGroup { get; set; }

        [DataMember(Name = "members")]
        public string[] Members { get; set; }
    }
}