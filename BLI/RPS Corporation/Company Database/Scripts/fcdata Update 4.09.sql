-- fcdata update 4.09

delete from dbo.tblMenuHistory
go


EXECUTE sp_rename N'dbo.tblMenuHistory.HistoryID', N'Tmp_ID', 'COLUMN'
GO
EXECUTE sp_rename N'dbo.tblMenuHistory.HistoryTime', N'Tmp_DateTime_1', 'COLUMN'
GO
EXECUTE sp_rename N'dbo.tblMenuHistory.HistoryUser', N'Tmp_UserID_2', 'COLUMN'
GO
EXECUTE sp_rename N'dbo.tblMenuHistory.HistorySwitchID', N'Tmp_SwitchboardID_3', 'COLUMN'
GO
EXECUTE sp_rename N'dbo.tblMenuHistory.Tmp_ID', N'ID', 'COLUMN'
GO
EXECUTE sp_rename N'dbo.tblMenuHistory.Tmp_DateTime_1', N'DateTime', 'COLUMN'
GO
EXECUTE sp_rename N'dbo.tblMenuHistory.Tmp_UserID_2', N'UserID', 'COLUMN'
GO
EXECUTE sp_rename N'dbo.tblMenuHistory.Tmp_SwitchboardID_3', N'SwitchboardID', 'COLUMN'
GO
ALTER TABLE dbo.tblMenuHistory
	DROP COLUMN HistorySwitchItem
GO

EXECUTE sp_rename N'dbo.tblMenuHistory.DateTime', N'Tmp_HistoryTime_8', 'COLUMN'
GO
EXECUTE sp_rename N'dbo.tblMenuHistory.Tmp_HistoryTime_8', N'HistoryTime', 'COLUMN'
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spGetSwitchboard]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spGetSwitchboard]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spSecurityCheck]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSecurityCheck]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spGetSwitchboardItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spGetSwitchboardItem]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spSetSwitchboardHistory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSetSwitchboardHistory]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spDisplayTip]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spDisplayTip]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tblTip]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblTip]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tblTipDisable]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblTipDisable]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tblSysMessage]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblSysMessage]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spGetNTUserList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spGetNTUserList]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spGetUserSwitchboardItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spGetUserSwitchboardItem]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spGetUserSwitchboardItems]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spGetUserSwitchboardItems]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spGetUserSwitchboards]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spGetUserSwitchboards]
GO

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
	SUM(ISNULL(wa.PartCost,0) + ISNULL(wa.LaborCost,0) + ISNULL(wa.Travel,0)) AS WarrantyCost
from tblAllWarranty wa
	INNER JOIN tblAllDealers dl ON dl.DealerID = wa.fkDealerID
where wa.DateOfFailure IS NOT NULL and dl.CurrentDealer = 1 and
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
	SUM(ISNULL(od.SalePrice,0)) AS OrdersSalePrice
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


update dbo.[Switchboard Items]
set ItemNumber = ItemNumber + 1
where SwitchboardID = 14 and ItemNumber > 10
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (14, 11, 'Orders vs. Warranty Report', '4', 'orrptDealerOrderWarranty')
go

update dbo.[Switchboard Items]
set ItemNumber = ItemNumber + 1
where SwitchboardID = 33 and ItemNumber > 10
go

insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument, OpenArgs)
values (33, 11, 'Orders vs. Warranty Report', '4', 'orrptDealerOrderWarranty', 'TomCatMode')
go

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tempView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[tempView]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[vSwitchboard]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[vSwitchboard]
GO


EXECUTE sp_rename N'dbo.[Switchboard Items]', N'tblSwitchboard', 'OBJECT'
GO
ALTER TABLE dbo.tblSwitchboard
	DROP CONSTRAINT [aaaaaSwitchboard Items_PK]
GO
ALTER TABLE dbo.tblSwitchboard ADD CONSTRAINT
	tblSwitchboard_PK PRIMARY KEY NONCLUSTERED 
	(
	ID
	) WITH FILLFACTOR = 90 ON [PRIMARY]

GO
ALTER TABLE dbo.tblSwitchboard ADD CONSTRAINT
	IX_tblSwitchboard_SwitchItem UNIQUE NONCLUSTERED 
	(
	SwitchboardID,
	ItemText
	) ON [PRIMARY]

GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spGetNTUserList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spGetNTUserList]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spGetUserSwitchboardItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spGetUserSwitchboardItem]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spGetUserSwitchboardItems]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spGetUserSwitchboardItems]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spGetUserSwitchboards]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spGetUserSwitchboards]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.spGetNTUserList
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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[spGetNTUserList]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE dbo.spGetUserSwitchboardItem
	@sUser nvarchar(50),
	@iSwitchID int
AS

set nocount on

-- Keep track of history
insert dbo.tblMenuHistory (HistoryTime, UserID, SwitchboardID)
values (GETDATE(), @sUser, @iSwitchID)

-- Select current item
select *
from dbo.tblSwitchboard
where ID = @iSwitchID and ID not in ( select SwID from dbo.tblSecurity where UserID = @sUser and AccessType = 0)
	and ItemNumber <> 0
order by ItemNumber
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[spGetUserSwitchboardItem]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE dbo.spGetUserSwitchboardItems
	@sUser nvarchar(50),
	@iSwitch int
AS
set nocount on
create table #tmp
	( ID int not null, SwitchboardID int not null, ItemNumber int not null, ItemText nvarchar(255), Command int,
	Argument nvarchar(255), AccessType int )
insert #tmp (ID, SwitchboardID, ItemNumber, ItemText, Command, Argument, AccessType)
select ID, SwitchboardID, ItemNumber, ItemText, Command, Argument, 2
from dbo.tblSwitchboard
where SwitchboardID = @iSwitch and ItemNumber <> 0
delete #tmp
where ID in (select SwID from dbo.tblSecurity sc where sc.UserID = @sUser and sc.AccessType = 0)
update #tmp
set #tmp.AccessType = sc.AccessType
from dbo.tblSecurity sc, #tmp
where sc.SwID = #tmp.ID and sc.UserID = @sUser
select *
from #tmp
order by ItemNumber
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[spGetUserSwitchboardItems]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE dbo.spGetUserSwitchboards
	@sUser nvarchar(50),
	@iSwitch int
AS
-- Used in 'Add Switchboard' method
set nocount on
create table #tmp
	( ID int not null, SwitchboardID int not null, ItemNumber int not null, ItemText nvarchar(255), Command int,
	Argument nvarchar(255), AccessType int )
insert #tmp (ID, SwitchboardID, ItemNumber, ItemText, Command, Argument, AccessType)
select ID, SwitchboardID, ItemNumber, ItemText, Command, Argument, 2
from dbo.tblSwitchboard
where SwitchboardID = @iSwitch and ItemNumber <> 0 and Command = 1
delete #tmp
where ID in (select SwID from dbo.tblSecurity sc where sc.UserID = @sUser and sc.AccessType = 0)
update #tmp
set #tmp.AccessType = sc.AccessType
from dbo.tblSecurity sc, #tmp
where sc.SwID = #tmp.ID and sc.UserID = @sUser
select *
from #tmp
order by ItemNumber
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[spGetUserSwitchboards]  TO [fcuser]
GO

update dbo.tblAllOrders
set Model = '40'
where Model = '40HD'
go

update dbo.tblAllOrders
set Model = '52'
where Model = '52HD'
go

update dbo.tblSwitchboard
set ItemText = 'Option Parts by Model'
where id = 96
go

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ldsprptLeadsReports]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ldsprptLeadsReports]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.ldsprptLeadsReports
	@FromDate varchar(20) = null,
	@ToDate varchar(20) = null,
	@ActiveInactive varchar(5) = null,
	@DealerName varchar(100) = null,
	@CompanyName varchar(100) = null, 
	@ResponseMethod varchar(50) = null, 
	@LeadType varchar(2) = null
AS
SELECT DealerName, CompanyName, LeadDate, Contact, Phone, Result, City, ActiveInactive, Location, LeadID,
	SUBSTRING(Phone, 1, 3) AS AreaCode, Salesman, Address, 	State, Zip, ApplicationNotes, ContactTitle
