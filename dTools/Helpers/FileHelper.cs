using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dTools
{
    /// <summary>
    /// 文件帮助类
    /// </summary>
    public class FileHelper
    {
        #region 获取文件MD5值
        /// <summary>
        /// 获取文件MD5值
        /// </summary>
        /// <param name="fileName">文件绝对路径</param>
        /// <returns>MD5值</returns>
        public static string GetMD5HashFromFile(string fileName)
        {
            MD5 md5 = MD5.Create();
            string s = string.Empty;
            using (FileStream fs = File.OpenRead(fileName))
            {
                byte[] b = md5.ComputeHash(fs);
                for (int i = 0; i < b.Length; i++)
                {
                    s += b[i].ToString("x2");
                }
            }
            md5.Clear();
            return s;
        }
        #endregion

        #region 内存级文件监控 IsChange
        private static List<FileCacheEntity> FileCacheEntities = new List<FileCacheEntity>();
        private static object locker = new object();
        /// <summary>
        /// 文件缓存实体
        /// </summary>
        private class FileCacheEntity
        {
            /// <summary>
            /// 文件名
            /// </summary>
            public string FileFullName { get; set; }
            /// <summary>
            /// 修改时间
            /// </summary>
            public DateTime LastWrite { get; set; }
        }
        private static void AddCache(FileInfo fileInfo)
        {
            lock (locker)
            {
                FileCacheEntities.Add(new FileCacheEntity { FileFullName = fileInfo.FullName, LastWrite = fileInfo.LastWriteTime });
            }
        }
        /// <summary>
        /// 文件是否发生改变-内存级文件监控
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="diffSeconds">精确秒数</param>
        /// <returns></returns>
        public static bool IsChange(FileInfo fileInfo, int diffSeconds = 10)
        {
            var cache = FileCacheEntities.OrderByDescending(x => x.LastWrite).FirstOrDefault(x => x.FileFullName == fileInfo.FullName);
            if (cache == null)
            {
                AddCache(fileInfo);
                return true;
            }
            else
            {
                //移除老的数据
                FileCacheEntities.Where(x => x.FileFullName == fileInfo.FullName).ToList().ForEach(x =>
                {
                    FileCacheEntities.Remove(x);
                });
                //添加最新的一笔数据.
                AddCache(fileInfo);
                var time = cache.LastWrite;
                //10s 差异认为文件改变,不能使用绝对值。。
                //if (Math.Abs((fileInfo.LastWriteTime - time).TotalSeconds) > diffSeconds)
                if ((fileInfo.LastWriteTime - time).TotalSeconds > diffSeconds)
                    return true;
                else
                    return false;
            }
        }
        #endregion

        #region 获取指定文件夹下的所有文件
        /// <summary>
        /// 获取指定文件夹下的所有文件,包括里面的嵌套文件夹
        /// </summary>
        /// <param name="directory">文件夹不存在</param>
        /// <param name="searchFilePattern">文件匹配符 多个使用|分割,例如 *.dat|*.txt 默认*.*</param>
        public static List<FileInfo> GetDirectoryAllFiles(string directory,
            string searchFilePattern = "*.*")
        {
            if (Directory.Exists(directory))
            {
                var files = searchFilePattern
                    .Split('|')
                    .SelectMany(sp => Directory.EnumerateFiles(directory, sp, SearchOption.AllDirectories));

                return files.Select(x => new FileInfo(x)).ToList();

                //不使用递归

                //fileFullNames.AddRange(files);

                //var directories = searchFolderPattern
                //     .Split('|')
                //     .SelectMany(sp => Directory.EnumerateDirectories(directory, sp));

                //foreach (var dir in directories)
                //{
                //    fileFullNames.AddRange(GetDirectoryAllFiles(dir, searchFilePattern, searchFolderPattern));
                //}
            }
            return new List<FileInfo>();
        }
        #endregion

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

        /// <summary>
        /// 转移文件
        /// </summary>
        /// <param name="fileInfo">要转移的文件</param>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <param name="path3"></param>
        /// <param name="processFlag"></param>
        /// <param name="zipFile">压缩转移后的文件</param>
        /// <param name="zipFileAdd">压缩后文件后面是否添加GUID默认4位</param>
        /// <param name="zipDelete">删除转移后的文件</param>
        /// <param name="moveOkSleep">转移->压缩过程睡眠毫秒数</param>
        /// <param name="OnException">出现异常</param>
        public static bool MoveFile(FileInfo fileInfo,
            string path1 = "AppDomain.CurrentDomain.BaseDirectory",
            string path2 = "BackFile",
            string path3 = "Date:yyyy-MM",
            string processFlag = "Successful",
            bool zipFile = true,
            bool zipFileAdd = true,
            bool zipDelete = true,
            int moveOkSleep = 1000,
            Action<Exception> OnException = null
            )
        {
            try
            {
                if (path1.IsEmpty() || path1 == "AppDomain.CurrentDomain.BaseDirectory")
                    path1 = AppDomain.CurrentDomain.BaseDirectory;

                if (path3.StartsWith("Date:"))
                {
                    path3 = DateTime.Now.ToString(path3.Split(':')[1]);
                }
                var back = Combine(path1, path2, path3, processFlag);
                var moved = Path.Combine(back, fileInfo.Name);
                File.Move(fileInfo.FullName, moved);
                Thread.Sleep(moveOkSleep);//保证文件转移完毕

                if (zipFile && zipFileAdd)
                {
                    var add = ".zip";
                    if (zipFileAdd)
                    {
                        add = $".{Guid.NewGuid().ToString().Substring(0, 4).ToLower()}.zip";
                    }
                    ZipHelper.ZipFile(moved, moved + add);
                }

                if (zipFile && zipDelete)
                {
                    Thread.Sleep(moveOkSleep);
                    File.Delete(moved);
                }
                return true;
            }
            catch (Exception ex)
            {
                OnException?.Invoke(ex);
                return false;
            }
        }
    }
}
