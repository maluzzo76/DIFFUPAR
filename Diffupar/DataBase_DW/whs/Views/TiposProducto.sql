CREATE VIEW [whs].[TiposProducto]
	AS 
Select 
Id,
Codigo OcrCode5,
trim(Tipo_Producto) Tipo_Producto,
trim(Proveedor) Marca
From whs.DimTiposProductos with(nolock)
