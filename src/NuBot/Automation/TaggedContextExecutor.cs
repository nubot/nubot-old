using System;
using NuBot.Automation.Contexts;

namespace NuBot.Automation
{
    public class TaggedContextExecutor : IContextExecutor
    {
        // Pre-defined tags
        public static readonly string Connected = "Connected";
        public static readonly string Disconnected = "Disconnected";

        private readonly Action<IContext> _contextCallback;

        public TaggedContextExecutor(Action<IContext> contextCallback, string tag)
        {
            if (contextCallback == null) throw new ArgumentNullException(nameof(contextCallback));
            if (tag == null) throw new ArgumentNullException(nameof(tag));
            _contextCallback = contextCallback;
            Tag = tag;
        }

        public string Tag { get; }

        public void Execute(IExecutionRequest request)
        {
            var context = new Context(request.Adapter);
            _contextCallback(context);
        }
    }
}