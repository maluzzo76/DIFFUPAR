CREATE TRIGGER trgQueryInsert
ON DbQuery
FOR INSERT
AS
	insert into DbMapping
	select 
		i.Id,
		null,
		c.name
	from inserted i
	inner join DbTableStg t on t.id = i.TableDestinoId
	inner join sys.columns c on c.object_id = t.ObjectId	

GO