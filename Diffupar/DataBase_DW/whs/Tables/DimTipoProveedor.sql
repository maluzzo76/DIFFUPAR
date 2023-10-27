CREATE TABLE [whs].[DimTipoProveedor]
(
	[Id] INT Identity(1,1) NOT NULL PRIMARY KEY,
	[Codigo] varchar(100), 
	[Nombre] varchar(500), 
	[CreateDate] datetime,
	[UpdateDate] datetime
)


