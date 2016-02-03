using NuBot.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NuBot.Automation.Messages;
using NuBot.Automation.WebHooks;
using NuBot.Http;

namespace NuBot.Automation
{
    internal sealed class RobotEngine : IRobotEngine
    {
        private readonly IAdapter _adapter;
        private readonly IHttpServer _httpServer;
        private readonly Func<IRobot> _robotFunc;
        private readonly Func<IEnumerable<RobotPart>> _parts;
        private readonly IList<IContextExecutor> _contextExecutors; 

        public RobotEngine(IAdapter adapter, IHttpServer httpServer, Func<IRobot> robotFunc, Func<IEnumerable<RobotPart>> parts)
        {
            _adapter = adapter;
            _httpServer = httpServer;
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

            // Set up HTTP
            _httpServer.OnRequest(OnHttpRequest);

            var runAdapter = _adapter.RunAsync(cancellationToken);
            var runHttp = _httpServer.RunAsync(cancellationToken);

            do
            {
                await Task.WhenAll(runAdapter, runHttp);
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

        private void OnHttpRequest(HttpListenerContext context)
        {
            var executors = _contextExecutors
                .OfType<WebHookContextExecutor>()
                .Where(e => e.ShouldExecute(context));
            RunExecutors(executors, context);
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
