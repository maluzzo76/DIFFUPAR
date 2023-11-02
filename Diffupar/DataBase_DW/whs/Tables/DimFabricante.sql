CREATE TABLE [whs].[DimFabricante]
(
	ID int identity(1,1) primary key,
	[Code] varchar(50),
	[Name] varchar(200),
	[CreateDate] datetime,
	[UpdateDate] datetime
)
