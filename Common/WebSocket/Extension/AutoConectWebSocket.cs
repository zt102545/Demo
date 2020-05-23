using Common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.WebSockets;
using System.Threading;

namespace Common
{
    public class AutoConectWebSocket : WebSocketClient
    {
        private readonly AutoResetEvent resetEvent = new AutoResetEvent(false);
        public AutoConectWebSocket(string url, bool autoConnect = true,bool useProxy = false,System.Net.CookieContainer cookies = null, Dictionary<string, string> requestHeader = null) : base(url)
        {
            if (autoConnect)
            {
                Thread thread = new Thread(() =>
                {
                    while (true)
                    {
                        try
                        {
                            if (ReadyState != WebSocketState.Open)
                            {
                                if (cookies != null)
                                {
                                    Socket.Options.Cookies = cookies;
                                }

                                if(ReadyState != WebSocketState.Open && ReadyState != WebSocketState.Connecting)
                                {
                                    if (useProxy)
                                    {
                                        string proxyIP = HttpProxy.GetRandomProxyIP();
                                        if (proxyIP != "")
                                        {
                                            IWebProxy webProxy = new WebProxy(proxyIP, HttpProxy.port);
                                            Socket.Options.Proxy = webProxy;
                                        }
                                    }
                                }

                                if (requestHeader != null)
                                {
                                    foreach (KeyValuePair<string, string> item in requestHeader)
                                    {
                                        Socket.Options.SetRequestHeader(item.Key, item.Value);
                                    }
                                }
                                Connect();
                            }
                        }
                        catch (Exception ex)
                        {
                            LogRecord.Warn("AutoConectWebSocket", string.Format("Client connect {0} error {1}.", Url, ex.ToString()));
                        }
                        resetEvent.WaitOne(3000);
                    }
                });
                thread.Start();
            }

            ConnectionClosed += AutoConectWebSocket_OnClose;
            Error += AutoConectWebSocket_OnError;
            ConnectionOpened += AutoConectWebSocket_OnOpen;
        }

        private void AutoConectWebSocket_OnOpen(object sender, EventArgs e)
        {
            Common.LogRecord.Info("AutoConectWebSocket", string.Format("Client connect {0}.", Url));
        }

        private void AutoConectWebSocket_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.IsBinary)
            {
                Common.LogRecord.Debug("AutoConectWebSocket", string.Format("Client receive message from {0}, size:{1}bytes", Url, e.RawData.Length));
            }
            else if (e.IsText)
            {
                Common.LogRecord.Debug("AutoConectWebSocket", "Client Receive:" + e.Data);
            }
        }

        private void AutoConectWebSocket_OnError(object sender, ErrorEventArgs e)
        {
            Common.LogRecord.Error("AutoConectWebSocket", string.Format("Client error {0}. Message:{1}", Url, e.Message));
        }

        private void AutoConectWebSocket_OnClose(object sender, CloseEventArgs e)
        {
            Common.LogRecord.Info("AutoConectWebSocket", string.Format("Client closed {0}.", Url));
        }
    }
}