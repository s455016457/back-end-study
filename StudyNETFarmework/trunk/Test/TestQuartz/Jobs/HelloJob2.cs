using Quartz;
using System;
using System.Threading.Tasks;

namespace TestQuartz.Jobs
{
    public class HelloJob2 : IJob
    {
        public Task Execute(IJobExecutionContext context) => new TaskFactory().StartNew(() =>
        {
            Console.WriteLine("HelloJob2！ time:{0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        });
    }
}
