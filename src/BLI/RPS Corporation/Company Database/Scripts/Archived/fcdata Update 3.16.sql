-- fcdata Update 3.16
-- - Add Tom Cat Orders switchboard items
-- - Redo Acknowledgement as an sp
-- - Drop several unneeded views and tables
-- - Change tblAllOrders TomCatOrder field to OrderType
-- - redo customer labels sp
-- - redo dealer sales
-- - redo dealer serial

if not exists(select * from [Switchboard Items] where SwitchboardID = 33 and ItemNumber = 0)
update [Switchboard Items]
set ItemNumber = ItemNumber + 1
where SwitchboardID = 1 and ItemNumber > 12 and ItemNumber < 20
go

if not exists(select * from [Switchboard Items] where SwitchboardID = 33 and ItemNumber = 0)
insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (1, 13, 'Tom Cat Orders', 1, '32')
go

if not exists(select * from [Switchboard Items] where SwitchboardID = 33 and ItemNumber = 0)
insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (32, 0, 'Tom Cat Orders', 0, '')
go

if not exists(select * from [Switchboard Items] where SwitchboardID = 33 and ItemNumber = 0)
insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (32, 1, 'Add/edit/view Tom Cat Orders', 3, 'ortfrmOrders')
go

if not exists(select * from [Switchboard Items] where SwitchboardID = 33 and ItemNumber = 0)
insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (32, 2, 'Tom Cat Order Reports', 1, '33')
go

if not exists(select * from [Switchboard Items] where SwitchboardID = 33 and ItemNumber = 0)
insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (32, 3, 'Tom Cat Orders End User Reports', 1, '34')
go

if not exists(select * from [Switchboard Items] where SwitchboardID = 33 and ItemNumber = 0)
insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (33, 0, 'Tom Cat Orders Reports', 0, '')
go

if not exists(select * from [Switchboard Items] where SwitchboardID = 33 and ItemNumber = 13)
insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (33, 1, 'Acknowledgement Report', 4, 'ortrptAcknowledgement')
go

if not exists(select * from [Switchboard Items] where SwitchboardID = 33 and ItemNumber = 13)
insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (33, 2, 'Customer Mailling Labels', 4, 'ortrptCustomerLabels')
go

if not exists(select * from [Switchboard Items] where SwitchboardID = 33 and ItemNumber = 13)
insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (33, 3, 'Dealer Sales List (no marginal prices)', 4, 'ortrptDealerSales')
go

if not exists(select * from [Switchboard Items] where SwitchboardID = 33 and ItemNumber = 13)
insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (33, 4, 'Dealer/Serial Number Report', 4, 'ortrptDealerSerial')
go

if not exists(select * from [Switchboard Items] where SwitchboardID = 33 and ItemNumber = 13)
insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (33, 5, 'Margin by Model Sales Report', 4, 'ortrptMarginByModel')
go

if not exists(select * from [Switchboard Items] where SwitchboardID = 33 and ItemNumber = 13)
insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (33, 6, 'Prep Sheet', 4, 'ortrptPrepSheet')
go

if not exists(select * from [Switchboard Items] where SwitchboardID = 33 and ItemNumber = 13)
insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (33, 7, 'Production Schedule', 4, 'ortrptProdSchedule')
go

if not exists(select * from [Switchboard Items] where SwitchboardID = 33 and ItemNumber = 13)
insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (33, 8, 'Open Orders', 4, 'ortrptOpenOrders')
go

if not exists(select * from [Switchboard Items] where SwitchboardID = 33 and ItemNumber = 13)
insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (33, 9, 'Orders/Sales Report by Model', 4, 'ortrptSalesModel')
go

if not exists(select * from [Switchboard Items] where SwitchboardID = 33 and ItemNumber = 13)
insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (33, 10, 'Sales by Dealer report', 4, 'ortrptSalesDealer')
go

if not exists(select * from [Switchboard Items] where SwitchboardID = 33 and ItemNumber = 13)
insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (33, 11, 'Sales by Model report', 4, 'ortrptSalesModel')
go

if not exists(select * from [Switchboard Items] where SwitchboardID = 33 and ItemNumber = 13)
insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (33, 12, 'Servicing Dealers', 4, 'ortrptServicingDealers')
go

if not exists(select * from [Switchboard Items] where SwitchboardID = 33 and ItemNumber = 13)
insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (33, 13, 'Unregistered Sweepers', 4, 'ortrptUnregSweepers')
go

if not exists(select * from [Switchboard Items] where SwitchboardID = 34 and ItemNumber = 0)
insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (34, 0, 'Tom Cat Orders End User Reports', 0, '')
go

if not exists(select * from [Switchboard Items] where SwitchboardID = 34 and ItemNumber = 4)
insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (34, 1, 'End User Report', 4, 'ortrptEndUser')
go

