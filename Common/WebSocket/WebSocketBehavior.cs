using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;

namespace Common
{
    public class WebSocketBehavior : WebSocketSession
    {
        public WebSocketBehavior() : base(null, CancellationToken.None, 4096)
        {

        }
        public WebSocketState State => ReadyState;

        public HttpContext Context { get; internal set; }

        public string RemoteIpAddress => Context.Connection.RemoteIpAddress.ToString();

        public WebSocketSessionManager Sessions => ServiceHost?.Sessions;

        public WebSocketSessionGroupManager Groups => ServiceHost?.Groups;

        public WebSocketServiceHost ServiceHost { get; internal set; }

        protected override void OnClose(CloseEventArgs e)
        {
        }

        private readonly ConcurrentDictionary<string, bool> joinedGroups = new ConcurrentDictionary<string, bool>();

        public ICollection<string> JoinedGroups => joinedGroups.Keys;

        public void JoinToGroup(string groupName)
        {
            joinedGroups.TryAdd(groupName, true);
            if (Groups != null)
            {
                Groups.Add(groupName, this);
            }
        }

        public void QuitFromGroup(string groupName)
        {
            joinedGroups.TryRemove(groupName, out bool val);
            if (Groups != null)
            {
                Groups.Remove(groupName, Id);
            }
        }

        public void QuitFromGroups()
        {
            if (JoinedGroups.Any())
            {
                foreach (string g in JoinedGroups)
                {
                    QuitFromGroup(g);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            ServiceHost = null;
        }
    }
}
