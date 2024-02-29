------------------------------------------------------------------------------------------------------
-- Limpia el log de la base de datos
------------------------------------------------------------------------------------------------------
use master
go
ALTER DATABASE [DiffuparAnalytics]
SET RECOVERY SIMPLE;


use DiffuparAnalytics
go
DBCC SHRINKFILE(DiffuparAnalytics_log, 1);


use master
go
ALTER DATABASE DiffuparAnalytics
SET RECOVERY FULL;
