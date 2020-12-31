using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Unity_MVC.DDD.Services.Repostories
{
    public class MyRepostory:IDisposable
    {
        ILog log = LogManager.GetLogger<MyRepostory>();
        public MyRepostory()
        {
            log.Debug("MyRepostory 实例化");
        }

        public void SaveChanges()
        {
            log.Debug("MyRepostory SaveChanges");
        }

        public void Dispose()
        {
            log.Debug("MyRepostory 销毁");
        }
    }
}