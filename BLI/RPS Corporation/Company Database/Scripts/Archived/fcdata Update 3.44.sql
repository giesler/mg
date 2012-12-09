-- fcdata update 3.44
-- - update rel notes, db version


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[waspfrmWarrantyParts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[waspfrmWarrantyParts]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[wasprptDealerReimburse]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprptDealerReimburse]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[wasprptRgaClaimDates]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprptRgaClaimDates]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[wasprptWarrantyCosts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprptWarrantyCosts]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[wasprptWarrantyPending]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprptWarrantyPending]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[wasprptWarrantyRGA]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprptWarrantyRGA]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[wasprptWarrantyTotalCost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprptWarrantyTotalCost]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[wasprsubRGAParts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprsubRGAParts]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE waspfrmWarrantyParts
	@WarrantyID int = 0
as
select wa.WarrantyPartID, pa.RPSPartNum, pa.PartName, wa.PartCost
from dbo.tblWarrantyParts wa, dbo.tblParts pa
where wa.PartID = pa.PartID and wa.WarrantyID = @WarrantyID
order by pa.RPSPNSort
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[waspfrmWarrantyParts]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE wasprptDealerReimburse 
	@sDealerName varchar(40) = null,
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@sOpen varchar(2) = null, 
	@iWarrantyType int = 0
AS
-- Check input parameters
IF @sDealerName IS NULL
	SELECT @sDealerName = '%'
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @sOpen IS NULL
	SELECT @sOpen = '%'
IF @iWarrantyType IS NULL
	SELECT @iWarrantyType = 0
-- Run query
SELECT wa.Dealer, wa.Model, wa.MachineSerialNumber, wa.CreditMemoNum, wa.CreditMemoAmt, 
	wa.RGANum, wa.DateOfFailure, wa.WarrantyOpen
FROM dbo.tblWarrantyParts wp INNER JOIN dbo.tblAllWarranty wa ON wp.WarrantyID = wa.WarrantyID
WHERE wa.DateOfFailure Between @sFromDate and @sToDate
	and wa.Dealer Like @sDealerName
	and wa.WarrantyOpen Like @sOpen
	and wa.WarrantyType = @iWarrantyType
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptDealerReimburse]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE wasprptRgaClaimDates
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iWarrantyType int = 0
AS
-- Check input parameters
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iWarrantyType IS NULL
	SELECT @iWarrantyType = 0
SELECT dl.DealerName, dl.StreetAddress, dl.City, dl.State, dl.Zip, 
	dl.City + quotename(', ','''') + dl.State + quotename('  ', '''') + dl.Zip AS CityStateZip,
	wa.Customer, wa.DateOfFailure, wa.RGANum, wa.DateEntered, wa.Comment, wa.WarrantyID, 
	wa.MachineSerialNumber, wa.Problem, wa.Resolution
FROM dbo.tblAllDealers dl INNER JOIN dbo.tblAllWarranty wa ON dl.DealerID = wa.fkDealerID
WHERE wa.DateEntered Between @sFromDate And @sToDate
	AND wa.WarrantyType = @iWarrantyType
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptRgaClaimDates]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE wasprptWarrantyCosts 
	@sPartName varchar(40) = null,
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@sOpen varchar(2) = null,
	@iWarrantyType int = 0
AS
-- Check input parameters
IF @sPartName IS NULL
	SELECT @sPartName = '%'
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @sOpen IS NULL
	SELECT @sOpen = '%'
IF @iWarrantyType IS NULL
	SELECT @iWarrantyType = 0
-- Run query
SELECT     wa.MachineSerialNumber, wa.DateOfFailure, wa.CreditMemoNum, wa.Dealer, wa.Customer, wa.RGANum, wa.LaborCost, wa.Freight, wa.Problem, 
                      wa.WarrantyID, pa.RPSPartNum, pa.PartName, wp.PartCost, wa.Model, wa.Hours, wa.WarrantyOpen, pa.RPSPNSort
FROM         dbo.tblParts pa RIGHT OUTER JOIN
                      dbo.tblWarrantyParts wp RIGHT OUTER JOIN
                      dbo.tblAllWarranty wa ON wp.WarrantyID = wa.WarrantyID ON pa.PartID = wp.PartID
