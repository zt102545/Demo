using System;
using System.Net;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public class WebSocketClient : WebSocketSession, IDisposable
    {
        public event Action<object, EventArgs> ConnectionOpened;

        public event Action<object, CloseEventArgs> ConnectionClosed;

        public event Action<object, ErrorEventArgs> Error;

        public event Action<object, MessageEventArgs> MessageReceived;

        public WebSocketClient(string url, int bufferSize = 4096) : this(new Uri(url), bufferSize)
        {
        }
        public WebSocketClient(Uri uri, int bufferSize = 4096) : base(new ClientWebSocket(), CancellationToken.None, bufferSize)
        {
            Url = uri;
        }

        public new ClientWebSocket Socket => base.Socket as ClientWebSocket;

        public Uri Url { get; private set; }
        public async Task ConnectAsync()
        {
            if (Socket.State == WebSocketState.Connecting || Socket.State == WebSocketState.Open)
            {
                return;
            }

            if (IsClosed)
            {
                OnClose(new CloseEventArgs(WebSocketCloseStatus.Empty, ""));
                Socket.Dispose();
                base.Socket = new ClientWebSocket();
            }

            Common.LogRecord.Info("WebSocketClient", $"正在连接{Url}....");
            
            await Socket.ConnectAsync(Url, CancelToken);
            OnOpen();

            Common.LogRecord.Info("WebSocketClient", $"已连接{Url}.");

            await EchoLoop(CancelToken);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool Connect()
        {
            try
            {
                ConnectAsync().Wait();
                return true;
            }
            catch (Exception ex)
            {
                Common.LogRecord.Debug("WebSocketClient", Newtonsoft.Json.JsonConvert.SerializeObject(ex));
            }
            return false;
        }

        public sealed override void OnOpen()
        {
            ConnectionOpened?.Invoke(this, EventArgs.Empty);
        }

        protected sealed override void OnClose(CloseEventArgs e)
        {
            ConnectionClosed?.Invoke(this, e);
        }

        protected sealed override void OnError(ErrorEventArgs e)
        {
            Error?.Invoke(this, e);
        }

        protected sealed override void OnMessage(MessageEventArgs e)
        {
            MessageReceived?.Invoke(this, e);
        }

        public void SetCredentials(string userName, string password)
        {
            Socket.Options.Credentials = new NetworkCredential(userName, password);
        }
    }
}
