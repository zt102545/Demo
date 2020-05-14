using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Common
{
    /// <summary>
    /// 方便易用的json封装
    /// </summary>
    public class JsonString
    {
        Dictionary<string, object> resultDic;

        public JsonString()
        {
            resultDic = new Dictionary<string, object>();
        }

        /// <summary>
        /// 失败可能抛异常。TryParse方法可以避免抛异常。
        /// </summary>
        /// <param name="json"></param>
        public JsonString(string json)
        {
            resultDic = new Dictionary<string, object>();
            var ay = ParseJson(json);
            foreach (var item in ay)
            {
                this.Set(item.Key, item.Value);
            }
        }

        public JsonString(IDictionary<string, object> ay)
        {
            resultDic = new Dictionary<string, object>();
            foreach (var item in ay)
            {
                this.Set(item.Key, item.Value);
            }
        }

        public JsonString(ExpandoObject exp) : this(exp, 0)
        {
        }

        protected JsonString(ExpandoObject exp, int depth)
        {
            if (depth > 10)
            {
                throw new Exception("too much recursive, maybe self-reference");
            }
            var ay = exp.ToList();
            resultDic = new Dictionary<string, object>();
            foreach (var item in ay)
            {
                if (item.Value is ExpandoObject)
                {
                    JsonString sub = new JsonString(item.Value as ExpandoObject, depth + 1);
                    resultDic.Add(item.Key, sub.ToDic());
                }
                else
                {
                    resultDic.Add(item.Key, item.Value);
                }
            }
        }

        /// <summary>
        /// 根据obj的公共属性来生成JsonString对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static JsonString FromModel(object obj)
        {
            string ss = JsonConvert.SerializeObject(obj);
            JsonString js = new JsonString(ss);
            return js;
        }

        /// <summary>
        /// key存在就更新，不存在就新增
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, object value)
        {
            if (resultDic.ContainsKey(key))
            {
                resultDic[key] = value;
            }
            else
            {
                resultDic.Add(key, value);
            }
        }

        /// <summary>
        /// key存在就更新，不存在就新增（符合一般逻辑的支持JsonString类型的value）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, JsonString value)
        {
            if (resultDic.ContainsKey(key))
            {
                resultDic[key] = value.ToDic();
            }
            else
            {
                resultDic.Add(key, value.ToDic());
            }
        }

        /// <summary>
        /// 设置价格或者金额，保留最多四位小数
        /// </summary>
        /// <param name="price"></param>
        /// <param name="keepDecimalZero">小数点保留几个零。0表示不额外保留</param>
        public void SetPrice(string key, double price, int keepDecimalZero = 0)
        {
            string ss;
            if (keepDecimalZero == 0)
            {
                ss = price.ToString("0.####");
            }
            else
            {
                ss = price.ToString("f" + keepDecimalZero);
            }

            Set(key, ss);
        }

        /// <summary>
        /// 设置价格或者金额，保留最少两位小数，最多四位小数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="price"></param>
        public void SetPrice2(string key, double price)
        {
            string ss = price.ToString("0.00##");

            Set(key, ss);
        }

        /// <summary>
        /// 给名为arrayName的数组，新增一个json格式的条目
        /// </summary>
        /// <param name="arrayName"></param>
        /// <param name="itemKey"></param>
        /// <param name="itemValue"></param>
        public void AddArrayItem(string arrayName, JsonString item)
        {
            var dic = item.ToDic();
            if (resultDic.ContainsKey(arrayName))
            {
                List<Dictionary<string, object>> list = (List<Dictionary<string, object>>)resultDic[arrayName];
                list.Add(dic);
            }
            else
            {
                var list = new List<Dictionary<string, object>>();
                list.Add(dic);
                resultDic.Add(arrayName, list);
            }
        }

        /// <summary>
        /// 给名为arrayName的数组，新增一个object的条目
        /// </summary>
        /// <param name="arrayName"></param>
        /// <param name="item"></param>
        public void AddArrayItem(string arrayName, object item)
        {
            if (resultDic.ContainsKey(arrayName))
            {
                List<object> list = (List<object>)resultDic[arrayName];
                list.Add(item);
            }
            else
            {
                var list = new List<object>();
                list.Add(item);
                resultDic.Add(arrayName, list);
            }
        }

        /// <summary>
        /// 尝试加一个空数组
        /// </summary>
        /// <param name="arrayName"></param>
        public void AddArray(string arrayName)
        {
            if (!resultDic.ContainsKey(arrayName))
            {
                var list = new List<object>();
                resultDic.Add(arrayName, list);
            }
        }

        /// <summary>
        /// 合并JsonString，如果有重复的key就覆盖
        /// </summary>
        /// <param name="js"></param>
        public void Merge(JsonString js)
        {
            var dic = js.ToDic();
            foreach (string key in dic.Keys)
            {
                this.Set(key, dic[key]);
            }
        }

        public bool ContainsKey(string key)
        {
            return resultDic.ContainsKey(key);
        }

        /// <summary>
        /// 如果存在则移除，如果不存在则不做任何事情
        /// </summary>
        /// <param name="key"></param>
        public void RemoveKey(string key)
        {
            if (resultDic.ContainsKey(key))
            {
                resultDic.Remove(key);
            }
        }

        /// <summary>
        /// 如果Set(key,val)设置了不可序列化的对象进来，则这个接口可能抛异常
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            try
            {
                return JsonConvert.SerializeObject(resultDic);
            }
            catch (Exception ex)
            {
                string ss = ex.ToString();
                throw;
            }

        }

        public string ToString(int MaxJsonLength = 0)
        {
            try
            {
                return JsonConvert.SerializeObject(resultDic);
            }
            catch (Exception ex)
            {
                string ss = ex.ToString();
                throw;
            }
        }

        /// <summary>
        /// ToString函数的不抛异常版本
        /// </summary>
        /// <returns></returns>
        public string ToStringNoThrow()
        {
            try
            {
                return this.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        public Dictionary<string, object> ToDic()
        {
            return resultDic;
        }

        private static Dictionary<string, object> ParseJson(string input)
        {
            Dictionary<string, object> sData = JsonConvert.DeserializeObject<Dictionary<string, object>>(input);
            return sData;
        }

        public static bool TryParse(string json, out JsonString js)
        {
            js = new JsonString();

            try
            {
                var ay = ParseJson(json);
                foreach (var item in ay)
                {
                    js.Set(item.Key, item.Value);
                }
                return true;
            }
            catch
            {
                js = null;
                return false;
            }
        }

        /// <summary>
        /// 根据key获取value
        /// 如果key不存在则返回null
        /// 如果value为null也返回null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            if (resultDic.ContainsKey(key))
            {
                return resultDic[key] == null ? null : resultDic[key].ToString();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据key获取value，失败抛异常
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetInt(string key)
        {
            int val = int.Parse(Get(key));
            return val;
        }

        /// <summary>
        /// 这个接口不会抛出异常。有任何异常（不存在或格式不对等）则返回默认值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int GetIntNoThrow(string key, int defVal = 0)
        {
            try
            {
                return GetInt(key);
            }
            catch
            {
                return defVal;
            }
        }

        /// <summary>
        /// 根据key获取value，失败抛异常
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long GetLong(string key)
        {
            long val = long.Parse(Get(key));
            return val;
        }

        /// <summary>
        /// 根据key获取value，失败抛异常
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public double GetDouble(string key)
        {
            double val = double.Parse(Get(key));
            return val;
        }
        public decimal GetDecimal(string key)
        {
            decimal val = decimal.Parse(Get(key));
            return val;
        }
        /// <summary>
        /// 根据key获取value，失败抛异常
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public float GetFloat(string key)
        {
            float val = float.Parse(Get(key));
            return val;
        }

        /// <summary>
        /// 根据key获取value，失败抛异常
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public DateTime GetDatetime(string key)
        {
            DateTime val = DateTime.Parse(Get(key));
            return val;
        }

        /// <summary>
        /// 根据key获取一个数组，失败抛异常
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetDic(string key)
        {
            return (Dictionary<string, object>)((JObject)GetObject(key)).ToDictionary();
        }

        /// <summary>
        /// 根据key获取一个JsonString，失败抛异常。对应 Set(string key, JsonString val)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public JsonString GetJson(string key)
        {
            var dic = GetDic(key);
            return new JsonString(dic);
        }

        public object GetObject(string key)
        {
            if (resultDic.ContainsKey(key))
            {
                return resultDic[key];
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 对应AddArrayItem()。key不存在返回长度为零的array
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public JsonString[] GetArray(string key)
        {
            JArray items = (JArray)GetObject(key);
            if (items == null)
            {
                return new JsonString[0];
            }
            if (items.Count == 0)
            {
                return new JsonString[0];
            }
            JsonString[] ret = new JsonString[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                Dictionary<string, object> dic = items.ElementAt(i).ToObject<Dictionary<string, object>>();
                ret[i] = new JsonString(dic);
            }
            return ret;
        }

        /// <summary>
        /// 遍历obj中的公共属性，序列化成json字符串
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string JsonSerialize(object obj)
        {
            var ss = JsonConvert.SerializeObject(obj);
            return ss;
        }

        public static T JsonDeserialize<T>(string json)
        {
            T ret = JsonConvert.DeserializeObject<T>(json);
            return ret;
        }
        public static object JsonDeserialize(string json)
        {
            var ret = JsonConvert.DeserializeObject(json);
            return ret;
        }

        public static void Test()
        {

            JsonString jsonString = new JsonString();
            jsonString.Set("status", "error");
            jsonString.Set("delegateid", 1);
            jsonString.Set("info", "低于跌停板");

            //array[]
            List<object> dataList = new List<object>();

            JsonString json1 = new JsonString();
            json1.Set("time", 20141022130900);
            json1.Set("price", 12.2);
            json1.Set("info", "test1");
            dataList.Add(json1.ToDic());

            JsonString json2 = new JsonString();
            json2.Set("time", 20141022131900);
            json2.Set("price", 10.2);
            json2.Set("info", "test2");
            dataList.Add(json2.ToDic());

            jsonString.Set("data", dataList);

            string json = jsonString.ToString();
            //{"status":"error","delegateid":1,"info":"低于跌停板","data":[{"time":20141022130900,"price":12.2,"info":"test1"},{"time":20141022131900,"price":10.2,"info":"test2"}]}
        }
    }

}
