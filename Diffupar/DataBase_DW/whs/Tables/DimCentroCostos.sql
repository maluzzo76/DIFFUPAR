CREATE TABLE [whs].[DimCentroCostos]
(
	ID int identity(1,1) primary key,
	[AcctCode] varchar(100), -- Codigo Cuenta
	[AcctName] varchar(500), -- Nombre Cuenta
	[CreateDate] datetime,
	[UpdateDate] datetime
)
