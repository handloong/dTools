using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Xml;

namespace dTools
{
    /// <summary>
    /// Config文件操作
    /// </summary>
    public class Config
    {
        /// <summary>
        /// AppSettings配置
        /// </summary>
        public static NameValueCollection AppSettings => ConfigurationManager.AppSettings;

        /// <summary>
        /// 根据Key取Value值
        /// </summary>
        /// <param name="key"></param>
        public static string GetValue(string key) => AppSettings[key]?.ToString().Trim();

        /// <summary>
        /// 根据Key取Value值
        /// </summary>
        /// <param name="key"></param>
        public static T GetValue<T>(string key)
        {
            var value = AppSettings[key]?.ToString().Trim();
            if (!string.IsNullOrEmpty(value))
            {
                return value.ToObject<T>();
            }
            return default;
        }

        /// <summary>
        /// 根据Key获取ConnectionString值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConnectionString(string key)
        {
            //public static string GetConnectionString(string key, bool useMapping = false)
            //if (useMapping)
            //{
            //    new JavaScriptSerializer().
            //        Deserialize<Dictionary<string, string>>(File.ReadAllText(GetPhysicalPath("~/XmlConfig/DBMapping.json")))
            //        .TryGetValue(key, out string dbConfig);

            //    if (string.IsNullOrEmpty(dbConfig))
            //        throw new Exception("您使用了Mapping 但是没有获取到DbConfig 配置.请检查Mapping文件。");
            //    else
            //        return dbConfig;
            //}
            return ConfigurationManager.ConnectionStrings[key]?.ConnectionString?.Trim() ?? GetValue(key);
        }
    }
}