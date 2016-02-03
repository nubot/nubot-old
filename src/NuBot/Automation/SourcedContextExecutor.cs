using System;
using NuBot.Automation.Contexts;
using NuBot.Automation.Filtering;
using NuBot.Automation.Messages;

namespace NuBot.Automation
{
    public class SourcedContextExecutor<T> : IContextExecutor where T : IMessage
    {
        private readonly Action<ISourcedContext<T>> _contextCallback;
        private readonly IFilter<T> _filter;

        public SourcedContextExecutor(Action<ISourcedContext<T>> contextCallback, IFilter<T> filter)
        {
            if (contextCallback == null) throw new ArgumentNullException(nameof(contextCallback));
            _contextCallback = contextCallback;
            _filter = filter;
        }

        public void Execute(IExecutionRequest request)
        {
            var dataSource = (T) request.DataSource;
            var parameters = _filter.GetOutput(dataSource);

            var context = new SourcedContext<T>(
                request.Adapter,
                dataSource,
                parameters);

            _contextCallback(context);
        }

        public bool ShouldExecute(T message)
        {
            return _filter.IsMatch(message);
        }
    }
}