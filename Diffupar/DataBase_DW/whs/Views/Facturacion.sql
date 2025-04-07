CREATE VIEW [whs].[Facturacion]
	AS 
select 
Numero,
TargetType,
TrgetEntry,
Fecha_Contabilizacion,
JrnlMemo,
numerocompleto,
Tipo_Ducumento,
Numero_Linea,
Codigo_BP_Base,
Codigo_Tributario,
Moneda,

case 
	when JrnlMemo like 'A/R Credit Memos%' and OpenInvQty < 0 and Precio_Unitario < 0 then Precio_Unitario * -1
	else Precio_Unitario
	end Precio_Unitario,

case 
	when JrnlMemo like 'A/R Credit Memos%' and OpenInvQty < 0 and InvQty < 0 then InvQty * -1
	else InvQty
	end InvQty,

OpenInvQty,

case 
	when JrnlMemo like 'A/R Credit Memos%' and OpenInvQty < 0 and Tasa_Impositiva < 0 then Tasa_Impositiva * -1
	else Tasa_Impositiva
	end Tasa_Impositiva,

Impuesto_Total,
Precio_Despues_Descuento,
ItemCode,
PlanCuenta_id,
Departamento_id,
LugarCliente_id,	
PropioTercero_id,
TipoProducto_id,
TipoComprobante_id,
Producto_id,
max(Cliente_id) Cliente_Id,
Transid,
Dscription,
unitMsr,
WhsCode,
OcrCode,
OcrCode2,
OcrCode3,
OcrCode4,
OcrCode5,

case 
	when JrnlMemo like 'A/R Credit Memos%' and OpenInvQty < 0 and StockPrice < 0 then StockPrice *-1	
	when JrnlMemo like 'A/R Credit Memos%' and OpenInvQty > 0 and StockPrice > 0 then StockPrice *-1	
	when JrnlMemo like 'A/R Invoices%' and StockValue < 0 and StockPrice > 0 then StockPrice *-1	
	when JrnlMemo like 'A/R Invoices%' and StockValue > 0 and StockPrice < 0 then StockPrice *-1	
	else StockPrice
	end StockPrice,


case 
	when JrnlMemo like 'A/R Credit Memos%' and OpenInvQty < 0 and StockValue < 0 then StockValue *-1
	when JrnlMemo like 'A/R Credit Memos%' and OpenInvQty > 0 and StockValue > 0 then StockValue *-1
	else StockValue
	end StockValue,

DiscPrcnt,
IDS_AP,
Precio_Venta_Articulo,
CogsAcct,
CtlAccount,
AcctCode,
VendorNum,
Direccion,
Origen,
Codigo_Empleado_Venta,

case 
	when JrnlMemo like 'A/R Credit Memos%' and OpenInvQty < 0 and U_Deslin < 0 then U_Deslin *-1
	else U_Deslin
	end U_Deslin,


case 
	when JrnlMemo like 'A/R Credit Memos%' and OpenInvQty < 0 and U_DescFide < 0 then U_DescFide *-1
	else U_DescFide
	end U_DescFide,

case 
	when JrnlMemo like 'A/R Credit Memos%' and OpenInvQty < 0 and U_DescCComer < 0 then U_DescCComer *-1
	else U_DescCComer
	end U_DescCComer,


case 
	when JrnlMemo like 'A/R Credit Memos%' and OpenInvQty < 0 and Total_Bruto < 0 then Total_Bruto *-1
	else Total_Bruto
	end  Total_Documento,

case 
	when JrnlMemo like 'A/R Credit Memos%' and OpenInvQty < 0 and Dto_Total_SIN_IVA < 0 then Dto_Total_SIN_IVA *-1
	when case when JrnlMemo like 'A/R Credit Memos%' and OpenInvQty < 0 and Total_Bruto < 0 then Total_Bruto *-1 else Total_Bruto end <0 and Dto_Total_SIN_IVA >0 then Dto_Total_SIN_IVA *-1
	
	else Dto_Total_SIN_IVA
	end  Dto_Total_SIN_IVA,

case 
	when JrnlMemo like 'A/R Credit Memos%' and OpenInvQty < 0 and VENTA_NETA < 0 then VENTA_NETA *-1
	else VENTA_NETA
	end VENTA_NETA,

case 
	when JrnlMemo like 'A/R Credit Memos%' and OpenInvQty < 0 and convert(decimal(18,4), Dto_Total_SIN_IVA + Venta_Neta) < 0 then convert(decimal(18,4), Dto_Total_SIN_IVA + Venta_Neta) *-1	
	when JrnlMemo like 'Notas de crédito clientes%' then convert(decimal(18,4), Dto_Total_SIN_IVA - Venta_Neta) *-1
	else convert(decimal(18,4), Dto_Total_SIN_IVA + Venta_Neta)
	end VENTA_PP_SIN_IVA,

convert(decimal(18,0), case when (Dto_Total_SIN_IVA) < 1 then 0 else ( Dto_Total_SIN_IVA / (Dto_Total_SIN_IVA + Venta_Neta))end *100) [%Dto],
Escenario,
convert(varchar(500),[U_RBI_EXTCOD]) [U_RBI_EXTCOD]

FROM Whs.Comprobantes
group by 
Numero,
TargetType,
TrgetEntry,
Fecha_Contabilizacion,
JrnlMemo,
Tipo_Ducumento,
numerocompleto,
Codigo_BP_Base,
Codigo_Tributario,
Moneda,
Precio_Unitario,
InvQty,
OpenInvQty,
Tasa_Impositiva,
Impuesto_Total,
Total_Bruto,
Precio_Despues_Descuento,
PlanCuenta_id,
Departamento_id,
LugarCliente_id,
PropioTercero_id,
TipoProducto_id,
TipoComprobante_id,
Producto_id,
Transid,
Dscription,
U_Deslin,
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
IDS_AP,
Precio_Venta_Articulo,
CogsAcct,
CtlAccount,
AcctCode,
VendorNum,
Direccion,
Origen,
Codigo_Empleado_Venta,
VENTA_NETA,
Precio_Despues_Descuento,
U_DescFide,
U_DescCComer,
Total_Bruto,
Dto_Total_SIN_IVA ,
Numero_Linea,
ItemCode,
Escenario,
convert(varchar(500),[U_RBI_EXTCOD])


union all
select * from whs.VentasMensual

