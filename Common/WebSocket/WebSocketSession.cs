using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public abstract class WebSocketSession : IDisposable
    {
        protected bool disposed = false;
        protected CancellationToken CancelToken;

        public WebSocketSession(System.Net.WebSockets.WebSocket socket, CancellationToken cancelToken, int bufferSize = 1024)
        {
            Socket = socket;
            CancelToken = cancelToken;
            BufferSize = bufferSize;
            byte[] bytes = new byte[bufferSize];
            Buffer = new Memory<byte>(bytes);
            Id = Guid.NewGuid().ToString("N");
        }

        public System.Net.WebSockets.WebSocket Socket { get; internal set; }

        internal void SetCancelToken(CancellationToken cancellationToken)
        {
            CancelToken = cancellationToken;
        }

        public WebSocketState ReadyState => Socket.State;

        public int BufferSize { get; internal set; }

        public string Id { get; set; }

        internal Memory<byte> Buffer { get; private set; }

        /// <summary>  
        /// 响应处理  
        /// </summary>  
        /// <returns></returns>  
        internal async Task EchoLoop(CancellationToken cancelToken)
        {
            //receive message
            while (Socket.State == WebSocketState.Open)
            {
                ValueWebSocketReceiveResult receiveResult = default(ValueWebSocketReceiveResult);
                MemoryStream bufferStream = new MemoryStream();
                try
                {
                    do
                    {
                        if (cancelToken.IsCancellationRequested)
                        {
                            break;
                        }

                        if (disposed)
                        {
                            break;
                        }

                        if (Socket.State != WebSocketState.Open)
                        {
                            break;
                        }

                        receiveResult = await Socket.ReceiveAsync(Buffer, cancelToken);
                        await bufferStream.WriteAsync(Buffer.Slice(0, receiveResult.Count), cancelToken);
                    }
                    while (!receiveResult.EndOfMessage);
                    if (bufferStream != null && bufferStream.Length > 0)
                    {
                        bufferStream.Position = 0;
                        byte[] buff = bufferStream.ToArray();
                        bufferStream.Dispose();
                        bufferStream = null;
                        OnMessage(new MessageEventArgs(receiveResult.MessageType, buff));
                        await OnMessageAsync(new MessageEventArgs(receiveResult.MessageType, buff));
                    }
                }
                catch (OperationCanceledException oEx)
                {
                    Common.LogRecord.Debug("WebSocketSession", "EchoLoop:" + oEx.Message);
                }
                catch (Exception ex)
                {
                    OnError(new ErrorEventArgs(ex, $"{Id} receive message error."));
                }
                finally
                {
                    if (bufferStream != null)
                    {
                        bufferStream.Dispose();
                    }
                }
            }

            CheckClose();
        }

        public bool IsClosed => Socket.State == WebSocketState.Aborted || Socket.State == WebSocketState.Closed ||
                  Socket.State == WebSocketState.CloseReceived || Socket.State == WebSocketState.CloseSent;
        internal void CheckClose()
        {
            if (Socket == null)
            {
                return;
            }

            if (Socket.State == WebSocketState.Closed ||
                    Socket.State == WebSocketState.CloseReceived || Socket.State == WebSocketState.CloseSent)
            {
                NativeOnClose(new CloseEventArgs(Socket.CloseStatus.Value, Socket.CloseStatusDescription));
            }
            else if (Socket.State == WebSocketState.Aborted)
            {
                NativeOnClose(new CloseEventArgs(WebSocketCloseStatus.EndpointUnavailable, $"Session {Id} has aborted."));
            }
        }
        public virtual void OnOpen()
        {
        }
        internal void NativeOnClose(CloseEventArgs e)
        {
            WebSocketBehavior webSocketBehavior = this as WebSocketBehavior;
            if (webSocketBehavior != null)
            {
                if (webSocketBehavior.Sessions != null)
                {
                    webSocketBehavior.Sessions.Remove(Id); //server
                }

                webSocketBehavior.QuitFromGroups();
            }

            OnClose(e);
            //server
            if (webSocketBehavior != null)
            {
                Dispose();
            }

        }

        protected virtual void OnClose(CloseEventArgs e)
        {
        }
        protected virtual void OnError(ErrorEventArgs e)
        {

        }
        protected virtual void OnMessage(MessageEventArgs e)
        {

        }

        protected virtual async Task OnMessageAsync(MessageEventArgs e)
        {
            await Task.CompletedTask;
        }

        public async Task SendAsync(ArraySegment<byte> segment)
        {
            await Socket.SendAsync(segment, WebSocketMessageType.Binary, true, CancelToken);
        }

        public async Task SendAsync(Memory<byte> segment)
        {
            await Socket.SendAsync(segment, WebSocketMessageType.Binary, true, CancelToken);
        }
        public async Task SendAsync(byte[] buffer)
        {
            Memory<byte> segment = new Memory<byte>(buffer);
            await SendAsync(segment);
        }
        public async Task SendAsync(string data)
        {
            ArraySegment<byte> segment = new ArraySegment<byte>(Encoding.UTF8.GetBytes(data));
            await Socket.SendAsync(segment, WebSocketMessageType.Text, true, CancelToken);
        }

        public async Task SendAsync(string data, int timeOut)
        {
            CancellationTokenSource source = new CancellationTokenSource(timeOut);

            ArraySegment<byte> segment = new ArraySegment<byte>(Encoding.UTF8.GetBytes(data));
            await Socket.SendAsync(segment, WebSocketMessageType.Text, true, source.Token);
        }

        public async Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
        {
            if (IsClosed)
            {
                return;
            }

            await Socket.CloseAsync(closeStatus, statusDescription, CancelToken);

            NativeOnClose(new CloseEventArgs(WebSocketCloseStatus.Empty, ""));
        }

        public async Task CloseAsync(WebSocketCloseStatus closeStatus = WebSocketCloseStatus.Empty, string statusDescription = "")
        {
            await CloseAsync(closeStatus, statusDescription, CancellationToken.None);
        }
        public bool Close(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
        {
            try
            {
                CloseAsync(closeStatus, statusDescription, CancelToken).Wait();
                return true;
            }
            catch (Exception ex)
            {
                Common.LogRecord.Debug("WebSocketSession", "Close:" + ex.Message);
            }
            return false;
        }

        public bool Close(WebSocketCloseStatus closeStatus = WebSocketCloseStatus.Empty, string statusDescription = "")
        {
            return Close(closeStatus, statusDescription, CancellationToken.None);
        }

        public bool Send(byte[] buffer)
        {
            return SendAsync(buffer).IsCompleted;
        }
        public bool Send(string data)
        {
            return SendAsync(data).IsCompleted;
        }

        public bool Send(string data, int timeOut = 1000 * 3)
        {
            return SendAsync(data, timeOut).IsCompleted;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            Task t = Socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close", CancelToken);
            if (!t.IsCompleted)
            {
                t.Wait(1000);
            }

            Socket.Dispose();
            Socket = null;
            if (disposing)
            {
                Buffer = null;
            }
            disposed = true;
        }

        ~WebSocketSession()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
