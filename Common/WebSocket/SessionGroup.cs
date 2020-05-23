using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public class SessionGroup<T> : IEnumerable<T>, ICollection<T>
        where T : WebSocketBehavior
    {

        public SessionGroup(string name)
        {
            Name = name;
        }
        public string Name { get; private set; }

        public int Count => sessions.Count;

        public bool IsReadOnly => false;

        public ConcurrentDictionary<string, T> sessions = new ConcurrentDictionary<string, T>();

        public bool Contains(string sessionId)
        {
            return sessions.ContainsKey(sessionId);
        }

        public bool Remove(string sessionId)
        {

            sessions.TryRemove(sessionId, out T session);
            return true;

        }

        public void Add(T session)
        {
            sessions[session.Id] = session;
        }

        public void Clear()
        {
            List<T> list = sessions.Values.ToList();
            if (list.Any())
            {
                foreach (T s in list)
                {
                    try
                    {
                        s.QuitFromGroup(Name);
                    }
                    catch (Exception ex)
                    {
                        Common.LogRecord.Debug("SessionGroup", ex.Message);
                    }
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return sessions.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return sessions.Values.GetEnumerator();
        }

        public bool Contains(T item)
        {
            return sessions.Values.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            sessions.Values.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return Remove(item.Id);
        }

        public T this[string sessionId] => sessions[sessionId];
    }
}
