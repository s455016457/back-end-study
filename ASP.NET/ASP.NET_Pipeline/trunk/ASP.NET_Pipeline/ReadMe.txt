此项目主要展示ASP.NET的管道模式
参考资料：http://www.cnblogs.com/artech/archive/2007/09/09/887528.html

IIS6的处理过程
浏览器发送请求
	|| Httprequest
  Http.Sys      Kernel Mode(核心模块)中
    || WAS
 W3wp.exe		User Model(用户模块)中

 在 User Mode 下，http.sys接收到一个基于aspx的http request
 然后他会根据IIS中的Metabase查看基于该请求的Application属于哪个Application Pool,
 如果Application Pool不存在，则创建之。
 否则直接将request发送到对应Application Pool的Queue中。
 每个Application Pool对应着一个Worker Process：w3wp.exe.
 在IIS Metabase中维护着application Pool和worker process的映射。
 WAS(Web Administative serice)根据这样一个映射，将存在于摸个Applocation Pool Queue的request传递到对应的worker process(如果没有，就创建这样一个进程)。
 在worker process初始化的时候，加载ASP.NET ISAPI,ASP.NET ISAPI进而加载CLR。
 最后通过AppManagerAPPDomainFactory的Create方法为Application创建一个Application Domain；
 通过ISAPIRuntime的ProcessRequest处理Request，进而将流程进入到Asp.Net Http Runtime Pipeline。

 现在真正的进入ASP.NET管理的范畴
			HttpRuntime
				||
		HttpApplicationFactory
				||
	  HttpApplication(Global.asax)
				||
HttpModules（Caching Module   Cusom Module   Session Module  Auth Module）
				||
HttpHandlers (Web Page   Web Service   ......)


HttpApplication本身并包含对Request的任何处理，他的工作方式是通过在不同阶段出发不同Event来调用我们注册的Event Hander。

下面列出了HttpApplication所有的Event，并按照触发的时间先后顺序排列：

BeginRequest：
AuthenticateRequest & Post AuthenticateRequest
AuthorizeRequest & Post AuthorizeRequest
ResolveRequestCache & Post ResolveRequestCache
PostMapRequestHandler:
AcquireRequestState & AcquireRequestState:
PreRequestHandlerExecute & Post RequestHandlerExecute:
ReleaseRequestState & Post ReleaseRequestState
UpdateRequestCache & PostUpdateRequestCache
EndRequest:



HttpModule

我们上面提到HttpApplication就是一个ASP.NET Application的体现，HttpApplication本身并不提供对Request的处理功能，而是通过在不同阶段出发不同的Event。
我们能做的只能是根据我们具体的需求将我们的功能代码作为Event Handler注册到需要的HttpApplication Event上面。
注册这些Event Handler，我们首先想到的肯定就直接在HttpApplication对应的Global.asax中定义我们的EventHandler好了。
这是最直接的办法，而且Global.asax提供一个简洁的方式是我们的实现显得简单：不需要向一般注册Event一样将Delegate添加到对应的Event上面，
而是直接通过方法名称和对应的Event匹配的方式直接将对应的方法作为相关的Event Handler。
比如Application_ AcquireRequestState就是AcquireRequestState Event handler。

但是这种方式在很多情况下却达不到我们的要求，更多地，
我们需要的是一种Plug-in的实现方式：
我们在外部定义一些Request Processing的功能，需要直接运用到我们的Application之中。
通过使用HttpModule封装这些功能模块，并将其注册到我们的Application的发式可以很简单的实现这种功能。

HttpModuls 配置示例：
	<httpModules>
		<add name="OutputCache" type="System.Web.Caching.OutputCacheModule" />
		<add name="Session" type="System.Web.SessionState.SessionStateModule" />
		<add name="WindowsAuthentication" type="System.Web.Security.WindowsAuthenticationModule" />
		<add name="FormsAuthentication" type="System.Web.Security.FormsAuthenticationModule" />
		<add name="PassportAuthentication" type="System.Web.Security.PassportAuthenticationModule" />
		<add name="UrlAuthorization" type="System.Web.Security.UrlAuthorizationModule" />
		<add name="FileAuthorization" type="System.Web.Security.FileAuthorizationModule" />
		<add name="ErrorHandlerModule" type="System.Web.Mobile.ErrorHandlerModule, System.Web.Mobile, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" />
	</httpModules>

