using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQuartz.Jobs
{
    /// <summary>
    /// 依赖JobFactory将数据映射值“注入”到作业
    /// </summary>
    public class FactoryJob : Quartz.IJob
    {
        public String DataBaseCon { get; set; }
        public String JsonParameter { get; set; }
        public Task Execute(IJobExecutionContext context)
        {
            return new TaskFactory().StartNew(() =>
            {
                var jobContext = context as IJobExecutionContext;
                var jobKey = jobContext.JobDetail.Key;

                //JobDataMap dataMap = jobContext.JobDetail.JobDataMap; //使用作业中的参数
                JobDataMap dataMap = jobContext.MergedJobDataMap;  // 触发器中的惨覆盖作业中的参数

                Console.WriteLine("作业【{0}】【{1}】 数据库配置【{2}】 json参数【{3}】", jobKey.Group, jobKey.Name, DataBaseCon, JsonParameter);
            });
        }
    }
}
