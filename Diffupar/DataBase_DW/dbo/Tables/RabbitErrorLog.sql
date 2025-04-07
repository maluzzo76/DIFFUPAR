CREATE TABLE [dbo].[RabbitErrorLog]
(
	[Id] INT identity(1,1) NOT NULL PRIMARY KEY,
	ProcessDate datetime,
	RabbitQueueId int,
	ErrorMessage text,
	Excepcion text,
	ProcessId uniqueidentifier
)
