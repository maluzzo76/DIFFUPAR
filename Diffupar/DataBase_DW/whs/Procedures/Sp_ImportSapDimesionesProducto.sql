Create PROCEDURE [stg].[Sp_ImportSapDimesionesProducto]
	
AS

DECLARE @code varchar(100), @name varchar(500)
declare @value varchar(200), @text1 varchar(300), @text2 varchar(300), @index int = 0

delete whs.DimUnidadesNegocioVenta
delete whs.DimDepartamentos
delete whs.DimLugarCliente
delete whs.DimPropioTercero
delete whs.DimTiposProductos

------------------------------------------------------------------------------------------------------
-- INSERTA DIMENSION 1
------------------------------------------------------------------------------------------------------
declare
    cur_dim cursor for
      select code, [name] from stg.Dimensiones_OOCR where Code between 10000 and 20000;
BEGIN
   OPEN cur_dim;
   FETCH  cur_dim INTO @code,@name
	WHILE (@@FETCH_STATUS = 0)
		BEGIN
			
			declare cur2 cursor for
				select value from string_split(@name,'-')
				begin
					open cur2
						FETCH  cur2 INTO @value						
						WHILE (@@FETCH_STATUS = 0) --If the fetch went well then we go for it
							BEGIN
							if @index = 0
							begin
								set @text1 = @value
								set @index = 1
							end
							else
							begin
								set @text2 = @value
								set @index = 0
							end
							FETCH  cur2 INTO @value							
							END
							insert into whs.DimUnidadesNegocioVenta(Codigo,Direccion,Unidad_Negocio )values(@code ,@text1,@text2)
					close cur2
					DEALLOCATE cur2
				end
		FETCH  cur_dim INTO @code,@name
		END
   CLOSE cur_dim;
   DEALLOCATE cur_dim
END


------------------------------------------------------------------------------------------------------
-- INSERTA DIMENSION 2
------------------------------------------------------------------------------------------------------
declare
    cur_dim cursor for
      select code, [name] from stg.Dimensiones_OOCR where Code between 20000 and 30000;
BEGIN
   OPEN cur_dim;
   FETCH  cur_dim INTO @code,@name
	WHILE (@@FETCH_STATUS = 0)
		BEGIN
			
			
			declare cur2 cursor for
				select value from string_split(@name,'-')
				begin
					open cur2
						FETCH  cur2 INTO @value						
						WHILE (@@FETCH_STATUS = 0) --If the fetch went well then we go for it
							BEGIN
							if @index = 0
							begin
								set @text1 = @value
								set @index = 1
							end
							else
							begin
								set @text2 = @value
								set @index = 0
							end
							FETCH  cur2 INTO @value							
							END
							insert into whs.DimDepartamentos(Codigo,Departamento,Area )values(@code ,@text1,@text2)
					close cur2
					DEALLOCATE cur2
				end
		FETCH  cur_dim INTO @code,@name
		END
   CLOSE cur_dim;
   DEALLOCATE cur_dim
END

------------------------------------------------------------------------------------------------------
-- INSERTA DIMENSION 3
------------------------------------------------------------------------------------------------------
insert into whs.DimLugarCliente
      select code, [name] from stg.Dimensiones_OOCR where Code between 30000 and 40000;


------------------------------------------------------------------------------------------------------
-- INSERTA DIMENSION 4
------------------------------------------------------------------------------------------------------

declare
    cur_dim cursor for
      select code, [name] from stg.Dimensiones_OOCR where Code between 40000 and 50000;
BEGIN
   OPEN cur_dim;
   FETCH  cur_dim INTO @code,@name
	WHILE (@@FETCH_STATUS = 0)
		BEGIN
			
			
			declare cur2 cursor for
				select value from string_split(@name,'-')
				begin
					open cur2
						FETCH  cur2 INTO @value						
						WHILE (@@FETCH_STATUS = 0) --If the fetch went well then we go for it
							BEGIN
							if @index = 0
							begin
								set @text1 = @value
								set @index = 1
							end
							else
							begin
								set @text2 = @value
								set @index = 0
							end
							FETCH  cur2 INTO @value							
							END
							insert into whs.DimPropioTercero (Codigo,Propio_Tercero,Proveedor)values(@code ,@text1,@text2)
					close cur2
					DEALLOCATE cur2
				end
		FETCH  cur_dim INTO @code,@name
		END
   CLOSE cur_dim;
   DEALLOCATE cur_dim
END

------------------------------------------------------------------------------------------------------
-- INSERTA DIMENSION 5
------------------------------------------------------------------------------------------------------

declare
    cur_dim cursor for
      select code, [name] from stg.Dimensiones_OOCR where Code between 50000 and 60000;
