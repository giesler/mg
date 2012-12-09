-- Creates new rfcdata database, then restores data for the database from a file

use master
go

drop database fcdata
go

create database fcdata
go

PRINT 'Restoring database...'
go

RESTORE DATABASE fcdata 
	FROM  DISK = N'D:\SQL Data Files\MSSQL\BACKUP\0911fcdata' 
	WITH  FILE = 1,  NOUNLOAD ,  STATS = 10,  RECOVERY ,  REPLACE ,  
	MOVE N'fcdata_log' TO N'D:\SQL Data Files\MSSQL\DATA\fcdata.ldf',  
	MOVE N'fcdata_data' TO N'D:\SQL Data Files\MSSQL\DATA\fcdata.mdf'
go


use fcdata
go

sp_change_users_login 'auto_fix', 'fcuser'
go

sp_password @new='fcuser', @loginame='fcuser'
go

sp_change_users_login 'auto_fix', 'bli_dbo'
go

sp_password @new='120960011014', @loginame='bli_dbo'
go

PRINT 'Updating stats...'
go

sp_updatestats
go

sp_dbcmptlevel 'fcdata', 80
go

SET NOCOUNT ON

IF NOT EXISTS(SELECT * FROM tblSecurity WHERE UserID = 'mike' and SwID = 0 and AccessType = 0) 
	INSERT tblSecurity(UserID, SwID, AccessType) values ('mike', 0, 0)
go

IF NOT EXISTS(SELECT * FROM tblSecurity WHERE UserID = 'mpgiesle' and SwID = 0 and AccessType = 0) 
	INSERT tblSecurity(UserID, SwID, AccessType) values ('mpgiesle', 0, 0)
go

IF NOT EXISTS(SELECT * FROM tblSecurity WHERE UserID = 'giesler' and SwID = 0 and AccessType = 0) 
	INSERT tblSecurity(UserID, SwID, AccessType) values ('giesler', 0, 0)
go

IF NOT EXISTS(SELECT * FROM tblSecurity WHERE UserID = 'Mike Giesler' and SwID = 0 and AccessType = 0) 
	INSERT tblSecurity(UserID, SwID, AccessType) values ('Mike Giesler', 0, 0)
go

SELECT 'Version = ' + PropertyValue FROM tblDBProperties WHERE PropertyName = 'DBStructVersion'
GO

PRINT 'Done restoring database.'
GO