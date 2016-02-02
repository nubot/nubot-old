using System.Runtime.Serialization;

namespace NuBot.Adapters.Slack.Models
{
    [DataContract]
    internal sealed class User
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "deleted")]
        public bool Deleted { get; set; }
    }
}