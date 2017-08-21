-- Creates new rfcdata database, then restores data for the database from a file
use master
go

drop database xmcatalog
go

create database xmcatalog
go

PRINT 'Restoring database...'
go

RESTORE DATABASE xmcatalog 
	FROM  DISK = N'D:\SQL Data Files\MSSQL\BACKUP\xmcatalog 20010715.mbf' 
	WITH  FILE = 1,  NOUNLOAD ,  STATS = 10,  RECOVERY ,  REPLACE ,  
	MOVE N'xmcatalog_log' TO N'D:\SQL Data Files\MSSQL\DATA\xmcatalog_Log.ldf',  
	MOVE N'xmcatalog_dat' TO N'D:\SQL Data Files\MSSQL\DATA\xmcatalog_Data.mdf'
go


use xmcatalog
go

sp_updatestats
go

PRINT 'Done restoring database.'
GO