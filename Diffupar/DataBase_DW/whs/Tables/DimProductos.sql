CREATE TABLE [whs].[DimProductos]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Codigo] varchar(100),
	[Nombre] varchar(500),
	[Categoria_Id] int, 
	[TipoProveedor_Id] int, 
	[ProveedorRetailCegid_Id] int, 
	[CreateDate] datetime,
	[UpdateDate] datetime,
	FOREIGN KEY	([Categoria_Id]) REFERENCES whs.DimCategoria(Id),
	FOREIGN KEY	([TipoProveedor_Id]) REFERENCES whs.DimTipoProveedor(Id),
	FOREIGN KEY	([ProveedorRetailCegid_Id]) REFERENCES whs.DimProveedorRetailCegid(Id)
)