using System.Runtime.Serialization;

namespace NuBot.Adapters.Gitter.Models
{
    [DataContract]
    internal sealed class Room
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
    }
}
