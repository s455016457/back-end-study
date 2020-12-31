using SignalRExercises.DDD.Extends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRExercises.DDD.Comm
{
    public class Utility
    {
        static string SourceStr = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
        /// <summary>
        /// 获取指定长度随机字符串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetRandomString(int length)
        {
            return SourceStr.GetRandomString(length);
        }
    }
}