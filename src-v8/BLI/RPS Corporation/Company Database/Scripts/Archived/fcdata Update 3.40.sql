-- fcdata update 3.40
-- - update rel notes, db version


if exists (select * from sysobjects where id = object_id(N'[dbo].[spGetNTUserList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spGetNTUserList]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE spGetNTUserList

AS

declare @object int
declare @hr int
declare @property varchar(2500)
declare @return varchar(255)


-- CREATE AN OBJECT
EXEC @hr = sp_OACreate 'axNTSec.ntsec', @object OUT
IF @hr <> 0
  BEGIN
	raiserror ('Error creating server component to retreive user list!', 16, 1)
	RETURN
  END

-- GET A PROPERTY BY CALLING METHOD
EXEC @hr = sp_OAMethod @object, 'GetUserStr', @property OUT
IF @hr <> 0
  BEGIN
	raiserror ('Error calling user retreival function', 16, 1)
	RETURN
  END

SELECT @property AS uList


-- DESTROY OBJECT
EXEC @hr = sp_OADestroy @object
IF @hr <> 0
  BEGIN
	raiserror ('Error destroying user object', 1, 1)
	RETURN
  END



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[spGetNTUserList]  TO [fcuser]
GO

insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (17, 20, 'Parts with no Models Report (Temporary)', 4, 'parptTempPartsReport')
go


insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.40','2000-4-3 00:00:00','- Fixed Database Security tool to allow other ''variants'' of Irene''s passwords
- Added report for parts with no Models
')
go

update tblDBProperties
set PropertyValue = '3.40'
where PropertyName = 'DBStructVersion'
go
