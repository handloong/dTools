using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace dTools
{
    /// <summary>
    /// 简单INI帮助类,更多操作请参考INIHelper
    /// </summary>
    public static class EasyINI
    {
        #region 读写INI文件相关
        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString", CharSet = CharSet.Ansi)]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString", CharSet = CharSet.Ansi)]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileInt(string lpApplicationName, string lpKeyName, int nDefault, string lpFileName);


        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileSectionNames", CharSet = CharSet.Ansi)]
        private static extern int GetPrivateProfileSectionNames(IntPtr lpszReturnBuffer, int nSize, string filePath);

        [DllImport("KERNEL32.DLL ", EntryPoint = "GetPrivateProfileSection", CharSet = CharSet.Ansi)]
        private static extern int GetPrivateProfileSection(string lpAppName, byte[] lpReturnedString, int nSize, string filePath);
        #endregion

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public static string _INIPath;
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释

        static EasyINI()
        {
            if (string.IsNullOrEmpty(_INIPath))
            {
                _INIPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EasyINISystem.INI");
            }
        }
        /// <summary>
        /// 初始化INI文件,当调用Write Read自动生成INI文件。
        /// </summary>
        /// <param name="INIPath"></param>
        public static void SetPath(string INIPath)
        {
            _INIPath = INIPath;
        }

        #region 读写操作（字符串）
        /// <summary>
        /// 向INI写入数据,默认INI文件为程序根目录下的EasyINISystem.INI,可以调用EasyINI.SetPath设置自定义地址
        /// </summary>
        /// <PARAM name="Key">键名</PARAM>
        /// <PARAM name="Value">值</PARAM>
        /// <PARAM name="Section">节点名</PARAM>
        public static void Write<T>(string Key, T Value, string Section = "SYSTEM")
        {
            WritePrivateProfileString(Section, Key, Value.ToString(), _INIPath);
        }
        /// <summary>
        /// 读取INI数据,默认INI文件为程序根目录下的EasyINISystem.INI,可以调用EasyINI.SetPath设置自定义地址
        /// </summary>
        /// <PARAM name="Key">键名</PARAM>
        /// <PARAM name="defultValue">默认值</PARAM>
        /// <PARAM name="Section">节点名</PARAM>
        /// <returns>值（字符串）</returns>
        public static T Read<T>(string Key, T defultValue, string Section = "SYSTEM")
        {
            StringBuilder temp = new StringBuilder();
            GetPrivateProfileString(Section, Key, defultValue.ToString(), temp, 255, _INIPath);
            return (T)Convert.ChangeType(temp.ToString(), typeof(T));
        }
        #endregion
    }
}
