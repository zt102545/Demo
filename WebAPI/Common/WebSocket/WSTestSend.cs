using Common;
using Common.WebSocket.Extension;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebAPI.Common.WebSocket
{
    public class WSTestSend
    {
        /// <summary>
        /// 路径：ws://localhost:{port}/test
        /// </summary>
        public static string testPath = ConfigurationManager.Appsettings.GetSection("WebSocketHosts:Server:TestUrl").Value;
        public static string sessionKey = "";

        /// <summary>
        /// 服务端发送消息
        /// </summary>
        /// <param name="dataStr"></param>
        public static void Send(string dataStr)
        {
            var po = new ParallelOptions
            {
                MaxDegreeOfParallelism = 10
            };
            if (WebAPIWebSocketService.ServiceHosts.Count > 0)
            {
                List<string> errorSessionIds = new List<string>();
                
                if (WSTestWSBehavior.GroupSessions.TryGetValue(sessionKey, out ConcurrentDictionary<string, WSTestWSBehavior> dic))
                {
                    Parallel.ForEach(dic.Keys, po, key =>
                    {
                        try
                        {
                            WebAPIWebSocketService.CurrentServiceHost(testPath).Sessions.SendTo(dataStr, key);
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
                        WebSocketBehavior webSocketBehavior = WebAPIWebSocketService.CurrentServiceHost(testPath).Sessions.Remove(errorSessionId);
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
