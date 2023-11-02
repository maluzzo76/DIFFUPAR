CREATE TABLE [whs].[DimCategoria]
(
	[Id] INT identity(1,1) NOT NULL PRIMARY KEY,
	[Codigo] varchar(100),
	[Nombre] varchar(100),
	[CreateDate] datetime,
	[UpdateDate] datetime
)
