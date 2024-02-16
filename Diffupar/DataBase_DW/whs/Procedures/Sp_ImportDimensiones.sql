CREATE PROCEDURE [stg].[Sp_ImportDimensiones]

AS

------------------------------------------------------------------------------------------------------
-- DELETE DIMENSIONES
------------------------------------------------------------------------------------------------------
--Dimensiones Principales
delete whs.FactComprobantes
delete whs.DimProductos
delete whs.DimBusinessPartner

--Dimensiones Secundarias
delete whs.DimCategoria
delete whs.DimFabricante
delete whs.DimGenero
delete whs.DimGruposArts
delete whs.DimLineas
delete whs.DimMarcas
delete whs.DimProveedorRetailCegid 
delete whs.DimTamanoReal
delete whs.DimTamanosConc
delete whs.DimTipoProveedor 
delete whs.DimTiposProductos
delete whs.DimUnidadesMedida
delete whs.DimUnidadesNegocio
delete whs.DimGrupoItems
delete whs.DimProvincias
delete whs.DimVendedores
delete whs.DimCondicionesPago
delete whs.DimBusinessPartnerGroup
delete whs.DimAlmacenes
delete whs.FactLibroMayor

------------------------------------------------------------------------------------------------------
-- INSERTA DIMENSIONES SECUNDARIAS
------------------------------------------------------------------------------------------------------
insert into whs.DimCategoria select code, [name], null, null from stg.Categoria
insert into whs.DimFabricante select code, [name], null, null from stg.Fabricante
insert into whs.DimGenero select code, [name], null, null from stg.Genero
insert into whs.DimGruposArts select code, [name], null, null from stg.GrupoArt
insert into whs.DimLineas select code, [name], null, null from stg.Linea
insert into whs.DimMarcas select code,[name], null, null from stg.Marca
insert into whs.DimProveedorRetailCegid select code, [name], null, null from stg.ProveedorRetailCegid
insert into whs.DimTamanoReal select code, [name], null, null from stg.TamanoReal
insert into whs.DimTamanosConc select code, [name], null, null from stg.TamanoConc
insert into whs.DimTipoProveedor select code, [name], null, null from stg.TipoProveedor
insert into whs.DimUnidadesMedida select code, [name], null, null from stg.UnidadMedida
insert into whs.DimUnidadesNegocio select code, [name], null, null from stg.UnidadNegocio
insert into whs.DimGrupoItems select ItmsGrpCod,ItmsGrpNam from stg.GrupoItems
insert into whs.DimProvincias select code,Country,[Name] from stg.Provincias
insert into whs.DimVendedores select SlpCode,SlpName from stg.Vendedores
insert into whs.DimCondicionesPago select GroupNum,PymntGroup from stg.CondicionPego
insert into whs.DimBusinessPartnerGroup select GroupCode, GroupName, GroupType from stg.BiusinessPartnerGruop
insert into whs.DimAlmacenes select WhsCode,WhsName, IntrnalKey from stg.Almacenes

------------------------------------------------------------------------------------------------------
-- INSERTA DIMENSIONES PARTICIONADAS DE LA 1 A LA 5
------------------------------------------------------------------------------------------------------
exec stg.Sp_ImportSapDimesionesProducto


------------------------------------------------------------------------------------------------------
-- INSERTA DIMENSION PRODUCTO
------------------------------------------------------------------------------------------------------

insert into whs.DimProductos
	select 
	ItemCode,
	Itemname,
	c.id,
	tp.id,
	prc.id,
	null,
	null,
	null,
	f.id,
	g.id,
	ga.id,
	l.id,
	m.id,
	tc.id,
	tr.id,
	d5.Id,
	um.id,
	une.id,
	p.CreateDate,
	p.UpdateDate,
	d4.id,
	gi.id
	

	from stg.Productos p
	left join whs.DimCategoria c on c.Codigo = replicate('0', 2 - len(p.U_RBI_Categoria))+p.U_RBI_Categoria
	left join whs.DimTipoProveedor tp on tp.Codigo = p.U_RBI_TipoProveedor
	left join whs.DimProveedorRetailCegid prc on prc.Code = p.U_RBI_ProvRetailCegid
	left join whs.DimFabricante f on f.Code = p.U_RBI_Fabricante
	left join whs.DimGenero g on trim(g.Code) =replicate('0',3-len(trim(p.U_RBI_Genero))) + trim(p.U_RBI_Genero)
	left join whs.DimGruposArts ga on ga.Code = p.U_RBI_GrupoArt
	left join whs.DimLineas l on l.Code = replicate('0',3-len(trim(p.U_RBI_Linea))) + trim(p.U_RBI_Linea)
	left join whs.DimMarcas m on m.Code =replicate('0',3-len(trim(p.U_RBI_Marca))) + trim(p.U_RBI_Marca)
	left join whs.DimTamanosConc tc on tc.Code = replicate('0',3-len(trim(p.U_RBI_TamanoConc))) + trim(p.U_RBI_TamanoConc)
	left join whs.DimTamanoReal tr on tr.Code = replicate('0',3-len(trim(p.U_RBI_TamanoReal))) + trim(p.U_RBI_TamanoReal)
	left join whs.DimUnidadesMedida um on um.Code = p.U_RBI_UM
	left join whs.DimUnidadesNegocio une on une.Code = p.U_RBI_UnideNegocio
	left join whs.DimPropioTercero d4 on d4.codigo = p.U_RBI_Dimension4
	left join whs.DimTiposProductos d5 on d5.Codigo = p.U_RBI_Dimension5
	left join whs.DimGrupoItems gi on gi.ItmsGrpCod = p.ItmsGrpCod


