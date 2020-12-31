using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConfig.Test
{
    [ClassDiscrible("在配置文件中添加自定义节配置")]
    public class AddUserActionOptLogConfig : ITest
    {
        public Task RunAsync()
        {
            return Task.Factory.StartNew(Test);
        }

        public void Test()
        {
            {
                UserActionOptLogConfig config = UserActionOptLogConfig.GetUserActionOptLogConfig();

                config.ConnectionType = "System.Data.SqlClient.SqlConnection, System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
                config.ConnectionString = "Data Source=.;Initial Catalog=SWERP_ORDER;User ID=sa;Password=sa@2008;";
                config.CommandText = @"INSERT INTO [SWERP_ORDERSYS_ERRORLOG]([Date],[Thread],[Level],[Logger],[Message],[Exception]) 
VALUES (@log_date, @thread, @log_level, @logger, @message, @exception)";

                config.Parameters.Add("@log_date", "log_date");
                config.Parameters.Add("@thread", "thread");
                config.Parameters.Add("@log_level", "log_level");
                config.Parameters.Add("@logger", "logger");
                config.Parameters.Add("@message", "message");
                config.Parameters.Add("@exception", "exception");
                config.Parameters.Add(new UserActionOptLogParameterElement("Parametertest3", "Parametervalue4") { ParameterType = typeof(DateTime).FullName, Format = "yyyy-mm-dd hh:MM:ss" });

                config.Save();
            }
            {
                var config = UserActionOptLogConfig.GetUserActionOptLogConfig();

                Console.WriteLine($"连接类型： {config.ConnectionType}");
                Console.WriteLine($"连接配置字符串： {config.ConnectionString}");
                Console.WriteLine($"执行SQL： {config.CommandText}");
                foreach (UserActionOptLogParameterElement item in config.Parameters)
                {
                    Console.WriteLine($"参数名：{item.ParameterName}；参数类型：{item.ParameterType}；参数值：{item.ParameterValue}；格式：{item.Format}");
                }
            }
        }
    }

    public class UserActionOptLogConfig : ConfigurationSection
    {
        const string sectionName = "userActionOptLog", 
            connectionType= "connectionType", 
            connectionString= "connectionString",
            commandText= "commandText",
            parameters= "parameters";
        Configuration config;

        /// <summary>
        /// 必须要有不带参数的构造函数
        /// </summary>
        public UserActionOptLogConfig() { }

        private UserActionOptLogConfig(Configuration config)
        {
            this.config = config;
        }

        public static UserActionOptLogConfig GetUserActionOptLogConfig()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            object obj = config.Sections[sectionName];
            UserActionOptLogConfig section =null;
            if (obj == null)
            {
                section = new UserActionOptLogConfig(config);
                config.Sections.Add(sectionName, section);
            }
            else {
                section = obj as UserActionOptLogConfig;
            }
            return section;
        }

        public void Save()
        {
            config.Save(ConfigurationSaveMode.Modified);
        }
        [ConfigurationProperty(connectionType, DefaultValue = "", IsRequired = true, IsKey = true)]
        public string ConnectionType
        {
            get
            {
                return (string)this[connectionType];
            }
            set
            {
                this[connectionType] = value;
            }
        }
        [ConfigurationProperty(connectionString, DefaultValue = "", IsRequired = true)]
        public string ConnectionString
        {
            get
            {
                return (string)this[connectionString];
            }
            set
            {
                this[connectionString] = value;
            }
        }
        [ConfigurationProperty(commandText, DefaultValue = "", IsRequired = true)]
        public string CommandText
        {
            get
            {
                return (string)this[commandText];
            }
            set
            {
                this[commandText] = value;
            }
        }

        [ConfigurationProperty(parameters, IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(UserActionOptLogParameter), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public UserActionOptLogParameter Parameters
        {
            get
            {
                object urlsCollection = this[parameters];

                return urlsCollection == null ? new UserActionOptLogParameter(): urlsCollection as UserActionOptLogParameter;
            }
        }
    }    
    public class UserActionOptLogParameter : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new UserActionOptLogParameterElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((UserActionOptLogParameterElement)element).ParameterName;
        }

        public UserActionOptLogParameter this[int index]
        {
            get
            {
                return (UserActionOptLogParameter)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public void Add(UserActionOptLogParameterElement element)
        {
            BaseAdd(element);
        }

        public void Add(string Name, string Value)
        {
            Add(new UserActionOptLogParameterElement(Name, Value));
        }

        public void Remove(string Name)
        {
            BaseRemove(Name);
        }

        public void Clear()
        {
            BaseClear();
        }
    }

    public class UserActionOptLogParameterElement : ConfigurationElement
    {
        const string configName = "ParameterName", configValue = "ParameterValue",configType= "ParameterType",configFormat="Format";
        public UserActionOptLogParameterElement(string ParameterName, string ParameterValue)
        {
            this.ParameterName = ParameterName;
            this.ParameterValue = ParameterValue;
        }

        public UserActionOptLogParameterElement()
        {

        }

        [ConfigurationProperty(configName, DefaultValue = "", IsRequired = true, IsKey = true)]
        public string ParameterName
        {
            get
            {
                return (string)this[configName];
            }
            set
            {
                this[configName] = value;
            }
        }

        [ConfigurationProperty(configValue, DefaultValue = "", IsRequired = true)]
        public string ParameterValue
        {
            get
            {
                return (string)this[configValue];
            }
            set
            {
                this[configValue] = value;
            }
        }

        [ConfigurationProperty(configType, DefaultValue = "", IsRequired = true)]
        public string ParameterType
        {
            get
            {
                return (string)this[configType];
            }
            set
            {
                this[configType] = value;
            }
        }

        [ConfigurationProperty(configFormat, DefaultValue = "", IsRequired = true)]
        public string Format
        {
            get
            {
                return (string)this[configFormat];
            }
            set
            {
                this[configFormat] = value;
            }
        }
    }
}
