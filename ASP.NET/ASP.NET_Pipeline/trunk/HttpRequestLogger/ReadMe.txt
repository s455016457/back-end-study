请求日志Http模组

1、配置请求日志HttpModule
在webconfig中配置
  <system.webServer>
    <modules>
      <add name="RequestLogHttpModule" type="HttpRequestLogger.RequestLoggerHttpModule"/>
    </modules>
  </system.webServer>  
  <configSections>
    <section name="HttpRequestLogger_RequestLoggerConfig" type="HttpRequestLogger.Config.RepositoryConfigurationSectionHandler, HttpRequestLogger" />
  </configSections>
2、配置请求日志仓储
	a、使用默认仓储，需要在程序初始化时配置DefaultRequestLoggerRepository数据库连接。
		如在程序开始方法中输入以下代码：
		DefaultRequestLoggerRepository.SetConnectionString("数据库连接字符串");
	b、使用自定义仓储
		需要实现IRequestLoggerRepository接口。
		并在WebConfig文件中添加以下配置：		
	<HttpRequestLogger_RequestLoggerConfig>
		<Repository AssemblyName="MyWebApp.MyDataStore" FullName="MyWebApp.MyDataStore.MyRequestLoggerRepository" />
	</HttpRequestLogger_RequestLoggerConfig>
		或者设置HttpRequestLoggerConfig.CreateRequestLoggerRepositoryFunc的值：
		HttpRequestLoggerConfig.CreateRequestLoggerRepositoryFunc=()=>new MyWebApp.MyDataStore.MyRequestLoggerRepository();