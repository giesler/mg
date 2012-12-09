-- fcdata update 4.10

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[dlspfrmDealerAdvert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlspfrmDealerAdvert]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE dbo.dlspfrmDealerAdvert
	@DealerID int = null
AS

select *
from tblDealerAdvert
where DealerID = @DealerID
order by AdvertDate DESC
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[dlspfrmDealerAdvert]  TO [fcuser]
GO


update dbo.tblAllOrders
set dbo.tblAllOrders.fkDealerID = dl.DealerID
from dbo.tblAllOrders, dbo.tblAlldealers dl
where dbo.tblAllOrders.Dealer = dl.DealerName
	and dbo.tblAllOrders.fkDealerID <> dl.DealerID
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tblPartSales]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblPartSales]
GO

CREATE TABLE [dbo].[tblPartSales] (
	[PartSaleID] [int] IDENTITY (1, 1) NOT NULL ,
	[DealerID] [int] NULL ,
	[SaleDate] [smalldatetime] NULL ,
	[DealerPO] [nvarchar] (100) NULL ,
	[AmountParts] [smallmoney] NULL ,
	[AmountFreight] [smallmoney] NULL ,
	[AmountMisc] [smallmoney] NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblPartSales] WITH NOCHECK ADD 
	CONSTRAINT [PK_tblPartSales] PRIMARY KEY  CLUSTERED 
	(
		[PartSaleID]
	)  ON [PRIMARY] 
GO

 CREATE  INDEX [IX_tblPartSales_DealerID] ON [dbo].[tblPartSales]([DealerID]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_tblPartSales_SaleDate] ON [dbo].[tblPartSales]([SaleDate]) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblPartSales] ADD 
	CONSTRAINT [FK_tblPartSales_tblAllDealers] FOREIGN KEY 
	(
		[DealerID]
	) REFERENCES [dbo].[tblAllDealers] (
		[DealerID]
	)
GO


ALTER TABLE dbo.tblPartSales ADD
	PartSaleType tinyint NULL
GO
ALTER TABLE dbo.tblPartSales ADD CONSTRAINT
	DF_tblPartSales_PartSaleType DEFAULT 0 FOR PartSaleType
GO
ALTER TABLE dbo.tblPartSales ADD CONSTRAINT
	DF_tblPartSales_AmountParts DEFAULT 0 FOR AmountParts
GO
ALTER TABLE dbo.tblPartSales ADD CONSTRAINT
	DF_tblPartSales_AmountFreight DEFAULT 0 FOR AmountFreight
GO
ALTER TABLE dbo.tblPartSales ADD CONSTRAINT
	DF_tblPartSales_AmountMisc DEFAULT 0 FOR AmountMisc
GO
CREATE NONCLUSTERED INDEX IX_tblPartSales_PartSaleType ON dbo.tblPartSales
	(
	PartSaleType
	) ON [PRIMARY]
GO


