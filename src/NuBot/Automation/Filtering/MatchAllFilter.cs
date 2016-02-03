using System.Collections.Generic;
using NuBot.Automation.Messages;

namespace NuBot.Automation.Filtering
{
    internal sealed class MatchAllFilter<T> : IFilter<T> where T : IMessage
    {
        public IDictionary<string, string> GetOutput(T data)
        {
            return new Dictionary<string, string>();
        }

        public bool IsMatch(T data)
        {
            return true;
        }
    }
}