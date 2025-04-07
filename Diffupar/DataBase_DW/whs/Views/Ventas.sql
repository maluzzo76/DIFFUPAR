CREATE VIEW [whs].[Ventas]
	AS 
select * from whs.FactVentas with(nolock)
