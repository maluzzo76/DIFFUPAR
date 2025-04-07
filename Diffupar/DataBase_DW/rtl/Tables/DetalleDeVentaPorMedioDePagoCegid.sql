
CREATE TABLE [rtl].[DetalleDeVentaPorMedioDePagoCegid]
(
Fecha datetime,
fk_Piece varchar(200),
GPE_NUMERO varchar(100),
Sucursal varchar(255),
pk_piece varchar(200),
GP_REFINTERNE varchar(300),
PK_Control_OB varchar(300),
NombreApellidoCliente varchar(500),
caja_almacen VARCHAR(100) ,
CodigoTipo varchar(100),
MedioDePago varchar(200),
DESCRIPCION_PAGO varchar(300),
TipoMedioPago varchar(300),
Banco_Terjeta varchar(300),
cuotas int,
Cuotasv3 varchar(100),
nro_tarjeta varchar(100),
apellido_tarjeta varchar(300),
monto_compra decimal(18,4)
)