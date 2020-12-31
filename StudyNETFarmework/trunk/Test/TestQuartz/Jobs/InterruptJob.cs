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
    /// 中断作业
    /// </summary>
    public class InterruptJob : IJob
    {
        private static object lock_obj = new object();
        /// <summary>
        /// 执行计数
        /// </summary>
        public static int ExecutedCount { get; private set; } = 0;
        /// <summary>
        /// 成功计数
        /// </summary>
        public static int SuccessfulCount { get; private set; } = 0;
        /// <summary>
        /// 中断计数
        /// </summary>
        public static int InterruptlCount { get; private set; } = 0;
        public async Task Execute(IJobExecutionContext context)
        {
            var executeTime = DateTime.Now;

            ConsoleExtend.WriteLineDebug($"【{executeTime}】 开始执行作业 作业【{context.JobDetail.Key}】 触发器【{context.Trigger.Key}】");
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    LockAction(() => ExecutedCount++);
                    await Task.Delay(TimeSpan.FromSeconds(10));
                    if (context.CancellationToken.IsCancellationRequested)
                    {
                        ConsoleExtend.WriteLineDebug1($"【{DateTime.Now}】 作业被打断 作业【{context.JobDetail.Key}】 触发器【{context.Trigger.Key}】 触发时间【{executeTime}】");
                        // 也可以抛出JobExecutionException异常
                        LockAction(() => InterruptlCount++);
                    }
                    else
                    {
                        LockAction(() => SuccessfulCount++);
                    }
                }
            }
            finally
            {
                ConsoleExtend.WriteLineDebug2($"【{DateTime.Now}】 作业执行完成 作业【{context.JobDetail.Key}】 触发器【{context.Trigger.Key}】 触发时间【{executeTime}】");
            }
        }

        /// <summary>
        /// 线程安全行为
        /// </summary>
        /// <param name="action"></param>
        private void LockAction(Action action)
        {
            if (action == null) return;
            lock (lock_obj)
            {
                action.Invoke();
            }
        }
    }
}