if not exists(select * from [Switchboard Items] where SwitchboardID = 34 and ItemNumber = 4)
insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (34, 2, 'End User Report by Dealer', 4, 'ortrptEndUserDealer')
go

if not exists(select * from [Switchboard Items] where SwitchboardID = 34 and ItemNumber = 4)
insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (34, 3, 'End User Mailling labels by Dealer', 4, 'ortrptEndUserLabels')
go

if not exists(select * from [Switchboard Items] where SwitchboardID = 34 and ItemNumber = 4)
insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (34, 4, 'End User Report by State', 4, 'ortrptEndUserState')
go


if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptAcknowledgement]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptAcknowledgement]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orqrptAcknowledgement]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[orqrptAcknowledgement]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO

-- drop unneeded tables
if exists (select * from sysobjects where id = object_id(N'[dbo].[patmprptBillOfMatPParts]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[patmprptBillOfMatPParts]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[pltblImportedData]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[pltblImportedData]
GO

-- update tblAllOrders - change 'Tom Cat Order' to 'OrderType', tinyint

-- first see if already run, if so rename column back
if exists(select sc.name from syscolumns sc, sysobjects so where (sc.id = so.id
	and so.id = object_id(N'dbo.tblAllOrders') and OBJECTPROPERTY(so.id, N'IsUserTable') = 1 and sc.name = 'OrderType'))
EXEC sp_rename 'tblAllOrders.OrderType','TomCatOrder','COLUMN'
go


BEGIN TRANSACTION

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
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
	Model smallint NULL,
	OrderDate datetime NULL,
	OrderNumber nvarchar(50) NULL,
	OrderKey int NULL,
	Dealer nvarchar(50) NULL,
	PurchaseOrder nvarchar(50) NULL,
	Quantity smallint NULL,
	PromisedDate datetime NULL,
	ShippedDate datetime NULL,
	Battery nchar(10) NULL,
	Size nchar(10) NULL,
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
	MajorAccountID int NULL CONSTRAINT DF_tblOrders_MajorAccountID DEFAULT (0),
	fkServDealerID int NULL,
	OrderType tinyint NOT NULL CONSTRAINT DF_tblOrders_TomCatOrder DEFAULT (0)
	) ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblAllOrders ON
GO
IF EXISTS(SELECT * FROM dbo.tblAllOrders)
	 EXEC('INSERT INTO dbo.Tmp_tblAllOrders(OrderID, Model, OrderDate, OrderNumber, OrderKey, Dealer, PurchaseOrder, Quantity, PromisedDate, ShippedDate, Battery, Size, AmpCharger, HourMeter, SerialNumber, TwelveVMotor, Eighteen1hpMotor, TwoHP, SalePrice, CostPrice, Margin, Terms, ShipVia, CollectPrepaid, Notes, Plus2Batteries, FortyAmp, Horn, Alarm, Name, SaleDate, Address, City, State, Zip, ContactName, Phone, EighteenMonthOption, DealerDemo, StandardWarranty, LastDateMailedInfoTo, Note, ShipName, StreetAddress, CityStateZip, TagForEndUserReport, TypeOfBusiness, PartialList, LastUsedDate, ContactedDate, ContactedBy, Options, SICCode, TermsInfo, fkDealerID, MajorAccount, MajorAccountID, fkServDealerID, OrderType)
		SELECT OrderID, Model, OrderDate, OrderNumber, OrderKey, Dealer, PurchaseOrder, Quantity, PromisedDate, ShippedDate, Battery, Size, AmpCharger, HourMeter, SerialNumber, TwelveVMotor, Eighteen1hpMotor, TwoHP, SalePrice, CostPrice, Margin, Terms, ShipVia, CollectPrepaid, Notes, Plus2Batteries, FortyAmp, Horn, Alarm, Name, SaleDate, Address, City, State, Zip, ContactName, Phone, EighteenMonthOption, DealerDemo, StandardWarranty, LastDateMailedInfoTo, Note, ShipName, StreetAddress, CityStateZip, TagForEndUserReport, TypeOfBusiness, PartialList, LastUsedDate, ContactedDate, ContactedBy, Options, SICCode, TermsInfo, fkDealerID, MajorAccount, MajorAccountID, fkServDealerID, CONVERT(tinyint, TomCatOrder) FROM dbo.tblAllOrders TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblAllOrders OFF
GO
DROP TABLE dbo.tblAllOrders
GO
EXECUTE sp_rename 'dbo.Tmp_tblAllOrders', 'tblAllOrders'
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
CREATE NONCLUSTERED INDEX IX_tblAllOrders_OrderDate ON dbo.tblAllOrders
	(
	OrderDate
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [tblallorders0] ON dbo.tblallorders 
	(orderdate, model, dealer, quantity, saleprice, costprice, margin, ordertype)
GO

COMMIT


if exists (select * from sysobjects where id = object_id(N'[dbo].[orqrptCustomerLabels]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[orqrptCustomerLabels]
GO


-- recreate views
if exists (select * from sysobjects where id = object_id(N'[dbo].[tblOrders]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[tblOrders]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblTCOrders]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[tblTCOrders]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE VIEW dbo.tblOrders
AS
SELECT tblAllOrders.*
FROM tblAllOrders
WHERE OrderType = 0


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE VIEW dbo.tblTCOrders
AS
SELECT tblAllOrders.*
FROM tblAllOrders
WHERE OrderType = 1


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orqrptPrepSheet]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orqrptPrepSheet]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orqrptEndUserState]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orqrptEndUserState]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptAcknowledgement]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptAcknowledgement]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptCustomerLabels]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptCustomerLabels]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptDealerSales]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptDealerSales]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptDealerSerial]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptDealerSerial]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptEndUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptEndUser]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptEndUserDealer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptEndUserDealer]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptEndUserState]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptEndUserState]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptMajorAccountSales]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptMajorAccountSales]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptMarginByModel]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptMarginByModel]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptOpenOrders]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptOpenOrders]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptPrepSheet]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptPrepSheet]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptProdSchedule]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptProdSchedule]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptSalesDealer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptSalesDealer]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptSalesModel]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptSalesModel]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptServicingDealers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptServicingDealers]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptUnregSweepers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptUnregSweepers]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO
CREATE PROCEDURE orsprptAcknowledgement
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
	ao.Quantity, ao.Model, md.Description, md.Price, ao.Options, ao.PromisedDate, ao.ShipVia, ao.OrderNumber, 
	ao.CollectPrepaid, ao.Terms
