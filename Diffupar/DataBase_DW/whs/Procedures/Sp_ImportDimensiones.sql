CREATE PROCEDURE [stg].[Sp_ImportDimensiones]
AS

delete whs.DimProductos
delete whs.DimCategoria
delete whs.DimFabricante
delete whs.DimGenero
delete whs.DimGruposArts
delete whs.DimLineas
delete whs.DimMarcas
delete whs.DimProveedorRetailCegid 
delete whs.DimTamanoReal
delete whs.DimTamanosConc
delete whs.DimTipoProveedor 
delete whs.DimTiposProductos
delete whs.DimUnidadesMedida
delete whs.DimUnidadesNegocio


insert into whs.DimCategoria select code, [name], null, null from stg.Categoria
insert into whs.DimFabricante select code, [name], null, null from stg.Fabricante
insert into whs.DimGenero select code, [name], null, null from stg.Genero
insert into whs.DimGruposArts select code, [name], null, null from stg.GrupoArt
insert into whs.DimLineas select code, [name], null, null from stg.Linea
insert into whs.DimMarcas select code, [name], null, null from stg.Marca
insert into whs.DimProveedorRetailCegid select code, [name], null, null from stg.ProveedorRetailCegid
insert into whs.DimTamanoReal select code, [name], null, null from stg.TamanoReal
insert into whs.DimTamanosConc select code, [name], null, null from stg.TamanoConc
insert into whs.DimTipoProveedor select code, [name], null, null from stg.TipoProveedor
insert into whs.DimUnidadesMedida select code, [name], null, null from stg.UnidadMedida
insert into whs.DimUnidadesNegocio select code, [name], null, null from stg.UnidadNegocio

insert into whs.DimProductos
	select 
	ItemCode,
	Itemname,
	c.id,
	tp.id,
	prc.id,
	null,
	null,
	null,
	f.id,
	g.id,
	ga.id,
	l.id,
	m.id,
	tc.id,
	tr.id,
	null,
	um.id,
	une.id,
	p.CreateDate,
	p.UpdateDate

	from stg.Productos p
	left join whs.DimCategoria c on c.Codigo =  p.U_RBI_Categoria
	left join whs.DimTipoProveedor tp on tp.Codigo = p.U_RBI_TipoProveedor
	left join whs.DimProveedorRetailCegid prc on prc.Code = p.U_RBI_ProvRetailCegid
	left join whs.DimFabricante f on f.Code = p.U_RBI_Fabricante
	left join whs.DimGenero g on g.Code = p.U_RBI_Genero 
	left join whs.DimGruposArts ga on ga.Code = p.U_RBI_GrupoArt
	left join whs.DimLineas l on l.Code = p.U_RBI_Linea
	left join whs.DimMarcas m on m.Code = p.U_RBI_Marca
	left join whs.DimTamanosConc tc on tc.Code = p.U_RBI_TamanoConc
	left join whs.DimTamanoReal tr on tr.Code = p.U_RBI_TamanoReal
	left join whs.DimUnidadesMedida um on um.Code = p.U_RBI_UM
	left join whs.DimUnidadesNegocio une on une.Code = p.U_RBI_UnideNegocio

