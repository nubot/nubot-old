using NuBot.Adapters;
using NuBot.Automation.MessageHandlers;
using System;
using System.Collections.Generic;
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

        public RobotEngine(IAdapter adapter, Func<IRobot> robotFunc, Func<IEnumerable<RobotPart>> parts)
        {
            _adapter = adapter;
            _robotFunc = robotFunc;
            _parts = parts;
        }

        public IAdapter Adapter => _adapter;

        public void RegisterHandler<T>(IMessageHandler<T> handler, Action<IContext<T>> callback) where T : IMessage
        {
            MessageContextContainer<T>.Add(new MessageContext<T>(handler, callback));
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            _adapter.On<ChannelJoinMessage>(OnChannelJoinMessage);
            _adapter.On<TextMessage>(OnTextMessage);
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

        private void OnChannelJoinMessage(ChannelJoinMessage message)
        {
            var messageContexts = MessageContextContainer<ChannelJoinMessage>.GetAll();

            foreach (var context in messageContexts)
            {
                if (!context.MessageHandler.CanHandle(message))
                {
                    continue;
                }

                var parameters = context.MessageHandler.Handle(message);
                var ctx = new Context<ChannelJoinMessage>(_adapter, message, parameters);

                try
                {
                    context.Callback(ctx);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private void OnTextMessage(TextMessage message)
        {
            var messageContexts = MessageContextContainer<TextMessage>.GetAll();

            foreach(var context in messageContexts)
            {
                if (!context.MessageHandler.CanHandle(message))
                {
                    continue;
                }

                var parameters = context.MessageHandler.Handle(message);
                var ctx = new Context<TextMessage>(_adapter, message, parameters);

                try
                {
                    context.Callback(ctx);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
