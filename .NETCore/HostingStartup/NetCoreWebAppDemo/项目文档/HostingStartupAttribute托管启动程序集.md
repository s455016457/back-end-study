# [HostingStartupAttribute托管启动程序集](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/host/platform-specific-configuration?view=aspnetcore-3.1)

[IHostingstartup](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.hosting.ihostingstartup)实现在应用程序启动时从外部程序集中添加功能到应用程序中。

## HostingStartup属性

[HostingStartup](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.hosting.hostingstartupattribute)属性指示存在要在运行时激活的托管启动程序集。

自动扫描条目程序集或包含`Startup`类的程序集以查找`HostingStartup`属性。在运行时从[WebHostDefaults.HostingStartupAssembliesKey](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.hosting.webhostdefaults.hostingstartupassemblieskey)中的配置加载用于查找`HostingStartup`属性的程序集列表。在[WebHostDefaults.HostingStartupExcludeAssembliesKey](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.hosting.webhostdefaults.hostingstartupexcludeassemblieskey)中加载用于排除的程序集列表。

在下面示例中，托管启动程序集命名空间为`StartupEnhancement`，托管启动代码类为`StartupEnhancementHostingStartup`:

```C#
[assembly:Hostingstartup(typeof(StartupEnhancement.StartupEnhancementHostingStartup))]
```

`HostingStartup`属性通常位于托管启动程序集的`IHostingStartup`实现类文件中。

## 发现已加载的托管启动程序集

要发现加载的托管启动程序集，请启用日志记录并检查应用程序的日志。当加载程序集时出现错误，异常会被记录，已加载的托管启动程序集会在Debug级别被记录，并会记录所有异常。

## 禁用自定加载托管启动程序集

使用以下任何一种方法都可以禁用自动加载托管启动程序集：

* 阻止所有托管启动程序集加载，请设置以下任何一个为`ture`或`1`：
  * 阻止托管启动主机配置设置：
  
    ```C#
        public static IHostBuilder CreateHostBuilder(string[] args)=>{
            Host
            .CreatedefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder=>{
                webBuilder
                .Usesetting(WebHostDefaults.PreventHostingStartupKey,"true")
                .UseStartup<Startup>();
            });
        }
    ```

  * `ASPNETCORE_PREVENTHOSTINGSTARTUP`环境变量
* 阻止指定的托管启动程序集加载，以下任何一个设置为已分号分割的托管启动程序集，以阻止启动：
  * 主机配置阻止特定的托管启动程序集

    ```C#
        public static IhostBuilder createHostBuilder(string[] args)=>{
            Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder=>{
                webBuilder
                .UseSetting(
                    WebHostDefaults.HostingStartupExcludeAssembliesKey,"{ASSEMBLY1;ASSEMBLY2;...}"
                )
            });
        }
    ```

    * `ASPNETCORE_HOSTINGSTARTUPEXCLUDEASSEMBLIES`环境变量

如果在主机配置中和环境变量中都设置了，则主机设置控制加载行为。

使用主机设置或环境变量禁用托管启动程序集将全局禁用该程序集，并可能禁用应用程序的一些特性。

## 类库

托管启动增强可以在类库中提供，库包含一个`HostingStartup`属性。

