using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestAsync
{
    [TestClass]
    public class TestTask
    {
        /// <summary>
        /// 测试任务
        /// 只运行了9个任务
        /// </summary>
        [TestMethod]
        public void TestTask1()
        {
            var cts = new CancellationTokenSource();
            int MaxWorkerThreads = 0, MaxCompletionPortThreads = 0;
            ThreadPool.GetMaxThreads(out MaxWorkerThreads, out MaxCompletionPortThreads);
            Console.WriteLine($"最大可用辅助线程数目为{MaxCompletionPortThreads}，最大可用异步 I/O 线程数目为{MaxCompletionPortThreads}");
            MaxWorkerThreads = 10;
            MaxCompletionPortThreads = 10;
            Console.WriteLine(@"设置线程池中辅助线程的最大数目为{0}， 线程池中异步 I/O 线程的最大数目为{1}
同时运行30个长时运行线程，每个线程中运行一个同步方法，看是否30个线程是否都能运行。", MaxWorkerThreads, MaxCompletionPortThreads);
            ThreadPool.SetMaxThreads(10, 10);
            ThreadPool.GetMaxThreads(out MaxWorkerThreads, out MaxCompletionPortThreads);
            Console.WriteLine($"最大可用辅助线程数目为{MaxCompletionPortThreads}，最大可用异步 I/O 线程数目为{MaxCompletionPortThreads}");
            
            int count = 0;
            while (count++ < 30)
            {
                Task.Factory.StartNew(p =>
                {
                    int index = (int)p;
                    int runCount = 0;
                    LongRunningTask($"线程{index}", runCount, cts.Token);
                }, count, cts.Token, TaskCreationOptions.None, TaskScheduler.Default);
            }

            Task.Delay(TimeSpan.FromSeconds(10)).Wait(TimeSpan.FromSeconds(20)); // 等待超时，等待任务没有执行
            cts.Cancel();
        }
        /// <summary>
        /// 测试任务
        /// 只运行前9个任务
        /// </summary>
        [TestMethod]
        public void TestTask2()
        {
            var cts = new CancellationTokenSource();
            int MaxWorkerThreads = 10, MaxCompletionPortThreads = 10;
            Console.WriteLine(@"设置线程池中辅助线程的最大数目为{0}， 线程池中异步 I/O 线程的最大数目为{1}
同时运行30个长时运行线程，每个线程中运行一个异步方法，看是否30个线程是否都能运行。", MaxWorkerThreads, MaxCompletionPortThreads);
            ThreadPool.GetMaxThreads(out MaxWorkerThreads, out MaxCompletionPortThreads);
            Console.WriteLine($"最大可用辅助线程数目为{MaxCompletionPortThreads}，最大可用异步 I/O 线程数目为{MaxCompletionPortThreads}");
            ThreadPool.SetMaxThreads(10, 10);
            ThreadPool.GetMaxThreads(out MaxWorkerThreads, out MaxCompletionPortThreads);
            Console.WriteLine($"最大可用辅助线程数目为{MaxCompletionPortThreads}，最大可用异步 I/O 线程数目为{MaxCompletionPortThreads}");
            
            int count = 0;
            while (count++ < 30)
            {
                Task.Factory.StartNew(async p =>
                {
                    int index = (int)p;
                    int runCount = 0;
                    await LongRunningTaskAsync($"线程{index}", runCount, cts.Token);
                }, count, cts.Token, TaskCreationOptions.None, TaskScheduler.Default);
            }

            Task.Delay(TimeSpan.FromSeconds(10)).Wait(TimeSpan.FromSeconds(20));    // 等待没有超时，等待任务有执行
            cts.Cancel();
        }

        /// <summary>
        /// 测试长时运行任务
        /// 30个任务全部都运行了
        /// </summary>
        [TestMethod]
        public void TestTaskLongRunning()
        {
            var cts = new CancellationTokenSource();
            int MaxWorkerThreads = 0, MaxCompletionPortThreads = 0;
            ThreadPool.GetMaxThreads(out MaxWorkerThreads, out MaxCompletionPortThreads);
            Console.WriteLine($"最大可用辅助线程数目为{MaxCompletionPortThreads}，最大可用异步 I/O 线程数目为{MaxCompletionPortThreads}");
            MaxWorkerThreads = 10;
            MaxCompletionPortThreads = 10;
            Console.WriteLine(@"设置线程池中辅助线程的最大数目为{0}， 线程池中异步 I/O 线程的最大数目为{1}
同时运行30个长时运行线程，每个线程中运行一个同步方法，看是否30个线程是否都能运行。", MaxWorkerThreads, MaxCompletionPortThreads);
            ThreadPool.SetMaxThreads(10, 10);
            ThreadPool.GetMaxThreads(out MaxWorkerThreads, out MaxCompletionPortThreads);
            Console.WriteLine($"最大可用辅助线程数目为{MaxCompletionPortThreads}，最大可用异步 I/O 线程数目为{MaxCompletionPortThreads}");
            
            int count = 0;
            while (count++ < 30)
            {

                Task.Factory.StartNew(p =>
                {
                    int index = (int)p;
                    int runCount = 0;
                    LongRunningTask($"线程{index}", runCount, cts.Token);
                }, count, cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }

            Task.Delay(TimeSpan.FromSeconds(10)).Wait(TimeSpan.FromSeconds(20));    // 等待没有超时，等待任务有执行
            cts.Cancel();
        }
        /// <summary>
        /// 测试长时运行任务
        /// 只运行了前9个任务
        /// </summary>
        [TestMethod]
        public void TestTaskLongRunning2()
        {
            var cts = new CancellationTokenSource();
            int MaxWorkerThreads = 0, MaxCompletionPortThreads = 0;
            ThreadPool.GetMaxThreads(out MaxWorkerThreads, out MaxCompletionPortThreads);
            Console.WriteLine($"最大可用辅助线程数目为{MaxCompletionPortThreads}，最大可用异步 I/O 线程数目为{MaxCompletionPortThreads}");

            MaxWorkerThreads = 10;
            MaxCompletionPortThreads = 10;
            Console.WriteLine(@"设置线程池中辅助线程的最大数目为{0}， 线程池中异步 I/O 线程的最大数目为{1}
同时运行30个长时运行线程，每个线程中运行一个异步方法，看是否30个线程是否都能运行。", MaxWorkerThreads, MaxCompletionPortThreads);
            ThreadPool.SetMaxThreads(10, 10);
            ThreadPool.GetMaxThreads(out MaxWorkerThreads, out MaxCompletionPortThreads);
            Console.WriteLine($"最大可用辅助线程数目为{MaxCompletionPortThreads}，最大可用异步 I/O 线程数目为{MaxCompletionPortThreads}");
            
            int count = 0;
            while (count++ < 30)
            {

                Task.Factory.StartNew(async p =>
                {
                    int index = (int)p;
                    int runCount = 0;
                    await LongRunningTaskAsync($"线程{index}", runCount, cts.Token);
                }, count, cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }

            Task.Delay(TimeSpan.FromSeconds(10)).Wait(TimeSpan.FromSeconds(20));    // 等待没有超时，等待任务有执行
            cts.Cancel();
        }
        /// <summary>
        /// 异步长时运行任务
        /// </summary>
        /// <param name="taskName">任务名称</param>
        /// <param name="runCount">运行次数</param>
        /// <param name="token">传播有关取消操作的通知</param>
        /// <returns></returns>
        private async Task LongRunningTaskAsync(string taskName, int runCount, CancellationToken token)
        {
            await PrintTask($"任务【{taskName}】线程ID【{Environment.CurrentManagedThreadId}】第【{++runCount}】次运行");
            if (!token.IsCancellationRequested)
                await LongRunningTaskAsync(taskName, runCount, token);
        }
        /// <summary>
        /// 长时运行任务
        /// 递归运行
        /// </summary>
        /// <param name="taskName">任务名称</param>
        /// <param name="runCount">运行次数</param>
        /// <param name="token">传播有关取消操作的通知</param>
        private void LongRunningTask(string taskName, int runCount, CancellationToken token)
        {
            PrintTask($"任务【{taskName}】线程ID【{Environment.CurrentManagedThreadId}】第【{++runCount}】次运行").Wait();
            if (!token.IsCancellationRequested)
                LongRunningTask(taskName, runCount, token);
        }
        /// <summary>
        /// 异步打印任务 等待1秒后打印消息
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns></returns>
        private Task PrintTask(string message)
        {
            return Task.Factory.StartNew(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine(message);
            });
        }
    }
}
