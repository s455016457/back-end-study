# .NET CORE 类库项目

用户身份、权限类库

## 开发语言

* .NETCore

## 创建项目

``` NET Core CLI
dotnet new classlib -o IdentityNetCore
```

> 说明
>
> * 创建类库项目
> * `-o IdentityNetCore`参数使用应用的源文件创建名为Example1的目录

## 打开项目

``` NET Core CLI
code -r IdentityNetCore
```

## 编译项目

``` NET Core CLI
dotnet build
```

## 添加引用包日志

```NET CLI
dotnet add package microsoft.entityframeworkcore
```

## 添加引用包EntityFrameWork

```NET CLI
dotnet add package microsoft.extensions.logging
```

## 添加身份验证引用包

```NET CLI
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
```