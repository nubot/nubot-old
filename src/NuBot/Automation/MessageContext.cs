using NuBot.Adapters;
using NuBot.Automation.MessageHandlers;
using System;

namespace NuBot.Automation
{
    internal class MessageContext<T> where T : IMessage
    {
        private readonly IMessageHandler<T> _handler;
        private readonly Action<IContext> _callback;

        public MessageContext(IMessageHandler<T> handler, Action<IContext> callback)
        {
            _handler = handler;
            _callback = callback;
        }

        public IMessageHandler<T> MessageHandler => _handler;

        public Action<IContext> Callback => _callback;
    }
}
