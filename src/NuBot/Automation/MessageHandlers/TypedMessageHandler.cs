using System.Collections.Generic;
using NuBot.Automation.Messages;

namespace NuBot.Automation.MessageHandlers
{
    internal sealed class TypedMessageHandler<T> : IMessageHandler<T> where T : IMessage
    {
        public bool CanHandle(T message)
        {
            return true;
        }

        public IDictionary<string, string> Handle(T message)
        {
            return new Dictionary<string, string>();
        }
    }
}