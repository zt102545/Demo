using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Http
    /// </summary>
    public static class HttpHelper
    {
        private const int Timeout = 100000;

        public static string Request(string url, string method, byte[] data, string contentType, bool useProxy = false,
            Encoding encoding = null, int timeout = Timeout, NameValueCollection header = null, string userAgent = null,
            string referer = null, CookieContainer cookieContainer = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            request.Method = method;
            request.ContentType = contentType;
            request.Timeout = timeout;
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }

            if (!string.IsNullOrEmpty(referer))
            {
                request.Referer = referer;
            }

            if (header != null && header.Count > 0)
            {
                request.Headers.Add(header);
            }

            if (useProxy)
            {
                string proxyIP = HttpProxy.GetRandomProxyIP();
                if (proxyIP != "")
                {
                    IWebProxy webProxy = new WebProxy(HttpProxy.GetRandomProxyIP(), 3128);
                    request.Proxy = webProxy;
                }
            }

            if (cookieContainer != null)
            {
                request.CookieContainer = cookieContainer;
            }

            if (method.ToLower() != "get" && data != null)
            {
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(data, 0, data.Length);
                }
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, encoding))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        public static async Task<string> RequestAsync(string url, string method, byte[] data,
            string contentType, Encoding encoding = null, int timeout = Timeout, NameValueCollection header = null,
            string userAgent = null, IWebProxy webProxy = null, string referer = null, CookieContainer cookieContainer = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            HttpWebRequest request = HttpWebRequest.CreateHttp(url);
            request.Method = method;
            request.ContentType = contentType;
            request.Timeout = timeout;
            if (!string.IsNullOrEmpty(userAgent))
            {
                request.UserAgent = userAgent;
            }

            if (!string.IsNullOrEmpty(referer))
            {
                request.Referer = referer;
            }

            if (header != null && header.Count > 0)
            {
                request.Headers.Add(header);
            }

            if (webProxy != null)
            {
                request.Proxy = webProxy;
            }

            if (cookieContainer != null)
            {
                request.CookieContainer = cookieContainer;
            }

            if (method.ToLower() != "get" && data != null)
            {
                using (Stream requestStream = await request.GetRequestStreamAsync())
                {
                    await requestStream.WriteAsync(data, 0, data.Length);
                }
            }

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, encoding))
                    {
                        return await reader.ReadToEndAsync();
                    }
                }
            }
        }

        public static string Request(string url, string method, string data, string contentType, bool useProxy = false,
            Encoding encoding = null, int timeout = Timeout, NameValueCollection header = null, string userAgent = null, string referer = null, CookieContainer cookieContainer = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            byte[] buffer = encoding.GetBytes(data);
            return Request(url, method, buffer, contentType, useProxy, encoding, timeout, header, userAgent, referer, cookieContainer);
        }

        public static string Request(string url, string method, Stream data, string contentType, bool useProxy = false,
            Encoding encoding = null, int timeout = Timeout, NameValueCollection header = null, string userAgent = null,
            string referer = null, CookieContainer cookieContainer = null)
        {
            if (data == null)
            {
                return "";
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            StreamReader reader = new StreamReader(data);
            string strBody = reader.ReadToEnd();
            return Request(url, method, strBody, contentType, useProxy, encoding, timeout, header, userAgent, referer, cookieContainer);
        }

        public static async Task<string> RequestAsync(string url, string method, string data,
            string contentType, Encoding encoding = null, int timeout = Timeout, NameValueCollection header = null,
            string userAgent = null, IWebProxy webProxy = null, string referer = null, CookieContainer cookieContainer = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            byte[] buffer = encoding.GetBytes(data);
            return await RequestAsync(url, method, buffer, contentType, encoding, timeout, header, userAgent, webProxy,
                referer, cookieContainer);
        }

        public static string Post(string url, string data, string contentType, bool useProxy = false, Encoding encoding = null,
            int timeout = Timeout, NameValueCollection header = null, string userAgent = null,
            string referer = null, CookieContainer cookieContainer = null)
        {
            return Request(url, "POST", data, contentType, useProxy, encoding, timeout, header, userAgent, referer, cookieContainer);
        }

        public static async Task<string> PostAsync(string url, string data, string contentType,
            Encoding encoding = null, int timeout = Timeout, NameValueCollection header = null, string userAgent = null,
            IWebProxy webProxy = null, string referer = null, CookieContainer cookieContainer = null)
        {
            return await RequestAsync(url, "POST", data, contentType, encoding, timeout, header, userAgent, webProxy,
                referer, cookieContainer);
        }

        public static string Post(string url, byte[] data, string contentType, bool useProxy = false, Encoding encoding = null,
            int timeout = Timeout, NameValueCollection header = null, string userAgent = null,
            string referer = null, CookieContainer cookieContainer = null)
        {
            return Request(url, "POST", data, contentType, useProxy, encoding, timeout, header, userAgent, referer, cookieContainer);
        }

        public static async Task<string> PostAsync(string url, byte[] data, string contentType,
            Encoding encoding = null, int timeout = Timeout, NameValueCollection header = null, string userAgent = null,
            IWebProxy webProxy = null, string referer = null, CookieContainer cookieContainer = null)
        {
            return await RequestAsync(url, "POST", data, contentType, encoding, timeout, header, userAgent, webProxy,
                referer, cookieContainer);
        }

        public static string Get(string url, bool useProxy = false, Encoding encoding = null, int timeout = Timeout,
            NameValueCollection header = null, string userAgent = null,
            string referer = null, CookieContainer cookieContainer = null)
        {
            return Request(url, "GET", string.Empty, null, useProxy, encoding, timeout, header, userAgent, referer, cookieContainer);
        }

        public static async Task<string> GetAsync(string url, Encoding encoding = null,
            int timeout = Timeout, NameValueCollection header = null, string userAgent = null,
            IWebProxy webProxy = null, string referer = null, CookieContainer cookieContainer = null)
        {
            return await RequestAsync(url, "GET", string.Empty, null, encoding, timeout, header, userAgent, webProxy,
                referer, cookieContainer);
        }

        public static string PostForm(string url, string data, bool useProxy = false, Encoding encoding = null, int timeout = Timeout,
            NameValueCollection header = null, string userAgent = null,
            string referer = null, CookieContainer cookieContainer = null)
        {
            return Post(url, data, "application/x-www-form-urlencoded", useProxy, encoding, timeout, header, userAgent,
                referer, cookieContainer);
        }

        public static async Task<string> PostFormAsync(string url, string data,
            Encoding encoding = null, int timeout = Timeout, NameValueCollection header = null, string userAgent = null,
            IWebProxy webProxy = null, string referer = null, CookieContainer cookieContainer = null)
        {
            return await PostAsync(url, data, "application/x-www-form-urlencoded", encoding, timeout, header, userAgent,
                webProxy, referer, cookieContainer);
        }

        public static string PostForm(string url, byte[] data, bool useProxy = false, Encoding encoding = null, int timeout = Timeout,
            NameValueCollection header = null, string userAgent = null,
            string referer = null, CookieContainer cookieContainer = null)
        {
            return Post(url, data, "application/x-www-form-urlencoded", useProxy, encoding, timeout, header, userAgent,
                referer, cookieContainer);
        }

        public static async Task<string> PostFormAsync(string url, byte[] data,
            Encoding encoding = null, int timeout = Timeout, NameValueCollection header = null, string userAgent = null,
            IWebProxy webProxy = null, string referer = null, CookieContainer cookieContainer = null)
        {
            return await PostAsync(url, data, "application/x-www-form-urlencoded", encoding, timeout, header, userAgent,
                webProxy, referer, cookieContainer);
        }

        public static string PostJson(string url, string data, bool useProxy = false, Encoding encoding = null, int timeout = Timeout,
            NameValueCollection header = null, string userAgent = null,
            string referer = null, CookieContainer cookieContainer = null)
        {
            return Post(url, data, "application/json", useProxy, encoding, timeout, header, userAgent, referer, cookieContainer);
        }

        public static async Task<string> PostJsonAsync(string url, string data,
            Encoding encoding = null, int timeout = Timeout, NameValueCollection header = null, string userAgent = null,
            IWebProxy webProxy = null, string referer = null, CookieContainer cookieContainer = null)
        {
            return await PostAsync(url, data, "application/json", encoding, timeout, header, userAgent, webProxy,
                referer, cookieContainer);
        }

        public static string PostJson(string url, byte[] data, bool useProxy = false, Encoding encoding = null, int timeout = Timeout,
            NameValueCollection header = null, string userAgent = null,
            string referer = null, CookieContainer cookieContainer = null)
        {
            return Post(url, data, "application/json", useProxy, encoding, timeout, header, userAgent, referer, cookieContainer);
        }

        public static async Task<string> PostJsonAsync(string url, byte[] data,
            Encoding encoding = null, int timeout = Timeout, NameValueCollection header = null, string userAgent = null,
            IWebProxy webProxy = null, string referer = null, CookieContainer cookieContainer = null)
        {
            return await PostAsync(url, data, "application/json", encoding, timeout, header, userAgent, webProxy,
                referer, cookieContainer);
        }
    }
}
