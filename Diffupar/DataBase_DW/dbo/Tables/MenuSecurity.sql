CREATE TABLE [dbo].[MenuSecurity]
(
	[Id] INT identity(1,1) NOT NULL PRIMARY KEY,
	[User_Id] NVARCHAR (128),
	Nombre varchar(100),
	IsActivo bit default(0),
	Orden int,
	foreign key ([User_Id]) references AspNetUsers(Id)
)
