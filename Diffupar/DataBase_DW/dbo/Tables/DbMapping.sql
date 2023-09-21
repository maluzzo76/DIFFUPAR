CREATE TABLE [dbo].[DbMapping]
(
	[Id] INT identity(1,1) NOT NULL PRIMARY KEY,
	[QueryId] int not null,	
	[ColumnSourceId] int,
	[ColumnDestino] varchar(100),	
	Foreign key (QueryId) references dbo.DbQuery(Id),
	Foreign key (ColumnSourceId) references dbo.DbColumns(Id)
)
