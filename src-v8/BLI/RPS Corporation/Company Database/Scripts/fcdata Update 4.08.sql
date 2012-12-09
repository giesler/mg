-- fcdata update 4.08

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument, OpenArgs)
values (32, 4, 'Add/edit/view model listing', '3', 'orfrmModelEdit', 'TomCatMode')
go

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[wasprptWarrantyPending]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprptWarrantyPending]
GO

SET QUOTED_IDENTIFIER OFF 
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

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[orsprptDealerOrders]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptDealerOrders]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE dbo.orsprptDealerOrders
	@sDealerName varchar(40) = null,
	@bNullShipDate varchar(20) = 0,
	@iOrderType varchar(3) = 0
AS
-- Run query
SELECT dl.DealerName, ao.Model, ao.OrderDate, ao.PromisedDate, ao.PurchaseOrder
FROM tblAllOrders ao 
	INNER JOIN tblAllDealers dl ON ao.fkDealerID = dl.DealerID
WHERE (dl.CurrentDealer = 1) AND ao.OrderType = @iOrderType
	AND CASE
			WHEN @sDealerName IS NULL THEN 1
			WHEN dl.DealerName = @sDealerName THEN 1
			ELSE 0 END = 1
	AND CASE
			WHEN @bNullShipDate = 0 THEN 1
			WHEN ao.ShippedDate IS NULL THEN 1 ELSE 0
		END = 1
ORDER BY dl.DealerName, ao.PurchaseOrder
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptDealerOrders]  TO [fcuser]
GO


BEGIN TRANSACTION
ALTER TABLE dbo.tblAllOrders
	DROP CONSTRAINT DF_tblOrders_MajorAccountID
GO
ALTER TABLE dbo.tblAllOrders
	DROP CONSTRAINT DF_tblOrders_TomCatOrder
