--------------------------------------------------------------------------------------
-- Agrega las colas a consultar
--------------------------------------------------------------------------------------
insert into dbo.RabbitQueue
(Nombre, ExchangeName, QueueName, RoutingKey,UltimaEjecucion, ProximaEjecucion, Estado)
Values
('InventoryAdjustment',null,'InventoryAdjustment',null,null,getdate(),'Pendiente')

insert into dbo.RabbitQueue
(Nombre, ExchangeName, QueueName, RoutingKey,UltimaEjecucion, ProximaEjecucion, Estado)
Values
('InventoryAdjustment',null,'InventoryAdjustment',null,null,getdate(),'Pendiente')

insert into dbo.RabbitQueue
(Nombre, ExchangeName, QueueName, RoutingKey,UltimaEjecucion, ProximaEjecucion, Estado)
Values
('InventoryTransfer',null,'InventoryTransfer',null,null,getdate(),'Pendiente')

