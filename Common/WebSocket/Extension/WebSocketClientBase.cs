using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;

namespace Common
{
    public abstract class WebSocketClientBase : IDisposable
    {

        protected AutoConectWebSocket webSocket;
        private System.Timers.Timer timer;

        public WebSocketClientBase(string name, string address, bool autoPing = false)
        {
            Name = name;
            Address = address;
            AutoPing = autoPing;
            Logined = false;
        }

        public string Address { get; private set; }

        public bool AutoPing { get; private set; }

        public bool Logined { get; set; }

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
                Common.LogRecord.Error("WebSocketClientBase", $"{Name} receive message error {ex}.");
            }
        }

        protected virtual void WebSocketOnOpen(object sender, EventArgs e)
        {
            Login();

            Thread.Sleep(1000 * 5);

            while (!Subscribe())
            {
                Thread.Sleep(10000);
            }

            EnablePing();
        }

        protected virtual void WebSocketOnClosed(object sender, CloseEventArgs e)
        {
            Common.LogRecord.Error("WebSocketClientBase", $"{Name} was closed.");
        }

        protected virtual void WebSocketOnError(object sender, ErrorEventArgs e)
        {
            Common.LogRecord.Error("WebSocketClientBase", $"{Name} has error:{e.Exception}.");
        }

        public string Name { get; private set; }

        private int pingInterval = 30000;
        public int PingInterval
        {
            get => pingInterval;
            set
            {
                pingInterval = value;
                if (timer != null)
                {
                    timer.Interval = value;
                }
            }
        }

        public IEnumerable<string> CustomSubscribeSymbols { get; set; } = null;
        protected virtual bool Subscribe()
        {
            try
            {
                IEnumerable<string> symbols = CustomSubscribeSymbols != null ? CustomSubscribeSymbols : GetSubscribeSymbols();
                if (symbols != null && symbols.Any())
                {
                    foreach (string s in symbols)
                    {
                        Common.LogRecord.Info("WebSocketClientBase", $"{Name} begin subscribe {s}");
                        Subscribe(webSocket, s);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Common.LogRecord.Error("WebSocketClientBase", $"{Name} subscribe symbols error {ex}.");
            }
            return false;
        }

        protected abstract void Subscribe(AutoConectWebSocket webSocket, string symbol);

        protected abstract IEnumerable<string> GetSubscribeSymbols();

        protected virtual void Login()
        {
            try
            {
                Login(webSocket);
            }
            catch (Exception ex)
            {
                Common.LogRecord.Error("WebSocketClientBase", $"Login error {ex}.");
            }
        }

        protected abstract void Login(AutoConectWebSocket webSocket);


        protected abstract void Ping(AutoConectWebSocket webSocket);

        protected abstract void OnReceiveMessage(AutoConectWebSocket webSocket, MessageEventArgs e);

        protected bool Ping()
        {
            try
            {
                Ping(webSocket);
                return true;
            }
            catch (Exception ex)
            {
                Common.LogRecord.Warn("WebSocketClientBase", $"{Name} Ping error {ex}.");
            }
            return false;
        }
        private void DisablePing()
        {
            if (timer != null)
            {
                timer.Enabled = false;
            }
        }

        private void EnablePing()
        {
            if (timer != null)
            {
                timer.Enabled = true;
            }
        }

        protected void Init(bool useProxy = false,System.Net.CookieContainer cookies = null, Dictionary<string, string> requestHeader = null, bool autoConnect = true)
        {
            if (webSocket == null)
            {
                Address = ReBuildAddress(Address);
                Common.LogRecord.Info("WebSocketClientBase", $"初始化websocket：{Address}....");
                webSocket = new AutoConectWebSocket(Address, cookies: cookies, useProxy: useProxy, requestHeader: requestHeader, autoConnect: autoConnect);
                webSocket.ConnectionOpened += WebSocketOnOpen;
                webSocket.MessageReceived += WebSocketOnMessage;
                webSocket.ConnectionClosed += WebSocketOnClosed;
                webSocket.Error += WebSocketOnError;

                if (AutoPing)
                {
                    timer = new System.Timers.Timer(PingInterval);
                    timer.Elapsed += Timer_Elapsed;
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
        public virtual bool Connect(bool useProxy = false, System.Net.CookieContainer cookies = null, Dictionary<string, string> requestHeader = null, bool autoConnect = true)
        {
            Init(useProxy,cookies, requestHeader, autoConnect);

            if (autoConnect && useProxy)
            {
                System.Threading.Thread.Sleep(1000);
            }

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
            timer.Dispose();
        }
    }
}
