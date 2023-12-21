CREATE TABLE [whs].[DimAlmacenes]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	WhsCode varchar(100),
    WhsName varchar(100),
    IntrnalKey varchar(100)
)
