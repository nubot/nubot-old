using System;
using System.Threading;
using System.Threading.Tasks;

namespace NuBot.Adapters
{
    public interface IAdapter
    {
        string UserName { get; }

        Task SetupAsync();

        void On<T>(Action<T> callback);

        Task RunAsync(CancellationToken cancellationToken);
    }
}
