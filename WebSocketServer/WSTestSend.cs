using Common;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketServer
{
    public class WSTestSend
    {
        public static MultiPathBroadcastService broadcastService;
        public const string testPath = "/test";
        private static ParallelOptions po;


        public static void Start()
        { 
            Thread t = new Thread(init);
            t.Start();
            new HostBuilder().Build().Run();
        }

        public static void init()
        {
            po = new ParallelOptions
            {
                MaxDegreeOfParallelism = 10
            };
            //浏览器控制台，监听地址:var webSocket = new WebSocket("ws://127.0.0.1:29000/test?usertoken=xxxx")
            string serverListenUrl = "http://*:29002";
            broadcastService = new MultiPathBroadcastService(serverListenUrl);
            broadcastService.AddWebSocketService<WSTestWSBehavior>(testPath);
            broadcastService.Start(); //服务启动 
        }

        public static void Send(string dataStr)
        {
            if (broadcastService != null)
            {
                List<string> errorSessionIds = new List<string>();
                string sessionKey = "testkey";
                if (WSTestWSBehavior.GroupSessions.TryGetValue(sessionKey, out ConcurrentDictionary<string, WSTestWSBehavior> dic))
                {
                    Parallel.ForEach(dic.Keys, po, key =>
                    {
                        try
                        {
                            broadcastService.CurrentServiceHost(testPath).Sessions.SendTo(dataStr, key);
                        }
                        catch (Exception)
                        {
                            errorSessionIds.Add(key);
                        }
                    });
                }
                if (errorSessionIds != null && errorSessionIds.Count > 0)
                {
                    foreach (string errorSessionId in errorSessionIds)
                    {
                        WebSocketBehavior webSocketBehavior = broadcastService.CurrentServiceHost(testPath).Sessions.Remove(errorSessionId);
                        if (webSocketBehavior != null)
                        {
                            webSocketBehavior.QuitFromGroups();
                            webSocketBehavior.Dispose();
                        }
                    }
                }
            }

        }
    }
}
