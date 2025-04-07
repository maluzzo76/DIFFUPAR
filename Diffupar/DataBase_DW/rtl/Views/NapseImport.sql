alter VIEW rtl.v_NapseImport
AS
select 
null id,
 CASE 
    WHEN TRY_CONVERT(DATE, FechaTrx, 111) IS NOT NULL 
        THEN CONVERT(DATE, FechaTrx, 111)
    WHEN TRY_CONVERT(DATE, FechaTrx, 103) IS NOT NULL 
        THEN CONVERT(DATE, FechaTrx, 103)
    ELSE NULL
END Fecha,
case 
	when Comprobante like 'FC%' then 'Sale'
	when Comprobante like 'NC%' then 'Return'
else null end TipoOperacion,
NroTrx NroOperacion,
SUBSTRING(tienda,0,5) codigoTienda,
SUBSTRING(tienda,6,LEN(tienda)-4) tienda,
null motivo,
SUBSTRING(Comprobante,0,5) TipoDocumento,
trim(SUBSTRING(Comprobante,13,50)) NroDocumento,
null EstadoDocumento,
Códigodedepósito DepositoOrigen,
'' DepositoOrigenNombre,
'' DepostioDestino,
Codigo CodigoArticulo,
item NombreArticulo,
convert(int,Unidades) Cantidad,
rtl.F_GetVendedorCodigo(n.vendedor) CodigoVendedor,
n.Vendedor NombreVendedor,
'OnSale'estadoInventario,
null MenssgeProcessId,
null userName,
null proveedor,
null tipoAjuste,
null CodigoRecepcion,
null NroOcSap,
NroPedido NumeroPedidoExterno,
null CodigoTiendaDestino,
null NombreTiendaDestino,
null codigoExterno,
trim(SUBSTRING(Comprobante,6,5)) PuntoDeVenta,
trim(SUBSTRING(Comprobante,4,1)) LetraComprobante,
convert(Decimal(18,4),(Bruto/Unidades ) / 1.21) PrecioUnitarioSinIva,
convert(Decimal(18,4),(Bruto/Unidades )) PrecioInitarioTotal,
convert(Decimal(18,4),VentaNeta) PrecioTotalSinIva,
convert(Decimal(18,4),BrutoDesc) PrecioTotal,
convert(Decimal(18,4),[Desc])DescuentoPromo,
0 DescuentoManual,
canal,
CodCliente ClienteCodigo,
Nombre ClienteNombre,
'' ClienteApellido
from rtl.NapseImport n




