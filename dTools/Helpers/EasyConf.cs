using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dTools
{
    public static class EasyConf
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public static string _conf;
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释

        static EasyConf()
        {
            if (string.IsNullOrEmpty(_conf))
            {
                _conf = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EasyConf.conf");
            }
        }

        private static void WriteDefalut()
        {
            if (!File.Exists(_conf))
            {
                File.WriteAllLines(_conf, new string[] {
                        $"### Create by dTools.EasyConf {DateTime.Now:yyyyMMdd HH:mm:ss}",
                    }, Encoding.UTF8);
            }
        }

        /// <summary>
        /// 初始化Conf文件,自动生成
        /// </summary>
        /// <param name="confPath"></param>
        public static void SetPath(string confPath)
        {
            _conf = confPath;
            WriteDefalut();
        }

        public static T Read<T>()
        {
            WriteDefalut(); 

            var lines = File.ReadAllLines(_conf, Encoding.UTF8)
                .ToList()
                .Where(x => !x.StartsWith("#") && x.Contains("="))
                .Select(x => new { Key = x.Split('=')[0].Trim(), Value = x.Split('=')[1].Trim() })
                .ToList();

            var type = typeof(T);
            var obj = Activator.CreateInstance(type);

            var properties = type.GetProperties();
            foreach (var item in properties)
            {
                var line = lines.FirstOrDefault(x => x.Key == item.Name);
                if (line != null)
                {
                    object value = Convert.ChangeType(line.Value, item.PropertyType);
                    item.SetValue(obj, value);
                }
            }
            return (T)obj;
        }

        public static bool Write<T>(T value)
        {
            try
            {
                var allLines = File.ReadAllLines(_conf, Encoding.UTF8);

                var lines = allLines
                            .Where(x => !x.StartsWith("#") && x.Contains("="))
                            .Select(x => new LineInfo
                            {
                                Key = x.Split('=')[0].Trim(),
                                Value = x.Split('=')[1].Trim(),
                                Original = x
                            })
                            .ToList();

                var type = typeof(T);
                var properties = type.GetProperties();


                foreach (var item in properties)
                {
                    var newValue = item.GetValue(value);
                    var oldLine = lines.FirstOrDefault(x => x.Key == item.Name);

                    if (oldLine != null)
                    {
                        object objectValue = Convert.ChangeType(oldLine.Value, item.PropertyType);

                        //发生变动
                        if (!newValue.Equals(objectValue))
                        {

                            for (int i = 0; i < allLines.Length; i++)
                            {
                                if (allLines[i] == oldLine.Original)
                                {
                                    allLines[i] = $"{item.Name} = {newValue}";
                                }
                            }
                        }
                    }
                    else
                    {
                        //新增数据
                        allLines = allLines.Concat(new List<string> { $"{item.Name} = {newValue}" }).ToArray();
                    }
                }

                File.WriteAllLines(_conf, allLines, Encoding.UTF8);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private class LineInfo
        {
            public string Key { get; set; }
            public string Value { get; set; }
            public string Original { get; set; }
        }
    }
}
