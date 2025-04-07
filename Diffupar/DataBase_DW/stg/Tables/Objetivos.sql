CREATE TABLE [stg].[Objetivos]
(
	Id int identity(1,1) Primary Key,
	Anio int,
	mes int,
	LugarClienteCodigo int,
	LugarCliente varchar(100),
	ObjetivoMoneda decimal(18,4),
	ObjetivoCantidad decimal(18,4),
	IsNew bit,
	Origen varchar(100),
	UpdateDate datetime,
	Usuario varchar(100),
	PorcentajeDiffupar int
)
