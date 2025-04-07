CREATE TABLE rtl.FacturaDetalleCegit
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
	Segmento varchar(100),
	CantidadItems int,
	TotalDescuentoComercial decimal(18,4),
	TotalDescuentofidelidad decimal(18,4),
	PrecioUnitario decimal(18,4),
	EAN Varchar(100),
	Origen Varchar(100)
)
