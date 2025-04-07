use DiffuparAnalytics
go 

DECLARE @UI NVARCHAR(128), @USERNAME VARCHAR(100) = 'mariano.aluzzo@alusoft.com.ar'

SET @UI = (SELECT Id FROM AspNetUsers WHERE [USERNAME] = @USERNAME)

declare @imnuid int = (select top 1 Id from MenuSecurity where User_Id = @ui)
delete ItemMenuSecurity where exists (select Id from MenuSecurity where User_Id = @ui and Menu_Id  = MenuSecurity.Id  ) 
delete MenuSecurity where User_Id = @UI

-- insert menu finanzas --
declare @idMf int
insert into MenuSecurity ([User_Id],Nombre,IsActivo,Orden) values(@UI,'Configuración',1,1)
set @idMf = @@IDENTITY
insert into ItemMenuSecurity (Menu_Id,Nombre, IsActivo, Orden) values (@idMf,'Source',1,1)
insert into ItemMenuSecurity (Menu_Id,Nombre, IsActivo, Orden) values (@idMf,'Tables', 1,2)
insert into ItemMenuSecurity (Menu_Id,Nombre, IsActivo, Orden) values (@idMf,'Querys',1,3)
insert into ItemMenuSecurity (Menu_Id,Nombre, IsActivo, Orden) values (@idMf,'Schedule',1,4)
insert into ItemMenuSecurity (Menu_Id,Nombre, IsActivo, Orden) values (@idMf,'Seguridad',1,5)

-- insert menu rabbit --
declare @idNp int
insert into MenuSecurity ([User_Id],Nombre,IsActivo,Orden) values(@UI,'Nasep',1,2)
set @idNp = @@IDENTITY
insert into ItemMenuSecurity (Menu_Id,Nombre, IsActivo, Orden) values (@idNp,'NExtracciones',1,1)


-- insert menu CRM --
declare @idCRM int
insert into MenuSecurity ([User_Id],Nombre,IsActivo,Orden) values(@UI,'Templates',1,3)
set @idCRM = @@IDENTITY
insert into ItemMenuSecurity (Menu_Id,Nombre,IsActivo, Orden) values (@idCRM,'TLibroMayor',1,1)
insert into ItemMenuSecurity (Menu_Id,Nombre,IsActivo, Orden) values (@idCRM,'TObjetivosTotal',1,2)

-- insert menu Configuracion --
declare @idConf int
insert into MenuSecurity ([User_Id],Nombre,IsActivo,Orden) values(@UI,'Subir Complementos',1,4)
set @idConf = @@IDENTITY
insert into ItemMenuSecurity (Menu_Id,Nombre,IsActivo, Orden) values (@idConf,'UpLibroMayor',1,1)
insert into ItemMenuSecurity (Menu_Id,Nombre,IsActivo, Orden) values (@idConf,'UpObjetivosTotal',1,2)


-- insert menu Process --
declare @idDev int
insert into MenuSecurity ([User_Id],Nombre,IsActivo,Orden) values(@UI,'Procesos',1,5)
set @idConf = @@IDENTITY
insert into ItemMenuSecurity (Menu_Id,Nombre,IsActivo, Orden) values (@idConf,'Ver Procesos',1,1)

-- insert menu Reports --
declare @idRep int
insert into MenuSecurity ([User_Id],Nombre,IsActivo,Orden) values(@UI,'Reportes',1,5)
set @idConf = @@IDENTITY
insert into ItemMenuSecurity (Menu_Id,Nombre,IsActivo, Orden) values (@idConf,'Venta Retail',1,1)



-----------------------------------------------------
-- Inserta el menu para todos los usuario
-----------------------------------------------------
declare 
	@MenuName varchar(100)= 'Reportes',
	@Orden int = 6,
	@ItemMenuName varchar(100) = 'Venta Retail'
	
INSERT INTO MenuSecurity
select 
	id,
	@MenuName,
	0,
	@Orden,
	NULL
from AspNetUsers U
WHERE NOT EXISTS(SELECT ID FROM MenuSecurity M WHERE M.User_Id = U.ID AND M.Nombre = @MenuName)

insert into ItemMenuSecurity 
SELECT
m.id,
@ItemMenuName,
0,
1
FROM MenuSecurity M 
WHERE M.Nombre = @MenuName
and not exists ( select id from ItemMenuSecurity i where i.Menu_Id = M.id and i.Nombre = @ItemMenuName)

-----------------------------------------------------
-- Valida Items Duplicados
-----------------------------------------------------
declare @UserID varchar(100) = 'f1c63feb-be3b-490b-94bf-8720ebe62718'

select 
	i.*
from ItemMenuSecurity i
inner join MenuSecurity m on m.User_Id = @UserID and m.id = i.Menu_Id

--delete ItemMenuSecurity where id in(209,210)
