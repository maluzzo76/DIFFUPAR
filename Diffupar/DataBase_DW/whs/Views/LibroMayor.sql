CREATE VIEW [whs].[LibroMayor]
	AS 
select 
	TransId ,	
	lm.PlanCuenta_Id,
	pc.Codigo,
	LineMemo,
	RefDate,
	year(RefDate) anio,
	month(RefDate) mes,
	Ref1,
	Ref2,
	BaseRef,
	Project,
	profitCode OcrCode1,
	OcrCode2,
	OcrCode3,
	OcrCode4,
	OcrCode5,	
	Origen,
	FCCurrency,
 Debit - Credit Saldo 
from whs.FactLibroMayor lm
left join whs.PlanCuentas pc on pc.PlanCuenta_Id = lm.PlanCuenta_Id

union all

select
	TransId,
	pc.id PlanCuenta_Id,
	codigo Codigo,
	LineMemo,
	RefDate,
	year(RefDate) anio,
	month(RefDate) mes,
	Ref1,
	Ref2,
	BaseRef,
	Project,
	OcrCode1,
	OcrCode2,
	OcrCode3,
	OcrCode4,
	OcrCode5,
	Origen,
	FCCurrency,
	saldo
from stg.JDT1Complementos t1
left join whs.DimPlanDeCuentas pc on pc.AcctCode = t1.Codigo 


