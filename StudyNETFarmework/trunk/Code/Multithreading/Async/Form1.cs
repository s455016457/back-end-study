using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Async
{
    public partial class Form1 : Form
    {
        delegate string AsyncMethodCaller(out int threadId);
        System.IO.StringWriter StringWriter = new System.IO.StringWriter();
        public Form1()
        {
            InitializeComponent();
            Console.SetOut(StringWriter);
            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 500;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            richTextBox1.Text = StringWriter.ToString();
            richTextBox1.Select(richTextBox1.TextLength, 0);
        }

        private void btnSingleThread_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            btnSingleThread.Enabled = false;
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Console.WriteLine( "单线程等待2秒显示，期间窗口不能操作");
            btnSingleThread.Enabled = true;
        }

        private void btnAsync_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            btnAsync.Enabled = false;
            Console.WriteLine($"方法运行的线程：{Environment.CurrentManagedThreadId}" );
            int threadId;
            AsyncMethodCaller caller = new AsyncMethodCaller(AsyncMethod);
            var asyncResult = caller.BeginInvoke(out threadId, new AsyncCallback(GetResult), null);
        }

        private async void btnAsync2_ClickAsync(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            btnAsync2.Enabled = false;
            Console.WriteLine($"await前方法运行的线程：{Environment.CurrentManagedThreadId}" );
            await GetTask();
            Console.WriteLine($"await后方法运行的线程：{Environment.CurrentManagedThreadId}" );
        }

        private Task GetTask()
        {
            return Task.Factory.StartNew(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(2));
                int threadId = Thread.CurrentThread.ManagedThreadId;
                Console.WriteLine($"异步方法等待2秒显示，期间窗口可以操作{Environment.NewLine}异步方法运行的线程：{threadId}{Environment.NewLine}");
                btnAsync2.Enabled = true;
            });
        }

        public string AsyncMethod(out int threadId)
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            threadId = Thread.CurrentThread.ManagedThreadId;
            return $"异步方法等待2秒显示，期间窗口可以操作{Environment.NewLine}异步方法运行的线程：{threadId}";
        }
        private void GetResult(IAsyncResult result)
        {
            int threadId;
            AsyncMethodCaller caller = (AsyncMethodCaller)((AsyncResult)result).AsyncDelegate;
            // 调用EndInvoke去等待异步调用完成并且获得返回值  
            // 如果异步调用尚未完成，则 EndInvoke 会一直阻止调用线程，直到异步调用完成  
            string resultvalue = caller.EndInvoke(out threadId, result);
            Console.WriteLine($"{resultvalue}{Environment.NewLine}结果方法运行的线程：{Environment.CurrentManagedThreadId}");
            btnAsync.Enabled = true;
        }

        private void btnMultithreading_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            btnMultithreading.Enabled = false;
            Console.WriteLine($"方法运行的线程：{Environment.CurrentManagedThreadId}" );
            ThreadPool.QueueUserWorkItem(new WaitCallback(p => {
                Thread.Sleep(TimeSpan.FromSeconds(2));
                int threadId = Thread.CurrentThread.ManagedThreadId;
                Console.WriteLine($"多线程方法等待2秒显示，期间窗口可以操作{Environment.NewLine}多线程方法运行的线程：{threadId}");
                btnMultithreading.Enabled = true;
            }), null);
        }

        bool isStop = false, isStart = false, isComm = true;
        CancellationTokenSource cts = new CancellationTokenSource();

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            StringWriter = new System.IO.StringWriter();
            Console.SetOut(StringWriter);
        }

        private void btn_TaskLongRunning_Click(object sender, EventArgs e)
        {
            if (btn_TaskLongRunning.Text == "TaskLongRunning")
            {
                cts = new CancellationTokenSource();
                int MaxWorkerThreads = 10, MaxCompletionPortThreads = 10;
                Console.WriteLine(@"设置线程池中辅助线程的最大数目为{0}， 线程池中异步 I/O 线程的最大数目为{1}
同时运行30个长时运行线程，看是否30个线程是否都能运行。", MaxWorkerThreads, MaxCompletionPortThreads);
                ThreadPool.GetMaxThreads(out MaxWorkerThreads, out MaxCompletionPortThreads);
                Console.WriteLine($"最大可用辅助线程数目为{MaxCompletionPortThreads}，最大可用异步 I/O 线程数目为{MaxCompletionPortThreads}");
                ThreadPool.SetMaxThreads(10, 10);
                ThreadPool.GetMaxThreads(out MaxWorkerThreads, out MaxCompletionPortThreads);
                Console.WriteLine($"最大可用辅助线程数目为{MaxCompletionPortThreads}，最大可用异步 I/O 线程数目为{MaxCompletionPortThreads}");

                int count = 1;
                while (count++ <= 30)
                {

                    //int runCount = 0;
                    //Task.Factory.FromAsync(LongRunningTask($"线程{count}", runCount), null, TaskCreationOptions.LongRunning, TaskScheduler.Default);

                    Task.Factory.StartNew(p =>
                    {
                        int index = (int)p;
                        int runCount = 0;
                        LongRunningTask($"线程{index}", runCount, cts.Token);
                    }, count, cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                }
                btn_TaskLongRunning.Text = "暂停";
            }
            else
            {
                cts.Cancel();
                btn_TaskLongRunning.Text = "TaskLongRunning";
            }
        }

        private void LongRunningTask(string taskName, int runCount, CancellationToken token)
        {
            PrintTask($"任务【{taskName}】线程ID【{Environment.CurrentManagedThreadId}】第【{++runCount}】次运行").Wait();
            if (!token.IsCancellationRequested)
                LongRunningTask(taskName, runCount, token);
        }

        private Task PrintTask(string message)
        {
            return Task.Factory.StartNew(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine(message);
            });
        }

        private async void btn_TaskProgress_ClickAsync(object sender, EventArgs e)
        {
            if (isStop || isComm)
            {
                btn_TaskProgress.Text = "暂停";
                cts = new CancellationTokenSource();
                isStart = true;
                isComm = isStop = false;
            }
            else if (isStart)
            {
                btn_TaskProgress.Text = "TAP进度";
                isStop = true;
                isComm = isStart = false;
                cts.Cancel();
            }

            if (isStart)
            {
                //Console.Clear();
                var task = Download("下载文件.test", cts.Token, p => { Console.WriteLine($"下载进度：{p}"); });
                task.Start();
                isComm = await task;
            }

            isStop = !isComm;
            if (isStop || isComm)
            {
                isStart = false;
                Console.WriteLine("任务返回状态：{0}", isStop ? "暂停" : "完成");
                btn_TaskProgress.Text = "TAP进度";
            }
        }

        private Task<bool> Download(string Url, CancellationToken token, Action<decimal> progress)
        {
            return new Task<bool>(p =>
            {
                int fileLength = 20480, _progress = 0; ;
                Console.WriteLine($"开始下载：{Url}");
                Console.WriteLine($"文件大小：{fileLength}");
                while (true)
                {
                    Thread.Sleep(500);
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("已取消下载");
                        return false;
                    }
                    if (_progress < fileLength)
                    {
                        _progress += 1024;
                        Console.WriteLine($"已下载：{_progress}");
                        if (progress != null)
                        {
                            var pro = progress as Action<decimal>;
                            if (pro != null)
                                pro.Invoke(_progress / (decimal)fileLength);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"下载完成");
                        return true;
                    }
                }
            }, progress, token, TaskCreationOptions.None);
        }
    }    
}
