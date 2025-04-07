Create PROCEDURE [rtl].[SP_InyectarCegid]
AS
---------------------------------------------------------------
-- Inyecto los datos de cabecera de cegid en el dw
---------------------------------------------------------------
delete rtl.FacturaCabeceraCegid
insert into rtl.FacturaCabeceraCegid
select 
	Fecha,
	Numero,
	TotalCIva,
	Venta_Neta,
	NumeroCompelto,
	Pk_Piece,
	Sucursal,
	Grupo,
	Representante,
	'CEGID' Orgien
from rtl.FacturaCabecera

---------------------------------------------------------------
-- Inyecto los datos de factura detalle de cegid en el dw
---------------------------------------------------------------
delete rtl.FacturaDetalleCegit
Insert into rtl.FacturaDetalleCegit
select 
	Gl_datepiece,
	tiempo,
	NroDoc,
	NroLinea,
	TipoArticulo,
	Gl_CODEARTICLE,
	Article_desc,
	Total_CIva,
	Venta_Neta,
	Fk_Piece,
	Pk_Ligne,
	Proveedor1,
	Rubro,
	Marca,
	Segmento,
	CantidadItems,
	GL_REMISELIBRE2,
	GL_REMISELIBRE1,
	GL_PUTTC,
	GA_CODEBARRE,
	'CEGID' Orgien
from rtl.FacturaDetalle


---------------------------------------------------------------
-- Cargo Maestro de Articulos
---------------------------------------------------------------
delete rtl.ProductosRetails
insert into rtl.ProductosRetails
select distinct
TipoArticulo TipoArticulo,
GL_CODEARTICLE codigoArticulo,
article_desc Descripcion,
PROVEEDOR1,
Rubro,
Marca,
SEGMENTO,
GA_CODEBARRE barCode
from rtl.FacturaDetalle


---------------------------------------------------------------
-- Normaliza los nombre de las sucursales
---------------------------------------------------------------
update rtl.FacturaCabeceraCegid set sucursal = 'ALTO ROSARIO MAC'
where Sucursal = 'MAC ALTO ROSARIO' and Origen = 'Napse'