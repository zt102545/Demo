using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Common
{
    public class WebSocketServiceHost : IDisposable
    {
        public WebSocketSessionManager Sessions { get; private set; }

        public WebSocketSessionGroupManager Groups { get; private set; }
        protected bool disposed;

        public WebSocketServiceHost(string urlSegment, int bufferSize = 4096)
        {
            BufferSize = bufferSize;
            Path = urlSegment;
            Sessions = new WebSocketSessionManager(this);
            Groups = new WebSocketSessionGroupManager(this);
        }

        public int BufferSize { get; private set; }

        public string Path { get; private set; }
        //创建链接  
        internal async Task Accept<T>(HttpContext context, Func<Task> n)
            where T : WebSocketBehavior, new()
        {
            try
            {
                //是websocket请求才进来
                if (context.WebSockets.IsWebSocketRequest)
                {
                    //执行接收  
                    System.Net.WebSockets.WebSocket socket = await context.WebSockets.AcceptWebSocketAsync();
                    T session = new T
                    {
                        Socket = socket
                    };
                    session.SetCancelToken(context.RequestAborted);
                    session.BufferSize = BufferSize;
                    session.Context = context;
                    Sessions.Add(session);
                    session.OnOpen();
                    //执行监听  
                    await session.EchoLoop(context.RequestAborted);
                }
            }
            catch (Exception ex)
            {
                Common.LogRecord.Debug("Accept", ex.Message);
                //throw ex;
            }
        }

        internal virtual async Task Accept(HttpContext context, Func<Task> n)
        {
            await Accept<WebSocketBehavior>(context, n);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            Sessions.Dispose();
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~WebSocketServiceHost()
        {
            Dispose(false);
        }
    }

    public class WebSocketServiceHost<T> : WebSocketServiceHost
        where T : WebSocketBehavior, new()
    {
        public WebSocketServiceHost(string urlSegment, int bufferSize = 4096) : base(urlSegment, bufferSize)
        {
        }
        internal override async Task Accept(HttpContext context, Func<Task> n)
        {
            await base.Accept<T>(context, n);
        }
    }
}
