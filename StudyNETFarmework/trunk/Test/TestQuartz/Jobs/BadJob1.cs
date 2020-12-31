using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Extend;
using Quartz;

namespace TestQuartz.Jobs
{
    /// <summary>
    /// 错误作业
    /// 设置作业失败后马上重新开始
    /// </summary>
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class BadJob1 : IJob
    {
        public const String denominatorKey = "denominator";
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                int denominator = context.JobDetail.JobDataMap.GetIntValue(denominatorKey);
                ConsoleExtend.WriteLineDebug1($"【{DateTime.Now}】 开始执行作业 作业【{context.JobDetail.Key}】 触发器【{context.Trigger.Key}】 参数【分母：{denominator}】");
                var i = 10 / denominator;                
            }
            catch (Exception e)
            {
                ConsoleExtend.WriteLineError($"{GetType().FullName}作业执行异常：{e.Message}");
                context.JobDetail.JobDataMap.Put(denominatorKey, 1);
                JobExecutionException e2 = new JobExecutionException();
                e2.RefireImmediately = true; // 设置作业马上重新开始
                throw e2;
            }

            ConsoleExtend.WriteLineDebug2($"【{DateTime.Now}】 作业执行完成 作业【{context.JobDetail.Key}】 触发器【{context.Trigger.Key}】");
            return Task.FromResult(true);
        }
    }
}
