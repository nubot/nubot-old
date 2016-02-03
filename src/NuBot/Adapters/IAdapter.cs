using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuBot.Automation;
using NuBot.Automation.Contexts;

namespace NuBot.Adapters
{
    public interface IAdapter
    {
        string UserName { get; }

        IEnumerable<IChannel> Channels { get; }

        IEnumerable<IUser> Users { get; }

        Task SetupAsync();

        void On<T>(Action<T> callback);

        Task RunAsync(CancellationToken cancellationToken);
    }
}
