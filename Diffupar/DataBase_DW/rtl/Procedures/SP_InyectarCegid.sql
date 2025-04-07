ALTER PROCEDURE [rtl].[SP_InyectarCegid]
AS

declare @anio int, @mes int

set @anio = year(getdate())
set @mes = month(getdate())



delete rtl.FacturaCabeceraCegid where year(Fecha)=@anio and MONTH(Fecha)=@mes
insert into rtl.FacturaCabeceraCegid
select 
	Fecha,
	Numero,
	TotalCIva,
	Venta_Neta,
	NumeroCompelto,
	Pk_Piece,
	Sucursal,
	Grupo,
	Representante,
	'CEGID' Orgien,
	ET_ETABLISSEMENT CodigoTienda,	
	ClienteNombre,
	ClienteApellido,
	ClienteZona,
	ClienteEmail,
	ClienteGenero,
	Cliente as ClienteCodigo
from rtl.FacturaCabecera
where year(Fecha)=@anio
and MONTH(fecha)=@mes

---------------------------------------------------------------
-- Inyecto los datos de factura detalle de cegid en el dw
---------------------------------------------------------------
delete rtl.FacturaDetalleCegit where year(Fecha)=@anio and MONTH(Fecha)=@mes
Insert into rtl.FacturaDetalleCegit
select 
	Gl_datepiece,
	tiempo,
	NroDoc,
	NroLinea,
	TipoArticulo,
	Gl_CODEARTICLE,
	Article_desc,
	Total_CIva,
	Venta_Neta,
	Fk_Piece,
	Pk_Ligne,
	Proveedor1,
	Rubro,
	Marca,
	Segmento,
	CantidadItems,
	GL_REMISELIBRE2,
	GL_REMISELIBRE3,
	GL_PUTTC, 
	GA_CODEBARRE,
	'CEGID' Orgien	
from rtl.FacturaDetalle
where year(Gl_datepiece)=@anio
and month(Gl_datepiece)=@mes


---------------------------------------------------------------
-- Cargo Maestro de Articulos
---------------------------------------------------------------
delete rtl.ProductosRetails
insert into rtl.ProductosRetails
select distinct
TipoArticulo TipoArticulo,
codigoArticulo,
ArticuloDescripcion,
Proveedor,
Rubro,
Marca,
SEGMENTO,
EAN barCode
from rtl.FacturaDetalleCegit

insert into rtl.ProductosRetails
select Nombre_Categoria TipoArticulo, Codigo CodigoArticulo, Nombre ArticuloDescripcion, Nombre_ProveedorRetailCegid Proveedor,
Nombre_GrupoArt Rubro,
Nombre_Marca Marca,
Nombre_Genero Segmento,
null EAN
from DiffuparAnalytics.whs.Productos 
where not exists (select codigoArticulo from rtl.ProductosRetails where CodigoArticulo collate French_CI_AS =  codigo collate French_CI_AS)
and nombre is not null

--------------------------------------------------------------
-- Inyecto los clientes
---------------------------------------------------------------
delete  rtl.ClientesRtl
insert into  rtl.ClientesRtl
select
	T_TIERS AS clienteCodigo,
	UPPER(t_prenom) as ClienteNombre,
	upper(t_libelle) as ClienteApellido,
	t_ville as ClienteZona,
	LOWER(t_email) as ClienteEmail,
	case
		when t_sexe = 'F' then 'FEMENINO'
		when t_sexe = 'M' then 'MASCULINO'
	else 
	UPPER(t_sexe)
	end as  ClienteGenero 
FROM [200.32.54.90].Produccion.dbo.TIERS p WITH(Nolock)
			where T_NATUREAUXI='CLI'

--------------------------------------------------------------
-- Normaliza los nombre de las sucursales
---------------------------------------------------------------
update rtl.FacturaCabeceraCegid set sucursal = 'ALTO ROSARIO MAC'
where Sucursal = 'MAC ALTO ROSARIO' and Origen = 'Napse'

update DiffuparAnalytics.stg.NapseTransactions set NombreTienda = 'DEVOTO GOND' 
where NombreTienda = 'DEVOTO GONDOLA'

update DiffuparAnalytics.stg.NapseTransactions set NombreTienda = 'DISTRITO ARCOS GOND' 
where NombreTienda = 'DISTRITO ARCOS'



---------------------------------------------------------------
-- Inyecto las sucursales
---------------------------------------------------------------
delete rtl.ScursalesRetails
insert into rtl.ScursalesRetails
select  CodigoTienda,
case
	when Codigotienda = 1330 then 'ALTO AVELLANEDA GOND'
	when Codigotienda = 1361 then 'ABASTO MAC'
	when Codigotienda = 1362 then 'ALTO PALERMO MAC'
	when Codigotienda = 1363 then 'ALTO ROSARIO MAC'
	when CodigoTienda = 1352 then 'ALTO AVELLANEDA FALABELLA'
	else Max(nombreTienda)
