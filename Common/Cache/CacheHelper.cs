using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Cache
{
    public class CacheHelper
    {
        /// <summary>
        /// 用MemoryCache缓存getData的结果。可以减少对db和redis的读取，也可以减少任何其他函数的执行。
        /// Func部分可以 ()=>getData(a,b,c)
        /// 用例：var preData = CacheHelper.WithCache("userstock_GetUSPreDataTradingCode_" + stockCode, ()=>Redis.QuoteData.GetUSPreDataTradingCode(stockCode), 5000);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">注意key要够独特</param>
        /// <param name="getData"></param>
        /// <param name="arg"></param>
        /// <param name="cacheMilli"></param>
        /// <returns></returns>
        public static T WithCache<T>(string key, Func<T> getData, int cacheSec = 1000) where T : new()
        {
            object obj = MemoryCache.Get(key);
            if (obj != null)
            {
                return (T)obj;
            }
            else
            {
                T dd = getData();
                if (dd == null)
                {
                    dd = new T();
                }
                MemoryCache.Set(key, dd, cacheSec);
                return dd;
            }
        }

        public static void RemoveCache(string key)
        {
            MemoryCache.Remove(key);
        }

        public static void RemoveAllCache()
        {
            MemoryCache.RemoveAll();
        }

    }
}
