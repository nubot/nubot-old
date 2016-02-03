using System.Collections.Generic;
using NuBot.Automation.Messages;

namespace NuBot.Automation.Filtering
{
    public interface IFilter<in T> where T : IMessage
    {
        IDictionary<string, string> GetOutput(T data); 

        bool IsMatch(T data);
    }
}