using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quartz
{
    /// <summary>
    /// 计划触发器配置
    /// </summary>
    public class CronTriggerConfig
    {
        private int? year, day, hour, minute, second;

        /// <summary>
        /// 年
        /// </summary>
        public int? Year { get { return year; } set { year = value; } }
        /// <summary>
        /// 月
        /// </summary>
        public Month? Month { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public int? Day {
            get { return day; }
            set { if (value > 31 || value < 0) throw new Exception("日期必须在0到31之间！");day = value; }
        }
        /// <summary>
        /// 小时
        /// </summary>
        public int? Hour {
            get { return hour; }
            set { if (value > 23 || value < 0) throw new Exception("小时必须在0到23之间！"); hour = value; }
        }
        /// <summary>
        /// 分钟
        /// </summary>
        public int? Minute
        {
            get { return minute; }
            set { if (value > 59 || value < 0) throw new Exception("分钟必须在0到59之间！"); minute = value; }
        }
        /// <summary>
        /// 秒
        /// </summary>
        public int? Second
        {
            get { return second; }
            set { if (value > 59 || value < 0) throw new Exception("秒必须在0到59之间！"); second = value; }
        }
        /// <summary>
        /// 星期
        /// </summary>
        public Week? Week { get; set; }

        public String GetStringConfig()
        {
            String c = "*";
            return String.Format("{0} {1} {2} {3} {4} {5} {6}",
                Second.HasValue ? second.Value.ToString() : c,
                Minute.HasValue ? Minute.Value.ToString() : c,
                Hour.HasValue ? Hour.Value.ToString() : c,
                Day.HasValue ? Day.Value.ToString() : c,
                Month.HasValue ? Month.Value.ToString() : c,
                Week.HasValue ? Week.Value.ToString() : c,
                Year.HasValue ? Year.Value.ToString() : c
                );
        }
    }

    /// <summary>
    /// 月份
    /// </summary>
    public enum Month
    {
        /// <summary>
        /// 一月
        /// </summary>
        JAN = 0,
        /// <summary>
        /// 二月
        /// </summary>
        FEB = 1,
        /// <summary>
        /// 三月
        /// </summary>
        MAR = 2,
        /// <summary>
        /// 四月
        /// </summary>
        APR = 3,
        /// <summary>
        /// 五月
        /// </summary>
        MAY = 4,
        /// <summary>
        /// 六月
        /// </summary>
        JUN = 5,
        /// <summary>
        /// 七月
        /// </summary>
        JUL = 6,
        /// <summary>
        /// 八月
        /// </summary>
        AUG = 7,
        /// <summary>
        /// 九月
        /// </summary>
        SEP = 8,
        /// <summary>
        /// 十月
        /// </summary>
        OCT = 9,
        /// <summary>
        /// 十一月
        /// </summary>
        NOV = 10,
        /// <summary>
        /// 十二月
        /// </summary>
        DEC = 11
    }

    /// <summary>
    /// 星期
    /// </summary>
    public enum Week
    {
        /// <summary>
        /// 星期七天
        /// </summary>
        SUN = 0,
        /// <summary>
        /// 星期一
        /// </summary>
        MON = 1,
        /// <summary>
        /// 星期二
        /// </summary>
        TUE = 2,
        /// <summary>
        /// 星期三
        /// </summary>
        WED = 3,
        /// <summary>
        /// 星期四
        /// </summary>
        THU = 4,
        /// <summary>
        /// 星期五
        /// </summary>
        FRI = 5,
        /// <summary>
        /// 星期六
        /// </summary>
        SAT = 6
    }

}
