using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Common
{
    public class WebSocketSessionManager : SessionManager<WebSocketBehavior>
    {
        public WebSocketSessionManager(WebSocketServiceHost serviceHost) : base()
        {
            ServiceHost = serviceHost;
        }

        public override WebSocketBehavior Add(WebSocketBehavior session)
        {
            session.ServiceHost = ServiceHost;
            return base.Add(session);
        }

        public override WebSocketBehavior Remove(string sessionId)
        {
            WebSocketBehavior session = base.Remove(sessionId);
            if (session != null)
            {
                session.ServiceHost = null;
            }

            return session;
        }

        public WebSocketServiceHost ServiceHost { get; private set; }
    }
    public class SessionManager<T> : IDisposable, IEnumerable<T>
        where T : WebSocketSession
    {
        private readonly Timer timer;
        private readonly ConcurrentDictionary<string, T> container = new ConcurrentDictionary<string, T>();
        public SessionManager()
        {

            timer = new Timer(60000)
            {
                Enabled = true
            };
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            List<WebSocketSession> deadSessions = new List<WebSocketSession>();
            foreach (T s in Sessions)
            {
                if (s.IsClosed)
                {
                    deadSessions.Add(s);
                }
            }

            if (deadSessions.Any())
            {
                foreach (WebSocketSession c in deadSessions)
                {
                    c.CheckClose();
                }
            }
        }

        public double KeepAliveInterval
        {
            get => timer.Interval;
            set => timer.Interval = value;
        }


        public int Count => Ids.Count;
        public virtual T Add(T session)
        {
            return container.AddOrUpdate(session.Id, session, (id, s) => s);
        }

        public virtual T Remove(string sessionId)
        {
            container.TryRemove(sessionId, out T session);
            return session;
        }

        public bool TryGetSession(string sessionId, out T session)
        {
            return container.TryGetValue(sessionId, out session);
        }

        public async Task SendToAsync(byte[] data, string sessionId)
        {
            if (TryGetSession(sessionId, out T session))
            {
                await session.SendAsync(data);
            }
        }

        public async Task SendToAsync(string data, string sessionId)
        {
            if (TryGetSession(sessionId, out T session))
            {
                await session.SendAsync(data);
            }
        }
        public void SendTo(byte[] data, string sessionId)
        {
            if (TryGetSession(sessionId, out T session))
            {
                session.Send(data);
            }
        }

        public void SendTo(string data, string sessionId)
        {
            if (TryGetSession(sessionId, out T session))
            {
                session.Send(data);
            }
        }
        public void SendTo(string data, string sessionId,int timeOut)
        {
            if (TryGetSession(sessionId, out T session))
            {
                session.Send(data, timeOut);
            }
        }

        public ICollection<string> Ids => container.Keys;

        public ICollection<T> Sessions => container.Values;

        public void Clear()
        {
            container.Clear();
        }

        private bool disposed = false;
        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            foreach (T c in container.Values)
            {
                c.Dispose();
            }

            container.Clear();
            disposed = true;
            GC.SuppressFinalize(this);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Sessions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
