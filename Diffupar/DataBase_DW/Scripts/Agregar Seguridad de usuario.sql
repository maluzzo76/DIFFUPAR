DECLARE @UI NVARCHAR(128), @USERNAME VARCHAR(100) = 'mariano.aluzzo@alusoft.com.ar'

SET @UI = (SELECT Id FROM AspNetUsers WHERE [USERNAME] = @USERNAME)

delete ItemMenuSecurity
delete MenuSecurity

-- insert menu finanzas --
declare @idMf int
insert into MenuSecurity ([User_Id],Nombre,IsActivo,Orden) values(@UI,'Configuración',1,1)
set @idMf = @@IDENTITY
insert into ItemMenuSecurity (Menu_Id,Nombre, IsActivo, Orden) values (@idMf,'Source',1,1)
insert into ItemMenuSecurity (Menu_Id,Nombre, IsActivo, Orden) values (@idMf,'Tables', 1,2)
insert into ItemMenuSecurity (Menu_Id,Nombre, IsActivo, Orden) values (@idMf,'Querys',1,3)
insert into ItemMenuSecurity (Menu_Id,Nombre, IsActivo, Orden) values (@idMf,'Schedule',1,3)

-- insert menu CRM --
declare @idCRM int
insert into MenuSecurity ([User_Id],Nombre,IsActivo,Orden) values(@UI,'Templates',1,2)
set @idCRM = @@IDENTITY
insert into ItemMenuSecurity (Menu_Id,Nombre,IsActivo, Orden) values (@idCRM,'TLibroMayor',1,1)
insert into ItemMenuSecurity (Menu_Id,Nombre,IsActivo, Orden) values (@idCRM,'TObjetivosTotal',1,2)

-- insert menu Configuracion --
declare @idConf int
insert into MenuSecurity ([User_Id],Nombre,IsActivo,Orden) values(@UI,'Subir Complementos',1,3)
set @idConf = @@IDENTITY
insert into ItemMenuSecurity (Menu_Id,Nombre,IsActivo, Orden) values (@idConf,'UpLibroMayor',1,1)
insert into ItemMenuSecurity (Menu_Id,Nombre,IsActivo, Orden) values (@idConf,'UpObjetivosTotal',1,2)


-- insert menu Devops --
declare @idDev int
insert into MenuSecurity ([User_Id],Nombre,IsActivo,Orden) values(@UI,'Procesos',1,4)
set @idConf = @@IDENTITY
insert into ItemMenuSecurity (Menu_Id,Nombre,IsActivo, Orden) values (@idConf,'Ver Procesos',1,1)
