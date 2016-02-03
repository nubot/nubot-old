using System.Linq;

namespace NuBot.Automation.Contexts
{
    public static class ContextExtensions
    {
        public static IChannel GetChannel(this IContext context, string name)
        {
            return context.Channels.FirstOrDefault(c => c.Name == name);
        }
    }
}