create view whs.Productos
as
select
	p.Id,
	p.Codigo,
	p.Nombre,
	c.Codigo Codigo_Categoria,
	c.Nombre Nombre_Categoria,
	pr.Code Codigo_ProveedorRetailCegid,
	pr.Name Nombre_ProveedorRetailCegid,
	tp.Codigo Codigo_TipoProveedor,
	Tp.Nombre Nombre_TipoProveedor,		
	fa.Code Codigo_Fabricante,
	fa.Name Nombre_Fabricante,
	ge.Code Codigo_Genero,
	ge.Name Nombre_Genero,
	ga.Code Codigo_GrupoArt,
	ga.Name Nombre_GrupoArt,
	li.Code Codigo_Linea,
	li.Name Nombre_Linea,
	ma.Code Codigo_Marca,
	ma.Name Nombre_Marca,
	tr.Code Codigo_TamanioReal,
	tr.Name Nombre_TamanioReal,
	tc.Code Codigo_TamanioConc,
	tc.Name Nombre_TamanioConc,
	tpr.codigo Codigo_TipoProducto,
	tpr.Tipo_Producto Nombre_TipoProducto,
	tpr.Proveedor Proveedor_TipoProducto,
	um.Code Codigo_UnidadMedida,
	um.Name Nombre_UnidadMedida,
	une.Code Codigo_UnidadNegocio,
	une.Name Nombre_UnidadNegocio,
	p.CreateDate Fecha_Creacion,
	p.UpdateDate Fecha_Modificacion,
	d4.Codigo Codigo_Propio_Tercero,
	d4.Propio_Tercero Nombre_Propio_Tercero,
	d4.Proveedor Nombre_Propio_Proveedor,
	gi.ItmsGrpCod Codigo_GrupoItem,
	gi.ItmsGrpNam Nomnre_GrupoItem


from whs.DimProductos p
left join whs.DimCategoria c on c.id = p.Categoria_Id
left join whs.DimFabricante fa on fa.id = p.Fabricante_Id
left join whs.DimGenero  ge on ge.id = p.Genero_Id	
left join whs.DimGruposArts ga on ga.id = p.GrupoArt_Id
left join whs.DimLineas li on li.id = p.Linea_Id
left join whs.DimMarcas ma on ma.id = p.Marca_Id 
left join whs.DimProveedorRetailCegid pr on pr.Id = p.ProveedorRetailCegid_Id
left join whs.DimTamanoReal tr on tr.id = p.TamanoReal_Id
left join whs.DimTamanosConc tc on tc.ID = p.TamanosConc_Id 
left join whs.DimTipoProveedor tp on tp.id = p.TipoProveedor_Id
left join whs.DimTiposProductos tpr on tpr.id = p.TipoProducto_Id
left join whs.DimUnidadesMedida um on um.id = p.UnidadMedida_Id
left join whs.DimUnidadesNegocio une on une.id = p.UnidadNegocio_Id
left join whs.DimPropioTercero d4 on d4.id = p.Propio_Tercero_Id
left join whs.DimGrupoItems gi on gi.id = p.Grupo_Item_Id

