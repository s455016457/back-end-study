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
    /// 长时间作业
    /// </summary>
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class LogTimeJob : Quartz.IJob
    {
        public const string NumExecutions = "NumExecutions";
        public const string ExecutionDelay = "ExecutionDelay";
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap map = context.JobDetail.JobDataMap;

            int executeCount = 0;
            if (map.ContainsKey(NumExecutions))
            {
                executeCount = map.GetInt(NumExecutions);
            }

            executeCount++;

            map.Put(NumExecutions, executeCount);

            int delay = 5;
            if (map.ContainsKey(ExecutionDelay))
            {
                delay = map.GetInt(ExecutionDelay);
            }

            ConsoleExtend.WriteLineDebug1($"【{DateTime.Now}】 开始执行 触发器【{context.Trigger.Key}】 作业【{context.JobDetail.Key}】 执行次数【{executeCount}】");
            await Task.Delay(TimeSpan.FromSeconds(delay));

            ConsoleExtend.WriteLineDebug2($"【{DateTime.Now}】 执行完成 触发器【{context.Trigger.Key}】 作业【{context.JobDetail.Key}】 执行次数【{executeCount}】");            
        }
    }
}
