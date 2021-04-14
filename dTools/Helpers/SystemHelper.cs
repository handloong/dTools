using Microsoft.Win32;
using NetFwTypeLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;

namespace dTools
{
    /// <summary>
    /// 系统帮助类
    /// </summary>
    public class SystemHelper
    {
        #region 属性:当前执行程序的路径
        /// <summary>
        /// 当前执行程序的路径
        /// </summary>
        public static string ExcutePath => AppDomain.CurrentDomain.BaseDirectory;
        #endregion

        #region 方法:电脑蓝屏
        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);

        /// <summary>
        /// 设置电脑蓝屏,管理员权限有效
        /// </summary>
        /// <returns></returns>
        public static bool SetBsod()
        {
            try
            {
                int isCritical = 1;
                int BreakOnTermination = 0x1D;
                Process.EnterDebugMode();
                NtSetInformationProcess(Process.GetCurrentProcess().Handle, BreakOnTermination, ref isCritical, sizeof(int));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region 属性:判断程序是否拥有管理员权限
        /// <summary>
        /// /// <summary>
        /// 判断程序是否拥有管理员权限
        /// </summary>
        /// <returns>true:是管理员；false:不是管理员</returns>
        /// </summary>
        public static bool IsAdministrator
        {
            get
            {
                try
                {
                    WindowsIdentity current = WindowsIdentity.GetCurrent();
                    WindowsPrincipal windowsPrincipal = new WindowsPrincipal(current);
                    return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        #endregion

        #region 方法:通过对象防火墙操作
        /// <summary>
        /// 防火墙操作
        /// </summary>
        /// <param name="isOpenDomain">域网络防火墙（禁用：false；启用（默认）：true）</param>
        /// <param name="isOpenPublicState">公共网络防火墙（禁用：false；启用（默认）：true）</param>
        /// <param name="isOpenStandard">专用网络防火墙（禁用: false；启用（默认）：true）</param>
        /// <returns></returns>
        public static bool FirewallOpen(bool isOpenDomain = true, bool isOpenPublicState = true, bool isOpenStandard = true)
        {
            try
            {
                if (!IsAdministrator)
                {
                    throw new Exception("操作防火墙必须已管理员身份运行。");
                }
                INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                // 启用<高级安全Windows防火墙> - 专有配置文件的防火墙
                firewallPolicy.set_FirewallEnabled(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PRIVATE, isOpenStandard);
                // 启用<高级安全Windows防火墙> - 公用配置文件的防火墙
                firewallPolicy.set_FirewallEnabled(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PUBLIC, isOpenPublicState);
                // 启用<高级安全Windows防火墙> - 域配置文件的防火墙
                firewallPolicy.set_FirewallEnabled(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_DOMAIN, isOpenDomain);
            }
            catch (Exception e)
            {
                string error = $"防火墙修改出错：{e.Message}";
                throw new Exception(error);
            }
            return true;
        }

        /// <summary>
        /// 添加防火墙入栈端例外端口
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="port">端口</param>
        /// <param name="protocol">协议(TCP、UDP)</param>
        public static void FirewallAddPorts(string name, int port, string protocol)
        {
            //创建firewall管理类的实例
            INetFwMgr netFwMgr = (INetFwMgr)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwMgr"));

            INetFwOpenPort objPort = (INetFwOpenPort)Activator.CreateInstance(
                Type.GetTypeFromProgID("HNetCfg.FwOpenPort"));

            objPort.Name = name;
            objPort.Port = port;
            if (protocol.ToUpper() == "TCP")
            {
                objPort.Protocol = NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP;
            }
            else
            {
                objPort.Protocol = NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP;
            }
            objPort.Scope = NET_FW_SCOPE_.NET_FW_SCOPE_ALL;
            objPort.Enabled = true;

            bool exist = false;
            //加入到防火墙的管理策略
            foreach (INetFwOpenPort mPort in netFwMgr.LocalPolicy.CurrentProfile.GloballyOpenPorts)
            {
                if (objPort == mPort)
                {
                    exist = true;
                    break;
                }
            }
            if (!exist) netFwMgr.LocalPolicy.CurrentProfile.GloballyOpenPorts.Add(objPort);
        }

        /// <summary>
        /// 删除防火墙例外端口
        /// </summary>
        /// <param name="port">端口</param>
        /// <param name="protocol">协议（TCP、UDP）</param>
        public static void FirewallDelete(int port, string protocol)
        {
            INetFwMgr netFwMgr = (INetFwMgr)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwMgr"));
            if (protocol == "TCP")
            {
                netFwMgr.LocalPolicy.CurrentProfile.GloballyOpenPorts.Remove(port, NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP);
            }
            else
            {
                netFwMgr.LocalPolicy.CurrentProfile.GloballyOpenPorts.Remove(port, NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP);
            }
        }
        #endregion

        #region 属性:获取Gpu
        /// <summary>
        /// 获取Gpu
        /// </summary>
        /// <returns></returns>
        public static string GpuName
        {
            get
            {
                try
                {
                    string gpuName = string.Empty;
                    string query = "SELECT * FROM Win32_DisplayConfiguration";

                    using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
                    {
                        foreach (ManagementObject mObject in searcher.Get())
                        {
                            gpuName += mObject["Description"].ToString() + "; ";
                        }
                    }

                    return (!string.IsNullOrEmpty(gpuName)) ? gpuName : "N/A";
                }
                catch
                {
                    return "Unknown";
                }
            }
        }
        #endregion

        #region 属性:获取系统中杀毒软件的信息
        /// <summary>
        /// 获取系统中杀毒软件的信息
        /// </summary>
        /// <returns></returns>
        public static string Antivirus
        {
            get
            {
                try
                {
                    string antivirusName = string.Empty;
                    // starting with Windows Vista we must use the root\SecurityCenter2 namespace
                    string scope = (PlatformHelper.VistaOrHigher) ? "root\\SecurityCenter2" : "root\\SecurityCenter";
                    string query = "SELECT * FROM AntivirusProduct";

                    using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
                    {
                        foreach (ManagementObject mObject in searcher.Get())
                        {
                            antivirusName += mObject["displayName"].ToString() + "; ";
                        }
                    }

                    return (!string.IsNullOrEmpty(antivirusName)) ? antivirusName : "N/A";
                }
                catch
                {
                    return "Unknown";
                }
            }
        }
        #endregion

        #region 属性:获取IPV4地址
        /// <summary>
        /// 获取IPV4地址
        /// </summary>
        /// <returns></returns>
        public static string LocalIPV4
        {
            get
            {
                try
                {
                    string HostName = Dns.GetHostName();
                    IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                    for (int i = 0; i < IpEntry.AddressList.Length; i++)
                    {
                        if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                            return IpEntry.AddressList[i].ToString();
                    }
                    return "Unknown";
                }
                catch
                {
                    return "Unknown";
                }
            }
        }
        #endregion

        #region 属性:获取硬盘序列号
        /// <summary>
        /// 获取硬盘序列号
        /// </summary>
        public static string BIOSSerialNumber
        {
            get
            {
                try
                {
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * From Win32_BIOS");
                    string sBIOSSerialNumber = "";
                    foreach (ManagementObject mo in searcher.Get())
                    {
                        sBIOSSerialNumber = mo["SerialNumber"].ToString().Trim();
                    }
                    return sBIOSSerialNumber;
                }
                catch (Exception)
                {
                    return "Unknown";
                }
            }
        }
        #endregion

        #region 属性:MAC
        /// <summary>
        /// MAC地址
        /// </summary>
        public static string Mac
        {
            get
            {
                try
                {
                    string mac = "";
                    ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
                    ManagementObjectCollection queryCollection = query.Get();
                    foreach (ManagementObject mo in queryCollection)
                    {
                        if (mo["IPEnabled"].ToString() == "True")
                            mac = mo["MacAddress"].ToString();
                    }
                    return mac;
                }
                catch
                {
                    return "Unknown";
                }
            }
        }
        #endregion

        #region 属性:获取操作系统版本
        /// <summary>
        /// 获取操作系统版本
        /// </summary>
        public static string OSFullName
        {
            get
            {
                string fullName = "Unknown";
                try
                {
                    using (var searcher = new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem"))
                    {
                        foreach (ManagementObject os in searcher.Get())
                        {
                            fullName = os["Caption"].ToString();
                            break;
                        }
                    }
                    fullName = Regex.Replace(fullName, "^.*(?=Windows)", "").TrimEnd().TrimStart(); // Remove everything before first match "Windows" and trim end & start
                    var is64Bit = Environment.Is64BitOperatingSystem;
                    return $"{fullName} {(is64Bit ? 64 : 32)} Bit";
                }
                catch
                {
                    return "Unknown";
                }
            }
        }
        #endregion

        #region 属性:获取CPU信息
        /// <summary>
        /// 获取CPU信息
        /// </summary>
        public static string CPU
        {
            get
            {
                try
                {
                    RegistryKey reg = Registry.LocalMachine;
                    reg = reg.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\0");
                    return reg.GetValue("ProcessorNameString").ToString();
                }
                catch
                {
                    return "Unknown";
                }
            }
        }

        #endregion

        #region 属性:获取内存信息
        /// <summary>
        /// 获取内存信息
        /// </summary>
        public static long MemorySize
        {
            get
            {
                try
                {
                    Microsoft.VisualBasic.Devices.Computer My = new Microsoft.VisualBasic.Devices.Computer();
                    return (long)My.Info.TotalPhysicalMemory;
                }
                catch
                {
                    return 0;
                }
            }
        }
        #endregion

        #region FindAllDesktopWindows
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        private delegate bool WNDENUMPROC(IntPtr hWnd, int lParam);
        //枚举窗体
        [DllImport("user32.dll")]
        private static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, int lParam);
        // 获取窗口Text 
        [DllImport("user32.dll")]
        private static extern int GetWindowTextW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);
        //获取窗口类名 
        [DllImport("user32.dll")]
        private static extern int GetClassNameW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)]StringBuilder lpString, int nMaxCount);
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释

        /// <summary>
        /// 获取桌面正在运行的程序
        /// </summary>
        /// <returns></returns>
        public static List<WindowInfoEntity> FindAllDesktopWindows()
        {
            List<WindowInfoEntity> lstEntits = new List<WindowInfoEntity>();
            try
            {
                EnumWindows(delegate (IntPtr hWnd, int lParam)
                {
                    WindowInfoEntity info = new WindowInfoEntity();
                    StringBuilder sb = new StringBuilder(256);
                    info.hWnd = hWnd;
                    GetWindowTextW(hWnd, sb, sb.Capacity);
                    info.szWindowName = sb.ToString();
                    GetClassNameW(hWnd, sb, sb.Capacity);
                    info.szClassName = sb.ToString();
                    lstEntits.Add(info);
                    return true;
                }, 0);
            }
            catch { }
            return lstEntits;
        }
        #endregion

        #region 句柄操作
        private const int MAX_PATH = 260;
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        public const int PROCESS_ALL_ACCESS = 0x000F0000 | 0x00100000 | 0xFFF;
        [DllImport("User32.dll")]
        public extern static int GetWindowThreadProcessId(IntPtr hWnd, ref int lpdwProcessId);
        [DllImport("Kernel32.dll")]
        public extern static IntPtr OpenProcess(int fdwAccess, int fInherit, int IDProcess);
        [DllImport("User32.dll")]
        public extern static bool TerminateProcess(IntPtr hProcess, int uExitCode);
        [DllImport("Kernel32.dll")]
        public extern static bool CloseHandle(IntPtr hObject);
        [DllImport("Kernel32.dll", EntryPoint = "GetModuleFileName")]
        private static extern uint GetModuleFileName(IntPtr hModule, [Out] StringBuilder lpszFileName, int nSize);
        [DllImport("User32.dll", SetLastError = true)]
        private static extern IntPtr ExtractIconEx(string fileName, int index, ref IntPtr hIconLarge, ref IntPtr hIconSmall, uint nIcons);
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释

        /// <summary>
        /// 通过句柄获取运行程序路径
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>

        public static string GetAppRunPathFromHandle(IntPtr hwnd)
        {
            int pId = 0;
            IntPtr pHandle = IntPtr.Zero;
            GetWindowThreadProcessId(hwnd, ref pId);
            pHandle = OpenProcess(PROCESS_ALL_ACCESS, 0, pId);
            StringBuilder sb = new StringBuilder(MAX_PATH);
            GetModuleFileName(pHandle, sb, sb.Capacity);
            CloseHandle(pHandle);
            return sb.ToString();
        }

        /// <summary>
        /// 根据句柄获取运行程序小图标 //当然也可以获取大图标
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns>异常返回null</returns>
        public static Icon GetSmallIconFromHandle(IntPtr hwnd)
        {
            IntPtr hLargeIcon = IntPtr.Zero;
            IntPtr hSmallIcon = IntPtr.Zero;
            String filePath = GetAppRunPathFromHandle(hwnd);

            ExtractIconEx(filePath, 0, ref hLargeIcon, ref hSmallIcon, 1);
            Icon icon = null;
            try
            {
                icon = Icon.FromHandle(hSmallIcon);
            }
            catch
            {
                return null;
            }
            return icon;
        }
        #endregion

        #region 获取进程路径
        /// <summary>
        /// 获取ProcessId
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        public static int GetRunPathProcessId(int hwnd)
        {
            int processId = 0;
            GetWindowThreadProcessId(hwnd, out processId);
            return processId;
        }
        /// <summary>
        /// 获取运行路径
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static string GetRunPath(int pid)
        {
            string path = Process.GetProcessById(pid).MainModule.FileName;
            return path.Substring(0, path.LastIndexOf("\\"));
        }
        /// <summary>
        /// 获取运行Module
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static ProcessModule GetRunProcessModule(int pid)
        {
            try
            {
                return Process.GetProcessById(pid).MainModule;
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 获取运行路径
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        public static string GetRunPathByhwnd(int hwnd)
        {
            StringBuilder sb = new StringBuilder(65535);
            int processId = 0;
            GetWindowThreadProcessId(hwnd, out processId);
            if (processId == 0)
            {
                return "";
            }
            return GetRunPath(processId);
        }
        #endregion

        #region GetWindowThreadProcessId
        [DllImport("user32.dll")]
        private static extern int GetWindowThreadProcessId(int hwnd, out int processId);

        #endregion

        #region 设置程序Lock为自己的启动方式
        private static void setAssociatedFileType(string typeName, string app)
        {
            string fileType = getTypeKeyName(typeName);
            Registry.ClassesRoot.OpenSubKey(fileType + "\\shell\\open\\command", true).SetValue(null, app);
        }
        /// <summary>
        /// 把程序打开方式设置为ExecutablePath
        /// </summary>
        /// <param name="ExecutablePath"></param>
        /// <param name="type"></param>
        public static void Lock(string ExecutablePath, string type = ".exe")
        {
            setAssociatedFileType(type, "\"" + ExecutablePath + "\"" + " \"%1\"");
        }
        /// <summary>
        /// 解锁打开方式为系统默认
        /// </summary>
        public static void UnLock(string type = ".exe")
        {
            setAssociatedFileType(type, "\"%1\" %*");
        }
        private static string getTypeKeyName(string typeName)
        {
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(typeName);
            return (string)key.GetValue(null);
        }
        #endregion

        #region CloseThis
        /// <summary>
        /// 进程的方式Kill自身
        /// </summary>
        public static void CloseThis()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            foreach (Process process in processes)
            {
                if (process.Id == current.Id)
                {
                    process.Kill();
                }
            }
        }
        /// <summary>
        /// 进程的方式Kill自身,保留一个
        /// </summary>
        public static void CloseThisOnlyOne(string processName)
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(processName);
            for (int i = 0; i < processes.Length; i++)
            {
                if (processes[i].Id == current.Id)
                {
                    processes[i].Kill();
                }
            }
        }
        #endregion

        #region BytesToString
        /// <summary>
        /// BytesToString
        /// </summary>
        /// <param name="byteCount">1024*19</param>
        /// <returns>19KB</returns>
        public static string BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return $"{(Math.Sign(byteCount) * num).ToString(CultureInfo.InvariantCulture)} {suf[place]}";
        }
        #endregion


    }
}
