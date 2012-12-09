-- fcdata update 3.41
-- - update rel notes, db version

if exists (select * from sysobjects where id = object_id(N'[dbo].[wasprptWarrantyPending]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprptWarrantyPending]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE wasprptWarrantyPending
	@iWarrantyType int = 0
AS
SELECT wa.Dealer, wa.MachineSerialNumber, wa.RGANum, pa.RPSPartNum, wa.WarrantyOpen, wa.Hours
FROM dbo.tblWarranty wa, dbo.tblWarrantyParts wp, dbo.tblParts pa
WHERE wa.WarrantyID = wp.WarrantyID and wp.PartID = pa.PartID
	and wa.WarrantyType = @iWarrantyType

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptWarrantyPending]  TO [fcuser]
GO



--insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
--values (17, 20, 'Parts with no Models Report (Temporary)', 4, 'parptTempPartsReport')
--go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.41','2000-4-7 00:00:00','- Fixed Warranty Pending reports
- Added report for parts with no Models
')
go

update tblDBProperties
set PropertyValue = '3.41'
where PropertyName = 'DBStructVersion'
go
