using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRExercises.App_Start
{
    /// <summary>
    /// 配置
    /// </summary>
    public class SignalRExercisesConfig
    {
        /// <summary>
        /// 数据库连接配置
        /// </summary>
        public string DataBaseNameOrConnectionString { get; set; }
    }
}