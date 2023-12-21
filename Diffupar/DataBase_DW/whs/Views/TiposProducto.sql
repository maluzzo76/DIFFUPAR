CREATE VIEW [whs].[TiposProducto]
	AS 
Select 
Codigo,
trim(Tipo_Producto) Tipo_Producto,
trim(Proveedor) Proveedor
From whs.DimTiposProductos with(nolock)
