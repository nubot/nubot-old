using System;
using System.Collections.Generic;
using System.Linq;
using NuBot.Automation.MessageHandlers;
using NuBot.Brains;

namespace NuBot.Automation
{
    internal sealed class Robot : IRobot
    {
        private readonly IRobotEngine _engine;

        public Robot(IRobotEngine engine)
        {
            _engine = engine;
        }

        public IBrain Brain
        {
            get { throw new NotImplementedException(); }
        }

        public void Hear(string pattern, Action<IContext> context)
        {
            _engine.RegisterHandler(new PatternMessageHandler(pattern), context);
        }

        public void Listen(string pattern, Action<IContext> context)
        {
            pattern = $"^\\s*[@]?{_engine.Adapter.UserName}[:,]?\\s*(?:{pattern})";
            _engine.RegisterHandler(new PatternMessageHandler(pattern), context);
        }

        public T Random<T>(IEnumerable<T> collection)
        {
            var arr = collection.ToArray();
            var rnd = new Random(DateTime.Now.Millisecond);
            var idx = rnd.Next(0, arr.Length);

            return arr[idx];
        }
    }
}
