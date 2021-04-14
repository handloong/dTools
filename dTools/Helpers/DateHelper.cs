using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dTools
{
    /// <summary>
    /// 时间帮助类
    /// </summary>
    public class DateHelper
    {
        /// <summary>
        /// 获取当前时间第几周
        /// </summary>
        /// <returns></returns>
        public static int GetWeekOfYear()
        {
            GregorianCalendar gc = new GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay/*设置第一天*/, DayOfWeek.Monday/*设置周一为一周的第一天*/);
            return weekOfYear;
        }
    }
}
