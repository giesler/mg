-- fcdata update 3.47

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
COMMIT
BEGIN TRANSACTION
CREATE TABLE dbo.tblPartsOptions
	(
	PartOptionID int NOT NULL IDENTITY (1, 1),
	ParentPartID int NOT NULL,
	ChildPartID int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.tblPartsOptions ADD CONSTRAINT
	PK_tblPartsOptions PRIMARY KEY CLUSTERED 
	(
	PartOptionID
	) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX IX_ParentPartID ON dbo.tblPartsOptions
	(
	ParentPartID
	) ON [PRIMARY]
GO
COMMIT

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[paspfrmPartsOptions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[paspfrmPartsOptions]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE paspfrmPartsOptions
	@iPartID int = 0
AS
select po.PartOptionID, po.ChildPartID, pa.RPSPartNum, pa.PartName
from dbo.tblPartsOptions po, dbo.tblParts pa
where po.ParentPartID = @iPartID and po.ChildPartID = pa.PartID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[paspfrmPartsOptions]  TO [fcuser]
GO

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblParts ADD
	OptionSubPart bit NOT NULL CONSTRAINT DF_tblParts_OptionSubPart DEFAULT 0
GO
COMMIT

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ppsprptPartsOnHold]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ppsprptPartsOnHold]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ppsprptProdReleases]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ppsprptProdReleases]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE ppsprptPartsOnHold

AS

SELECT po.POID, po.Vendor, pp.RPSPartNum, pp.PartDescription, 
	pd.RequestedShipDate, pd.Quantity, pd.Value, pd.ReceivedDate, 
	pd.CostEach, pd.Notes, pa.RPSPNSort
FROM dbo.tblPO po, dbo.tblPOPart pp, dbo.tblPOPartDetail pd, dbo.tblParts pa
WHERE po.POID = pp.fkPOID and pp.POPartID = pd.fkPOPartID
	and pa.RPSPartNum = pp.RPSPartNum
	and pd.Quantity = 0
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[ppsprptPartsOnHold]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE ppsprptProdReleases
	@sToDate varchar(20)
AS
IF @sToDate is null
	select @sToDate = '1/1/2020'
SELECT po.POID, po.Vendor, pp.VendorPartNumber, pp.RPSPartNum, pp.PartDescription, 
	pd.RequestedShipDate, pd.Quantity, pd.Value, pd.ReceivedDate, 
	pd.CostEach, pd.Quantity AS ExtCost, pd.Notes, pa.RPSPNSort, 
	DATEPART(yyyy, pd.RequestedShipDate) AS ShipYear, 
	DATEPART(wk, pd.RequestedShipDate) AS ShipWeek
FROM dbo.tblPO po, dbo.tblPOPart pp, dbo.tblPOPartDetail pd, dbo.tblParts pa
WHERE po.POID = pp.fkPOID and pp.POPartID = pd.fkPOPartID and
	pd.ReceivedDate IS NULL and pd.RequestedShipDate < @sToDate
	and pa.RPSPartNum = pp.RPSPartNum
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[ppsprptProdReleases]  TO [fcuser]
GO


update dbo.[Switchboard Items] set ItemNumber = ItemNumber + 1
where SwitchboardID = 19 and ItemNumber > 4
go

insert dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (19, 5, 'Parts On Hold', 4, 'pprptPartsOnHold')
go

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
COMMIT
BEGIN TRANSACTION
CREATE TABLE dbo.tlkuDealerRep
	(
	DealerRepID int NOT NULL IDENTITY (1, 1),
	DealerRepName nvarchar(255) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.tlkuDealerRep ADD CONSTRAINT
	PK_tlkuDealerRep PRIMARY KEY CLUSTERED 
	(
	DealerRepID
	) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX IX_tlkuDealerRep_DealerRepName ON dbo.tlkuDealerRep
	(
	DealerRepName
	) ON [PRIMARY]
GO
COMMIT

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblAllDealers ADD
	DealerRepID int NULL
GO
COMMIT


insert dbo.tlkuDealerRep (DealerRepName) values ('Darin Park')
go
insert dbo.tlkuDealerRep (DealerRepName) values ('Tom Botzau')
go
insert dbo.tlkuDealerRep (DealerRepName) values ('Rick Haertel')
go
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
SELECT o.Model, o.Dealer, o.Quantity, o.SalePrice, o.OrderNumber, o.OrderType, r.DealerRepName
FROM tblAllOrders o, tblAllDealers d, tlkuDealerRep r
WHERE o.SalePrice <> 0
	AND (o.OrderDate Between @sFromDate and @sToDate)
	AND d.DealerRepID = r.DealerRepID
	AND o.Dealer = d.DealerName

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptSalesRepCommission]  TO [fcuser]
GO


update dbo.[Switchboard Items] set ItemNumber = ItemNumber + 1
where SwitchboardID = 14 and ItemNumber > 13
go

insert dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (14, 14, 'Sales Rep Commission', 4, 'orrptSalesRepCommission')
go

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spGetSwitchboardItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spGetSwitchboardItem]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spGetUserSwitchboardItems]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spGetUserSwitchboardItems]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE spGetSwitchboardItem
	@iSwitchboardID int = 0,
	@iItemNumber int = 0
AS
select ItemText, Command, Argument, OpenArgs
from dbo.[Switchboard Items]
where SwitchboardID = @iSwitchboardID and ItemNumber = @iItemNumber
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[spGetSwitchboardItem]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE dbo.spGetUserSwitchboardItems
	@sUser nvarchar(50),
	@iSwitch int
AS

select *
from dbo.[Switchboard Items]
where SwitchboardID = @iSwitch and ID not in ( select SwID from dbo.tblSecurity where UserID = @sUser and AccessType = 0)
	and ItemNumber <> 0
order by ItemNumber
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[spGetUserSwitchboardItems]  TO [fcuser]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spGetUserSwitchboardItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spGetUserSwitchboardItem]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE dbo.spGetUserSwitchboardItem
	@sUser nvarchar(50),
	@iSwitchID int
AS

select *
from dbo.[Switchboard Items]
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


insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.47','2000-5-28 00:00:00','- Warranty: Part cost is now pulled from ''Current Cost'' field in Parts.
- Parts: Added ''Options'' tab to allow setting and selecting option parts
- Orders: Fixed save prompt to not save if you say no (also in TC Orders)
- Prod Parts: Fixed part sorting in Prod Releases
- Prod Parts: Added ''Parts On Hold'' report
- Dealers: Added ''Dealer Rep Name'' to dealers screen page 2 (also in TC Dealers)
- Orders: Added ''Sales Rep Commission'' report that shows FC and TC order details
')
go

update tblDBProperties
set PropertyValue = '3.47'
where PropertyName = 'DBStructVersion'
go
