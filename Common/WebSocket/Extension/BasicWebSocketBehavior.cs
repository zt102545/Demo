using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;

namespace Common
{
    public class BasicWebSocketBehavior : WebSocketBehavior
    {
        private readonly object extendDataSyncRoot = new object();
        public BasicWebSocketBehavior()
        {
            ExtendData = new ConcurrentDictionary<string, object>();
        }

        private IPAddress endPoint;

        public override void OnOpen()
        {
            base.OnOpen();
            endPoint = Context.Connection.RemoteIpAddress;
            LogRecord.Info("BasicWebSocketBehavior", string.Format("Socket Client Connect:{0}", endPoint));
        }

        protected override void OnClose(CloseEventArgs e)
        {
            LogRecord.Info("BasicWebSocketBehavior", string.Format("Socket Client Closed:{0}", endPoint));
            base.OnClose(e);
        }

        protected override void OnError(ErrorEventArgs e)
        {
            base.OnError(e);
            LogRecord.Info("BasicWebSocketBehavior", string.Format("Socket Client Error:{0}.\r\nException:{1}\r\nMessage:", endPoint, e.Exception, e.Message));
        }

        public IDictionary<string, object> ExtendData
        {
            get;
            private
            set;
        }

        public T GetExtendDataItem<T>(string key, Func<T> newItemFun = null)
        {
            object item = null;
            if (!ExtendData.ContainsKey(key))
            {
                if (newItemFun != null)
                {
                    lock (extendDataSyncRoot)
                    {
                        if (!ExtendData.ContainsKey(key))
                        {
                            item = newItemFun();
                            ExtendData.Add(key, item);
                        }
                        else
                        {
                            item = ExtendData[key];
                        }
                    }
                }
                else
                {
                    return default(T);
                }
            }
            else
            {
                item = ExtendData[key];
            }
            return (T)item;
        }
    }
}
