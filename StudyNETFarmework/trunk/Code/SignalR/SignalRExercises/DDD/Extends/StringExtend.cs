using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace SignalRExercises.DDD.Extends
{
    public static class StringExtend
    {
        /// <summary>
        /// 获取指定长度随机字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static string GetRandomString(this string sourceStr , int length)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Random random = new Random();
            for(int i = length; i > 0; i--)
            {
                stringBuilder.Append(sourceStr[random.Next(0, sourceStr.Length)]);
            }
            return stringBuilder.ToString();
        }
    }
}