
-- 请求日志
IF EXISTS (SELECT 1 FROM SYSOBJECTS WHERE ID=OBJECT_ID(N'RequestLogger') AND OBJECTPROPERTY(ID,'IsTable')=1)
BEGIN
	DROP TABLE[RequestLogger]
END
GO
CREATE TABLE [RequestLogger](
	[Id]				[UNIQUEIDENTIFIER]			NOT NULL,			--ID
	[Url]				[VARCHAR](200)				NOT NULL,			--请求路径
	[Method]			[VARCHAR](20)				NOT NULL,			--请求方法
	[Stamp]				[DATETIME]					NOT NULL,			--请求时间戳
	[IsFile]			[BIT]						NOT NULL,			--是否是文件URL
	[Ip]				[VARCHAR](50)				NOT NULL,			--ip地址
	[TimeConsuming]		[DECIMAL](28,5)					NULL,			--耗时
	CONSTRAINT [PK_RequestLogger] PRIMARY KEY CLUSTERED(
		[Id] ASC
	) ON [PRIMARY]
)
GO

SELECT * FROM ITEMGRP