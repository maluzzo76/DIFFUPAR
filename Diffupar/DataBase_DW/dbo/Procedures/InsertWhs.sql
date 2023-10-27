CREATE PROCEDURE [dbo].[InsertWhs]
AS
	delete whs.DimCategoria
delete whs.DimProveedorRetailCegid
delete whs.DimTipoProveedor
delete whs.DimProductos

insert into whs.DimCategoria
select * from stg.Categoria

insert into  whs.DimProveedorRetailCegid
select code, name, getdate(), getdate() from stg.ProveedorRetailCegid


insert into whs.DimTipoProveedor
select code, code, name,getdate(), getdate()  from stg.TipoProveedor


insert into whs.DimProductos
select p.ItemCode,p.Itemname,c.id,tp.id,pc.Id, p.CreateDate,p.UpdateDate
from stg.Productos p
left join whs.DimCategoria c on c.Codigo = p.U_RBI_Categoria
left join whs.DimProveedorRetailCegid pc on pc.Code = p.U_RBI_ProvRetailCegid
left join whs.DimTipoProveedor tp on tp.Codigo = p.U_RBI_TipoProveedor




