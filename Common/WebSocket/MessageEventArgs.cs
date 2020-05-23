using System;
using System.Net.WebSockets;
using System.Text;

namespace Common
{
    public class MessageEventArgs : EventArgs
    {
        public MessageEventArgs(WebSocketMessageType webSocketMessageType, byte[] rawData)
        {
            RawData = rawData;
            MessageType = webSocketMessageType;
            IsBinary = MessageType == WebSocketMessageType.Binary;
            IsText = MessageType == WebSocketMessageType.Text;
            if (IsText)
            {
                Data = Encoding.UTF8.GetString(RawData);
            }
        }
        public string Data { get; private set; }

        public bool IsBinary { get; private set; }

        public bool IsText { get; private set; }
        public byte[] RawData { get; private set; }

        public WebSocketMessageType MessageType { get; private set; }
    }
}
