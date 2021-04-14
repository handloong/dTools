namespace dTools
{
    using Microsoft.Win32;
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    /// <summary>
    /// winform帮助类
    /// </summary>
    public class WinformHelper
    {
        #region Winform自动启动
        /// <summary>  
        /// Winform自动启动
        /// </summary>  
        /// <param name="isAuto">true:开机启动,false:不开机自启</param>
        /// <param name="executablePath"></param>
        /// <param name="registryKey"></param> 
        public static void AutoStart(bool isAuto, string executablePath, string registryKey)
        {
            if (registryKey is null)
            {
                throw new ArgumentNullException(nameof(registryKey));
            }

            using (RegistryKey R_local = Registry.LocalMachine)//RegistryKey R_local = Registry.CurrentUser;
            {
                using (RegistryKey R_run = R_local.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run"))
                {
                    if (isAuto == true)
                        R_run.SetValue(registryKey, executablePath);
                    else
                        R_run.DeleteValue(registryKey, false);
                }
            }
        }
        #endregion

        #region 释放内存
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
        /// <summary>
        /// 释放内存
        /// </summary>
        public static void ClearMemory()
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
                }
            }
            catch
            {
            }
        }
        #endregion

        #region Invoke
        //public static Result Method(string dllName, string className, string methodName, object paramList)
        //{
        //    return Method(dllName, className, methodName, null, paramList);
        //}

        //public static Result Method(string dllName, string className, string methodName, string folderPath, object paramList)
        //{
        //    Result s = new Result();
        //    object[] parametersArray = new object[] { paramList };

        //    var dll = _getAssembly(dllName, null);
        //    var classNamespace = _getClassNamespace(dllName);
        //    Type classType = dll.GetType(_getClassName(classNamespace, className));

        //    object obj = CacheControl.Instance.GetObject(classType, dll);

        //    MethodInfo methodInfo = classType.GetMethod(methodName);
        //    if (paramList == null)
        //        s = (Result)methodInfo.Invoke(obj, null);
        //    else
        //        s = (Result)methodInfo.Invoke(obj, parametersArray);
        //    return s;
        //}

        /// <summary>
        /// Dynamic Call Winfrom
        /// </summary>
        /// <param name="dllName">Dll name</param>
        /// <param name="formName">Dll mainform</param>
        /// <returns></returns>
        public static object GetWinFormObj(string dllName, string formName)
        {
            return GetWinFormObj(dllName, formName, null);
        }

        /// <summary>
        ///反射获取Winform
        /// </summary>
        /// <param name="dllName"></param>
        /// <param name="formName"></param>
        /// <returns></returns>
        public static System.Windows.Forms.Form GetWinForm(string dllName, string formName)
        {
            return (System.Windows.Forms.Form)GetWinFormObj(dllName, formName, null);
        }
        /// <summary>
        /// 反射获取Winform
        /// </summary>
        /// <param name="dllName"></param>
        /// <param name="formName"></param>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static System.Windows.Forms.Form GetWinForm(string dllName, string formName, string folderPath)
        {
            return (System.Windows.Forms.Form)GetWinFormObj(dllName, formName, folderPath);
        }
        /// <summary>
        /// 反射获取Winform
        /// </summary>
        /// <param name="dllName"></param>
        /// <param name="formName"></param>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static object GetWinFormObj(string dllName, string formName, string folderPath)
        {
            var dll = GetAssembly(dllName, folderPath);
            var classNamespace = dllName.Replace(".dll", string.Empty).Replace(".DLL", string.Empty).Replace(".Dll", string.Empty);
            Type classType = dll.GetType($"{classNamespace}.{formName}");
            object classInstance = dll.CreateInstance(classType.FullName, true);
            return classInstance;
        }

        private static Assembly GetAssembly(string dllName, string folderPath)
        {
            //采用字节流转换.避免dll被占用
            string path;
            if (folderPath == null)
                path = string.Format(@"{0}\{1}", AppDomain.CurrentDomain.BaseDirectory, dllName);
            else
                path = string.Format(@"{0}\{1}\{2}", AppDomain.CurrentDomain.BaseDirectory, folderPath, dllName);
            return Assembly.Load(File.ReadAllBytes(path));
        }
        #endregion

        #region 获取Exe的图标
        [System.Runtime.InteropServices.DllImport("shell32.dll", EntryPoint = "ExtractAssociatedIcon")]
        private static extern IntPtr ExtractAssociatedIconA(
        IntPtr hInst,
        [MarshalAs(
                UnmanagedType.LPStr)] string lpIconPath,
        ref int lpiIcon);

        [DllImport("shell32.dll", EntryPoint = "ExtractIcon")]
        private static extern IntPtr ExtractIconA(
            IntPtr hInst,
            [System.Runtime.InteropServices.MarshalAs(
                System.Runtime.InteropServices.UnmanagedType.LPStr)] string lpszExeFileName,
            int nIconIndex);


        private static IntPtr hInst;
        /// <summary>
        /// 抽取图标
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static System.Drawing.Icon ExtractIcon(string fileName, int index)
        {
            if (System.IO.File.Exists(fileName) || System.IO.Directory.Exists(fileName))
            {
                System.IntPtr hIcon;

                // 文件所含图标的总数
                hIcon = ExtractIconA(hInst, fileName, -1);

                // 没取到的时候
                if (hIcon.Equals(IntPtr.Zero))
                {
                    // 取得跟文件相关的图标
                    return ExtractAssociatedIcon(fileName);
                }
                else
                {
                    // 图标的总数
                    int numOfIcons = hIcon.ToInt32();

                    if (0 <= index && index < numOfIcons)
                    {
                        hIcon = ExtractIconA(hInst, fileName, index);

                        if (!hIcon.Equals(IntPtr.Zero))
                        {
                            return System.Drawing.Icon.FromHandle(hIcon);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 抽取文件图标
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static System.Drawing.Icon ExtractAssociatedIcon(string fileName)
        {
            if (System.IO.File.Exists(fileName) || System.IO.Directory.Exists(fileName))
            {
                int i = 0;
                IntPtr hIcon = ExtractAssociatedIconA(hInst, fileName, ref i);
                if (!hIcon.Equals(IntPtr.Zero))
                {
                    return System.Drawing.Icon.FromHandle(hIcon);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 注册热键
        //如果函数执行成功，返回值不为0。
        //如果函数执行失败，返回值为0。要得到扩展错误信息，调用GetLastError。
        /// <summary>
        /// 注册热键
        /// </summary>
        /// <param name="hWnd">Handle</param>
        /// <param name="id">唯一数字即可,例如:100</param>
        /// <param name="fsModifiers">KeyModifiers.Ctrl</param>
        /// <param name="vk">Keys.S</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(
            IntPtr hWnd,                //要定义热键的窗口的句柄
            int id,                     //定义热键ID（不能与其它ID重复）           
            KeyModifiers fsModifiers,   //标识热键是否在按Alt、Ctrl、Shift、Windows等键时才会生效
            Keys vk                     //定义热键的内容
            );
        /// <summary>
        /// 取消热键
        /// </summary>
        /// <param name="hWnd">Handle</param>
        /// <param name="id">注册热键的Id</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(
            IntPtr hWnd,                //要取消热键的窗口的句柄
            int id                      //要取消热键的ID
            );

        /// <summary>
        /// 定义了辅助键的名称（将数字转变为字符以便于记忆，也可去除此枚举而直接使用数值）
        /// </summary>
        [Flags()]
        public enum KeyModifiers
        {
            /// <summary>
            /// 
            /// </summary>
            None = 0,
            /// <summary>
            /// 
            /// </summary>
            Alt = 1,
            /// <summary>
            /// 
            /// </summary>
            Ctrl = 2,
            /// <summary>
            /// 
            /// </summary>
            Shift = 4,
            /// <summary>
            /// 
            /// </summary>
            WindowsKey = 8
        }
        #endregion

        #region  减少闪烁 代码注释
        //public FrmBbx()
        //{
        //    InitializeComponent();
        //    SetStyles();
        //}
        ////减少闪烁
        //private void SetStyles()
        //{
        //    SetStyle(
        //      ControlStyles.UserPaint |
        //      ControlStyles.AllPaintingInWmPaint |
        //      ControlStyles.OptimizedDoubleBuffer |
        //      ControlStyles.ResizeRedraw |
        //      ControlStyles.DoubleBuffer, true);
        //    UpdateStyles();
        //    AutoScaleMode = AutoScaleMode.None;
        //}
        #endregion

    }
}
