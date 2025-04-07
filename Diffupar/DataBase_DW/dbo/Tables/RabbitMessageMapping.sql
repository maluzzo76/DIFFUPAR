CREATE TABLE [dbo].[RabbitMessageMapping]
(
	Id int identity(1,1) primary key,	
	ColumnName varchar(200),
	JsonAttribute varchar(200),
	RabbitQueueId int,
	foreign key (RabbitQueueId) References dbo.RabbitQueue(Id)
)
