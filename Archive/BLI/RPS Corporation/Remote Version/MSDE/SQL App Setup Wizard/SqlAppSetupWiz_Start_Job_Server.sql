USE master
GO

IF EXISTS(SELECT * FROM sysobjects WHERE name="SqlAppSetupWiz_Start_Job_Server" AND type='P') 
  DROP PROC SqlAppSetupWiz_Start_Job_Server
GO

CREATE PROC SqlAppSetupWiz_Start_Job_Server (
  @sapwd VARCHAR(255) = NULL) AS 
BEGIN
  DECLARE @oDbServer INT
  DECLARE @oJobServer INT
  DECLARE @hr INT
  DECLARE @source VARCHAR(255)
  DECLARE @description VARCHAR(255)
  DECLARE @msg VARCHAR(440)

  EXEC @hr = sp_OACreate 'SQLDMO.SQLServer', @oDbServer OUT

  IF @hr <> 0
  BEGIN
    EXEC sp_OAGetErrorInfo @oDbServer, @source OUT, @description OUT
    SELECT @msg = @source + ", " + @description
    RAISERROR(@msg, 11, 2) WITH LOG
    RETURN 1
  END

  IF @sapwd IS NULL
	EXEC @hr = sp_OAMethod @oDbServer, 'Connect', NULL, '(local)', 'sa'
  ELSE
	EXEC @hr = sp_OAMethod @oDbServer, 'Connect', NULL, '(local)', 'sa', @sapwd

  IF @hr <> 0
  BEGIN
    EXEC sp_OAGetErrorInfo @oDbServer, @source OUT, @description OUT
    SELECT @msg = @source + ", " + @description
    RAISERROR(@msg, 11, 2) WITH LOG
    RETURN 1
  END

  EXEC @hr = sp_OAMethod @oDbServer, 'JobServer.Start', NULL

  IF @hr <> 0
  BEGIN
    EXEC sp_OAGetErrorInfo @oDbServer, @source OUT, @description OUT
    SELECT @msg = @source + ", " + @description
    RAISERROR(@msg, 11, 2) WITH LOG
    RETURN 1
  END

  RETURN 0  
END
GO



  
