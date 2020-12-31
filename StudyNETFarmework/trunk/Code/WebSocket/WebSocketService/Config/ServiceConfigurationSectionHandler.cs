using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WebSocketService.Config
{
    internal class ServiceConfigurationSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            ServiceConfig config = new ServiceConfig();

            if (section == null || section.ChildNodes.Count == 0) return config;
            string assemblyString = string.Empty, fullName = string.Empty;

            foreach (XmlNode xn in section.ChildNodes)
            {
                switch (xn.Name)
                {
                    case "WebSocketSubProtocol":
                        config.WebSocketSubProtocol = xn.SelectSingleNode("@value").InnerText;
                        break;
                    case "Ip":
                        config.Ip = xn.SelectSingleNode("@value").InnerText;
                        break;
                    case "Origin":
                        config.Origin = xn.SelectSingleNode("@value").InnerText;
                        break;
                    case "Extensions":
                        config.Extensions = xn.SelectSingleNode("@value").InnerText;
                        break;
                    case "Version":
                        config.Version = xn.SelectSingleNode("@value").InnerText;
                        break;
                    case "MaxThreadCount":
                        var strMaxThreadCount = xn.SelectSingleNode("@value").InnerText;
                        int MaxThreadCount = 0;
                        if (!string.IsNullOrEmpty(strMaxThreadCount) && !string.IsNullOrEmpty(strMaxThreadCount) && !int.TryParse(strMaxThreadCount, out MaxThreadCount))
                        {
                            throw new Exception("节 MaxThreadCount 的值必须为数值！");
                        }
                        config.MaxThreadCount = MaxThreadCount;
                        break;
                    case "Port":
                        var strPort = xn.SelectSingleNode("@value").InnerText;
                        int Port = 0;
                        if (!string.IsNullOrEmpty(strPort) && !string.IsNullOrEmpty(strPort) && !int.TryParse(strPort, out Port))
                        {
                            throw new Exception("节 Port 的值必须为数值！");
                        }
                        config.Port = Port;
                        break;
                    case "WebSocketPulse":
                        var strWebSocketPulse = xn.SelectSingleNode("@value").InnerText;
                        int WebSocketPulse = 0;
                        if (!string.IsNullOrEmpty(strWebSocketPulse) && !string.IsNullOrEmpty(strWebSocketPulse) && !int.TryParse(strWebSocketPulse, out WebSocketPulse))
                        {
                            throw new Exception("节 WebSocketPulse 的值必须为数值！");
                        }
                        config.WebSocketPulse = WebSocketPulse;
                        break;
                }
            }


            return config;
        }
    }
}
