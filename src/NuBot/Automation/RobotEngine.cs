using NuBot.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NuBot.Automation.Messages;

namespace NuBot.Automation
{
    internal sealed class RobotEngine : IRobotEngine
    {
        private readonly IAdapter _adapter;
        private readonly Func<IRobot> _robotFunc;
        private readonly Func<IEnumerable<RobotPart>> _parts;
        private readonly IList<IContextExecutor> _contextExecutors; 

        public RobotEngine(IAdapter adapter, Func<IRobot> robotFunc, Func<IEnumerable<RobotPart>> parts)
        {
            _adapter = adapter;
            _robotFunc = robotFunc;
            _parts = parts;
            _contextExecutors = new List<IContextExecutor>();
        }

        public IAdapter Adapter => _adapter;

        public void RegisterExecutor(IContextExecutor executor)
        {
            _contextExecutors.Add(executor);
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            _adapter.On<IChannelJoinMessage>(OnChannelJoinMessage);
            _adapter.On<IChannelLeaveMessage>(OnChannelLeaveMessage);
            _adapter.On<ITextMessage>(OnTextMessage);

            await _adapter.SetupAsync();

            var robot = _robotFunc();

            foreach(var part in _parts())
            {
                part.Attach(robot);
            }

            do
            {
                await _adapter.RunAsync(cancellationToken);
            } while (!cancellationToken.IsCancellationRequested);
        }

        private void OnChannelJoinMessage(IChannelJoinMessage message)
        {
            var executors = _contextExecutors.OfType<SourcedContextExecutor<IChannelJoinMessage>>();
            RunExecutors(executors, message);
        }

        private void OnChannelLeaveMessage(IChannelLeaveMessage message)
        {
            var executors = _contextExecutors.OfType<SourcedContextExecutor<IChannelLeaveMessage>>();
            RunExecutors(executors, message);
        }

        private void OnTextMessage(ITextMessage message)
        {
            var executors = _contextExecutors
                .OfType<SourcedContextExecutor<ITextMessage>>()
                .Where(e => e.ShouldExecute(message));
            RunExecutors(executors, message);
        }

        private void RunExecutors(IEnumerable<IContextExecutor> executors, object dataSource = null)
        {
            foreach (var executor in executors)
            {
                try
                {
                    var request = new ExecutionRequest(_adapter, dataSource);
                    executor.Execute(request);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
