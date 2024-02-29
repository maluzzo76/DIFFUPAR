CREATE TABLE [dbo].[ItemMenuSecurity]
(
	[Id] INT identity(1,1) NOT NULL PRIMARY KEY,
	Menu_Id int,
	Nombre varchar(100),
	IsActivo bit default(0),
	Orden int,
	foreign key (Menu_Id) references MenuSecurity(Id)
)