[示例代码](https://github.com/dotnet/AspNetCore.Docs/tree/master/aspnetcore/fundamentals/host/platform-specific-configuration/samples/)包含一个Razor页面应用*HostingStartupApp* 和一个类库*HostingStartupLibrary*。类库：

* 包含一个实现了`IHostingStartup`接口的托管启动类`ServiceKeyInjection`。`ServiceKeyInjection`使用内存配置提供程序([AddInMemoryCollection](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.extensions.configuration.memoryconfigurationbuilderextensions.addinmemorycollection))添加一对服务字符串到应用程序的配置中。
* 包含一个`HostingStartup`属性，它定义了托管启动的命名空间和类。

`ServiceKeyInjection`类的[Configure](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.hosting.ihostingstartup.configure)方法使用[IWebHostBuilder](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.hosting.iwebhostbuilder)添加增强功能到应用程序中。

*HostingStartupLibrary/ServiceKeyInjection.css*:

```C#
[assembly:HostingStartup(typeof(HostingStartupLibrary.ServiceKeyInjection))]

namespace HostingStartupLibrary{
    public class ServiceKeyInjection:IHostingStartup{
        public void Configure(IWebHostBuilder builder){
            builder
            .ConfigureAppConfiguration(config=>{
                var dict = new Dictionary<string,string>{
                    {"DevAccount_FromLibrary","Dev_1111111-1111"},
                    {"ProdAcction_FromLibrary","PROD_2222222-2222"}
                };
                config.AddInMemoryCollection(dict);
            });
        }
    }
}
```

[示例代码](https://github.com/dotnet/AspNetCore.Docs/tree/master/aspnetcore/fundamentals/host/platform-specific-configuration/samples/)也包含了一个NuGet项目，它提供了另一个托管启动`HostingStartupPackage`。这个包具有与前面描述的类库相同的特征:

* 包含一个实现了`IHostingStartup`接口的托管启动类`ServiceKeyInjection`。`ServiceKeyInjection`添加一对服务字符串到应用程序配置中。
* 包含`HostingStartup`属性

*HostingStartupPackage/ServiceKeyInjeciton.cs*：

```C#
[assembly:HOstingStartup(typeof(HostingStartupPackage.ServiceKeyInjection))]

namespace HostingStartupPackage{
    public class ServiceKeyInjection:IHostingStartup{
        public void Configure(IWebHostBuilder builder){
            builder
            .ConfigureAppConfiguration(config=>{
                var dict = new Dictionary<string,string>{
                    {"DeveAccount_FromPackage","DEV_3333333-3333"},
                    {"ProdAccount_FromPackage","PROD_4444444-4444"}
                };

                config.AddInMemoryCollection(dict);
            });
        }
    }
}
```

应用程序的Index页面读取并呈现类库托管启动程序集设置的四个键的配置值:

*HostingStartupApp/Pages/Index.cshtml.cs*:

```C#
public class IndexModel:PageModel{
    public IndexModel(IConfiguration config){
        ServiceKey_Development_Library = config["DevAccount_FromLibrary"];
        ServiceKey_Production_Library=config["ProdAcction_FromLibrary"];
        ServiceKey_Development_Package = config["DevAccount_FromPackage"];
        ServiceKey_Production_Package = config["ProdAccount_FromPackage"];
    }

    public string ServiceKey_Development_Library{get;private set;}
    public string ServiceKey_Production_Library{get;private set;}
    public string ServiceKey_Development_Package { get; private set; }
    public string ServiceKey_Production_Package { get; private set; }

    public void OnGet(){

    }
}
```

## 没有入口点的控制台应用程序

这种方式仅仅适用于 .NET Core 应用程序，不适用于 .NET Framework。

动态托管启动增强不需要编译时引用激活提供给不包含入口点的控制程序，它包含一个`HostingStartup`属性。发布控制台应用程序将生成一个可以从运行时存储区使用的托管启动程序集。

一个没有入口点的控制台应用程在这个过程中序使用：

* 在托管启动程序集中使用托管启动需要一个依赖文件。依赖文件是可运行的应用程序资产，它是通过发布应用程序而产生的，而不是库。
* 库不能直接添加到[运行时包存储](https://docs.microsoft.com/zh-cn/dotnet/core/deploying/runtime-store)中，这需要一个以共享运行时为目标的可运行项目。

在创建一个动态托管启动:

* 托管启动程序集是从没有入口点的控制台应用程序创建的：
  * 包含一个实现了`IHostingStartup`接口的类
  * 包含一个[HostingStartup](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.hosting.hostingstartupattribute)属性去定义`IHostingStartup`实现类型。
* 控制台程序是已发布的，以获得托管启动的依赖项。发布控制台应用程序的一个后果是从依赖项文件中删除未使用的依赖项。
* 依赖项文件被修改为设置宿主启动程序集的运行时位置。
* 托管启动程序集合依赖项文件被放置在运行时包存储中(runtime package store)。要发现宿主启动程序集及其依赖项文件，将它们列在一对环境变量中。

控制台程序引用[Microsoft.AspNetCore.Hosting.Abstractions](https://www.nuget.org/packages/Microsoft.AspNetCore.Hosting.Abstractions/)包：

```XML
<Project Sdk="Microsoft.NET.Sdk">

    <ProjectGroup>
        <TargetFromework>netcoreapp3.0</TargetFromework>
    </ProjectGroup>

    <ItemGroup>
        <PackageReference Include="Microsoft.AspNetCore.Hosting.Abstractions" Version="3.0.0">
    </ItemGroup>
</Project>
```

[HostingStartup](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.hosting.hostingstartupattribute)属性将一个类标识为`IHostingStartup`的实现，以便在构建`IWebHost`时加载和执行。在如下面示例，命名空间为`StartupEnhancement`，类为`StartupEnhancementHostingStartup`：

```C#
[assembly:HostingStartup(typeof(StartupEnhancement.StartupEnhancementHostingStartup))]
```

一个实现`IHostingStartup`的类的[`Configure`](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.hosting.ihostingstartup.configure)方法使用[IWebHostBuilder](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.hosting.iwebhostbuilder)添加增强功能到应用程序。运行时在用户代码`Startup.Configure`之前调用托管启动程序集中的`IHostingStartup.Configure`，这允许用户代码覆盖由托管启动程序集中提供的任何配置。

```C#
namespace StartupEnhancement{
    public class StartupEnhancementHostingStartup:IHostingStartup{
        public void Configure(IWebHostBuilder builder){
            // 使用IWebHostBuilder添加应用程序增强功能
        }
    }
}
```

当构建一个`IHostingStartup`项目，开发文件(.deps.json)将程序集的运行时位置设置为bin文件夹：

```json
"targets":{
    ".NETCoreApp,Vresion=v3.0":{
        "StartupEnchancement/1.0.0":{
            "dependencies":{
                "Microsoft.AspNetcore.Hosting.Abstractions":"3.0.0"
            },
            "runtime":{
                "StartupEnhancement.dll":{}
            }
        }
    }
}
```

>只显示了文件的一部分。示例中的程序集名称为StartupEnhancement。

## 提供托管启动配置

有两中方法处理配置依赖，取决于你是想托管启动的配置优先还是应用程序的配置优先：

1. 提供配置到应用程序，使用[ConfigureAppConfiguration](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.hosting.webhostbuilder.configureappconfiguration)加载配置在应用程序的[ConfigureAppConfiguration](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.hosting.webhostbuilder.configureappconfiguration)委托后执行，使用这种方法，托管启动配置优先于应用程序的配置。
2. 提供配置到应用程序，使用[UseConfiguration](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.hosting.hostingabstractionswebhostbuilderextensions.useconfiguration)加载配置在应用程序的[ConfigureAppConfiguration](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.hosting.webhostbuilder.configureappconfiguration)委托前执行，使用这种方法，应用程序的配置优先于托管启动配置。

```C#
public class ConfigurationInjection:IHostingStartup
{
    public void Configure(IWebHostBuilder builder){
        Dictionary<string,string> dict;

        builder.ConfigureAppConfiguration(config=>{
            dict = new Dictionary<string,string>{
                {"ConfigurationKey1","From IHostingStartup:比应用程序的配置优先级更高"}
            };

            config.AddInMemoryCollection(dict);
        });

        dict = new Dictionary<string,string>{
            {"ConfigurationKey2","From IHostingStartup:应用程序的配置的优先级更高"}
        };

        var builtConfig = new ConfigurationBuilder()
        .AddInMemoryCollection(dict)
        .Build();

        builder.UseConfiguration(builtConfig);
    }
}
```

## 指定托管启动程序集

无论是类库还是控制台提供托管启动，都要在环境变量ASPNETCORE_HOSTINGSTARTUPASSEMBLIES中指定托管启动程序集名。环境变量是一个用分号分隔的程序集列表。

只有拥有`HostingStartup`属性的托管启动程序集才能被扫描。
如示例应用程序，`HostingStartupApp`，为了发现前面描述的托管启动，环境变量被设置为以下值:

```cmd
HostingStartupLibrary;HostingStartupPackage;StartupDiagnostics
```

一个托管启动程序集也能设置使用托管启动程序集主机配置设置：

```C#
public static IHostBuilder CreateHostBuilder(string[] args){
    Host
    .CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder=>{
        webBuilder.UseSetting(webHostDefaults.HostingStartupAssembliesKey,"{ASSEMBLY1;ASSEMBLY2;...}")
        .UseStartup<Startup>();
    });
}
```

> 当有多个托管启动程序集时，[Configure](https://docs.microsoft.com/zh-cn/dotnet/api/microsoft.aspnetcore.hosting.ihostingstartup.configure)方法按列出程序集的顺序执行。

## 激活

托管启动激活属性有：

* [Runtime store](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/host/platform-specific-configuration?view=aspnetcore-3.1#runtime-store)：激活不需要编译时引用来激活。示例应用程序的托管启动程序集合和依赖文件放在*deployment*文件夹中，以促进托管启动部署到多级环境中。*deployment*文件夹还包含一个PowerShell脚本，该脚本在部署系统上创建或修改环境变量，以启用托管启动。
* 激活需要编译时引用
  * [NuGet package](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/host/platform-specific-configuration?view=aspnetcore-3.1#nuget-package)
  * [Project bin folder](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/host/platform-specific-configuration?view=aspnetcore-3.1#project-bin-folder)

## Runtime store

托管启动实现被放在[runtime store](https://docs.microsoft.com/zh-cn/dotnet/core/deploying/runtime-store)中。增强的应用程序不需要程序集的编译时引用。

在托管启动构造之后，将使用manifest项目文件和[dotnet store](https://docs.microsoft.com/zh-cn/dotnet/core/tools/dotnet-store)命令生成一个runtime store。

```.NET Core CLI
 dotnet store --manifest {MANIFEST FILE} --runtime {FUNTIME IDENTIFIER} --output {OUT PUT LOCATION} --skip-optimization
```

在示例应用程序(*RuntimeStore* project)中，使用下面的命令：

``` .NET Core CLI
dotnet store --manifest store.manifest.csproj --runtime win7-x64 --output ./deployment/store --skip-optimization
```

要让运行时发现runtime store，要在DOTNET_SHARED_STORE环境变量中添加runtime store的位置。

### 修改并放置托管启动程序的依赖项文件

要在没有包引用的情况下激活增强，使用`additionalDeps`指定对运行时的其他依赖项。

`additionalDeps`允许你：

* 通过提供一组额外的`.deps.json`文件来扩展应用程序的库图，以便在启动时与应用程序自己的`.deps.json`文件合并。
* 使托管启动程序集可被发现和加载。

产生额外的依赖文件推荐的方法是:

1. 对上一节引用的runtime store manifest文件执行`dotnet publish`。
2. 从库和生成的 *.deps.json* 文件的`runtime`部分中删除manifest引用。

在示例项目中，`store.manifest/1.0.0`属性从`target`和`libraries`部分中删除:

``` JSON
{
  "runtimeTarget": {
    "name": ".NETCoreApp,Version=v3.0",
    "signature": ""
  },
  "compilationOptions": {},
  "targets": {
    ".NETCoreApp,Version=v3.0": {
      "store.manifest/1.0.0": {
        "dependencies": {
          "StartupDiagnostics": "1.0.0"
        },
        "runtime": {
          "store.manifest.dll": {}
        }
      },
      "StartupDiagnostics/1.0.0": {
        "runtime": {
          "lib/netcoreapp3.0/StartupDiagnostics.dll": {
            "assemblyVersion": "1.0.0.0",
            "fileVersion": "1.0.0.0"
          }
        }
      }
    }
  },
  "libraries": {
    "store.manifest/1.0.0": {
      "type": "project",
      "serviceable": false,
      "sha512": ""
    },
    "StartupDiagnostics/1.0.0": {
      "type": "package",
      "serviceable": true,
      "sha512": "sha512-xrhzuNSyM5/f4ZswhooJ9dmIYLP64wMnqUJSyTKVDKDVj5T+qtzypl8JmM/aFJLLpYrf0FYpVWvGujd7/FfMEw==",
      "path": "startupdiagnostics/1.0.0",
      "hashPath": "startupdiagnostics.1.0.0.nupkg.sha512"
    }
  }
}
```

将 *.deps.json* 文件放到以下位置:

```txt
{ADDITIONAL DEPENDENCIES PATH}/shared/{SHARED FRAMEWORK NAME}/{SHARED FRAMEWORK VERSION}/{ENHANCEMENT ASSEMBLY NAME}.deps.json
```

* {ADDITIONAL DEPENDENCIES PATH}: 位置添加到DOTNET_ADDITIONAL_DEPS环境变量。
* {SHARED FRAMEWORK NAME}：此附加依赖项文件需要共享框架。
* {SHARED FRAMEWORK VERSION}: 最想共享框架版本
* {ENHANCEMENT ASSEMBLY NAME}：增强的程序集名称。

在示例应用程序( *RuntimeStore* 项目)中，附加依赖文件放置在下面的位置：

```txt
deployment/additionalDeps/shared/Microsoft.AspNetCore.App/3.0.0/StartupDiagnostics.deps.json
```

为了允许时发现runtime store位置，附件引用文件位置必须添加到`DOTNET_ADDITIONAL_DEPS`环境变量中。

在示例应用程序(*RuntimeStore* 项目)中，使用[PowerShell](https://docs.microsoft.com/zh-cn/powershell/scripting/powershell-scripting)脚本完成构建runtime store和产生附加引用文件。

> 如何在各个系统中设置环境变量，请见[Use Multiple environments](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/environments?view=aspnetcore-3.1)

### 部署

为了促进托管启动部署到个环境，示例应用程序在published输出中创建了一个 *deployment* 文件夹，文件夹包含：

* 托管启动runtime store。
* 托管启动引用文件
* 一个PowerShell脚本，在部署系统上从管理员PowerShell命令提示中运行脚本，以创建或修改ASPNETCORE_HOSTINGSTARTUPASSEMBLIES, DOTNET_SHARED_STORE和DOTNET_ADDITIONAL_DEPS以支持激活托管启动。

## NuGet包

可以在NuGet包中提供一个托管启动增强。包中有一个`HostingStartup`属性。包提供的托管启动类型可以使App使用以下任何一种方式：

* 增强的应用程序的项目文件在应用程序的项目文件中为托管启动创建了一个包引用（一个编译时引用）。 有了编译时引用，承载启动程序集及其所有依赖项都会合并到应用的依赖项文件 (.deps.json) 中。 此方法适用于已发布到 [nuget.org ](https://www.nuget.org/)的承载启动程序集包。
* 托管启动的依赖文件可以提供给增强型App，就像在[Runtime store](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/host/platform-specific-configuration?view=aspnetcore-3.1#runtime-store)章节（没有编译时引用）描述的一样。

更多有关NuGet包和runtimestore的信息，请见以下文章：

* [如何使用跨平台工具创建一个NuGet包](https://docs.microsoft.com/zh-cn/dotnet/core/deploying/creating-nuget-packages)
* [发布包](https://docs.microsoft.com/zh-cn/nuget/create-packages/publish-a-package)
* [运行时包存储](https://docs.microsoft.com/zh-cn/dotnet/core/deploying/runtime-store)

## 项目bin文件夹

一个托管启动增强可以由增强的应用程序中的bin部署的程序集提供。由程序集提供的托管启动类型使用以下方法之一提供给应用程序:

* 增强应用程序项目文件为托管启动进行了程序集引用（一个编译时引用）。托管启动程序集及其所有依赖项都会合并到应用的依赖项文件 (*.deps.json*) 中。当部署方案要求对托管启动程序集（*.dll* 文件）进行编译时引用并将程序集移动到以下任一位置时，此方法适用：
  * 进行中的项目
  * 进行中的项目可访问的位置
* 托管启动程序的依赖项文件对增强的应用程序可用，如Runtime store中所述(没有编译时引用)。
* 当以.NET Framework为目标时，程序集可以在默认的加载上下文中加载，这在.NET Framework中意味着程序集位于以下任意一个位置:
  * 应用程序基路径：应用程序的可执行文件(*.exe*)所在的*bin*文件夹。
  * 全局程序集缓存（GAC）：GAC 存储多个 .NET Framework 应用共享的程序集。 有关详细信息，请参阅[如何：将程序集安装到 .NET Framework 文档中的全局程序集缓存中](https://docs.microsoft.com/zh-cn/dotnet/framework/app-domains/how-to-install-an-assembly-into-the-gac)。

## 示例代码

[示例代码](https://github.com/dotnet/AspNetCore.Docs/tree/master/aspnetcore/fundamentals/host/platform-specific-configuration/samples/)演示了托管启动实现方案：

* 两个托管启动程序集(类库)分别设置一对内存中的配置键值对:
  * NuGet包(*HostingStartupPackage*)
  * 类库(HostingStartupLibrary)
* 从运行时存储部署程序集激活托管启动（*StartupDiagnostics*）。程序集在应用程序的Startup中添加两个中间件，以提供一下诊断信息：
  * 已注册的服务
  * 地址（scheme,host,path base,path query string）
  * 连接（remote IP,remote port,local IP,local port,client certificate）
  * 请求头
  * 环境变量

运行示例：

### 从NuGet包激活

1. 使用[dotnet pack](https://docs.microsoft.com/zh-cn/dotnet/core/tools/dotnet-pack)命令编译 *HostingStartupPackage*包。
2. 将包的程序集名称 *HostingStartupPackage*添加到环境变量ASPNETCORE_HOSTINGSTARTUPASSEMBLIES中。
3. 编译和运行应用程序。 增强型应用中存在包引用（编译时引用）。 应用项目文件中的 `<PropertyGroup>` 指定包项目的输出 (*../HostingStartupPackage/bin/Debug*) 作为包源。 这允许应用使用该包而无需将包上传到 [nuget.org](https://www.nuget.org/)。有关详细信息，请参阅 HostingStartupApp 项目文件中的说明。

    ```XML
    <PropertyGroup>
        <RestoreSources>$(RestoreSources);https://api.nuget.org/v3/index.json;../HostingStartupPackage/bin/Debug</RestoreSources>
    </PropertyGroup>
    ```

4. 观察由Index页呈现的服务配置键值是否与包的`ServiceKeyInjection.Configure`设置的值相匹配。

如果更改了*HostingStartupPackage*项目并重新编译它，清理本地NuGet包缓存，以确保*HostingStartupApp*接受到更改后的包而不是本地缓存的旧包。执行以下[dotnet nuget locals](https://docs.microsoft.com/zh-cn/dotnet/core/tools/dotnet-nuget-locals)清理本地NuGet缓存：

```.NET Core CLI
dotnet nuget locals all --clear
```

> **注意**
>
> 设置环境变量后一定要重启电脑，否则设置的环境变量可能不生效
>
> 1. 托管启动类必须继承`IHostingStartup`类
> 2. 程序集中必须包含`HostingStartup`属性，该属性标记托管启动的类
> 3. 使用`dotnet pack`命令打包程序集
> 4. 在使用应用中，在项目文件中设置包的引用，配置如下
>
>     ```XML
>     <!-- 指定程序所需的引用 -->
>     <ItemGroup>
>       <PackageReference Include="HostingStartupPackage" Version="1.0.0" />
>     </ItemGroup>
>     <!-- 指定的包位置 -->
>     <PropertyGroup>
>       <RestoreSources>..\MyPacks;$(RestoreSources);https://api.nuget.org/v3/index.json;</RestoreSources>
>     </PropertyGroup>
>     ```

### 从类库中激活

1. 使用[dotnet build](https://docs.microsoft.com/zh-cn/dotnet/core/tools/dotnet-build)命令编译*HostingStartupLibrary*类库
2. 添加类库程序集的名称*HostingStartupLibrary*到ASPNETCORE_HOSTINGSTARTUPASSEMBLIES环境变量。
3. 通过将*HostingStartupLibrary.dll*文件从类库的编译输出复制到应用程序的bin/Debug文件夹，将类库的程序集部署到应用程序中。
4. 编译并运行应用程序。应用程序项目文件中的`<ItemGroup>`引用类库的程序集(*.\bin\Debug\netcoreapp3.0\HostingStartupLibrary.dll*) (编译时引用)。请参阅*HostingStartupApp*项目文件中的注释。

    ```XML
    <ItemGroup>
      <Reference Include=".\bin\Debug\netcoreapp3.0\HostingStartupLibrary.dll">
        <HintPath>.\bin\Debug\netcoreapp3.0\HostingStartupLibrary.dll</HintPath>
        <SpecificVersion>False</SpecificVersion>
      </Reference>
    </ItemGroup>
    ```

5. 观察由Index页呈现的服务配置键值是否与包的`ServiceKeyInjection.Configure`设置的值相匹配。

> **注意**
>
> 设置环境变量后一定要重启电脑，否则设置的环境变量可能不生效
>
> 1. 托管启动类必须继承`IHostingStartup`类
> 2. 程序集中必须包含`HostingStartup`属性，该属性标记托管启动的类
> 3. 使用`dotnet build`命令编译程序集
> 4. 在使用应用中，在项目文件中设置包的引用

### 从运行时存储部署激活程序集

1. *StartupDiagnostics*项目使用[PowerShell](https://docs.microsoft.com/zh-cn/powershell/scripting/powershell-scripting)修改它的*StartupDiagnostics.json*文件。PowerShell已在Windows Starting、Windows 7 SP1和Windows Server 2008R2 SP1上默认安装。其他平台上获得PowerShell，请见[安装各种版本的PowerShell](https://docs.microsoft.com/zh-cn/powershell/scripting/install/installing-powershell)。
2. 在*RuntimeStore*文件夹中扩展*build.sp1*脚本：
   * 在*obj\packages*文件夹中产生`startupDiagnostics`包
   * 在Store中产生`startupDiagnostics`的runtime store。该脚本中的`dotnet store` 命令使用 win7-x64 [运行时标识符 (RID)](https://docs.microsoft.com/zh-cn/dotnet/core/rid-catalog) 将托管启动部署到 Windows。当为不同的运行时提供托管启动时，用正确的RID替代脚本中的37行。`StartupDiagnostics`的runtime store稍后会移动到将使用程序集的计算机上的用户的runtime store中或系统的runtime store中。`StartupDiagnostics`程序集的用户runtime store安装在 *.dotnet/store/x64/netcoreapp3.0/startupdiagnostics/1.0.0/lib/netcoreapp3.0/StartupDiagnostics.dll.*。
   * 在*additionalDeps*文件夹中产生`StartupDiagnostics`的`additionalDeps`。附加依赖稍后会移动到用户或系统的附加依赖中。用户`StartupDiagnostics`附加依赖安装在*.dotnet/x64/additionalDeps/StartupDiagnostics/shared/Microsoft.NETCore.App/3.0.0/StartupDiagnostics.deps.json*。
   * 将 *deploy.ps1* 文件放置在 *deployment* 文件夹中。
3. 在*deployment*文件夹运行*deploy.ps1*脚本。脚本附加：
   * `StartupDiagnostics`追加到ASPNETCORE_HOSTINGSTARTUPASSEMBLIES环境变量
   * 托管启动依赖路径（在RuntimeStore项目的*deployment*文件夹中）追加到DOTNET_ADDITIONAL_DEPS环境变量
4. 运行示例应用程序
5. 请求`/services`终端，查看应用程序注册的服务。请求`/diag`终端，查看诊断信息。

