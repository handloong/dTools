using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dTools
{
    /// <summary>
    /// 窗体信息实体
    /// </summary>
    public class WindowInfoEntity
    {
        /// <summary>
        /// 窗口句柄
        /// </summary>
        public IntPtr hWnd { get; set; }
        /// <summary>
        /// 窗体名
        /// </summary>
        public string szWindowName { get; set; }
        /// <summary>
        /// 窗口类名 
        /// </summary>
        public string szClassName { get; set; }

    }
}
