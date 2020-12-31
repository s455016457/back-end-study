官网地址 https://babeljs.io/

ReactJS.NET项目提供对.NET中的Babel的支持
Babel是一个JavaScript编译器
Babel 是一个广泛使用的 ES6 转码器，可以将 ES6 代码转为 ES5 代码，从而在现有环境执行。这意味着，你可以用 ES6 的方式编写程序，又不用担心现有环境是否支持。


ReactJS.NET JavaScriptEngineSwitcher.ChakraCore 模式需要安装Microsoft Visual C++ 2015
下载地址 https://www.microsoft.com/zh-CN/download/details.aspx?id=53840


需要安装包
JavaScriptEngineSwitcher.Core
React.Core

JavaScriptEngineSwitcher.ChakraCore
JavaScriptEngineSwitcher.ChakraCore.Native.win-x64
JavaScriptEngineSwitcher.ChakraCore.Native.win-x86
JavaScriptEngineSwitcher.ChakraCore.Native.win8-arm

可选包： 使用V8模式
JavaScriptEngineSwitcher.V8

项目配置文件<project>节中添加配置：
	<ItemGroup Condition=" $(RuntimeFrameworkVersion) == '' ">
		<None Include="$(MSBuildThisFileDirectory)runtimes\win8-arm\native\ChakraCore.dll">
		  <Link>arm\ChakraCore.dll</Link>
		  <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
		  <Visible>False</Visible>
		</None>
		<None Include="$(MSBuildThisFileDirectory)runtimes\win-x86\native\ChakraCore.dll">
			<Link>x86\ChakraCore.dll</Link>
			<CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
			<Visible>False</Visible>
		</None>
		<None Include="$(MSBuildThisFileDirectory)runtimes\win-x64\native\ChakraCore.dll">
			<Link>x64\ChakraCore.dll</Link>
			<CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
			<Visible>False</Visible>
		</None>
	</ItemGroup>
并将 JavaScriptEngineSwitcher.ChakraCore.Native.win8-arm、JavaScriptEngineSwitcher.ChakraCore.Native.win-x86、JavaScriptEngineSwitcher.ChakraCore.Native.win-x64 包中 runtimes 文件夹复制到项目中

问题论坛
https://github.com/reactjs/React.NET/issues/409



