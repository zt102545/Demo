using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public class WebSocketServer
    {
        private IWebHost webHost;
        public Dictionary<string, WebSocketServiceHost> ServiceHosts { get; private set; }
        public WebSocketServer(string url, IConfiguration configuration = null)
        {
            Url = url;
            ServiceHosts = new Dictionary<string, WebSocketServiceHost>();
            Configuration = configuration;
        }

        public string Url { get; private set; }



        protected virtual void Configure(IApplicationBuilder app)
        {
            foreach (WebSocketServiceHost c in ServiceHosts.Values)
            {
                app.Map(c.Path, ap => { app.UseWebSockets(); ap.Use(c.Accept); });
            }
        }
        protected virtual void ConfigureServices(IServiceCollection services)
        {
        }

        public IConfiguration Configuration { get; }

        public WebSocketServiceHost AddWebSocketService<T>(string path, int bufferSize = 4096)
            where T : WebSocketBehavior, new()
        {
            WebSocketServiceHost<T> host = new WebSocketServiceHost<T>(path, bufferSize);
            ServiceHosts.Add(path, host);
            return host;
        }


        public void Start()
        {
            if (webHost == null)
            {
                IWebHostBuilder webHostBuilder = WebHost.CreateDefaultBuilder().UseUrls(Url)
                .Configure(app => Configure(app)).ConfigureServices(services => ConfigureServices(services));
                if (ServiceHosts.Any())
                {
                    webHost = webHostBuilder.Build();
                }
            }
            webHost.Run();
        }

        public void Stop()
        {
            if (webHost != null)
            {
                webHost.StopAsync().Wait();
            }
        }

        public bool CheckItHasBeenRun()
        {
            if (webHost == null)
            {
                return false;
            }

            return true;
        }
    }
}
