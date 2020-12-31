在程序包管理器控制台中执行 Enable-Migrations -ContextTypeName MVCDbContext 命令，搭建数据库迁移
产生 Migrations\201810241231496_InitialCreate.cs文件和Migrations\Configuration.cs文件
执行 update-Database -Verbose 命令 将Migrations\Configuration.cs文件 中 Seed 方法中配置的数据更新到数据库

当实体新增、修改、删除属性后，执行update-Database -Verbose 命令同步数据

Web.config 中配置
<add name="DefaultConnection" connectionString="Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\MVCDB.mdf;Initial Catalog=MVCDB;Integrated Security=SSPI" providerName="System.Data.SqlClient" />
在项目中生成数据库MVCDB


elmah-mvc 异常日志包
输入地址：http://localhost:59542/elmah  查看日志


common.logging 库引入了一个简单的抽象，允许您在运行时选择特定的日志实现。

Common.Logging.Log4Net1213
Log4Net 1.2.13 日志框架的common.logging库绑定。
  <configSections>
      <sectionGroup name="common">
        <section name="logging" type="Common.Logging.ConfigurationSectionHandler, Common.Logging" />
      </sectionGroup>
  </configSections>
  <common>
    <logging>
      <factoryAdapter type="Common.Logging.Log4Net.Log4NetLoggerFactoryAdapter, Common.Logging.Log4Net1213">
        <arg key="configType" value="INLINE" />
        <!--<arg key="configType" value="FILE-WATCH" />
                <arg key="configFile" value="~/log4net.config" />-->
      </factoryAdapter>
    </logging>
  </common>

log4Net 日志记录
  <configSections>
      <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, log4net" />
  </configSections>  
  <log4net>
    <!--定义输出到文件中-->
    <!--定义输出到文件中-->
    <appender name="ASP.NET.MVC.Error.Appender" type="log4net.Appender.RollingFileAppender">
      <!--输出日志文件的路径-->
      <file value="Logs\ASP.NET.MVC.Error.log" />
      <!--输出日志时自动向后追加-->
      <appendToFile value="true" />
      <!--防止多线程时不能写Log,官方说线程非安全-->
      <lockingModel type="log4net.Appender.FileAppender+MinimalLock" />
      <!--置为true,当前最新日志文件名永远为file节中的名字-->
      <staticLogFileName value="false" />
      <!--日志以大小作为备份样式，还有一种方式是Date（日期）-->
      <rollingStyle value="size" />
      <countDirection value="-1" />
      <!--单个日志的最大容量,(可用的单位:KB|MB|GB)不要使用小数,否则会一直写入当前日志-->
      <maximumFileSize value="1MB" />
      <!--日志最大个数,都是最新的-->
      <maxSizeRollBackups value="10" />
      <datePattern value="&quot;.&quot;yyyy-MM-dd-HH-mm-ss&quot;.log&quot;" />
      <layout type="log4net.Layout.PatternLayout">
        <!--每条日志末尾的文字说明-->
        <!--<footer value="**************************************************************" />-->
        <!--输出格式-->
        <conversionPattern value="%date [%thread] %-5level %logger [%ndc] - %message%newline" />
      </layout>
      <filter type="log4net.Filter.LevelRangeFilter">
        <levelMin value="ERROR" />
        <levelMax value="FATAL" />
      </filter>
    </appender>
    <root>
      <level value="DEBUG" />
      <appender-ref ref="ASP.NET.MVC.All.Appender" />
    </root>
  </log4net>