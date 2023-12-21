CREATE TABLE [whs].[DimBusinessPartnerGroup]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	GroupCode varchar(100),
	GroupName varchar(300),
	GroupType varchar(300)
)
