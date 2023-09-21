create procedure dbo.DbGetQuery(@Tid int)
as
select 
p.Name providerName,
t.Name tableName,
c.Name columnName

from DbTables t
inner join Dbsource s on s.id = t.DbSourceId
inner join DbProviders p on p.id = s.ProviderId
inner join DbColumns c on c.DbtableId = t.id
where t.id = @tid