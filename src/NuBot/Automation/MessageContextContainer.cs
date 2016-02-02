using NuBot.Adapters;
using System.Collections.Generic;
using System.Linq;
using NuBot.Automation.Messages;

namespace NuBot.Automation
{
    internal static class MessageContextContainer<T> where T : IMessage
    {
        private static readonly IList<MessageContext<T>> Contexts;

        static MessageContextContainer()
        {
            Contexts = new List<MessageContext<T>>();
        }

        public static void Add(MessageContext<T> context)
        {
            Contexts.Add(context);
        }

        public static IEnumerable<MessageContext<T>> GetAll()
        {
            return Contexts.ToList();
        }
    }
}
