-- fcdata update 3.25
-- - update rel notes, db version

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (35, 3, 'ISSA Mailling Labels', 4, 'isrptISSAList')
go


BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblModels
	DROP CONSTRAINT DF__Temporary__Model__5441852A
GO
ALTER TABLE dbo.tblModels
	DROP CONSTRAINT DF__Temporary__Price__5535A963
GO
CREATE TABLE dbo.Tmp_tblModels
	(
 	ModelID int NOT NULL IDENTITY (1, 1),
	Model nvarchar(20) NULL CONSTRAINT DF__Temporary__Model__5441852A DEFAULT (''),
	Description ntext NULL,
	Price money NULL CONSTRAINT DF__Temporary__Price__5535A963 DEFAULT (0),
	upsize_ts timestamp NULL
	) ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblModels ON
GO
IF EXISTS(SELECT * FROM dbo.tblModels)
	 EXEC('INSERT INTO dbo.Tmp_tblModels(ModelID, Model, Description, Price)
		SELECT ModelID, CONVERT(nvarchar(20), Model), Description, Price FROM dbo.tblModels TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblModels OFF
GO
DROP TABLE dbo.tblModels
GO
EXECUTE sp_rename 'dbo.Tmp_tblModels', 'tblModels'
GO
ALTER TABLE dbo.tblModels ADD CONSTRAINT
	aaaaatblModels_PK PRIMARY KEY NONCLUSTERED 
	(
	ModelID
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX ModelID ON dbo.tblModels
	(
	ModelID
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
COMMIT


BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
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
	Model nvarchar(20) NULL,
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
		SELECT OrderID, CONVERT(nvarchar(20), Model), OrderDate, OrderNumber, OrderKey, Dealer, PurchaseOrder, Quantity, PromisedDate, ShippedDate, Battery, Size, AmpCharger, HourMeter, SerialNumber, TwelveVMotor, Eighteen1hpMotor, TwoHP, SalePrice, CostPrice, Margin, Terms, ShipVia, CollectPrepaid, Notes, Plus2Batteries, FortyAmp, Horn, Alarm, Name, SaleDate, Address, City, State, Zip, ContactName, Phone, EighteenMonthOption, DealerDemo, StandardWarranty, LastDateMailedInfoTo, Note, ShipName, StreetAddress, CityStateZip, TagForEndUserReport, TypeOfBusiness, PartialList, LastUsedDate, ContactedDate, ContactedBy, Options, SICCode, TermsInfo, fkDealerID, MajorAccount, MajorAccountID, fkServDealerID, OrderType FROM dbo.tblAllOrders TABLOCKX')
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
CREATE NONCLUSTERED INDEX tblallorders0 ON dbo.tblAllOrders
	(
	OrderDate,
	Model,
	Dealer,
	Quantity,
	SalePrice,
	CostPrice,
	Margin,
	OrderType
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
COMMIT


BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblPartsModels
	DROP CONSTRAINT DF__Temporary__fkPar__76969D2E
GO
ALTER TABLE dbo.tblPartsModels
	DROP CONSTRAINT DF__Temporary__Model__778AC167
GO
ALTER TABLE dbo.tblPartsModels
	DROP CONSTRAINT DF__Temporary__Quant__787EE5A0
GO
ALTER TABLE dbo.tblPartsModels
	DROP CONSTRAINT DF__tblPartsM__Optio__0EF836A4
GO
CREATE TABLE dbo.Tmp_tblPartsModels
	(
 	PartModelID int NOT NULL IDENTITY (1, 1),
	fkPartID int NULL CONSTRAINT DF__Temporary__fkPar__76969D2E DEFAULT (0),
	Model nvarchar(20) NULL CONSTRAINT DF__Temporary__Model__778AC167 DEFAULT (''),
	Quantity float(53) NULL CONSTRAINT DF__Temporary__Quant__787EE5A0 DEFAULT (0),
	upsize_ts timestamp NULL,
	Optional bit NOT NULL CONSTRAINT DF__tblPartsM__Optio__0EF836A4 DEFAULT (0)
	) ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblPartsModels ON
GO
IF EXISTS(SELECT * FROM dbo.tblPartsModels)
	 EXEC('INSERT INTO dbo.Tmp_tblPartsModels(PartModelID, fkPartID, Model, Quantity, Optional)
		SELECT PartModelID, fkPartID, CONVERT(nvarchar(20), Model), Quantity, Optional FROM dbo.tblPartsModels TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblPartsModels OFF
GO
DROP TABLE dbo.tblPartsModels
GO
EXECUTE sp_rename 'dbo.Tmp_tblPartsModels', 'tblPartsModels'
GO
ALTER TABLE dbo.tblPartsModels ADD CONSTRAINT
	tblPartsModels_PK PRIMARY KEY NONCLUSTERED 
	(
	PartModelID
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX fkPartID ON dbo.tblPartsModels
	(
	fkPartID
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
COMMIT


BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblProdSchedItems
	DROP CONSTRAINT DF__Temporary__Sched__0E6E26BF
GO
ALTER TABLE dbo.tblProdSchedItems
	DROP CONSTRAINT DF__Temporary__Model__0F624AF8
GO
ALTER TABLE dbo.tblProdSchedItems
	DROP CONSTRAINT DF__Temporary__Quant__10566F31
GO
CREATE TABLE dbo.Tmp_tblProdSchedItems
	(
 	ScheduleID int NOT NULL CONSTRAINT DF__Temporary__Sched__0E6E26BF DEFAULT (0),
	Model nvarchar(20) NOT NULL CONSTRAINT DF__Temporary__Model__0F624AF8 DEFAULT (0),
	Quantity smallint NOT NULL CONSTRAINT DF__Temporary__Quant__10566F31 DEFAULT (0),
	SchedItemID int NOT NULL IDENTITY (1, 1),
	upsize_ts timestamp NULL
	) ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblProdSchedItems ON
GO
IF EXISTS(SELECT * FROM dbo.tblProdSchedItems)
	 EXEC('INSERT INTO dbo.Tmp_tblProdSchedItems(ScheduleID, Model, Quantity, SchedItemID)
		SELECT ScheduleID, CONVERT(nvarchar(20), Model), Quantity, SchedItemID FROM dbo.tblProdSchedItems TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblProdSchedItems OFF
GO
DROP TABLE dbo.tblProdSchedItems
GO
EXECUTE sp_rename 'dbo.Tmp_tblProdSchedItems', 'tblProdSchedItems'
GO
ALTER TABLE dbo.tblProdSchedItems ADD CONSTRAINT
	aaaaatblProdSchedItems_PK PRIMARY KEY NONCLUSTERED 
	(
	ScheduleID,
	Model,
	Quantity
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX SchedItemID ON dbo.tblProdSchedItems
	(
	SchedItemID
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
COMMIT

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblWarranty
	DROP CONSTRAINT DF_tblWarranty_DateEntered
GO
CREATE TABLE dbo.Tmp_tblWarranty
	(
 	WarrantyID int NOT NULL IDENTITY (1, 1),
	MachineSerialNumber nvarchar(50) NULL,
	DateOfFailure datetime NULL,
	CreditMemoNum nvarchar(50) NULL,
	CreditMemoAmt money NULL,
	Dealer nvarchar(50) NULL,
	Customer nvarchar(50) NULL,
	RGANum float(53) NULL,
	PartCost float(53) NULL,
	LaborCost float(53) NULL,
	Freight float(53) NULL,
	Problem ntext NULL,
	Model nvarchar(20) NULL,
	Resolution ntext NULL,
	WarrantyOpen bit NULL,
	Travel money NULL,
	Policy money NULL,
	DateEntered datetime NULL CONSTRAINT DF_tblWarranty_DateEntered DEFAULT (getdate()),
	PartReceived bit NULL,
	Hours real NULL,
	fkDealerID smallint NULL,
	Comment ntext NULL
	) ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblWarranty ON
GO
IF EXISTS(SELECT * FROM dbo.tblWarranty)
	 EXEC('INSERT INTO dbo.Tmp_tblWarranty(WarrantyID, MachineSerialNumber, DateOfFailure, CreditMemoNum, CreditMemoAmt, Dealer, Customer, RGANum, PartCost, LaborCost, Freight, Problem, Model, Resolution, WarrantyOpen, Travel, Policy, DateEntered, PartReceived, Hours, fkDealerID, Comment)
		SELECT WarrantyID, MachineSerialNumber, DateOfFailure, CreditMemoNum, CreditMemoAmt, Dealer, Customer, RGANum, PartCost, LaborCost, Freight, Problem, CONVERT(nvarchar(20), Model), Resolution, WarrantyOpen, Travel, Policy, DateEntered, PartReceived, Hours, fkDealerID, Comment FROM dbo.tblWarranty TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblWarranty OFF
GO
ALTER TABLE dbo.tblWarrantyParts
	DROP CONSTRAINT FK_tblWarrantyParts_tblWarranty
GO
DROP TABLE dbo.tblWarranty
GO
EXECUTE sp_rename 'dbo.Tmp_tblWarranty', 'tblWarranty'
GO
ALTER TABLE dbo.tblWarranty ADD CONSTRAINT
	PK_tblWarranty PRIMARY KEY NONCLUSTERED 
	(
	WarrantyID
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblWarranty_Dealer ON dbo.tblWarranty
	(
	Dealer
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblWarranty_RGANum ON dbo.tblWarranty
	(
	RGANum
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblWarrantyParts WITH NOCHECK ADD CONSTRAINT
	FK_tblWarrantyParts_tblWarranty FOREIGN KEY
	(
	fkWarrantyID
	) REFERENCES dbo.tblWarranty
	(
	WarrantyID
	)
GO
COMMIT



insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.26','2000-1-4 00:00:00','- Added ISSA mailling labels.
- Fixed ''Vendor Info Labels'' report.
- Changed tables with a Model field to allow alphanumeric entries.')
go

update tblDBProperties
set PropertyValue = '3.26'
where PropertyName = 'DBStructVersion'
go
