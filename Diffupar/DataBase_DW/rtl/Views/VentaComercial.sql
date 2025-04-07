create VIEW rtl.VentaComercial
as
select 
	fc.id,
	fc.Fecha Fecha_Contabilizacion,
	fd.hora,
	Numero,
	NumeroCompleto,	
	Proveedor,
	Rubro,
	Marca,
	Segmento,
	Direccion,
	sucursal Sucursal,
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
	fc.Origen

from rtl.facturaCabeceraCegid fc
left join rtl.FacturaDetallecegit fd on fd.FacturaCabecera_id = fc.id
left join rtl.Vendedores v on v.GCL_COMMERCIAL collate French_CI_AS = fc.vendedor collate French_CI_AS
where fc.Origen = 'CEGID'

union ALL


select 
	CONCAT(PuntoDeVenta, '-', TipoDocumento,'-', LetraComprobante, '-', NroDocumento) collate Modern_Spanish_CI_AS AS id,
	FORMAT(t.Fecha, 'yyyy-MM-dd') as Fecha,
	convert(char,t.Fecha,114) as Hora,
	t.NroOperacion as Numero,
	t.NroDocumento as NumeroCompleto,
	p.Proveedor collate French_CI_AS ,
	p.Rubro ,
	p.Marca,
	p.Segmento,
	case																			
		when	   NombreTienda ='ALTO AVELLANEDA'
				OR NombreTienda ='ALTO PALERMO GOND' 
				OR NombreTienda ='IMPRENTA' 
				OR NombreTienda ='VILLA DEL PARQUE' 
				OR NombreTienda ='PASEO ALCORTA 2P'
				OR NombreTienda ='MENDOZA PLAZA'
				OR NombreTienda ='PATIO BULLRICH PB'
				OR NombreTienda ='VILLA DEL PARQUE'
				OR NombreTienda ='PATIO BULLRICH PB'
				OR NombreTienda ='QUILMES FACTORY'
				OR NombreTienda ='MARKETING RETAIL'
				OR NombreTienda ='DISTRITO ARCOS GOND'
				OR NombreTienda ='CREED ALVEAR'
				OR NombreTienda ='CABILDO Y PAMPA'
				OR NombreTienda ='ALTO ROSARIO'
				OR NombreTienda ='ABASTO MAC'
				OR NombreTienda ='ALTO PALERMO MAC'
				OR NombreTienda ='MAC ALTO ROSARIO'
				OR NombreTienda ='ALTO ROSARIO MAC'
				OR NombreTienda ='PALERMO SOHO MAC'
				OR NombreTienda ='MENDOZA PLAZA GOND'
				OR NombreTienda ='GALERIAS PACIFICO MAC'
				OR NombreTienda ='PALMARES'
				OR NombreTienda ='ALTO AVELLANEDA MALA' then 'ST NO COMPARABLE'
		when (LEFT(NombreTienda,3)='B24' and NombreTienda<>'B24 ECOMMERCE' )then 'BEAUTY 24'
		when NombreTienda='CONNECTE' THEN 'ECOMMERCE'
		when NombreTienda='B24 ECOMMERCE' THEN 'B24 ECOMMERCE'
		when NombreTienda='OUTLET' THEN 'OUTLET'
		ELSE 'ST COMPARABLE'
		END Direccion,
		case 
			when NombreTienda = 'MAC ALTO ROSARIO' then 'ALTO ROSARIO MAC'
			else NombreTienda
		end Sucursal,

		case 
			when TipoOperacion = 'Return' then t.Cantidad *-1
		else t.Cantidad
		end CantidadItems,

		case 
			when TipoOperacion = 'Return' then t.DescuentoPromo *-1
		else t.DescuentoPromo
		end TotalDescuentoComercial,

		case 
			when TipoOperacion = 'Return' then t.DescuentoManual *-1
		else t.DescuentoManual
		end TotalDescuentofidelidad,

		case 
			when TipoOperacion = 'Return' then t.PrecioInitarioTotal *-1
		else t.PrecioInitarioTotal
		end  precioUnitario,

		case 
			when TipoOperacion = 'Return' then isnull(t.PrecioTotal*-1,0) 
		else t.PrecioTotal
		end TotalCIva,

		case 
			when TipoOperacion = 'Return' then isnull(t.PrecioTotalSinIva *-1,0)
		else isnull(t.PrecioTotalSinIva,0)
		end  Venta_Neta,

		t.CodigoArticulo,
		p.ArticuloDescripcion ,
		NombreVendedor collate French_CI_AS,
		CodigoVendedor collate French_CI_AS,
		p.EAN,
		'NAPSE' Origen

From DiffuparAnalytics.stg.NapseTransactions t
inner join rtl.ProductosRetails p on p.codigoArticulo  = t.CodigoArticulo

where (TipoOperacion = 'Sale' or TipoOperacion = 'Return') and t.fecha > '2024-12-17' and t.fecha < '2124-12-31'
