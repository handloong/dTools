using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
    }
}
