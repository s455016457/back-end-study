
using Microsoft.Practices.Unity;

namespace UnityService
{
    public static class UnityService
    {
        private static IUnityContainer _unity;
        private static object lock_obj = new object();

        public static void Start() {
            if (_unity == null)
            {
                lock (lock_obj)
                {
                    if (_unity == null)
                    {
                        _unity = new UnityContainer();
                        RegisterType();
                    }
                }
            }
        }

        public static IUnityContainer GetUnity()
        {
            return _unity;
        }
        
        private static void RegisterType()
        {
            //_unity.RegisterType<IDbContext, swerpEntities>(new InjectionConstructor("name=swerpEntities"));//LeftTimeMangger管理生命周期
            //_unity.RegisterType<IUnitOfWork, UnitOfWork>();
            ////_unity.RegisterType<IPoRepository, PoRepository>(new InjectionConstructor(SqlServerContextService.GetContext()));//使用一个参数的构造函数实例化对象，并传入指定参数
            //_unity.RegisterType<IPoRepository, PoRepository>();
            //_unity.RegisterType<IPOService, PoService>();
        }
    }
}
