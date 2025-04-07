CREATE VIEW [dbo].[V_PropioTercero]
	AS 
	SELECT * 
	FROM whs.DimLugarCliente 
	WHERE Lugar_Cliente LIKE 'LOC%' 
