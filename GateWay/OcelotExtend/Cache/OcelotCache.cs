using Common;
using Ocelot.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GateWay.OcelotExtend
{
    /// <summary>
    /// 自定义Ocelot的缓存类，实现IOcelotCache接口
    /// </summary>
    public class OcelotCache : IOcelotCache<CachedResponse>
    {
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ttl"></param>
        /// <param name="region"></param>
        public void Add(string key, CachedResponse value, TimeSpan ttl, string region)
        {
            Console.WriteLine($"This is OcelotCache.Add");
            MemoryCache.Set(key, value, ttl.TotalSeconds);
        }

        /// <summary>
        /// 覆盖缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ttl"></param>
        /// <param name="region"></param>
        public void AddAndDelete(string key, CachedResponse value, TimeSpan ttl, string region)
        {
            Console.WriteLine($"This is OcelotCache.AddAndDelete");
            MemoryCache.Remove(key);
            MemoryCache.Set(key, value, ttl.TotalSeconds);
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="region">OcelotConfig.json里配置的region</param>
        public void ClearRegion(string region)
        {
            Console.WriteLine($"This is OcelotCache.ClearRegion");
            //简单处理，清除所有缓存，根据需要自己优化
            MemoryCache.RemoveAll();
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public CachedResponse Get(string key, string region)
        {
            try
            {
                Console.WriteLine($"This is OcelotCache.Get");
                return (CachedResponse)MemoryCache.Get(key);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
