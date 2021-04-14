using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace dTools
{
    /// <summary>
    /// IO帮助类
    /// </summary>
    public class IOHelper
    {
        #region CreateDirectory
        /// <summary>
        /// 如果目录不存在则创建一个目录
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        /// <summary>
        /// 如果目录不存在则创建一个目录
        /// </summary>
        /// <param name="paths"></param>
        public static void CreateDirectory(string[] paths)
        {
            var full = Path.Combine(paths);
            if (!Directory.Exists(full))
            {
                CreateDirectory(full);
            }
        }
        #endregion

        #region Path.Combine 并自动创建Combine后的文件夹
        /// <summary>
        /// Path.Combine 并自动创建Combine后的文件夹
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        public static string Combine(string path1, string path2)
        {
            var ret = Path.Combine(path1, path2);
            CreateDirectory(ret);
            return ret;
        }
        /// <summary>
        /// Path.Combine 并自动创建Combine后的文件夹
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <param name="path3"></param>
        /// <returns></returns>
        public static string Combine(string path1, string path2, string path3)
        {
            var ret = Path.Combine(path1, path2, path3);
            CreateDirectory(ret);
            return ret;
        }
        /// <summary>
        /// Path.Combine 并自动创建Combine后的文件夹
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <param name="path3"></param>
        /// <param name="path4"></param>
        /// <returns></returns>
        public static string Combine(string path1, string path2, string path3, string path4)
        {
            var ret = Path.Combine(path1, path2, path3, path4);
            CreateDirectory(ret);
            return ret;
        }
        /// <summary>
        /// Path.Combine 并自动创建Combine后的文件夹
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string Combine(params string[] paths)
        {
            var ret = Path.Combine(paths);
            CreateDirectory(ret);
            return ret;
        }
        #endregion

        #region GetFiles
        /// <summary>
        /// 获取路径下的所有文件,如果目录不存在返回空集合(不是null)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<FileInfo> GetFiles(string path)
        {
            if (!Directory.Exists(path))
            {
                return new List<FileInfo>();
            }
            return new DirectoryInfo(path).GetFiles().ToList();
        }
        #endregion

    }
}
