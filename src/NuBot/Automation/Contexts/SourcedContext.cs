using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using NuBot.Adapters;
using NuBot.Automation.Messages;

namespace NuBot.Automation.Contexts
{
    internal sealed class SourcedContext<TSource> : ISourcedContext<TSource> where TSource : IMessage
    {
        private readonly IAdapter _adapter;

        public SourcedContext(IAdapter adapter, TSource source, IDictionary<string, string> parameters)
        {
            if (adapter == null) throw new ArgumentNullException(nameof(adapter));
            if (source == null) throw new ArgumentNullException(nameof(source));

            _adapter = adapter;

            Parameters = new ReadOnlyDictionary<string, string>(
                parameters ?? new Dictionary<string, string>());
            Source = source;
        }

        public IEnumerable<IChannel> Channels { get; }

        public IReadOnlyDictionary<string, string> Parameters { get; set; }

        public TSource Source { get; }

        public IEnumerable<IUser> Users { get; }

        public async Task BroadcastAsync(string format, params object[] parameters)
        {
            foreach (var channel in Channels)
            {
                await channel.SendAsync(format, parameters);
            }
        }

        public Task RespondAsync(string format, params object[] parameters)
        {
            throw new System.NotImplementedException();
        }

        public async Task SendAsync(string format, params object[] parameters)
        {
            await Source.Channel.SendAsync(format, parameters);
        }
    }
}