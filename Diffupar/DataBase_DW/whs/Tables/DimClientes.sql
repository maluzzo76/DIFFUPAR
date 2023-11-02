CREATE TABLE [whs].[DimClientes]
(
	ID int identity(1,1) primary key,
	[CardCode] varchar(15), -- Codigo Cliente
	[CardName] varchar(100), -- Nombre Cliente
	[Address] varchar(100), -- Direccion
	[CreateDate] datetime, -- Fecha Creacion
	[UpdateDate] datetime -- Fecha Actualización
)
