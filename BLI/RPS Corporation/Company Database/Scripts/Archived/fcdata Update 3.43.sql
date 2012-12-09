-- fcdata update 3.43
-- - update rel notes, db version

if exists (select * from sysobjects where id = object_id(N'[dbo].[ppsprptCashFlow]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ppsprptCashFlow]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ppsprptVendorRelease]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ppsprptVendorRelease]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE ppsprptCashFlow
	@sVendorName varchar(40) = null,
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@sOpenClosed varchar(10) = null
 AS

IF @sFromDate is null
	select @sFromDate = '1/1/1900'
if @sToDate is null
	select @sToDate = '1/1/2100'
if @sVendorName is null
	select @sVendorName = '%'

if @sOpenClosed is null
	select pd.RequestedShipDate, pp.VendorPartNumber, po.Vendor, pp.RPSPartNum, pd.CostEach, pd.Quantity, pd.Value, 
		pd.ReceivedDate, po.POID
	from dbo.tblPO po, dbo.tblPOPart pp, dbo.tblPOPartDetail pd 
	where po.POID = pp.fkPOID and pp.POPartID = pd.fkPOPartID
		and pd.RequestedShipDate between @sFromDate and @sToDate
		and po.Vendor like @sVendorName
else if @sOpenClosed = 'open'
	select pd.RequestedShipDate, pp.VendorPartNumber, po.Vendor, pp.RPSPartNum, pd.CostEach, pd.Quantity, pd.Value, 
		pd.ReceivedDate, po.POID
	from dbo.tblPO po, dbo.tblPOPart pp, dbo.tblPOPartDetail pd 
	where po.POID = pp.fkPOID and pp.POPartID = pd.fkPOPartID
		and pd.RequestedShipDate between @sFromDate and @sToDate
		and po.Vendor like @sVendorName
		and pd.ReceivedDate is null
else
	select pd.RequestedShipDate, pp.VendorPartNumber, po.Vendor, pp.RPSPartNum, pd.CostEach, pd.Quantity, pd.Value, 
		pd.ReceivedDate, po.POID
	from dbo.tblPO po, dbo.tblPOPart pp, dbo.tblPOPartDetail pd 
	where po.POID = pp.fkPOID and pp.POPartID = pd.fkPOPartID
		and pd.RequestedShipDate between @sFromDate and @sToDate
		and po.Vendor like @sVendorName
		and pd.ReceivedDate is not null



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ppsprptCashFlow]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE ppsprptVendorRelease
	@sPOID varchar(20) = null,
	@sRPSPartNum varchar(40) = null,
	@sVendorPartNum varchar(40) = null,
	@iOpenFlag varchar(2) = null
AS
if @sPOID is null
	select @sPOID = '%'
if @sRPSPartNum is null
	select @sRPSPartNum = '%'
if @sVendorPartNum is null
	select @sVendorPartNum = '%'
if @iOpenFlag = 1
	SELECT po.POID, po.Vendor, pp.VendorPartNumber, pp.RPSPartNum, pp.PartDescription, pd.RequestedShipDate,
		pd.Quantity, pd.Value, pd.ReceivedDate, pa.RPSPNSort
	FROM dbo.tblPO po, dbo.tblPOPart pp, dbo.tblPOPartDetail pd, dbo.tblParts pa
	WHERE po.POID = pp.fkPOID and pp.POPartID = pd.fkPOPartID and pd.RequestedShipDate is not null
		and pa.RPSPartNum = pp.RPSPartNum
		and po.POID like @sPOID
		and pp.RPSPartNum like @sRPSPartNum
		and pp.VendorPartNumber like @sVendorPartNum
		and pd.ReceivedDate is null
	ORDER BY pp.RPSPartNum, po.POID, pp.VendorPartNumber, pd.RequestedShipDate
else
	SELECT po.POID, po.Vendor, pp.VendorPartNumber, pp.RPSPartNum, pp.PartDescription, pd.RequestedShipDate,
		pd.Quantity, pd.Value, pd.ReceivedDate
	FROM dbo.tblPO po, dbo.tblPOPart pp, dbo.tblPOPartDetail pd
	WHERE po.POID = pp.fkPOID and pp.POPartID = pd.fkPOPartID and pd.RequestedShipDate is not null
		and po.POID like @sPOID
		and pp.RPSPartNum like @sRPSPartNum
		and pp.VendorPartNumber like @sVendorPartNum
	ORDER BY pp.RPSPartNum, po.POID, pp.VendorPartNumber, pd.RequestedShipDate

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ppsprptVendorRelease]  TO [fcuser]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ppsprptVendorRelease]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ppsprptVendorRelease]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE ppsprptVendorRelease
	@sPOID varchar(20) = null,
	@sRPSPartNum varchar(40) = null,
	@sVendorPartNum varchar(40) = null,
	@iOpenFlag varchar(2) = null
AS
if @sPOID is null
	select @sPOID = '%'
if @sRPSPartNum is null
	select @sRPSPartNum = '%'
if @sVendorPartNum is null
	select @sVendorPartNum = '%'
if @iOpenFlag = 1
	SELECT po.POID, po.Vendor, pp.VendorPartNumber, pp.RPSPartNum, pp.PartDescription, pd.RequestedShipDate,
		pd.Quantity, pd.Value, pd.ReceivedDate, pa.RPSPNSort, pp.POPartID, pd.POPartDetailID
	FROM dbo.tblPO po, dbo.tblPOPart pp, dbo.tblPOPartDetail pd, dbo.tblParts pa
	WHERE po.POID = pp.fkPOID and pp.POPartID = pd.fkPOPartID and pd.RequestedShipDate is not null
		and pa.RPSPartNum = pp.RPSPartNum
		and po.POID like @sPOID
		and pp.RPSPartNum like @sRPSPartNum
		and pp.VendorPartNumber like @sVendorPartNum
		and pd.ReceivedDate is null
	ORDER BY pp.RPSPartNum, po.POID, pp.VendorPartNumber, pd.RequestedShipDate
