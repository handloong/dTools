using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dTools
{
    /// <summary>
    /// 注册表帮助类
    /// </summary>
    public class RegeditHelper
    {
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="path">路径,例如:HKEY_CURRENT_USER\Control Panel\Colors</param>
        /// <param name="name">名 例如:Window</param>
        /// <returns></returns>
        public static string GetKey(string path, string name)
        {
            var ps = path.Replace("\\", "//").TrimStart('/').TrimEnd('/').Split('/').ToList().FindAll(x => x != "").ToList();
            if (ps.Count < 2)
                throw new Exception($"{path}不正确,至少两个节点");
            var rkey = GetRegistryByPath(path);
            var ik = rkey.OpenSubKey(ps[1], true);
            if (ik == null)
                throw new Exception($"{path}下的{ps[1]}节点不存在");
            for (int i = 2; i < ps.Count; i++)
            {
                ik = ik.OpenSubKey(ps[i], true);
                if (ik == null)
                    throw new Exception($"{path}下的{ps[i]}节点不存在");
            }
            try
            {
                return ik.GetValue(name).ToString();
            }
            catch (Exception)
            {
                throw new Exception($"{path}下的名称:{name} 不存在。");
            }
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="path">路径,例如:HKEY_CURRENT_USER\Control Panel\Colors</param>
        /// <param name="name">名 例如:Window</param>
        /// <param name="value">值 例如:109 109 109</param>
        /// <returns></returns>
        public static bool SetKey(string path, string name, object value)
        {
            var ps = path.Replace("\\", "//").TrimStart('/').TrimEnd('/').Split('/').ToList().FindAll(x => x != "").ToList();
            if (ps.Count<2)
                throw new Exception($"{path}不正确,至少两个节点");

            var rkey = GetRegistryByPath(path);
            var ik = rkey.OpenSubKey(ps[1], true);
            if (ik==null)
                throw new Exception($"{path}下的{ps[1]}节点不存在");
            for (int i = 2; i < ps.Count; i++)
            {
                ik = ik.OpenSubKey(ps[i], true);
                if (ik==null)
                {
                    throw new Exception($"{path}下的{ps[i]}节点不存在");
                }
            }
            try
            {
                ik.SetValue(name, value);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"在对{path}下的名为{name}设置值为{value.ToString()}的时候出现异常,信息:{ex.Message}");
            }
        }

        /// <summary>
        /// 根据路径获取RegistryKey
        /// </summary>
        /// <returns></returns>
        private static RegistryKey GetRegistryByPath(string path)
        {
            var p = path.Replace("\\", "//").TrimStart('/').TrimEnd('/').Split('/').ToList().FindAll(x => x != "").ToList();
            switch (p[0].ToString().ToUpper())
            {
                case "HKEY_CLASSES_ROOT": return Registry.ClassesRoot;
                case "HKEY_CURRENT_USER": return Registry.CurrentUser;
                case "HKEY_LOCAL_MACHINE": return Registry.LocalMachine;
                case "HKEY_USERS": return Registry.Users;
                case "HKEY_CURRENT_CONFIG": return Registry.CurrentConfig;
                default:
                    return Registry.PerformanceData;
            }
        }
    }
}
