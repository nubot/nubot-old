using System;
using System.Runtime.Serialization;

namespace NuBot.Adapters.Slack.Models
{
    [DataContract]
    internal sealed class RtmStart
    {
        [DataMember(Name = "ok")]
        public bool Ok { get; set; }

        [DataMember(Name = "self")]
        public Self Self { get; set; }

        [DataMember(Name = "users")]
        public User[] Users { get; set; }

        [DataMember(Name = "channels")]
        public Channel[] Channels { get; set; }

        [DataMember(Name = "url")]
        public Uri WebSocketUri { get; set; }
    }

    [DataContract]
    internal sealed class Self
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
