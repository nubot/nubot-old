using NuBot.Adapters;

namespace NuBot.Automation
{
    public interface IExecutionRequest
    {
        IAdapter Adapter { get; }

        object DataSource { get; }

        T GetDataSource<T>() where T : class;
    }
}