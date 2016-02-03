using System;
using System.Net;
using System.Text.RegularExpressions;

namespace NuBot.Automation.WebHooks
{
    public class WebHookContextExecutor : IContextExecutor
    {
        private readonly string _method;
        private readonly string _pattern;
        private readonly Action<IWebHookContext> _contextCallback;

        public WebHookContextExecutor(string method, string pattern, Action<IWebHookContext> contextCallback)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (pattern == null) throw new ArgumentNullException(nameof(pattern));
            if (contextCallback == null) throw new ArgumentNullException(nameof(contextCallback));

            _method = method;
            _pattern = pattern;
            _contextCallback = contextCallback;
        }

        public bool ShouldExecute(HttpListenerContext context)
        {
            if (context.Request.HttpMethod != _method)
            {
                return false;
            }

            var regex = new Regex($"^{_pattern}$", RegexOptions.IgnoreCase);
            var url = context.Request.Url.AbsolutePath;

            if (url.StartsWith("/nubot"))
            {
                url = url.Substring("/nubot".Length);
            }

            return regex.IsMatch(url);
        }

        public void Execute(IExecutionRequest request)
        {
            var context = new WebHookContext(
                request.Adapter,
                null,
                request.GetDataSource<HttpListenerContext>());

            _contextCallback(context);
        }
    }
}