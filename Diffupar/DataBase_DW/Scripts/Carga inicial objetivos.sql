delete stg.Objetivos 
insert into stg.Objetivos
select 
Year_target_Diffupar anio,
convert(int,substring(ID_YM_target_rouge,5,2)) mes,
lc.OcrCode3 sucursal,
lc.sucursalRetail sucursalNombre,
convert(decimal(18,4), target_diffupar_mensual) objetivo,
0 objetivoCantidad,
0,
'OBETIVOS TOTALES' origen,
getdate(),
'Importación Incial',
30

from [DiffuparAnalyticsQA].dbo.[Objetivos Diffupar]
left join whs.LugarCliente lc on lc.SucursalRetail like '%'+ replace(Sucursales,'"','')
where lc.OcrCode3 is  null 

-- USAR ARCHIVO EXPORTADO DE OJGETIVOS DIFFUPAR
update stg.Objetivos set ObjetivoCantidad = r1.cantidad
from(
select  o.id, anio,mes,sum(InvQty) cantidad
from stg.Objetivos o 
inner join [whs].[VentasRetails] v on convert(int, v.OcrCode3) = convert(int, o.LugarClienteCodigo )
										and year(Fecha_Contabilizacion) = o.anio 
										and month(Fecha_Contabilizacion) = o.mes
group by
o.id, anio,mes
) as r1
where stg.Objetivos.Id = r1.Id

------------------------------------------------------------------------------------------------------
-- Importar Vendedores
------------------------------------------------------------------------------------------------------
--[200.32.54.90].Produccion.[dbo]
delete stg.VendedoresRetails

insert into stg.VendedoresRetails
SELECT 
GCL_COMMERCIAL Codigo,
GCL_PRENOM Nombre,
GCL_SURNOM Apellido

FROM [200.32.54.90].Produccion.[dbo].COMMERCIAL

------------------------------------------------------------------------------------------------------
-- Importar la asociacion entre venta y vendedor
------------------------------------------------------------------------------------------------------
delete stg.cegidVentaVendedor
insert into stg.cegidVentaVendedor
select vta.U_RBI_EXTERCODE, vend.codigo VendCodigo
from dbo.v_CegidVenta vta
inner join stg.VendedoresRetails vend on 
										vend.codigo collate Modern_Spanish_CI_AS  = vta.GP_REPRESENTANT collate Modern_Spanish_CI_AS
