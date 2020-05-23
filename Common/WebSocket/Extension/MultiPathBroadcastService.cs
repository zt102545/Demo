using System;
using System.Collections.Generic;

namespace Common
{
    /// <summary>
    /// 控制台ws服务端调用，自动创建IWebHost，支持多个端口和路径
    /// </summary>
    public class MultiPathBroadcastService : IDisposable
    {
        private readonly string listenUrl;

        public CommandWebSocketServer Server { get; }

        public WebSocketServiceHost CurrentServiceHost(string servicePath)
        {
            if (Server != null &&
                Server.ServiceHosts != null &&
                Server.ServiceHosts.ContainsKey(servicePath))
            {
                return Server.ServiceHosts[servicePath];
            }
            return null;
        }

        public MultiPathBroadcastService(string serverListenUrl)
        {
            listenUrl = serverListenUrl;
            Server = new CommandWebSocketServer(serverListenUrl);
        }
        public void AddWebSocketService<T>(string servicePath) where T : WebSocketBehavior, new()
        {
            Server.AddWebSocketService<T>(servicePath);
        }

        public void Start()
        {
            Server.Start();
            if (Server.ServiceHosts != null && Server.ServiceHosts.Count > 0)
            {
                foreach (KeyValuePair<string, WebSocketServiceHost> serviceHost in Server.ServiceHosts)
                {
                    LogRecord.Info("MultiPathBroadcastService", string.Format("Service Start. Listen:{0}", listenUrl + serviceHost.Key));
                }
            }
        }

        public bool CheckItHasBeenRun()
        {
            return Server.CheckItHasBeenRun();
        }

        public void Stop()
        {
            Server.Stop();
        }

        public void Dispose()
        {
            Stop();
        }
    }
}