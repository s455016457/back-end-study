using Common.Extend;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQuartz.Jobs
{
    /// <summary>
    /// 带参数作业
    /// </summary>
    public class ParameterJob : Quartz.IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return new TaskFactory().StartNew(() =>
            {
                var jobContext = context as IJobExecutionContext;
                var jobKey = jobContext.JobDetail.Key;

                //JobDataMap dataMap = jobContext.JobDetail.JobDataMap; //使用作业中的参数
                JobDataMap dataMap = jobContext.MergedJobDataMap;  // 触发器中的惨覆盖作业中的参数

                string dataBaseCon = dataMap.GetString("dataBaseCon");
                string JsonParameter = dataMap.GetString("jsonParameter");

                ConsoleExtend.WriteLineDebug2($"【{DateTime.Now.ToLongTimeString()}】 作业【{jobKey}】 " +
                    $"描述【{jobContext.JobDetail.Description}】 数据库配置【{dataBaseCon}】 " +
                    $"json参数【{JsonParameter}】");
            });
        }
    }
}
