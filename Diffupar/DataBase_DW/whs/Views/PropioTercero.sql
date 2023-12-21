CREATE VIEW [whs].[PropioTercero]
	AS 
Select 
Codigo,
trim(Propio_Tercero) Propio_Tercero,
trim(Proveedor) Proveedor
From whs.DimPropioTercero with(nolock)
