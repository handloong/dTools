//using System;
//using System.Collections.Generic;
//using System.Text;
//using RestSharp;
//namespace dTools
//{
//    /// <summary>
//    /// Post帮助类
//    /// </summary>
//    public class PostHelper
//    {
//        /// <summary>
//        /// 执行Post
//        /// </summary>
//        /// <typeparam name="T">除了泛型,另外支持string,不支持其他int double等</typeparam>
//        /// <param name="url"></param>
//        /// <param name="contentType"></param>
//        /// <param name="headers"></param>
//        /// <param name="parameters"></param>
//        /// <param name="timeout"></param>
//        /// <returns></returns>
//        public static T Post<T>(string url,
//            ContentTypeEnum contentType,
//            Dictionary<string, string> headers = null,
//            Dictionary<string, string> parameters = null,
//            int timeout = -1
//            )
//        {
//            var client = new RestClient(url)
//            {
//                Timeout = timeout
//            };
//            var request = new RestRequest(Method.POST);
//            switch (contentType)
//            {
//                case ContentTypeEnum.x_www_form_urlencoded:
//                    request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
//                    break;
//                case ContentTypeEnum.Json:
//                    request.AddHeader("Content-Type", "application/json");
//                    break;
//                default:
//                    break;
//            }
//            if (headers != null)
//            {
//                foreach (KeyValuePair<string, string> kvp in headers)
//                {
//                    if (kvp.Key != "Content-Type")
//                    {
//                        request.AddHeader(kvp.Key, kvp.Value);
//                    }
//                }
//            }
//            if (parameters != null)
//            {
//                foreach (KeyValuePair<string, string> kvp in parameters)
//                {
//                    request.AddParameter(kvp.Key, kvp.Value);
//                }
//            }
//            try
//            {
//                IRestResponse response = client.Execute(request);
//                var json = response.Content;
//                if (!string.IsNullOrEmpty(json))
//                {
//                    if (typeof(T) == typeof(string))
//                    {
//                        T ret = (T)Convert.ChangeType(json, typeof(T));
//                        return ret;
//                    }
//                    return json.ToObject<T>();
//                }
//                return default;
//            }
//            catch (Exception)
//            {
//                return default;
//            }
//        }

//        /// <summary>
//        /// 执行Get请求
//        /// </summary>
//        /// <typeparam name="T">除了泛型,另外支持string,不支持其他int double等</typeparam>
//        /// <param name="url"></param>
//        /// <param name="headers"></param>
//        /// <param name="timeout"></param>
//        /// <returns></returns>
//        public static T Get<T>(string url, Dictionary<string, string> headers = null, int timeout = -1)
//        {
//            var client = new RestClient(url)
//            {
//                Timeout = timeout
//            };
//            var request = new RestRequest(Method.GET);
//            if (headers != null)
//            {
//                foreach (KeyValuePair<string, string> kvp in headers)
//                {
//                    request.AddHeader(kvp.Key, kvp.Value);
//                }
//            }
//            try
//            {
//                IRestResponse response = client.Execute(request);
//                var json = response.Content;
//                if (!string.IsNullOrEmpty(json))
//                {
//                    if (typeof(T) == typeof(string))
//                    {
//                        T ret = (T)Convert.ChangeType(json, typeof(T));
//                        return ret;
//                    }
//                    return json.ToObject<T>();
//                }
//                return default;
//            }
//            catch (Exception)
//            {
//                return default;
//            }
//        }
//    }

//    /// <summary>
//    /// 常用ContentType枚举
//    /// </summary>
//    public enum ContentTypeEnum
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        x_www_form_urlencoded,
//        /// <summary>
//        /// 
//        /// </summary>
//        Json
//    }
//}
