using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConfig.Test
{
    [ClassDiscrible("在配置文件中添加自定义节配置")]
    public class AddCustomSection : ITest
    {
        public Task RunAsync()
        {
            return Task.Factory.StartNew(Test);
        }

        public void Test()
        {
            CustomSection customSection = CustomSection.GetCustomSection();
            customSection.Name += "name ";
            customSection.Value += "value ";
            customSection.Save();
        }
    }

    public class CustomSection : ConfigurationSection
    {
        const string configName = "Name";
        const string configValue = "Value";
        Configuration config;

        /// <summary>
        /// 必须要有不带参数的构造函数
        /// </summary>
        public CustomSection() { }

        private CustomSection(Configuration config)
        {
            this.config = config;
        }

        public static CustomSection GetCustomSection()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            CustomSection section = config.Sections["CustomSection"] as CustomSection;
            if (section == null)
            {
                section = new CustomSection(config);
                config.Sections.Add("CustomSection", section);

            }
            return section;
        }

        public void Save()
        {
            config.Save(ConfigurationSaveMode.Modified);
        }

        [ConfigurationProperty(CustomSection.configName, DefaultValue = "default", IsKey = true)]
        public string Name
        {
            get{ return this[configName] as string; }
            set { this[configName] = value; }
        }

        [ConfigurationProperty(CustomSection.configValue, DefaultValue = "default")]
        public string Value
        {
            get { return this[configValue] as string; }
            set { this[configValue] = value; }
        }

        
    }
}
