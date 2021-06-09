using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace dTools
{
    /// <summary>
    /// 字符串帮助类
    /// </summary>
    public class StringHelper
    {
        #region 创建随机字符串

        /// <summary>
        /// 创建随机字符串
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <returns>string</returns>
        public static string BuildRandomString(int length)
        {
            var codeSerial = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            if (length == 0)
            {
                length = 16;
            }
            var arr = codeSerial.Split(',');
            var code = "";
            var rand = new Random(unchecked((int)DateTime.Now.Ticks));
            for (var i = 0; i < length; i++)
            {
                int randValue = rand.Next(0, arr.Length - 1);
                code += arr[randValue];
            }
            return code;
        }

        /// <summary>
        /// 创建随机字符串
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <param name="allowedChars">随机字符串源</param>
        /// <returns></returns>
        public static string BuildRandomString(int length, string allowedChars)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), "length cannot be less than zero.");

            if (string.IsNullOrEmpty(allowedChars))
                allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            const int byteSize = 0x100;
            char[] allowedCharSet = new HashSet<char>(allowedChars).ToArray();
            if (byteSize < allowedCharSet.Length)
                throw new ArgumentException($"allowedChars may contain no more than {byteSize} characters.");

            using (var rng = new RNGCryptoServiceProvider())
            {
                var result = new StringBuilder();
                byte[] buf = new byte[128];

                while (result.Length < length)
                {
                    rng.GetBytes(buf);
                    for (int i = 0; i < buf.Length && result.Length < length; ++i)
                    {
                        int outOfRangeStart = byteSize - (byteSize % allowedCharSet.Length);
                        if (outOfRangeStart <= buf[i])
                            continue;
                        result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
                    }
                }
                return result.ToString();
            }
        }

        #endregion 创建随机字符串

    }
}
