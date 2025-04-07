CREATE VIEW [rtl].[Calendario]
	AS 
select sucursal, Anio, mes,Grupo from rtl.tcalendario

/*
select distinct
	r2.sucursal,
	Anio, mes ,
	case
		when	   r2.sucursal='ALTO AVELLANEDA'
				OR r2.sucursal='ALTO PALERMO GOND' 
				OR r2.sucursal ='IMPRENTA' 
				OR r2.sucursal ='VILLA DEL PARQUE' 
				OR r2.sucursal ='PASEO ALCORTA 2P'
				OR r2.sucursal ='MENDOZA PLAZA'
				OR r2.sucursal ='PATIO BULLRICH PB'
				OR r2.sucursal ='VILLA DEL PARQUE'
				OR r2.sucursal ='PATIO BULLRICH PB'
				OR r2.sucursal ='QUILMES FACTORY'
				OR r2.sucursal ='MARKETING RETAIL'
				OR r2.sucursal ='DISTRITO ARCOS GOND'
				OR r2.sucursal ='CREED ALVEAR'
				OR r2.sucursal ='CABILDO Y PAMPA'
				OR r2.sucursal ='ALTO ROSARIO'
				OR r2.sucursal ='ABASTO MAC'
				OR r2.sucursal ='ALTO PALERMO MAC'
				OR r2.sucursal ='ALTO ROSARIO MAC'
				OR r2.sucursal ='PALERMO SOHO MAC'
				OR r2.sucursal ='MENDOZA PLAZA GOND'
				OR r2.sucursal ='GALERIAS PACIFICO MAC'
				OR r2.sucursal ='PALMARES'
				OR r2.sucursal ='ALTO AVELLANEDA MALA' then 'ST NO COMPARABLE'
  
		when (LEFT(r2.sucursal,3)='B24' and r2.sucursal<>'B24 ECOMMERCE' )then 'BEAUTY 24'
		when r2.sucursal ='CONNECTE' THEN 'ECOMMERCE'
		when r2.sucursal ='B24 ECOMMERCE' THEN 'B24 ECOMMERCE'
		when r2.sucursal ='OUTLET' THEN 'OUTLET'
		ELSE 'ST COMPARABLE'
		END Grupo
from
(
	select
		distinct sucursal collate French_CI_AS Sucursal,
		year(fecha) anio, MONTH(fecha) mes
	from rtl.FacturaCabecera fc 
	where year(Fecha) <= year(getdate())
		 and  month(Fecha) < month(getdate())

	union 

	select distinct o.LugarCliente collate French_CI_AS Sucursal , anio,mes
	from whs.ObjetivosRetails o
) as r2


union all


select 
	r1.sucursal,
	year(getdate()) Anio, month(getdate()) mes  ,
	case
		when	   r1.sucursal='ALTO AVELLANEDA'
				OR r1.sucursal='ALTO PALERMO GOND' 
				OR r1.sucursal ='IMPRENTA' 
				OR r1.sucursal ='VILLA DEL PARQUE' 
				OR r1.sucursal ='PASEO ALCORTA 2P'
				OR r1.sucursal ='MENDOZA PLAZA'
				OR r1.sucursal ='PATIO BULLRICH PB'
				OR r1.sucursal ='VILLA DEL PARQUE'
				OR r1.sucursal ='PATIO BULLRICH PB'
				OR r1.sucursal ='QUILMES FACTORY'
				OR r1.sucursal ='MARKETING RETAIL'
				OR r1.sucursal ='DISTRITO ARCOS GOND'
				OR r1.sucursal ='CREED ALVEAR'
				OR r1.sucursal ='CABILDO Y PAMPA'
				OR r1.sucursal ='ALTO ROSARIO'
				OR r1.sucursal ='ABASTO MAC'
				OR r1.sucursal ='ALTO PALERMO MAC'
				OR r1.sucursal ='ALTO ROSARIO MAC'
				OR r1.sucursal ='PALERMO SOHO MAC'
				OR r1.sucursal ='MENDOZA PLAZA GOND'
				OR r1.sucursal ='GALERIAS PACIFICO MAC'
				OR r1.sucursal ='PALMARES'
				OR r1.sucursal ='ALTO AVELLANEDA MALA' then 'ST NO COMPARABLE'
  
		when (LEFT(r1.sucursal,3)='B24' and r1.sucursal<>'B24 ECOMMERCE' )then 'BEAUTY 24'
		when r1.sucursal ='CONNECTE' THEN 'ECOMMERCE'
		when r1.sucursal ='B24 ECOMMERCE' THEN 'B24 ECOMMERCE'
		when r1.sucursal ='OUTLET' THEN 'OUTLET'
		ELSE 'ST COMPARABLE'
		END Grupo
from
(
	select
		distinct sucursal collate French_CI_AS	Sucursal
	from rtl.FacturaCabecera fc 

	union 

	select distinct o.LugarCliente collate French_CI_AS Sucursal 
	from whs.ObjetivosRetails o
) as r1
*/