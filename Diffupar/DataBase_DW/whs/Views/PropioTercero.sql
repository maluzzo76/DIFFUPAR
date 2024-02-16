CREATE VIEW [whs].[PropioTercero]
	AS 
Select 
Id,
Codigo OcrCode4,
trim(Propio_Tercero) Propio_Tercero,
trim(Proveedor) Proveedor
From whs.DimPropioTercero with(nolock)
