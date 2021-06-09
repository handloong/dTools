using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dTools
{
    /// <summary>
    /// CSV解析帮助类
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class CSVColumnAttribute : Attribute
    {
        public CSVColumnAttribute(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                this.Name = name;
            }
        }
        /// <summary>
        /// 映射列名 - 给列用
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// CSV实体行标记类
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class CSVHeaderAttribute : Attribute
    {
        /// <summary>
        /// CSV实体行标记类,默认头部行在第0行
        /// </summary>
        /// <param name="whereHeaderLine"></param>
        public CSVHeaderAttribute(int whereHeaderLine)
        {
            this.WhereHeaderLine = whereHeaderLine;
        }
        /// <summary>
        /// 从第几开始 - 给实体用
        /// </summary>
        public int WhereHeaderLine { get; set; }
    }
}
