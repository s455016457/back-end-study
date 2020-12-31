using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HttpRequestLogger.Config
{
    internal class RepositoryConfigurationSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            if (section == null || section.ChildNodes.Count == 0) return new HttpRequestLoggerConfig();
            string assemblyString=string.Empty,fullName = string.Empty;

            foreach (XmlNode xn in section.ChildNodes)
            {
                switch (xn.Name)
                {
                    case "Repository":
                        fullName = xn.SelectSingleNode("@FullName").InnerText;
                        assemblyString = xn.SelectSingleNode("@AssemblyName").InnerText;
                        break;
                }
            }
            //if (string.IsNullOrEmpty(assemblyString))
            //    throw new Exception($"未配置节Repository中AssemblyName的值！");
            if (string.IsNullOrEmpty(fullName))
                throw new Exception($"未配置节Repository中FullName的值！");

            return new HttpRequestLoggerConfig
            {
                RepositoryFullName = fullName,
                RepositoryAssemblyName = assemblyString
            };
        }
    }
}
