using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceDescriptorsFactory
{
    /// <summary>
    /// 描述应用程序中的单个服务。
    /// </summary>
    public class AppService
    {
        /// <summary>
        /// 服务全名
        /// </summary>
        /// <value></value>
        public string FullName { get; internal set; }
        /// <summary>
        /// 服务注册的生命周期
        /// </summary>
        /// <value></value>
        public string Lifetime { get;internal set; }
        /// <summary>
        /// 服务的实现类型
        /// </summary>
        /// <value></value>
        public string ImplementationType { get; internal set; }
    }

    /// <summary>
    /// 服务描述符接口
    /// </summary>
    public interface IServiceDescriptorsService{
        /// <summary>
        /// 返回引用程序所有服务
        /// </summary>
        /// <returns></returns>
        IEnumerable<AppService> GetServices();
    }

    public class ServiceDescriptorsService :IServiceDescriptorsService{
        private readonly IServiceCollection _serviceCollection;

        public ServiceDescriptorsService(IServiceCollection serviceCollection){
            _serviceCollection=serviceCollection;
        }

        public IEnumerable<AppService> GetServices(){
            foreach(var srv in _serviceCollection){
                yield return(new AppService(){
                    FullName=srv.ServiceType.FullName,
                    Lifetime=srv.Lifetime.ToString(),
                    ImplementationType=srv.ImplementationType?.FullName
                });
            }
        }
    }
}