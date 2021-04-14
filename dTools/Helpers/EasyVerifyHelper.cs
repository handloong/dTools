using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dTools
{
    /// <summary>
    /// 验证帮助类
    /// </summary>
    public class EasyVerifyHelper
    {
        /// <summary>
        /// T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public static void Trythrow<T>(T data)
        {
            foreach (var property in data.GetType().GetProperties())
            {
                var attribute = property.GetCustomAttributes(typeof(DSVAttribute), false).FirstOrDefault();
                if (attribute != null)
                {
                    var attr = ((DSVAttribute)attribute);
                    var value = property.GetValue(data);
                    if (attr != null && !attr.IsValid(value))
                    {
                        throw new Exception($"{property.Name} Check Error.value:{value}");
                    }
                }
            }
        }
    }

    /// <summary>
    /// 验证帮助类 DToolsVerify
    /// </summary>
    public class DSVAttribute : Attribute
    {
        /// <summary>
        /// 不能为空 (字符串或者对象)
        /// </summary>
        public bool NotNull { get; set; }

        public bool IsValid(object data)
        {
            if (data.IsEmpty() || data.IsNull())
            {
                return false;
            }
            return true;
        }
    }
}
