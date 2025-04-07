CREATE TABLE [stg].[NotaCreditoCabecera] (
    [DocEntry]   INT           NULL,
    [DocNum]     INT           NULL,
    [DocType]    VARCHAR (10)  NULL,
    [CANCELED]   VARCHAR (10)  NULL,
    [Handwrtten] VARCHAR (10)  NULL,
    [Printed]    VARCHAR (10)  NULL,
    [DocStatus]  VARCHAR (10)  NULL,
    [DocDate]    DATETIME      NULL,
    [DocDueDate] DATETIME      NULL,
    [CardCode]   VARCHAR (200) NULL,
    [CardName]   VARCHAR (200) NULL,
    [CtlAccount] VARCHAR (100) NULL,
    [PTICode]    VARCHAR (100) NULL,
    [Letter]     VARCHAR (100) NULL,
    [NumAtCard]  VARCHAR (100) NULL,
    [FolNumTo]   VARCHAR (100) NULL,
    [JrnlMemo]   VARCHAR (100) NULL,
    [UpdateDate] DATETIME      NULL,
    [CreateDate] DATETIME      NULL,
    [TransId]    VARCHAR (100) NULL,
    [Indicator]  VARCHAR (100) NULL,
    [U_RBI_EXTCOD] VARCHAR (200) NULL
);


