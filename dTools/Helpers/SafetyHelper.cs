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
    /// 安全加密类
    /// </summary>
    public class SafetyHelper
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="_securityKey">盐</param>
        /// <returns></returns>
        public static string DescEncrypt(object obj, string _securityKey = "&*dtl6#$")
        {
            StringBuilder builder = new StringBuilder();
            string str = obj.ToString();
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            provider.Key = Encoding.UTF8.GetBytes(_securityKey);
            provider.IV = Encoding.UTF8.GetBytes(_securityKey);
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(), CryptoStreamMode.Write);
            stream2.Write(bytes, 0, bytes.Length);
            stream2.FlushFinalBlock();
            foreach (byte num in stream.ToArray())
            {
                builder.AppendFormat("{0:X2}", num);
            }
            stream.Close();
            return builder.ToString();
        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="_securityKey"></param>
        /// <returns></returns>
        public static string DescCrypt(string str, string _securityKey = "&*dtl6#$")
        {
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            provider.Key = Encoding.ASCII.GetBytes(_securityKey);
            provider.IV = Encoding.ASCII.GetBytes(_securityKey);
            byte[] buffer = new byte[str.Length / 2];
            for (int i = 0; i < (str.Length / 2); i++)
            {
                int num2 = Convert.ToInt32(str.Substring(i * 2, 2), 0x10);
                buffer[i] = (byte)num2;
            }
            MemoryStream stream = new MemoryStream();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(), CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
            stream.Close();
            return Encoding.UTF8.GetString(stream.ToArray());
        }

    }
}
