--Ejecutar cuando se crea una nueva tabla
insert into [dbo].[DbTableStg] 
select t.object_id, s.name + '.' + t.name  from sys.tables t
inner join sys.schemas s on s.schema_id = t.schema_id
where s.name = 'stg'
and t.name not like '%Complemento%'
and not exists (select name from DbTableStg where DbTableStg.name = s.name + '.' + t.name )





--Ejecutar cuando se crea una columna y existe un db query
declare @tableName varchar(100) = 'DIFFUPARSA.OWHS', @TableId int, @queryID int, @TableDestinoId int, @tableDestinoName varchar(200)
-- Setea las variables
set @TableId = (select Id  from DbTables where name = @tableName)
set @queryID = (select id from DbQuery where TableId = @TableId)
set @TableDestinoId = (select TableDestinoId from DbQuery where id = @queryID)
set @tableDestinoName = (select REPLACE(name,'stg.','') from DbTableStg where id = @TableDestinoId)

--select * from DbMapping where QueryId = @queryID

delete DbMapping where QueryId = @queryID

insert into DbMapping
select @queryID,co.id, c.name from sys.tables t
inner join sys.columns c on (c.object_id = t.object_id 
							and not exists (select st.ColumnDestino from DbMapping st where st.QueryId = @queryID and trim(st.ColumnDestino) =trim(c.name)))

left join DbColumns co on co.DbtableId = @TableId and co.Name = c.name
where t.name = @tableDestinoName



