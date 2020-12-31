using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.Extend;
using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Spi;

namespace TestQuartz.Plugin
{
    /// <summary>
    /// 作业历史记录插件
    /// 继承 ISchedulerPlugin(调度插件)、IJobListener(作业监听)接口
    /// </summary>
    public class LoggingJobHistoryPlugin : ISchedulerPlugin, IJobListener
    {
        /// <summary>
        /// 作业监听名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 调度器
        /// </summary>
        public IScheduler Scheduler { get; private set; }
        /// <summary>
        /// 传播有关应取消操作的通知
        /// </summary>
        public CancellationToken CancellationToken { get; private set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="pluginName"></param>
        /// <param name="scheduler"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Initialize(string pluginName, IScheduler scheduler, CancellationToken cancellationToken = default(CancellationToken))
        {
            Name = pluginName;
            Scheduler = scheduler;
            // 为所有作业添加此监听器
            Scheduler.ListenerManager.AddJobListener(this, EverythingMatcher<JobKey>.AllJobs());
            CancellationToken = cancellationToken;
            return Task.FromResult(true);
        }

        /// <summary>
        /// 作业执行取消
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            
            ConsoleExtend.WriteLineYellow($"插件【{Name}】 作业【{context.JobDetail.Key}】执行失败 触发器【{context.Trigger.Key}】" +
                $" 触发时间【{SystemTime.UtcNow()}】 下次触发时间【{context.Trigger.GetNextFireTimeUtc()}】");
            return Task.FromResult(true);
        }

        /// <summary>
        /// 作业将执行
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            ConsoleExtend.WriteLineYellow($"插件【{Name}】 作业【{context.JobDetail.Key}】执行 触发器【{context.Trigger.Key}】 触发时间【{SystemTime.UtcNow()}】");
            return Task.FromResult(true);
        }
        /// <summary>
        /// 作业执行完成
        /// </summary>
        /// <param name="context"></param>
        /// <param name="jobException"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default(CancellationToken))
        {
            ConsoleExtend.WriteLineYellow($"插件【{Name}】 作业【{context.JobDetail.Key}】执行完成 触发器【{context.Trigger.Key}】" +
                $" 触发时间【{SystemTime.UtcNow()}】 下次触发时间【{context.Trigger.GetNextFireTimeUtc()}】");
            return Task.FromResult(true);
        }

        /// <summary>
        /// 调度器关闭
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Shutdown(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(true);
        }
        /// <summary>
        /// 调度器开启
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task Start(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(true);
        }
    }
}
