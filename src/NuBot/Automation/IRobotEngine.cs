using NuBot.Adapters;
using NuBot.Automation.MessageHandlers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NuBot.Automation
{
    public interface IRobotEngine
    {
        IAdapter Adapter { get; }

        void RegisterHandler<T>(IMessageHandler<T> handler, Action<IContext> callback) where T : IMessage;

        Task RunAsync(CancellationToken cancellationToken);
    }
}
