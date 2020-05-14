using System.Collections.Specialized;
using System.Net;
using System.Text;
using RestSharp;

namespace Common
{
    /// <summary>
    /// 基于RestSharp组件的HTTP
    /// </summary>
    public class HttpRest
    {
        private const int Timeout = 100000;

        public static string Post(string url, string data, string contentType, bool useProxy = false,
            Encoding encoding = null, int timeout = Timeout, NameValueCollection header = null, string userAgent = null,
            string referer = null, CookieContainer cookieContainer = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            var client = new RestClient(url);
            client.Timeout = timeout;
            if (!string.IsNullOrEmpty(userAgent))
            {
                client.UserAgent = userAgent;
            }

            if (useProxy)
            {
                string proxyIP = HttpProxy.GetRandomProxyIP();
                if (proxyIP != "")
                {
                    IWebProxy webProxy = new WebProxy(HttpProxy.GetRandomProxyIP(), 3128);
                    client.Proxy = webProxy;
                }
            }

            if (cookieContainer != null)
            {
                client.CookieContainer = cookieContainer;
            }

            var request = new RestRequest(Method.POST);
            if (header != null && header.Count > 0)
            {
                string[] keys = header.AllKeys;
                foreach (string key in keys)
                {
                    request.AddHeader(key, header[key]);
                }
            }

            request.AddParameter(contentType, data, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            return response.Content;
        }
    }

}
