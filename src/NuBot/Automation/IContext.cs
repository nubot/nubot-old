using System.Collections.Generic;
using System.Threading.Tasks;

namespace NuBot.Automation
{
    public interface IContext
    {
        IDictionary<string, string> Parameters { get; }

        Task SendAsync(string format, params object[] parameters);
    }
}
