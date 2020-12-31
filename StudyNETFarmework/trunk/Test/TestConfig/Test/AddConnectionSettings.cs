using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConfig.Test
{
    [ClassDiscrible("在配置文件中添加连接字符串配置")]
    public class AddConnectionSettings : ITest
    {
        public Task RunAsync()
        {
            return Task.Factory.StartNew(Test);
        }

        public void Test()
        {
            // 将当前应用程序的配置文件作为 System.Configuration.Configuration 对象打开
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // 获取 connection strings 配置数量
            int connStrCnt = ConfigurationManager.ConnectionStrings.Count;

            string csName = "ConnStr" + connStrCnt.ToString();

            // 创建一个connection string 节，并保存进配置文件

            // 创建一个connection string 节.
            ConnectionStringSettings csSettings = new ConnectionStringSettings(csName,
                    "LocalSqlServer: data source=127.0.0.1;Integrated Security=SSPI;Initial Catalog=aspnetdb",
                    "System.Data.SqlClient");

            ConnectionStringsSection csSection =config.ConnectionStrings;

            csSection.ConnectionStrings.Clear();
            csSection.ConnectionStrings.Add(csSettings);

            // 保存配置文件.
            config.Save(ConfigurationSaveMode.Modified);
        }
    }
}
