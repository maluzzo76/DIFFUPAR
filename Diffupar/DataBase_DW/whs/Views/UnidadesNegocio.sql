CREATE VIEW [whs].[UnidadesNegocio]
	AS 
SELECT 
	codigo, 
	trim(Direccion) Direccion , 
	trim(Unidad_Negocio) Unidad_Negocio 
FROM whs.DimUnidadesNegocioVenta with(nolock)
GO;


