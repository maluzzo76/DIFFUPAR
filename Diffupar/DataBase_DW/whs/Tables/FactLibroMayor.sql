CREATE TABLE [whs].[FactLibroMayor]
(
	[Id] INT identity(1,1) NOT NULL PRIMARY KEY,
	TransId varchar(100),
	PlanCuenta_Id int,
	LineMemo varchar(500),
	RefDate datetime,
	Ref1 varchar(200),
	Ref2 varchar(200),
	BaseRef varchar(200),
	Project varchar(200),
	profitCode int,
	OcrCode2 int,
	OcrCode3 int,
	OcrCode4 int,
	OcrCode5 int,
	Debit decimal(18,4),
	Credit decimal(18,4),
	Origen varchar(100) default 'SAP',
	FCCurrency varchar(100),
	Foreign key (PlanCuenta_Id) References whs.DimPlanDeCuentas(Id)
)
