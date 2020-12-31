# .NET CORE ClassLibrary

.NET CORE 类库

> 该项目练习在NuGet中提供托管启动增强，类库包含一个`HostingStartup`属性

## 开发语言

* .NETCore

## 创建项目

``` NET Core CLI
dotnet new classlib -o HostingStartupPackage
```

## 打开项目

``` NET Core CLI
code -r HostingStartupPackage
```

## 添加引用

``` NET Core CLI
dotnet add package Microsoft.AspNetCore.Hosting.Abstractions
dotnet add package Microsoft.Extensions.Configuration
```

## 编译

``` NET Core CLI
dotnet build
```

## 打包类库

``` NET Core CLI
dotnet pack -o ../MyPacks
```

> 将类库打包到上级MyPacks文件夹中
