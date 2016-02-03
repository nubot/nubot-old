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

            while (!cancellationToken.IsCancellationRequested)
            {
                var ctx = await _listener.GetContextAsync();

                foreach (var callback in _callbacks)
                {
                    callback(ctx);
                }

                ctx.Response.StatusCode = 200;
                ctx.Response.OutputStream.Close();
            }

            _listener.Stop();
        }
    }
}