FROM dbo.tblAllOrders ao, dbo.tblModels md
WHERE ao.Model = md.Model AND OrderType = @iOrderType
	AND (ao.OrderDate Between @sFromDate And @sToDate)
	AND ao.OrderID like @iOrderID

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptAcknowledgement]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE orsprptCustomerLabels
	@iOrderType varchar(3) = 0
AS

IF @iOrderType IS NULL
	SELECT @iOrderType = 0

SELECT [Name] AS CName, Address, City, State, Zip, ContactName
FROM tblAllOrders
WHERE [Name] IS NOT NULL AND City IS NOT NULL AND OrderType = @iOrderType
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptCustomerLabels]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE orsprptDealerSales 
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
SELECT ao.Dealer, ao.Model, ao.OrderDate, ao.ShippedDate, ao.Quantity, ao.SalePrice, ao.OrderNumber, ao.SerialNumber
FROM tblAllOrders ao INNER JOIN tblDealers ON ao.Dealer = tblDealers.DealerName
WHERE (tblDealers.CurrentDealer = 1) AND SalePrice <> 0 AND ao.OrderType = @iOrderType
	AND (ao.Dealer like @sDealerName)
	AND (ao.OrderDate Between @sFromDate And @sToDate)


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptDealerSales]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE orsprptDealerSerial
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
SELECT ao.Dealer, ao.Model, ao.SerialNumber, ao.Name, ao.SaleDate, ao.Quantity, ao.SalePrice, ao.ShippedDate
FROM tblAllOrders ao
WHERE ao.OrderType = @iOrderType
	AND (ao.Dealer like @sDealerName)
	AND (ao.ShippedDate Between @sFromDate and @sToDate)


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptDealerSerial]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE orsprptEndUser
	@iOrderType varchar(3) = 0
AS

-- Check input parameters
IF @iOrderType IS NULL
	SELECT @iOrderType = 0

--- Run query
SELECT PartialList,[Name], ContactName, Address, City, State, Zip, SaleDate, TypeOfBusiness, Phone, SICCode 
FROM tblAllOrders 
WHERE PartialList='X'
	AND OrderType = @iOrderType
ORDER BY [Name];


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptEndUser]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE orsprptEndUserDealer
-- also used by orrptEndUserLabels
	@sDealer varchar(50) = null,
	@iOrderType varchar(3) = 0
AS

-- Check input parameters
IF @sDealer IS NULL
	SELECT @sDealer = '%'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0

--- Run query
SELECT Dealer, Model, [Name], SaleDate, Address, City, State, Zip, ContactName, Phone, [city] + ', ' + [state] + '  ' + [zip] AS AddressLine
FROM tblAllOrders 
WHERE ([Name] IS NOT NULL) 
	AND OrderType = @iOrderType
	AND Dealer like @sDealer
ORDER BY [Name], SaleDate DESC




GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptEndUserDealer]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE orsprptEndUserState
	@sState varchar(50) = null,
	@iOrderType varchar(3) = 0
AS

-- Check input parameters
IF @sState IS NULL
	SELECT @sState = '%'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0

