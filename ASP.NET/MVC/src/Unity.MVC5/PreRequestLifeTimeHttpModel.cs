using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Unity.MVC5
{
    public class PreRequestLifeTimeHttpModel : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            // 请求结束时释放子容器
            context.EndRequest += (sender, even) => UnityDependencyResolver.DisposeOfChildContainer();
        }
    }
}
