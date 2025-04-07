CREATE TABLE [dbo].[RabbitMenssage]
(
	[Id] INT identity(1,1) NOT NULL PRIMARY KEY,
	Msg text,
	Estado varchar(100),
	RabbitQueue_Id int,
	ImportDate datetime,
	ProcessId uniqueidentifier,
	JsonTransactionID varchar(max),
	Foreign key (RabbitQueue_Id) references dbo.RabbitQueue(Id)
)
