using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dTools
{
    /// <summary>
    /// 设备帮助类
    /// </summary>
    public class DeviceHelper
    {
        /// <summary>
        /// 通过注册表启用USB
        /// </summary>
        /// <returns></returns>
        public static bool RunUSB()
        {
            try
            {
                RegistryKey regKey = Registry.LocalMachine; //读取注册列表HKEY_LOCAL_MACHINE
                string keyPath = @"SYSTEM\CurrentControlSet\Services\USBSTOR"; //USB 大容量存储驱动程序
                RegistryKey openKey = regKey.OpenSubKey(keyPath, true);
                openKey.SetValue("Start", 3); //设置键值对（3）为开启USB（4）为关闭
                openKey.Close(); //关闭注册列表读写流
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 通过注册表禁用USB
        /// </summary>
        /// <returns></returns>
        public static bool StopUSB()
        {
            try
            {
                RegistryKey regKey = Registry.LocalMachine;
                string keyPath = @"SYSTEM\CurrentControlSet\Services\USBSTOR";
                RegistryKey openKey = regKey.OpenSubKey(keyPath, true);
                openKey.SetValue("Start", 4);
                openKey.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