FROM dbo.tblAllLeads
WHERE 	
	CASE 	WHEN @LeadType IS NULL THEN 1
		WHEN Purchased = @LeadType THEN 1
		ELSE 0 END = 1 AND
	-- Lead Date criteria
	CASE
		WHEN @FromDate IS NULL AND @ToDate IS NULL THEN 1
		WHEN @FromDate IS NULL AND @ToDate IS NOT NULL
			AND LeadDate < @ToDate THEN 1
		WHEN @FromDate IS NOT NULL AND @ToDate IS NULL
			AND LeadDate > @FromDate THEN 1
		WHEN @FromDate IS NOT NULL AND @ToDate IS NOT NULL
			AND LeadDate Between @FromDate and @ToDate THEN 1
		ELSE 0 END = 1 AND
	-- Active / Inactive
	CASE	WHEN @ActiveInactive IS NULL THEN 1
		WHEN ActiveInactive = @ActiveInactive THEN 1
		ELSE 0 END = 1 AND
	-- Dealer Name
	CASE	WHEN @DealerName IS NULL THEN 1
		WHEN DealerName = @DealerName THEN 1
		ELSE 0 END = 1 AND
	-- Company Name
	CASE 	WHEN @CompanyName IS NULL THEN 1
		WHEN CompanyName like '%' + @CompanyName + '%' THEN 1
		ELSE 0 END = 1 AND
	-- Response Method
	CASE 	WHEN @ResponseMethod IS NULL THEN 1
		WHEN ResponseMethod = @ResponseMethod THEN 1
		ELSE 0 END = 1
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[ldsprptLeadsReports]  TO [fcuser]
GO

insert into dbo.tblSwitchboard (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (2, 3, 'Add/edit Sales Rep Names', 3, 'dlfrmSalesRep')
go

insert into dbo.tblDBProperties (PropertyName, PropertyValue) values ('Security.SecAdminPW', 'none')
go


ALTER TABLE dbo.tblDealerAdvert
	DROP COLUMN upsize_ts
GO


EXECUTE sp_rename N'dbo.tblDealerAdvert.fkDealerID', N'Tmp_DealerID', 'COLUMN'
GO
EXECUTE sp_rename N'dbo.tblDealerAdvert.AdvertAmt', N'Tmp_Amount_1', 'COLUMN'
GO
EXECUTE sp_rename N'dbo.tblDealerAdvert.AdvertNote', N'Tmp_Note_2', 'COLUMN'
GO
EXECUTE sp_rename N'dbo.tblDealerAdvert.Tmp_DealerID', N'DealerID', 'COLUMN'
GO
EXECUTE sp_rename N'dbo.tblDealerAdvert.Tmp_Amount_1', N'Amount', 'COLUMN'
GO
EXECUTE sp_rename N'dbo.tblDealerAdvert.Tmp_Note_2', N'Note', 'COLUMN'
GO

update dbo.tblSwitchboard set ItemNumber = ItemNumber + 1 where SwitchboardID = 37 and ItemNumber > 0
go

insert into dbo.tblSwitchboard (SwitchboardID, ItemNumber, ItemText, Command, Argument, OpenArgs)
values (37, 1, 'Advertising Allocation', 4, 'dlrptAdvertAlloc', 'TomCatMode')
go

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[dlqrptMaillingLabelServicePartsMan]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqrptMaillingLabelServicePartsMan]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[dlqrptAdvertAlloc]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[dlqrptAdvertAlloc]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[dlqfrmDealerGoals]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqfrmDealerGoals]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[dlqrptDealerContractDate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqrptDealerContractDate]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[dlqrptDealerContractAlpha]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqrptDealerContractAlpha]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[dlqrptDealerListAddress]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqrptDealerListAddress]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[dlqrptDealerListFax]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqrptDealerListFax]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[dlqrptDealerListPhone]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqrptDealerListPhone]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[dlqrptMaillingLabels]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqrptMaillingLabels]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[dlspfrmDealersPartSales]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlspfrmDealersPartSales]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[dlqrptMaillingService]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqrptMaillingService]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[dlqfrmDealerGoals]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqfrmDealerGoals]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tblDealerPartSales]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblDealerPartSales]
GO

update dbo.tblSwitchboard set Argument = 'dlrptMailingLabelsNames', OpenArgs = 'TomCatMode' where ID = 221
go

update dbo.tblSwitchboard set Argument = 'dlrptDealerContractAlpha', OpenArgs = 'TomCatMode' where ID = 219
go

update dbo.tblSwitchboard set Argument = 'dlrptDealerContractDate', OpenArgs = 'TomCatMode' where ID = 218
go

