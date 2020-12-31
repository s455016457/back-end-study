using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace SignalRExercises.App_Start
{
    public class SignalRExercisesConfigHelper
    {
        /// <summary>
        /// 获取配置
        /// </summary>
        public static SignalRExercisesConfig SignalRExercisesConfig
        {
            get { return System.Configuration.ConfigurationManager.GetSection("SignalRExercisesConfig") as SignalRExercisesConfig; }
        }
    }

    public class SignalRExercisesConfigHandler : System.Configuration.IConfigurationSectionHandler
    {
        Lazy<SignalRExercisesConfig> lazy_config = new Lazy<SignalRExercisesConfig>();

        public object Create(object parent, object configContext, XmlNode section)
        {
            var config = lazy_config.Value;

            if (section == null || section.ChildNodes.Count == 0) return config;

            foreach(XmlNode node in section.ChildNodes)
            {
                switch (node.Name)
                {
                    case "DataBaseNameOrConnectionString":
                        config.DataBaseNameOrConnectionString = node.Value.ToString();
                        break;
                    default:
                        break;
                }
            }

            return config;
        }
    }
}