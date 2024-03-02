-------------------------------------------------------
----------- Carga los tipos de comprobates ------------
-------------------------------------------------------
insert into whs.DimTipoComprobantes(codigo, nombre) values('FAC','Factura')
insert into whs.DimTipoComprobantes(codigo, nombre) values('NC','Nota de Credito')

-------------------------------------------------------
------ Schedulle actualización Data Warehouse ---------
-------------------------------------------------------
insert into DBProcess
select '2024-02-19 02:00:00',null,'Actualización DW','Pendiente',null,'Data Warehouse',null,'D'


select top 10 
	fc.DocEntry,
	fc.DocNum ,
	fc.DocType, 
	fc.CANCELED,
	fc.Handwrtten,
	fc.Printed,
	fc.DocStatus,
	fc.DocDate,
	fc.DocDueDate,
	fc.CardCode,
	fc.CardName,
	fc.CtlAccount,
	fc.UpdateDate,
	fc.CreateDate,
	fd.LineNum,
	fd.ItemCode,
	fd.Price,
	fd.Currency,
	fd.LineTotal,
	fd.OpenSum,
	fd.SlpCode,
	fd.AcctCode,
	fd.PriceBefDi,
	fd.BaseCard,
	fd.TotalSumSy,
	fd.OpenSumSys,
	fd.VatPrcnt,
	fd.PriceAfVAT,
	fd.VatSum,
	fd.VatSumSy,
	fd.DedVatSum,
	fd.DedVatSumS,
	fd.GrssProfit,
	fd.GrssProfSC,
	fd.VisOrder,
	fd.INMPrice,
	fd.TaxCode,
	fd.LineVat,
	fd.LineVatS,
	fd.OwnerCode,
	fd.StockSum,
	fd.StockSumSc,
	fd.ShipToCode,
	fd.ShipToDesc,
	fd.GTotal,
	fd.GTotalSC,
	fd.TaxOnly,
	un.Id UnidadNegocioVentaId,
	cta.Id PlanCuenta_Id,
	dto.Id Departamento_Id,
	lc.Id LugarCliente_Id,
	pt.Id PropioTercero_Id,
	tp.Id TipoProducto_Id,
	ct.id TipoComprobante_Id,
	fd.GPBefDisc
from stg.FacturaDetalle fd
inner join stg.FacturasCabecera fc on fc.DocEntry = fd.DocEntry
left join whs.DimUnidadesNegocioVenta un on un.Codigo = fd.CogsOcrCod
left join whs.DimPlanDeCuentas cta on cta.AcctCode =fd.AcctCode
left join whs.DimDepartamentos dto on dto.Codigo = fd.CogsOcrCo2
left join whs.DimLugarCliente lc on lc.Codigo = fd.CogsOcrCo3
left join whs.DimPropioTercero pt on pt.Codigo = fd.CogsOcrCo4
left join whs.DimTiposProductos tp on tp.Codigo = fd.CogsOcrCo5
left join whs.DimTipoComprobantes ct on ct.Codigo = 'FAC'


