using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NuBot.Adapters;

namespace NuBot.Automation.Contexts
{
    internal abstract class Context : IContext
    {
        private readonly IAdapter _adapter;

        protected Context(IAdapter adapter)
        {
            if (adapter == null) throw new ArgumentNullException(nameof(adapter));
            _adapter = adapter;
        }

        public IEnumerable<IChannel> Channels => _adapter.Channels;

        public IEnumerable<IUser> Users => _adapter.Users;

        public async Task BroadcastAsync(string format, params object[] parameters)
        {
            foreach (var channel in Channels)
            {
                await channel.SendAsync(format, parameters);
            }
        }
    }
}