CREATE VIEW [whs].[ListaPrecios]
	AS 
SELECT 
Lp.*,
P.Nombre,
P.Codigo_Marca,
P.Codigo_TipoProducto,
p.Codigo_TamanioConc

FROM whs.FactListaPrecios Lp with(nolock)
LEFT JOIN whs.Productos P with(nolock) on P.Id = Lp.Producto_Id

