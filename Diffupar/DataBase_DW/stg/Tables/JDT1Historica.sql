CREATE TABLE [stg].[JDT1Historica]
(
	TransId int,
	Account varchar(100),
	LineMemo varchar(500),
	RefDate datetime,
	Ref1 varchar(200),
	Ref2 varchar(200),
	BaseRef varchar(200),
	Project varchar(200),
	ProfitCode varchar(100),
	OcrCode2 varchar(100),
	OcrCode3 varchar(100),
	OcrCode4 varchar(100),
	OcrCode5 varchar(100),
	Debit decimal(18,4),
	Credit decimal(18,4),
	FCCurrency varchar(100)
)
