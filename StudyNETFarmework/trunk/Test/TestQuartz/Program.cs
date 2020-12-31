using Common.Extend;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestQuartz.JobFactory;
using TestQuartz.Jobs;
using TestQuartz.Listener;
using TestQuartz.SchedulerLog;

namespace TestQuartz
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine($"{DateTime.Now} will run at: {DateTime.Now:r}");//语法糖
            
            LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());
            
            Console.WriteLine("测试执行开始");
            RunTestAsync().Wait();
            Console.WriteLine("测试执行结束");
        }

        private static async Task RunTestAsync()
        {
            //获取程序集的 System.Reflection.Assembly 实例
            var assembly = typeof(Program).Assembly;

            var ITestsType = new List<Type>(GetAssemblyTypes<ITest>(assembly));
            ITestsType.Sort(new TypeNameComparer());
            var count = 1;

            Console.WriteLine("选择要运行的测试(输入exit退出测试)：");

            if (ITestsType.Count <= 0) return;

            foreach (var iTestType in ITestsType)
            {
                ClassDiscribleAttribute classDiscribleAttribute = GetTypeAttribute<ClassDiscribleAttribute>(iTestType);
                Console.WriteLine("{0} {1} {2}"
                    , $"[{count++}]".PadRight(4)
                    , $"{iTestType.Namespace}  {iTestType.Name}".PadRight(50)
                    , classDiscribleAttribute == null?String.Empty: classDiscribleAttribute.Discrible);
            }

            Console.WriteLine();
            Console.Write("> ");
            String writeText = Console.ReadLine();
            Console.WriteLine("");

            if (writeText.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
                Environment.Exit(Environment.ExitCode);

            int num = 0;

            while (!int.TryParse(writeText, out num) || num > ITestsType.Count() || num < 1)
            {
                Console.WriteLine($"请输入正确的数字，范围在1到{ITestsType.Count()}之间");
                Console.Write("> ");
                writeText = Console.ReadLine();
            }

            Type type = ITestsType[num-1];

            ITest test = (ITest)Activator.CreateInstance(type);

            await test.RunAsync().ConfigureAwait(false);

            await Task.Delay(TimeSpan.FromSeconds(10)).ConfigureAwait(false);
        }

        /// <summary>
        /// 获取程序集中所有类型
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        private static Type[] GetAssemblyTypes(Assembly assembly)
        {
            return assembly.GetTypes();
        }

        /// <summary>
        /// 获取程序集中指的所有类型
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="assembly">程序集</param>
        /// <returns></returns>
        private static IEnumerable<Type> GetAssemblyTypes<T>(Assembly assembly) where T : class
        {
            Type tType = typeof(T);
            foreach (Type type in GetAssemblyTypes(assembly))
            {
                if (type is T|| type.GetInterfaces().ToList().Contains(tType))
                {
                    yield return type;
                }
            }
        }

        /// <summary>
        /// 获取类型指定自定义特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        private static T GetTypeAttribute<T>(Type type) where T : Attribute
        {
            return (T)type.GetCustomAttribute(typeof(T));
        }

        public static void Test()
        {
            // 通过工厂获取默认调度器
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;

            scheduler.ListenerManager.AddTriggerListener(new TriggerListener());
            // 开启一个调度
            scheduler.Start();


            {
                // 定义作业并将其与我们的HelloJob类联系起来
                IJobDetail job = JobBuilder.Create<HelloJob>()
                    .WithIdentity("job1", "group1")
                    .Build();

                // 创建一个触发器，现在就运行，每10秒执行一次
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger1", "group1")
                    .StartNow()
                    .WithSimpleSchedule(p => p
                        .WithIntervalInSeconds(10)
                        .RepeatForever())
                    .Build();

                // 调度作业
                DateTimeOffset dateTimeOffset = scheduler.ScheduleJob(job, trigger).Result;
                Console.WriteLine(dateTimeOffset.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            {
                IJobDetail job2 = JobBuilder.Create<HelloJob2>()
                    .WithIdentity("job2", "group1")
                    .Build();
                var job1 = JobBuilder.Create<HelloJob>()
                    .WithIdentity("NewJob1", "group1")
                    .Build();

                ITrigger trigger2 = TriggerBuilder.Create()
                    .WithIdentity("trigger2", "group1")
                    .WithSimpleSchedule(p => p
                        .WithIntervalInSeconds(5)
                        .RepeatForever())
                    .Build();
                var dateTimeOffset = scheduler.ScheduleJob(job2, trigger2).Result;
                Console.WriteLine(dateTimeOffset.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            {
                IJobDetail parameterJob = JobBuilder.Create<ParameterJob>()
                    .WithIdentity("带参数作业", "带参数作业组")
                    .WithDescription("测试带参数作业")
                    //.UsingJobData("dataBaseCon", "这里是数据库连接配置")
                    //.UsingJobData("jsonParameter", "这里是JSon参数配置")
                    .SetJobData(new JobDataMap() {
                        {"dataBaseCon","数据库配置字符串" },
                        {"jsonParameter","json参数字符串" }
                    })
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("带参数作业触发器", "带参数作业触发器组")
                    .WithDescription("触发器描述")
                    .StartNow()
                    .UsingJobData("jsonParameter", "触发器中的Json参数字符串")
                    .WithSimpleSchedule(p => p
                        .WithIntervalInSeconds(5)
                        .RepeatForever())
                    .Build();



                var dateTimeOffset = scheduler.ScheduleJob(parameterJob, trigger).Result;
                Console.WriteLine(dateTimeOffset.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            {
                //scheduler.JobFactory = new MyJobFactory();


                //TriggerBuilder triggerBuilder = TriggerBuilder.Create()
                //    .WithIdentity("工厂作业触发器", "工厂作业触发器组")
                //    .WithDescription("触发器描述")
                //    .ForJob("","")
                //    .StartNow()
                //    .UsingJobData("jsonParameter", "触发器中的Json参数字符串")
                //    .WithSimpleSchedule(p => p
                //        .WithIntervalInSeconds(5)
                //        .RepeatForever());


                //scheduler.ScheduleJob(triggerBuilder.Build());

            }

            // 获取当前正在执行的作业
            var list = scheduler.GetCurrentlyExecutingJobs().Result;
            Console.WriteLine("当前正在执行作业：");
            foreach (IJobExecutionContext item in list)
            {
                var itemJob = item.JobDetail;
                var itemTrigger = item.Trigger;
                Console.WriteLine("当前触发器：组：【{0}】 名称【{1}】 描述【{2}】", itemTrigger.Key.Group, itemTrigger.Key.Name, itemTrigger.Description);
                Console.WriteLine("当前触作业：组：【{0}】 名称【{1}】 描述【{2}】", itemJob.Key.Group, itemJob.Key.Name, itemJob.Description);
            }


            Thread.Sleep(TimeSpan.FromSeconds(60));
            // 关闭调度
            scheduler.Shutdown(false).Wait();
        }

        public static void TestListener()
        {
            // 通过工厂获取默认调度器
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;

            scheduler.ListenerManager.AddSchedulerListener(new SchedulerListener());
            scheduler.ListenerManager.AddTriggerListener(new TriggerListener());
            scheduler.ListenerManager.AddJobListener(new JobListener());
            // 开启一个调度
            scheduler.Start();

            {
                IJobDetail parameterJob = JobBuilder.Create<ParameterJob>()
                    .WithIdentity("带参数作业", "带参数作业组")
                    .WithDescription("测试带参数作业")
                    //.UsingJobData("dataBaseCon", "这里是数据库连接配置")
                    //.UsingJobData("jsonParameter", "这里是JSon参数配置")
                    .SetJobData(new JobDataMap() {
                        {"dataBaseCon","数据库配置字符串" },
                        {"jsonParameter","json参数字符串" }
                    })
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("带参数作业触发器", "带参数作业触发器组")
                    .WithDescription("触发器描述")
                    .StartNow()
                    .UsingJobData("jsonParameter", "触发器中的Json参数字符串")
                    .WithSimpleSchedule(p => p
                        .WithIntervalInSeconds(5)
                        .RepeatForever())
                    .Build();



                var dateTimeOffset = scheduler.ScheduleJob(parameterJob, trigger).Result;
                Console.WriteLine(dateTimeOffset.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            }            

            Thread.Sleep(TimeSpan.FromSeconds(15));
            // 关闭调度
            scheduler.Shutdown(false).Wait();
        }
    }

    class TypeNameComparer : IComparer<Type>
    {
        public int Compare(Type t1, Type t2)
        {
            if (t1.Namespace.Length > t2.Namespace.Length)
            {
                return 1;
            }

            if (t1.Namespace.Length < t2.Namespace.Length)
            {
                return -1;
            }

            return t1.Namespace.CompareTo(t2.Namespace);
        }
    }
}
