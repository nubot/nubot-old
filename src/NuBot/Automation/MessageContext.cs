using NuBot.Automation.MessageHandlers;
using System;
using NuBot.Automation.Messages;

namespace NuBot.Automation
{
    internal class MessageContext<T> where T : IMessage
    {
        public MessageContext(IMessageHandler<T> handler, Action<IContext<T>> callback)
        {
            Callback = callback;
            MessageHandler = handler;
        }

        public Action<IContext<T>> Callback { get; }

        public IMessageHandler<T> MessageHandler { get; }
    }
}
