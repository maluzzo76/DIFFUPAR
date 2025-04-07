CREATE TABLE [dbo].[RabbitQueue]
(
	[Id] INT identity(1,1) NOT NULL PRIMARY KEY,
	Nombre varchar(100),
	ExchangeName Varchar(100),
	QueueName varchar(100),
	RoutingKey	varchar(100),
	UltimaEjecucion datetime,
	ProximaEjecucion datetime,
	Estado varchar(100),
	Origen varchar(100),
	tipo varchar(100),
	TableDestinoDw varchar(200)	
)