GO
CREATE TABLE dbo.Tmp_tblAllOrders
	(
	OrderID int NOT NULL IDENTITY (1, 1),
	Model nvarchar(20) NULL,
	OrderDate datetime NULL,
	OrderNumber nvarchar(50) NULL,
	OrderKey int NULL,
	Dealer nvarchar(50) NULL,
	PurchaseOrder nvarchar(50) NULL,
	Quantity decimal(5, 2) NULL,
	PromisedDate datetime NULL,
	ShippedDate datetime NULL,
	Battery nchar(10) NULL,
	[Size] nchar(10) NULL,
	AmpCharger nchar(10) NULL,
	HourMeter nchar(10) NULL,
	SerialNumber nchar(30) NULL,
	TwelveVMotor nchar(15) NULL,
	Eighteen1hpMotor nchar(10) NULL,
	TwoHP nchar(10) NULL,
	SalePrice money NULL,
	CostPrice money NULL,
	Margin money NULL,
	Terms nvarchar(50) NULL,
	ShipVia nvarchar(50) NULL,
	CollectPrepaid nchar(30) NULL,
	Notes ntext NULL,
	Plus2Batteries nchar(10) NULL,
	FortyAmp nchar(10) NULL,
	Horn nchar(10) NULL,
	Alarm nchar(10) NULL,
	Name nvarchar(100) NULL,
	SaleDate datetime NULL,
	Address nvarchar(100) NULL,
	City nvarchar(50) NULL,
	State nvarchar(20) NULL,
	Zip nvarchar(20) NULL,
	ContactName nvarchar(100) NULL,
	Phone nvarchar(35) NULL,
	EighteenMonthOption nchar(10) NULL,
	DealerDemo nchar(10) NULL,
	StandardWarranty nchar(10) NULL,
	LastDateMailedInfoTo datetime NULL,
	Note ntext NULL,
	ShipName nvarchar(100) NULL,
	StreetAddress nvarchar(100) NULL,
	CityStateZip nvarchar(100) NULL,
	TagForEndUserReport nchar(10) NULL,
	TypeOfBusiness nvarchar(50) NULL,
	PartialList nvarchar(50) NULL,
	LastUsedDate datetime NULL,
	ContactedDate datetime NULL,
	ContactedBy nvarchar(50) NULL,
	Options ntext NULL,
	SICCode float(53) NULL,
	TermsInfo nvarchar(50) NULL,
	fkDealerID int NULL,
	MajorAccount bit NULL,
	MajorAccountID int NULL,
	fkServDealerID int NULL,
	OrderType tinyint NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_tblAllOrders ADD CONSTRAINT
	DF_tblOrders_MajorAccountID DEFAULT (0) FOR MajorAccountID
GO
ALTER TABLE dbo.Tmp_tblAllOrders ADD CONSTRAINT
	DF_tblOrders_TomCatOrder DEFAULT (0) FOR OrderType
GO
SET IDENTITY_INSERT dbo.Tmp_tblAllOrders ON
GO
IF EXISTS(SELECT * FROM dbo.tblAllOrders)
	 EXEC('INSERT INTO dbo.Tmp_tblAllOrders (OrderID, Model, OrderDate, OrderNumber, OrderKey, Dealer, PurchaseOrder, Quantity, PromisedDate, ShippedDate, Battery, [Size], AmpCharger, HourMeter, SerialNumber, TwelveVMotor, Eighteen1hpMotor, TwoHP, SalePrice, CostPrice, Margin, Terms, ShipVia, CollectPrepaid, Notes, Plus2Batteries, FortyAmp, Horn, Alarm, Name, SaleDate, Address, City, State, Zip, ContactName, Phone, EighteenMonthOption, DealerDemo, StandardWarranty, LastDateMailedInfoTo, Note, ShipName, StreetAddress, CityStateZip, TagForEndUserReport, TypeOfBusiness, PartialList, LastUsedDate, ContactedDate, ContactedBy, Options, SICCode, TermsInfo, fkDealerID, MajorAccount, MajorAccountID, fkServDealerID, OrderType)
		SELECT OrderID, Model, OrderDate, OrderNumber, OrderKey, Dealer, PurchaseOrder, CONVERT(decimal(5, 0), Quantity), PromisedDate, ShippedDate, Battery, [Size], AmpCharger, HourMeter, SerialNumber, TwelveVMotor, Eighteen1hpMotor, TwoHP, SalePrice, CostPrice, Margin, Terms, ShipVia, CollectPrepaid, Notes, Plus2Batteries, FortyAmp, Horn, Alarm, Name, SaleDate, Address, City, State, Zip, ContactName, Phone, EighteenMonthOption, DealerDemo, StandardWarranty, LastDateMailedInfoTo, Note, ShipName, StreetAddress, CityStateZip, TagForEndUserReport, TypeOfBusiness, PartialList, LastUsedDate, ContactedDate, ContactedBy, Options, SICCode, TermsInfo, fkDealerID, MajorAccount, MajorAccountID, fkServDealerID, OrderType FROM dbo.tblAllOrders TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblAllOrders OFF
GO
DROP TABLE dbo.tblAllOrders
GO
EXECUTE sp_rename N'dbo.Tmp_tblAllOrders', N'tblAllOrders', 'OBJECT'
GO
CREATE CLUSTERED INDEX IX_tblAllOrders_OrderDate ON dbo.tblAllOrders
	(
	OrderDate
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
ALTER TABLE dbo.tblAllOrders ADD CONSTRAINT
	PK_tblOrders PRIMARY KEY NONCLUSTERED 
	(
	OrderID
	) WITH FILLFACTOR = 90 ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX IX_tblAllOrders_OrderNumber ON dbo.tblAllOrders
	(
	OrderNumber
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblAllOrders_OrderType ON dbo.tblAllOrders
	(
	OrderType
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
COMMIT

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ldsprptDealerLeads]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ldsprptDealerLeads]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ldpqrptDirectMailLead]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ldpqrptDirectMailLead]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ldqrptDirectMailLead]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ldqrptDirectMailLead]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ldqrptLeadsReports]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ldqrptLeadsReports]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ldqrptMaillingLabels]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ldqrptMaillingLabels]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ldqrptMaillingLabelsDate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ldqrptMaillingLabelsDate]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ldqrptResponseMethod]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ldqrptResponseMethod]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ldsprptResponseMethod]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ldsprptResponseMethod]
GO


