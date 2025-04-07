CREATE VIEW [whs].[TiposProducto]
	AS 
Select 
Id,
Codigo OcrCode5,
trim(Tipo_Producto) Tipo_Producto,
trim(Proveedor) Marca,
Case 
	When trim(Tipo_Producto) = 'ZPEL' Then 'Peluquería'
	When trim(Tipo_Producto) = 'ZFRA' Then 'Fragancia'
	When trim(Tipo_Producto) = 'ZACC' Then 'Accesorios'
	When trim(Tipo_Producto) = 'Accesorios – MEMO' Then 'Accesorios'
	When trim(Tipo_Producto) = 'ZTRA' Then 'Tratamientos'
	When trim(Tipo_Producto) = 'ZCOS' Then 'Cosmética'
	When trim(Tipo_Producto) = 'ZTEX' Then 'Maison'
	When trim(Tipo_Producto) = 'ZBAZ' Then 'Maison'
	When trim(Tipo_Producto) = 'ZDEC' Then 'Maison'
	When trim(Tipo_Producto) = 'ZCAP' Then 'Capilares'
	When trim(Tipo_Producto) = 'ZREL' Then 'Otros'
	When trim(Tipo_Producto) = 'ZGWP' Then 'Otros'
	When trim(Tipo_Producto) = 'ZGIF' Then 'Otros'
	When trim(Tipo_Producto) = 'Otros.' Then 'Otros'
	When trim(Tipo_Producto) = 'ZPROB' Then 'Otros'
	When trim(Tipo_Producto) = 'ZPLV' Then 'Otros'
	When trim(Tipo_Producto) = 'TODAS' Then 'Otros'
	When trim(Tipo_Producto) = 'SERV' Then 'Otros'
Else trim(Tipo_Producto)
end Tipo_Producto_Negocio

From whs.DimTiposProductos with(nolock)
