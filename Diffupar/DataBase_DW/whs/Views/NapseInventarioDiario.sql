CREATE VIEW [whs].[NapseInventarioDiario]
	AS 
Select
    convert(char,Fecha,103) Fecha,   
    nt.CodigoTienda,	
    nt.CodigoArticulo,
    Cantidad,
	EstadoInventario,
    NombreTienda DepositoCodigo,
    DepositoOrigenNombre DepositoNombre
    

FROM stg.NapseTransactions nt with(nolock)
left join whs.DimTiendaRetail t with(nolock) on trim(t.codigo) = trim(nt.CodigoTienda)
left join whs.DimProductos a with(nolock) on a.Codigo = nt.CodigoArticulo
where TipoOperacion = 'StockReport'
