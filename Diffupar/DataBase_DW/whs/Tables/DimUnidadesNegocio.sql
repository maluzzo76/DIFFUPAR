CREATE TABLE [whs].[DimUnidadesNegocio]
(
	ID int identity(1,1) primary key,
	[Code] varchar(100),
	[Name] varchar(200),
	[CreateDate] datetime,
	[UpdateDate] datetime
)
