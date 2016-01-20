using System;
using System.Threading;
using System.Threading.Tasks;

namespace NuBot.Adapters
{
    public interface IAdapter
    {
        string UserName { get; }

        Task SetupAsync();

        void On<T>(Action<T> callback) where T : IMessage;

        Task RunAsync(CancellationToken cancellationToken);

        Task SendAsync(string channel, string message);
    }
}