update dbo.tblSwitchboard set Argument = 'dlrptDealerListAddress', OpenArgs = 'TomCatMode' where ID = 215
go

update dbo.tblSwitchboard set Argument = 'dlrptDealerListFax', OpenArgs = 'TomCatMode' where ID = 216
go

update dbo.tblSwitchboard set Argument = 'dlrptDealerListPhone', OpenArgs = 'TomCatMode' where ID = 214
go

update dbo.tblSwitchboard set Argument = 'dlrptMailingLabels', OpenArgs = 'TomCatMode' where ID = 217
go

update dbo.tblSwitchboard set Argument = 'dlrptMailingLabelsPhone', OpenArgs = 'TomCatMode' where ID = 220
go

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[dlsprptAdvertAlloc]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlsprptAdvertAlloc]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[dlsprptDealer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlsprptDealer]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[dlsprptMailingLabelsNames]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlsprptMailingLabelsNames]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE dbo.dlsprptAdvertAlloc
	@DealerType int = null
AS

SELECT dl.DealerName, YEAR(da.AdvertDate) as AdvertYear, sum(da.Amount) AS ExtAmount
FROM tblDealerAdvert da
	INNER JOIN tblAllDealers dl ON dl.DealerID = da.DealerID
WHERE dl.CurrentDealer = 1 AND
	CASE 	WHEN @DealerType IS NULL THEN 1
		WHEN DealerType = @DealerType THEN 1
		ELSE 0 END = 1
GROUP BY dl.DealerName, YEAR(da.AdvertDate)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[dlsprptAdvertAlloc]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE dbo.dlsprptDealer
	@DealerType int = 0
AS

SELECT DealerName, ContactName, StreetAddress, City, State, Zip, CurrentDealer, ContractExpires,
	Phone, TollFreeNumber, Fax, TerritoryCovered
FROM tblAllDealers
WHERE CurrentDealer = 1 AND 
	CASE 	WHEN @DealerType IS NULL THEN 1
		WHEN DealerType = @DealerType THEN 1
		ELSE 0 END = 1
ORDER BY DealerName, ContactName
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[dlsprptDealer]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE dbo.dlsprptMailingLabelsNames
	@PartsManager int = 1,
	@ServiceManager int = 1,
	@WarrantyAdmin int = 1,
	@Salesman int = 1,
	@OfficeManager int = 1,
	@DealerType int = null
AS

set nocount on

-- First create temp table to return
CREATE TABLE #tmpNames  (
	PersonName nvarchar(100), 
	DealerName nvarchar(100), 
	StreetAddress nvarchar(100),
	City nvarchar(50),
	State nvarchar(50),
	Zip nvarchar(20))


if @PartsManager <> 0
	INSERT INTO #tmpNames (PersonName, DealerName, StreetAddress, City, State, Zip)
	SELECT PartsManagerName, DealerName, StreetAddress, City, State, Zip
	FROM dbo.tblAllDealers
	WHERE CurrentDealer = 1 AND PartsManagerName IS NOT NULL AND
		CASE 	WHEN @DealerType IS NULL THEN 1
			WHEN DealerType = @DealerType THEN 1
			ELSE 0 END = 1

if @ServiceManager <> 0
	INSERT INTO #tmpNames (PersonName, DealerName, StreetAddress, City, State, Zip)
	SELECT ServiceManagerName, DealerName, StreetAddress, City, State, Zip
	FROM dbo.tblAllDealers
	WHERE CurrentDealer = 1 AND ServiceManagerName IS NOT NULL AND
		CASE 	WHEN @DealerType IS NULL THEN 1
			WHEN DealerType = @DealerType THEN 1
			ELSE 0 END = 1

if @WarrantyAdmin <> 0
	INSERT INTO #tmpNames (PersonName, DealerName, StreetAddress, City, State, Zip)
	SELECT WarrentyAdministrator, DealerName, StreetAddress, City, State, Zip
	FROM dbo.tblAllDealers
	WHERE CurrentDealer = 1 AND WarrentyAdministrator IS NOT NULL AND
		CASE 	WHEN @DealerType IS NULL THEN 1
			WHEN DealerType = @DealerType THEN 1
			ELSE 0 END = 1

