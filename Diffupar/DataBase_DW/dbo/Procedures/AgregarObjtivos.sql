CREATE PROCEDURE [dbo].[AgregarObjtivos]
 @NombreUsuario varchar(100)
AS
DECLARE @periodo datetime,@periodoNew datetime

set @periodo = (select max(convert(datetime, cast(anio as varchar(5)) + '-' + cast(mes as varchar(2)) + '- 01')) fecha from stg.Objetivos)
set @periodoNew = (select DATEADD(month,1,@periodo))
insert into stg.objetivos 
select 
	year(@periodoNew),
	MONTH(@periodoNew),
	null, 
	s.Sucursal ,
	isnull(max(o.ObjetivoMoneda),0) ObjetivoMoneda,
	isnull(max(o.ObjetivoCantidad),0) ObjetivoCantidad,
	1,
	'OBJETIVOS TOTALES',
	GetDate(),
	@NombreUsuario,
	30

from DiffuparAnalyticsQA.rtl.FacturaCabecera s
left join stg.Objetivos o on o.LugarCliente collate French_CI_AS = s.Sucursal collate French_CI_AS and o.Anio = year(@periodo) and o.mes = month(@periodo)
group by
LugarClienteCodigo, 
s.Sucursal