BEGIN
   OPEN cur_dim;
   FETCH  cur_dim INTO @code,@name
	WHILE (@@FETCH_STATUS = 0)
		BEGIN
			
			set @value = null
			set @text1 = null
			set @text2 = null
			set @index  = 0
			declare cur2 cursor for
				select value from string_split(@name,'-')
				begin
					open cur2
						FETCH  cur2 INTO @value						
						WHILE (@@FETCH_STATUS = 0) --If the fetch went well then we go for it
							BEGIN
							if @index = 0
							begin
								set @text1 = @value
								set @index = 1
							end
							else
							begin
								set @text2 = @value
								set @index = 0
							end
							FETCH  cur2 INTO @value							
							END
							insert into whs.DimTiposProductos(Codigo,Tipo_Producto,Proveedor)values(@code ,@text1,@text2)
					close cur2
					DEALLOCATE cur2
				end
		FETCH  cur_dim INTO @code,@name
		END
   CLOSE cur_dim;
   DEALLOCATE cur_dim
END

------------------------------------------------------------------------------------------------------
-- INSERTA PLAN DE CUENTAS
------------------------------------------------------------------------------------------------------
delete whs.DimPlanDeCuentas
declare @l1 varchar(20),@l2 varchar(20),@l3 varchar(20),@l4 varchar(20),@l5 varchar(20)
declare
    cur_pc cursor for
      select acctcode from stg.PlanCuentas 
BEGIN
   OPEN cur_pc;
   FETCH  cur_pc INTO @code
	WHILE (@@FETCH_STATUS = 0)
		BEGIN
			
			set @value = null
			set @text1 = null
			set @text2 = null
			set @index  = 0
			declare cur2 cursor for
				select value from string_split(@code,'.')
				begin
					open cur2
						FETCH  cur2 INTO @value						
						WHILE (@@FETCH_STATUS = 0) --If the fetch went well then we go for it
							BEGIN							
							if @index = 0
							begin
								 set @l1 = @value
								set @index = @index +1
							end
							else if @index =1 
							begin
								 set @l2 = @value
								set @index = @index +1
							end
								else if @index =2 
							begin
								 set @l3 = @value
								set @index = @index +1
							end
								else if @index =3 
							begin
								 set @l4 = @value
								set @index = @index +1
							end
								else if @index =4
							begin
								 set @l5 = @value
								set @index = @index +1
							end
							else
							begin
								
								set @index = 0
							end
							FETCH  cur2 INTO @value							
							END
							--insert into whs.DimTiposProductos(Codigo,Tipo_Producto,Proveedor)values(@code ,@text1,@text2)
							insert into whs.DimPlanDeCuentas
							select AcctCode,@l1,@l2,@l3,@l4,@l5,null,null,null,null,null,null,null,AcctName,AcctCurent ,Financial,null,CreateDate,UpdateDate from stg.PlanCuentas where AcctCode = @code
							
					close cur2
					DEALLOCATE cur2
				end
		FETCH  cur_pc INTO @code
		END
   CLOSE cur_pc;
   DEALLOCATE cur_pc
END

-- Update levels
update whs.DimPlanDeCuentas  set Level1 = REPLACE(acctcode,'0',''),  Level2 = '0' ,Level3 = '000' ,Level4 = '00' ,Level5 ='000' where AcctCurrent is null

update whs.DimPlanDeCuentas set LevelName1 = q1.l1, LevelName2 = q1.l2, LevelName3 = q1.l3, LevelName4 = q1.l4,LevelName5 = q1.l5
from(
select  p.AcctCode, l1.AcctName l1, l2.AcctName l2,l3.AcctName l3,l4.AcctName l4,l5.AcctName l5
from whs.DimPlanDeCuentas p
left join whs.DimPlanDeCuentas l1 on l1.Level1 = p.Level1 and l1.Level2 = '0' and l1.Level3 = '000' and l1.Level4 = '00' and l1.Level5 ='000' and l1.AcctCode <> p.AcctCode
left join whs.DimPlanDeCuentas l2 on l2.Level1 = p.Level1 and l2.Level2 = p.Level2 and l2.Level3 = '000' and l2.Level4 = '00' and l2.Level5 ='000'and l2.AcctCode <> p.AcctCode
left join whs.DimPlanDeCuentas l3 on l3.Level1 = p.Level1 and l3.Level2 = p.Level2 and l3.Level3 = p.Level3 and l3.Level4 = '00' and l3.Level5 ='000'and l3.AcctCode <> p.AcctCode
left join whs.DimPlanDeCuentas l4 on l4.Level1 = p.Level1 and l4.Level2 = p.Level2 and l4.Level3 = p.Level3 and l4.Level4 = p.Level4 and l4.Level5 ='000'and l4.AcctCode <> p.AcctCode
left join whs.DimPlanDeCuentas l5 on l5.Level1 = p.Level1 and l5.Level2 = p.Level2 and l5.Level3 = p.Level3 and l5.Level4 = p.Level4 and l5.Level5 = p.Level5 and l5.AcctCode <> p.AcctCode
) as q1
where whs.DimPlanDeCuentas.AcctCode = q1.AcctCode

update  whs.DimPlanDeCuentas
set LevelName2 = case when Level2 <>'0' then AcctName else LevelName2 end ,
	LevelName3 = case when Level3 <>'000' then AcctName else LevelName3 end,
	LevelName4 = case when Level4 <>'00' then AcctName else LevelName4 end ,
	LevelName5 = case when Level5 <>'000' then AcctName else LevelName5 end