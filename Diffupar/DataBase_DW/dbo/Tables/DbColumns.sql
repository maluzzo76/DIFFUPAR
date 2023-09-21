CREATE TABLE [dbo].[DbColumns]
(
	[Id] INT identity(1000,1) PRIMARY KEY,
	[DbtableId] int,
	[Name] varchar(100),
	foreign key (DbtableId) references DbTables(Id)
)