if @Salesman <> 0
	INSERT INTO #tmpNames (PersonName, DealerName, StreetAddress, City, State, Zip)
	SELECT SalesmanName, DealerName, StreetAddress, City, State, Zip
	FROM dbo.tblAllDealers
	WHERE CurrentDealer = 1 AND SalesmanName IS NOT NULL AND
		CASE 	WHEN @DealerType IS NULL THEN 1
			WHEN DealerType = @DealerType THEN 1
			ELSE 0 END = 1

if @OfficeManager <> 0
	INSERT INTO #tmpNames (PersonName, DealerName, StreetAddress, City, State, Zip)
	SELECT OfficeManagerName, DealerName, StreetAddress, City, State, Zip
	FROM dbo.tblAllDealers
	WHERE CurrentDealer = 1 AND OfficeManagerName IS NOT NULL AND
		CASE 	WHEN @DealerType IS NULL THEN 1
			WHEN DealerType = @DealerType THEN 1
			ELSE 0 END = 1

SELECT * FROM #tmpNames
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[dlsprptMailingLabelsNames]  TO [fcuser]
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[orsprptAcknowledgement]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptAcknowledgement]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[orsprptDealerOrderWarranty]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptDealerOrderWarranty]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.orsprptAcknowledgement
	@iOrderID varchar(40) = null,
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(2) = 0
AS
-- Check input parameters
IF @iOrderID IS NULL
	SELECT @iOrderID = '%'
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
-- Run query
SELECT ao.OrderID, ao.Dealer, ao.OrderDate, ao.PurchaseOrder, ao.ShipName, ao.StreetAddress, ao.CityStateZip, 
	ao.Quantity, ao.Model, md.Description, md.Price * ao.Quantity AS Price, ao.Options, ao.PromisedDate, ao.ShipVia, ao.OrderNumber, 
	ao.CollectPrepaid, ao.Terms
FROM dbo.tblAllOrders ao, dbo.tblModels md
WHERE ao.Model = md.Model AND OrderType = @iOrderType
	AND (ao.OrderDate Between @sFromDate And @sToDate)
	AND ao.OrderID like @iOrderID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptAcknowledgement]  TO [fcuser]
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
	SUM(ISNULL(wa.PartCost,0) + ISNULL(wa.LaborCost,0) + ISNULL(wa.Travel,0)) AS WarrantyCost
from tblAllWarranty wa
	INNER JOIN tblAllDealers dl ON dl.DealerID = wa.fkDealerID
where wa.DateOfFailure IS NOT NULL and dl.CurrentDealer = 1 and
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

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[orsprptDealerSales]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptDealerSales]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[orsprptDealerSerial]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptDealerSerial]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.orsprptDealerSales 
	@sDealerName varchar(40) = null,
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @sDealerName IS NULL
	SELECT @sDealerName = '%'
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
-- Run query
SELECT DISTINCT ao.Dealer, ao.Model, ao.OrderDate, ao.ShippedDate, ao.Quantity, ao.SalePrice * ao.Quantity AS SalePrice, ao.OrderNumber, ao.SerialNumber
FROM tblAllOrders ao INNER JOIN tblAllDealers dl ON ao.Dealer = dl.DealerName
WHERE (dl.CurrentDealer = 1) AND SalePrice <> 0 AND ao.OrderType = @iOrderType
	AND (ao.Dealer like @sDealerName)
	AND (ao.OrderDate Between @sFromDate And @sToDate)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptDealerSales]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.orsprptDealerSerial
	@sDealerName varchar(40) = null,
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @sDealerName IS NULL
	SELECT @sDealerName = '%'
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT ao.Dealer, ao.Model, ao.SerialNumber, ao.Name, ao.SaleDate, ao.Quantity, ao.SalePrice * ao.Quantity AS SalePrice, ao.ShippedDate
FROM tblAllOrders ao
WHERE ao.OrderType = @iOrderType
	AND (ao.Dealer like @sDealerName)
	AND (ao.ShippedDate Between @sFromDate and @sToDate)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptDealerSerial]  TO [fcuser]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[orsprptMajorAccountSales]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptMajorAccountSales]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[orsprptMarginByModel]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptMarginByModel]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[orsprptOpenOrders]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptOpenOrders]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.orsprptMajorAccountSales
	@sMAName varchar(40) = null,
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @sMAName IS NULL
	SELECT @sMAName = '%'
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT ma.MACompName, ma.MAHeadqAddress, ma.MACity, ma.MAState, 
    ma.MAZip, ao.SaleDate, ao.[Name], ao.Address, ao.City, ao.State, ao.Zip, 
    ao.Dealer, ao.Model, ao.SalePrice * ao.Quantity AS SalePrice, ao.OrderDate
