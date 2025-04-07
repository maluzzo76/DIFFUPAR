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
	case when Ref1 like '%blan%' then null else Ref1 end Ref1,
	case when Ref2 like '%blan%' then null else Ref2 end Ref2,
	case when BaseRef like '%blan%' then null else BaseRef end BaseRef,
	case when Project like '%blan%' then null else Project end Project,
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
where codigo is not null

union all

select
	TransId,
	pc.id PlanCuenta_Id,
	codigo Codigo,
	LineMemo,
	RefDate,
	year(RefDate) anio,
	month(RefDate) mes,
	case when Ref1 like '%blan%' then null else Ref1 end Ref1,
	case when Ref2 like '%blan%' then null else Ref2 end Ref2,
	case when BaseRef like '%blan%' then null else BaseRef end BaseRef,
	case when Project like '%blan%' then null else Project end Project,
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
where codigo is not null

