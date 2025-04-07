CREATE PROCEDURE [rtl].[SP_InyectarCalendarioCegid]	
AS
DECLARE @FechaDesde datetime, @FechaFin Datetime

SET @FechaDesde  = (select min(Fecha_Contabilizacion) from DiffuparAnalyticsQA.rtl.VentaComercial)
SET @FechaFin = (select DATEADD(day,1,getdate()))

declare @Sucursales table(codigo int,sucursal varchar(255))

insert into @Sucursales
select distinct CodigoTienda, upper(Sucursal) Sucursal from rtl.VentaComercial
--select distinct NombreTienda from DiffuparAnalyticsQA.rtl.ScursalesRetails

delete rtl.TCalendario

WHILE ( @FechaDesde <= @FechaFin)
BEGIN
	SET @FechaDesde  = DATEADD(month,1,@FechaDesde)

	INSERT INTO rtl.TCalendario
	SELECT
		codigo,
		sucursal,
		year(@FechaDesde),
		month(@FechaDesde),
		case
		when	Sucursal ='ALTO AVELLANEDA'
				OR Sucursal ='ALTO PALERMO GONDOLA' 
				OR Sucursal ='IMPRENTA' 
				OR Sucursal ='VILLA DEL PARQUE' 
				OR Sucursal ='PASEO ALCORTA 2P'
				OR Sucursal ='MENDOZA PLAZA'
				OR Sucursal ='PATIO BULLRICH PB'
				OR Sucursal ='VILLA DEL PARQUE'
				OR Sucursal ='PATIO BULLRICH PB'
				OR Sucursal ='QUILMES FACTORY'
				OR Sucursal ='MARKETING RETAIL'
				OR Sucursal ='DISTRITO ARCOS GOND'
				OR Sucursal ='CREED ALVEAR'
				OR Sucursal ='CABILDO Y PAMPA'
				OR Sucursal ='ALTO ROSARIO'
				OR Sucursal ='ABASTO MAC'
				OR Sucursal ='ALTO PALERMO MAC'
				OR Sucursal ='MAC ALTO ROSARIO'
				OR Sucursal ='ALTO ROSARIO MAC'
				OR Sucursal ='PALERMO SOHO MAC'
				OR Sucursal ='MENDOZA PLAZA GOND'
				OR Sucursal ='GALERIAS PACIFICO MAC'
				OR Sucursal ='PALMARES'
				OR Sucursal ='ALTO AVELLANEDA MALA' then 'ST NO COMPARABLE'
		when (LEFT(Sucursal,3)='B24' and Sucursal<>'B24 ECOMMERCE' )then 'BEAUTY 24'
		when Sucursal='CONNECTE' THEN 'ECOMMERCE'
		when Sucursal='B24 ECOMMERCE' THEN 'B24 ECOMMERCE'
		when Sucursal='OUTLET' THEN 'OUTLET'
		ELSE 'ST COMPARABLE'
		END Grupo
	FROM  @Sucursales	
END
