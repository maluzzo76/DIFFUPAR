CREATE TABLE [stg].[config] (
    [Id]     INT           IDENTITY (1, 1) NOT NULL,
    [Nombre] VARCHAR (100) NULL,
    [Valor]  VARCHAR (200) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

