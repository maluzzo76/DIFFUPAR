CREATE TABLE [stg].[VentasComplemento]
(
	Fecha_Contabilizacion datetime,
	Tipo_Documento varchar (200),
	Socio_Negocio varchar (200),
	Numero_Documento varchar (200),
	Numero_Transaccion varchar (500),
	Direccion varchar (100),
	Origen varchar (10),
	Escenario varchar (15),
	Total_Documento decimal (18,4),
	Venta_Neta decimal (18,4),
	Cantidad int,
	Codigo_Articulo varchar (200),
	Dimension_1 varchar(200),
	Dimension_2 varchar(200),
	Dimension_3 varchar(200),
	Dimension_4 varchar(200),
	Dimension_5 varchar(200),
	Costo_Unitario decimal (18,4),
	Costo_Total decimal (18,4),
	Cuenta_Venta varchar (500),
	Cuenta_Costo varchar (500),
	Cuenta_Inventario varchar (500),
	Precio_Unitario_sin_IVA decimal (18,4),
	Descuento_Total_sin_IVA decimal (18,4),
	Venta_PP_sin_IVA decimal (18,4),
	Porcentaje_Descuento decimal (18,4),
	Targetype varchar(200),
	TrgetEntry varchar(200),
	Numero_Linea int,
	Codigo_Tributario varchar(200),
	Moneda varchar (200),
	OpenInvQty varchar (200),
	Tasa_Impositiva decimal(18,4),
	Impuesto_Total decimal(18,4),
	Precio_Despues_Descuento decimal(18,4),
	Codigo_Empleado_Venta varchar(100)


)
GO

CREATE NONCLUSTERED INDEX IX_FechaContabilizacion_stgVC
ON [stg].[VentasComplemento] ([Origen],[Cuenta_Venta])
INCLUDE ([Fecha_Contabilizacion])
GO
