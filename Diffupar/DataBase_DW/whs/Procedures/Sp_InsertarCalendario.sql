CREATE PROCEDURE [dbo].[Sp_InsertarCalendario]
AS

DECLARE @FechaDesde datetime, @FechaFin Datetime,@añoIncio varchar(4),@añoFin varchar(4)

SET @añoIncio  = cast( (select min(year(Fecha_Contabilizacion)) from whs.Ventas) as varchar(4))
SET @añoFin = cast( (select max(year(Fecha_Contabilizacion )) from whs.Ventas)  as varchar(4))

SET  @FechaDesde =  convert(datetime, @añoIncio + '/1/1')
SET @FechaFin = convert(datetime, @añoFin + '/12/31')

DELETE whs.factcalendario

WHILE ( @FechaDesde <= @FechaFin)
BEGIN
	INSERT INTO whs.factcalendario
	SELECT
		@FechaDesde,
		DATEADD(year,-1,@FechaDesde),
		vta.OcrCode,
		null,
		vta.OcrCode3,
		null,
		vta.OcrCode5 ,
		0,
		VendorNum
	FROM  whs.Ventas vta

	GROUP BY vta.OcrCode, vta.OcrCode3,vta.OcrCode5,VendorNum

    SET @FechaDesde  = DATEADD(day,1,@FechaDesde)


END
