using System;
using System.ComponentModel;


namespace Lux.Util.Extension
{
    /// <summary>
    /// 枚举扩展类
    /// </summary>
    public static partial class ExtensionsEnum
    {
        /// <summary>
        /// 获取当前枚举的描述
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum @this)
        {
            var result = string.Empty;
            var enumType = @this.GetType();
            var fieldInfo = enumType.GetField(@this.ToString());
            if (Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute), false) is DescriptionAttribute attr)
            {
                result = attr.Description;
            }
            return result;
        }
    }
}