------------------------------------------------------------------------------------------------------
-- INSERTA DIMENSION BUSINESS PARTNER
------------------------------------------------------------------------------------------------------
insert into whs.DimBusinessPartner
select 
CardCode,
CardName,
CardType,
CmpPrivate,
bpg.id,
cp.id,
v.id,
p.id

from stg.BusinessPartner bp
left join whs.DimBusinessPartnerGroup bpg on bpg.GroupCode = bp.GroupCode
left join whs.DimCondicionesPago cp on cp.GroupNum = bp.GroupNum
left join whs.DimVendedores v on v.SlpCode = bp.SlpCode
left join whs.DimProvincias p on p.Codigo = bp.State1



------------------------------------------------------------------------------------------------------
-- INSERTA FAC COMPROBANTES FACTURA
------------------------------------------------------------------------------------------------------
insert into whs.FactComprobantes
select
	fc.DocEntry,
	fc.DocNum ,
	fc.DocType, 
	fc.CANCELED,
	fc.Handwrtten,
	fc.Printed,
	fc.DocStatus,
	fc.DocDate,
	fc.DocDueDate,	
	fc.CtlAccount,
	fc.UpdateDate,
	fc.CreateDate,
	fd.LineNum,	
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
	PTICode, 
	Letter,
	NumAtCard,
	FolNumTo,
	JrnlMemo,
	un.Id UnidadNegocioVentaId,
	cta.Id PlanCuenta_Id,
	dto.Id Departamento_Id,
	lc.Id LugarCliente_Id,
	pt.Id PropioTercero_Id,
	tp.Id TipoProducto_Id,
	ct.id TipoComprobante_Id,
	fd.GPBefDisc,
	bp.Id Cliente_Id,
	pr.Id
from stg.FacturaDetalle fd
inner join stg.FacturasCabecera fc on fc.DocEntry = fd.DocEntry
left join whs.DimUnidadesNegocioVenta un on un.Codigo = fd.CogsOcrCod
left join whs.DimPlanDeCuentas cta on cta.AcctCode =fd.AcctCode
left join whs.DimDepartamentos dto on dto.Codigo = fd.CogsOcrCo2
left join whs.DimLugarCliente lc on lc.Codigo = fd.CogsOcrCo3
left join whs.DimPropioTercero pt on pt.Codigo = fd.CogsOcrCo4
left join whs.DimTiposProductos tp on tp.Codigo = fd.CogsOcrCo5
left join whs.DimBusinessPartner bp on bp.CardCode = fc.CardCode
left join whs.DimProductos pr on pr.Codigo = fd.ItemCode
inner join whs.DimTipoComprobantes ct on ct.Codigo = 'FAC'

------------------------------------------------------------------------------------------------------
-- INSERTA FAC COMPROBANTES NOTAS DE CREDITOS
------------------------------------------------------------------------------------------------------
insert into whs.FactComprobantes
select
	fc.DocEntry,
	fc.DocNum ,
	fc.DocType, 
	fc.CANCELED,
	fc.Handwrtten,
	fc.Printed,
	fc.DocStatus,
	fc.DocDate,
	fc.DocDueDate,	
	fc.CtlAccount,
	fc.UpdateDate,
	fc.CreateDate,
	fd.LineNum,
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
	PTICode, 
	Letter,
	NumAtCard,
	FolNumTo,
	JrnlMemo,
	un.Id UnidadNegocioVentaId,
	cta.Id PlanCuenta_Id,
	dto.Id Departamento_Id,
	lc.Id LugarCliente_Id,
	pt.Id PropioTercero_Id,
	tp.Id TipoProducto_Id,
	ct.id TipoComprobante_Id,
	fd.GPBefDisc,
	bp.Id Cliente_Id,
	pr.id
from stg.NotaCreditoDetalle fd
inner join stg.NotaCreditoCabecera fc on fc.DocEntry = fd.DocEntry
left join whs.DimUnidadesNegocioVenta un on un.Codigo = fd.CogsOcrCod
left join whs.DimPlanDeCuentas cta on cta.AcctCode =fd.AcctCode
left join whs.DimDepartamentos dto on dto.Codigo = fd.CogsOcrCo2
left join whs.DimLugarCliente lc on lc.Codigo = fd.CogsOcrCo3
left join whs.DimPropioTercero pt on pt.Codigo = fd.CogsOcrCo4
left join whs.DimTiposProductos tp on tp.Codigo = fd.CogsOcrCo5
left join whs.DimBusinessPartner bp on bp.CardCode = fc.CardCode
left join whs.DimProductos pr on pr.Codigo = fd.ItemCode
inner join whs.DimTipoComprobantes ct on ct.Codigo = 'NC'

------------------------------------------------------------------------------------------------------
-- INSERTA FAC LIBRO MAYOR
------------------------------------------------------------------------------------------------------
insert into whs.FactLibroMayor
select 
	TransId,
	pc.id, 
	LineMemo,
	RefDate,
	Ref1,
	Ref2,
	BaseRef,
	Project,
	ProfitCode,
	OcrCode2,
	OcrCode3,
	OcrCode4,
	OcrCode5,
	Debit,
	Credit,
	'SAP',
	FCCurrency
from stg.jdt1 t1
left join whs.DimPlanDeCuentas pc on pc.AcctCode = t1.Account 
where RefDate >='2023-01-01' 
union all
select 
	TransId,
	pc.id, 
	LineMemo,
	RefDate,
	Ref1,
	Ref2,
	BaseRef,
	Project,
	ProfitCode,
	OcrCode2,
	OcrCode3,
	OcrCode4,
	OcrCode5,
	Debit,
	Credit,
	'SAP1',
	FCCurrency
from stg.JDT1Historica t1
left join whs.DimPlanDeCuentas pc on pc.AcctCode = t1.Account 
where RefDate >= '2023-08-01'




