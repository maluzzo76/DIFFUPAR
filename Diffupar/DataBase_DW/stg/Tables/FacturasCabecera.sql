CREATE TABLE [stg].[FacturasCabecera]
(
	DocEntry int, 
	DocNum int,
	DocType varchar(10), 
	CANCELED varchar(10),
	Handwrtten varchar(10) ,
	Printed varchar(10),
	DocStatus varchar(10),
	DocDate datetime,
	DocDueDate datetime,
	CardCode varchar(200),
	CardName varchar(200),
	CtlAccount varchar(100),
	[PTICode]    varchar(100) NULL,
    [Letter]     VARCHAR (100)  NULL,
    [NumAtCard]  VARCHAR (100)  NULL,
	FolNumTo varchar(100) null,
	JrnlMemo varchar(100) null,
	UpdateDate datetime,
	CreateDate datetime
)
