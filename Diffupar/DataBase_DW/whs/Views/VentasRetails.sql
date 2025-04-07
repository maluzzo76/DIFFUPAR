CREATE VIEW [whs].[VentasRetails]
	AS 
SELECT 
	*,
	0 PorcentajeDiffupar
FROM whs.Facturacion
WHERE Origen = 'SAP'

union all

select 
	*, 
	0 PorcentajeDiffupar
from whs.FactVentas
WHERE Origen = 'BUDGET'

union all

select 
null Numero,
null TargetType,
null TrgetEntry,
DATEFROMPARTS(anio, mes, 1) Fecha_Contabilizacion,
null JrnlMemo,
null numerocompleto,
null Tipo_Ducumento,
null Numero_Linea,
null Codigo_BP_Base,
null Codigo_Tributario,
null Moneda,
null Precio_Unitario,
ObjetivoCantidad InvQty,
null OpenInvQty,
null Tasa_Impositiva,
null Impuesto_Total,
null Precio_Despues_Descuento,
null ItemCode,
null PlanCuenta_id,
null Departamento_id,
null LugarCliente_id,	
null PropioTercero_id,
null TipoProducto_id,
null TipoComprobante_id,
null Producto_id,
null Cliente_Id,
null Transid,
null Dscription,
null unitMsr,
null WhsCode,
null OcrCode,
null OcrCode2,
LugarClienteCodigo OcrCode3,
null OcrCode4,
null OcrCode5,
null StockPrice,
null StockValue,
null DiscPrcnt,
null IDS_AP,
null Precio_Venta_Articulo,
null CogsAcct,
null CtlAccount,
null AcctCode,
null VendorNum,
null Direccion,
'OBJETIVO' Origen,
null Codigo_Empleado_Venta,
null U_Deslin,
null U_DescFide,
null U_DescCComer,
null Total_Documento,
null Dto_Total_SIN_IVA,
ObjetivoMoneda Venta_Neta,
null VENTA_PP_SIN_IVA,
null [%Dto],
null Escenario,
null [U_RBI_EXTCOD],
PorcentajeDiffupar
from stg.Objetivos




