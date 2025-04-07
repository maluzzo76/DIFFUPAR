alter VIEW rtl.VentaComercial
as
--select * from DiffuparAnalytics.rtl.FactVentasRTL
select
id,
Fecha_Contabilizacion,
hora,
Numero,
NumeroCompleto,	
max(Proveedor)Proveedor,
max(Rubro) Rubro,
max(Marca) Marca,
max(Segmento)Segmento,
max(Direccion)Direccion,
Sucursal,
sum(CantidadItems) CantidadItems,
sum(TotalDescuentoComercial) TotalDescuentoComercial,
sum(TotalDescuentofidelidad)TotalDescuentofidelidad,
max(PrecioUnitario) PrecioUnitario,
sum(Total_CIva)Total_CIva,
sum(VENTA_NETA)VENTA_NETA,
CodigoArticulo,
ArticuloDescripcion,
max(GCL_LIBELLE +' - '+GCL_PRENOM)  collate Modern_Spanish_CI_AS Vendedor,
CodigoVendedor,
EAN,
Origen,
NroTransaction,
CodigoTienda,
ClienteCodigo,
max(ClienteNombre)ClienteNombre,
max(ClienteApellido) ClienteApellido,
max(ClienteZona) ClienteZona,
max(ClienteEmail)ClienteEmail,
max(ClienteGenero) ClienteGenero
from DiffuparAnalytics.rtl.FactVentasRTL vr
left join rtl.Vendedores v on GCL_COMMERCIAL collate Modern_Spanish_CI_AS = vr.CodigoVendedor collate Modern_Spanish_CI_AS

group by 
id,
Fecha_Contabilizacion,
hora,
Numero,
NumeroCompleto,	
Sucursal,
CodigoArticulo,
ArticuloDescripcion,
EAN,
Origen,
NroTransaction,
CodigoTienda,
CodigoVendedor,
ClienteCodigo
