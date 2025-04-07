CREATE VIEW [dbo].[V_RabbitMessageProcess]
	AS 
select m.*, r.tipo, r.Origen 
from dbo.RabbitMenssage m
inner join RabbitQueue r on r.id = m.RabbitQueue_Id
where m.estado = 'New' 
