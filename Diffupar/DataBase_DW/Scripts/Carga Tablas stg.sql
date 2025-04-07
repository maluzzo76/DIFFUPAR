--Ejecutar cuando se crea una nueva tabla
insert into [dbo].[DbTableStg] 
select t.object_id, s.name + '.' + t.name  from sys.tables t
inner join sys.schemas s on s.schema_id = t.schema_id
where s.name = 'stg'
and t.name not like '%Complemento%'
and not exists (select name from DbTableStg where DbTableStg.name = s.name + '.' + t.name )


-- Cargar columnas a DBMAPPING ejecutar por cada tabla creada cuando se agregan columnas
declare @idtd int, @idQ int, @tableDestinoName varchar(100)='productos'

set @idtd =(select top 1 id from DbTableStg where name = 'stg.'+ @tableDestinoName)

set @idQ =(select top 1 id from DbQuery where TableDestinoId = @idtd)

select * from DbMapping where QueryId = @idQ

insert into DbMapping
select @idQ,null, c.name from sys.tables t
inner join sys.columns c on (c.object_id = t.object_id and not exists (select st.ColumnDestino from DbMapping st where st.QueryId = @idQ and trim(st.ColumnDestino) =trim(c.name)))
where t.name = @tableDestinoName

