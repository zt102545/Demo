using Microsoft.Extensions.Configuration;
using System;

namespace Common
{
    /// <summary>
    /// 控制台ws服务端调用，自动创建IWebHost
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CommandBroadcastService<T> : IDisposable where T : WebSocketBehavior, new()
    {
        private readonly CommandWebSocketServer server;
        private readonly WebSocketServiceHost serviceHost;
        private readonly string listenUrl;

        public CommandWebSocketServer Server => server;

        public WebSocketServiceHost DefaultServiceHost => serviceHost;
        public CommandBroadcastService(string serverListenUrl, string servicePath)
        {
            listenUrl = serverListenUrl;
            server = new CommandWebSocketServer(serverListenUrl);
            serviceHost = server.AddWebSocketService<T>(servicePath);
        }

        public CommandBroadcastService(BroadcastConfig config) : this(config.Server.ListenUrl, config.Server.ServicePath)
        {
        }

        public void Start()
        {
            server.Start();
            Common.LogRecord.Info("CommandBroadcastService", string.Format("Service Start. Listen:{0}", listenUrl));
        }

        public void Stop()
        {
            server.Stop();
        }

        public void Dispose()
        {
            Stop();
        }
    }

    public class BroadcastConfig
    {
        public ServerConfig Server { get; private set; }

        public static BroadcastConfig Parse(IConfiguration config)
        {
            BroadcastConfig broadConfig = new BroadcastConfig();
            IConfigurationSection serverSection = config.GetSection("server");
            broadConfig.Server = ServerConfig.Parse(serverSection);
            return broadConfig;
        }
    }

    public class ServerConfig
    {
        public string ListenUrl { get; set; }

        public string ServicePath { get; set; }

        public static ServerConfig Parse(IConfiguration config)
        {
            IConfigurationSection listenConfig = config.GetSection("listen");
            IConfigurationSection servicePathConfig = config.GetSection("servicePath");
            return new ServerConfig { ListenUrl = listenConfig.Value, ServicePath = servicePathConfig.Value };
        }
    }
}