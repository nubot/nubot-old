using NuBot.Adapters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NuBot.Automation
{
    internal sealed class Context : IContext
    {
        private readonly IAdapter _adapter;
        private readonly IMessage _message;
        private readonly IDictionary<string, string> _parameters;

        public Context(IAdapter adapter, IMessage message, IDictionary<string, string> parameters)
        {
            _adapter = adapter;
            _message = message;
            _parameters = parameters;
        }

        public IDictionary<string, string> Parameters
        {
            get { return _parameters; }
        }

        public Task SendAsync(string format, params object[] parameters)
        {
            return _adapter.SendAsync(_message.ChannelId, string.Format(format, parameters));
        }
    }
}
