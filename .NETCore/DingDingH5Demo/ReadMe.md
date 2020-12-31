# 钉钉H5微应用项目

## 开发语言

* .NETCore

## 创建项目

``` NET Core CLI
dotnet new webapp -o DingDingH5Demo
```

> 说明
>
> * 创建Web引用
> * `-o DingDingH5Demo`参数使用应用的源文件创建名为DingDingH5Demo的目录

## 打开项目

``` NET Core CLI
code -r DingDingH5Demo
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
