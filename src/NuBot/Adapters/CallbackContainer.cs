using System;
using System.Collections.Generic;
using System.Linq;

namespace NuBot.Adapters
{
    internal static class CallbackContainer<T> where T : IMessage
    {
        private readonly static IList<Action<T>> Callbacks;

        static CallbackContainer()
        {
            Callbacks = new List<Action<T>>();
        }

        public static void Add(Action<T> callback)
        {
            Callbacks.Add(callback);
        }

        public static IEnumerable<Action<T>> GetAll()
        {
            return Callbacks.ToList();
        }
    }
}
