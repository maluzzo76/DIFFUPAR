CREATE VIEW [whs].[NapseInventarioTransaction]
	AS 
Select
    nt.Id,
    Fecha,
    TipoOperacion,
    NroOperacion,
    nt.CodigoTienda,
    t.Nombre NombreTienda,
    Motivo,
    TipoDocumento,
    convert(varchar(30),nt.PuntoDeVenta) + '-' + nt.LetraComprobante + '_' + NroDocumento NroDocumento ,
    EstadoDocumento,
    DepositoOrigen,
    DepositoDestino,
    nt.CodigoArticulo,
    a.Nombre NombreArticulo,
    Cantidad,
    CodigoVendedor,
    NombreVendedor,  
    EstadoInventario,
    UserName,
    Proveedor,
    TipoAjuste TipoAjuste,
    case  
        when TipoAjuste = '1007' then 'SALIDA EXTRAORDINARIA'
        when TipoAjuste = '1006' then 'ENTRADA EXTRAORDINARIA'
        when TipoAjuste = '1011' then 'RECUENTO DE INVENTARIO'
        when TipoAjuste = '1005' then 'CONSUMO MATERIAL PROPIO MARKETING'
    END TipoAjusteDescripcion,
    CodigoRecepcion,
    NroOcSap,
    NumeroPedidoExterno,
    CodigoTiendaDestino,
    NombreTiendaDestino,
    CodigoExterno,
    PuntoDeVenta,
    LetraComprobante,
    PrecioUnitarioSinIva,
    PrecioInitarioTotal,
    PrecioTotalSinIva,
    PrecioTotal,
    DescuentoPromo,
    DescuentoManual,
    Canal,
    ClienteCodigo,
    ClienteNombre,
    ClienteApellido


FROM stg.NapseTransactions nt with(nolock)
left join whs.DimTiendaRetail t with(nolock) on trim(t.codigo) = trim(nt.CodigoTienda)
left join whs.DimProductos a with(nolock) on a.Codigo = nt.CodigoArticulo
where TipoOperacion <> 'StockReport'


