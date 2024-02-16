CREATE VIEW [whs].[PlanCuentas]
	AS 
Select 
	Id PlanCuenta_Id,
	AcctCode Codigo,
	LevelName1 Nivel1,
	LevelName2 Nivel2,
	LevelName3 Nivel3,
	LevelName4 Nivel4,
	LevelName5 Nivel5,
	AcctCurrent Moneda,
	Financial Es_Financiera,
	case 
		when Level5 <>'000' then 5
		when Level4 <>'00' then 4
		when Level3 <>'000' then 3
		when Level2 <>'0' then 2
		else 1
	end	NivelCuenta,
	CreateDate Fecha_Creacion,
	UpdateDate Fecha_Modificacion
from whs.DimPlanDeCuentas with(nolock)

