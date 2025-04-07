CREATE TABLE [whs].[DimTipoCambio]
(
	[Id] INT Identity (1,1) NOT NULL PRIMARY KEY,
	Fecha Datetime,
	Moneda Varchar (100),
	TipoCambio decimal (18,4)
)
