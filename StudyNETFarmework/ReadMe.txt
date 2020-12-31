在插件项目属性--生成事件--后期生成事件命令行中设置：

rd /s /q $(SolutionDir)主程序名称\Plugins\插件名称

xcopy /s /y $(ProjectDir)Bin $(SolutionDir)主程序名称\Plugins\插件名称\bin\
xcopy /s /y $(ProjectDir)Views $(SolutionDir)主程序名称\Plugins\插件名称\Views\
xcopy /s /y $(ProjectDir)Content $(SolutionDir)主程序名称\Plugins\插件名称\Content\
xcopy /s /y $(ProjectDir)Scripts $(SolutionDir)主程序名称\Plugins\插件名称\Scripts\