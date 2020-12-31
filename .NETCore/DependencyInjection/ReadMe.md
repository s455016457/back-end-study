# [ASP.NET Core中的依赖注入](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1)

`ASP.NET Core` 支持依赖注入(DI)软件设计模式，不管实现技术是[控制注入(IoC)](https://docs.microsoft.com/zh-cn/dotnet/standard/modern-web-apps-azure-architecture/architectural-principles#dependency-inversion)还是类之间依赖 `ASP.NET Core`都支持。

更多有关在MVC控制器中实现依赖注入的信息，请见[在ASP.NET Core中将依赖注入到控制器](https://docs.microsoft.com/zh-cn/aspnet/core/mvc/controllers/dependency-injection?view=aspnetcore-3.1)。

更多有关依赖注入的参数信息，请见[ASP.NET Core中的选项模式](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/configuration/options?view=aspnetcore-3.1)。

本主题提供了关于`ASP.NET Core`中的依赖注入的信息。关于在控制台程序中使用依赖注入的信息，请见[在.NET中使用依赖注入](https://docs.microsoft.com/zh-cn/dotnet/core/extensions/dependency-injection)。

[查看或下载示例代码](https://github.com/dotnet/AspNetCore.Docs/tree/master/aspnetcore/fundamentals/dependency-injection/samples)([如何下载](https://docs.microsoft.com/zh-cn/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-3.1#how-to-download-a-sample))。

## 依赖注入概述

*依赖*是其他对象对对象的依赖。检查下面的`MyDependency`类，该类拥有一个`WriteMessage`方法，其他的类依赖它：

```C#
public class MyDependency{
    public void WriteMessage(string message){
        Console.WriteLine($"MyDependency.WriteMessage called. Message:{message}");
    }
}
```

一个类可以创建`MyDependency`类的一个实例来使用它的`WriteMessage`方法。在下面的实例中，`MyDependency`类是`IndexModel`类的一个依赖：

```C#
public class IndexModel:PageModel{
    private readonly MyDependency _dependency = new MyDependency();

    public void OnGet(){
        _dependency.WriteMessage("IndexModel.OnGet create this message");
    }
}
```

`IndexModel`类创建并直接依赖`MyDependency`类。直接代码依赖，这样是有问题的，应该极力避免，原因如下：

* 如果要用不同的实现替换`MyDependency`，`IndexModel`类就必须修改。
* 如果`MyDependency`有依赖项，他们还必须由`IndexModel`类配置。在大型项目中有多个类依赖`MyDependency`，这样的配置代码将分散在应用程序中。
* 这种实现很难进行单元测试。 应用需使用模拟或存根 `MyDependency` 类，而该类不能使用此方法。

依赖注入通过以下方法解决了这些问题：

* 使用接口或基类抽象依赖项实现。
* 在服务容器中注册依赖。`ASP.NET Core`提供一个内置的服务容器[`IServiceProvider`](https://docs.microsoft.com/zh-cn/dotnet/api/system.iserviceprovider)。服务通常在应用程序的`Startup.configureServices`方法中注入。
* 将服务注入到使用它的类的构造函数中。 框架负责创建依赖关系的实例，并在不再需要时将其释放。

在[示例应用](https://github.com/dotnet/AspNetCore.Docs/tree/master/aspnetcore/fundamentals/dependency-injection/samples)中，`IMyDependency`接口定义`WriteMessage`方法：

```C#
public interface IMyDependency{
    void WriteMessage(string message);
}
```

该几口被具体的类型`MyDependency`实现：

```C#
public class MyDependency:IMyDependency{
    public void WriteMessage(string message){
        Console.WriteLine($"MyDependency.WriteMessage called. Message:{message}");
    }
}
```

这个示例应用使用具体类型`MyDependency`注册`IMyDependency`服务。[AddScoped](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.dependencyinjection.servicecollectionserviceextensions.addscoped)方法使用一个内生产生命周期（（单个请求的生命周期））注册服务。后面将介绍[服务生命周期](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1#service-lifetimes)。

```C#
public void ConfigureServices(IServiceCollection services){
    services.AddScoped<IMyDependency,MyDependency>();
    services.AddRazorPages();
}
```

在示例应用程序中，需要`IMyDependency`服务用于`WriteMessage`方法：

```C#
public class IndexModel2:PageModel{
    private readonly IMyDependency _myDependency;

    public IndexModel2(IMyDependency myDenpendency){
        _myDependency = myDenpendency;
    }

    public void OnGet(){
        _dependency.WriteMessage("IndexModel2.OnGet create this message");
    }
}
```

通过使用DI模式，控制器：

* 不使用具体类型`MyDependency`，仅使用它实现的接口`IMyDependency`。这样可以轻松地更改控制器使用的实现，而无需修改控制器。
* 不创建 `MyDependency` 的实例，这由 DI 容器创建。

`IMyDependency`接口的实现可以通过使用内置的日志API来改进:

```C#
public class MyDependency2:IMyDependency{
    private readonly ILogger<MyDependency2> _logger;

    public MyDependency2(ILogger<MyDependency2> logger){
        _logger = logger;
    }

    public void WriteMessage(string message){
        _logger.LogInformation($"MyDependency2.WriteMessage Message: {message}");
    }
}
```

修改`ConfigureServices`方法，注册新的`IMyDependency`实现：

```C#
public void ConfigureServices(IServiceCollection services){
    services.AddScoped<IMyDependency,MyDependency2>();

    service.AddRazorPages();
}
```

`MyDependency2`依赖于`ILogger<TCategoryName>`，它在构造函数中请求它。`ILogger<TCategoryName>`是一个[框架提供服务](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1#framework-provided-services)。

以链式方式使用依赖注入并不罕见。每个被请求的依赖项依次请求它自己的依赖项。容器解析图中的依赖关系并返回完全解析的服务。必须被解析的依赖关系的集合通常被称为“依赖关系树”、“依赖关系图”或“对象图”。

容器通过利用[（泛型）开放类型](https://docs.microsoft.com/zh-cn/dotnet/csharp/language-reference/language-specification/types#open-and-closed-types)解析 `ILogger<TCategoryName>`，而无需注册每个[（泛型）构造类型](https://docs.microsoft.com/zh-cn/dotnet/csharp/language-reference/language-specification/types#constructed-types)。

在依赖项注入术语中，服务：

* 通常是一个对象，他想其他对象提供服务，例如`IMyDependency`服务。
* 与web服务无关，尽管服务可能会使用到Web服务。

框架提供一个强健的[日志](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/logging/?view=aspnetcore-3.1)系统。 编写上述示例中的 `IMyDependency` 实现是用来演示基本的 DI，而不是来实现日志记录。 大多数应用都不需要编写记录器。 下面的代码演示如何使用默认日志记录，这不要求在 `ConfigureServices` 中注册任何服务：

```C#
public class AboutModel:PageModel{
    private readonly ILogger _logger;

    public AboutModel(ILogger<AboutModel> logger){
        _logger = logger;
    }
    public string Message{get;set;}

    public void OnGet(){
        Message = $"About page visited at {DateTime.UtcNow.ToLongTimeString()}";
        _logger.LogInformation(Message);
    }
}
```

使用前面的代码时，无需更新 ConfigureServices，因为框架提供[日志记录](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/logging/?view=aspnetcore-3.1)。

## 注入Startup的服务

服务可以注入`Startup`够着函数和`startup.Configure`方法。

是用泛型主机([IHostBuilder](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.hosting.ihostbuilder))时，只能将以下服务注入`Startup`构造函数：

* [IWebHostEnvironment](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.hosting.iwebhostenvironment)
* [IHostEnviroment](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.hosting.ihostenvironment)
* [IConfiguration](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.configuration.iconfiguration)

任何是用DI容器注册的服务都可以注入到`Startup.Configure`方法中：

```C#
public void Configure(IApplicationBuilder app,ILogger<StartUp> logger){
    // ?ToDo
}
```

更多信息请见[ASP.NET Core应用启动](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/startup?view=aspnetcore-3.1)和[访问StartUp中配置](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1#access-configuration-in-startup)。

## 是用扩展方法注册服务组

`ASP.NET Core`框架使用一种约定来注册一组相关服务。约定使用单个 `Add{GROUP_NAME}` 扩展方法来注册该框架功能所需的所有服务。 例如`<Microsoft.Extensions.DependencyInjection.MvcServiceCollectionExtensions.AddControllers>`扩展方法注册MVC控制器所需的服务。

下面的代码通过个人用户帐户由 Razor 页面模板生成，并演示如何使用扩展方法 AddDbContext 和 AddDefaultIdentity 将其他服务添加到容器中：

```C#
public void ConfigureServices(Iservicecollection services){
    services.AddDbContext<ApplicationDbContext>(options=>
        options.UseSqlserver(Configuration.GetConnectionString("DefaultConnection"));
    );
    services
        .AddDefaultIdentity<IdentityUser>(options=>options.SignIn.RequireConfirmeAccount=true)
        .AddEntityFrameworkStores<ApplicationDbContext>();
    services.AddRazorPages();
}
```

考虑以下ConfigureServices方法，它注册服务并配置选项:

```C#
public void ConfigureServices(IServicecollection services){
    // 配置位置服务
    services.Configure<PositionOptions>(Configuration.GetSection(PositionOptions.Position));
    // 配置颜色服务
    services.Configure<ColorOptions>(Configuration.GetSection(ColorOptions.Color));
    // 注册服务
    services.AddScoped<IMyDependency,MyDependency>();
    services.AddScoped<IMyDependency2,MyDependency2>();

    services.AddRazorPages();
}
```

可以将相关的注册组移动到扩展方法以注册服务。 例如，配置服务会被添加到以下类中：

```C#
using ConfigSample.Options;
using Microsoft.Extensions.Configuration;

//Dependency Injection  依赖注入
namespace Microsoft.extensions.DependencyInjection{
    // 我的配置服务集合扩展
    public static class MyConfigServiceCollectionExtensions{
        public static IServiceCollection AddConfig(this IServiceCollection services,IConfiguration config){
            services.Configure<PositionOptions>(config.GetSecion(PositionOptions.Position));
            services.Configure<ColorOptions>(config.GetSection(ColorOptions.Color));

            return Services;
        }
    }
}
```

其余的服务在类似的类中注册。下面的`ConfigureServices`方法使用新的方法去注册服务：

```C#
public void ConfigureServices(IServiceCollection serivces){
    services.AddConfig(Configuration)
        .AddMyDependencyGroup();

        services.AddRazorPages();
}
```

> **注意**
>
> 每个`services.Add{GROUP_NAME}`扩展方法添加并且可能配置服务。例如，[`AddControllesWithviews`](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.dependencyinjection.mvcservicecollectionextensions.addcontrollerswithviews)添加MVC控制器和视图所需的服务，[`AddRazorPages`](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.dependencyinjection.mvcservicecollectionextensions.addrazorpages)添加Razor页面所需的服务。我们建议应用程序遵循这个命名约定。 将扩展方法置于 [Microsoft.Extensions.DependencyInjection](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.dependencyinjection) 命名空间中以封装服务注册的组。

## 服务生命周期

可以使用以下任一生命周期注册服务：

* Transient 暂时
* Scoped    作用域
* Singleton 单例

下面的部分描述了前面的每一个生命周期。为每个注册的服务选择合适的生命周期。

### Transient 暂时

暂时生命周期服务在每次从服务容器中被请求时创建。这个生命周期最适合轻量级、无状态的服务。使用[AddTransient](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.dependencyinjection.servicecollectionserviceextensions.addtransient)注册暂时服务。

在处理请求的应用程序中，暂时服务在请求结束时被释放。

### Scoped  作用域

作用域生命周期为每个客户端请求（连接）创建一次服务。使用[AddScoped](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.dependencyinjection.servicecollectionserviceextensions.addscoped)注册作用域服务。

在处理请求的应用程序中，作用域服务在请求结束时被释放。

当使用Entity Framework Core时，[AddDbContext]扩展方法使用默认的作用域生命周期注册`DbContext`类型。

不要在单例中解析作用域服务。 当处理后续请求时，它可能会导致服务处于不正确的状态。 可以：

* 在作用域或暂时服务中解析一个单例服务
* 在其他的作用域或暂时服务解析一个作用域服务

默认情况，在开发环境中，从其他拥有更长什么周期的服务中解析一个服务会引发异常。更多信息请见[作用域验证](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1#sv)。

要在中间件中使用作用域服务，请使用以下方法之一:

* 将服务注入中间件的 `Invoke` 或 `InvokeAsync` 方法。使用[构造函数注入](https://docs.microsoft.com/zh-cn/aspnet/core/mvc/controllers/dependency-injection?view=aspnetcore-3.1#constructor-injection)将抛出一个运行时异常，因为它会强制作用域服务表现为单例服务。[生命周期和注册选项](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1#lifetime-and-registration-options)章节中的示例演示了`InvokeAsync`的用法。
* 使用[Factory-based中间件](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/middleware/extensibility?view=aspnetcore-3.1)，使用此方法注册的中间件在每个客户端请求(连接)时激活，它允许将作用域服务注入到中间件的`InvokeAsync`方法中。

更多信息请见[写入自定义ASP.NET Core中间件](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/middleware/write?view=aspnetcore-3.1#per-request-middleware-dependencies)。

### Sigleton 单例

创建单例生命周期服务情况如下：

* 第一次被请求
* 在向容器直接提供实现实例时由开发人员进行创建。 很少用到此方法。

每个后续的请求都是用相同的实例。如果应用程序需要单例行为，允许服务容器管理服务的生命周期，不要实现单例设计模式，也不要提供处理单例的代码。从容器解析的服务不应该被代码销毁。如果一个类型或工程被注册为单例，则容器自动销毁单例。

使用[AddSingleton](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.dependencyinjection.servicecollectionserviceextensions.addsingleton)注册单例。单例服务必须是线程安全的，并且通常在无状态服务中使用。

在处理请求的应用程序中，当[`ServiceProvider`](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.dependencyinjection.serviceprovider)在应用程序关闭时被释放时，单例服务被释放。因为内存没有被释放，直到应用程序关闭，因此请考虑单例服务的内存使用。

> **警告**
>
> 不要在单例中解析作用域服务。当在处理后续请求时，它可能会导致服务处于不正确的状态。 可以在周用于或暂时服务中解析单例服务。

## 服务注册方法

框架提供了服务注册扩展方法，这些方法在特定场景中非常有用:

方法|示例|自动对象(Object)释放|多种实现|传递参数
:----------------|:----|:----:|:----:|:----:
`Add{LIFETIME}<{SERVICE},{IMPLEMENTATION}>()`|`services.AddSingletion<IMyDep,MyDep>()`|Yes|Yes|No
`Add{LIFETIME}<{SERVICE}>(sp=>new{IMPLEMENTATION})`|`services.AddSingletion<IMyDep>(sp=>new MyDep());services.AddSingleton<IMyDep>(sp=>new MyDep(99));`|Yes|Yes|Yes
`Add{LIFETIME}<{IMPLEMENTATION}>()`|`services.Addsingleton<MyDep>();`|Yes|No|No
`AddSingleton<{SERVICE}>(new {IMPLEMENTATION})`|`services.AddSingleton<IMyDep>(new MyDep();services.AddSingleton<IMyDep>(new MyDep(99)));`|No|Yes|Yes
`AddSingleton(new {IMPLEMENTATION})`|`services.AddSingleton(new MyDep());services.AddSingleton(new MyDep(99));`|No|No|Yes

更多有关销毁的信息请见[服务销毁](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1#disposal-of-services)章节。在[模拟类型测试](https://docs.microsoft.com/zh-cn/aspnet/core/test/integration-tests?view=aspnetcore-3.1#inject-mock-services)时，通常使用多种实现方式。

框架也提供了`TryAdd{LIFETIME}`扩展方法，只有在还没有注册实现时才注册服务。

在下面示例中，调用`AddSingleton`注册`MyDependency`实现`IMyDependency`。 对 `TryAddSingleton` 的调用没有任何作用，因为 `IMyDependency` 已有一个已注册的实现：

```C#
services.AddSingleton<IMyDependency,MyDependency>();
// 以下代码无效:
services.TryAddSingleton<IMyDependency,DifferentDependency>();
```

有关信息请见：

* [TryAdd](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.dependencyinjection.extensions.servicecollectiondescriptorextensions.tryadd)
* [TryAddTransient](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.dependencyinjection.extensions.servicecollectiondescriptorextensions.tryaddtransient)
* [TryAddScoped](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.dependencyinjection.extensions.servicecollectiondescriptorextensions.tryaddscoped)
* [TryAddSingleton](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.dependencyinjection.extensions.servicecollectiondescriptorextensions.tryaddsingleton)

[TryAddEnumerable(ServiceDescriptor)](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.dependencyinjection.extensions.servicecollectiondescriptorextensions.tryaddenumerable)方法仅会在没有同一类型实现的情况下才注册该服务。 多个服务由`IEnumerable<{SERVICE}>`解析。在注册服务时，如果还没有添加相同类型的实例，开发人员应该添加一个实例。通常，库作者使用`TryAddEnumerable`来避免在容器中注册一个实现的多个副本。

在下面的示例中，第一次调用`TryAddEnumerable`将`MyDependency`注册为`IMyDependency1`的实现。第二次调用向`IMyDependency1`注册`MyDependency`。 第三次调用没有任何作用，因为 `IMyDependency1` 已有一个 `MyDependency` 的已注册的实现：

```C#
public interface IMyDependency1{}
public interface IMyDependency2{}
public class MyDependency:IMyDependency1,IMyDependency2{}

services.TryAddEnumerable(ServiceDescriptor.Singleton<IMyDependency1,MyDependency>());
services.TryAddEnumerable(ServiceDescriptor.Singleton<IMyDependency2,MyDependency>());
services.TryAddEnumerable(ServiceDescriptor.Singleton<IMyDependency1,MyDependency>());
```

服务注册通常与顺序无关，除了注册同一类型的多个实现时。

`IServiceCollection`是[ServiceDescriptor](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.dependencyinjection.servicedescriptor)对象的集合。以下示例演示如何通过创建和添加 ServiceDescriptor 来注册服务：

```C#
var myKey = Configuration["MyKey"];
var descriptor = new SerivceDescriptor(typeof(IMyDependency),sp=>new MyDependency(myKey),ServiceLifetime.Transient);

services.Add(descriptor);
```

内置 Add{LIFETIME} 方法使用同一种方式。 相关示例请参阅 [AddScoped源代码](https://github.com/dotnet/extensions/blob/v3.1.6/src/DependencyInjection/DI.Abstractions/src/ServiceCollectionServiceExtensions.cs#L216-L237)。

## 构造函数注入行为

服务可以使用一下方法解析：

* [IServiceProvider](https://docs.microsoft.com/zh-cn/dotnet/api/system.iserviceprovider)
* [ActivatorUtilities](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.dependencyinjection.activatorutilities)：
  * 创建未在容器中注册的对象。
  * 与框架功能一起使用，例如[标记帮助程序](https://docs.microsoft.com/zh-cn/aspnet/core/mvc/views/tag-helpers/intro?view=aspnetcore-3.1)、[MVC 控制器](https://docs.microsoft.com/zh-cn/aspnet/core/mvc/models/model-binding?view=aspnetcore-3.1)和模型绑定器。

构造函数可以接受非依赖关系注入提供的参数，但参数必须分配默认值。

当服务由 `IServiceProvider` 或 `ActivatorUtilities` 解析时，[构造函数注入](https://docs.microsoft.com/zh-cn/aspnet/core/mvc/controllers/dependency-injection?view=aspnetcore-3.1#constructor-injection)需要 public 构造函数。当服务由 `ActivatorUtilities` 解析时，[构造函数注入](https://docs.microsoft.com/zh-cn/aspnet/core/mvc/controllers/dependency-injection?view=aspnetcore-3.1#constructor-injection)要求只存在一个适用的构造函数。 支持构造函数重载，但其参数可以全部通过依赖注入来实现的重载只能存在一个。

## 实体框架上下文

默认情况下，使用[作用域生命周期](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1#service-lifetimes)将实体框架上下文添加到服务容器中，因为 Web 应用数据库操作通常将作用域为客户端请求。 要使用其他生命周期，请使用[AddDbContext](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.dependencyinjection.entityframeworkservicecollectionextensions.adddbcontext)重载来指定生命周期。 给定生命周期的服务不应该使用生命周期比服务生命周期端的数据库上下文（也就是说不应该出现服务还未销毁，而数据库上下文已被销毁的情况）。

## 生命周期和注册属性

演示服务生命周期及其注册选项之间的差异，考虑以下接口，它们将任务表示为具有标识符`OperationId`的操作。根据为以下接口配置操作服务的生命周期的方式，容器在类请求时提供相同或不同的服务实例：

```C#
public interface IOperation{
    string OperationId{get;}
}

public interface IOperationTransient:IOperation{}
public interface IOperationScoped:IOperation{}
public interface IOperationSingleton:IOperation{}
```

一下`Operation`类实现了前面的所有接口。`Operation`构造函数生成GUID，并将最后4个字符存储在`OperationId`属性中：

```C#
public class Operation:IOperationTransient,IOperationScoped,IOperationSingleton{
    public Operation(){
        OperationId=Guid.NewGuid().ToString()[^4..];
    }
    public string OperationId{get;}
}
```

`Startup.configureServices`方法创建多个符合生命周期名称的`Operation`实例：

```C#
public void ConfigureServices(IServiceCollection services){
    services.AddTransient<IOperationTransient,Operation>();
    service.AddScoped<IOperationScoped,Operation>();
    service.AddSingleton<IOperationSingleton,Operation>();

    services.AddRazorPages();
}
```

示例应用程序演示了请求内部和请求之间的对象生命周期。`IndexModel`和中间件请求每种`IOperation`类型，并为每种类型记录`OperationId`:

```C#
public class IndexModel:PageModel{
    private readonly ILogger _logger;
    private readonly IOperationTransient _transientOperation;
    private readonly IOperationScoped _scopedOperation;
    private readonly IOperationSingleton _singletonOperation;

    public IndexModel(ILogger<IndexModel> logger
                        ,IOperationTransient transientOperation
                        ,IOperationScoped scopedOperation
                        ,IOperationSingleton singletonOperation){
        _logger = logger;
        _transientOperation = transientOperation;
        _scopedOperation = scopedOperation;
        _singletionOperation = singletonOperation;
    }

    public void OnGet(){
        _logger.LogInformation("Transient:"+_transientOperation.OperationId);
        _logger.LogInformation("Scoped:"+_scopedOperation.OperationId);
        _logger.LogInformation("Singleton:"+_singletonOperation.OperationId);
    }
}
```

与 `IndexModel` 类似，中间件会解析相同的服务：

```C#
public class MyMiddleware{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly IOperationTransient _transientOperation;
    private readonly IOperationSingleton _singletonOperation;

    public MyMiddleware(RequestDelegate next,ILogger<MyMiddleware> logger
                        ,IOperationTransient transientOperation
                        ,IOperationSingleton singletonOperation){
        _next = next;
        _looger = logger;
        _transientOperation = transientOperation;
        _singletionOperation = singletonOperation;
    }

    public async Task Invokeasync(HttpContext context,IOperationScoped scopedOperation){
        _logger.LogInformation("Transient: " + _transientOperation.OperationId);
        _logger.LogInformation("Scoped: "    + scopedOperation.OperationId);
        _logger.LogInformation("Singleton: " + _singletonOperation.OperationId);

        await _next(context);
    }
}

// UseMyMiddleware扩展方法 用来启用 MyMiddleware中间件
public static class MyMiddlewareExtensions
{
    public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<MyMiddleware>();
    }
}
```

> 作用域服务必须在`InvokeAsync`方法中解析。

日志输出显示：

* 暂时对象始终不同。 `IndexModel` 和中间件中的临时 `OperationId` 值不同。
* 作用域对象对每个请求而言是相同的，但在请求之间不同。
* 单例对象对于每个请求是相同的。

若要减少日志记录输出，请在 `appsettings.Development.json` 文件中设置

```JSON
{
  "MyKey": "MyKey from appsettings.Developement.json",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "System": "Debug",
      "Microsoft": "Error"
    }
  }
}
```

> 将 `"Microsoft"`设置为 `"Error"`

## 从main中调用服务

使用 [IServiceScopeFactory.CreateScope](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.dependencyinjection.iservicescopefactory.createscope) 创建 [IServiceScope](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.dependencyinjection.iservicescope) 以解析应用范围内的作用域服务。 此方法可以用于在启动时访问有作用域的服务以便运行初始化任务。

以下示例演示如何访问作用域 `IMyDependency` 服务并在 `Program.Main` 中调用其 `WriteMessage` 方法：

```C#
public class Program{
    public static void Main(string[] args){
        var host = CreateHostBuilder(args).Build();

        using(var serviceScope = host.Services.createScope()){
            var services = servicesScope.ServiceProvider;

            try{
                var myDependency = services.GetRequiredService<IMyDependency>();
                myDependency.WriteMessage("Call services from main");
            }catch(Exception ex){
                var logger = services.GetRequiredService<ILogger<Program>()>();
                logger.LogErroe(ex,"An error occurred");
            }
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args){
        return Host.CreatedefaultBuilder(args)
                    .ConfigureWebHostDefaults(webBuilder=>{
                        webBuilder.UseStartup<Startup>();
                    });
    }
}
```

## 作用域验证

如果应用程序在[开发环境](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/environments?view=aspnetcore-3.1)中运行，并调用[CreateDefaultBuilder](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-3.1#default-builder-settings)以生产主机，默认服务提供程序会执行检查，以确认一下内容：

* 作用域服务不饿ngn从跟服务提供程序解析
* 作用域服务不能注入到单例中。

调用 BuildServiceProvider 时创建根服务提供程序。 在启动提供程序和应用时，根服务提供程序的生命周期对应于应用的生命周期，并在关闭应用时释放。

有作用域的服务由创建它们的容器释放。 如果作用域服务创建于根容器，则该服务的生命周期实际上提升至单例，因为根容器只会在应用关闭时将其释放。 验证服务作用域，将在调用 `BuildServiceProvider` 时收集这类情况。

有关详细信息，请参阅[作用域验证](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/host/web-host?view=aspnetcore-3.1#scope-validation)。

## 请求服务

`ASP.NET Core` 请求中可用的服务通过 `HttpContext.RequestServices` 集合公开。 当服务在请求内部被需要时，将从 `RequestServices` 集合解析服务及其依赖项。

> **备注**
>
> 与解析 `RequestServices` 集合中的服务相比，以构造函数参数的形式请求依赖项是更优先的选择。 这样生成的类更易于测试。

## 设计能够进行依赖关系注入的服务

在设计能够进行依赖注入的服务时：

* 避免有状态的、静态类和成员。 通过将应用设计为改用单例服务，避免创建全局状态。
* 避免在服务中直接实例化依赖类。 直接实例化会将代码耦合到特定实现。
* 不在服务中包含过多内容，确保设计规范，并易于测试。

如果一个类有过多注入依赖项，这可能表明该类拥有过多的责任并且违反了[单一责任原则 (SRP)](https://docs.microsoft.com/zh-cn/dotnet/standard/modern-web-apps-azure-architecture/architectural-principles#single-responsibility)。 尝试通过将某些职责移动到一个新类来重构类。 请记住，Razor Pages 页面模型类和 MVC 控制器类应关注用户界面问题。

## 服务释放

容器为其创建的 [IDisposable](https://docs.microsoft.com/zh-cn/dotnet/api/system.idisposable) 类型调用 [Dispose](https://docs.microsoft.com/zh-cn/dotnet/api/system.idisposable.dispose)。 从容器中解析的服务绝对不应由开发人员释放。 如果类型或工厂注册为单例，则容器自动释放单例。

在下面的示例中，服务由服务容器创建，并自动释放：

```C#
public class Service1:IDisposable{
    private bool _disposed;

    public void Write(string message){
        Console.WriteLine($"Service1:{message}");
    }

    public void Disposable(){
        if(_disposed)
            return;

        Console.WriteLine("Service1.Dispose");
        _disposed=true;
    }
}

public class Service2:IDisposable{
    private bool _disposed;

    public void Write(string message){
        Console.WriteLine($"Service2:{message}");
    }

    public void Disposable(){
        if(_disposed)
            return;

        Console.WriteLine("Service2.Dispose");
        _disposed=true;
    }
}

public interface IService3{
    void Write(string message);
}

public class Service3:IService3,IDisposable{
    private bool _disposable;

    public Service3(string myKey){
        MyKey=myKey;
    }

    public string MyKey{get;}

    public void Write(string message){
        Console.WriteLine($"Service3:{message},MyKey={MyKey}");
    }

    public void Dispose(){
        if(_disposable)
            return;

        Console.WriteLine("Service3.Dispose");
        _disposed=true;
    }
}
```

```C#
public void ConfigureServices(IServiceCollection services){
    services.AddScoped<Service1>();
    services.AddSingleton<Service2>();

    var myKey = Configuration["MyKey"];
    services.AddSingleton<IService3>(sp=>new Service3(myKey));

    services.AddRazorPages();
}
```

```C#
public class IndexModel:PageModel1{
    private readonly Service1 _service1;
    private readonly Service2 _service2;
    private readonly Service3 _service3;

    public IndexModel(Service1 service1,Service2 service2,Service3 service3){
        _service1=service1;
        _service2=service2;
        _service3=service3;
    }

    public void OnGet(){
        _service1.Write("IndexModel.OnGet");
        _service2.Write("IndexModel.OnGet");
        _service3.Write("IndexModel.OnGet");
    }
}
```

每次刷新索引页后，调试控制台显示以下输出：

```CLI
Service1: IndexModel.OnGet
Service2: IndexModel.OnGet
Service3: IndexModel.OnGet
Service1.Dispose
```

## 不由服务容器创建的服务

考虑下列代码：

```C#
public void ConfigureServices(IServiceCollection services){
    services.Addsingleton(new Service1());
    services.Addsingleton(new Service2());

    services.AddRazorPages();
}
```

在上述代码中：

* 服务实例不是由服务容器创建的。
* 框架不会自动释放服务。
* 开发人员负责释放服务。

## 暂时和共享实例的 IDisposable 指南

### 暂时、有限的生命周期

#### 方案

应用需要一个 [IDisposable](https://docs.microsoft.com/zh-cn/dotnet/api/system.idisposable) 实例，该实例在以下任一情况下具有暂时性生命周期：

* 在根作用域（根容器）内解析实例。
* 该实例应该在该作用域结束之前被释放

#### 解决方案

使用工厂模式在父作用域外创建实例。 在这种情况下，应用通常会使用一个 `Create` 方法，该方法直接调用最终类型的构造函数。 如果最终类型具有其他依赖项，则工厂可以：

* 在其构造函数中接收 [IServiceProvider](https://docs.microsoft.com/zh-cn/dotnet/api/system.iserviceprovider)。
* 使用 [ActivatorUtilities.CreateInstance](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.dependencyinjection.activatorutilities.createinstance) 在容器外部实例化实例，同时将容器用于其依赖项。

### 共享实例，有限的生命周期

#### 方案

应用需要跨多个服务的共享  [IDisposable](https://docs.microsoft.com/zh-cn/dotnet/api/system.idisposable)  实例，但  [IDisposable](https://docs.microsoft.com/zh-cn/dotnet/api/system.idisposable)  实例应具有有限的生命周期。

#### 解决方案

为实例注册作用域生命周期。 使用 [IServiceScopeFactory.CreateScope](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.dependencyinjection.iservicescopefactory.createscope) 创建新 [IServiceScope](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.dependencyinjection.iservicescope)。 使用作用域的 [IServiceProvider](https://docs.microsoft.com/zh-cn/dotnet/api/system.iserviceprovider) 获取所需的服务。 如果不再需要作用域，请将其释放。

## 默认服务容器替换

内置的服务容器旨在满足框架和大多数消费者应用的需求。 我们建议使用内置容器，除非你需要的特定功能不受它支持，例如：

* 属性注入
* 基于名称的注入
* 子容器
* 自定义生存期管理
* 对迟缓初始化的 `Func<T>` 支持
* 基于约定的注册

以下第三方容器可用于 `ASP.NET Core `应用：

* [Autofac](https://autofac.readthedocs.io/en/latest/integration/aspnetcore.html)
* [DryIoc](https://www.nuget.org/packages/DryIoc.Microsoft.DependencyInjection)
* [Grace](https://www.nuget.org/packages/Grace.DependencyInjection.Extensions)
* [LightInject](https://github.com/seesharper/LightInject.Microsoft.DependencyInjection)
* [Lamar](https://jasperfx.github.io/lamar/)
* [Stashbox](https://github.com/z4kn4fein/stashbox-extensions-dependencyinjection)
* [Unity](https://www.nuget.org/packages/Unity.Microsoft.DependencyInjection)

## 线程安全

创建线程安全的单例服务。 如果单例服务依赖于一个暂时服务，那么暂时服务可能也需要线程安全，具体取决于单例使用它的方式。

单个服务的工厂方法（例如 [AddSingleton<TService>(IServiceCollection, Func<IServiceProvider,TService>)](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.dependencyinjection.servicecollectionserviceextensions.addsingleton)的第二个参数）不必是线程安全的。 像类型 (`static`) 构造函数一样，它保证仅由单个线程调用一次。

## 建议

* 不支持基于`async/await` 和 `Task` 的服务解析。 由于 C# 不支持异步构造函数，因此请在同步解析服务后使用异步方法。
* 避免在服务容器中直接存储数据和配置。 例如，用户的购物车通常不应添加到服务容器中。 配置应使用 [选项模型](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/configuration/options?view=aspnetcore-3.1)。 同样，避免“数据持有者”对象，也就是仅仅为实现对另一个对象的访问而存在的对象。 最好通过 DI 请求实际项。
* 避免静态访问服务。 例如，避免将 [IApplicationBuilder.ApplicationServices](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.builder.iapplicationbuilder.applicationservices#Microsoft_AspNetCore_Builder_IApplicationBuilder_ApplicationServices) 捕获为静态字段或属性以便在其他地方使用。
* 使 DI 工厂保持快速且同步。
* 避免使用服务定位器模式。 例如，可以使用 DI 代替时，不要调用 [GetService](https://docs.microsoft.com/zh-cn/dotnet/api/system.iserviceprovider.getservice) 来获取服务实例：

**不正确：**

```C#
public class MyClass{
    public void MyMethod(){
        var optionsMonitor = _services.GetService<IOptionsMonitor<MyOptions>>();

        var option =optionsMonitor.CurrentValueOption;
        ····
    }
}
```

**正确：**

```C#
public class MyClass{
    private readonly IOptionMonitor<MyOptions> _optionsMonitor;

    public MyClass(IOptionMonitor<MyOptions> optionsMonitor){
        _optionsMonitor = optionsMonitor;
    }

    public void MyMethod(){
        var options = optionsMonitor.CurrentValueOption;
        ····
    }
}
```

* 另一个要避免的是服务定位器变体是注入一个在运行时解析依赖关系的工厂。 这两种做法混合了[控制反转](https://docs.microsoft.com/zh-cn/dotnet/standard/modern-web-apps-azure-architecture/architectural-principles#dependency-inversion)策略。
* 避免静态访问HTTPContext(例如：[IHttpContextAccessor.HttpContext](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.http.ihttpcontextaccessor.httpcontext#Microsoft_AspNetCore_Http_IHttpContextAccessor_HttpContext))
* 避免在`CongfigureServices`[中调用BuildServiceProvider](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.dependencyinjection.servicecollectioncontainerbuilderextensions.buildserviceprovider).。当开发人员想在`CongfigureServices`中解析服务时，通常会调用`BuildServiceProvider`。例如，假设`LoginPath`从配置中加载。避免采用以下方法：
    ![错误示例](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/dependency-injection/_static/badcodex.png?view=aspnetcore-3.1)
    在上图中，选择 services.BuildServiceProvider 下的绿色波浪线将显示以下 ASP0000 警告：
    > ASP0000 从应用程序代码调用“BuildServiceProvider”会导致创建单一实例服务的其他副本。 考虑依赖项注入服务等替代项作为“Configure”的参数。

    调用 BuildServiceProvider 会创建第二个容器，该容器可创建残缺的单一实例并导致跨多个容器引用对象图。
    获取 LoginPath 的正确方法是使用选项模式对 DI 的内置支持：

    ```C#
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie();

        services.AddOptions<CookieAuthenticationOptions>(
                            CookieAuthenticationDefaults.AuthenticationScheme)
            .Configure<IMyService>((options, myService) =>
            {
                options.LoginPath = myService.GetLoginPath();
            });

        services.AddRazorPages();
    }
    ```

* 可释放的暂时性服务由容器捕获以进行释放。 如果从顶级容器解析，这会变为内存泄漏。
* 启用范围验证，确保应用没有捕获范围内服务的单一实例。 有关详细信息，请参阅[作用域验证](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1#scope-validation)。

像任何一组建议一样，你可能会遇到需要忽略某建议的情况。 例外情况很少见，主要是框架本身内部的特殊情况。

DI 是静态/全局对象访问模式的替代方法。 如果将其与静态对象访问混合使用，则可能无法意识到 DI 的优点。

## DI 中适用于多租户的推荐模式

[Orchard Core](https://github.com/OrchardCMS/OrchardCore) 是用于在 ASP.NET Core 上构建模块化多租户应用程序的应用程序框架。 有关详细信息，请参阅 [Orchard Core 文档](https://docs.orchardcore.net/en/dev/)。

请参阅 [Orchard Core 示例](https://github.com/OrchardCMS/OrchardCore.Samples)，获取有关如何仅使用 Orchard Core Framework 而无需任何 CMS 特定功能来构建模块化和多租户应用的示例。

## 框架提供的服务

`Startup.ConfigureServices`方法注册应用使用的服务，包括 `Entity Framework Core` 和 `ASP.NET Core MVC` 等平台功能。 提供给 `ConfigureServices` 的 `IServiceCollection` 具有框架定义的服务（具体取决于[主机配置方式](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/?view=aspnetcore-3.1#host)）。 对于基于 `ASP.NET Core` 模板的应用，该框架会注册 250 个以上的服务。

下表列出了框架注册的这些服务的一小部分：

服务类型|生命周期
:--|:--:
[Microsoft.AspNetCore.Hosting.Builder.IApplicationBuilderFactory](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.hosting.builder.iapplicationbuilderfactory)|暂时
[IHostApplicationLifetime](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.hosting.ihostapplicationlifetime)|单例
[IWebHostEnvironment](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.hosting.iwebhostenvironment)|单例
[Microsoft.AspNetCore.Hosting.IStartup](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.hosting.istartup)|单例
[Microsoft.AspNetCore.Hosting.IStartupFilter](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.hosting.istartupfilter)|暂时
[Microsoft.AspNetCore.Hosting.Server.IServer](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.hosting.server.iserver)|单例
[Microsoft.AspNetCore.Http.IHttpContextFactory](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.http.ihttpcontextfactory)|暂时
[Microsoft.Extensions.Logging.ILogger<TCategoryName>](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.logging.ilogger-1)|单例
[Microsoft.Extensions.Logging.ILoggerFactory](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.logging.iloggerfactory)|单例
[Microsoft.Extensions.ObjectPool.ObjectPoolProvider](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.objectpool.objectpoolprovider)|单例
[Microsoft.Extensions.Options.IConfigureOptions<TOptions>](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.options.iconfigureoptions-1)|暂时
[Microsoft.Extensions.Options.IOptions<TOptions>](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.options.ioptions-1)|单例
[System.Diagnostics.DiagnosticSource](https://docs.microsoft.com/zh-cn/dotnet/api/system.diagnostics.diagnosticsource)|单例
[System.Diagnostics.DiagnosticListener](https://docs.microsoft.com/zh-cn/dotnet/api/system.diagnostics.diagnosticlistener)|单例
