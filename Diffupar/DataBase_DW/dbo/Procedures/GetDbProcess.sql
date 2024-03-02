CREATE PROCEDURE [dbo].[GetDbProcess]
AS
select * from dbprocess where estado ='Pendiente' order by id 

