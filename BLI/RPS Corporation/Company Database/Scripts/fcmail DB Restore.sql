-- Creates new rfcdata database, then restores data for the database from a file

use master
go

drop database fcmail
go

create database fcmail
go

PRINT 'Restoring database...'
go

RESTORE DATABASE fcmail 
	FROM  DISK = N'E:\SQL Data\MSSQL\BACKUP\fcmail 12-30-99.bak' 
	WITH  FILE = 1,  NOUNLOAD ,  STATS = 10,  RECOVERY ,  REPLACE ,  
	MOVE N'fcmail_log' TO N'E:\SQL Data\MSSQL\DATA\fcmail.ldf',  
	MOVE N'fcmail_data' TO N'E:\SQL Data\MSSQL\DATA\fcmail.mdf'
go


use fcmail
go

sp_change_users_login 'auto_fix', 'fcuser'
go

sp_password @new='fcuser', @loginame='fcuser'
go

sp_change_users_login 'auto_fix', 'bli_dbo'
go

PRINT 'Updating stats...'
go

sp_updatestats
go

sp_dbcmptlevel 'fcdata', 80
go

PRINT 'Done restoring database.'
GO