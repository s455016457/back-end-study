# [Startup.cs类说明](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/startup?view=aspnetcore-3.1#the-configureservices-method)

## Startup

`startup`类配置Services和应用程序的请求管道。

* 包含一个[ConfigureServices](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.hosting.startupbase.configureservices)方法去配置应用程序的services。service是一个提供应用程序功能的可重用组件。通过[依赖注入(dependency injection (DI))](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1)或[应用程序服务(ApplicationServices)](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.builder.iapplicationbuilder.applicationservices)在整个应用程序中使用。
* 包含一个[Configure](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.hosting.startupbase.configure)方法，用来创建应用程序的请求处理管道。

当应用程序启动时，`ConfigureService`和`Configure`方法在ASP.NET Core运行环境中被调用。

主机提供`Startup`类构造函数可用的服务。应用程序通过`ConfigureServices`附加服务。主机和应用程序服务都可以在`Configure`和整个应用程序中使用。

当使用[通用主句(GenericHost(IHOstBuilder))](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.hosting.ihostbuilder)时，只有以下类型服务可以注入到`Startup`构造函数中。

* [IWebHostEnvironment](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.hosting.iwebhostenvironment) Web主机环境接口，提供有关应用程序正在运行的 Web 托管环境的信息。
* [IHostEnvironment](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.hosting.ihostenvironment) 主机环境接口，提供有关其中正在运行应用程序的宿主环境的信息。
* [IConfiguration](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.configuration.iconfiguration?view=dotnet-plat-ext-3.1) 配置接口，表示一组键/值应用程序配置属性。

## 多个Startup

