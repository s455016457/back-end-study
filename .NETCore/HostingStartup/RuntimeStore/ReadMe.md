# 运行时存储

## 创建console程序

执行命令

```CLI
dotnet new console -o RuntimeStore
```

## 添加引用包

```CLI
dotnet add package Microsoft.aspnetcore.app
dotnet add package Microsoft.netcore.app
```

## 设置引用源

```XML
  <PropertyGroup>
    <RestoreSuccess>../MyPacks;$(RestoreSuccess);https://api.nuget.org/v3/index.json</RestoreSuccess>
  </PropertyGroup>
```

## 设置引用

```XML
  <ItemGroup>
    <PackageReference Include="StartupDiagnostics" Version="1.0.0-*" />
  </ItemGroup>
```
