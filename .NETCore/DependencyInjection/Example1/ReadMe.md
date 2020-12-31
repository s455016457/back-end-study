# .NET CORE MVC 项目

## 开发语言

* .NETCore

## 创建项目

``` NET Core CLI
dotnet new mvc -o Example1
```

> 说明
>
> * 创建Web引用
> * `-o Example1`参数使用应用的源文件创建名为Example1的目录

## 打开项目

``` NET Core CLI
code -r Example1
```

## 运行项目

``` NET Core CLI
dotnet watch run
```

> **注意**
>
> `Startup.css`类`Configure`方法中`app.UseHttpsRedirection();`将HTTP请求重定向到HTTPS，使用HTTP请求需要注释该代码

## 安装/删除HTTPS开发证书

### 安装HTTPS开发证书

``` CRL
dotnet dev-certs https --trust
```

### 删除HTTPS开发证书

``` CRL
dotnet dev-certs https --clean
```

### ERR_HTTP2_INADEQUATE_TRANSPORT_SECURITY 异常解决方法

在`appsettings.json`配置文件中加入以下配置，配置终端默认协议为`HTTP1`请求。

``` json
    "Kestrel": {
        "EndpointDefaults": {
            "Protocols": "Http1"
        }
    }
```

配置后的`appsettings.json`配置文件如下：

``` json
{
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft": "Warning",
            "Microsoft.Hosting.Lifetime": "Information"
        }
    },
    "AllowedHosts": "*",
    "Kestrel": {
        "EndpointDefaults": {
            "Protocols": "Http1"
        }
    }
}
```

## 添加Log4Net日志

### 添加应用程序

```NET CLI
dotnet add package Microsoft.Extensions.Logging.Log4Net.AspNetCore --version 3.1.3
```

### 在启动配置中添加Log4Net

```C#
using Microsoft.Extensions.Logging;

public class Startup
{
    //...

    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
        //...

        loggerFactory.AddLog4Net(); // << Add this line
        app.UseMvc();

        //...
    }
}
```

### 添加`logNet.config`配置文件

```XML
<?xml version="1.0" encoding="utf-8" ?>
<log4net>
  <appender name="DebugAppender" type="log4net.Appender.DebugAppender" >
    <layout type="log4net.Layout.PatternLayout">
      <conversionPattern value="%date [%thread] %-5level %logger - %message%newline" />
    </layout>
  </appender>
  <appender name="RollingFile" type="log4net.Appender.RollingFileAppender">
    <file value="example.log" />
     <appendToFile value="true" />
    <maximumFileSize value="1024KB" />
    <maxSizeRollBackups value="10" />
    <layout type="log4net.Layout.PatternLayout">
        <conversionPattern value="%date %5level %logger.%method [%line] - MESSAGE: %message%newline %exception" />
    </layout>
  </appender>
  <root>
    <level value="ALL"/>
    <appender-ref ref="DebugAppender" />
    <appender-ref ref="RollingFile" />
  </root>
</log4net>
```

更多配置请见[configuration documentation](https://github.com/huorswords/Microsoft.Extensions.Logging.Log4Net.AspNetCore/blob/develop/doc/CONFIG.md)

## 添加Newtonsoft.Json包

```NET CLI
dotnet add package Newtonsoft.Json
```

## 应用环境

在`Properties/launchSettings.json`中修改应用环境，ASP.NET Core 在应用启动时读取环境变量 ASPNETCORE_ENVIRONMENT，并将该值存储在 IHostingEnvironment.EnvironmentName 中。 ASPNETCORE_ENVIRONMENT 可设置为任意值，但框架提供三个值：

* Development
* Staging
* Production（默认值）

### 开发环境配置

```json
{
    "iisSettings": {
        "windowsAuthentication": false,
        "anonymousAuthentication": true,
        "iisExpress": {
            "applicationUrl": "http://localhost:26371",
            "sslPort": 44308
        }
    },
    "profiles": {
        "IIS Express": {
            "commandName": "IISExpress",
            "launchBrowser": true,
            "environmentVariables": {
                "ASPNETCORE_ENVIRONMENT": "Development"
            }
        },
        "Example1": {
            "commandName": "Project",
            "launchBrowser": true,
            "applicationUrl": "https://localhost:5001;http://localhost:5000",
            "environmentVariables": {
                "ASPNETCORE_ENVIRONMENT": "Development"
            }
        }
    }
}
```

### 演示环境

```json
{
    "iisSettings": {
        "windowsAuthentication": false,
        "anonymousAuthentication": true,
        "iisExpress": {
            "applicationUrl": "http://localhost:26371",
            "sslPort": 44308
        }
    },
    "profiles": {
        "IIS Express": {
            "commandName": "IISExpress",
            "launchBrowser": true,
            "environmentVariables": {
                "ASPNETCORE_ENVIRONMENT": "Staging"
            }
        },
        "Example1": {
            "commandName": "Project",
            "launchBrowser": true,
            "applicationUrl": "https://localhost:5001;http://localhost:5000",
            "environmentVariables": {
                "ASPNETCORE_ENVIRONMENT": "Staging"
            }
        }
    }
}
```

### 产品环境

```json
{
    "iisSettings": {
        "windowsAuthentication": false,
        "anonymousAuthentication": true,
        "iisExpress": {
            "applicationUrl": "http://localhost:26371",
            "sslPort": 44308
        }
    },
    "profiles": {
        "IIS Express": {
            "commandName": "IISExpress",
            "launchBrowser": true,
            "environmentVariables": {
                "ASPNETCORE_ENVIRONMENT": "Production"
            }
        },
        "Example1": {
            "commandName": "Project",
            "launchBrowser": true,
            "applicationUrl": "https://localhost:5001;http://localhost:5000",
            "environmentVariables": {
                "ASPNETCORE_ENVIRONMENT": "Production"
            }
        }
    }
}
```

## 添加应用包EntityFrameWork

```NET CLI
dotnet add package entityframework
```