end
from 
(
select  distinct e.ET_ETABLISSEMENT CodigoTienda, e.ET_LIBELLE NombreTienda from  [200.32.54.90].Produccion.[dbo].[ETABLISS] as e
union all
-- Tienda napse
select  distinct  CodigoTienda collate French_CI_AS, NombreTienda collate French_CI_AS from DiffuparAnalytics.stg.NapseTransactions where NombreTienda <>'' 
) as r1
where NombreTienda <> 'DEP1'
and CodigoTienda NOT LIKE '%[^0-9]%'
group by CodigoTienda
order by 1

---------------------------------------------------------------
-- Objetivos
---------------------------------------------------------------
update DiffuparAnalytics.stg.objetivos set LugarCliente ='UNICENTER GONDOLA' where LugarCliente ='UNICENTER GOND'
update DiffuparAnalytics.stg.objetivos set LugarCliente ='ALTO PALERMO GONDOLA' where LugarCliente ='ALTO PALERMO GOND'
update DiffuparAnalytics.stg.objetivos set LugarCliente ='DOT FALLABELLA' where LugarCliente ='DOT FALABELLA'
update DiffuparAnalytics.stg.objetivos set LugarCliente ='MAISON ECOMMERCE' where LugarCliente ='MAISON ECOMMERCE'

---------------------------------------------------------------
-- inyectar FactVentasRtl Prod
---------------------------------------------------------------
delete DiffuparAnalytics.rtl.FactVentasRTL where year(Fecha_Contabilizacion)=@anio and month(Fecha_Contabilizacion)=@mes
---------------------------------------------------------------
-- inyectar Trasacciones Cegid
---------------------------------------------------------------
insert into DiffuparAnalytics.rtl.FactVentasRTL
select 
	fc.id,
	fc.Fecha Fecha_Contabilizacion,
	fd.hora,
	Numero,
	NumeroCompleto,	
	Proveedor,
	rtl.F_CastRubroName(Rubro) Rubro,
	Marca,
	Segmento,
	Direccion,
	s.nombreTienda Sucursal,
	fd.CantidadItems,
	TotalDescuentoComercial,
	TotalDescuentofidelidad,
	PrecioUnitario,
	fd.Total_CIva,
	fd.VENTA_NETA,
	fd.CodigoArticulo,
	fd.ArticuloDescripcion,
	v.GCL_LIBELLE + ' - ' + GCL_PRENOM Vendedor,
	v.GCL_COMMERCIAL CodigoVendedor,
	fd.EAN,
	fc.Origen,
	fc.id NroTransaction,
	fc.CodigoTienda,
	fc.ClienteCodigo,
	fc.ClienteNombre,
	fc.ClienteApellido,
	fc.ClienteZona,
	fc.ClienteEmail,
	fc.ClienteGenero

from rtl.facturaCabeceraCegid fc WITH(nolock)
left join rtl.FacturaDetallecegit fd WITH(nolock) on fd.FacturaCabecera_id = fc.id
left join rtl.Vendedores v WITH(nolock) on v.GCL_COMMERCIAL collate French_CI_AS = fc.vendedor collate French_CI_AS
inner join rtl.ScursalesRetails s WITH(nolock) on s.CodigoTienda = fc.CodigoTienda
where fc.Origen = 'CEGID' 
and year(fc.Fecha) =@anio 
and month(fc.Fecha) =@mes