应用程序为不同的环境（例如，`StartupDevelopment`）单独定义`Startup`类时，响应的`Startup`类会在运行时被选中。优先考虑名称后缀与当前环境相匹配的类。 如果应用在开发环境中运行并包含 `Startup` 类和 `StartupDevelopment` 类，则使用 `StartupDevelopment` 类。 有关详细信息，请参阅[使用多个环境](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/environments?view=aspnetcore-3.1#environment-based-startup-class-and-methods)。

请参阅[主机](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/?view=aspnetcore-3.1#host)，了解有关主机的详细信息。 有关在启动过程中处理错误的信息，请参阅[启动异常处理](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/error-handling?view=aspnetcore-3.1#startup-exception-handling)。

## ConfigureServices 方法

`ConfigureServices`方法是可选的，他在主机调用`Configure`方法前调用，以配置应用程序的服务。[配置选项](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1)按约定设置。

主机可能会在调用`Startup`的方法之前配置某些服务。 有关详细信息，请参阅[主机](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/?view=aspnetcore-3.1#host)。

对于需要大量设置的功能，[IServiceCollection](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection)上有`Add{Service}` 扩展方法。 例如，`AddDbContext`、`AddDefaultIdentity`、`AddEntityFrameworkStores` 和 `AddRazorPages`：

``` C#
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // 配置数据库上下文
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                Configuration.GetConnectionString("DefaultConnection")));

        // 配置默认的身份验证
        services.AddDefaultIdentity<IdentityUser>(
            options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();
        // 添加Razor页面
        services.AddRazorPages();
    }
```

将服务添加到服务容器，使其在应用和 `Configure` 方法中可用。 服务通过[依赖关系注入](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1)或 [ApplicationServices](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.builder.iapplicationbuilder.applicationservices) 进行解析。

## Configure 方法

[Configure](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.hosting.startupbase.configure)方法用于指定应用程序如何响应HTTP请求。可通过将[中间件](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.1)组件添加到[IApplicationBuilder](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.builder.iapplicationbuilder)示例来配置请求管道。`IApplicationBuilder`虽然没有在服务容器中注册，但是可以直接用于`Configure`方法，Hosting创建了一个`IApplicationBuilder`并直接提供给了`Configure`方法。

[ASP.NET Core 模板](https://docs.microsoft.com/zh-cn/dotnet/core/tools/dotnet-new)配置的管道支持：

* [开发人员异常页](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/error-handling?view=aspnetcore-3.1#developer-exception-page)
* [异常处理程序](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/error-handling?view=aspnetcore-3.1#exception-handler-page)
* [HTTP 严格传输安全性 (HSTS)](https://docs.microsoft.com/zh-cn/aspnet/core/security/enforcing-ssl?view=aspnetcore-3.1#http-strict-transport-security-protocol-hsts)
* [HTTPS 重定向](https://docs.microsoft.com/zh-cn/aspnet/core/security/enforcing-ssl?view=aspnetcore-3.1)
* [静态文件](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/static-files?view=aspnetcore-3.1)
* `ASP.NET Core` [MVC](https://docs.microsoft.com/zh-cn/aspnet/core/mvc/overview?view=aspnetcore-3.1) 和 [Razor Pages](https://docs.microsoft.com/zh-cn/aspnet/core/razor-pages/?view=aspnetcore-3.1)

``` C#
public void Configure(IApplicationBuilder app,IWebHostEnvironment env){
    if(env.IsDevelopment()){
        // 开发人员异常页
        app.UseDeveloperExceptionPage();
    }else{
        // 异常处理
        app.UseExceptionHandler("/Error");
        // http严格传输安全协议管道，让浏览器强制使用HTTPS与网站进行通信
        app.UseHsts();
    }
    // HTTP请求重定向到HTTPS请求管道
    app.UseHttpRedirection();
    // 静态文件管道，为当前请求路径启用静态文件服务
    app.Usestaticfiles();
    // 断点路由管道
    app.UseRouting();
    // 授权管道
    app.UseAuthorization();
    // 将Razor页端点添加到断点路由中
    app.UseEndpoints(endpoints=>{
        endpoints.MaprazorPages();
    });
}
```

示例是用于`Razor Pages`；MVC于此类似。

## 使用 Startup过滤器扩展Startup

使用[IStartupFilter](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.hosting.istartupfilter):

* 在应用程序`Configure`中间管道的开头或结尾配置管道，而无需显示调用`Use{Middleware}`。`IStartupFilter`被`ASP.NET Core`用户管道开头添加默认值，而无需作者显示注册默认中间件。`IStartupFilter`允许维护应用程序的作者使用不同的组件调用`Use{Middleware}`。
* 创建一个`Configure`方法的管道，[IStartupFilter.Configure](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.hosting.istartupfilter.configure)可以将中间件设置为在库添加的中间件之前或之后运行。

`IStartupFilter` 实现 `Configure`，即接收并返回 `Action<IApplicationBuilder>`。 [IApplicationBuilder](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.builder.iapplicationbuilder) 定义用于配置应用请求管道的类。 有关详细信息，请参阅[使用 IApplicationBuilder 创建中间件管道](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.1#create-a-middleware-pipeline-with-iapplicationbuilder)。

每个 `IStartupFilter` 可以在请求管道中添加一个或多个中间件。 筛选器按照添加到服务容器的顺序调用。 筛选器可在将控件传递给下一个筛选器之前或之后添加中间件，从而附加到应用管道的开头或末尾。

下面的示例演示如何使用 `IStartupFilter` 注册中间件。 `RequestSetOptionsMiddleware` 中间件从查询字符串参数中设置选项值。

```C#
// 请求设置属性中间件
public class RequestSetOptionsMiddleware{
    private readonly RequestDelegate _next;

    public RequestSetOptionsMiddleware(RequestDelegate next){
        _next = next;
    }

    // Test with https://localhost:5001/Privacy/?option=Hello
    public async Task Invoke(HttpContext httpContext){
        var option = httpContext.Request.Query["option"];
        if(!string.IsNullOrWhiteSpace(option)){
            httpContext.Items["option"] = WebUtility.HtmlEncode(option);
        }

        await _next(httpContext);
    }
}
```

在 `RequestSetOptionsStartupFilter` 类中配置 `RequestSetOptionsMiddleware`：

``` C#
// Startup过滤器
public class RequestSetOptionsStartupFilter:IStartupFilter{
    public Action<IApplicationBuilder> Configure(Action<IApplicationBulider> next){
        return builder=>{
            builder.UseMiddleware<RequestSetOptionsMiddleware>();
            next(builder);
        }
    }
}
```

在 `ConfigureServices` 中的服务容器中注册 `IStartupFilter。`]

``` C#
public class Program{
    public static void Main(string[] args){
        CreateHostBuilder(args).Build().Run();
    }
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder=>{
             webBuilder.UseStartup<Startup>();
        })
        .ConfigureServices(services=>{
            services.AddTransient<Istartupfilter,RequestSetOptionsStartupFilter>();
        });
}
```

中间件执行顺序由 `IStartupFilter` 注册顺序设置：

* 多个`IStartupFilter`实现可能操作相同的对象。如果执行顺序很重要，请将`IStartupFilter`按照顺序注册，以匹配中间件正确的执行顺序。
* 类库可能包含一个或多个`IStartupFilter`实现的中间件，这些实现在向`IStartupFilter`注册其他应用中间之前或之后运行。若要在类库添加的`IStartupFilter`中间件前调用`IStartupFilter`中间件，请执行以下操作：
  * 在类库添加到服务容器之前定位服务注册。
  * 要在伺候调用，请在添加类库之后定位服务注册。

## 在启动时从外部程序集添加配置

通过 [IHostingStartup](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.hosting.ihostingstartup) 实现，可在启动时从应用 `Startup` 类之外的外部程序集向应用添加增强功能。 有关详细信息，请参阅 [在 ASP.NET Core 中使用承载启动程序集](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/host/platform-specific-configuration?view=aspnetcore-3.1)。
