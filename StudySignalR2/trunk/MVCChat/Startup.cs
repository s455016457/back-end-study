using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MVCChat.Startup))]
namespace MVCChat
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var hubConfiguration = new HubConfiguration();
#if DEBUG
            hubConfiguration.EnableDetailedErrors = true;   // 显示详细异常信息
#endif
            /**
             * 禁用自动产生JavaScript代理文件
             * 如需要手工添加代理文件
             * 详情请见：https://docs.microsoft.com/zh-cn/aspnet/signalr/overview/guide-to-the-api/hubs-api-guide-javascript-client#manualproxy
             * **/
            //hubConfiguration.EnableJavaScriptProxies = false;

            /**
             * 运行跨域访问
             * 客户端设置详情请见：https://docs.microsoft.com/zh-cn/aspnet/signalr/overview/guide-to-the-api/hubs-api-guide-javascript-client#crossdomain
             * */
            //hubConfiguration.EnableJSONP = true;

            /**
             * 自定义signalr URL
             * Web客户端连接需要做一下设置：
             * 有代理文件的情况：
             * $.connection.hub.url = "/signalr"；
             * $.connection.hub.start().done(init);// init在初始化是调用该方法
             * 无代理文件的情况：
             * var connection = $.hubConnection("/signalr", { useDefaultPath: false });
             * 
             * .NET客户端代码：
             * var hubConnection = new HubConnection("http://contoso.com/signalr", useDefaultUrl: false);
             * **/
            var srApp =  app.MapSignalR("/signalr", hubConfiguration);
            var type = srApp.GetType();
        }
    }
}