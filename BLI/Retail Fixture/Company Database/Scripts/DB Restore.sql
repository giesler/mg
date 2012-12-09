-- Creates new rfcdata database, then restores data for the database from a file
use master
go

drop database rfcdata
go

create database rfcdata
go

PRINT 'Restoring database...'
go

RESTORE DATABASE rfcdata 
	FROM  DISK = N'E:\SQL Data\MSSQL\BACKUP\010801rfcdata' 
	WITH  FILE = 1,  NOUNLOAD ,  STATS = 10,  RECOVERY ,  REPLACE ,  
	MOVE N'rfcdata_log' TO N'E:\SQL Data\MSSQL\DATA\rfcdata.ldf',  
	MOVE N'rfcdata_dat' TO N'E:\SQL Data\MSSQL\DATA\rfcdata.mdf'
go


use rfcdata
go

sp_change_users_login 'auto_fix', 'rfcuser'
go

sp_password @new='rfcuser', @loginame='rfcuser'
go

sp_change_users_login 'auto_fix', 'bli_dbo'
go

sp_updatestats
go

PRINT 'Done restoring database.'
GO