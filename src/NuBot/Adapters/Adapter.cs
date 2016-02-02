using System;
using System.Threading;
using System.Threading.Tasks;
using NuBot.Automation.Messages;

namespace NuBot.Adapters
{
    public abstract class Adapter : IAdapter
    {
        public abstract string UserName { get; }

        public abstract Task SetupAsync();

        public abstract Task RunAsync(CancellationToken cancellationToken);

        public abstract Task SendAsync(string channel, string message);

        public void On<T>(Action<T> callback) where T : IMessage
        {
            CallbackContainer<T>.Add(callback);
        }

        protected void Emit<T>(T message) where T : IMessage
        {
            foreach(var callback in CallbackContainer<T>.GetAll())
            {
                callback(message);
            }
        }
    }  
}
