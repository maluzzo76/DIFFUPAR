﻿CREATE VIEW [whs].[VentasMensual]
	AS 

select  
null Numero,
TargetType,
TrgetEntry,
Fecha_Contabilizacion,
Tipo_Documento + ' - ' + Numero_Documento JrnlMemo,
Numero_Documento numerocompleto,
Tipo_Documento,
Numero_Linea,
'' Codigo_BP_Base,
Codigo_Tributario,
Moneda,
Costo_Unitario Precio_Unitario,
Cantidad InvQty,
OpenInvQty,
Tasa_Impositiva,
Impuesto_Total,
Precio_Despues_Descuento,
Codigo_Articulo ItemCode,
null PlanCuenta_id,
null Departamento_id,
null LugarCliente_id,
null PropioTercero_id,
null TipoProducto_id,
null TipoComprobante_id,
null Producto_id,
null Cliente_id,
null Transid,
'' Dscription,
'' unitMsr,
'' WhsCode,
Dimension_1 OcrCode,
Dimension_2 OcrCode2,
Dimension_3 OcrCode3,
Dimension_4 OcrCode4,
Dimension_5 OcrCode5,
Costo_Unitario StockPrice,
Costo_Total StockValue,
null DiscPrcnt,
null IDS_AP,
null Precio_Venta_Articulo,
Cuenta_Costo CogsAcct,
Cuenta_Inventario CtlAccount,
Cuenta_Venta  AcctCode,

'' VendorNum,
Direccion,
Origen,
slpCode Codigo_Empleado_Venta,
null U_Deslin,
null U_DescFide,
null U_DescCComer,
Total_Documento,
Descuento_Total_sin_IVA Dto_Total_SIN_IVA,
Venta_Neta VENTA_NETA, 
Venta_PP_sin_IVA VENTA_PP_SIN_IVA, 
Porcentaje_Descuento [%Dto],
Escenario,
convert(varchar(500),[U_RBI_EXTCOD]) [U_RBI_EXTCOD]

FROM(
select 
	Fecha_Contabilizacion,
	Tipo_Documento,
	Socio_Negocio,
	Numero_Documento,
	Numero_Transaccion,
	Direccion,
	Origen,
	Escenario,
	Total_Documento,
	Venta_Neta,
	Cantidad,
	Codigo_Articulo,
	Dimension_1,
	Dimension_2,
	Dimension_3,
	Dimension_4,
	Dimension_5,
	Costo_Unitario,
	Costo_Total,
	Cuenta_Venta,
	Cuenta_Costo,
	Cuenta_Inventario,
	Precio_Unitario_sin_IVA,
	Descuento_Total_sin_IVA,
	Venta_PP_sin_IVA,
	Porcentaje_Descuento,
	Targetype TargetType,
	TrgetEntry,
	Numero_Linea,
	Codigo_Tributario,
	Moneda,
	OpenInvQty,
	Tasa_Impositiva,
	Impuesto_Total,
	Precio_Despues_Descuento,
	null [U_RBI_EXTCOD],
	Codigo_Empleado_Venta slpCode
from stg.ventascomplemento

union all

select 
	Fecha_Contabilizacion,
	Tipo_Documento,
	Socio_Negocio,
	Numero_Documento,
	Numero_Transaccion,
	Direccion,
	Origen,
	Escenario,
	Total_Documento,
	Venta_Neta,
	Cantidad,
	Codigo_Articulo,
	Dimension_1,
	Dimension_2,
	Dimension_3,
	Dimension_4,
	Dimension_5,
	Costo_Unitario,
	Costo_Total,
	Cuenta_Venta,
	Cuenta_Costo,
	Cuenta_Inventario,
	Precio_Unitario_sin_IVA,
	Descuento_Total_sin_IVA,
	Venta_PP_sin_IVA,
	Porcentaje_Descuento,
	null TargetType,
	null TrgetEntry,
	null Numero_Linea,
	null Codigo_Tributario,
	null Moneda,
	null OpenInvQty,
	null Tasa_Impositiva,
	null Impuesto_Total,
	null Precio_Despues_Descuento,
	null [U_RBI_EXTCOD],
	Codigo_Empleado_Venta slpCode
from stg.VentasHistoricas
) AS R1

