CREATE TABLE [stg].[Productos]
(
	[ItemCode] varchar(100), -- Codigo Producto
	[Itemname] varchar(500), -- Nombre Producto
	[U_RBI_Categoria] varchar(100), -- Id @Categoria
	[U_RBI_TipoProveedor] varchar(100), -- Id @Categoria
	[U_RBI_ProvRetailCegid] varchar(100), -- Id @Categoria
	[CreateDate] datetime,
	[UpdateDate] datetime
)
