using NuBot.Adapters;
using System.Threading;
using System.Threading.Tasks;

namespace NuBot.Automation
{
    public interface IRobotEngine
    {
        IAdapter Adapter { get; }

        void RegisterExecutor(IContextExecutor executor);

        Task RunAsync(CancellationToken cancellationToken);
    }
}
