using System.Collections.Generic;
using NuBot.Automation.Contexts;

namespace NuBot.Automation.WebHooks
{
    public interface IWebHookContext : IContext
    {
        IReadOnlyDictionary<string, string> Parameters { get; }

        TModel GetContent<TModel>();
    }
}