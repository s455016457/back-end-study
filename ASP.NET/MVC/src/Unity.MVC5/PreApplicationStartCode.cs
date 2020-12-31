using Common.Logging;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Unity.MVC5
{
    /// <summary>
    /// 程序集运行时启动方法
    /// 需要设置PreApplicationStartMethodAttribute 属性
    /// 可以在AssemblyInfo.cs中设置属性
    /// </summary>
    public class PreApplicationStartCode
    {
        static ILog log = LogManager.GetLogger<PreApplicationStartCode>();
        private static bool isStart = false;
        public static void Start()
        {
            if (!isStart)
            {
                log.Debug("注册PreRequestLifeTimeHttpModel");
                // 注册HttpModel，程序启动时调用IHttpModel的Init方法进行初始化
                DynamicModuleUtility.RegisterModule(typeof(PreRequestLifeTimeHttpModel));
                isStart = true;
            }
        }
    }
}