FROM dbo.tblAllMajorAcnts ma INNER JOIN dbo.tblAllOrders ao ON 
    ma.MajorAccountID = ao.MajorAccountID
WHERE ao.OrderType = @iOrderType
	AND ao.OrderDate Between @sFromDate And @sToDate
	AND ma.MACompName like @sMAName
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptMajorAccountSales]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.orsprptMarginByModel
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT ShippedDate, Dealer, Model, OrderNumber, Quantity, SalePrice * Quantity AS SalePrice, CostPrice * Quantity AS CostPrice, (SalePrice - CostPrice) * Quantity AS Margin, SerialNumber 
FROM dbo.tblAllOrders 
WHERE (SalePrice <> 0) 
	AND (OrderType = @iOrderType)
	AND (ShippedDate Between @sFromDate and @sToDate)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptMarginByModel]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.orsprptOpenOrders
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT  ShippedDate, Model, Dealer, OrderNumber, Quantity, SalePrice * Quantity AS SalePrice, CostPrice * Quantity AS CostPrice, Margin * Quantity AS Margin
FROM dbo.tblAllOrders
WHERE (ShippedDate Is Null)
	AND (OrderType = @iOrderType)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptOpenOrders]  TO [fcuser]
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[orsprptSalesDealer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptSalesDealer]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[orsprptSalesModel]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptSalesModel]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[orsprptSalesModelON]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptSalesModelON]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.orsprptSalesDealer
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT Dealer, Model, OrderNumber, ShippedDate, Quantity, SalePrice * Quantity AS SalePrice, CostPrice * Quantity AS CostPrice, (SalePrice - CostPrice)*Quantity as Margin, OrderDate 
FROM dbo.tblAllOrders 
WHERE (SalePrice <> 0) 
	AND (OrderType = @iOrderType)
	AND (OrderDate Between @sFromDate and @sToDate)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptSalesDealer]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.orsprptSalesModel
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS 
-- Check input parameters
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT Model, Dealer, Sum(Quantity) AS Quantity, Sum(SalePrice*Quantity) AS SalePrice, Sum(CostPrice*Quantity) AS CostPrice, 
	Sum((SalePrice - CostPrice) * Quantity) AS Margin
FROM tblAllOrders
WHERE SalePrice <> 0
	AND (OrderType = @iOrderType)
	AND (OrderDate Between @sFromDate and @sToDate)
GROUP BY Model, Dealer
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptSalesModel]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.orsprptSalesModelON
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS 
-- Check input parameters
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT Model, Dealer, OrderNumber, SalePrice*Quantity AS SalePrice, CostPrice*Quantity AS CostPrice, (SalePrice - CostPrice)*Quantity AS Margin
FROM tblAllOrders
WHERE SalePrice <> 0
	AND (OrderType = @iOrderType)
	AND (OrderDate Between @sFromDate and @sToDate)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptSalesModelON]  TO [fcuser]
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[orsprptSalesRepCommission]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptSalesRepCommission]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE dbo.orsprptSalesRepCommission
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null
AS 
-- Check input parameters
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
--- Run query
SELECT o.Model, o.Dealer, o.Quantity, o.SalePrice*o.Quantity AS SalePrice, o.OrderNumber, o.OrderType, r.DealerRepName
FROM tblAllOrders o
	INNER JOIN tblAllDealers d ON d.DealerID = o.fkDealerID
	INNER JOIN tlkuDealerRep r ON r.DealerRepID = d.DealerRepID
WHERE o.SalePrice <> 0
	AND (o.OrderDate Between @sFromDate and @sToDate)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptSalesRepCommission]  TO [fcuser]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ldsprptLeadActionSummary]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ldsprptLeadActionSummary]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE dbo.ldsprptLeadActionSummary
	@FromDate varchar(20) = null,
	@ToDate varchar(20) = null,
	@ActiveInactive varchar(5) = null,
	@DealerName varchar(100) = null,
	@CompanyName varchar(100) = null, 
	@ResponseMethod varchar(50) = null, 
	@LeadType varchar(2) = null
