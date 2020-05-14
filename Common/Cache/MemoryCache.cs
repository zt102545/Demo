using System;
using Microsoft.Extensions.Caching.Memory;


namespace Common
{
    public class MemoryCache
    {
        private static IMemoryCache cache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new MemoryCacheOptions());

        /// <summary>
        /// 设置超时时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="duration">以秒为单位</param>
        public static void Set(string key, object value, double duration = 0)
        {
            if (!string.IsNullOrEmpty(key))
            {
                cache.Set(key, value, new MemoryCacheEntryOptions().SetAbsoluteExpiration(DateTimeOffset.Now.AddSeconds(duration)));
            }
        }

        public static object Get(string key)
        {
            if (key != null && cache.TryGetValue(key, out object val))
            {
                return val;
            }
            else
            {
                return null;
            }
        }

        public static void Remove(string key)
        {
            cache.Remove(key);
        }

        public static void RemoveAll()
        {
            cache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new MemoryCacheOptions());
        }
    }
}
