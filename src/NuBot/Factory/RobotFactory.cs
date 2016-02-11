using NuBot.Adapters;
using NuBot.Automation;
using NuBot.Brains;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NuBot.Diagnostics;
using NuBot.Factory.Registrations;
using NuBot.Http;

namespace NuBot.Factory
{
    public sealed class RobotFactory
    {
        private IAdapter _adapter;
        private IBrain _brain;
        private IHttpServer _httpServer;
        private INuBotLog _log;
        private readonly IContainer _container;
        private readonly List<Type> _parts;

        public RobotFactory()
            : this(new DefaultContainer())
        {
        }

        public RobotFactory(IContainer container)
        {
            _httpServer = new NullHttpServer();
            _parts = new List<Type>();
            _container = container;
        }

        public RobotFactory AddPart<T>()
            where T : RobotPart
        {
            _parts.Add(typeof(T));
            return this;
        }

        public RobotFactory UseAdapter(IAdapter adapter)
        {
            _adapter = adapter;
            return this;
        }

        public RobotFactory UseBrain(IBrain brain)
        {
            _brain = brain;
            return this;
        }

        public RobotFactory UseHttpServer(int port, string host = "localhost")
        {
            _httpServer = new HttpServer(port, host);
            return this;
        }

        public RobotFactory UseLog(INuBotLog log)
        {
            _log = log;
            return this;
        }

        public Task RunAsync(CancellationToken cancellationToken)
        {
            // Register robot parts.
            _container
                .RegisterInstance(_log ?? new ConsoleLog())
                .RegisterInstance(_adapter)
                .RegisterInstance(_brain ?? new InMemoryBrain())
                .RegisterMultiple<RobotPart>(_parts)
                .RegisterType<IRobotEngine, RobotEngine>(Lifetime.Singleton)
                .RegisterType<IRobot, Robot>(Lifetime.Singleton)
                .RegisterInstance(_httpServer);

            // Resolve and run the engine.
            return _container
                .Resolve<IRobotEngine>()
                .RunAsync(cancellationToken);
        }
    }
}
