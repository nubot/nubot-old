using NuBot.Adapters;

namespace NuBot.Automation
{
    internal sealed class ExecutionRequest : IExecutionRequest
    {
        public ExecutionRequest(IAdapter adapter, object dataSource = null)
        {
            Adapter = adapter;
            DataSource = dataSource;
        }

        public IAdapter Adapter { get; }

        public object DataSource { get; }

        public T GetDataSource<T>() where T : class
        {
            return (T) DataSource;
        }
    }
}