CREATE TABLE [whs].[DimTiposProductos]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	Codigo int,
	Tipo_Producto varchar(200),
	Proveedor varchar(200)
)
