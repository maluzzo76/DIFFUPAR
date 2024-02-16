CREATE TABLE [dbo].[DBProcess]
(
	[Id] INT identity(1,1) NOT NULL PRIMARY KEY,
	FechaCarga Datetime,
	FechaProcesado datetime,
	NombreArchivo varchar(100),
	Estado varchar(200),
	EstadoDescripcion varchar(max),
	TipoProceso varchar(100),
	Archivo varchar(max)

)
