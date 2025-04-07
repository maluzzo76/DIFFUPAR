 CREATE VIEW [dbo].[CegidVentaDetalle]
	AS
alter view dbo.v_CegidVenta
as
select *
from(
select 
L.GL_DATEPIECE Fecha,
CONVERT(TIME, l.GL_DATECREATION) as 'tiempo',
l.GL_NUMERO,--- as nro_doc_cegid,
l.GL_NUMLIGNE,--- as nro_linea,
l.GL_TYPEARTICLE,---- as tipo_articulo,
l.GL_QTEFACT,---- as cant_linea,
l.GL_CODEARTICLE,
l.GL_PUTTCBASE ,----- as precio_ud_incl_IVA_base_linea,
l.GL_PUTTC,---- as precio_ud_IVA_incl_linea,
l.GL_PUHT,---- as precio_ud_excl_Iva_linea,
l.GL_REMISELIGNE,--- as descuento_porcentaje_linea,
l.GL_TOTREMLIGNE,--- as total_descuento_com_linea,
l.GL_REMISELIBRE2,--- as total_cond_comercial,
l.GL_REMISELIBRE3,---- as total_descuento,
l.GL_REMISELIBRE1,---- as total_desc_fidelidad,
l.GL_TOTALTAXE1,---- as total_tasa_1_linea,
l.GL_TOTALHT Venta_Neta,--- as 'total exc IVA linea',
l.GL_TOTALTTC ,--- as 'total_IVA_inc_linea',

L.GL_QTERESTE,
l.GL_COLLECTION,---- as 'COD. INCENTIVO',

A.GA_LIBELLE AS article_desc,
CONCAT( l.GL_ETABLISSEMENT, l.GL_NUMERO) as U_RBI_EXTERCODE,
CONCAT( l.GL_ETABLISSEMENT, l.GL_NUMERO, l.GL_NUMLIGNE) as pk_ligne,
---cast( CONCAT( cast (CH1.YX_CODE as int),cast(T.T_TIERS as int)) as int)as pk_costv1,
-------CONCAT( CH1.YX_CODE ,T.T_TIERS, FN4.cc_code )as pk_cost,
A.GA_CODEARTICLE as CODEARTICLE2,
A.GA_CODEBARRE,
A.GA_LIBELLE AS descripcion_articulo,
T.T_ABREGE AS PROVEEDOR2,
T.T_TIERS,
T.T_LIBELLE AS PROVEEDOR1,
CH1.YX_LIBELLE AS RUBRO,
CH1.YX_CODE,
CH1.YX_TYPE,
FN4.cc_code as code_marca,
FN4.CC_LIBELLE as marca,
LA9.SEG_CODE AS SEG_CODE,
LA9.SEGMENTO AS SEGMENTO
from [200.32.54.90].Produccion.[dbo].[LIGNE] AS l
inner JOIN [200.32.54.90].Produccion.[dbo].ARTICLE AS A ON A.GA_CODEARTICLE=L.GL_CODEARTICLE
inner join [200.32.54.90].Produccion.[dbo].ARTICLECOMPL as ac on ac.GA2_ARTICLE=A.GA_ARTICLE
LEFT JOIN
(SELECT 
T_TIERS,
T_LIBELLE,
T_ABREGE

FROM [200.32.54.90].Produccion.[dbo].TIERS 
WHERE T_NATUREAUXI='FOU'---fou solo trae proveedores y no clientes
)AS T ON a.GA_FOURNPRINC=T.T_TIERS  ---Join por art. para tener los proveedores
left JOIN 
(
select 
YX_LIBELLE,---RUBRO
YX_CODE,---CODIGO RUBRO
YX_TYPE

from [200.32.54.90].Produccion.[dbo].CHOIXEXT

where YX_CODE<>'304' ---se excluyen los articulos publicitarios
and YX_TYPE='LA2'---Filtra por el rubro

) AS CH1 ON CH1.YX_CODE=A.GA_LIBREART2 
LEFT JOIN (
SELECT * 
FROM [200.32.54.90].Produccion.[dbo].CHOIXCOD 
WHERE CC_TYPE='FN4'---FILTRO PARA TRAER MARCA
) as FN4 on AC.GA2_FAMILLENIV4=FN4.CC_CODE
 LEFT JOIN (
SELECT
YX_CODE AS SEG_CODE,
YX_LIBELLE AS SEGMENTO
from  [200.32.54.90].Produccion.[dbo].CHOIXEXT
where YX_TYPE='LA9' ---FILTRO PARA TRAER SEGMENTO

) as LA9 ON A.GA_LIBREART9=LA9.SEG_CODE



where  convert (DATE,l.GL_DATEPIECE)>='2019-01-01'
and l.GL_NATUREPIECEG='FFO'---trae lineas de ticket de retail por caja
--And CH1.YX_CODE<>304 ---se excluyen los articulos publicitarios
-----and L.GL_ETABLISSEMENT<>1401 ---SE descarta el outlet

and l.GL_TYPECOM<>'TRE'
and A.GA_CODEARTICLE <>'IN' ----Se filtran los intereses de ventas por tarjeta
) as r1


