create procedure dbo.GetSchedule
@scheduleid int 
as
select
q.id queryId,
p.Name providerName,
td.Name tableDestino,
t.Name tableOrgien,
q.[Where] [where],
so.Connetion sourceConnection,
null destinoConnection
from dbschedule s
inner join DbQuery q on q.id = s.queryId
inner join DbTables t on t.Id = q.TableId
inner join DbTableStg td on td.id = q.TableDestinoId
inner join DbSource so on so.id = t.DbSourceId
inner join DbProviders p on p.id = so.ProviderId
where s.id = @scheduleid

select 
c.Name columnOrigen,
mp.ColumnDestino

from DbMapping mp 
inner join DbQuery q on q.id = mp.QueryId
inner join DbColumns c on c.id = mp.ColumnSourceId
inner join dbschedule s on s.queryId =q.Id 
where s.id = @scheduleid

