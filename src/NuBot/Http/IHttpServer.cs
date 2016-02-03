using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace NuBot.Http
{
    public interface IHttpServer
    {
        void OnRequest(Action<HttpListenerContext> context);

        Task RunAsync(CancellationToken cancellationToken);
    }
}