insert into dbo.tblSwitchboard (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (44, 0, 'Part Sales', 0, '')
go

insert into dbo.tblSwitchboard (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (44, 1, 'Add/edit/view Part Sales', 3, 'psfrmPartSales')
go

update dbo.tblSwitchboard set ItemNumber = ItemNumber + 1
where SwitchboardID = 1 and ItemNumber > 10
go

insert into dbo.tblSwitchboard (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (1, 11, 'Part Sales', 1, '44')
go


insert into dbo.tblSwitchboard (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (45, 0, 'TC Part Sales', 0, '')
go

insert into dbo.tblSwitchboard (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (45, 1, 'Add/edit/view TC Part Sales', 3, 'psfrmPartSales')
go

update dbo.tblSwitchboard set ItemNumber = ItemNumber + 1
where SwitchboardID = 41 and ItemNumber > 6
go

insert into dbo.tblSwitchboard (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (41, 7, 'Tom Cat Part Sales', 1, '45')
go

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[wasprptDealerReimburse]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprptDealerReimburse]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.wasprptDealerReimburse 
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
	wa.RGANum, wa.DateOfFailure, wa.WarrantyOpen, wa.DealerRefNum
FROM dbo.tblAllWarranty wa 
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


ALTER TABLE dbo.tblAllWarranty ADD
	NewPart bit NULL
GO
ALTER TABLE dbo.tblAllWarranty ADD CONSTRAINT
	DF_tblAllWarranty_NewPart DEFAULT 0 FOR NewPart
GO

UPDATE dbo.tblAllWarranty SET NewPart = 0
GO

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

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.waspfrmWarrantyParts
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

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.wasprptDealerReimburse 
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
	wa.RGANum, wa.DateOfFailure, wa.WarrantyOpen, wa.DealerRefNum
FROM dbo.tblAllWarranty wa 
WHERE wa.DateOfFailure Between @sFromDate and @sToDate
	and wa.Dealer Like @sDealerName
	and wa.WarrantyOpen Like @sOpen
	and wa.WarrantyType = @iWarrantyType
	and wa.NewPart = 0
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptDealerReimburse]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.wasprptRgaClaimDates
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
	wa.MachineSerialNumber, wa.Problem, wa.Resolution, wa.DealerRefNum, wa.WarrantyType
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

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.wasprptWarrantyCosts 
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
                      wa.WarrantyID, pa.RPSPartNum, pa.PartName, wp.PartCost, wa.Model, wa.Hours, wa.WarrantyOpen, pa.RPSPNSort, wa.DealerRefNum
FROM         dbo.tblParts pa RIGHT OUTER JOIN
                      dbo.tblWarrantyParts wp RIGHT OUTER JOIN
                      dbo.tblAllWarranty wa ON wp.WarrantyID = wa.WarrantyID ON pa.PartID = wp.PartID
WHERE wa.DateOfFailure Between @sFromDate and @sToDate
	and pa.RPSPartNum like @sPartName
	and wa.WarrantyOpen like @sOpen
	and wa.WarrantyType = @iWarrantyType
	and wa.NewPart = 0
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptWarrantyCosts]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.wasprptWarrantyPending
	@iWarrantyType int = 0,
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@sOpen varchar(2) = null,	
	@sDealerName varchar(255) = null
AS
SELECT     	wa.Dealer, wa.MachineSerialNumber, wa.RGANum, wa.WarrantyOpen, wa.Hours, 
		wa.DealerRefNum, wa.Comment, wa.WarrantyID
FROM         	dbo.tblAllWarranty wa
WHERE     
	wa.NewPart = 0 and 
	wa.WarrantyType = @iWarrantyType 
	AND CASE
			WHEN @sOpen IS NULL THEN 1
			WHEN wa.WarrantyOpen = @sOpen THEN 1
			ELSE 0 END = 1
	AND CASE
			WHEN @sFromDate IS NULL AND @sToDate IS NULL THEN 1
			WHEN @sFromDate IS NULL AND @sToDate IS NOT NULL
				AND wa.DateOfFailure < @sToDate THEN 1
			WHEN @sFromDate IS NOT NULL AND @sToDate IS NULL
				AND wa.DateOfFailure > @sFromDate THEN 1
			WHEN @sFromDate IS NOT NULL AND @sToDate IS NOT NULL
				AND wa.DateOfFailure Between @sFromDate and @sToDate THEN 1
			ELSE 0 END = 1
	AND CASE
			WHEN @sDealerName IS NOT NULL AND wa.Dealer = @sDealerName THEN 1
			WHEN @sDealerName IS NULL THEN 1
			ELSE 0 END = 1
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptWarrantyPending]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.wasprptWarrantyRGA
	@iWarrantyType int = 0
AS
SELECT wa.RGANum, wa.MachineSerialNumber, wa.DateOfFailure, wa.CreditMemoNum, wa.Dealer, wa.Customer, wa.LaborCost, 
	wa.Freight, wa.Problem, pa.RPSPartNum, wa.WarrantyID, pa.PartName, wp.PartCost, wa.WarrantyOpen, wa.WarrantyType, wa.DealerRefNum
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

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.wasprptWarrantyTotalCost
	@sDealerName varchar(40) = null,
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@sOpen varchar(2) = null,
	@sFromDate2 varchar(20) = null,
	@sToDate2 varchar(20) = null,
	@sFromDate3 varchar(20) = null,
	@sToDate3 varchar(20) = null,
	@iWarrantyType int = 0
AS
-- Run query
SELECT wa.Dealer, wa.Model, wa.MachineSerialNumber, wa.CreditMemoDate, wa.CreditMemoAmt, wa.DateOfFailure, wa.LaborCost, 
	wp.ExtPartCost, wa.Travel, wa.Hours
FROM dbo.tblAllWarranty wa 
	INNER JOIN (SELECT WarrantyID, SUM(PartCost) AS ExtPartCost FROM dbo.tblWarrantyParts GROUP BY WarrantyID) wp 
		ON wa.WarrantyID = wp.WarrantyID 
WHERE 
	wa.NewPart = 0 AND 
	CASE 	WHEN @sFromDate IS NULL AND @sToDate IS NULL THEN 1
		WHEN @sFromDate IS NULL AND wa.DateOfFailure < @sToDate THEN 1
		WHEN @sFromDate < wa.DateOfFailure AND @sToDate IS NULL THEN 1
		WHEN wa.DateOfFailure Between @sFromDate and @sToDate THEN 1
		ELSE 0 END = 1 AND
	CASE 	WHEN @sFromDate2 IS NULL AND @sToDate2 IS NULL THEN 1
		WHEN @sFromDate2 IS NULL AND wa.DateEntered < @sToDate2 THEN 1
		WHEN @sFromDate2 < wa.DateEntered AND @sToDate2 IS NULL THEN 1
		WHEN wa.DateEntered Between @sFromDate2 and @sToDate2 THEN 1
		ELSE 0 END = 1 AND
	CASE 	WHEN @sFromDate3 IS NULL AND @sToDate3 IS NULL THEN 1
		WHEN @sFromDate3 IS NULL AND wa.CreditMemoDate < @sToDate3 THEN 1
		WHEN @sFromDate3 < wa.CreditMemoDate AND @sToDate2 IS NULL THEN 1
		WHEN wa.CreditMemoDate Between @sFromDate3 and @sToDate3 THEN 1
		ELSE 0 END = 1 AND
	CASE	WHEN @sDealerName IS NULL THEN 1
		WHEN wa.Dealer LIKE @sDealerName THEN 1
		ELSE 0 END = 1 AND
	CASE 	WHEN @sOpen IS NULL THEN 1
		WHEN wa.WarrantyOpen = @sOpen THEN 1
		ELSE 0 END = 1 AND
	CASE	WHEN @iWarrantyType IS NULL THEN 1
		WHEN wa.WarrantyTYpe = @iWarrantyType THEN 1
		ELSE 0 END = 1
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptWarrantyTotalCost]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.wasprsubRGAParts
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

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[psrptPartSales]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[psrptPartSales]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE dbo.psrptPartSales
	@FromDate varchar(20) = null,
	@ToDate varchar(20) = null,
	@DealerName varchar(100) = null,
	@PartSaleType varchar(2) = null
AS

SELECT dl.DealerName, ps.SaleDate, ps.DealerPO, ps.AmountParts, ps.AmountFreight, ps.AmountMisc, 
	ps.AmountParts + ps.AmountFreight + ps.AmountMisc AS AmountTotal
FROM dbo.tblPartSales ps
	INNER JOIN dbo.tblAllDealers dl ON dl.DealerID = ps.DealerID
WHERE 	
	CASE 	WHEN @PartSaleType IS NULL THEN 1
		WHEN PartSaleType = @PartSaleType THEN 1
		ELSE 0 END = 1 AND
	-- Sale Date criteria
	CASE
		WHEN @FromDate IS NULL AND @ToDate IS NULL THEN 1
		WHEN @FromDate IS NULL AND @ToDate IS NOT NULL
			AND SaleDate < @ToDate THEN 1
		WHEN @FromDate IS NOT NULL AND @ToDate IS NULL
			AND SaleDate > @FromDate THEN 1
		WHEN @FromDate IS NOT NULL AND @ToDate IS NOT NULL
			AND SaleDate Between @FromDate and @ToDate THEN 1
		ELSE 0 END = 1 AND
	-- Dealer Name
	CASE	WHEN @DealerName IS NULL THEN 1
		WHEN dl.DealerName = @DealerName THEN 1
		ELSE 0 END = 1
ORDER BY DealerName, SaleDate
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[psrptPartSales]  TO [fcuser]
GO

insert into dbo.tblSwitchboard (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (44, 2, 'Part Sales Report', 4, 'psrptPartSales')
go

insert into dbo.tblSwitchboard (SwitchboardID, ItemNumber, ItemText, Command, Argument, OpenArgs)
values (45, 2, 'Part Sales Report', 4, 'psrptPartSales', 'TomCatMode')
go


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[orsprptDealerOrderWarranty]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptDealerOrderWarranty]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE dbo.orsprptDealerOrderWarranty
	@DealerName varchar(40) = null,
	@FromDate varchar(20) = null,
	@ToDate varchar(20) = null,
	@Type varchar(3) = 0
AS
SET NOCOUNT ON
-- Get warranty totals
CREATE TABLE #tmpWarranty
	(DealerName nvarchar(100), LineYear nvarchar(100), LineQuarter nvarchar(100),
	WarrantyTotal money, OrderTotal money)
INSERT INTO #tmpWarranty (DealerName, LineYear, LineQuarter, WarrantyTotal)
select dl.DealerName, DatePart(yy, wa.DateOfFailure) as LineYear, DatePart(qq, wa.DateOfFailure) as LineQuarter,
	SUM(ISNULL(wp.ExtPartCost,0) + ISNULL(wa.LaborCost,0) + ISNULL(wa.Travel,0)) AS WarrantyCost
from tblAllWarranty wa
	INNER JOIN tblAllDealers dl ON dl.DealerID = wa.fkDealerID
	INNER JOIN (SELECT WarrantyID, SUM(PartCost) AS ExtPartCost FROM dbo.tblWarrantyParts GROUP BY WarrantyID) wp 
		ON wa.WarrantyID = wp.WarrantyID 
where wa.DateOfFailure IS NOT NULL and wa.NewPart = 0 and dl.CurrentDealer = 1 and
	CASE 	WHEN @Type IS NULL THEN 1
		WHEN WarrantyType = @Type THEN 1
		ELSE 0 END = 1 AND
	-- Warranty Date criteria
	CASE
		WHEN @FromDate IS NULL AND @ToDate IS NULL THEN 1
		WHEN @FromDate IS NULL AND @ToDate IS NOT NULL
			AND wa.DateOfFailure < @ToDate THEN 1
		WHEN @FromDate IS NOT NULL AND @ToDate IS NULL
			AND wa.DateOfFailure > @FromDate THEN 1
		WHEN @FromDate IS NOT NULL AND @ToDate IS NOT NULL
			AND wa.DateOfFailure Between @FromDate and @ToDate THEN 1
		ELSE 0 END = 1 AND
	-- Dealer Name
	CASE	WHEN @DealerName IS NULL THEN 1
		WHEN dl.DealerName = @DealerName THEN 1
		ELSE 0 END = 1
group by dl.DealerName, DatePart(yy, wa.DateOfFailure), DatePart(qq, wa.DateOfFailure)
order by dl.DealerName, LineYear, LineQuarter

-- Get Order totals
CREATE TABLE #tmpOrders
	(DealerName nvarchar(100), LineYear nvarchar(100), LineQuarter nvarchar(100),
	WarrantyTotal money, OrderTotal money)
INSERT INTO #tmpOrders (DealerName, LineYear, LineQuarter, OrderTotal)
select dl.DealerName, DatePart(yy, od.OrderDate) AS LineYear, DatePart(qq, od.OrderDate) as LineQuarter,
	SUM(ISNULL(od.SalePrice*od.Quantity,0)) AS OrdersSalePrice
from tblAllOrders od
	INNER JOIN tblAllDealers dl ON dl.DealerID = od.fkDealerID
where od.OrderDate IS NOT NULL and dl.CurrentDealer = 1 and
	CASE 	WHEN @Type IS NULL THEN 1
		WHEN OrderType = @Type THEN 1
		ELSE 0 END = 1 AND
	-- Warranty Date criteria
	CASE
		WHEN @FromDate IS NULL AND @ToDate IS NULL THEN 1
		WHEN @FromDate IS NULL AND @ToDate IS NOT NULL
			AND od.OrderDate < @ToDate THEN 1
		WHEN @FromDate IS NOT NULL AND @ToDate IS NULL
			AND od.OrderDate > @FromDate THEN 1
		WHEN @FromDate IS NOT NULL AND @ToDate IS NOT NULL
			AND od.OrderDate Between @FromDate and @ToDate THEN 1
		ELSE 0 END = 1 AND
	-- Dealer Name
	CASE	WHEN @DealerName IS NULL THEN 1
		WHEN dl.DealerName = @DealerName THEN 1
		ELSE 0 END = 1
group by dl.DealerName, DatePart(yy, od.OrderDate), DatePart(qq, od.OrderDate)
order by dl.DealerName, LineYear, LineQuarter
-- Now update Order amount for matching records in warranty table
UPDATE #tmpWarranty
SET OrderTotal = #tmpOrders.OrderTotal
FROM #tmpOrders, #tmpWarranty
WHERE #tmpWarranty.DealerName = #tmpOrders.DealerName
	and #tmpWarranty.LineYear = #tmpOrders.LineYear
	and #tmpWarranty.LineQuarter = #tmpOrders.LineQuarter
-- And finally, update Warranty table with recs only in Orders table
INSERT INTO #tmpWarranty (DealerName, LineYear, LineQuarter, OrderTotal)
SELECT ord.DealerName, ord.LineYear, ord.LineQuarter, ord.OrderTotal
FROM #tmpOrders ord
	LEFT OUTER JOIN #tmpWarranty war ON war.DealerName = ord.DealerName and 
		war.LineYear = ord.LineYear and war.LineQuarter = ord.LineQuarter
WHERE war.DealerName IS NULL and war.LineYear IS NULL and war.LineQuarter IS NULL
UPDATE #tmpWarranty
SET OrderTotal = 0
WHERE OrderTotal IS NULL
UPDATE #tmpWarranty
SET WarrantyTotal = 0
WHERE WarrantyTotal IS NULL
SELECT *
FROM #tmpWarranty
ORDER BY DealerName, LineYear, LineQuarter
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptDealerOrderWarranty]  TO [fcuser]
GO

insert into dbo.tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('4.10','2001-1-3','- FC/TC Orders:  Fixed Sales Rep Commission Report
- FC/TC Dealers:  Fixed Advert Alloc not working, also fixed ''Delete'' on Goals
- TC Prospects:  Changed ''Active'' to default to true when adding a new record
- TC Prospects:  Changed by State report to include A/I column
- Release Notes:  Hopefully finally fixed printing the release notes
- FC/TC Part Sales:  Added this new database section, a form and a report
- FC/TC Warranty:  Fixed ''Dealer Warranty Reimbursement'' showing duplicates
- FC/TC Orders:  Fixed ''Orders vs. Warranty'' report showing incorrect warranty totals
- FC/TC Warranty:  Added ''New Part'' check box.  Also updated reports to not print warranty records with ''New Part'' checked.
')
go

update dbo.tblDBProperties
set PropertyValue = '4.10'
where PropertyName = 'DBStructVersion'
go

