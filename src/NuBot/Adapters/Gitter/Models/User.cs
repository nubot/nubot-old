using System.Runtime.Serialization;

namespace NuBot.Adapters.Gitter.Models
{
    [DataContract]
    internal sealed class User
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "username")]
        public string UserName { get; set; }

        [DataMember(Name = "displayName")]
        public string DisplayName { get; set; }
    }
}
