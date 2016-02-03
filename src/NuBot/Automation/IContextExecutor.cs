namespace NuBot.Automation
{
    public interface IContextExecutor
    {
        void Execute(IExecutionRequest request);
    }
}