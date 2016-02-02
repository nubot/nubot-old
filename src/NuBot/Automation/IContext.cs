using System.Collections.Generic;
using System.Threading.Tasks;
using NuBot.Automation.Messages;

namespace NuBot.Automation
{
    public interface IContext<out TMessage> where TMessage : IMessage
    {
        TMessage Message { get; }

        IDictionary<string, string> Parameters { get; }

        Task SendAsync(string format, params object[] parameters);
    }
}
