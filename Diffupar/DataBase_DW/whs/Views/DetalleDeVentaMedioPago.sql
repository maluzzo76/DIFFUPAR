CREATE VIEW [rtl].[DetalleDeVentaMedioPago]
	AS 
select * from DiffuparAnalyticsQA.[rtl].[DetalleDeVentaPorMedioDePagoCegid]
where fecha > DATEADD(month,-6,getdate())

