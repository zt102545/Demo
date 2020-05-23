using System;
using System.Net.WebSockets;

namespace Common
{
    public class CloseEventArgs : EventArgs
    {
        public CloseEventArgs(WebSocketCloseStatus webSocketCloseStatus, string reason)
        {
            Code = webSocketCloseStatus;
            Reason = reason;
        }
        public WebSocketCloseStatus Code { get; private set; }
        public string Reason { get; private set; }
    }
}
