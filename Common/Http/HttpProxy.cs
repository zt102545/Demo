using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    /// <summary>
    /// 代理域名
    /// </summary>
    public class HttpProxy
    {
        private static List<string> proxyList = new List<string>()
            { "103.241.229.2", "103.241.229.3" };
        public static int port = 3128;

        public static string GetRandomProxyIP()
        {
            int proxyNum = proxyList.Count;
            int randomNum = RandomHelper.Random(0, proxyNum);

            return proxyList[randomNum];
        }

    }
}