WHERE wa.DateOfFailure Between @sFromDate and @sToDate
	and pa.RPSPartNum like @sPartName
	and wa.WarrantyOpen like @sOpen
	and wa.WarrantyType = @iWarrantyType
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptWarrantyCosts]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE wasprptWarrantyPending
	@iWarrantyType int = 0
AS
SELECT     wa.Dealer, wa.MachineSerialNumber, wa.RGANum, pa.RPSPartNum, wa.WarrantyOpen, wa.Hours
FROM         dbo.tblWarranty wa LEFT OUTER JOIN
                      dbo.tblWarrantyParts wp ON wa.WarrantyID = wp.WarrantyID LEFT OUTER JOIN
                      dbo.tblParts pa ON wp.PartID = pa.PartID
WHERE     (wa.WarrantyType = @iWarrantyType) AND (wa.WarrantyOpen = 1)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptWarrantyPending]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE wasprptWarrantyRGA
	@iWarrantyType int = 0
AS
SELECT wa.RGANum, wa.MachineSerialNumber, wa.DateOfFailure, wa.CreditMemoNum, wa.Dealer, wa.Customer, wa.LaborCost, 
	wa.Freight, wa.Problem, pa.RPSPartNum, wa.WarrantyID, pa.PartName, wp.PartCost, wa.WarrantyOpen 
FROM dbo.tblParts pa RIGHT OUTER JOIN dbo.tblWarrantyParts wp INNER JOIN dbo.tblAllWarranty wa ON 
	wp.WarrantyID = wa.WarrantyID ON pa.PartID = wp.PartID
WHERE wa.WarrantyOpen = 1 and wa.WarrantyType = @iWarrantyType
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptWarrantyRGA]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE wasprptWarrantyTotalCost
	@sDealerName varchar(40) = null,
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@sOpen varchar(2) = null,
	@sFromDate2 varchar(20) = null,
	@sToDate2 varchar(20) = null,
	@iWarrantyType int = 0
AS
-- Check input parameters
IF @sDealerName IS NULL
	SELECT @sDealerName = '%'
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @sOpen IS NULL
	SELECT @sOpen = '%'
IF @sFromDate2 IS NULL
	SELECT @sFromDate2 = '1/1/1900'
IF @sToDate2 IS NULL
	SELECT @sToDate2 = '1/1/2100'
IF @iWarrantyType IS NULL
	SELECT @iWarrantyType = 0
-- Run query
SELECT wa.Dealer, wa.Model, wa.MachineSerialNumber, wa.DateOfFailure, wa.LaborCost, 
	SUM(wp.PartCost) AS ExtPartCost, wa.Travel, wa.Hours
FROM dbo.tblAllWarranty wa INNER JOIN dbo.tblWarrantyParts wp ON wa.WarrantyID = wp.WarrantyID 
WHERE wa.DateOfFailure Between @sFromDate and @sToDate
	and DateEntered Between @sFromDate2 and @sToDate2
	and wa.Dealer like @sDealerName
	and wa.WarrantyOpen like @sOpen
	and wa.WarrantyType = @iWarrantyType
GROUP BY wa.Dealer, wa.Model, wa.MachineSerialNumber, wa.DateOfFailure, wa.LaborCost, wa.Travel, wa.Hours
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptWarrantyTotalCost]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE wasprsubRGAParts
	@sID varchar(10) = null
AS
IF @sID = null
	SELECT @sID = 0

select pa.RPSPartNum, pa.PartName, wp.WarrantyID
from dbo.tblParts pa inner join dbo.tblWarrantyParts wp ON pa.PartID = wp.PartID
where wp.WarrantyID = @sID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprsubRGAParts]  TO [fcuser]
GO



insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.44','2000-5-1 00:00:00','- Warranty:  Pending reports now include RGAs with no parts
- Warranty:  Fixed all reports giving error messages (Also in TC)
- TC Orders:  Major Accounts now show up
- Prod Parts:  Vendor Release Schedule by PN actually sorts by PN
- Parts:  Cycle Counts reports now print by PN (they really do!)
- Orders:  Prep Sheets now work for 40HD and 52HD
')
go

update tblDBProperties
set PropertyValue = '3.44'
where PropertyName = 'DBStructVersion'
go
