-- fcdata update 3.30
-- - update rel notes, db version
-- - change dealer table

insert [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (31, 2, 'Tom Cat Prospects by State', 4, 'tcrptProspectListState')
go

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblParts
	DROP CONSTRAINT DF__Temporary__RPSPu__693CA210
GO
ALTER TABLE dbo.tblParts
	DROP CONSTRAINT DF__TemporaryUp__Qty__6A30C649
GO
ALTER TABLE dbo.tblParts
	DROP CONSTRAINT DF__Temporary__Total__6B24EA82
GO
ALTER TABLE dbo.tblParts
	DROP CONSTRAINT DF__Temporary__SubPa__6C190EBB
GO
ALTER TABLE dbo.tblParts
	DROP CONSTRAINT DF__Temporary__SubFl__6D0D32F4
GO
ALTER TABLE dbo.tblParts
	DROP CONSTRAINT DF__Temporary__Shelf__6E01572D
GO
ALTER TABLE dbo.tblParts
	DROP CONSTRAINT DF__Temporary__Level__6EF57B66
GO
ALTER TABLE dbo.tblParts
	DROP CONSTRAINT DF__Temporary__HideO__6FE99F9F
GO
ALTER TABLE dbo.tblParts
	DROP CONSTRAINT DF__Temporary__Optio__70DDC3D8
GO
ALTER TABLE dbo.tblParts
	DROP CONSTRAINT DF__Temporary__RunQu__71D1E811
GO
CREATE TABLE dbo.Tmp_tblParts
	(
 	PartID int NOT NULL IDENTITY (1, 1),
	Model nvarchar(30) NULL,
	RPSPurchased bit NOT NULL CONSTRAINT DF__Temporary__RPSPu__693CA210 DEFAULT (0),
	BlanketRelease nvarchar(20) NULL,
	DateOrdered datetime NULL,
	LeadTime nvarchar(20) NULL,
	Qty smallint NULL CONSTRAINT DF__TemporaryUp__Qty__6A30C649 DEFAULT (0),
	PartName nvarchar(150) NULL,
	Location nvarchar(150) NULL,
	VendorName nvarchar(100) NULL,
	VendorPartName nvarchar(150) NULL,
	ManfPartNum nvarchar(50) NULL,
	RPSPartNum nvarchar(50) NULL,
	CostEach money NULL,
	Notes nvarchar(255) NULL,
	QuoteDate datetime NULL,
	QtyReq int NULL,
	TotalCostPerMachine money NULL,
	TotalCostPerUnitForPart money NULL CONSTRAINT DF__Temporary__Total__6B24EA82 DEFAULT (0),
	PageRef int NULL,
	VendorCost money NULL,
	AutoCalcPrice nvarchar(10) NULL,
	SubPartTotal money NULL CONSTRAINT DF__Temporary__SubPa__6C190EBB DEFAULT (0),
	DealerNet money NULL,
	SuggestedList money NULL,
	Note nvarchar(255) NULL,
	CanadianDealerNet money NULL,
	Quantity nvarchar(50) NULL,
	Total float(53) NULL,
	CanadianSuggestedList money NULL,
	GSAList money NULL,
	FrenchPartDescription nvarchar(150) NULL,
	SubFlag bit NOT NULL CONSTRAINT DF__Temporary__SubFl__6D0D32F4 DEFAULT (0),
	ShelfNo float(53) NULL CONSTRAINT DF__Temporary__Shelf__6E01572D DEFAULT (0),
	Section nvarchar(10) NULL,
	PartLevel float(53) NULL CONSTRAINT DF__Temporary__Level__6EF57B66 DEFAULT (0),
	HideOnReports bit NOT NULL CONSTRAINT DF__Temporary__HideO__6FE99F9F DEFAULT (0),
	PartOption bit NOT NULL CONSTRAINT DF__Temporary__Optio__70DDC3D8 DEFAULT (0),
	EffectiveDate datetime NULL,
	SNEffective nvarchar(50) NULL,
	FinishDesc nvarchar(50) NULL,
	FinishVendor int NULL,
	RunQuantity int NULL CONSTRAINT DF__Temporary__RunQu__71D1E811 DEFAULT (0),
	LeadTimeDays nvarchar(50) NULL,
	PartCode nvarchar(1) NULL,
	DrawingNum nvarchar(50) NULL,
	RevisionNum nvarchar(50) NULL,
	RPSPNSort char(30) NULL
	) ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblParts ON
GO
IF EXISTS(SELECT * FROM dbo.tblParts)
	 EXEC('INSERT INTO dbo.Tmp_tblParts(PartID, Model, RPSPurchased, BlanketRelease, DateOrdered, LeadTime, Qty, PartName, Location, VendorName, VendorPartName, ManfPartNum, RPSPartNum, CostEach, Notes, QuoteDate, QtyReq, TotalCostPerMachine, TotalCostPerUnitForPart, PageRef, VendorCost, AutoCalcPrice, SubPartTotal, DealerNet, SuggestedList, Note, CanadianDealerNet, Quantity, Total, CanadianSuggestedList, GSAList, FrenchPartDescription, SubFlag, ShelfNo, Section, PartLevel, HideOnReports, PartOption, EffectiveDate, SNEffective, FinishDesc, RunQuantity, LeadTimeDays, PartCode, DrawingNum, RevisionNum, RPSPNSort)
		SELECT PartID, Model, RPSPurchased, BlanketRelease, DateOrdered, LeadTime, Qty, PartName, Location, VendorName, VendorPartName, ManfPartNum, RPSPartNum, CostEach, Notes, QuoteDate, QtyReq, TotalCostPerMachine, TotalCostPerUnitForPart, PageRef, VendorCost, AutoCalcPrice, SubPartTotal, DealerNet, SuggestedList, Note, CanadianDealerNet, Quantity, Total, CanadianSuggestedList, GSAList, FrenchPartDescription, SubFlag, ShelfNo, Section, PartLevel, HideOnReports, PartOption, EffectiveDate, SNEffective, FinishDesc, RunQuantity, LeadTimeDays, PartCode, DrawingNum, RevisionNum, RPSPNSort FROM dbo.tblParts TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblParts OFF
GO
DROP TABLE dbo.tblParts
GO
EXECUTE sp_rename 'dbo.Tmp_tblParts', 'tblParts'
GO
ALTER TABLE dbo.tblParts ADD CONSTRAINT
	aaaaatblParts_PK PRIMARY KEY NONCLUSTERED 
	(
	PartID
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX dateorder ON dbo.tblParts
	(
	DateOrdered
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX DrawingNum ON dbo.tblParts
	(
	DrawingNum
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX ManfPartNum ON dbo.tblParts
	(
	ManfPartNum
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX model ON dbo.tblParts
	(
	Model
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX PartCode ON dbo.tblParts
	(
	PartCode
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX partname ON dbo.tblParts
	(
	PartName
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX RevisionNum ON dbo.tblParts
	(
	RevisionNum
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX rpspn ON dbo.tblParts
	(
	RPSPartNum
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX Vendorname ON dbo.tblParts
	(
	VendorName
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblParts_RPSPNSort ON dbo.tblParts
	(
	RPSPNSort
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE TRIGGER tblParts_ITrig  ON dbo.tblParts 
FOR INSERT 
AS
SET NOCOUNT ON
IF (SELECT COUNT(*) FROM inserted WHERE RPSPartNum IS NOT NULL and charindex('-',RPSPartNum) > 0) > 0
  begin
	update a
	set a.RPSPNSort = substring(a.RPSPartNum, 1, charindex('-', a.RPSPartNum)-1) + replicate('0', 10 - charindex('-', a.RPSPartNum)-1)
		+ '-' + substring(a.RPSPartNum, charindex('-', a.RPSPartNum) + 1, 21) + replicate('0', 21 - len(substring(a.RPSPartNum, charindex('-', a.RPSPartNum) + 1, 21)))
	from dbo.tblParts a, inserted
	where inserted.RPSPartNum is not null and charindex('-',inserted.RPSPartNum) > 0 and inserted.RPSPartNum = a.RPSPartNum
  end
IF (SELECT COUNT(*) FROM inserted WHERE RPSPartNum IS NOT NULL and charindex('-',RPSPartNum)= 0) > 0
  begin
	update a
	set a.RPSPNSort = a.RPSPartNum + replicate('0',30 - len(a.RPSPartNum))
	from dbo.tblParts a, inserted
	where a.RPSPartNum is not null and charindex('-',a.RPSPartNum) = 0 and inserted.RPSPartNum = a.RPSPartNum
  end
GO
CREATE TRIGGER tblParts_UTring ON dbo.tblParts
FOR UPDATE
AS
SET NOCOUNT ON
IF (SELECT COUNT(*) FROM inserted WHERE RPSPartNum IS NOT NULL and charindex('-',RPSPartNum) > 0) > 0
  begin
	update a
	set a.RPSPNSort = substring(a.RPSPartNum, 1, charindex('-', a.RPSPartNum)-1) + replicate('0', 10 - charindex('-', a.RPSPartNum)-1)
		+ '-' + substring(a.RPSPartNum, charindex('-', a.RPSPartNum) + 1, 21) + replicate('0', 21 - len(substring(a.RPSPartNum, charindex('-', a.RPSPartNum) + 1, 21)))
	from dbo.tblParts a, inserted
	where inserted.RPSPartNum is not null and charindex('-',inserted.RPSPartNum) > 0 and inserted.RPSPartNum = a.RPSPartNum
  end
IF (SELECT COUNT(*) FROM inserted WHERE RPSPartNum IS NOT NULL and charindex('-',RPSPartNum)= 0) > 0
  begin
	update a
	set a.RPSPNSort = a.RPSPartNum + replicate('0',30 - len(a.RPSPartNum))
	from dbo.tblParts a, inserted
	where a.RPSPartNum is not null and charindex('-',a.RPSPartNum) = 0 and inserted.RPSPartNum = a.RPSPartNum
  end
GO
COMMIT


BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
CREATE TABLE dbo.Tmp_tblProspects
	(
 	ProspectID int NOT NULL IDENTITY (1, 1),
	CompanyName nvarchar(100) NULL,
	FirstName nvarchar(50) NULL,
	LastName nvarchar(50) NULL,
	StreetAddress nvarchar(100) NULL,
	StreetZipCode nvarchar(10) NULL,
	POBox nvarchar(20) NULL,
	POBoxZipCode nvarchar(10) NULL,
	City nvarchar(50) NULL,
	State char(20) NULL,
	Phone nvarchar(30) NULL,
	Fax nvarchar(30) NULL,
	Territory nvarchar(255) NULL,
	EquipmentCarried ntext NULL,
	Notes ntext NULL,
	RecordUpdateDate smalldatetime NULL,
	Active bit NOT NULL CONSTRAINT DF_tblProspects_Active DEFAULT (1)
	) ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblProspects ON
GO
IF EXISTS(SELECT * FROM dbo.tblProspects)
	 EXEC('INSERT INTO dbo.Tmp_tblProspects(ProspectID, CompanyName, FirstName, LastName, StreetAddress, StreetZipCode, POBox, POBoxZipCode, City, State, Phone, Fax, Territory, EquipmentCarried, Notes, RecordUpdateDate)
		SELECT ProspectID, CompanyName, FirstName, LastName, StreetAddress, StreetZipCode, POBox, POBoxZipCode, City, State, Phone, Fax, Territory, EquipmentCarried, Notes, RecordUpdateDate FROM dbo.tblProspects TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblProspects OFF
GO
DROP TABLE dbo.tblProspects
GO
EXECUTE sp_rename 'dbo.Tmp_tblProspects', 'tblProspects'
GO
ALTER TABLE dbo.tblProspects ADD CONSTRAINT
	PK_tblProspects PRIMARY KEY NONCLUSTERED 
	(
	ProspectID
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblProspects_CompanyName ON dbo.tblProspects
	(
	CompanyName
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblProspects_State ON dbo.tblProspects
	(
	State
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
ALTER TABLE dbo.tblProspects
	DROP CONSTRAINT DF_tblProspects_Active
GO
CREATE TABLE dbo.Tmp_tblProspects
	(
 	ProspectID int NOT NULL IDENTITY (1, 1),
	CompanyName nvarchar(100) NULL,
	FirstName nvarchar(50) NULL,
	LastName nvarchar(50) NULL,
	StreetAddress nvarchar(100) NULL,
	StreetZipCode nvarchar(10) NULL,
	POBox nvarchar(20) NULL,
	POBoxZipCode nvarchar(10) NULL,
	City nvarchar(50) NULL,
	State char(20) NULL,
	Phone nvarchar(30) NULL,
	Fax nvarchar(30) NULL,
	Principle nvarchar(50) NULL,
	SalesMgr nvarchar(50) NULL,
	Other nvarchar(50) NULL,
	QualityLevel nvarchar(10) NULL,
	InterestLevel nvarchar(50) NULL,
	ScrubbersSold int NULL,
	Territory nvarchar(255) NULL,
	EquipmentCarried ntext NULL,
	Notes ntext NULL,
	RecordUpdateDate smalldatetime NULL,
	Active bit NOT NULL CONSTRAINT DF_tblProspects_Active DEFAULT (1)
	) ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblProspects ON
GO
IF EXISTS(SELECT * FROM dbo.tblProspects)
	 EXEC('INSERT INTO dbo.Tmp_tblProspects(ProspectID, CompanyName, FirstName, LastName, StreetAddress, StreetZipCode, POBox, POBoxZipCode, City, State, Phone, Fax, Territory, EquipmentCarried, Notes, RecordUpdateDate, Active)
		SELECT ProspectID, CompanyName, FirstName, LastName, StreetAddress, StreetZipCode, POBox, POBoxZipCode, City, State, Phone, Fax, Territory, EquipmentCarried, Notes, RecordUpdateDate, Active FROM dbo.tblProspects TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblProspects OFF
GO
DROP TABLE dbo.tblProspects
GO
EXECUTE sp_rename 'dbo.Tmp_tblProspects', 'tblProspects'
GO
ALTER TABLE dbo.tblProspects ADD CONSTRAINT
	PK_tblProspects PRIMARY KEY NONCLUSTERED 
	(
	ProspectID
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblProspects_CompanyName ON dbo.tblProspects
	(
	CompanyName
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblProspects_State ON dbo.tblProspects
	(
	State
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
CREATE TABLE dbo.tblProspectContacts
	(
 	ProspectContactID int NOT NULL IDENTITY (1, 1),
	ProspectID int NOT NULL,
	ContactDate smalldatetime NOT NULL CONSTRAINT DF_tblProspectContacts_ContactDate DEFAULT (GETDATE()),
	ContactSubject nvarchar(255) NULL,
	ContactInitials nvarchar(50) NULL
	) ON [PRIMARY]
GO
ALTER TABLE dbo.tblProspectContacts ADD CONSTRAINT
	PK_tblProspectContacts PRIMARY KEY NONCLUSTERED 
	(
	ProspectContactID
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblProspectContacts_ProspectID ON dbo.tblProspectContacts
	(
	ProspectID
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblProspectContacts_ContactDate ON dbo.tblProspectContacts
	(
	ContactDate
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
ALTER TABLE dbo.tblProspects ADD
	ActiveBy nvarchar(100) NULL,
	ActiveDate smalldatetime NULL
GO
COMMIT

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
EXECUTE sp_rename 'dbo.tblParts.FinishVendor', 'Tmp_FinishVendorID', 'COLUMN'
GO
EXECUTE sp_rename 'dbo.tblParts.Tmp_FinishVendorID', 'FinishVendorID', 'COLUMN'
GO
COMMIT

if exists (select * from sysobjects where id = object_id(N'[dbo].[pasprptPartFinishVendorSched]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[pasprptPartFinishVendorSched]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE pasprptPartFinishVendorSched
	@sVendorName nvarchar(100) = null,
	@sFromDate nvarchar(50) = null,
	@sToDate nvarchar(50) = null
AS

if @sVendorName is null
	SELECT @sVendorName = '%'
if @sFromDate is null
	SELECT @sFromDate = '1/1/1900'
if @sToDAte is null
	SELECT @sToDate = '1/1/2233'

SELECT ve.VendorName, pd.RequestedShipDate, pa.RPSPartNum, pa.FinishDesc, pd.Quantity, pa.PartName
FROM dbo.tblParts pa, dbo.tblPOPartDetail pd, dbo.tblPO po, dbo.tblPOPart pp, dbo.tblVendors ve
where pp.POPartID = pd.fkPOPartID and pp.fkPOID = po.POID and ve.VendorID = pa.FinishVendorID 
	and pp.RPSPartNum = pa.RPSPartNum and po.Vendor = ve.VendorName
	and pd.ReceivedDate is null
	and ve.VendorName like @sVendorName
	and pd.RequestedShipDate between @sFromDate and @sToDate


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[pasprptPartFinishVendorSched]  TO [fcuser]
GO



update [Switchboard Items] 
set ItemNumber = ItemNumber + 1
where SwitchboardID = 17 and ItemNumber > 8
go

insert [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (17, 9, 'Parts Finish Vendor Schedule', 4, 'parptPartFinishVendorSched')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.30','2000-2-12 00:00:00','- Added Prospect List by State report
- Updated Tom Cat Prospects screen to include contact history and a bunch of other fields.
- Updated Tom Cat Prospects form printout to include new fields
- Added Finish Vendor to Parts
- Added Parts Finish Vendor Schedule to Parts reports
')
go

update tblDBProperties
set PropertyValue = '3.30'
where PropertyName = 'DBStructVersion'
go
