using System;
using System.Text;
using System.Net;
using System.IO;

namespace WebPrintService
{
    public class HttpListenerService
    {
        public HttpListenerService() { }

        internal void OnStart(params string[] args)
        {
            Console.Title = "HTTP 监听服务！";
            HttpListener httplistener = new HttpListener();

            httplistener.Prefixes.Add("http://localhost:11223/");

            try
            {
                httplistener.Start();
                System.Threading.ThreadPool.SetMinThreads(10, 3);

                System.Threading.ThreadPool.SetMaxThreads(1000, 512);

                System.Threading.ThreadPool.QueueUserWorkItem((start) => { Handle(httplistener); });

                FluentConsole.Green.Line("API监听服务启动成功！");

                FluentConsole.Green.Line("监听端口：");
                foreach (var item in httplistener.Prefixes)
                {
                    FluentConsole.Green.Line(item);
                }
            }
            catch (Exception ex)
            {
                FluentConsole.Red.Line(ex.Message);
                if (httplistener.IsListening)
                {
                    httplistener.Stop();
                }
            }
        }

        private void Handle(System.Net.HttpListener httpListener)
        {
            while (true)
            {
                HttpListenerContext context = httpListener.GetContext();
                HttpListenerRequest request = context.Request;

                FluentConsole.Gray.Line($"监听到请求：{request.Url}");

                if (httpListener.IsListening && request != null)
                {
                    HttpListenerResponse response = context.Response;
                    response.ContentEncoding = Encoding.UTF8;
                    response.ContentType = "text/html; charset=utf-8";

                    if (request.RawUrl.IndexOf("/UploadPrinterLists") > -1)
                    {
                        FluentConsole.Green.Line("UploadPrinterLists 请求成功>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                        FluentConsole.Green.Line("-------------------------------<>---------------------------");
                        using (StreamReader streamReader = new StreamReader(request.InputStream))
                        {
                            FluentConsole.DarkGray.Line($"请求body：{streamReader.ReadToEnd()}");
                        }
                    }
                    else if (request.RawUrl.IndexOf("/UploadDefaultPrinter") > -1)
                    {
                        FluentConsole.Green.Line("UploadDefaultPrinter 请求成功>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                        FluentConsole.Green.Line("-------------------------------<>---------------------------");
                        using (StreamReader streamReader = new StreamReader(request.InputStream))
                        {
                            FluentConsole.DarkGray.Line($"请求body：{streamReader.ReadToEnd()}");
                        }
                    }
                    else if (request.RawUrl.IndexOf("/GetReport") > -1)
                    {
                        FluentConsole.Green.Line("GetReport 请求成功>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                        FluentConsole.Green.Line("-------------------------------<>---------------------------");
                        using (StreamReader streamReader = new StreamReader(request.InputStream))
                        {
                            FluentConsole.DarkGray.Line($"请求body：{streamReader.ReadToEnd()}");
                        }
                        using (var fileStream = File.OpenRead("TestDocument/ASP.NET WEB MVC生命周期.pdf"))
                        {
                            var bufferBytes = new byte[1024];
                            var index = fileStream.Read(bufferBytes, 0, bufferBytes.Length);
                            while (index > 0)
                            {
                                response.OutputStream.Write(bufferBytes, 0, index);
                                index = fileStream.Read(bufferBytes, 0, bufferBytes.Length);
                            }
                        }
                    }
                    else
                    {
                        var bytes = Encoding.UTF8.GetBytes("Web打印服务启动成功！");
                        response.OutputStream.Write(bytes, 0, bytes.Length);
                    }
                    response.OutputStream.Flush();
                    response.StatusCode = 200;
                    response.Close();
                    FluentConsole.Gray.Line($"响应到请求：{request.Url}");
                }
            }
        }
    }
}
