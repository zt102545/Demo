using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    /// <summary>
    /// 扩展类
    /// </summary>
    public static class Extensions
    {
        public static short TryParseByInt16(this object obj, short defaultValue = 0)
        {
            short temp = defaultValue;
            if (obj == null)
            {
                return temp;
            }

            return short.TryParse(obj.ToString(), out temp) ? temp : defaultValue;
        }
        public static int TryParseByInt(this object obj, int defaultValue = 0)
        {
            int temp = defaultValue;
            if (obj == null)
            {
                return temp;
            }

            return int.TryParse(obj.ToString(), out temp) ? temp : defaultValue;
        }
        public static string TryParseByString(this object obj, string defaultValue = "")
        {
            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                return obj.ToString();
            }
        }

        public static int TryParseByIntMax(this object obj)
        {
            int temp = int.MaxValue;
            if (obj == null)
            {
                return temp;
            }

            return int.TryParse(obj.ToString(), out temp) ? temp : int.MaxValue;
        }
        public static short TryParseByShort(this object obj, short defaultValue = 0)
        {
            short temp = defaultValue;
            if (obj == null)
            {
                return temp;
            }

            return short.TryParse(obj.ToString(), out temp) ? temp : defaultValue;
        }
        public static long TryParseByLong(this object obj, long defaultValue = 0)
        {
            long temp = defaultValue;
            if (obj == null)
            {
                return temp;
            }

            return long.TryParse(obj.ToString(), out temp) ? temp : defaultValue;
        }

        public static uint TryParseByUInt32(this object obj, uint defaultValue = 0)
        {
            uint temp = defaultValue;
            if (obj == null)
            {
                return temp;
            }

            uint.TryParse(obj.ToString(), out temp);
            return uint.TryParse(obj.ToString(), out temp) ? temp : defaultValue;
        }
        public static ushort TryParseByUInt16(this object obj, ushort defaultValue = 0)
        {
            ushort temp = defaultValue;
            if (obj == null)
            {
                return temp;
            }

            return ushort.TryParse(obj.ToString(), out temp) ? temp : defaultValue;
        }
        public static ulong TryParseByUlong(this object obj, ulong defaultValue = 0)
        {
            ulong temp = defaultValue;
            if (obj == null)
            {
                return temp;
            }

            return ulong.TryParse(obj.ToString(), out temp) ? temp : defaultValue;
        }

        public static decimal TryParseByDecimal(this object obj, decimal defaultValue = 0)
        {
            decimal temp = defaultValue;
            if (obj == null)
            {
                return temp;
            }

            string _decimalContent = obj.ToString();
            if (_decimalContent.Contains("E"))
            {
                try
                {
                    return Convert.ToDecimal(decimal.Parse(_decimalContent, System.Globalization.NumberStyles.Float));
                }
                catch
                {

                }
                return defaultValue;
            }
            else
            {
                return decimal.TryParse(obj.ToString(), out temp) ? temp : defaultValue;
            }
        }

        public static decimal TryParseByDecimal(this object obj, int precision)
        {
            return Math.Round(TryParseByDecimal(obj, 0m), precision);
        }

        public static float TryParseByFloat(this object obj, float defaultValue = 0)
        {
            float temp = defaultValue;
            if (obj == null)
            {
                return temp;
            }

            return float.TryParse(obj.ToString(), out temp) ? temp : defaultValue;
        }
        public static double TryParseByDouble(this object obj, double defaultValue = 0)
        {
            double temp = defaultValue;
            if (obj == null)
            {
                return temp;
            }

            string _doubleContent = obj.ToString();
            if (_doubleContent.Contains("E"))
            {
                try
                {
                    return Convert.ToDouble(double.Parse(_doubleContent, System.Globalization.NumberStyles.Float));
                }
                catch
                {

                }
                return defaultValue;
            }
            else
            {
                return double.TryParse(obj.ToString(), out temp) ? temp : defaultValue;
            }



        }

        /// <summary>
        /// obj.ToString之后DateTime.TryParse
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime TryParseByDateTime(this object obj)
        {
            DateTime temp = DateTime.MinValue;
            if (obj == null)
            {
                return temp;
            }

            if (DateTime.TryParse(obj.ToString(), out temp))
            {
                return Convert.ToDateTime(obj);
            }
            return temp;
        }

        /// <summary>
        /// obj.ToString之后DateTime.TryParse带格式
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime TryParseExactByDateTime(this object obj, string format)
        {
            DateTime temp = DateTime.MinValue;
            if (obj == null)
            {
                return temp;
            }

            try
            {
                return DateTime.ParseExact(obj.ToString(), format, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch { }
            return temp;
        }
        public static bool TryParseByBool(this object obj, bool defaultValue = false)
        {
            bool temp = defaultValue;
            if (obj == null)
            {
                return temp;
            }

            return bool.TryParse(obj.ToString(), out temp) ? temp : defaultValue;
        }

        #region date

        public static DateTime ParseMilliBeijingDateTimes(this Int64 timesTamp)
        {
            return new DateTime(1970, 1, 1, 8, 0, 0).AddMilliseconds(timesTamp);
        }

        /// <summary>
        /// utc时间
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static Int64 ParseMilliTimesTamp(this DateTime datetime)
        {
            DateTime utcDate = new DateTime(1970, 1, 1, 0, 0, 0);
            return (Int64)(datetime - utcDate).TotalMilliseconds;
        }

        #endregion

        /// <summary>
        /// 去掉小数点后的0，for decimal(38,20)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string ToStringDrop0(this decimal val)
        {
            return val.ToString("0.####################");
        }

        /// <summary>
        /// decimal 转换成%
        /// </summary>
        /// <param name="val"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static string ToStringPercent(this decimal val, int decimals = 2)
        {
            return $"{Math.Round(val * 100, 2)}%";
        }

        public static IDictionary<string, object> ToDictionary(this JObject @object)
        {
            var result = @object.ToObject<Dictionary<string, object>>();

            var JObjectKeys = (from r in result
                               let key = r.Key
                               let value = r.Value
                               where value.GetType() == typeof(JObject)
                               select key).ToList();

            var JArrayKeys = (from r in result
                              let key = r.Key
                              let value = r.Value
                              where value.GetType() == typeof(JArray)
                              select key).ToList();

            JArrayKeys.ForEach(key => result[key] = ((JArray)result[key]).Values().Select(x => ((JValue)x).Value).ToArray());
            JObjectKeys.ForEach(key => result[key] = ToDictionary(result[key] as JObject));

            return result;
        }

    }

}
