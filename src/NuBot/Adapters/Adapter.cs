using System;
using System.Threading;
using System.Threading.Tasks;

namespace NuBot.Adapters
{
    public abstract class Adapter : IAdapter
    {
        public abstract string UserName { get; }

        public abstract Task SetupAsync();

        public abstract Task RunAsync(CancellationToken cancellationToken);

        public void On<T>(Action<T> callback)
        {
            CallbackContainer<T>.Add(callback);
        }

        protected void Emit<T>(T message)
        {
            foreach(var callback in CallbackContainer<T>.GetAll())
            {
                callback(message);
            }
        }
    }  
}
