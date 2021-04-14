using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dTools
{
    /// <summary>
    /// 重试类
    /// </summary>
    public static class RetryHelper
    {
        /// <summary>
        /// 无论遇到任何错误，最多尝试<paramref name="times"/>次
        /// <list type="string" > 关于重试睡眠机制,这里采用每次睡眠5秒,有的采用times的乘积,即:重试2次,睡眠5s
        ///  <item>1**:1  +%=:5</item>
        ///  <item>2**:5  +%=:15</item>
        ///  <item>3**:14  +%=:30</item>
        ///  <item>4**:30  +%=:50</item>
        ///  <item>5**:55  +%=:75</item>
        ///  <item>6**:91  +%=:105</item>
        ///  <item>7**:140  +%=:140</item>
        ///  <item>8**:204  +%=:180</item>
        ///  <item>9**:285  +%=:225</item>
        ///  <item>10**:385  +%=:275</item>
        /// </list>
        /// <typeparam name="T"></typeparam>
        /// <param name="times"></param>
        /// <param name="action"></param>
        /// </summary>
        /// <returns></returns>
        public static T RetryOnAny<T>(int times, Func<T> action)
        {
            return RetryOnAny(times, a =>
            {
                return action.Invoke();
            }, (i, e) =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(i * 5));
            });
        }


        /// <summary>
        /// 无论遇到任何错误，最多尝试<paramref name="times"/>次
        /// <list type="string" > 
        /// <item>关于重试睡眠机制,这里采用每次睡眠5秒,有的采用times的乘积,即:重试2次,睡眠5s</item>
        ///  <item>1**:1  +%=:5</item>
        ///  <item>2**:5  +%=:15</item>
        ///  <item>3**:14  +%=:30</item>
        ///  <item>4**:30  +%=:50</item>
        ///  <item>5**:55  +%=:75</item>
        ///  <item>6**:91  +%=:105</item>
        ///  <item>7**:140  +%=:140</item>
        ///  <item>8**:204  +%=:180</item>
        ///  <item>9**:285  +%=:225</item>
        ///  <item>10**:385  +%=:275</item>
        /// </list>
        /// <typeparam name="T"></typeparam>
        /// <param name="times"></param>
        /// <param name="action"></param>
        /// <param name="efunc"></param>
        /// </summary>
        /// <returns></returns>
        public static T RetryOnAny<T>(int times, Func<int, T> action, Action<int, Exception> efunc)
        {
            for (int i = 0; i < times; i++)
            {
                try
                {
                    return action.Invoke(i + 1);
                }
                catch (Exception ex)
                {
                    efunc?.Invoke((i + 1), ex);
                }
            }
            return default;
        }

        /// <summary>
        /// 无论遇到任何错误，最多尝试<paramref name="times"/>次 
        /// <list type="string" > 关于重试睡眠机制,这里采用每次睡眠5秒,有的采用times的乘积,即:重试2次,睡眠5s
        ///  <item>1**:1  +%=:5</item>
        ///  <item>2**:5  +%=:15</item>
        ///  <item>3**:14  +%=:30</item>
        ///  <item>4**:30  +%=:50</item>
        ///  <item>5**:55  +%=:75</item>
        ///  <item>6**:91  +%=:105</item>
        ///  <item>7**:140  +%=:140</item>
        ///  <item>8**:204  +%=:180</item>
        ///  <item>9**:285  +%=:225</item>
        ///  <item>10**:385  +%=:275</item>
        /// </list>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="times"></param>
        /// <param name="action"></param>
        /// <param name="efunc"></param>
        /// <returns></returns>
        public static T RetryOnAny<T>(int times, Func<T> action, Action<int, Exception> efunc)
        {
            for (int i = 0; i < times; i++)
            {
                try
                {
                    return action.Invoke();
                }
                catch (Exception ex)
                {
                    Thread.Sleep(TimeSpan.FromSeconds((i + 1) * 5));
                    efunc?.Invoke((i + 1), ex);
                }
            }
            return default;
        }

        /// <summary>
        /// 当遇到<typeparamref name="E"/>的异常时重试指定<paramref name="times"/>次，遇到其他异常则认为失败。
        /// </summary>
        /// <list type="string" > 关于重试睡眠机制,这里采用每次睡眠5秒,有的采用times的乘积,即:重试2次,睡眠5s
        ///  <item>1**:1  +%=:5</item>
        ///  <item>2**:5  +%=:15</item>
        ///  <item>3**:14  +%=:30</item>
        ///  <item>4**:30  +%=:50</item>
        ///  <item>5**:55  +%=:75</item>
        ///  <item>6**:91  +%=:105</item>
        ///  <item>7**:140  +%=:140</item>
        ///  <item>8**:204  +%=:180</item>
        ///  <item>9**:285  +%=:225</item>
        ///  <item>10**:385  +%=:275</item>
        /// </list>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="E"></typeparam>
        /// <param name="times"></param>
        /// <param name="action"></param>
        /// <param name="efunc"></param>
        /// <returns></returns>
        public static T RetryOnException<T, E>(int times, Func<int, T> action, Action<int, Exception> efunc) where E : Exception
        {
            for (int i = 0; i < times; i++)
            {
                try
                {
                    try
                    {
                        return action.Invoke(i);
                    }
                    catch (E e)
                    {
                        Thread.Sleep(TimeSpan.FromSeconds((i + 1) * 5));
                        efunc?.Invoke((i + 1), e);
                    }
                }
                catch (Exception)
                {
                    break;
                }
            }
            return default;
        }
    }
}
