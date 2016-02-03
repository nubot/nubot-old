using System;
using System.Collections.Generic;
using System.Linq;
using NuBot.Automation.Contexts;
using NuBot.Automation.Filtering;
using NuBot.Automation.Messages;
using NuBot.Automation.WebHooks;
using NuBot.Brains;

namespace NuBot.Automation
{
    internal sealed class Robot : IRobot
    {
        private readonly IRobotEngine _engine;

        public Robot(IRobotEngine engine)
        {
            if (engine == null) throw new ArgumentNullException(nameof(engine));

            _engine = engine;
        }

        public IBrain Brain
        {
            get { throw new NotImplementedException(); }
        }

        public void Hear(string pattern, Action<ISourcedContext<ITextMessage>> context)
        {
            _engine.RegisterExecutor(
                new SourcedContextExecutor<ITextMessage>(
                    context,
                    new TextMessageRegexFilter(pattern)));
        }

        public void Listen(string pattern, Action<ISourcedContext<ITextMessage>> context)
        {
            pattern = $"^\\s*[@]?{_engine.Adapter.UserName}[:,]?\\s*(?:{pattern})";
            Hear(pattern, context);
        }

        public void OnChannelJoin(Action<ISourcedContext<IChannelJoinMessage>> context)
        {
            _engine.RegisterExecutor(
                new SourcedContextExecutor<IChannelJoinMessage>(
                    context,
                    new MatchAllFilter<IChannelJoinMessage>()));
        }

        public void OnChannelLeave(Action<ISourcedContext<IChannelLeaveMessage>> context)
        {
            _engine.RegisterExecutor(
                new SourcedContextExecutor<IChannelLeaveMessage>(
                    context,
                    new MatchAllFilter<IChannelLeaveMessage>()));
        }

        public T Random<T>(IEnumerable<T> collection)
        {
            var arr = collection.ToArray();
            var rnd = new Random(DateTime.Now.Millisecond);
            var idx = rnd.Next(0, arr.Length);

            return arr[idx];
        }

        public void WebHook(string method, string pattern, Action<IWebHookContext> context)
        {
            _engine.RegisterExecutor(
                new WebHookContextExecutor(
                    method,
                    pattern,
                    context));
        }
    }
}
