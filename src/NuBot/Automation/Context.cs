using NuBot.Adapters;
using System.Collections.Generic;
using System.Threading.Tasks;
using NuBot.Automation.Messages;

namespace NuBot.Automation
{
    internal sealed class Context<TMessage> : IContext<TMessage> where TMessage : IMessage
    {
        private readonly IAdapter _adapter;

        public Context(IAdapter adapter, TMessage message, IDictionary<string, string> parameters)
        {
            _adapter = adapter;
            Message = message;
            Parameters = parameters ?? new Dictionary<string, string>();
        }

        public TMessage Message { get; }

        public IDictionary<string, string> Parameters { get; }

        public Task SendAsync(string format, params object[] parameters)
        {
            return _adapter.SendAsync(Message.ChannelId, string.Format(format, parameters));
        }
    }
}