else
	SELECT po.POID, po.Vendor, pp.VendorPartNumber, pp.RPSPartNum, pp.PartDescription, pd.RequestedShipDate,
		pd.Quantity, pd.Value, pd.ReceivedDate, pa.RPSPNSort, pp.POPartID, pd.POPartDetailID
	FROM dbo.tblPO po, dbo.tblPOPart pp, dbo.tblPOPartDetail pd, dbo.tblParts pa
	WHERE po.POID = pp.fkPOID and pp.POPartID = pd.fkPOPartID and pd.RequestedShipDate is not null
		and pa.RPSPartNum = pp.RPSPartNum
		and po.POID like @sPOID
		and pp.RPSPartNum like @sRPSPartNum
		and pp.VendorPartNumber like @sVendorPartNum
	ORDER BY pp.RPSPartNum, po.POID, pp.VendorPartNumber, pd.RequestedShipDate



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ppsprptVendorRelease]  TO [fcuser]
GO

insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (41, 0, 'Tom Cat Menu', 0, '')
go

update dbo.[Switchboard Items]
set SwitchboardID = 41, ItemNumber = ItemNumber - 13
where SwitchboardID = 1 and ItemNumber > 13
go

update dbo.[Switchboard Items]
set ItemNumber = ItemNumber + 1
where SwitchboardID = 1 and ItemNumber > 0
go

insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (1, 1, 'BSCAI List', 1, '42')
go

insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (1, 18, 'Tom Cat Menu', 1, '41')
go

insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (42, 0, 'BSCAI List', 0, '')
go

insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (42, 1, 'Add/edit/view entries', 3, 'bsfrmList')
go

insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (42, 2, 'BSCAI List Report', 4, 'bsrptList')
go

insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (42, 3, 'BSCAI Mailling Labels', 4, 'bsrptMailLabels')
go


CREATE TABLE [dbo].[tblBSCAI] (
	[ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Code] [nvarchar] (50) NULL ,
	[Title] [nvarchar] (50) NULL ,
	[FirstName] [nvarchar] (50) NULL ,
	[LastName] [nvarchar] (50) NULL ,
	[Suffix] [nvarchar] (20) NULL ,
	[Company] [nvarchar] (75) NULL ,
	[Division] [nvarchar] (75) NULL ,
	[Address] [nvarchar] (75) NULL ,
	[Address2] [nvarchar] (75) NULL ,
	[City] [nvarchar] (75) NULL ,
	[State] [nvarchar] (20) NULL ,
	[Zip] [nvarchar] (15) NULL ,
	[StateFullName] [nvarchar] (50) NULL ,
	[Country] [nvarchar] (50) NULL ,
	[Phone] [nvarchar] (30) NULL ,
	[Fax] [nvarchar] (30) NULL ,
	[TollFree] [nvarchar] (30) NULL ,
	[Email] [nvarchar] (50) NULL 
) ON [PRIMARY]
GO

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblBSCAI ADD CONSTRAINT
	PK_tblBSCAI PRIMARY KEY NONCLUSTERED 
	(
	ID
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblBSCAI_State ON dbo.tblBSCAI
	(
	State
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblBSCAI_Code ON dbo.tblBSCAI
	(
	Code
	) ON [PRIMARY]
GO
COMMIT

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblAllWarranty ADD
	DealerRefNum nvarchar(50) NULL
GO
CREATE NONCLUSTERED INDEX IX_tblAllWarranty_DealerRefNum ON dbo.tblAllWarranty
	(
	DealerRefNum
	) ON [PRIMARY]
GO
COMMIT

if exists (select * from sysobjects where id = object_id(N'[dbo].[waqrptRgaClaim]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[waqrptRgaClaim]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE VIEW dbo.waqrptRgaClaim
AS
SELECT dl.DealerName, dl.StreetAddress, dl.City, dl.State, dl.Zip, 
	wa.Customer, wa.DateOfFailure, wa.RGANum, wa.WarrantyID, 
	wa.Comment, wa.Problem, wa.Resolution, wa.MachineSerialNumber, wa.DateEntered, wa.DealerRefNum
FROM dbo.tblAllDealers dl INNER JOIN dbo.tblAllWarranty wa ON dl.DealerID = wa.fkDealerID

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO



insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.43','2000-4-24 00:00:00','- Prod Parts: Fixed Prod Parts Cash Flow report
- Prod Parts: Fixed sorting in Prod Parts Vendor Release Schedule by RPS Part Number report
- Parts: Cost Each, Dealer Net, and Sug List are updated when a sub part price is updated.
- Database: Created Tom Cat menu screen
- Parts: Increased size of Part Name field in label reports.
- BSCAI List: added form and reports
- Parts: Added ''Edit'' button for sub parts (you can also double click a line)
- Warranty/TC Warranty: Added ''Dealer Ref #'' field
- Orders: Prep Sheet report now works for 40HD and other alphanumeric model numbers
- Vendors: ''Cointact Name'' is now ''Contact Name''
')
go

update tblDBProperties
set PropertyValue = '3.43'
where PropertyName = 'DBStructVersion'
go
