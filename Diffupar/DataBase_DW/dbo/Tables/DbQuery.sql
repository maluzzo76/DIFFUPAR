CREATE TABLE [dbo].[DbQuery] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [TableId]        INT           NOT NULL,
    [TableDestinoId] INT           NULL,
    [Where]          VARCHAR (100) NULL,
    [Name]           VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([TableDestinoId]) REFERENCES [dbo].[DbTableStg] ([Id]),
    FOREIGN KEY ([TableId]) REFERENCES [dbo].[DbTables] ([Id])
);
GO

CREATE TRIGGER trgQueryInsert
ON DbQuery
FOR INSERT
AS
	insert into DbMapping
	select 
		i.Id,
		null,
		c.[name]
	from inserted i
	inner join DbTableStg t on t.id = i.TableDestinoId
	inner join sys.columns c on c.object_id = t.ObjectId
GO

