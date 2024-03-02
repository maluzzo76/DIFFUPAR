CREATE PROCEDURE [dbo].[InsertProcessDW]
@processId int
AS
declare @fecha datetime, @Estado varchar(100)

set @Estado = (select MAX(id) from DBProcess where TipoProceso = 'Data Warehouse' and estado = 'Pendiente' )
set @fecha = (select  dateadd(DAY,1,FechaCarga) from DBProcess where id =@processId)

if isnull(@Estado,@processId) = @processId 
begin
insert into DBProcess select @fecha,null,'Actualización DW','Pendiente',null,'Data Warehouse',null,'D'
end
