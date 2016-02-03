using System;
using System.Collections.Generic;
using System.Linq;

namespace NuBot.Adapters
{
    internal static class CallbackContainer<T>
    {
        private static readonly IList<Action<T>> Callbacks;

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