--- Run query
SELECT Model, SaleDate, [Name], Address, City, State, Zip, ContactName, Phone
FROM tblAllOrders
WHERE (State like @sState)
	AND OrderType = @iOrderType
ORDER BY SaleDate DESC
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptEndUserState]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE orsprptMajorAccountSales
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
    ao.Dealer, ao.Model, ao.SalePrice, ao.OrderDate
FROM dbo.tblMajorAccounts ma INNER JOIN dbo.tblOrders ao ON 
    ma.MajorAccountID = ao.MajorAccountID
WHERE ao.OrderType = @iOrderType
	AND ao.OrderDate Between @sFromDate And @sToDate
	AND ma.MACompName like @sMAName
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptMajorAccountSales]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE orsprptMarginByModel
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
SELECT ShippedDate, Dealer, Model, OrderNumber, Quantity, SalePrice, CostPrice, Margin, SerialNumber 
FROM dbo.tblOrders 
WHERE (SalePrice <> 0) 
	AND (OrderType = @iOrderType)
	AND (ShippedDate Between @sFromDate and @sToDate)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptMarginByModel]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE orsprptOpenOrders
	@iOrderType varchar(3) = 0
AS

-- Check input parameters
IF @iOrderType IS NULL
	SELECT @iOrderType = 0

--- Run query
SELECT  ShippedDate, Model, Dealer, OrderNumber, Quantity, SalePrice, CostPrice, Margin
FROM dbo.tblAllOrders
WHERE (ShippedDate Is Null)
	AND (OrderType = @iOrderType)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptOpenOrders]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE orsprptPrepSheet 
	@sModel varchar(40) = null,
	@iOrderType varchar(3) = 0
AS

-- Check input parameters
IF @sModel IS NULL
	SELECT @sModel = '%'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0

-- Run query
SELECT Model, PromisedDate, OrderNumber, Dealer, Battery, AmpCharger, ShipVia, Notes, [Size], ShippedDate
FROM tblAllOrders
WHERE (Model<>0) 
	AND (ShippedDate Is Null)
	AND Model like @sModel
	AND OrderType = @iOrderType


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptPrepSheet]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE orsprptProdSchedule
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT  PromisedDate, ShipVia, Dealer, Quantity, OrderNumber, Model, Notes, ShippedDate
FROM tblAllOrders
WHERE (ShippedDate Is Null)
	AND OrderType = @iOrderType

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptProdSchedule]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE orsprptSalesDealer
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
SELECT Dealer, Model, OrderNumber, ShippedDate, Quantity, SalePrice, CostPrice, Margin, OrderDate 
FROM dbo.tblAllOrders 
WHERE (SalePrice <> 0) 
	AND (OrderType = @iOrderType)
	AND (OrderDate Between @sFromDate and @sToDate)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptSalesDealer]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
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
SELECT Model, Dealer, Sum(Quantity) AS Quantity, Sum(SalePrice) AS SalePrice, Sum(CostPrice) AS CostPrice, Sum(Margin) AS Margin
FROM tblAllOrders
WHERE SalePrice <> 0
	AND (OrderType = @iOrderType)
	AND (OrderDate Between @sFromDate and @sToDate)
GROUP BY Model, Dealer
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptSalesModel]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE orsprptServicingDealers
	@iOrderType varchar(3) = 0
AS

-- Check input parameters
IF @iOrderType IS NULL
	SELECT @iOrderType = 0

--- Run query
SELECT ao.Model, ao.SerialNumber, ao.[Name], ao.Phone, ao.City, ao.State, ao.SaleDate, ao.Dealer, dl.DealerName 
FROM dbo.tblAllOrders ao INNER JOIN dbo.tblDealers dl ON ao.fkServDealerID = dl.DealerID
WHERE OrderType = @iOrderType
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptServicingDealers]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE orsprptUnregSweepers
	@iOrderType varchar(3) = 0
AS

-- Check input parameters
IF @iOrderType IS NULL
	SELECT @iOrderType = 0

--- Run query
SELECT DISTINCT ao.Dealer, ao.SerialNumber, ao.PurchaseOrder, ao.OrderDate, ao.ShippedDate, ao.[Name], ao.Model 
FROM dbo.tblDealers dl RIGHT OUTER JOIN dbo.tblAllOrders ao ON dl.DealerName = ao.Dealer 
WHERE (dl.CurrentDealer = 1) 
	AND (ao.ShippedDate IS NOT NULL) 
	AND (ao.[Name] IS NULL)
	AND OrderType = @iOrderType

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptUnregSweepers]  TO [fcuser]
GO



insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.16','1999-11-18 00:00:00','- Fixed slight problem with security program
- Added ''Tom Cat Orders'' to database.
- Fixed several existing Orders reports, hopefully improving speed a bit')
go

update tblDBProperties
set PropertyValue = '3.16'
where PropertyName = 'DBStructVersion'
go
