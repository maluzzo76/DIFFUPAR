CREATE TABLE [whs].[DimDepartamentos]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	Codigo int,
	Departamento varchar(255),
	Area varchar(255)
)
