CREATE TABLE [dbo].[DbTables]
(
	[Id] INT identity(1000,1) PRIMARY KEY,
	[DbSourceId] int,
	[Name] varchar(100),
	foreign key (DbSourceId) references DbSource(Id)
)
