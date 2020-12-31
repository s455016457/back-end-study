using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity_MVC.DDD.Services.Interfaces;
using Unity_MVC.DDD.Services.Repostories;

namespace Unity_MVC.DDD.Services
{
    public abstract class BaseService : IBaseService
    {
        //[Unity.Dependency]
        //private MyRepostory myRepostory;
        ILog log =null;
        public BaseService()
        {
            log = LogManager.GetLogger<BaseService>();
            log.Debug($"{GetType().FullName}：实例化");
        }


        public void Dispose()
        {
            log.Debug($"{GetType().FullName}：销毁");
        }

        public void ToDoSomeThing()
        {
            log.Debug($"{GetType().FullName}：To Do SomeThing");
            //myRepostory.SaveChanges();
        }
    }
}