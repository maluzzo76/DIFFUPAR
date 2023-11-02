CREATE TABLE [whs].[DimProveedorRetailCegid]
(
	[Id] INT identity(1,1) NOT NULL PRIMARY KEY,
	[Code] varchar(200),
	[Name] varchar(200),
	[CreateDate] datetime,
	[UpdateDate] datetime 
)
