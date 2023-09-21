insert into [dbo].[DbTableStg] 
select t.object_id, s.name + '.' + t.name  from sys.tables t
inner join sys.schemas s on s.schema_id = t.schema_id
where s.name = 'stg'
and not exists (select name from DbTableStg where DbTableStg.name = s.name + '.' + t.name)