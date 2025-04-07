select top 1 * from rtl.FacturaCabecera fc --Factura Cabecera

Select top 1 * from rtl.FacturaDetalle fc -- Factura detalle

select distinct sucursal from rtl.FacturaCabecera fc -- Sucursales

Select distinct Proveedor1 from rtl.FacturaDetalle where proveedor1 like '%facsa%' -- Proveedores

Select distinct Rubro from rtl.FacturaDetalle  -- Rubro

Select distinct Marca from rtl.FacturaDetalle  -- Marca

Select distinct Segmento from rtl.FacturaDetalle  -- Segmento

select distinct Grupo from rtl.FacturaCabecera fc --Direccion