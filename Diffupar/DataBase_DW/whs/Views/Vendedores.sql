CREATE VIEW [whs].[Vendedores]
	AS 
select 
	Id,
	SlpCode Codigo_Vendedor,
	SlpName Nombre_Vendedor
from whs.DimVendedores
union all
select 
	Id,
	Codigo Codigo_Vendedor,
	Nombre + ' ' + Apellido Nombre_Vendedor	
from stg.vendedoresRetails