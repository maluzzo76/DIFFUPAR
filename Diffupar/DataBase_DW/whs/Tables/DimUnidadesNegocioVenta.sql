CREATE TABLE [whs].[DimUnidadesNegocioVenta]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	Codigo int,
	Direccion varchar(300),
	Unidad_Negocio varchar(300)
)
