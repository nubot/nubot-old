using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using NuBot.Adapters;
using NuBot.Automation.Contexts;

namespace NuBot.Automation.WebHooks
{
    internal sealed class WebHookContext : Context, IWebHookContext
    {
        private readonly HttpListenerContext _httpContext;

        public WebHookContext(IAdapter adapter, IDictionary<string, string> parameters, HttpListenerContext httpContext)
            : base(adapter)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));
            _httpContext = httpContext;

            Parameters = new ReadOnlyDictionary<string, string>(
                parameters ?? new Dictionary<string, string>());
        }

        public IReadOnlyDictionary<string, string> Parameters { get; }

        public TModel GetContent<TModel>()
        {
            var serializer = new JavaScriptSerializer();

            using (var ms = new MemoryStream())
            {
                _httpContext.Request.InputStream.CopyTo(ms);

                var json = Encoding.UTF8.GetString(ms.ToArray());
                return serializer.Deserialize<TModel>(json);
            }
        }
    }
}