CREATE  VIEW [whs].[ObjetivosRetails]
	AS 
select 
o.id,
o.Anio,
o.mes,
s.CodigoTienda LugarClienteCodigo,
s.NombreTienda LugarCliente,
o.ObjetivoMoneda,
o.ObjetivoCantidad,
o.IsNew,
o.Origen,
o.UpdateDate,
o.Usuario,
o.PorcentajeDiffupar
from DiffuparAnalytics.stg.Objetivos o
inner join rtl.ScursalesRetails s on s.NombreTienda = o.LugarCliente 


