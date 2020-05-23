using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Common
{
    /// <summary>
    /// ws客户端
    /// </summary>
    public abstract class StompWebSocketClientBase : IDisposable
    {

        protected WebSocketClient webSocket;
        //private System.Timers.Timer timer;

        public StompWebSocketClientBase(string name, string address, bool autoPing = false)
        {
            Name = name;
            Address = address;
            AutoPing = autoPing;
        }

        public string Address { get; private set; }

        public bool AutoPing { get; private set; }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Ping();
        }

        protected virtual void WebSocketOnMessage(object sender, MessageEventArgs e)
        {
            try
            {
                OnReceiveMessage(webSocket, e);
            }
            catch (Exception ex)
            {
                LogRecord.Error("StompWebSocketMaketClientBase", $"{Name} receive message error {ex}.");
            }
        }

        protected virtual void WebSocketOnOpen(object sender, EventArgs e)
        {
            while (!StompConnect())
            {
                //AlarmManager.AlarmText(AlarmLevel.Info, AlarmCategory.MarketPush, $"Websocket连接握手失败，5秒后重试....");
                Thread.Sleep(5000);
            }

            while (!SubscribeSymbols())
            {
                Thread.Sleep(10000);
            }
            EnablePing();
        }

        protected virtual void WebSocketOnClosed(object sender, CloseEventArgs e)
        {
            LogRecord.Error("StompWebSocketMaketClientBase", $"{Name} was closed.");
        }

        protected virtual void WebSocketOnError(object sender, ErrorEventArgs e)
        {
            LogRecord.Error("StompWebSocketMaketClientBase", $"{Name} has error:{e.Exception}.");
        }

        public string Name { get; private set; }

        //private int pingInterval = 30000;
        //public int PingInterval
        //{
        //    get
        //    {
        //        return this.pingInterval;
        //    }
        //    set
        //    {
        //        this.pingInterval = value;
        //        if (this.timer != null)
        //            this.timer.Interval = value;
        //    }
        //}

        protected virtual bool SubscribeSymbols()
        {
            try
            {
                IEnumerable<string> topics = GetSubscribeTopics();
                if (topics != null && topics.Any())
                {
                    foreach (string t in topics)
                    {
                        LogRecord.Info("StompWebSocketMaketClientBase", $"{Name} begin subscribe {t}");
                        SubscribeTopic(webSocket, t);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogRecord.Error("StompWebSocketMaketClientBase", $"{Name} subscribe symbols error {ex}.");
            }
            return false;
        }

        protected abstract void SubscribeTopic(WebSocketClient webSocket, string data);


        protected virtual bool StompConnect()
        {
            try
            {
                StompConnect(webSocket);
                return true;
            }
            catch (Exception ex)
            {
                LogRecord.Error("StompWebSocketMaketClientBase", $"{Name} subscribe symbols error {ex}.");
            }
            return false;
        }
        protected abstract void StompConnect(WebSocketClient webSocket);

        protected abstract IEnumerable<string> GetSubscribeTopics();

        protected abstract void Ping(WebSocketClient webSocket);

        protected abstract void OnReceiveMessage(WebSocketClient webSocket, MessageEventArgs e);

        protected bool Ping()
        {
            try
            {
                Ping(webSocket);
                return true;
            }
            catch (Exception ex)
            {
                LogRecord.Warn("StompWebSocketMaketClientBase", $"{Name} Ping error {ex}.");
            }
            return false;
        }
        private void DisablePing()
        {
            //if (this.timer != null)
            //    this.timer.Enabled = false;
        }

        private void EnablePing()
        {
            //if (this.timer != null)
            //    this.timer.Enabled = true;
        }

        protected void Init(System.Net.CookieContainer cookies = null, Dictionary<string, string> requestHeader = null)
        {
            //if (this.webSocket == null)
            {
                Address = ReBuildAddress(Address);
                webSocket = new WebSocketClient(Address);
                webSocket.Socket.Options.Cookies = cookies;
                if (requestHeader != null)
                {
                    foreach (KeyValuePair<string, string> item in requestHeader)
                    {
                        webSocket.Socket.Options.SetRequestHeader(item.Key, item.Value);
                    }
                }
                webSocket.ConnectionOpened += WebSocketOnOpen;
                webSocket.MessageReceived += WebSocketOnMessage;
                webSocket.ConnectionClosed += WebSocketOnClosed;
                webSocket.Error += WebSocketOnError;

                if (AutoPing)
                {
                    Task.Run(() =>
                    {
                        while (true)
                        {
                            Timer_Elapsed(null, null);
                            Thread.Sleep(3000);
                        }
                    });
                    //this.timer = new System.Timers.Timer(this.PingInterval);
                    //this.timer.Elapsed += Timer_Elapsed;
                    webSocket.ConnectionClosed += delegate
                    {
                        DisablePing();
                    };
                }
            }
        }

        protected virtual string ReBuildAddress(string oldAddress)
        {
            return oldAddress;
        }
        public virtual bool Connect(System.Net.CookieContainer cookies = null, Dictionary<string, string> requestHeader = null)
        {
            Init(cookies, requestHeader);
            return webSocket.Connect();
        }
        public void Close()
        {
            webSocket.Close();
        }

        public void Dispose()
        {
            webSocket.Close();
            DisablePing();
            //this.timer.Dispose();
        }
    }
}
