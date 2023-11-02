CREATE TABLE [whs].[DimTiposProductos]
(
	ID int identity(1,1) primary key,
	[Code] varchar(100), -- Codigo Dimension
	[Name] varchar(100), -- Nombre Dimension
	[CreateDate] datetime, -- Fecha Creacion
	[UpdateDate] datetime -- Fecha Actualización
)
