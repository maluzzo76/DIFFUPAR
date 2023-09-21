CREATE TABLE [dbo].[DbSource]
(
	[Id] INT identity(1000,1) PRIMARY KEY,
	[Name] varchar(100) not null,
	[Connetion] varchar(max) not null,
	[ProviderId] int not null,	
	foreign key (ProviderId) references DbProviders (Id)
)
