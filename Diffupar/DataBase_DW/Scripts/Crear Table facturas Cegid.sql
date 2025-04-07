---------------------------------------------------------------
-- Crear tables ventas Retails
---------------------------------------------------------------
create table rtl.FacturaCabeceraCegid
(
	Fecha datetime,
	Numero int,
	Total_CIva decimal(18,4),
	Venta_Neta decimal(18,4),
	NumeroCompleto varchar(200),
	ID varchar(100),
	Sucursal varchar(200),
	Direccion varchar(200)
)

create table rtl.FacturaDetalleCegit
(
	Fecha Datetime,
	Hora time,
	NroDoc int,
	NroLinea Int,
	TipoArticulo varchar(100),
	CodigoArticulo varchar(200),
	ArticuloDescripcion varchar(500),
	Total_CIva decimal(18,4),
	Venta_Neta decimal(18,4),
	FacturaCabecera_Id varchar(100),
	Id varchar(100),
	Proveedor varchar(400),
	Rubro varchar(100),
	Marca varchar(100),
	Segmento varchar(100)
)