using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.WebSocket.Extension
{
    /// <summary>
    /// web站点ws服务端调用，Startup的Configure方法使用IApplicationBuilder的AddWebSocketMap
    /// </summary>
    public static class WebAPIWebSocketService
    {
        public static Dictionary<string, WebSocketServiceHost> ServiceHosts { get; private set; }

        static WebAPIWebSocketService()
        {
            if (ServiceHosts == null)
            {
                ServiceHosts = new Dictionary<string, WebSocketServiceHost>();
            }
        }

        /// <summary>
        /// 添加ws路径组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        public static WebSocketServiceHost AddWebSocketService<T>(string path, int bufferSize = 4096) where T : WebSocketBehavior, new()
        {
            WebSocketServiceHost<T> host = new WebSocketServiceHost<T>(path, bufferSize);
            ServiceHosts.Add(path, host);
            return host;
        }

        /// <summary>
        /// 扩展方法，路由绑定处理 
        /// </summary>
        /// <param name="app"></param>
        public static void AddWebSocketMap(this IApplicationBuilder app)
        {
            foreach (WebSocketServiceHost c in ServiceHosts.Values)
            {
                app.Map(c.Path, ap => { app.UseWebSockets(); ap.Use(c.Accept); });
            }
        }

        /// <summary>
        /// 获取该路径的WS
        /// </summary>
        /// <param name="servicePath"></param>
        /// <returns></returns>
        public static WebSocketServiceHost CurrentServiceHost(string servicePath)
        {
            if (ServiceHosts != null && ServiceHosts.ContainsKey(servicePath))
            {
                return ServiceHosts[servicePath];
            }
            return null;
        }
    }
}
