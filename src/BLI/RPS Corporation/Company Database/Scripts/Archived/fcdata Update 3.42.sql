-- fcdata update 3.42
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
	and wa.WarrantyType = @iWarrantyType and wa.WarrantyOpen = 1



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptWarrantyPending]  TO [fcuser]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ppsprptProdReleases]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ppsprptProdReleases]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE ppsprptProdReleases
	@sToDate varchar(20)
AS

IF @sToDate is null
	select @sToDate = '1/1/2020'

SELECT po.POID, po.Vendor, pp.VendorPartNumber, pp.RPSPartNum, pp.PartDescription, 
	pd.RequestedShipDate, pd.Quantity, pd.Value, pd.ReceivedDate, 
	pd.CostEach, pd.Quantity AS ExtCost, pd.Notes,
	DATEPART(yyyy, pd.RequestedShipDate) AS ShipYear, 
	DATEPART(wk, pd.RequestedShipDate) AS ShipWeek
FROM dbo.tblPO po, dbo.tblPOPart pp, dbo.tblPOPartDetail pd
WHERE po.POID = pp.fkPOID and pp.POPartID = pd.fkPOPartID and
	pd.ReceivedDate IS NULL and pd.RequestedShipDate < @sToDate


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ppsprptProdReleases]  TO [fcuser]
GO


update dbo.[Switchboard Items]
set ItemText = 'Vendor Release Schedule by Vendor'
where SwitchboardID = 19 and ItemNumber = 7
go

update dbo.[Switchboard Items]
set ItemNumber = ItemNumber + 1
where SwitchboardID = 19 and ItemNumber > 6
go

insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (19, 7, 'Vendor Release Schedule by Part Number', 4, 'pprptVendorReleaseByPN')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.42','2000-4-7 00:00:00','- Fixed Warranty Pending reports showing closed reports
- Fixed Dealer Parts pricing report showing part numbers out of order
- Removed extra column fkPOPartID in Prod Parts
- Printout of Sub Parts shows total cost for part card printout
- Fixed Tom Cat Dealer Sales report showing FC dealers in drop down
- Added Vendor Release Schedule by Part Number
- Added Notes field to Production Releases
')
go

update tblDBProperties
set PropertyValue = '3.42'
where PropertyName = 'DBStructVersion'
go
