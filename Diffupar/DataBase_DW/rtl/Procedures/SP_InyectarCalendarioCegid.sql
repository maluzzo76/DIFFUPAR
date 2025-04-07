CREATE PROCEDURE [rtl].[SP_InyectarCalendarioCegid]	
AS
DECLARE @FechaDesde datetime, @FechaFin Datetime

SET @FechaDesde  = (select min(Fecha_Contabilizacion) from DiffuparAnalyticsQA.rtl.VentaComercial)
SET @FechaFin = (select getdate())

declare @Sucursales table(sucursal varchar(255))

insert into @Sucursales
select distinct sucursal from DiffuparAnalyticsQA.rtl.VentaComercial

delete rtl.TCalendario

WHILE ( @FechaDesde <= @FechaFin)
BEGIN
	SET @FechaDesde  = DATEADD(month,1,@FechaDesde)

	INSERT INTO rtl.TCalendario
	SELECT
		sucursal,
		year(@FechaDesde),
		month(@FechaDesde),
		case
		when	   sucursal='ALTO AVELLANEDA'
				OR sucursal='ALTO PALERMO GOND' 
				OR sucursal ='IMPRENTA' 
				OR sucursal ='VILLA DEL PARQUE' 
				OR sucursal ='PASEO ALCORTA 2P'
				OR sucursal ='MENDOZA PLAZA'
				OR sucursal ='PATIO BULLRICH PB'
				OR sucursal ='VILLA DEL PARQUE'
				OR sucursal ='PATIO BULLRICH PB'
				OR sucursal ='QUILMES FACTORY'
				OR sucursal ='MARKETING RETAIL'
				OR sucursal ='DISTRITO ARCOS GOND'
				OR sucursal ='CREED ALVEAR'
				OR sucursal ='CABILDO Y PAMPA'
				OR sucursal ='ALTO ROSARIO'
				OR sucursal ='ABASTO MAC'
				OR sucursal ='ALTO PALERMO MAC'
				OR sucursal ='ALTO ROSARIO MAC'
				OR sucursal ='PALERMO SOHO MAC'
				OR sucursal ='MENDOZA PLAZA GOND'
				OR sucursal ='GALERIAS PACIFICO MAC'
				OR sucursal ='PALMARES'
				OR sucursal ='ALTO AVELLANEDA MALA' then 'ST NO COMPARABLE'
  
		when (LEFT(sucursal,3)='B24' and sucursal<>'B24 ECOMMERCE' )then 'BEAUTY 24'
		when sucursal ='CONNECTE' THEN 'ECOMMERCE'
		when sucursal ='B24 ECOMMERCE' THEN 'B24 ECOMMERCE'
		when sucursal ='OUTLET' THEN 'OUTLET'
		ELSE 'ST COMPARABLE'
		END Grupo
	FROM  @Sucursales	
END
