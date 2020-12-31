# .NET CORE ClassLibrary

.NET CORE 类库

> 该项目练习在类库中提供托管启动增强，类库包含一个`HostingStartup`属性

## 开发语言

* .NETCore

## 创建项目

``` NET Core CLI
dotnet new classlib -o HostingStartupLibrary
```

## 打开项目

``` NET Core CLI
code -r HostingStartupLibrary
```

## 添加引用

``` NET Core CLI
dotnet add package Microsoft.AspNetCore.Hosting
dotnet add package Microsoft.Extensions.Configuration
```

## 编译

``` NET Core CLI
dotnet build
```

