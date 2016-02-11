using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace NuBot.Http
{
    internal sealed class HttpServer : IHttpServer
    {
        private readonly string _host;
        private readonly int _port;
        private readonly IList<Action<HttpListenerContext>> _callbacks;
        private readonly HttpListener _listener;

        public HttpServer(int port, string host = "localhost")
        {
            _host = host;
            _port = port;
            _callbacks = new List<Action<HttpListenerContext>>();
            _listener = new HttpListener();
        } 

        public void OnRequest(Action<HttpListenerContext> context)
        {
            _callbacks.Add(context);
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            _listener.Prefixes.Add($"http://{_host}:{_port}/nubot/");
            _listener.Start();

            _listener.BeginGetContext(GetContext, null);

            try
            {
                await Task.Delay(Timeout.Infinite, cancellationToken);
            }
            catch (TaskCanceledException)
            {
            }

            _listener.Stop();
        }

        private void GetContext(IAsyncResult ar)
        {
            HttpListenerContext ctx;

            try
            {
                ctx = _listener.EndGetContext(ar);
            }
            catch (HttpListenerException httpException)
            {
                // 995 is operation aborted
                if (httpException.ErrorCode == 995)
                {
                    return;
                }

                throw;
            }

            foreach (var callback in _callbacks)
            {
                callback(ctx);
            }

            ctx.Response.StatusCode = 200;
            ctx.Response.OutputStream.Close();

            _listener.BeginGetContext(GetContext, null);
        }
    }
}