注册在经典模式下运行的IIS 6.0和IIS 7.0的模块
<configuration>
  <system.web>
    <httpModules>
      <add name="HelloWorldModule" type="HelloWorldModule"/>
     </httpModules>
  </system.web>
</configuration>

在IIS 7.0集成模式下注册HTTP模块
<configuration>
  <system.webServer>
    <modules>
      <add name="HelloWorldModule" type="HelloWorldModule"/>
    </modules>
  </system.webServer>
</configuration>


 HttpHandler

如果说HttpModule关注的是所有Inbound Request的处理的话，
Handler确实关注基于某种类型的ASP.NET Resource的Request。
比如一个.apsx的Web Page通过一个System.Web.UI.Page来处理。
HttpHandler和他所处理的Resource通过Config中的system.web/handlers section来定义，
下面是Machine.config中的定义。
<httpHandlers>
    <add verb="*" path="trace.axd" type="System.Web.Handlers.TraceHandler" />
    <add verb="*" path="*.aspx" type="System.Web.UI.PageHandlerFactory" />
    <add verb="*" path="*.ashx" type="System.Web.UI.SimpleHandlerFactory" />
    <add verb="*" path="*.asmx" type="System.Web.Services.Protocols.WebServiceHandlerFactory, System.Web.Services, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" validate="false"/> 
    <add verb="*" path="*.rem" type="System.Runtime.Remoting.Channels.Http.HttpRemotingHandlerFactory, System.Runtime.Remoting, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" validate="false"/>
    <add verb="*" path="*.soap" type="System.Runtime.Remoting.Channels.Http.HttpRemotingHandlerFactory, System.Runtime.Remoting, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" validate="false"/>
    <add verb="*" path="*.asax" type="System.Web.HttpForbiddenHandler" />
    <add verb="*" path="*.ascx" type="System.Web.HttpForbiddenHandler" />
    <add verb="GET,HEAD" path="*.dll.config" type="System.Web.StaticFileHandler" />
    <add verb="GET,HEAD" path="*.exe.config" type="System.Web.StaticFileHandler" />
    <add verb="*" path="*.config" type="System.Web.HttpForbiddenHandler" />
    <add verb="*" path="*.cs" type="System.Web.HttpForbiddenHandler" />
    <add verb="*" path="*.csproj" type="System.Web.HttpForbiddenHandler" />
    <add verb="*" path="*.vb" type="System.Web.HttpForbiddenHandler" />
    <add verb="*" path="*.vbproj" type="System.Web.HttpForbiddenHandler" />
    <add verb="*" path="*.webinfo" type="System.Web.HttpForbiddenHandler" />
    <add verb="*" path="*.asp" type="System.Web.HttpForbiddenHandler" />
    <add verb="*" path="*.licx" type="System.Web.HttpForbiddenHandler" />
    <add verb="*" path="*.resx" type="System.Web.HttpForbiddenHandler" />
    <add verb="*" path="*.resources" type="System.Web.HttpForbiddenHandler" />
    <add verb="GET,HEAD" path="*" type="System.Web.StaticFileHandler" />
    <add verb="*" path="*" type="System.Web.HttpMethodNotAllowedHandler" />
</httpHandlers>

需要注意的是，我们不但可以单纯地定义一个实现了System.Web.IHttpHandler的Type，也可以定义一个实现了System.Web.IHttpHandlerFactory 的Type。
System.Web.UI.Page是一个典型的Httphandler，相信对此大家已经很熟悉了。
在最后还说说另一个典型的HttpHandler：System.Web.HttpForbiddenHandler，从名称我们不难看出，它用于那些禁止访问的Resource，
现在应该知道了为了Global.asax不同通过IIS访问了吧。