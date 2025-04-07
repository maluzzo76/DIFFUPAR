CREATE PROCEDURE [rtl].[SP_IntectarVtaPorMedioPago]
AS

delete [rtl].[DetalleDeVentaPorMedioDePagoCegid]

insert into [rtl].[DetalleDeVentaPorMedioDePagoCegid]
select * from rtl.DetalleDeVantaPorMedioDePago