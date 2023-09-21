CREATE TABLE [dbo].[DbQuery]
(
	[Id] INT identity(1,1) NOT NULL PRIMARY KEY,
	[Name] varchar(100),
	[TableId] int not null,	
	[TableDestinoId] Int,
	[Where] varchar(100),
	Foreign key (TableId) references dbo.DbTables(Id),
	Foreign key (TableDestinoId) references dbo.DbTableStg(Id)
)
