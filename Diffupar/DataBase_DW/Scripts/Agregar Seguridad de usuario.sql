DECLARE @UI NVARCHAR(128), @USERNAME VARCHAR(100) = 'mariano.aluzzo@alusoft.com.ar'

SET @UI = (SELECT Id FROM AspNetUsers WHERE [USERNAME] = @USERNAME)


-- insert menu finanzas --
declare @idMf int
insert into MenuSecurity ([User_Id],Nombre,IsActivo,Orden) values(@UI,'Configuración',0,1)
set @idMf = @@IDENTITY
insert into ItemMenuSecurity (Menu_Id,Nombre, IsActivo, Orden) values (@idMf,'Source',0,1)
insert into ItemMenuSecurity (Menu_Id,Nombre, IsActivo, Orden) values (@idMf,'Tables', 0,2)
insert into ItemMenuSecurity (Menu_Id,Nombre, IsActivo, Orden) values (@idMf,'Querys',0,3)
insert into ItemMenuSecurity (Menu_Id,Nombre, IsActivo, Orden) values (@idMf,'Schedule',0,3)

-- insert menu CRM --
declare @idCRM int
insert into MenuSecurity ([User_Id],Nombre,IsActivo,Orden) values(@UI,'Templates',0,2)
set @idCRM = @@IDENTITY
insert into ItemMenuSecurity (Menu_Id,Nombre,IsActivo, Orden) values (@idCRM,'Template Pedidos Arcor',0,2)

-- insert menu Configuracion --
declare @idConf int
insert into MenuSecurity ([User_Id],Nombre,IsActivo,Orden) values(@UI,'Upload File',0,3)
set @idConf = @@IDENTITY
insert into ItemMenuSecurity (Menu_Id,Nombre,IsActivo, Orden) values (@idConf,'Pedidos Arcor',0,1)

-- insert menu Devops --
declare @idDev int
insert into MenuSecurity ([User_Id],Nombre,IsActivo,Orden) values(@UI,'Ver Procesos',0,4)
set @idConf = @@IDENTITY
insert into ItemMenuSecurity (Menu_Id,Nombre,IsActivo, Orden) values (@idConf,'Procesos',0,1)



--select * from MenuSecurity
--update MenuSecurity set IsActivo = 1
--select * from ItemMenuSecurity
--update ItemMenuSecurity set IsActivo = 1

