CREATE PROCEDURE [dbo].[InsertRabbitMessageMappingColumns]
	@RabbitQueueId int
AS

declare @Tablename varchar(100)
set @Tablename = (select TableDestinoDw from RabbitQueue where id = @RabbitQueueId)


insert into dbo.RabbitMessageMapping
select c.name columnsName,null, @RabbitQueueId from sys.tables t
inner join sys.columns c on c.object_id = t.object_id 
left join dbo.RabbitMessageMapping m on m.ColumnName = c.name and RabbitQueueId = @RabbitQueueId
where t.name = @Tablename
and m.ColumnName is null