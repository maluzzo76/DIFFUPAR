------------------------------------------------------------------------------------
--Orden de Compra UNION ALL Budget
------------------------------------------------------------------------------------
CREATE VIEW [whs].[OrdenDeCompra] 
AS

select * from whs.VentasMensual with(nolock)
where Origen = 'BUDGET' 

UNION ALL

select
DocNum as Numero,
TargetType,
TrgetEntry,
DocDate as Fecha_Contabilizacion,
JrnlMemo,
PTICode + '-' + Letter + '-' + replicate('0', 8-len(FolNumTo))+ FolNumTo as numerocompleto,
DocType as Tipo_Documento,
LineNum as Numero_Linea,
BaseCard as Codigo_BP_Base,
TaxCode as Codigo_Tributario,
Currency as Moneda, --
PriceBefDi as Precio_Unitario,
InvQty,
OpenInvQty,

case 
		when TaxCode = 'IVA_EXEv' then 0 
		else VatPrcnt 
	end Tasa_Impositiva, 

case 
	when JrnlMemo like 'A/R Credit Memos%' and OpenInvQty < 0 and VatSum > 0 then VatSum *-1
	else VatSum
	end  Impuesto_Total ,

case 
	when JrnlMemo like 'A/R Credit Memos%' and OpenInvQty < 0 and Price < 0 then Price *-1
	else Price
	end	 Precio_Despues_Descuento,

p.Codigo as ItemCode,
PlanCuenta_id,
Departamento_id,
LugarCliente_id,
PropioTercero_id,
c.TipoProducto_id,
TipoComprobante_id,
Producto_id,
c.Cliente_id,
Transid,
Dscription,
unitMsr,
WhsCode,
OcrCode,
OcrCode2,
OcrCode3,
OcrCode4,
OcrCode5,
StockPrice,
StockValue,
DiscPrcnt,
DocEntry as IDS_AP,
INMPrice as  Precio_Venta_Articulo,
CogsAcct,
CtlAccount,
AcctCode,
VendorNum,

case 
		when JrnlMemo like '%Facturas clientes%' or JrnlMemo like '%Facturas de clientes%'   then 'MAYORISTA'
		when JrnlMemo like '%A/R Invoices%' AND OcrCode3 = 30003  then 'MAYORISTA'
		when JrnlMemo like '%/R Invoices%' AND OcrCode3 <> 30003  then 'RETAIL'
		when JrnlMemo like '%/R Credit Memos%' AND OcrCode3 = 30003  then 'MAYORISTA'
		when JrnlMemo like '%A/R Credit Memos%' AND OcrCode3 <> 30003  then 'RETAIL'
		when JrnlMemo like '%Notas de crédito clientes%'  then 'MAYORISTA'
		else NULL
	end Direccion,

Origen,
SlpCode as Codigo_Empleado_Venta,
U_Deslin,
U_DescFide,
U_DescCComer,
GTotal as Total_Documento,

case 
	when JrnlMemo like '%A/R Invoices%' or JrnlMemo like '%A/R Credit Memos%' then (U_DescCComer + U_Deslin + U_DescFide ) / Tasa_Impositiva_porc
	when JrnlMemo like '%Facturas clientes%' or JrnlMemo like '%Notas de crédito clientes%'or JrnlMemo like '%Notas de credito clientes%' then (PriceBefDi - Price) * InvQty 
	else 0
end Dto_Total_SIN_IVA,

convert( decimal(18,4), 
				case  when Tasa_Impositiva_porc > 0 then GTotal / Tasa_Impositiva_porc
					else 0
				end)  VENTA_NETA,

(PriceBefDi - Price) * InvQty as VENTA_PP_SIN_IVA,

DiscPrcnt as '%Dto',
'OC' Escenario,
U_RBI_EXTCOD
from whs.FactOrdenesDeCompra c with(nolock)
LEFT JOIN WHS.DimProductos  P with(nolock) ON P.id = Producto_Id