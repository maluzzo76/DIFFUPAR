
create PROCEDURE rtl.InyectarNapse
AS
declare  @mind datetime,@maxd datetime
select @mind = min(fecha),@maxd = max(fecha) from rtl.v_NapseImport

delete DiffuparAnalytics.stg.NapseTransactions where TipoOperacion in ('Sale','Return') and fecha between @mind and @maxd

insert into DiffuparAnalytics.stg.NapseTransactions
select 
*
From rtl.V_NapseImport i


--------------------------------------------------------------
------- inyecta transacciones Napse en FactVentasRTL ---------
--------------------------------------------------------------
delete  DiffuparAnalytics.rtl.FactVentasRTL where origen='NAPSE'
insert into DiffuparAnalytics.rtl.FactVentasRTL
select 	
	NroDocumento collate Modern_Spanish_CI_AS AS id,
	FORMAT(t.Fecha, 'yyyy-MM-dd') as Fecha,
	convert(char,t.Fecha,114) as Hora,
	t.NroOperacion as Numero,
	concat(TipoDocumento,'-',t.PuntoDeVenta,'-',t.NroDocumento)  as NumeroCompleto,
	p.Proveedor collate French_CI_AS proveedor ,
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
		end TotalCIva,
		
		case 
			when t.id is null then t.PrecioTotalSinIva
			when TipoOperacion = 'Return' and t.PrecioTotalSinIva >0 then isnull(t.PrecioTotalSinIva *-1,0)
		else isnull(t.PrecioTotalSinIva,0)
		end  Venta_Neta,

		t.CodigoArticulo,
		p.ArticuloDescripcion ,
		v.GCL_LIBELLe collate French_CI_AS codigoVendedor,
		v.GCL_COMMERCIAL collate French_CI_AS nombreVendedor,
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
left join rtl.Vendedores v WITH(nolock) on v.GCL_COMMERCIAL collate Modern_Spanish_CI_AS = t.CodigoVendedor collate Modern_Spanish_CI_AS
left join  rtl.ClientesRtl cl on cl.clienteCodigo collate Modern_Spanish_CI_AS = t.ClienteCodigo collate Modern_Spanish_CI_AS
		  
where TipoOperacion in ('Sale','Return')
and t.fecha > '2024-12-10' and t.fecha < '2124-12-31'
