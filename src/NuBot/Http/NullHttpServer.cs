using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace NuBot.Http
{
    public class NullHttpServer : IHttpServer
    {
        public void OnRequest(Action<HttpListenerContext> context)
        {
        }

        public Task RunAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}