AS

SET NOCOUNT ON

CREATE TABLE #tmpTotals (
	DealerName nvarchar(100),
	LeadTotal int,
	DemoTotal int,
	SoldTotal int)

-- Total lead counts
INSERT INTO #tmpTotals (DealerName, LeadTotal, DemoTotal, SoldTotal)
SELECT DealerName, COUNT(LeadID), 0, 0
FROM dbo.tblAllLeads
WHERE DealerName IS NOT NULL AND
	CASE 	WHEN @LeadType IS NULL THEN 1
		WHEN Purchased = @LeadType THEN 1
		ELSE 0 END = 1 AND
	-- Lead Date criteria
	CASE
		WHEN @FromDate IS NULL AND @ToDate IS NULL THEN 1
		WHEN @FromDate IS NULL AND @ToDate IS NOT NULL
			AND LeadDate < @ToDate THEN 1
		WHEN @FromDate IS NOT NULL AND @ToDate IS NULL
			AND LeadDate > @FromDate THEN 1
		WHEN @FromDate IS NOT NULL AND @ToDate IS NOT NULL
			AND LeadDate Between @FromDate and @ToDate THEN 1
		ELSE 0 END = 1 AND
	-- Active / Inactive
	CASE	WHEN @ActiveInactive IS NULL THEN 1
		WHEN ActiveInactive = @ActiveInactive THEN 1
		ELSE 0 END = 1 AND
	-- Dealer Name
	CASE	WHEN @DealerName IS NULL THEN 1
		WHEN DealerName = @DealerName THEN 1
		ELSE 0 END = 1 AND
	-- Company Name
	CASE 	WHEN @CompanyName IS NULL THEN 1
		WHEN CompanyName like '%' + @CompanyName + '%' THEN 1
		ELSE 0 END = 1 AND
	-- Response Method
	CASE 	WHEN @ResponseMethod IS NULL THEN 1
		WHEN ResponseMethod = @ResponseMethod THEN 1
		ELSE 0 END = 1
GROUP BY DealerName
ORDER BY DealerName

-- Figure out action totals
UPDATE #tmpTotals
SET SoldTotal = (SELECT COUNT(LeadID)
		FROM dbo.tblAllLeads ld
		WHERE ld.DealerName = #tmpTotals.DealerName AND LeadAction = 'Sold' AND
			CASE 	WHEN @LeadType IS NULL THEN 1
				WHEN Purchased = @LeadType THEN 1
				ELSE 0 END = 1 AND
			-- Lead Date criteria
			CASE
				WHEN @FromDate IS NULL AND @ToDate IS NULL THEN 1
				WHEN @FromDate IS NULL AND @ToDate IS NOT NULL
					AND LeadDate < @ToDate THEN 1
				WHEN @FromDate IS NOT NULL AND @ToDate IS NULL
					AND LeadDate > @FromDate THEN 1
				WHEN @FromDate IS NOT NULL AND @ToDate IS NOT NULL
					AND LeadDate Between @FromDate and @ToDate THEN 1
				ELSE 0 END = 1 AND
			-- Active / Inactive
			CASE	WHEN @ActiveInactive IS NULL THEN 1
				WHEN ActiveInactive = @ActiveInactive THEN 1
				ELSE 0 END = 1 AND
			-- Dealer Name
			CASE	WHEN @DealerName IS NULL THEN 1
				WHEN DealerName = @DealerName THEN 1
				ELSE 0 END = 1 AND
			-- Company Name
			CASE 	WHEN @CompanyName IS NULL THEN 1
				WHEN CompanyName like '%' + @CompanyName + '%' THEN 1
				ELSE 0 END = 1 AND
			-- Response Method
			CASE 	WHEN @ResponseMethod IS NULL THEN 1
				WHEN ResponseMethod = @ResponseMethod THEN 1
				ELSE 0 END = 1)
FROM #tmpTotals, dbo.tblAllLeads