update dbo.[Switchboard Items]
set Argument = 'ldrptDealerLeads', OpenArgs = 'FCPurchased'
where Argument = 'ldprptDealerLeads'
go

update dbo.[Switchboard Items]
set Argument = 'ldrptDealerLeads', OpenArgs = 'TomCatMode'
where Argument = 'ldtrptDealerLeads'
go

update dbo.[Switchboard Items]
set Argument = 'ldrptDealerLeadsCity', OpenArgs = 'FCPurchased'
where Argument = 'ldprptDealerLeadsCity'
go

update dbo.[Switchboard Items]
set Argument = 'ldrptDealerLeadsCity', OpenArgs = 'TomCatMode'
where Argument = 'ldtrptDealerLeadsCity'
go

update dbo.[Switchboard Items]
set Argument = 'ldrptDealerLeadsPhone', OpenArgs = 'FCPurchased'
where Argument = 'ldprptDealerLeadsPhone'
go

update dbo.[Switchboard Items]
set Argument = 'ldrptDealerLeadsPhone', OpenArgs = 'TomCatMode'
where Argument = 'ldtrptDealerLeadsPhone'
go

update dbo.[Switchboard Items]
set Argument = 'ldrptDirectMailLead', OpenArgs = 'FCPurchased'
where Argument = 'ldprptDirectMailLead'
go

update dbo.[Switchboard Items]
set Argument = 'ldrptDirectMailLead', OpenArgs = 'TomCatMode'
where Argument = 'ldtrptDirectMailLead'
go

update dbo.[Switchboard Items]
set Argument = 'ldrptDirectMailLeadLabels', OpenArgs = 'FCPurchased'
where Argument = 'ldprptDirectMailLeadLabels'
go

update dbo.[Switchboard Items]
set Argument = 'ldrptDirectMailLeadLabels', OpenArgs = 'TomCatMode'
where Argument = 'ldtrptDirectMailLeadLabels'
go

update dbo.[Switchboard Items]
set Argument = 'ldrptReponseMethod', OpenArgs = 'FCPurchased'
where Argument = 'ldprptReponseMethod'
go

update dbo.[Switchboard Items]
set Argument = 'ldrptReponseMethod', OpenArgs = 'TomCatMode'
where Argument = 'ldtrptResponseMethod'
go



delete from dbo.[Switchboard Items] where Argument in ('ldrptMaillingLabels', 'ldtrptMaillingLabels', 'ldprptMaillingLabels')
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
	SUBSTRING(Phone, 1, 3) AS AreaCode, Salesman, Address, City, State, Zip, ApplicationNotes, ContactTitle
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


update dbo.tblAllOrders
set fkDealerID = dl.DealerID
from tblAllOrders, tblAlldealers dl
where tblAllOrders.Dealer = dl.DealerName
	and (tblAllOrders.fkDealerID is null or tblAllOrders.fkDealerID = 0)
go


insert into dbo.tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('4.08','2000-11-22','- TC Warranty:  Fixed
- FC/TC Warranty Pending by Dealer:  Added ''Comments'' field, added criteria for open/closed/both, and made up to four parts print on each line for an RGA
- TC Orders:  Added model listing edit screen to TC Orders menu
- FC/TC Orders:  Fixed ''Dealer Orders'' report
- FC/TC Orders:  Changed Quantity field to allow non integer values (decimal and negative values)
- FC/TC/Pur Leads:  Updated all reports to show correct dealer names.  Also changed all reports to use a common criteria dialog - remember, entering info in any field is optional, so if you don''t want to limit by a field leave it blank
- FC/TC Orders:  Fixed Salesman Commission Report
')
go

update dbo.tblDBProperties
set PropertyValue = '4.08'
where PropertyName = 'DBStructVersion'
go

