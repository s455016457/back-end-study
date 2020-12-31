官网：https://docs.microsoft.com/zh-cn/visualstudio/msbuild/walkthrough-creating-an-msbuild-project-file-from-scratch?view=vs-2015

一、创建最小的应用程序
	1、在空文件夹中创建helloworld.cs文件
	2、在文件中写入一段主程序代码
	3、在命令提示符下，键入 csc helloworld.cs 来生成应用程序。
	4、在命令提示符下，键入 helloworld 测试应用程序。
	5、显示的消息应为 Hello, world! 。
	6、在命令提示符下，键入 del helloworld.exe 删除应用程序。

二、创建最小的 MSBuild 项目文件
	既然有了最小的应用程序源文件，你就可以创建最小的项目文件来生成应用程序。 此项目文件包含以下元素：
		a、必需的根 Project 节点。
		b、用于包含项元素的 ItemGroup 节点。
		c、引用应用程序源文件的项元素。
		d、一个 Target 节点，用于包含生成应用程序所需的任务。
		e、一个 Task 元素，用于启动 Visual C# 编译器以生成应用程序。
	
	1、创建Helloworld.csproj文件
	2、添加Project 节点：
		<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003">  
		</Project> 
	3、在Project节点中添加ItemGroup节点
		<ItemGroup>  
		  <Compile Include="helloworld.cs" />  
		</ItemGroup>  
	4、在Project节点中添加Target节点，将该节点命名为Build
		<Target Name="Build">  
		</Target> 
	5、插入task元素作为Target节点的子元素：
		<Csc Sources="@(Compile)"/>  
	
==========================================================================================================
	添加生成属性
	
	在Project后添加PropertyGroup 元素
		<PropertyGroup>  
		  <AssemblyName>MSBuildSample</AssemblyName>  	程序集名称
		  <OutputPath>Bin\</OutputPath>  				编译输出路径
		</PropertyGroup> 
		
	将此任务添加到 Build 目标，置于 Csc 任务的前面：
		<MakeDir Directories="$(OutputPath)" Condition="!Exists('$(OutputPath)')" />
		
	将此 OutputAssembly 特性添加到 Csc 任务：
		<Csc Sources="@(Compile)" OutputAssembly="$(OutputPath)$(AssemblyName).exe" />
	
这将指示 Visual C# 编译器生成由 AssemblyName 属性命名的程序集，并将其放在由 OutputPath 属性命名的文件夹中。

==========================================================================================================

==========================================================================================================
	添加生成目标
	a、一个用户删除旧文件的Clean目标
	b、一个Rebuild目标，该目标使用DependsOnTargets特性，强制Clean任务在Build任务之前运行
	
	1、在项目文件中添加一下两个目标，置于Build目标的后面：
		<Target Name="Clean">									<!-- 定义删除文件目标 -->
			<Delete Files="$(OutputPath)$(AssemblyName).exe"/>
		</Target>
		<Target Name="Rebuild" DependsOnTargets="Clean;Build" /> <!-- 定义在Rebuild时，Clean在Build前执行 -->
	2、将次DefaultTargets特性添加到起始Project元素：
		<Project DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">  
		这会将 Build 目标设置为默认目标。

==========================================================================================================

==========================================================================================================
	以增量方式生成
	1、在项目文件中，将以下特性添加到起始 Build 目标：
		Inputs="@(Compile)" Outputs="$(OutputPath)$(AssemblyName).exe"
		它指定 Build 目标依赖于 Compile 项组中指定的输入文件，并且输出目标为应用程序文件。
	2、在命令提示符下，键入 msbuild /v:d 测试 Build 目标。
		/v:d 开关指定生成过程的详细说明。
		
		此时应显示以下各行：
			正在跳过目标“Build”，因为所有输出文件相对于输入文件而言都是最新的
			输入文件：HelloWorld.cs
			输出文件：BinMSBuildSample.exe

		MSBuild 将跳过 Build 目标，原因是自上次生成应用程序以来没有任何源文件发生更改。

==========================================================================================================