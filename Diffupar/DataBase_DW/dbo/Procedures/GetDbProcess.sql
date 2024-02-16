CREATE PROCEDURE [dbo].[GetDbProcess]
AS
select * from dbprocess where estado='Pendiente'
--if (select count(*) from dbprocess where estado='Procesando') = 0
--begin
--select top 1 * from dbprocess where estado='Pendiente'
--end
--else
--begin
--select * from dbprocess where estado='no existe'
--end

