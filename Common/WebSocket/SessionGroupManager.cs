using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public class SessionGroupManager<T> : IEnumerable<SessionGroup<T>>
        where T : WebSocketBehavior
    {
        private readonly ConcurrentDictionary<string, SessionGroup<T>> groups = new ConcurrentDictionary<string, SessionGroup<T>>();

        public SessionGroupManager(WebSocketServiceHost serviceHost)
        {
            ServiceHost = serviceHost;
        }
        public int Count => groups.Count;

        /*
        public bool IsReadOnly => false;

        public void Add(SessionGroup<T> item)
        {
            var group = this.groups.GetOrAdd(item.Name, item);
            if(group != item)
            {
                foreach(var i in item)
                {
                    group.Add(i);
                }
            }
        }*/

        public void Add(string groupName, T session)
        {
            SessionGroup<T> group = groups.GetOrAdd(groupName, (n) => new SessionGroup<T>(groupName));
            group.Add(session);
        }

        public void Clear()
        {
            List<SessionGroup<T>> temp = groups.Values.ToList();
            if (temp.Any())
            {
                foreach (SessionGroup<T> g in temp)
                {
                    g.Clear();
                }
            }
        }

        /*
        public bool Contains(SessionGroup<T> item)
        {
            return this.groups.ContainsKey(item.Name);
        }
        */
        public bool Contains(string groupName, string sessionId)
        {
            if (groups.TryGetValue(groupName, out SessionGroup<T> group))
            {
                return group.Contains(sessionId);
            }

            return false;
        }

        public bool Contains(string groupName)
        {
            return groups.ContainsKey(groupName);
        }

        /*
        public void CopyTo(SessionGroup<T>[] array, int arrayIndex)
        {
            this.groups.Values.CopyTo(array, arrayIndex);
        }
        */
        public IEnumerator<SessionGroup<T>> GetEnumerator()
        {
            return groups.Values.GetEnumerator();
        }

        public bool Remove(string groupName)
        {
            if (groups.TryRemove(groupName, out SessionGroup<T> group))
            {
                group.Clear();
            }
            return true;
        }

        public bool Remove(string groupName, string sessionId)
        {
            if (groups.TryGetValue(groupName, out SessionGroup<T> group))
            {
                return group.Remove(sessionId);
            }

            return true;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return groups.Values.GetEnumerator();
        }

        /*
        public bool Remove(SessionGroup<T> item)
        {
            return this.Remove(item.Name);
        }*/

        public WebSocketServiceHost ServiceHost { get; private set; }

        public SessionGroup<T> this[string groupName] => groups[groupName];
    }

    public class WebSocketSessionGroupManager : SessionGroupManager<WebSocketBehavior>
    {
        public WebSocketSessionGroupManager(WebSocketServiceHost serviceHost) : base(serviceHost)
        {
        }
    }
}
