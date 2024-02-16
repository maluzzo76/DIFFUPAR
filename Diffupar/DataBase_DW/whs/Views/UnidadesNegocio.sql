CREATE VIEW [whs].[UnidadesNegocio]
	AS 
SELECT 
	Id,
	codigo OcrCode1, 
	trim(Direccion) Direccion , 
	trim(Unidad_Negocio) Unidad_Negocio 
FROM whs.DimUnidadesNegocioVenta with(nolock)
GO;


