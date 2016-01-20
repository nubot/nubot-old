using System.Runtime.Serialization;

namespace NuBot.Adapters.Gitter.Models
{
    [DataContract]
    internal sealed class Message
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "fromUser")]
        public User FromUser { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }
    }
}
