using Common.Extend;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQuartz.Jobs
{
    [PersistJobDataAfterExecution]// 在执行后持久化作业数据
    [DisallowConcurrentExecution] // 禁止并发执行
    public class ColorJob : Quartz.IJob
    {
        public const String ColorKey = "color";
        public const String ColorCountKey = "colorCount";

        /// <summary>
        /// 计数
        /// </summary>
        private int Count { get; set; } = 1;
        public Task Execute(IJobExecutionContext context)
        {
            IJobDetail jobDetail = context.JobDetail;

            JobDataMap jobDataMap = jobDetail.JobDataMap;
            JobDataMap dataMap = context.MergedJobDataMap;  // 触发器中的惨覆盖作业中的参数

            string color = dataMap.GetString(ColorKey);
            int colorCount = dataMap.GetIntValue(ColorCountKey);

            ConsoleExtend.WriteLineNextDebug($"【{DateTime.Now}】作业【{jobDetail.Key}】 描述【{jobDetail.Description}】" +
                $"触发器【{context.Trigger.Key}】 " +
                $"MergedJobDataMap中喜欢颜色【{color}】 jobDataMap中喜欢颜色【{jobDataMap.GetString(ColorKey)}】 " +
                $"作业MergedJobDataMap中计数【{colorCount}】 jobDataMap中计数【{jobDataMap.GetIntValue(ColorCountKey)}】 作业成员属性计数【{Count}】" +
                $"触发器jobDataMap中计数【{context.Trigger.JobDataMap.GetIntValue(ColorCountKey)}】 ");

            colorCount++;

            context.Trigger.JobDataMap.Put(ColorCountKey, colorCount);
            //dataMap.Put(ColorCountKey, colorCount);
            jobDataMap.Put(ColorCountKey, jobDataMap.GetIntValue(ColorCountKey) + 1);

            Count++;

            return Task.FromResult(true);
        }
    }
}
