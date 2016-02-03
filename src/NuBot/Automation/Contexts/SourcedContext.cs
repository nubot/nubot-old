using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using NuBot.Adapters;
using NuBot.Automation.Messages;

namespace NuBot.Automation.Contexts
{
    internal sealed class SourcedContext<TSource> : Context, ISourcedContext<TSource> where TSource : IMessage
    {
        public SourcedContext(IAdapter adapter, TSource source, IDictionary<string, string> parameters)
            : base(adapter)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Parameters = new ReadOnlyDictionary<string, string>(
                parameters ?? new Dictionary<string, string>());
            Source = source;
        }

        public IReadOnlyDictionary<string, string> Parameters { get; set; }

        public TSource Source { get; }

        public async Task RespondAsync(string format, params object[] parameters)
        {
            await Source.Channel.RespondAsync(Source.User, format, parameters);
        }

        public async Task SendAsync(string format, params object[] parameters)
        {
            await Source.Channel.SendAsync(format, parameters);
        }
    }
}