---------------------------------------------------------------
-- inyectar Trasacciones NAPSE
---------------------------------------------------------------
insert into DiffuparAnalytics.rtl.FactVentasRTL
select 
	NroDocumento collate Modern_Spanish_CI_AS AS id,
	FORMAT(t.Fecha, 'yyyy-MM-dd') as Fecha,
	convert(char,t.Fecha,114) as Hora,
	t.NroOperacion as Numero,
	concat(TipoDocumento,'-',t.PuntoDeVenta,'-',t.NroDocumento)  as NumeroCompleto,
	p.Proveedor collate French_CI_AS ,
	DiffuparAnalyticsQA.rtl.F_CastRubroName(replace(p.Rubro, 'Fragancias','Fragancia')) Rubro ,
	p.Marca,
	p.Segmento,
	case																			
		when	   s.NombreTienda ='ALTO AVELLANEDA'
				OR s.NombreTienda ='ALTO PALERMO GONDOLA' 
				OR s.NombreTienda ='IMPRENTA' 
				OR s.NombreTienda ='VILLA DEL PARQUE' 
				OR s.NombreTienda ='PASEO ALCORTA 2P'
				OR s.NombreTienda ='MENDOZA PLAZA'
				OR s.NombreTienda ='PATIO BULLRICH PB'
				OR s.NombreTienda ='VILLA DEL PARQUE'
				OR s.NombreTienda ='PATIO BULLRICH PB'
				OR s.NombreTienda ='QUILMES FACTORY'
				OR s.NombreTienda ='MARKETING RETAIL'
				OR s.NombreTienda ='DISTRITO ARCOS GOND'
				OR s.NombreTienda ='CREED ALVEAR'
				OR s.NombreTienda ='CABILDO Y PAMPA'
				OR s.NombreTienda ='ALTO ROSARIO'
				OR s.NombreTienda ='ABASTO MAC'
				OR s.NombreTienda ='ALTO PALERMO MAC'
				OR s.NombreTienda ='MAC ALTO ROSARIO'
				OR s.NombreTienda ='ALTO ROSARIO MAC'
				OR s.NombreTienda ='PALERMO SOHO MAC'
				OR s.NombreTienda ='MENDOZA PLAZA GOND'
				OR s.NombreTienda ='GALERIAS PACIFICO MAC'
				OR s.NombreTienda ='PALMARES'
				OR s.NombreTienda ='ALTO AVELLANEDA MALA' then 'ST NO COMPARABLE'
		when (LEFT(s.NombreTienda,3)='B24' and s.NombreTienda<>'B24 ECOMMERCE' )then 'BEAUTY 24'
		when s.NombreTienda='CONNECTE' THEN 'ECOMMERCE'
		when s.NombreTienda='B24 ECOMMERCE' THEN 'B24 ECOMMERCE'
		when s.NombreTienda='OUTLET' THEN 'OUTLET'
		ELSE 'ST COMPARABLE'
		END Direccion,

		 s.NombreTienda Sucursal,

		case 
			when t.id is null then t.Cantidad
			when TipoOperacion = 'Return'  then t.Cantidad *-1
		else t.Cantidad
		end CantidadItems,

		case 
			when TipoOperacion = 'Return' then t.DescuentoPromo *-1
		else t.DescuentoPromo
		end TotalDescuentoComercial,

		case 
			when t.id is null then t.DescuentoManual
			when TipoOperacion = 'Return' then t.DescuentoManual *-1
		else t.DescuentoManual
		end TotalDescuentofidelidad,

		case 
			when t.id is null then t.PrecioInitarioTotal
			when TipoOperacion = 'Return'  then t.PrecioInitarioTotal *-1
		else t.PrecioInitarioTotal
		end  precioUnitario,

		case 
			when t.id is null then t.PrecioTotal
			when TipoOperacion = 'Return' and t.PrecioTotal >0 then isnull(t.PrecioTotal*-1,0) 
		else t.PrecioTotal
		end -
		case 
			when TipoOperacion = 'Return' then t.DescuentoPromo 
		else 0
		end TotalCIva,
		
		case 
			when t.id is null then t.PrecioTotalSinIva
			when TipoOperacion = 'Return' and t.PrecioTotalSinIva >0 then isnull(t.PrecioTotalSinIva *-1,0)
		else isnull(t.PrecioTotalSinIva,0)
		end  Venta_Neta,

		t.CodigoArticulo,
		p.ArticuloDescripcion ,
		v.GCL_LIBELLe collate French_CI_AS,
		v.GCL_COMMERCIAL collate French_CI_AS,
		p.EAN,
		'NAPSE' Origen,
		t.NroOperacion NroTransaction,
		t.CodigoTienda,
        t.ClienteCodigo,
        UPPER(t.ClienteNombre) ClienteNombre,
        UPPER(t.ClienteApellido) ClienteApellido,
        cl.ClienteZona,
        cl.ClienteEmail,
        cl.ClienteGenero

From DiffuparAnalytics.stg.NapseTransactions t WITH(nolock)
inner join (
			select 
				CodigoArticulo,
				ArticuloDescripcion,
				max(TipoArticulo)TipoArticulo,
				max(Proveedor)Proveedor,
				max(Rubro)Rubro,
				max(Marca)Marca,
				max(Segmento)Segmento,
				max(EAN)Ean
			from rtl.ProductosRetails WITH(nolock)
			group by CodigoArticulo,ArticuloDescripcion
			) p  on p.codigoArticulo  = t.CodigoArticulo
inner join rtl.ScursalesRetails s WITH(nolock) on s.CodigoTienda = t.CodigoTienda
left join rtl.Vendedores v WITH(nolock) on v.GCL_COMMERCIAL collate Modern_Spanish_CI_AS = replicate('0',4 -len(t.CodigoVendedor)) + t.CodigoVendedor collate Modern_Spanish_CI_AS
--Obtenemos datos de cliente
left join  rtl.ClientesRtl cl on cl.clienteCodigo collate Modern_Spanish_CI_AS = t.ClienteCodigo collate Modern_Spanish_CI_AS
		  
where (TipoOperacion = 'Sale' or TipoOperacion = 'Return') 
and t.fecha > '2024-12-10' and t.fecha < '2124-12-31'
and year(Fecha) =@anio 
and month(Fecha) =@mes


-----------------------------
-- Normalizo sucursal salta
-----------------------------
update DiffuparAnalytics.rtl.FactVentasRTL set Sucursal = 'PORTAL SALTA'where CodigoTienda = 1340
update DiffuparAnalyticsQA.rtl.ScursalesRetails set NombreTienda='PORTAL SALTA'  where CodigoTienda = 1340