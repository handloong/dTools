using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dTools
{
    public class CSVHelper
    {
        /// <summary>
        /// 解析CSV
        /// </summary>
        /// <typeparam name="T">单个T</typeparam>
        /// <param name="allLines">所有行,包含头部</param>
        /// <param name="split">默认,分割</param>
        /// <returns>集合T</returns>
        public static List<T> Paser<T>(string[] allLines, char split = ',')
        {
            List<T> retval = new List<T>();

            var type = typeof(T);
            var beginLine = GetBeginLine(type);
            //数据行
            var dataLines = allLines.Skip(beginLine + 1).ToList();

            //除了 datetime 都是 string

            var mapping = GetMapping(type, allLines[beginLine], split);

            for (int i = 0; i < dataLines.Count; i++)
            {
                var v = dataLines[i].Split(split);
                var obj = Activator.CreateInstance(type);
                var properties = type.GetProperties();
                foreach (var item in properties)
                {
                    var properName = item.Name.ToUpper();

                    //Mapping 包含 类属性
                    if (mapping.ContainsKey(properName))
                    {
                        object value = new object();
                        //Type
                        var vv = v[mapping[properName]];
                        if (item.PropertyType == typeof(DateTime))
                        {
                            value = vv.ToDate();
                        }
                        else if (item.PropertyType.FullName.StartsWith("System.Nullable`1[[System.DateTime"))
                        {
                            value = vv.ToDate();
                        }
                        else if (item.PropertyType == typeof(int))
                        {
                            value = vv.ToInt();
                        }
                        else if (item.PropertyType == typeof(double))
                        {
                            value = vv.ToDouble();
                        }
                        else if (item.PropertyType == typeof(decimal))
                        {
                            value = vv.ToDecimal();
                        }
                        else if (item.PropertyType == typeof(string))
                        {
                            value = Convert.ToString(vv);
                        }
                        else
                        {
                            value = Convert.ChangeType(vv, item.PropertyType);
                        }
                        item.SetValue(obj, value);
                    }
                }
                var t = (T)obj;
                retval.Add(t);
            }
            return retval;
        }

        /// <summary>
        /// 获取实体名 和 列名
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Dictionary<string, string> GetPropertyAndColumnName(Type type)
        {
            var retval = new Dictionary<string, string>();
            foreach (var property in type.GetProperties())
            {
                var propertyName = property.Name.ToUpper();
                var mappingName = propertyName;

                var attribute = property.GetCustomAttributes(typeof(CSVColumnAttribute), false).FirstOrDefault();
                if (attribute != null)
                {
                    var attr = ((CSVColumnAttribute)attribute);
                    mappingName = attr.Name.ToUpper();
                }
                if (!retval.ContainsKey(propertyName))
                    retval[propertyName] = mappingName;
            }

            return retval;
        }

        /// <summary>
        /// 从第几行开始解析
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static int GetBeginLine(Type type)
        {
            if (type.GetCustomAttributes(typeof(CSVHeaderAttribute), false).FirstOrDefault() is CSVHeaderAttribute cta)
            {
                return cta.WhereHeaderLine;
            }
            return 0;
        }

        /// <summary>
        /// 获取Emapping信息
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        private static Dictionary<string, int> GetMapping(Type type, string header, char split = ',')
        {
            var retval = new Dictionary<string, int>();
            var bondHeader = header.Split(split);
            var columns = GetPropertyAndColumnName(type);
            foreach (var c in columns)
            {
                retval[c.Key] = bondHeader.IndexOfStartWith(c.Value.ToUpper());
            }
            return retval;
        }
    }
}
