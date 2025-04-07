CREATE VIEW [dbo].[V_RabbitMessageProcess]
	AS 
select m.*, r.tipo, r.Origen 
from dbo.RabbitMenssage m with (nolock)
inner join RabbitQueue r with (nolock) on r.id = m.RabbitQueue_Id
where m.estado = 'New' 
and r.id <> 9
