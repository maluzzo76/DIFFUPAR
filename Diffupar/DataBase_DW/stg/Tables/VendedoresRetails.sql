CREATE TABLE [stg].[VendedoresRetails]
(
	[Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Codigo]   VARCHAR (10)  NULL,
    [Nombre]   VARCHAR (100) NULL,
    [Apellido] VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
)
