CREATE TABLE [dbo].[DbSchedule]
(
	[Id] INT identity(1,1) NOT NULL PRIMARY KEY,
	[Name] varchar(100),
	[QueryId] int,
	[StarDate] datetime,
	[LastStarDate] datetime,
	[Status] int,
	foreign key (QueryId) references DbQuery(Id)
)
