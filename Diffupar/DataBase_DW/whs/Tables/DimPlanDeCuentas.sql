CREATE TABLE [whs].[DimPlanDeCuentas]
(
	[Id] INT NOT NULL PRIMARY KEY identity(1,1),
	AcctCode varchar(200),
	Level1 varchar(20),
	Level2 varchar(20),
	Level3 varchar(20),
	Level4 varchar(20),
	Level5 varchar(20),	
	Level6 varchar(20),	
	LevelName1 varchar(500),
	LevelName2 varchar(500),
	LevelName3 varchar(500),
	LevelName4 varchar(500),
	LevelName5 varchar(500),
	LevelName6 varchar(500),
	AcctName varchar(200),
	AcctCurrent varchar(50),
	Financial varchar(10),
	LevalAccount int,
	CreateDate datetime,
	UpdateDate datetime
	
)
