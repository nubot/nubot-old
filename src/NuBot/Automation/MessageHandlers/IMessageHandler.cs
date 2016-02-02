using NuBot.Adapters;
using System.Collections.Generic;
using NuBot.Automation.Messages;

namespace NuBot.Automation.MessageHandlers
{
    public interface IMessageHandler<T> where T : IMessage
    {
        bool CanHandle(T message);

        IDictionary<string, string> Handle(T message);
    }
}
