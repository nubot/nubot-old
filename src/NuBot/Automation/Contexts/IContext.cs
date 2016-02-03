using System.Collections.Generic;
using System.Threading.Tasks;

namespace NuBot.Automation.Contexts
{
    public interface IContext
    {
        IEnumerable<IChannel> Channels { get; }

        IEnumerable<IUser> Users { get; }

        Task BroadcastAsync(string format, params object[] parameters);
    }
}
