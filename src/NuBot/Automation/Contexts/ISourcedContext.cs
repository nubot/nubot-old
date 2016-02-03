using System.Collections.Generic;
using System.Threading.Tasks;
using NuBot.Automation.Messages;

namespace NuBot.Automation.Contexts
{
    public interface ISourcedContext<out TSource> : IContext where TSource : IMessage
    {
        IReadOnlyDictionary<string, string> Parameters { get; }
        
        TSource Source { get; }

        Task RespondAsync(string format, params object[] parameters);

        Task SendAsync(string format, params object[] parameters);
    }
}