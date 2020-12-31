using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unity;
using Unity.Lifetime;
using Unity.MVC5;
using Unity_MVC.DDD.Services;
using Unity_MVC.DDD.Services.Interfaces;

namespace Unity_MVC.App_Start
{
    public static class UnityConfig
    {
        internal static IUnityContainer container;
        public static void RegisterComponents()
        {
            container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            // 注册一类即系程序
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            Register(container);
        }

        private static void Register(IUnityContainer unityContainer)
        {
            unityContainer.RegisterType<IRequestLiftTimeSerivce, RequestLiftTimeSerivce>();
            unityContainer.RegisterSingleton<ISingletonService, SingletonService>();
            unityContainer.RegisterType<ITransientLifetimeService, TransientLifetimeService>(TransientLifetimeManager.Instance);
        }
    }
}