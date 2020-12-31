# 说明

托管启动程序集诊断类库

目标框架设置为`notcoreapp3.1`

```xml
  <PropertyGroup>
    <TargetFramework>netcoreapp3.1</TargetFramework>
  </PropertyGroup>
```

## 添加引用程序集

```CLI
dotnet add package Micorsofe.AspNetCore.App
```

## 打包类库

```CLI
dotnet pack -o ../MyPacks
```
