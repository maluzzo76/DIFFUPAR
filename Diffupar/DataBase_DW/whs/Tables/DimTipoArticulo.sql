CREATE TABLE [whs].[DimTipoArticulo]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	Codigo int,
	Propio_Tercero varchar(200),
	Proveedor varchar(200)
)
