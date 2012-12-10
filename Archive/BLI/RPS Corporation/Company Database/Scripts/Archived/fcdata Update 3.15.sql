-- fcdata Update 3.15
-- - make history user name 20 chars instead of 10
-- - add spSecurityCheck
-- - add tblUser
-- - add tblVersion
-- - update tblOrders with tom cat orders field
-- - add orders views
-- - add sp to get NT user list
-- - add menu option for fixed purchased lead import

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE dbo.Tmp_tblMenuHistory
	(
 	HistoryID int NOT NULL IDENTITY (1, 1),
	HistoryTime datetime NULL,
	HistoryUser char(20) NULL,
	HistorySwitchID int NULL,
	HistorySwitchItem int NULL
	) ON [PRIMARY]
GO

SET IDENTITY_INSERT dbo.Tmp_tblMenuHistory ON
GO

IF EXISTS(SELECT * FROM dbo.tblMenuHistory)
	 EXEC('INSERT INTO dbo.Tmp_tblMenuHistory(HistoryID, HistoryTime, HistoryUser, HistorySwitchID, HistorySwitchItem)
		SELECT HistoryID, HistoryTime, HistoryUser, HistorySwitchID, HistorySwitchItem FROM dbo.tblMenuHistory TABLOCKX')
GO

SET IDENTITY_INSERT dbo.Tmp_tblMenuHistory OFF
GO

DROP TABLE dbo.tblMenuHistory
GO

EXECUTE sp_rename 'dbo.Tmp_tblMenuHistory', 'tblMenuHistory'
GO

ALTER TABLE dbo.tblMenuHistory ADD CONSTRAINT
	PK_tblMenuHistory PRIMARY KEY NONCLUSTERED 
	(
	HistoryID
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[spSecurityCheck]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSecurityCheck]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE spSecurityCheck 
	@sUserID nvarchar(50),
	@iSwitchboardID int,
	@iItemNumber int
AS

SELECT AccessType 
FROM dbo.tblSecurity
WHERE UserID = @sUserID AND SwID IN (
	SELECT [ID]
	FROM dbo.[Switchboard Items]
	WHERE SwitchboardID = @iSwitchboardID AND ItemNumber = @iItemNumber )

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[spSecurityCheck]  TO [fcuser]
GO

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
CREATE TABLE dbo.tblUser
	(
 	UserID int NOT NULL IDENTITY (1, 1),
	UserName nvarchar(50) NOT NULL
	) ON [PRIMARY]
GO
ALTER TABLE dbo.tblUser ADD CONSTRAINT
	PK_tblUser PRIMARY KEY NONCLUSTERED 
	(
	UserID
	) ON [PRIMARY]
GO
COMMIT



if exists (select * from sysobjects where id = object_id(N'[dbo].[tblVersion]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblVersion]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[spGetNTUserList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spGetNTUserList]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE spGetNTUserList

AS

declare @object int
declare @hr int
declare @property varchar(2500)
declare @return varchar(255)


-- CREATE AN OBJECT
EXEC @hr = sp_OACreate 'axNTSec.ntsec', @object OUT
IF @hr <> 0
  BEGIN
	EXEC sp_displayerrorinfo @object, @hr
	RETURN
  END

-- GET A PROPERTY BY CALLING METHOD
EXEC @hr = sp_OAMethod @object, 'GetUserStr', @property OUT
IF @hr <> 0
  BEGIN
	EXEC sp_displayerrorinfo @object, @hr
	RETURN
  END

SELECT @property AS uList


-- DESTROY OBJECT
EXEC @hr = sp_OADestroy @object
IF @hr <> 0
  BEGIN
	EXEC sp_displayerrorinfo @object, @hr
	RETURN
  END

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[spGetNTUserList]  TO [fcuser]
GO





BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblOrders
	DROP CONSTRAINT DF_tblOrders_MajorAccountID
GO
CREATE TABLE dbo.Tmp_tblOrders
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
	TomCatOrder bit NOT NULL CONSTRAINT DF_tblOrders_TomCatOrder DEFAULT (0)
	) ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblOrders ON
GO
IF EXISTS(SELECT * FROM dbo.tblOrders)
	 EXEC('INSERT INTO dbo.Tmp_tblOrders(OrderID, Model, OrderDate, OrderNumber, OrderKey, Dealer, PurchaseOrder, Quantity, PromisedDate, ShippedDate, Battery, Size, AmpCharger, HourMeter, SerialNumber, TwelveVMotor, Eighteen1hpMotor, TwoHP, SalePrice, CostPrice, Margin, Terms, ShipVia, CollectPrepaid, Notes, Plus2Batteries, FortyAmp, Horn, Alarm, Name, SaleDate, Address, City, State, Zip, ContactName, Phone, EighteenMonthOption, DealerDemo, StandardWarranty, LastDateMailedInfoTo, Note, ShipName, StreetAddress, CityStateZip, TagForEndUserReport, TypeOfBusiness, PartialList, LastUsedDate, ContactedDate, ContactedBy, Options, SICCode, TermsInfo, fkDealerID, MajorAccount, MajorAccountID, fkServDealerID)
		SELECT OrderID, Model, OrderDate, OrderNumber, OrderKey, Dealer, PurchaseOrder, Quantity, PromisedDate, ShippedDate, Battery, Size, AmpCharger, HourMeter, SerialNumber, TwelveVMotor, Eighteen1hpMotor, TwoHP, SalePrice, CostPrice, Margin, Terms, ShipVia, CollectPrepaid, Notes, Plus2Batteries, FortyAmp, Horn, Alarm, Name, SaleDate, Address, City, State, Zip, ContactName, Phone, EighteenMonthOption, DealerDemo, StandardWarranty, LastDateMailedInfoTo, Note, ShipName, StreetAddress, CityStateZip, TagForEndUserReport, TypeOfBusiness, PartialList, LastUsedDate, ContactedDate, ContactedBy, Options, SICCode, TermsInfo, fkDealerID, MajorAccount, MajorAccountID, fkServDealerID FROM dbo.tblOrders TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblOrders OFF
GO
DROP TABLE dbo.tblOrders
GO
EXECUTE sp_rename 'dbo.Tmp_tblOrders', 'tblOrders'
GO
ALTER TABLE dbo.tblOrders ADD CONSTRAINT
	PK_tblOrders PRIMARY KEY NONCLUSTERED 
	(
	OrderID
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblOrders_OrderNumber ON dbo.tblOrders
	(
	OrderNumber
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblOrders_OrderDate ON dbo.tblOrders
	(
	OrderDate
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
COMMIT

EXECUTE sp_rename 'dbo.tblOrders', 'tblAllOrders'
GO


CREATE VIEW dbo.tblOrders
AS
SELECT tblAllOrders.*
FROM tblAllOrders
WHERE (TomCatOrder = 0)
GO

CREATE VIEW dbo.tblTCOrders
AS
SELECT tblAllOrders.*
FROM tblAllOrders
WHERE (TomCatOrder = 1)
GO


insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (24, 3, 'Import Purchased Leads file',3,'ldpfrmImportLeads')
go


CREATE TABLE [dbo].[tblVersion] (
	[VersionID] [int] IDENTITY (1, 1) NOT NULL ,
	[VersionNumber] [nvarchar] (50) NULL ,
	[VersionDate] [smalldatetime] NULL ,
	[VersionRelNotes] [ntext] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblVersion] WITH NOCHECK ADD 
	CONSTRAINT [PK_tblVersion] PRIMARY KEY  NONCLUSTERED 
	(
		[VersionID]
	)  ON [PRIMARY] 
GO

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.00','1999-08-25 00:00:00','- Created new single database with all Factory Cat databases.  Uses Access 2000 and SQL server')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.01','1999-08-26 00:00:00','- Minor changes')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.02','1999-08-27 00:00:00','- More misc changes.')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.03','1999-09-01 00:00:00','- Fixed ''Dealer Sales'' report in Orders
- Fixed ''Open POs'' report in Prod Parts.
- Fixed ''Cost of Parts'' report in Prod Parts.
- Fixed the sorting order in Leads, Purchased Leads, and Warranty
- Fixed the Manf Part Number report in Parts
- Added feature to update the database structure on a database update.')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.04','1999-09-04 00:00:00','- Changed size of ''Contact'' and ''Company'' in Leads and Pur Leads to 100 characters (was 50)
- Fixed ''Nag Sheet'' adding items.
- Added this form, Release Notes.
- Fixed tab order in Orders to go Sale Price -> Cost Price -> Terms instead of Sale Price -> Terms.
- Added ''Production Releases'' report to Production Parts Reports menu.
- Fixed ''Warranty Total Costs'' report.
- Fixed ''Warranty Dealer Reimbursement'' report.
- Changed ''Warranty Total Costs'' report to allow selecting both a date and a part (would cause error before)')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.05','1999-09-08 00:00:00','- Fixed the ''Parts: Parts Listing by Model with Quantity Required'' report.')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.06','1999-09-15 00:00:00','- Fixed all date fields filtering - they would not let you filter by entering greater then or less then symbols.
- Anywhere you choose ''Print'' from the menu bar now asks to save the record before printing - otherwise it won''t show any changes since you last saved the record when printed out.')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.07','1999-09-20 00:00:00','- Fixed the ''Dealers\Mailling Labels for specific titles'' report.')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.08','1999-09-24 00:00:00','- Fixed the ''Leads Program Results'' reports.
- Changed the ''Direct Mail Leads Report'' to allow you to enter part of a Company Name as criteria.')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.09','1999-09-28 00:00:00','- Fixed ''Leads Program Results'' showing all records, and added a count of leads to the reports')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.10','1999-10-01 00:00:00','- Fixed ''Leads\Direct Mail Leads Labels'' report in both Leads and Purchased leads (it was ''broken'' when I changed the ''Direct Mail Leads'' report to allow company name)')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.11','1999-10-05 00:00:00','- For some strange reason changing any field on the Dealer form was causing Access to crash.  Replaced the form with a copy and the problem went away.')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.12','1999-10-09 00:00:00','- As a result of the crashing below the ''Print'' menu on the toolbar was duplicated many times.
- Added a better error message information popup.')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.13','1999-10-11 00:00:00','- Fixed purchased leads reports showing leads reports information.')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.14','1999-10-20 00:00:00','- Added Tom Cat areas - Tom Cat Prospects and Tom Cat Leads.
- Updated several leads reports to fix misc. problems.')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.15','1999-11-15 00:00:00','- Added security to the database.
- Added a security administration tool to the database that only Irene or BLI can run.
- Added this form of the release notes.
- Fixed ''Unregistered Sweepers'' report in Orders
- Added ''Purchased Leads Import'' to import the leads files from email.
- Started adding changes for Tom Cat Orders database to be added (in the next version)')
go

update tblDBProperties
set PropertyValue = '3.15'
where PropertyName = 'DBStructVersion'
go
