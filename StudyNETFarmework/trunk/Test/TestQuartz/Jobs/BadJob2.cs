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
    /// 设置作业失败后停止相关触发器执行
    /// </summary>
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class BadJob2 : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            ConsoleExtend.WriteLineDebug1($"【{DateTime.Now}】 开始执行作业 作业【{context.JobDetail.Key}】 触发器【{context.Trigger.Key}】");
            int denominator = 0;
            try
            {
                var i = 10 / denominator;
            }
            catch (Exception e)
            {
                ConsoleExtend.WriteLineError($"{GetType().FullName}作业执行异常：{e.Message}");
                denominator = 1;
                JobExecutionException e2 = new JobExecutionException();
                e2.UnscheduleAllTriggers = true; // Quartz 会自动取消与此工作相关的所有触发器，这样它就不会再次运行了。
                throw e2;
            }

            ConsoleExtend.WriteLineDebug2($"【{DateTime.Now}】 作业执行完成 作业【{context.JobDetail.Key}】 触发器【{context.Trigger.Key}】");
            return Task.FromResult(true);
        }
    }
}