UPDATE #tmpTotals
SET DemoTotal = (SELECT COUNT(LeadID)
		FROM dbo.tblAllLeads ld
		WHERE ld.DealerName = #tmpTotals.DealerName AND LeadAction = 'Demo' AND
			CASE 	WHEN @LeadType IS NULL THEN 1
				WHEN Purchased = @LeadType THEN 1
				ELSE 0 END = 1 AND
			-- Lead Date criteria
			CASE
				WHEN @FromDate IS NULL AND @ToDate IS NULL THEN 1
				WHEN @FromDate IS NULL AND @ToDate IS NOT NULL
					AND LeadDate < @ToDate THEN 1
				WHEN @FromDate IS NOT NULL AND @ToDate IS NULL
					AND LeadDate > @FromDate THEN 1
				WHEN @FromDate IS NOT NULL AND @ToDate IS NOT NULL
					AND LeadDate Between @FromDate and @ToDate THEN 1
				ELSE 0 END = 1 AND
			-- Active / Inactive
			CASE	WHEN @ActiveInactive IS NULL THEN 1
				WHEN ActiveInactive = @ActiveInactive THEN 1
				ELSE 0 END = 1 AND
			-- Dealer Name
			CASE	WHEN @DealerName IS NULL THEN 1
				WHEN DealerName = @DealerName THEN 1
				ELSE 0 END = 1 AND
			-- Company Name
			CASE 	WHEN @CompanyName IS NULL THEN 1
				WHEN CompanyName like '%' + @CompanyName + '%' THEN 1
				ELSE 0 END = 1 AND
			-- Response Method
			CASE 	WHEN @ResponseMethod IS NULL THEN 1
				WHEN ResponseMethod = @ResponseMethod THEN 1
				ELSE 0 END = 1)
FROM #tmpTotals, dbo.tblAllLeads

SELECT * FROM #tmpTotals
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[ldsprptLeadActionSummary]  TO [fcuser]
GO

update dbo.tblSwitchboard set ItemNumber = ItemNumber + 1 where SwitchboardID = 7 and ItemNumber > 0
go

insert into dbo.tblSwitchboard (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (7, 1, 'Lead Action Summary', 4, 'ldrptLeadActionSummary')
go

update dbo.tblSwitchboard set ItemNumber = ItemNumber + 1 where SwitchboardID = 25 and ItemNumber > 0
go

insert into dbo.tblSwitchboard (SwitchboardID, ItemNumber, ItemText, Command, Argument, OpenArgs)
values (25, 1, 'Lead Action Summary', 4, 'ldrptLeadActionSummary', 'FCPurchased')
go

update dbo.tblSwitchboard set ItemNumber = ItemNumber + 1 where SwitchboardID = 29 and ItemNumber > 0
go

insert into dbo.tblSwitchboard (SwitchboardID, ItemNumber, ItemText, Command, Argument, OpenArgs)
values (29, 1, 'Lead Action Summary', 4, 'ldrptLeadActionSummary', 'TomCatMode')
go

insert into dbo.tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('4.09','2000-11-30','- FC/TC Orders:  Added ''Orders vs. Warranty'' report
- Fixed Release Notes not always printing (I think)
- Dealers:  Added ''Add/edit'' sales rep names menu item
- Added ''Secondary Admin Password'' menu item on the Switchboard, under Tools.  Only Irene can access this menu item, and change the secondary admin password.
- FC/TC Orders:  Allowed secondary admin password to open Margin by Model and Sales by Model
- FC/TC Dealers:  Fixed/added Advertising Allocations modifcation section
- FC/TC Dealers:  Updated/Added Advertising Allocation report, also cleaned up all other Dealer reports
- TC Dealers:  Fixed report ''Mailing labels for specific titles''
- Orders:  All 40HD and 52HD orders are now 40 and 52, respectively
- Parts:  Added ''Option Parts by Model'' (removed old version)
- FC/TC/Pur Leads:  Fixed Leads Program Results by Dealer report
- FC/TC Orders:  Updated following reports to reflect negative orders:  Acknowledgement, Orders vs. Warranty, Dealer Sales, Dealer Serial #, Major Account Sales, 
Margin by Model, Open Orders, Sales by Model, Sales by Dealer, Sales Rep Commission
- FC/TC/Pur Leads:  Added ''Lead Action Summary'' report
')
go

update dbo.tblDBProperties
set PropertyValue = '4.09'
where PropertyName = 'DBStructVersion'
go

