CREATE PROCEDURE [stg].[Sp_ImportDimensiones]

AS

------------------------------------------------------------------------------------------------------
-- DELETE DIMENSIONES
------------------------------------------------------------------------------------------------------
--Dimensiones Principales
delete whs.DimProductos
delete whs.DimBusinessPartner

--Dimensiones Secundarias
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
delete whs.DimGrupoItems
delete whs.DimProvincias
delete whs.DimVendedores
delete whs.DimCondicionesPago
delete whs.DimBusinessPartnerGroup
delete whs.DimAlmacenes

------------------------------------------------------------------------------------------------------
-- INSERTA DIMENSIONES SECUNDARIAS
------------------------------------------------------------------------------------------------------
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
insert into whs.DimGrupoItems select ItmsGrpCod,ItmsGrpNam from stg.GrupoItems
insert into whs.DimProvincias select code,Country,[Name] from stg.Provincias
insert into whs.DimVendedores select SlpCode,SlpName from stg.Vendedores
insert into whs.DimCondicionesPago select GroupNum,PymntGroup from stg.CondicionPego
insert into whs.DimBusinessPartnerGroup select GroupCode, GroupName, GroupType from stg.BiusinessPartnerGruop
insert into whs.DimAlmacenes select WhsCode,WhsName, IntrnalKey from stg.Almacenes

------------------------------------------------------------------------------------------------------
-- INSERTA DIMENSIONES PARTICIONADAS DE LA 1 A LA 5
------------------------------------------------------------------------------------------------------
exec stg.Sp_ImportSapDimesionesProducto


------------------------------------------------------------------------------------------------------
-- INSERTA DIMENSION PRODUCTO
------------------------------------------------------------------------------------------------------

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
	d5.Id,
	um.id,
	une.id,
	p.CreateDate,
	p.UpdateDate,
	d4.id,
	gi.id
	

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
	left join whs.DimPropioTercero d4 on d4.codigo = p.U_RBI_Dimension4
	left join whs.DimTiposProductos d5 on d5.Codigo = p.U_RBI_Dimension5
	left join whs.DimGrupoItems gi on gi.ItmsGrpCod = p.ItmsGrpCod


------------------------------------------------------------------------------------------------------
-- INSERTA DIMENSION BUSINESS PARTNER
------------------------------------------------------------------------------------------------------
insert into whs.DimBusinessPartner
select 
CardCode,
CardName,
CardType,
CmpPrivate,
bpg.id,
cp.id,
v.id,
p.id

from stg.BusinessPartner bp
left join whs.DimBusinessPartnerGroup bpg on bpg.GroupCode = bp.GroupCode
left join whs.DimCondicionesPago cp on cp.GroupNum = bp.GroupNum
left join whs.DimVendedores v on v.SlpCode = bp.SlpCode
left join whs.DimProvincias p on p.Codigo = bp.State1
