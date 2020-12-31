using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Unity.MVC5
{
    /// <summary>
    /// Unity依赖解析实现类
    /// 实现IDependencyResolver解析接口
    /// <see cref="https://docs.microsoft.com/zh-cn/dotnet/api/system.web.mvc.idependencyresolver?f1url=%3FappId%3DDev15IDEF1%26l%3DZH-CN%26k%3Dk(System.Web.Mvc.IDependencyResolver);k(TargetFrameworkMoniker-.NETFramework,Version%253Dv4.5);k(DevLang-csharp)%26rd%3Dtrue&view=aspnet-mvc-5.2"/>
    /// </summary>
    public class UnityDependencyResolver : IDependencyResolver
    {
        #region 私有属性
        /// <summary>
        /// 定义容器Key
        /// </summary>
        private const string containerKey = "preRequestContainer";
        /// <summary>
        /// 父级unity容器
        /// </summary>
        private IUnityContainer prentUnityContainer;

        private object lock_obj = new object();
        #endregion

        #region 成员属性
        /// <summary>
        /// 子容器
        /// 线程安全的
        /// </summary>
        public IUnityContainer ChildContainer
        {
            get
            {
                var childContainer = HttpContext.Current.Items[containerKey] as IUnityContainer;
                if (childContainer == null)
                {
                    lock (lock_obj)
                    {
                        childContainer = HttpContext.Current.Items[containerKey] as IUnityContainer;
                        if (childContainer == null)
                            HttpContext.Current.Items[containerKey] = childContainer = prentUnityContainer.CreateChildContainer();
                    }
                }
                return childContainer;
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="prentUnityContainer"></param>
        public UnityDependencyResolver(IUnityContainer prentUnityContainer)
        {
            this.prentUnityContainer = prentUnityContainer;
        }
        #endregion

        #region 实现接口方法
        /// <summary>
        /// 解析支持任意对象创建的一次注册的服务
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object GetService(Type serviceType)
        {
            return IsRegistered(serviceType) ? ChildContainer.Resolve(serviceType) : null;
        }

        /// <summary>
        /// 解析多次注册的服务。
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (IsRegistered(serviceType))
            {
                yield return ChildContainer.Resolve(serviceType);
            }

            foreach(var service in ChildContainer.ResolveAll(serviceType))
            {
                yield return service;
            }
        }
        #endregion

        #region 成员方法
        /// <summary>
        /// 销毁子容器
        /// </summary>
        public static void DisposeOfChildContainer()
        {
            var childContainer = HttpContext.Current.Items[containerKey] as IUnityContainer;
            if (childContainer != null)
            {
                childContainer.Dispose();
            }
        }
        #endregion

        #region 静态方法
        /// <summary>
        /// 类型已注册
        /// </summary>
        /// <param name="typeToCheck"></param>
        /// <returns></returns>
        public bool IsRegistered(Type typeToCheck)
        {
            var childContainer = HttpContext.Current.Items[containerKey] as IUnityContainer;

            var registered = true;
            // 如果类型接口或抽象类，则判断类型是否已注册
            if (typeToCheck.IsInterface || typeToCheck.IsAbstract)
            {
                if (childContainer == null) return false;
                registered = childContainer.IsRegistered(typeToCheck);

                // 如果未注册，则检查类型是否是泛型
                if (!registered && typeToCheck.IsGenericType)
                {
                    // 返回一个表示可用于构造当前泛型类型的泛型类型定义的 System.Type 对象。
                    var type = typeToCheck.GetGenericTypeDefinition();
                    registered = childContainer.IsRegistered(type);
                }
            }
            return registered;
        }
        #endregion
    }
}
