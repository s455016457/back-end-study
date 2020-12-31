# [`PreApplicationStartMethodAttribute`](https://docs.microsoft.com/zh-cn/dotnet/api/system.web.preapplicationstartmethodattribute?view=netframework-4.5.2)提供对应用程序启动的扩展支持

该属性可以让Web程序启动时执行我们制定的方法（在该方法中我们可以配置程序/组件所需的参数）。

> 当应用程序定义了多个`PreApplicationStartMethodAttribute`，应用程序不保证`PreApplicationStartMethodAttribute`的调用顺序。 因此，应将每个注册的启动方法编码为隔离运行，且不应依赖于其他注册的启动方法的副作用。

## 构造函数参数

type `Type`

一个描述启动方法的类型的对象。

methodName `String`

没有返回值的空参数签名，必须是公共的，并且可以是实例方法或静态方法。

## 定义程序启动时执行的方法

```C#
namespace Unity.MVC5
{
    /// <summary>
    /// 程序集运行时启动方法
    /// 需要设置PreApplicationStartMethodAttribute 属性
    /// 可以在AssemblyInfo.cs中设置属性
    /// </summary>
    public class PreApplicationStartCode
    {
        public static void Start()
        {
            /***
                程序/组件一些注册或初始化操作
            ***/
        }
    }
}
```

## 在AssemblyInfo.cs中注册程序运行时启动方法

```C#
// 设置应用程序启动时执行的方法
[assembly: PreApplicationStartMethod(typeof(Unity.MVC5.PreApplicationStartCode), "Start")]
```
