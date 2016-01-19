using NuBot.Adapters;
using NuBot.Automation;
using NuBot.Brains;
using NuBot.Factory.DI;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NuBot.Factory
{
    public class RobotFactory
    {
        private IAdapter _adapter;
        private IBrain _brain;
        private List<Type> _parts;

        public RobotFactory()
        {
            _parts = new List<Type>();
        }

        public RobotFactory AddPart<T>() where T : RobotPart
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

        public Task RunAsync(CancellationToken cancellationToken)
        {
            var container = TinyIoCContainer.Current;
            container.Register(_adapter);
            container.Register(_brain ?? new InMemoryBrain());
            container.RegisterMultiple<RobotPart>(_parts);
            container.Register<IRobotEngine, RobotEngine>();
            container.Register<IRobot, Robot>();

            return container.Resolve<IRobotEngine>().RunAsync(cancellationToken);
        }
    }
}
