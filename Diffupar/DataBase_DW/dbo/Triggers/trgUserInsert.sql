CREATE TRIGGER trgUserInsert
ON AspNetUsers
FOR INSERT
AS
	
DECLARE @UI NVARCHAR(128)

SET @UI = (select 	Id	from inserted i)

declare @imnuid int = (select top 1 Id from MenuSecurity where User_Id = @ui)


-- insert menu finanzas --
declare @idMf int
insert into MenuSecurity ([User_Id],Nombre,IsActivo,Orden) values(@UI,'Configuración',0,1)
set @idMf = @@IDENTITY
insert into ItemMenuSecurity (Menu_Id,Nombre, IsActivo, Orden) values (@idMf,'Source',0,1)
insert into ItemMenuSecurity (Menu_Id,Nombre, IsActivo, Orden) values (@idMf,'Tables', 0,2)
insert into ItemMenuSecurity (Menu_Id,Nombre, IsActivo, Orden) values (@idMf,'Querys',0,3)
insert into ItemMenuSecurity (Menu_Id,Nombre, IsActivo, Orden) values (@idMf,'Schedule',0,4)
insert into ItemMenuSecurity (Menu_Id,Nombre, IsActivo, Orden) values (@idMf,'Seguridad',0,5)

-- insert menu rabbit --
declare @idNp int
insert into MenuSecurity ([User_Id],Nombre,IsActivo,Orden) values(@UI,'Nasep',0,2)
set @idNp = @@IDENTITY
insert into ItemMenuSecurity (Menu_Id,Nombre, IsActivo, Orden) values (@idNp,'NExtracciones',0,1)


-- insert menu CRM --
declare @idCRM int
insert into MenuSecurity ([User_Id],Nombre,IsActivo,Orden) values(@UI,'Templates',1,3)
set @idCRM = @@IDENTITY
insert into ItemMenuSecurity (Menu_Id,Nombre,IsActivo, Orden) values (@idCRM,'TLibroMayor',0,1)
insert into ItemMenuSecurity (Menu_Id,Nombre,IsActivo, Orden) values (@idCRM,'TObjetivosTotal',0,2)

-- insert menu Configuracion --
declare @idConf int
insert into MenuSecurity ([User_Id],Nombre,IsActivo,Orden) values(@UI,'Subir Complementos',0,4)
set @idConf = @@IDENTITY
insert into ItemMenuSecurity (Menu_Id,Nombre,IsActivo, Orden) values (@idConf,'UpLibroMayor',0,1)
insert into ItemMenuSecurity (Menu_Id,Nombre,IsActivo, Orden) values (@idConf,'UpObjetivosTotal',0,2)


-- insert menu Process --
declare @idDev int
insert into MenuSecurity ([User_Id],Nombre,IsActivo,Orden) values(@UI,'Procesos',0,5)
set @idConf = @@IDENTITY
insert into ItemMenuSecurity (Menu_Id,Nombre,IsActivo, Orden) values (@idConf,'Ver Procesos',0,1)

-- insert menu Reports --
declare @idRep int
insert into MenuSecurity ([User_Id],Nombre,IsActivo,Orden) values(@UI,'Reportes',0,5)
set @idConf = @@IDENTITY
insert into ItemMenuSecurity (Menu_Id,Nombre,IsActivo, Orden) values (@idConf,'Venta Retail',0,1)




GO