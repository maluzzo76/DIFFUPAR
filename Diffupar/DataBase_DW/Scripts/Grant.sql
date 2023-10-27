Create view whs.Productos
as
select
	p.Codigo,
	p.Nombre,
	c.Codigo Codigo_Categoria,
	c.Nombre Nombre_Categoria,
	pr.Code Codigo_ProveedorRetailCegid,
	pr.Name Nombre_ProveedorRetailCegid,
	tp.Codigo Codigo_TipoProveedor,
	Tp.Nombre Nombre_TipoProveedor
from whs.DimProductos p
left join whs.DimCategoria c on c.id = p.Categoria_Id
left join whs.DimProveedorRetailCegid pr on pr.Id = p.ProveedorRetailCegid_Id
left join whs.DimTipoProveedor tp on tp.id = p.TipoProveedor_Id

GRANT SELECT ON whs.Productos TO PabloOns