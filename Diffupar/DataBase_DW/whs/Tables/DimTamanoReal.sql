CREATE TABLE [whs].[DimTamanoReal]
(
	ID int identity(1,1) primary key,
	[Code] varchar(100),
	[Name] varchar(200),
	[Createdate] datetime,
	[UpdateDate] datetime
)
