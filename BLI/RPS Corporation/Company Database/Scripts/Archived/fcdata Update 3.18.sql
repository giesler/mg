-- fcdata Update 3.18
-- - update parts by model/part range with qty report
-- - update rel notes, db version


if exists (select * from sysobjects where id = object_id(N'[dbo].[pasprptPartLabels]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[pasprptPartLabels]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE pasprptPartLabels 
	@sStartPart varchar(50) = null,
	@sEndPart varchar(50) = null,
	@sModel varchar(50) = null,
	@iQty int = 1
AS

IF @sStartPart IS NULL
	SELECT @sStartPart = '000000000000000000'
IF @sEndPart IS NULL
	SELECT @sEndPart = 'zzzzzzzzzzzzzzzzzzzzzz'
IF @sModel IS NULL
	SELECT @sModel = '%'
IF @iQty IS NULL
	SELECT @iQty = 1

SELECT pa.RPSPartNum, pa.PartName, pa.VendorName, pa.VendorPartName, pa.ManfPartNum, nu.Counter, pa.Model 
FROM dbo.tblParts pa, dbo.tbl_Numbers nu
WHERE nu.Counter <= @iQty		-- allows multiple labels
	and pa.RPSPartNum Between @sStartPart And @sEndPart
	and pa.Model Like @sModel
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[pasprptPartLabels]  TO [fcuser]
GO



insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.18','1999-12-05 00:00:00','- Updated ''RPS Parts Labels - by Model or range'' to work much faster.
- Fixed ''RPS Parts Labels - by parts entered'' report.
- Fixed ''#Name?'' error from showing up.  (should be fixed)
')
go

update tblDBProperties
set PropertyValue = '3.18'
where PropertyName = 'DBStructVersion'
go

dbcc checkdb
go
