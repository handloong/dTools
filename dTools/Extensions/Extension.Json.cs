using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace dTools
{
    /// <summary>
    /// Json操作
    /// </summary>
    public static class ExtensionJson
    {
        #region ToJson

        /// <summary>
        /// 对象序列化为json字符串
        /// </summary>
        /// <param name="obj">待序列化的对象</param>
        /// <returns>string</returns>
        public static string ToJson(this object obj)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }

        /// <summary>
        /// 对象序列化为json字符串
        /// </summary>
        /// <param name="obj">待序列化的对象</param>
        /// <param name="dateTimeFormat">日期格式化格式</param>
        /// <returns>string</returns>
        public static string ToJson(this object obj, string dateTimeFormat)
        {
            if (!string.IsNullOrEmpty(dateTimeFormat))
            {
                var timeConverter = new IsoDateTimeConverter { DateTimeFormat = dateTimeFormat };
                return JsonConvert.SerializeObject(obj, timeConverter);
            }
            else
            {
                return obj.ToJson();
            }
        }

        /// <summary>
        /// 对象序列化为json字符串
        /// </summary>
        /// <param name="obj">待序列化的对象</param>
        /// <param name="camelCase">是否驼峰</param>
        /// <param name="indented">是否缩进</param>
        /// <param name="nullValueHandling">空值处理</param>
        /// <param name="converter">json转换，如：new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" }</param>
        /// <returns>string</returns>
        public static string ToJson(this object obj, bool camelCase, bool indented = false, NullValueHandling nullValueHandling = NullValueHandling.Include, JsonConverter converter = null)
        {
            var options = new JsonSerializerSettings();
            if (camelCase)
            {
                options.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }
            if (indented)
            {
                options.Formatting = Formatting.Indented;
            }
            options.NullValueHandling = nullValueHandling;
            if (converter != null)
            {
                options.Converters?.Add(converter);
            }
            return JsonConvert.SerializeObject(obj, options);
        }

        #endregion ToJson

        #region ToObject

        /// <summary>
        /// json字符串反序列化为T类型对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Json"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string Json)
        {
            try
            {
                return Json == null ? default : JsonConvert.DeserializeObject<T>(Json);
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// json字符串反序列化为object类型对象
        /// </summary>
        /// <param name="Json"></param>
        /// <returns></returns>
        public static object ToObject(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject(Json);
        }

        #endregion ToObject

        #region ToList

        /// <summary>
        /// json字符串反序列化为List&lt;T&gt;类型字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Json"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject<List<T>>(Json);
        }

        #endregion ToList

        #region ToJObject

        /// <summary>
        /// json字符串反序列化为JObject
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static JObject ToJObject(this string json)
        {
            return json == null ? JObject.Parse("{}") : JObject.Parse(json.Replace("&nbsp;", ""));
        }

        #endregion ToJObject

        #region ToJArray

        /// <summary>
        /// json字符串反序列化为JArray
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static JArray ToJArray(this string json)
        {
            return json == null ? JArray.Parse("[]") : JArray.Parse(json.Replace("&nbsp;", ""));
        }

        #endregion ToJArray

        #region IsJsonObjectString

        /// <summary>
        /// 判断指定字符串是否对象类型的Json字符串格式
        /// </summary>
        /// <param name="this">json字符串</param>
        /// <returns>bool</returns>
        public static bool IsJsonObjectString(this string @this)
        {
            return @this != null && @this.StartsWith("{") && @this.EndsWith("}");
        }

        #endregion IsJsonObjectString

        #region IsJsonArrayString

        /// <summary>
        /// 判断指定字符串是否集合类型的Json字符串格式
        /// </summary>
        /// <param name="this">json字符串</param>
        /// <returns>bool</returns>
        public static bool IsJsonArrayString(this string @this)
        {
            return @this != null && @this.StartsWith("[") && @this.EndsWith("]");
        }

        #endregion IsJsonArrayString

        #region IsJsonString

        /// <summary>
        /// 验证字符串是否是Json字符串
        /// </summary>
        /// <param name="this">json字符串</param>
        /// <returns>bool</returns>
        public static bool IsJsonString(this string @this)
        {
            try
            {
                if (@this.IsJsonObjectString() && @this.ToJObject() != null)
                    return true;
                if (@this.IsJsonArrayString() && @this.ToJArray() != null)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion IsJsonString

        /// <summary>
        /// 日期格式化
        /// </summary>
        public class DateFormat : IsoDateTimeConverter
        {
            /// <summary>
            /// 
            /// </summary>
            public DateFormat()
            {
                base.DateTimeFormat = "yyyy-MM-dd";
            }
        }

        /// <summary>
        /// 时间格式化
        /// </summary>
        public class DateTimeFormat : IsoDateTimeConverter
        {
            /// <summary>
            /// 
            /// </summary>
            public DateTimeFormat()
            {
                base.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            }
        }
    }
}
