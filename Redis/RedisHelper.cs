using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Redis
{
    class RedisHelper
    {
        #region 配置属性   基于 StackExchange.Redis 封装
        //连接串 （注：IP:端口,属性=,属性=)
        public string _ConnectionString = "127.0.0.1:6379,password=123456";
        //操作的库（注：默认0库）
        public int _Db = 0;
        #endregion

        #region 管理器对象

        /// <summary>
        /// 获取redis操作类对象
        /// </summary>
        private static RedisHelper _StackRedis;
        private static readonly object _locker_StackRedis = new object();
        public static RedisHelper Current
        {
            get
            {
                if (_StackRedis == null)
                {
                    lock (_locker_StackRedis)
                    {
                        _StackRedis = _StackRedis ?? new RedisHelper();
                        return _StackRedis;
                    }
                }

                return _StackRedis;
            }
        }

        /// <summary>
        /// 获取并发链接管理器对象
        /// </summary>
        private static ConnectionMultiplexer _redis;
        private static readonly object _locker = new object();
        public ConnectionMultiplexer Manager
        {
            get
            {
                if (_redis == null)
                {
                    lock (_locker)
                    {
                        _redis = _redis ?? GetManager(this._ConnectionString);
                        return _redis;
                    }
                }

                return _redis;
            }
        }

        /// <summary>
        /// 获取链接管理器
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public ConnectionMultiplexer GetManager(string connectionString)
        {
            return ConnectionMultiplexer.Connect(connectionString);
        }

        /// <summary>
        /// 获取操作数据库对象
        /// </summary>
        /// <returns></returns>
        public IDatabase GetDb()
        {
            return Manager.GetDatabase(_Db);
        }
        #endregion

        #region 操作方法

        #region string 操作

        /// <summary>
        /// 根据Key移除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> Remove(string key)
        {
            var db = this.GetDb();

            return await db.KeyDeleteAsync(key);
        }

        /// <summary>
        /// 根据key获取string结果
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> Get(string key)
        {
            var db = this.GetDb();
            return await db.StringGetAsync(key);
        }

        /// <summary>
        /// 根据key获取string中的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> Get<T>(string key)
        {
            var t = default(T);
            try
            {
                var _str = await this.Get(key);
                if (string.IsNullOrWhiteSpace(_str)) { return t; }

                t = JsonConvert.DeserializeObject<T>(_str);
            }
            catch (Exception ex) { }
            return t;
        }

        /// <summary>
        /// 存储string数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireMinutes"></param>
        /// <returns></returns>
        public async Task<bool> Set(string key, string value, int expireMinutes = 0)
        {
            var db = this.GetDb();
            if (expireMinutes > 0)
            {
                return db.StringSet(key, value, TimeSpan.FromMinutes(expireMinutes));
            }
            return await db.StringSetAsync(key, value);
        }

        /// <summary>
        /// 存储对象数据到string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireMinutes"></param>
        /// <returns></returns>
        public async Task<bool> Set<T>(string key, T value, int expireMinutes = 0)
        {
            try
            {
                var jsonOption = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                var _str = JsonConvert.SerializeObject(value, jsonOption);
                if (string.IsNullOrWhiteSpace(_str)) { return false; }

                return await this.Set(key, _str, expireMinutes);
            }
            catch (Exception ex) { }
            return false;
        }

        /// <summary>
        /// 是否存在key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> KeyExists(string key)
        {
            try
            {
                var db = this.GetDb();
                return await db.KeyExistsAsync(key);
            }
            catch (Exception ex) { }
            return false;
        }

        #endregion

        #region hash操作

        /// <summary>
        /// 是否存在hash的列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="filedKey"></param>
        /// <returns></returns>
        public async Task<bool> HashFieldExists(string key, string filedKey)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(filedKey)) { return false; }

                var result = await this.HashFieldsExists(key, new Dictionary<string, bool> { { filedKey, false } });
                return result[filedKey];
            }
            catch (Exception ex) { }
            return false;
        }

        /// <summary>
        /// 是否存在hash的列集合
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dics"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, bool>> HashFieldsExists(string key, Dictionary<string, bool> dics)
        {
            try
            {
                if (dics.Count <= 0) { return dics; }

                var db = this.GetDb();
                foreach (var fieldKey in dics.Keys)
                {
                    dics[fieldKey] = await db.HashExistsAsync(key, fieldKey);
                }
            }
            catch (Exception ex) { }
            return dics;
        }

        /// <summary>
        /// 设置hash
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="filedKey"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<long> SetOrUpdateHashsField<T>(string key, string filedKey, T t, bool isAdd = true)
        {
            var result = 0L;
            try
            {
                return await this.SetOrUpdateHashsFields<T>(key, new Dictionary<string, T> { { filedKey, t } }, isAdd);
            }
            catch (Exception ex) { }
            return result;
        }

        /// <summary>
        /// 设置hash集合，添加和更新操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dics"></param>
        /// <returns></returns>
        public async Task<long> SetOrUpdateHashsFields<T>(string key, Dictionary<string, T> dics, bool isAdd = true)
        {
            var result = 0L;
            try
            {
                var jsonOption = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                var db = this.GetDb();
                foreach (var fieldKey in dics.Keys)
                {
                    var item = dics[fieldKey];
                    var _str = JsonConvert.SerializeObject(item, jsonOption);
                    result += await db.HashSetAsync(key, fieldKey, _str) ? 1 : 0;
                    if (!isAdd) { result++; }
                }
                return result;
            }
            catch (Exception ex) { }
            return result;
        }

        /// <summary>
        /// 移除hash的列
        /// </summary>
        /// <param name="key"></param>
        /// <param name="filedKey"></param>
        /// <returns></returns>
        public async Task<bool> RemoveHashField(string key, string filedKey)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(filedKey)) { return false; }

                var result = await this.RemoveHashFields(key, new Dictionary<string, bool> { { filedKey, false } });
                return result[filedKey];
            }
            catch (Exception ex) { }
            return false;
        }

        /// <summary>
        /// 异常hash的列集合
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dics"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, bool>> RemoveHashFields(string key, Dictionary<string, bool> dics)
        {

            try
            {
                var jsonOption = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                var db = this.GetDb();
                foreach (var fieldKey in dics.Keys)
                {
                    dics[fieldKey] = await db.HashDeleteAsync(key, fieldKey);
                }
                return dics;
            }
            catch (Exception ex) { }
            return dics;
        }

        /// <summary>
        /// 设置hash
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="filedKey"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<T> GetHashField<T>(string key, string filedKey)
        {
            var t = default(T);
            try
            {
                var dics = await this.GetHashFields<T>(key, new Dictionary<string, T> { { filedKey, t } });
                return dics[filedKey];
            }
            catch (Exception ex) { }
            return t;
        }

        /// <summary>
        /// 获取hash的列值集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="dics"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, T>> GetHashFields<T>(string key, Dictionary<string, T> dics)
        {
            try
            {
                var db = this.GetDb();
                foreach (var fieldKey in dics.Keys)
                {
                    var str = await db.HashGetAsync(key, fieldKey);
                    if (string.IsNullOrWhiteSpace(str)) { continue; }

                    dics[fieldKey] = JsonConvert.DeserializeObject<T>(str);
                }
                return dics;
            }
            catch (Exception ex) { }
            return dics;
        }

        /// <summary>
        /// 获取hash的key的所有列的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, T>> GetHashs<T>(string key)
        {
            var dic = new Dictionary<string, T>();
            try
            {
                var db = this.GetDb();

                var hashFiles = await db.HashGetAllAsync(key);
                foreach (var field in hashFiles)
                {
                    dic[field.Name] = JsonConvert.DeserializeObject<T>(field.Value);
                }
                return dic;
            }
            catch (Exception ex) { }
            return dic;
        }

        /// <summary>
        /// 获取hash的Key的所有列的值的list集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<T>> GetHashsToList<T>(string key)
        {
            var list = new List<T>();
            try
            {
                var db = this.GetDb();

                var hashFiles = await db.HashGetAllAsync(key);
                foreach (var field in hashFiles)
                {
                    var item = JsonConvert.DeserializeObject<T>(field.Value);
                    if (item == null) { continue; }
                    list.Add(item);
                }
            }
            catch (Exception ex) { }
            return list;
        }

        #endregion

        #region List操作（注：可以当做队列使用）

        /// <summary>
        /// list长度
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<long> GetListLen<T>(string key)
        {
            try
            {
                var db = this.GetDb();
                return await db.ListLengthAsync(key);
            }
            catch (Exception ex) { }
            return 0;
        }

        /// <summary>
        /// 获取List数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<List<T>> GetList<T>(string key)
        {
            var t = new List<T>();
            try
            {
                var db = this.GetDb();
                var _values = await db.ListRangeAsync(key);
                foreach (var item in _values)
                {
                    if (string.IsNullOrWhiteSpace(item)) { continue; }
                    t.Add(JsonConvert.DeserializeObject<T>(item));
                }
            }
            catch (Exception ex) { }
            return t;
        }

        /// <summary>
        /// 获取队列出口数据并移除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> GetListAndPop<T>(string key)
        {
            var t = default(T);
            try
            {
                var db = this.GetDb();
                var _str = await db.ListRightPopAsync(key);
                if (string.IsNullOrWhiteSpace(_str)) { return t; }
                t = JsonConvert.DeserializeObject<T>(_str);
            }
            catch (Exception ex) { }
            return t;
        }

        /// <summary>
        /// 集合对象添加到list左边
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public async Task<long> SetLists<T>(string key, List<T> values)
        {
            var result = 0L;
            try
            {
                var jsonOption = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                var db = this.GetDb();
                foreach (var item in values)
                {
                    var _str = JsonConvert.SerializeObject(item, jsonOption);
                    result += await db.ListLeftPushAsync(key, _str);
                }
                return result;
            }
            catch (Exception ex) { }
            return result;
        }

        /// <summary>
        /// 单个对象添加到list左边
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<long> SetList<T>(string key, T value)
        {
            var result = 0L;
            try
            {
                result = await this.SetLists(key, new List<T> { value });
            }
            catch (Exception ex) { }
            return result;
        }


        #endregion

        #region 额外扩展

        public async Task<List<string>> MatchKeys(params string[] paramArr)
        {
            var list = new List<string>();
            try
            {
                var result = await this.ExecuteAsync("keys", paramArr);

                var valArr = ((RedisValue[])result);
                foreach (var item in valArr)
                {
                    list.Add(item);
                }
            }
            catch (Exception ex) { }
            return list;
        }

        /// <summary>
        /// 执行redis原生命令
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="paramArr"></param>
        /// <returns></returns>
        public async Task<RedisResult> ExecuteAsync(string cmd, params string[] paramArr)
        {
            try
            {
                var db = this.GetDb();
                return await db.ExecuteAsync(cmd, paramArr);
            }
            catch (Exception ex) { }
            return default(RedisResult);
        }

        /// <summary>
        /// 手动回收管理器对象
        /// </summary>
        public void Dispose()
        {
            this.Dispose(_redis);
        }

        public void Dispose(ConnectionMultiplexer con)
        {
            if (con != null)
            {
                con.Close();
                con.Dispose();
            }
        }

        #endregion

        #endregion
    }
}
