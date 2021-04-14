using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dTools
{
    /// <summary>
    /// Int扩展
    /// </summary>
    public static class ExtensionsInt
    {
        #region 创建随机字符串
        /// <summary>
        /// 创建随机字符串
        /// </summary>
        /// <param name="this">字符串长度</param>
        /// <returns>string</returns>
        public static string GetRandCode(this int @this)
        {
            var codeSerial = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            if (@this == 0)
            {
                @this = 16;
            }
            var arr = codeSerial.Split(',');
            var code = new StringBuilder();
            for (var i = 0; i < @this; i++)
            {
                var t = 0.GetRandomNumber(arr.Length);
                code.Append(arr[t]);
            }
            return code.ToString();
        }
        #endregion

        #region 创建随机密码
        /// <summary>
        /// 创建随机密码
        /// </summary>
        /// <param name="this">字符串长度</param>
        /// <returns>string</returns>
        public static string GetRandPassword(this int @this)
        {
            var codeSerial = "$|%|#|@|_|+|=|@|#|$|%||&|*|(|.|)|,|.|_|0|1|2|3|4|5|6|7|8|9|a|b|c|d|e|f|g|h|i|j|k|l|m|n|o|p|q|r|s|t|u|v|w|x|y|z|A|B|C|D|E|F|G|H|I|J|K|L|M|N|O|P|Q|R|S|T|U|V|W|X|Y|Z";
            if (@this == 0)
            {
                @this = 16;
            }
            var arr = codeSerial.Split('|');
            var code = new StringBuilder();
            for (var i = 0; i < @this; i++)
            {
                var t = 0.GetRandomNumber(arr.Length);
                code.Append(arr[t]);
            }
            return code.ToString();
        }
        #endregion

        #region 创建随机数字
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        /// <summary>
        /// 创建随机数字
        /// </summary>
        /// <param name="this">最小值（包括最小值）</param>
        /// <param name="max">最大值（不包括最大值）</param>
        /// <returns></returns>
        public static int GetRandomNumber(this int @this, int max)
        {
            lock (syncLock)
            {
                return random.Next(@this | max);
            }
        }

        /// <summary>
        /// 创建随机数字
        /// </summary>
        /// <param name="this"></param>
        /// <param name="min">最小值（包括最小值）</param>
        /// <param name="max">最大值（不包括最大值）</param>
        /// <returns></returns>
        public static int GetRandomNumber(this int @this, int min, int max)
        {
            lock (syncLock)
            {
                return random.Next(min | max);
            }
        }
        #endregion

    }
}
