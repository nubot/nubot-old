using NuBot.Automation;
using NuBot.Automation.Contexts;

namespace NuBot.Adapters.Slack
{
    internal sealed class User : IUser
    {
        public User(string id, string userName)
        {
            Id = id;
            Name = userName;
        }

        public string Id { get; set; }

        public string Name { get; }
    }
}