CREATE DATABASE fcdata
go

USE fcdata
go

if not exists (select * from master..syslogins where name = N'fcuser')
BEGIN
	declare @logindb nvarchar(132), @loginlang nvarchar(132) select @logindb = N'fcdata', @loginlang = N'us_english'
	if @logindb is null or not exists (select * from master..sysdatabases where name = @logindb)
		select @logindb = N'master'
	if @loginlang is null or (not exists (select * from master..syslanguages where name = @loginlang) and @loginlang <> N'us_english')
		select @loginlang = @@language
	exec sp_addlogin N'fcuser', null, @logindb, @loginlang
END
GO

if not exists (select * from sysusers where name = N'fcuser' and uid < 16382)
	EXEC sp_grantdbaccess N'fcuser', N'fcuser'
GO

if not exists (select * from sysusers where name = N'fcgenuser' and uid > 16399)
	EXEC sp_addrole N'fcgenuser'
GO

exec sp_addrolemember N'db_datareader', N'fcuser'
GO

exec sp_addrolemember N'db_datawriter', N'fcuser'
GO

CREATE TABLE [dbo].[patmprptBillOfMatPParts] (
	[Purchase Order #] [int] NULL ,
	[Vendor] [nvarchar] (255) NULL ,
	[Vendor Part Number] [nvarchar] (255) NULL ,
	[RPS Part Number] [nvarchar] (255) NULL ,
	[Requested Ship Date] [datetime] NULL ,
	[Cost Each] [money] NULL ,
	[Quantity] [int] NULL ,
	[Value] [money] NULL ,
	[Received Date] [datetime] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[pltblImportedData] (
	[DATE] [nvarchar] (8) NULL ,
	[SECTION] [nvarchar] (6) NULL ,
	[FILE] [nvarchar] (30) NULL ,
	[COMPANY] [nvarchar] (38) NULL ,
	[SUBSIDIARY] [nvarchar] (40) NULL ,
	[STREET] [nvarchar] (38) NULL ,
	[CITY] [nvarchar] (45) NULL ,
	[STATE] [nvarchar] (2) NULL ,
	[ZIP] [nvarchar] (10) NULL ,
	[PHONE] [nvarchar] (20) NULL ,
	[FAX] [nvarchar] (25) NULL ,
	[COUNTRY] [nvarchar] (10) NULL ,
	[SIC_CODE] [nvarchar] (10) NULL ,
	[ENTRY] [ntext] NULL ,
	[CONTACT1FN] [nvarchar] (17) NULL ,
	[CONTACT1LN] [nvarchar] (15) NULL ,
	[CONTACT1TI] [nvarchar] (20) NULL ,
	[CONTACT2FN] [nvarchar] (15) NULL ,
	[CONTACT2LN] [nvarchar] (15) NULL ,
	[CONTACT2TI] [nvarchar] (20) NULL ,
	[CONTACT3FN] [nvarchar] (15) NULL ,
	[CONTACT3LN] [nvarchar] (15) NULL ,
	[CONTACT3TI] [nvarchar] (15) NULL ,
	[NAME] [nvarchar] (80) NULL ,
	[ORDER] [float] NULL ,
	[OK] [nvarchar] (1) NULL ,
	[EDITION] [nvarchar] (8) NULL ,
	[AREA_CODE] [nvarchar] (50) NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[pltblImportedFile] (
	[DateOfLead] [datetime] NULL ,
	[CompanyName] [nvarchar] (50) NULL ,
	[ContactName] [nvarchar] (50) NULL ,
	[Title] [nvarchar] (50) NULL ,
	[Address] [nvarchar] (50) NULL ,
	[City] [nvarchar] (50) NULL ,
	[State] [nvarchar] (2) NULL ,
	[Zip] [nvarchar] (5) NULL ,
	[Phone] [nvarchar] (50) NULL ,
	[SIC] [nvarchar] (50) NULL ,
	[Entry] [ntext] NULL ,
	[Include] [bit] NOT NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Switchboard Items] (
	[SwitchboardID] [int] NOT NULL ,
	[ItemNumber] [smallint] NOT NULL ,
	[ItemText] [nvarchar] (255) NULL ,
	[Command] [smallint] NULL ,
	[Argument] [nvarchar] (255) NULL ,
	[Tooltip] [nvarchar] (100) NULL ,
	[OpenArgs] [nvarchar] (100) NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Tbl_Numbers] (
	[Number] [real] NULL ,
	[Counter] [int] IDENTITY (1, 1) NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblAllLeads] (
	[LeadID] [int] IDENTITY (1, 1) NOT NULL ,
	[DealerName] [nvarchar] (50) NULL ,
	[Location] [nvarchar] (50) NULL ,
	[LeadDate] [datetime] NULL ,
	[Salesman] [nvarchar] (50) NULL ,
	[CompanyName] [nvarchar] (100) NULL ,
	[Contact] [nvarchar] (100) NULL ,
	[ContactTitle] [nvarchar] (50) NULL ,
	[Address] [nvarchar] (50) NULL ,
	[City] [nvarchar] (20) NULL ,
	[State] [nvarchar] (20) NULL ,
	[Zip] [nvarchar] (20) NULL ,
	[Phone] [nvarchar] (30) NULL ,
	[SIC] [nvarchar] (20) NULL ,
	[Size] [nvarchar] (20) NULL ,
	[ApplicationNotes] [ntext] NULL ,
	[ResponseMethod] [nvarchar] (20) NULL ,
	[Purchase] [nvarchar] (50) NULL ,
	[Code] [nvarchar] (50) NULL ,
	[ActiveInactive] [nchar] (10) NULL ,
	[Result] [ntext] NULL ,
	[SendInfoOn] [nvarchar] (50) NULL ,
	[DealerNumber] [nvarchar] (50) NULL ,
	[Purchased] [bit] NOT NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblDealerAdvert] (
	[AdvertID] [int] IDENTITY (1, 1) NOT NULL ,
	[fkDealerID] [int] NULL ,
	[AdvertDate] [datetime] NULL ,
	[AdvertAmt] [money] NULL ,
	[AdvertNote] [ntext] NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblDealerDemos] (
	[DealerDemoID] [int] IDENTITY (1, 1) NOT NULL ,
	[fkDealerID] [int] NULL ,
	[CustomerName] [nvarchar] (50) NULL ,
	[ContactName] [nvarchar] (50) NULL ,
	[Phone] [nvarchar] (50) NULL ,
	[Address] [nvarchar] (50) NULL ,
	[City] [nvarchar] (30) NULL ,
	[State] [nvarchar] (2) NULL ,
	[Zip] [nvarchar] (11) NULL ,
	[DateOfDemo] [datetime] NULL ,
	[EquipmentDemoed] [ntext] NULL ,
	[Notes] [ntext] NULL ,
	[Purchased] [bit] NOT NULL ,
	[Model40] [smallint] NULL ,
	[Model27] [smallint] NULL ,
	[Model10] [smallint] NULL ,
	[Model39] [smallint] NULL ,
	[Model34] [smallint] NULL ,
	[Model48] [smallint] NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblDealerPartsList] (
	[DealerPartID] [int] IDENTITY (1, 1) NOT NULL ,
	[RPSPartNum] [nvarchar] (50) NULL ,
	[PartName] [nvarchar] (150) NULL ,
	[DealerNetPrice] [money] NULL ,
	[SuggestedListPrice] [money] NULL ,
	[QuantityRequired] [int] NULL ,
	[PageReference] [int] NULL ,
	[Notes] [nvarchar] (255) NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblDealers] (
	[DealerID] [int] IDENTITY (1, 1) NOT NULL ,
	[CurrentDealer] [bit] NOT NULL ,
	[DealerName] [nvarchar] (100) NULL ,
	[TollFreeNumber] [nvarchar] (20) NULL ,
	[ContactName] [nvarchar] (50) NULL ,
	[ContactTitle] [nvarchar] (50) NULL ,
	[StreetAddress] [nvarchar] (50) NULL ,
	[City] [nvarchar] (50) NULL ,
	[State] [nvarchar] (20) NULL ,
	[Zip] [nvarchar] (20) NULL ,
	[Phone] [nvarchar] (20) NULL ,
	[Fax] [nvarchar] (20) NULL ,
	[Num] [nvarchar] (20) NULL ,
	[SweeperDealer] [bit] NOT NULL ,
	[CarPhoneNumber] [nvarchar] (20) NULL ,
	[TerritoryCovered] [nvarchar] (100) NULL ,
	[NumSalesman] [nvarchar] (20) NULL ,
	[MajorProducts] [nvarchar] (100) NULL ,
	[SalesmensNames] [nvarchar] (100) NULL ,
	[FirstName] [nvarchar] (50) NULL ,
	[LastName] [nvarchar] (50) NULL ,
	[HomePhoneNumber] [nvarchar] (20) NULL ,
	[Notes] [ntext] NULL ,
	[ContractExpires] [datetime] NULL ,
	[SalesmanName] [nvarchar] (50) NULL ,
	[WarrentyAdministrator] [nvarchar] (50) NULL ,
	[ServiceManagerName] [nvarchar] (50) NULL ,
	[LaborRate] [money] NULL ,
	[PartsManagerName] [nvarchar] (50) NULL ,
	[OfficeManagerName] [nvarchar] (50) NULL ,
	[Terms] [nvarchar] (50) NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblDealersGoals] (
	[fkDealerID] [int] NOT NULL ,
	[Year] [nvarchar] (4) NOT NULL ,
	[Model] [smallint] NOT NULL ,
	[Goal] [smallint] NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblLists] (
	[ListID] [int] IDENTITY (1, 1) NOT NULL ,
	[fkDealerID] [int] NULL ,
	[State] [nvarchar] (50) NULL ,
	[County] [nvarchar] (50) NULL ,
	[DateListOrdered] [datetime] NULL ,
	[LastDateMailed] [datetime] NULL ,
	[Flier] [smallint] NULL ,
	[ListCompany] [nvarchar] (50) NULL ,
	[Notes] [ntext] NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblMajorAccounts] (
	[MajorAccountID] [int] IDENTITY (1, 1) NOT NULL ,
	[fkMADealerID] [int] NULL ,
	[MACompName] [nvarchar] (50) NULL ,
	[MAHeadqAddress] [nvarchar] (50) NULL ,
	[MACity] [nvarchar] (30) NULL ,
	[MAState] [nvarchar] (2) NULL ,
	[MAZip] [nvarchar] (10) NULL ,
	[MAPurContact] [nvarchar] (50) NULL ,
	[MAManageContact] [nvarchar] (50) NULL ,
	[MAPhone] [nvarchar] (20) NULL ,
	[MAFax] [nvarchar] (20) NULL ,
	[MANumLocations] [int] NULL ,
	[MALocations] [nvarchar] (20) NULL ,
	[MASerialNums] [ntext] NULL ,
	[MAInitialPO] [ntext] NULL ,
	[MAApproved] [bit] NOT NULL ,
	[MADenied] [bit] NOT NULL ,
	[MAAccountNum] [nvarchar] (15) NULL ,
	[MAAccountNumKey] [int] NULL ,
	[MANotes] [ntext] NULL ,
	[MADateApproved] [datetime] NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblMenuHistory] (
	[HistoryID] [int] IDENTITY (1, 1) NOT NULL ,
	[HistoryTime] [datetime] NULL ,
	[HistoryUser] [char] (10) NULL ,
	[HistorySwitchID] [int] NULL ,
	[HistorySwitchItem] [int] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblModels] (
	[ModelID] [int] IDENTITY (1, 1) NOT NULL ,
	[Model] [smallint] NULL ,
	[Description] [ntext] NULL ,
	[Price] [money] NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblNewsletter] (
	[NewsletterID] [int] IDENTITY (1, 1) NOT NULL ,
	[DealerFlag] [bit] NOT NULL ,
	[DealerName] [nvarchar] (40) NULL ,
	[Name] [nvarchar] (37) NULL ,
	[Salutation] [nvarchar] (17) NULL ,
	[StreetAddress] [nvarchar] (26) NULL ,
	[City] [nvarchar] (19) NULL ,
	[State] [nvarchar] (4) NULL ,
	[Zip] [nvarchar] (13) NULL ,
	[Phone] [nvarchar] (20) NULL ,
	[Fax] [nvarchar] (22) NULL ,
	[Notes] [ntext] NULL ,
	[fkDealerID] [smallint] NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblOrders] (
	[OrderID] [int] IDENTITY (1, 1) NOT NULL ,
	[Model] [smallint] NULL ,
	[OrderDate] [datetime] NULL ,
	[OrderNumber] [nvarchar] (50) NULL ,
	[OrderKey] [int] NULL ,
	[Dealer] [nvarchar] (50) NULL ,
	[PurchaseOrder] [nvarchar] (50) NULL ,
	[Quantity] [smallint] NULL ,
	[PromisedDate] [datetime] NULL ,
	[ShippedDate] [datetime] NULL ,
	[Battery] [nchar] (10) NULL ,
	[Size] [nchar] (10) NULL ,
	[AmpCharger] [nchar] (10) NULL ,
	[HourMeter] [nchar] (10) NULL ,
	[SerialNumber] [nchar] (30) NULL ,
	[TwelveVMotor] [nchar] (15) NULL ,
	[Eighteen1hpMotor] [nchar] (10) NULL ,
	[TwoHP] [nchar] (10) NULL ,
	[SalePrice] [money] NULL ,
	[CostPrice] [money] NULL ,
	[Margin] [money] NULL ,
	[Terms] [nvarchar] (50) NULL ,
	[ShipVia] [nvarchar] (50) NULL ,
	[CollectPrepaid] [nchar] (30) NULL ,
	[Notes] [ntext] NULL ,
	[Plus2Batteries] [nchar] (10) NULL ,
	[FortyAmp] [nchar] (10) NULL ,
	[Horn] [nchar] (10) NULL ,
	[Alarm] [nchar] (10) NULL ,
	[Name] [nvarchar] (100) NULL ,
	[SaleDate] [datetime] NULL ,
	[Address] [nvarchar] (100) NULL ,
	[City] [nvarchar] (50) NULL ,
	[State] [nvarchar] (20) NULL ,
	[Zip] [nvarchar] (20) NULL ,
	[ContactName] [nvarchar] (100) NULL ,
	[Phone] [nvarchar] (35) NULL ,
	[EighteenMonthOption] [nchar] (10) NULL ,
	[DealerDemo] [nchar] (10) NULL ,
	[StandardWarranty] [nchar] (10) NULL ,
	[LastDateMailedInfoTo] [datetime] NULL ,
	[Note] [ntext] NULL ,
	[ShipName] [nvarchar] (100) NULL ,
	[StreetAddress] [nvarchar] (100) NULL ,
	[CityStateZip] [nvarchar] (100) NULL ,
	[TagForEndUserReport] [nchar] (10) NULL ,
	[TypeOfBusiness] [nvarchar] (50) NULL ,
	[PartialList] [nvarchar] (50) NULL ,
	[LastUsedDate] [datetime] NULL ,
	[ContactedDate] [datetime] NULL ,
	[ContactedBy] [nvarchar] (50) NULL ,
	[Options] [ntext] NULL ,
	[SICCode] [float] NULL ,
	[TermsInfo] [nvarchar] (50) NULL ,
	[fkDealerID] [int] NULL ,
	[MajorAccount] [bit] NULL ,
	[MajorAccountID] [int] NULL ,
	[fkServDealerID] [int] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblParts] (
	[PartID] [int] IDENTITY (1, 1) NOT NULL ,
	[Model] [nvarchar] (30) NULL ,
	[RPSPurchased] [bit] NOT NULL ,
	[BlanketRelease] [nvarchar] (20) NULL ,
	[DateOrdered] [datetime] NULL ,
	[LeadTime] [nvarchar] (20) NULL ,
	[Qty] [smallint] NULL ,
	[PartName] [nvarchar] (150) NULL ,
	[Location] [nvarchar] (150) NULL ,
	[VendorName] [nvarchar] (100) NULL ,
	[VendorPartName] [nvarchar] (150) NULL ,
	[ManfPartNum] [nvarchar] (50) NULL ,
	[RPSPartNum] [nvarchar] (50) NULL ,
	[CostEach] [money] NULL ,
	[Notes] [nvarchar] (255) NULL ,
	[QuoteDate] [datetime] NULL ,
	[QtyReq] [int] NULL ,
	[TotalCostPerMachine] [money] NULL ,
	[TotalCostPerUnitForPart] [money] NULL ,
	[PageRef] [int] NULL ,
	[VendorCost] [money] NULL ,
	[AutoCalcPrice] [nvarchar] (10) NULL ,
	[SubPartTotal] [money] NULL ,
	[DealerNet] [money] NULL ,
	[SuggestedList] [money] NULL ,
	[Note] [nvarchar] (255) NULL ,
	[CanadianDealerNet] [money] NULL ,
	[Quantity] [nvarchar] (50) NULL ,
	[Total] [float] NULL ,
	[CanadianSuggestedList] [money] NULL ,
	[GSAList] [money] NULL ,
	[FrenchPartDescription] [nvarchar] (150) NULL ,
	[SubFlag] [bit] NOT NULL ,
	[ShelfNo] [float] NULL ,
	[Section] [nvarchar] (10) NULL ,
	[PartLevel] [float] NULL ,
	[HideOnReports] [bit] NOT NULL ,
	[PartOption] [bit] NOT NULL ,
	[EffectiveDate] [datetime] NULL ,
	[SNEffective] [nvarchar] (50) NULL ,
	[FinishDesc] [nvarchar] (50) NULL ,
	[RunQuantity] [int] NULL ,
	[LeadTimeDays] [nvarchar] (50) NULL ,
	[PartCode] [nvarchar] (1) NULL ,
	[DrawingNum] [nvarchar] (50) NULL ,
	[RevisionNum] [nvarchar] (50) NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblPartsModels] (
	[PartModelID] [int] IDENTITY (1, 1) NOT NULL ,
	[fkPartID] [int] NULL ,
	[Model] [smallint] NULL ,
	[Quantity] [float] NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblPartsSubParts] (
	[SubPartID] [int] IDENTITY (1, 1) NOT NULL ,
	[SubNum] [nvarchar] (50) NULL ,
	[SubDescription] [nvarchar] (100) NULL ,
	[SubCost] [money] NULL ,
	[SubExtCost] [money] NULL ,
	[SubSource] [nvarchar] (50) NULL ,
	[SubSourcePartNum] [nvarchar] (50) NULL ,
	[SubQty] [int] NULL ,
	[fkPartID] [int] NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblPO] (
	[POID] [int] NOT NULL ,
	[Vendor] [nvarchar] (50) NULL ,
	[Confirmed] [bit] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblPOPart] (
	[POPartID] [int] IDENTITY (1, 1) NOT NULL ,
	[fkPOID] [int] NULL ,
	[VendorPartNumber] [nvarchar] (50) NULL ,
	[RPSPartNum] [nvarchar] (50) NULL ,
	[PartDescription] [nvarchar] (100) NULL ,
	[combo] [nvarchar] (50) NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblPOPartDetail] (
	[Quantity] [int] NULL ,
	[RequestedShipDate] [datetime] NULL ,
	[ReceivedDate] [datetime] NULL ,
	[QuantityReceived] [int] NULL ,
	[CostEach] [money] NULL ,
	[Value] [money] NULL ,
	[Notes] [nvarchar] (255) NULL ,
	[POPartDetailID] [int] IDENTITY (1, 1) NOT NULL ,
	[fkPOPartID] [int] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblProdSchedItems] (
	[ScheduleID] [int] NOT NULL ,
	[Model] [smallint] NOT NULL ,
	[Quantity] [smallint] NOT NULL ,
	[SchedItemID] [int] IDENTITY (1, 1) NOT NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblProdSchedules] (
	[ScheduleID] [int] IDENTITY (1, 1) NOT NULL ,
	[Month] [smallint] NULL ,
	[Year] [smallint] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblResponseMethod] (
	[ResposneID] [int] IDENTITY (1, 1) NOT NULL ,
	[ResponseMethod] [nvarchar] (50) NULL ,
	[ResponseMethodNotes] [ntext] NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblSysMessage] (
	[SysMessageFrom] [nvarchar] (20) NULL ,
	[SysMessage] [nvarchar] (255) NULL ,
	[SysMessageTime] [datetime] NULL ,
	[SysKick] [bit] NOT NULL ,
	[SysMessageID] [int] IDENTITY (0, 1) NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblTip] (
	[TipID] [int] IDENTITY (1, 1) NOT NULL ,
	[TipArea] [char] (30) NULL ,
	[TipText] [varchar] (255) NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblTipDisable] (
	[UserName] [nvarchar] (50) NOT NULL ,
	[TipID] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblToDo] (
	[ToDoID] [int] IDENTITY (1, 1) NOT NULL ,
	[ToDoArea] [varchar] (50) NULL ,
	[ToDoNote] [ntext] NULL ,
	[ToDoDone] [bit] NOT NULL ,
	[ToDoEnteredDate] [datetime] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblVendors] (
	[VendorID] [int] IDENTITY (1, 1) NOT NULL ,
	[VendorName] [nvarchar] (50) NULL ,
	[CointactName] [nvarchar] (50) NULL ,
	[StreetAddress] [nvarchar] (50) NULL ,
	[City] [nvarchar] (30) NULL ,
	[State] [nvarchar] (2) NULL ,
	[Zip] [nvarchar] (10) NULL ,
	[Phone] [nvarchar] (20) NULL ,
	[Fax] [nvarchar] (20) NULL ,
	[BlanketProductionOrders] [bit] NOT NULL ,
	[RPSPartNum] [nvarchar] (50) NULL ,
	[VendorPartNum] [nvarchar] (50) NULL ,
	[PartDescription] [ntext] NULL ,
	[Notes] [ntext] NULL ,
	[Active] [bit] NOT NULL ,
	[Email] [nvarchar] (50) NULL ,
	[upsize_ts] [timestamp] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblWarranty] (
	[WarrantyID] [int] IDENTITY (1, 1) NOT NULL ,
	[MachineSerialNumber] [nvarchar] (50) NULL ,
	[DateOfFailure] [datetime] NULL ,
	[CreditMemoNum] [nvarchar] (50) NULL ,
	[CreditMemoAmt] [money] NULL ,
	[Dealer] [nvarchar] (50) NULL ,
	[Customer] [nvarchar] (50) NULL ,
	[RGANum] [float] NULL ,
	[PartCost] [float] NULL ,
	[LaborCost] [float] NULL ,
	[Freight] [float] NULL ,
	[Problem] [ntext] NULL ,
	[Model] [smallint] NULL ,
	[Resolution] [ntext] NULL ,
	[WarrantyOpen] [bit] NULL ,
	[Travel] [money] NULL ,
	[Policy] [money] NULL ,
	[DateEntered] [datetime] NULL ,
	[PartReceived] [bit] NULL ,
	[Hours] [real] NULL ,
	[fkDealerID] [smallint] NULL ,
	[Comment] [ntext] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblWarrantyParts] (
	[WarrantyPartID] [int] IDENTITY (1, 1) NOT NULL ,
	[fkWarrantyID] [int] NULL ,
	[PartNumReplaced] [nvarchar] (50) NULL ,
	[PartDescription] [nvarchar] (200) NULL ,
	[PartCost] [money] NULL ,
	[PartFileIndex] [int] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tlkuContactTitles] (
	[ContactTitle] [nvarchar] (50) NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tlkuFinish] (
	[FinishID] [int] IDENTITY (1, 1) NOT NULL ,
	[FinishDesc] [nvarchar] (50) NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tmpLabelQtys] (
	[PartIndex] [int] NOT NULL ,
	[Quantity] [smallint] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tmpNames] (
	[PersonName] [nvarchar] (50) NULL ,
	[DealerIndex] [int] NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[pltblImportedFile] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__Inclu__0DAF0CB0] DEFAULT (0) FOR [Include]
GO

ALTER TABLE [dbo].[Switchboard Items] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__ItemN__1273C1CD] DEFAULT (0) FOR [ItemNumber],
	CONSTRAINT [aaaaaSwitchboard Items_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[SwitchboardID],
		[ItemNumber]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Tbl_Numbers] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__Numbe__164452B1] DEFAULT (0) FOR [Number],
	CONSTRAINT [PK_Tbl_Numbers] PRIMARY KEY  NONCLUSTERED 
	(
		[Counter]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblAllLeads] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__Activ__3F466844] DEFAULT ('A') FOR [ActiveInactive],
	CONSTRAINT [DF__Temporary__Purch__403A8C7D] DEFAULT (0) FOR [Purchased],
	CONSTRAINT [tblLeads_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[LeadID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblDealerAdvert] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__fkDea__30F848ED] DEFAULT (0) FOR [fkDealerID],
	CONSTRAINT [DF__Temporary__Adver__32E0915F] DEFAULT (0) FOR [AdvertAmt],
	CONSTRAINT [aaaaatblDealerAdvert_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[AdvertID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblDealerDemos] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__fkDea__21B6055D] DEFAULT (0) FOR [fkDealerID],
	CONSTRAINT [DF__Temporary__Purch__22AA2996] DEFAULT (0) FOR [Purchased],
	CONSTRAINT [DF__Temporary__Model__239E4DCF] DEFAULT (0) FOR [Model40],
	CONSTRAINT [DF__Temporary__Model__24927208] DEFAULT (0) FOR [Model27],
	CONSTRAINT [DF__Temporary__Model__25869641] DEFAULT (0) FOR [Model10],
	CONSTRAINT [DF__Temporary__Model__267ABA7A] DEFAULT (0) FOR [Model39],
	CONSTRAINT [DF__Temporary__Model__276EDEB3] DEFAULT (0) FOR [Model34],
	CONSTRAINT [DF__Temporary__Model__286302EC] DEFAULT (0) FOR [Model48],
	CONSTRAINT [aaaaatblDealerDemos_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[DealerDemoID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblDealerPartsList] WITH NOCHECK ADD 
	CONSTRAINT [aaaaatblDealerPartsList_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[DealerPartID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblDealers] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__Curre__1B0907CE] DEFAULT (0) FOR [CurrentDealer],
	CONSTRAINT [DF__Temporary__Sweep__1BFD2C07] DEFAULT (0) FOR [SweeperDealer],
	CONSTRAINT [DF__Temporary__Labor__1CF15040] DEFAULT (0) FOR [LaborRate],
	CONSTRAINT [aaaaatblDealers_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[DealerID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblDealersGoals] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__fkDea__37A5467C] DEFAULT (0) FOR [fkDealerID],
	CONSTRAINT [DF__Temporary__Model__398D8EEE] DEFAULT (0) FOR [Model],
	CONSTRAINT [DF__TemporaryU__Goal__3A81B327] DEFAULT (0) FOR [Goal],
	CONSTRAINT [aaaaatblDealersGoals_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[Year],
		[Model],
		[fkDealerID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblLists] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__fkDea__45F365D3] DEFAULT (0) FOR [fkDealerID],
	CONSTRAINT [DF__Temporary__Flier__46E78A0C] DEFAULT (0) FOR [Flier],
	CONSTRAINT [aaaaatblLists_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[ListID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblMajorAccounts] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__fkMAD__4BAC3F29] DEFAULT (0) FOR [fkMADealerID],
	CONSTRAINT [DF__Temporary__MANum__4CA06362] DEFAULT (0) FOR [MANumLocations],
	CONSTRAINT [DF__Temporary__MAApp__4D94879B] DEFAULT (0) FOR [MAApproved],
	CONSTRAINT [DF__Temporary__MADen__4E88ABD4] DEFAULT (0) FOR [MADenied],
	CONSTRAINT [DF__Temporary__MAAcc__4F7CD00D] DEFAULT (0) FOR [MAAccountNumKey],
	CONSTRAINT [aaaaatblMajorAccounts_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[MajorAccountID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblMenuHistory] WITH NOCHECK ADD 
	CONSTRAINT [PK_tblMenuHistory] PRIMARY KEY  NONCLUSTERED 
	(
		[HistoryID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblModels] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__Model__5441852A] DEFAULT (0) FOR [Model],
	CONSTRAINT [DF__Temporary__Price__5535A963] DEFAULT (0) FOR [Price],
	CONSTRAINT [aaaaatblModels_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[ModelID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblNewsletter] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__Deale__59FA5E80] DEFAULT (0) FOR [DealerFlag],
	CONSTRAINT [DF__Temporary__fkDea__5AEE82B9] DEFAULT (0) FOR [fkDealerID],
	CONSTRAINT [aaaaatblNewsletter_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[NewsletterID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblOrders] WITH NOCHECK ADD 
	CONSTRAINT [DF_tblOrders_MajorAccountID] DEFAULT (0) FOR [MajorAccountID],
	CONSTRAINT [PK_tblOrders] PRIMARY KEY  NONCLUSTERED 
	(
		[OrderID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblParts] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__RPSPu__693CA210] DEFAULT (0) FOR [RPSPurchased],
	CONSTRAINT [DF__TemporaryUp__Qty__6A30C649] DEFAULT (0) FOR [Qty],
	CONSTRAINT [DF__Temporary__Total__6B24EA82] DEFAULT (0) FOR [TotalCostPerUnitForPart],
	CONSTRAINT [DF__Temporary__SubPa__6C190EBB] DEFAULT (0) FOR [SubPartTotal],
	CONSTRAINT [DF__Temporary__SubFl__6D0D32F4] DEFAULT (0) FOR [SubFlag],
	CONSTRAINT [DF__Temporary__Shelf__6E01572D] DEFAULT (0) FOR [ShelfNo],
	CONSTRAINT [DF__Temporary__Level__6EF57B66] DEFAULT (0) FOR [PartLevel],
	CONSTRAINT [DF__Temporary__HideO__6FE99F9F] DEFAULT (0) FOR [HideOnReports],
	CONSTRAINT [DF__Temporary__Optio__70DDC3D8] DEFAULT (0) FOR [PartOption],
	CONSTRAINT [DF__Temporary__RunQu__71D1E811] DEFAULT (0) FOR [RunQuantity],
	CONSTRAINT [aaaaatblParts_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[PartID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblPartsModels] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__fkPar__76969D2E] DEFAULT (0) FOR [fkPartID],
	CONSTRAINT [DF__Temporary__Model__778AC167] DEFAULT (0) FOR [Model],
	CONSTRAINT [DF__Temporary__Quant__787EE5A0] DEFAULT (0) FOR [Quantity],
	CONSTRAINT [tblPartsModels_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[PartModelID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblPartsSubParts] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__SubCo__7D439ABD] DEFAULT (0) FOR [SubCost],
	CONSTRAINT [DF__Temporary__SubEx__7E37BEF6] DEFAULT (0) FOR [SubExtCost],
	CONSTRAINT [DF__Temporary__SubQt__7F2BE32F] DEFAULT (0) FOR [SubQty],
	CONSTRAINT [DF__Temporary__fkPar__00200768] DEFAULT (0) FOR [fkPartID],
	CONSTRAINT [aaaaatblPartsSubParts_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[SubPartID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblPO] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__Confi__04E4BC85] DEFAULT (0) FOR [Confirmed],
	CONSTRAINT [aaaaatblVendorPOs_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[POID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblPOPart] WITH NOCHECK ADD 
	CONSTRAINT [tblPODetail_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[POPartID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblPOPartDetail] WITH NOCHECK ADD 
	CONSTRAINT [PK_tblPO] PRIMARY KEY  NONCLUSTERED 
	(
		[POPartDetailID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblProdSchedItems] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__Sched__0E6E26BF] DEFAULT (0) FOR [ScheduleID],
	CONSTRAINT [DF__Temporary__Model__0F624AF8] DEFAULT (0) FOR [Model],
	CONSTRAINT [DF__Temporary__Quant__10566F31] DEFAULT (0) FOR [Quantity],
	CONSTRAINT [aaaaatblProdSchedItems_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[ScheduleID],
		[Model],
		[Quantity]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblProdSchedules] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__Month__151B244E] DEFAULT (0) FOR [Month],
	CONSTRAINT [DF__TemporaryU__Year__160F4887] DEFAULT (0) FOR [Year],
	CONSTRAINT [aaaaatblProdSchedules_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[ScheduleID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblResponseMethod] WITH NOCHECK ADD 
	CONSTRAINT [aaaaatblResponseMethod_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[ResposneID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblSysMessage] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__SysKi__1DB06A4F] DEFAULT (0) FOR [SysKick],
	CONSTRAINT [PK_tblSysMessage] PRIMARY KEY  NONCLUSTERED 
	(
		[SysMessageID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblTip] WITH NOCHECK ADD 
	CONSTRAINT [PK_tblTip] PRIMARY KEY  NONCLUSTERED 
	(
		[TipID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblTipDisable] WITH NOCHECK ADD 
	CONSTRAINT [PK_tblTipDisable] PRIMARY KEY  NONCLUSTERED 
	(
		[UserName],
		[TipID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblToDo] WITH NOCHECK ADD 
	CONSTRAINT [PK_tblNagList] PRIMARY KEY  NONCLUSTERED 
	(
		[ToDoID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblVendors] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__Blank__2739D489] DEFAULT (0) FOR [BlanketProductionOrders],
	CONSTRAINT [DF__Temporary__Activ__282DF8C2] DEFAULT ((-1)) FOR [Active],
	CONSTRAINT [aaaaatblVendors_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[VendorID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblWarranty] WITH NOCHECK ADD 
	CONSTRAINT [PK_tblWarranty] PRIMARY KEY  NONCLUSTERED 
	(
		[WarrantyID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblWarrantyParts] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__fkWar__395884C4] DEFAULT (0) FOR [fkWarrantyID],
	CONSTRAINT [DF__Temporary__PartC__3A4CA8FD] DEFAULT (0) FOR [PartCost],
	CONSTRAINT [DF__Temporary__PartF__3B40CD36] DEFAULT (0) FOR [PartFileIndex],
	CONSTRAINT [aaaaatblWarrantyParts_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[WarrantyPartID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tlkuContactTitles] WITH NOCHECK ADD 
	CONSTRAINT [aaaaatlkuContactTitles_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[ContactTitle]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tlkuFinish] WITH NOCHECK ADD 
	CONSTRAINT [aaaaatlkuFinish_PK] PRIMARY KEY  NONCLUSTERED 
	(
		[FinishID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tmpLabelQtys] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__PartI__46B27FE2] DEFAULT (0) FOR [PartIndex],
	CONSTRAINT [DF__Temporary__Quant__47A6A41B] DEFAULT (0) FOR [Quantity],
	CONSTRAINT [PK_tmpLabelQtys] PRIMARY KEY  NONCLUSTERED 
	(
		[PartIndex]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tmpNames] WITH NOCHECK ADD 
	CONSTRAINT [DF__Temporary__Deale__4B7734FF] DEFAULT (0) FOR [DealerIndex]
GO

 CREATE  INDEX [Title] ON [dbo].[pltblImportedFile]([Title]) ON [PRIMARY]
GO

 CREATE  INDEX [Number] ON [dbo].[Tbl_Numbers]([Number]) ON [PRIMARY]
GO

 CREATE  INDEX [active] ON [dbo].[tblAllLeads]([ActiveInactive]) ON [PRIMARY]
GO

 CREATE  INDEX [Code] ON [dbo].[tblAllLeads]([Code]) ON [PRIMARY]
GO

 CREATE  INDEX [company name] ON [dbo].[tblAllLeads]([CompanyName]) ON [PRIMARY]
GO

 CREATE  INDEX [Date of Lead] ON [dbo].[tblAllLeads]([LeadDate]) ON [PRIMARY]
GO

 CREATE  INDEX [Dealer Name] ON [dbo].[tblAllLeads]([DealerName]) ON [PRIMARY]
GO

 CREATE  INDEX [phone] ON [dbo].[tblAllLeads]([Phone]) ON [PRIMARY]
GO

 CREATE  INDEX [{DDDCE5F1-4923-11D3-BB05-004033532B7F}] ON [dbo].[tblDealerAdvert]([fkDealerID]) ON [PRIMARY]
GO

 CREATE  INDEX [fkDealerID] ON [dbo].[tblDealerAdvert]([fkDealerID]) ON [PRIMARY]
GO

 CREATE  INDEX [contact] ON [dbo].[tblDealerDemos]([ContactName]) ON [PRIMARY]
GO

 CREATE  INDEX [CustomerName] ON [dbo].[tblDealerDemos]([CustomerName]) ON [PRIMARY]
GO

 CREATE  INDEX [date of demo] ON [dbo].[tblDealerDemos]([DateOfDemo]) ON [PRIMARY]
GO

 CREATE  INDEX [dealernum] ON [dbo].[tblDealerDemos]([fkDealerID]) ON [PRIMARY]
GO

 CREATE  INDEX [RPSPartNum] ON [dbo].[tblDealerPartsList]([RPSPartNum]) ON [PRIMARY]
GO

 CREATE  INDEX [City] ON [dbo].[tblDealers]([City]) ON [PRIMARY]
GO

 CREATE  INDEX [Num] ON [dbo].[tblDealers]([Num]) ON [PRIMARY]
GO

 CREATE  INDEX [NumSalesman] ON [dbo].[tblDealers]([NumSalesman]) ON [PRIMARY]
GO

 CREATE  INDEX [phone] ON [dbo].[tblDealers]([Phone]) ON [PRIMARY]
GO

 CREATE  INDEX [Sort1] ON [dbo].[tblDealers]([DealerName]) ON [PRIMARY]
GO

 CREATE  INDEX [{DDDCE5F0-4923-11D3-BB05-004033532B7F}] ON [dbo].[tblDealersGoals]([fkDealerID]) ON [PRIMARY]
GO

 CREATE  INDEX [DealerIndex] ON [dbo].[tblDealersGoals]([fkDealerID]) ON [PRIMARY]
GO

 CREATE  INDEX [fkDealerID] ON [dbo].[tblLists]([fkDealerID]) ON [PRIMARY]
GO

 CREATE  INDEX [fkMADealerID] ON [dbo].[tblMajorAccounts]([fkMADealerID]) ON [PRIMARY]
GO

 CREATE  INDEX [MAAccountNum] ON [dbo].[tblMajorAccounts]([MAAccountNum]) ON [PRIMARY]
GO

 CREATE  INDEX [MAAccountNumKey] ON [dbo].[tblMajorAccounts]([MAAccountNumKey]) ON [PRIMARY]
GO

 CREATE  INDEX [VendorName] ON [dbo].[tblMajorAccounts]([MACompName]) ON [PRIMARY]
GO

 CREATE  INDEX [ModelID] ON [dbo].[tblModels]([ModelID]) ON [PRIMARY]
GO

 CREATE  INDEX [fkDealerID] ON [dbo].[tblNewsletter]([fkDealerID]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_tblOrders_OrderNumber] ON [dbo].[tblOrders]([OrderNumber]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_tblOrders_OrderDate] ON [dbo].[tblOrders]([OrderDate]) ON [PRIMARY]
GO

 CREATE  INDEX [dateorder] ON [dbo].[tblParts]([DateOrdered]) ON [PRIMARY]
GO

 CREATE  INDEX [DrawingNum] ON [dbo].[tblParts]([DrawingNum]) ON [PRIMARY]
GO

 CREATE  INDEX [ManfPartNum] ON [dbo].[tblParts]([ManfPartNum]) ON [PRIMARY]
GO

 CREATE  INDEX [model] ON [dbo].[tblParts]([Model]) ON [PRIMARY]
GO

 CREATE  INDEX [PartCode] ON [dbo].[tblParts]([PartCode]) ON [PRIMARY]
GO

 CREATE  INDEX [partname] ON [dbo].[tblParts]([PartName]) ON [PRIMARY]
GO

 CREATE  INDEX [RevisionNum] ON [dbo].[tblParts]([RevisionNum]) ON [PRIMARY]
GO

 CREATE  UNIQUE  INDEX [rpspn] ON [dbo].[tblParts]([RPSPartNum]) ON [PRIMARY]
GO

 CREATE  INDEX [Vendorname] ON [dbo].[tblParts]([VendorName]) ON [PRIMARY]
GO

 CREATE  INDEX [fkPartID] ON [dbo].[tblPartsModels]([fkPartID]) ON [PRIMARY]
GO

 CREATE  INDEX [fkPartID] ON [dbo].[tblPartsSubParts]([fkPartID]) ON [PRIMARY]
GO

 CREATE  INDEX [SubNum] ON [dbo].[tblPartsSubParts]([SubNum]) ON [PRIMARY]
GO

 CREATE  INDEX [SubSourcePartNum] ON [dbo].[tblPartsSubParts]([SubSourcePartNum]) ON [PRIMARY]
GO

 CREATE  INDEX [Vendor] ON [dbo].[tblPO]([Vendor]) ON [PRIMARY]
GO

 CREATE  INDEX [PO] ON [dbo].[tblPOPart]([fkPOID]) ON [PRIMARY]
GO

 CREATE  INDEX [RPSPN] ON [dbo].[tblPOPart]([RPSPartNum]) ON [PRIMARY]
GO

 CREATE  INDEX [VenPN] ON [dbo].[tblPOPart]([VendorPartNumber]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_tblPOPartDetail_fkPOPartID] ON [dbo].[tblPOPartDetail]([fkPOPartID]) ON [PRIMARY]
GO

 CREATE  INDEX [SchedItemID] ON [dbo].[tblProdSchedItems]([SchedItemID]) ON [PRIMARY]
GO

 CREATE  UNIQUE  INDEX [apkResponse] ON [dbo].[tblResponseMethod]([ResposneID]) ON [PRIMARY]
GO

 CREATE  UNIQUE  INDEX [ResponseMethod] ON [dbo].[tblResponseMethod]([ResponseMethod]) ON [PRIMARY]
GO

 CREATE  INDEX [RPSPartNum] ON [dbo].[tblVendors]([RPSPartNum]) ON [PRIMARY]
GO

 CREATE  INDEX [VendorName] ON [dbo].[tblVendors]([VendorName]) ON [PRIMARY]
GO

 CREATE  INDEX [VendorPartNum] ON [dbo].[tblVendors]([VendorPartNum]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_tblWarranty_Dealer] ON [dbo].[tblWarranty]([Dealer]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_tblWarranty_RGANum] ON [dbo].[tblWarranty]([RGANum]) ON [PRIMARY]
GO

 CREATE  INDEX [fkWarrantyID] ON [dbo].[tblWarrantyParts]([fkWarrantyID]) ON [PRIMARY]
GO

 CREATE  UNIQUE  INDEX [FinishDesc] ON [dbo].[tlkuFinish]([FinishDesc]) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblDealerAdvert] ADD 
	CONSTRAINT [tblDealerAdvert_FK00] FOREIGN KEY 
	(
		[fkDealerID]
	) REFERENCES [dbo].[tblDealers] (
		[DealerID]
	)
GO

ALTER TABLE [dbo].[tblDealersGoals] ADD 
	CONSTRAINT [tblDealersGoals_FK00] FOREIGN KEY 
	(
		[fkDealerID]
	) REFERENCES [dbo].[tblDealers] (
		[DealerID]
	)
GO

ALTER TABLE [dbo].[tblPOPart] ADD 
	CONSTRAINT [tblPODetail_FK00] FOREIGN KEY 
	(
		[fkPOID]
	) REFERENCES [dbo].[tblPO] (
		[POID]
	)
GO

ALTER TABLE [dbo].[tblPOPartDetail] ADD 
	CONSTRAINT [FK_tblPOPartDetail_tblPOPart] FOREIGN KEY 
	(
		[fkPOPartID]
	) REFERENCES [dbo].[tblPOPart] (
		[POPartID]
	)
GO

ALTER TABLE [dbo].[tblWarrantyParts] ADD 
	CONSTRAINT [FK_tblWarrantyParts_tblWarranty] FOREIGN KEY 
	(
		[fkWarrantyID]
	) REFERENCES [dbo].[tblWarranty] (
		[WarrantyID]
	)
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW dlqrptAdvertAlloc
AS
SELECT tblDealers.DealerName, sum(tblDealerAdvert.AdvertAmt) AS SumOfAdvertAmt
FROM tblDealerAdvert RIGHT JOIN tblDealers ON (tblDealerAdvert.fkDealerID=tblDealers.DealerID)
WHERE (((tblDealers.CurrentDealer)=1))
GROUP BY tblDealers.DealerName


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "dlqrptDealerListAddressView"
AS
SELECT tblDealers.DealerName, tblDealers.ContactName, tblDealers.StreetAddress, tblDealers.City, tblDealers.State, tblDealers.Zip, tblDealers.CurrentDealer
FROM tblDealers
WHERE (((tblDealers.CurrentDealer)=1))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "dlqrptDealerListAllDealersView"
AS
SELECT tblDealers.DealerName, tblDealers.ContactName, tblDealers.City, tblDealers.State, tblDealers.Phone, tblDealers.TollFreeNumber
FROM tblDealers


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "dlqrptDealerListFaxView"
AS
SELECT tblDealers.DealerName, tblDealers.ContactName, tblDealers.City, tblDealers.State, tblDealers.Fax, tblDealers.TerritoryCovered, tblDealers.CurrentDealer
FROM tblDealers
WHERE (((tblDealers.CurrentDealer)=1))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "dlqrptDealerListPhoneView"
AS
SELECT tblDealers.DealerName, tblDealers.ContactName, tblDealers.City, tblDealers.State, tblDealers.Phone, tblDealers.TollFreeNumber, tblDealers.CurrentDealer
FROM tblDealers
WHERE (((tblDealers.CurrentDealer)=1))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "dlqrptMaillingLabelServicePartsManView"
AS
SELECT tblDealers.DealerName, tmpNames.PersonName, tblDealers.StreetAddress, tblDealers.City, tblDealers.State, tblDealers.Zip, tblDealers.CurrentDealer, tblDealers.Phone, tblDealers.ContactName
FROM tmpNames INNER JOIN tblDealers ON (tmpNames.DealerIndex=tblDealers.DealerID)
WHERE (((tblDealers.CurrentDealer)=1))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "dlqrptMaillingLabelsView"
AS
SELECT tblDealers.DealerName, tblDealers.ContactName AS Name, tblDealers.ContactName AS PersonName, tblDealers.StreetAddress, tblDealers.City, tblDealers.State, tblDealers.Zip, tblDealers.CurrentDealer, tblDealers.Phone, tblDealers.TollFreeNumber
FROM tblDealers
WHERE (((tblDealers.CurrentDealer)=1))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "dlqrptMaillingServiceView"
AS
SELECT tblDealers.DealerName, tblDealers.ServiceManagerName AS PersonName, tblDealers.StreetAddress, tblDealers.City, tblDealers.State, tblDealers.Zip, tblDealers.CurrentDealer, tblDealers.Phone, tblDealers.ContactName
FROM tblDealers
WHERE (((tblDealers.CurrentDealer)=1))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "dmqrptDealerDemoReportView"
AS
SELECT tblDealers.DealerName, tblDealerDemos.CustomerName, tblDealerDemos.DateOfDemo, tblDealerDemos.Purchased, tblDealerDemos.EquipmentDemoed
FROM tblDealerDemos INNER JOIN tblDealers ON (tblDealerDemos.fkDealerID=tblDealers.DealerID)


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW ldqfrmLeads
AS
SELECT tblLeads.*, tblLeads.Purchased AS Expr1001
FROM tblLeads
WHERE (((tblLeads.Purchased)=0))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "ldqrptMaillingLabelsView"
AS
SELECT tblLeads.Contact, tblLeads.CompanyName, tblLeads.Address, tblLeads.City, tblLeads.State, tblLeads.Zip, tblLeads.LeadDate
FROM tblLeads
WHERE (((tblLeads.Address) Is Not Null) AND ((tblLeads.State) Is Not Null))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "ldqrptResponseMethodView"
AS
SELECT tblLeads.DealerName, tblLeads.CompanyName, tblLeads.LeadDate, tblLeads.Contact, tblLeads.State, tblLeads.Phone, tblLeads.Result, tblLeads.City, tblLeads.ResponseMethod
FROM tblLeads


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "lsqrptStatesDealerListView"
AS
SELECT tblDealers.DealerName, tblLists.LastDateMailed, tblLists.State, tblLists.Flier, tblLists.ListCompany
FROM tblDealers RIGHT JOIN tblLists ON (tblDealers.DealerID=tblLists.fkDealerID)
WHERE (((tblLists.LastDateMailed) Is Not Null))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "lsqrptStatesLastMailedView"
AS
SELECT tblLists.State, tblLists.LastDateMailed, tblLists.Flier, tblDealers.DealerName, tblLists.ListCompany
FROM tblDealers RIGHT JOIN tblLists ON (tblDealers.DealerID=tblLists.fkDealerID)


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "lsqrptStatesNotMailedView"
AS
SELECT tblLists.Flier, tblLists.State, tblLists.LastDateMailed, tblDealers.DealerName, tblLists.ListCompany
FROM tblDealers RIGHT JOIN tblLists ON (tblDealers.DealerID=tblLists.fkDealerID)
WHERE (((tblLists.LastDateMailed) Is Null))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO



CREATE VIEW dbo.maqrptMajorAccountInfoPage
AS
SELECT tblMajorAccounts.*, tblDealers.DealerName, 
    tblDealers.StreetAddress, tblDealers.City, tblDealers.State, 
    tblDealers.Zip
FROM tblDealers INNER JOIN
    tblMajorAccounts ON 
    tblDealers.DealerID = tblMajorAccounts.fkMADealerID



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE VIEW dbo.mlvCompany
AS
SELECT CompID, MailState, AreaCode, MailCounty, ProductDesc, 
    MailZip, PlantSqFt, Employment, MailAddress, Fax, 
    CompName, SICDesc, SICCode, Division, Phone800, HarrisID, 
    Phone, MailCity
FROM fcmail.dbo.tblMailCompany


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE VIEW dbo.mlvContact
AS
SELECT ContactID, fkCompID, ContactName, fkContactTitleID, 
    DoNotMail
FROM fcmail.dbo.tblMailContact


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE VIEW dbo.mlvMaillist
AS
SELECT mlvCompany.CompID, mlvContact.ContactName, 
    mlvContact.fkContactTitleID, mlvCompany.CompName, 
    mlvCompany.MailAddress, mlvCompany.MailCity, 
    mlvCompany.MailState, mlvCompany.MailZip, 
    mlvCompany.MailCounty, mlvCompany.SICCode, 
    mlvCompany.SICDesc, mlvCompany.ProductDesc, 
    mlvCompany.PlantSqFt, mlvCompany.Employment, 
    mlvCompany.AreaCode, mlvCompany.Phone, 
    mlvCompany.Fax, mlvCompany.Phone800
FROM fcmail.dbo.tblMailCompany mlvCompany INNER JOIN
    fcmail.dbo.tblMailContact mlvContact ON 
    mlvCompany.CompID = mlvContact.fkCompID


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "nwqrptAddressListingView"
AS
SELECT tblNewsletter.DealerFlag, tblNewsletter.DealerName, tblNewsletter.Name, tblNewsletter.Salutation, tblNewsletter.StreetAddress, tblNewsletter.City, tblNewsletter.State, tblNewsletter.Zip, tblNewsletter.Phone, tblNewsletter.Fax
FROM tblNewsletter


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "nwqrptMailingLabelsView"
AS
SELECT tblNewsletter.DealerFlag, tblNewsletter.DealerName, tblNewsletter.Name AS PersonName, tblNewsletter.Salutation, tblNewsletter.StreetAddress, tblNewsletter.City, tblNewsletter.State, tblNewsletter.Zip, tblNewsletter.Phone, tblNewsletter.Fax
FROM tblNewsletter


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE VIEW dbo.orqrptAcknowledgement
AS
SELECT tblOrders.OrderID, tblOrders.Dealer, tblOrders.OrderDate, 
    tblOrders.PurchaseOrder, tblOrders.ShipName, 
    tblOrders.StreetAddress, tblOrders.CityStateZip, 
    tblOrders.Quantity, tblOrders.Model, tblModels.Description, 
    tblModels.Price, tblOrders.Options, tblOrders.PromisedDate, 
    tblOrders.ShipVia, tblOrders.OrderNumber, 
    tblOrders.CollectPrepaid, tblOrders.Terms
FROM tblOrders INNER JOIN
    tblModels ON tblOrders.Model = tblModels.Model


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE VIEW dbo.orqrptCustomerLabels
AS
SELECT tblOrders.Name AS CName, tblOrders.Address, 
    tblOrders.City, tblOrders.State, tblOrders.Zip, 
    tblOrders.ContactName
FROM tblOrders
WHERE (((tblOrders.Name) IS NOT NULL) AND ((tblOrders.City) 
    IS NOT NULL))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "paqrptListPricesView"
AS
SELECT tblParts.RPSPartNum, tblParts.PartName, tblParts.SuggestedList, tblParts.Note, tblParts.HideOnReports
FROM tblParts
WHERE (((tblParts.RPSPartNum) Not Like 'V%' And (tblParts.RPSPartNum) Not Like 'E%' And (tblParts.RPSPartNum) Not Like 'H%') And ((tblParts.SuggestedList)>0.01) And ((tblParts.HideOnReports)=0))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW paqrptPartLabels
AS
SELECT tblParts.RPSPartNum, tblParts.PartName, tblParts.VendorName, tblParts.VendorPartName, tblParts.ManfPartNum, Tbl_Numbers.Counter, tblParts.Model
FROM Tbl_Numbers, tblParts


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW paqrptPartLabelsMulti
AS
SELECT tblParts.RPSPartNum, tblParts.PartName, tblParts.VendorName, tblParts.VendorPartName, tblParts.ManfPartNum, Tbl_Numbers.Counter, tblParts.HideOnReports
FROM Tbl_Numbers, tblParts INNER JOIN tmpLabelQtys ON (tblParts.PartID=tmpLabelQtys.PartIndex)
WHERE (((tblParts.RPSPartNum) Not Like 'V%' And (tblParts.RPSPartNum) Not Like 'E%' And (tblParts.RPSPartNum) Not Like 'H%') And ((Tbl_Numbers.Counter)<=tmpLabelQtys.Quantity) And ((tblParts.HideOnReports)=0))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "paqrptPartListFinishView"
AS
SELECT tblParts.RPSPartNum, tblParts.PartName, tblParts.FinishDesc
FROM tblParts
WHERE (((tblParts.FinishDesc) Is Not Null))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE VIEW dbo.paqrptPartsPerModel
AS
SELECT paqrptPartsPerModel2.RPSPartNum, 
    paqrptPartsPerModel2.PartName, 
    paqrptPartsPerModel2.Model, tblPartsModels.Quantity
FROM paqrptPartsPerModel2 LEFT JOIN
    tblPartsModels ON 
    (paqrptPartsPerModel2.Model = tblPartsModels.Model) AND 
    (paqrptPartsPerModel2.PartID = tblPartsModels.fkPartID)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW paqrptPartsPerModel2
AS
SELECT tblParts.PartID, tblParts.RPSPartNum, tblParts.PartName, tblModels.Model
FROM tblParts, tblModels


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "parptRPSPartsPricingView"
AS
SELECT tblParts.RPSPartNum, tblParts.VendorName, tblParts.ManfPartNum, tblParts.SuggestedList, tblParts.DealerNet, tblParts.PartName, tblParts.HideOnReports
FROM tblParts
WHERE (((tblParts.RPSPartNum) Not Like 'V%' And (tblParts.RPSPartNum) Not Like 'E%' And (tblParts.RPSPartNum) Not Like 'H%') And ((tblParts.HideOnReports)=0))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "parqryDealerPartsListView"
AS
SELECT tblParts.RPSPartNum, tblParts.PartName, tblParts.DealerNet, tblParts.SuggestedList, tblParts.Note
FROM tblParts
WHERE (((tblParts.RPSPartNum) Not Like 'V%' And (tblParts.RPSPartNum) Not Like 'E%' And (tblParts.RPSPartNum) Not Like 'H%') And ((tblParts.DealerNet)<>0 And (tblParts.DealerNet) Is Not Null) And ((tblParts.HideOnReports)=0))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE VIEW dbo.ppqfrmProdParts
AS
SELECT tblPO.POID, tblPO.Vendor, tblPO.Confirmed, 
    tblPOPartDetail.ReceivedDate
FROM tblPO INNER JOIN
    tblPOPart ON tblPO.POID = tblPOPart.fkPOID INNER JOIN
    tblPOPartDetail ON 
    tblPOPart.POPartID = tblPOPartDetail.fkPOPartID
WHERE (tblPOPartDetail.ReceivedDate IS NULL)


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE VIEW dbo.ppqrptOpenPOsDetailed
AS
SELECT POA.POID, POPart.POPartID, POA.Vendor, 
    POPart.RPSPartNum, POPart.VendorPartNumber, 
    POPart.PartDescription, PODetail.CostEach, PODetail.Quantity, 
    PODetail.RequestedShipDate, PODetail.ReceivedDate, 
    PODetail.QuantityReceived, PODetail.Value, 
    PODetail.Notes
FROM tblPO POA INNER JOIN
    tblPOPart POPart ON POA.POID = POPart.fkPOID AND 
    POA.POID = POPart.fkPOID INNER JOIN
    tblPOPartDetail PODetail ON 
    POPart.POPartID = PODetail.fkPOPartID
WHERE (PODetail.RequestedShipDate IS NOT NULL) AND 
    (PODetail.ReceivedDate IS NULL) AND (POA.POID IN
        (SELECT A.POID
      FROM dbo.tblPO A INNER JOIN
           dbo.tblPOPart B ON A.POID = B.fkPOID AND 
           A.POID = B.fkPOID INNER JOIN
           dbo.tblPOPartDetail C ON 
           B.POPartID = C.fkPOPartID
      WHERE (C.RequestedShipDate IS NOT NULL) AND 
           (C.ReceivedDate IS NULL)))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW dbo.qselContactTitle
AS
SELECT ContactTitle
FROM tlkuContactTitles



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE VIEW dbo.tblLeads
AS
SELECT tblAllLeads.*
FROM tblAllLeads
WHERE (Purchased = 0)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE VIEW dbo.tblPurLeads
AS
SELECT tblAllLeads.*
FROM tblAllLeads
WHERE (Purchased = 1)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE VIEW dbo.tempView
AS
SELECT tblParts.RPSPartNum, tblParts.PartName, 
    tblParts.DealerNet AS [Dealer Net Price], 
    tblParts.SuggestedList AS [Suggested List Price], 
    tblParts.QtyReq AS [Quantity Required], tblParts.Notes, 
    tblParts.PartID
FROM tblParts
WHERE (((tblParts.DealerNet) > 0))

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "veqrptVendorLabelsView"
AS
SELECT tblVendors.CointactName, tblVendors.VendorName, tblVendors.StreetAddress, (CITY+', '+STATE+'  '+ZIP) AS "City Address"
FROM tblVendors
WHERE (((tblVendors.Active)=1))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE VIEW dbo.vSwitchboard
AS
SELECT [Switchboard Items].*
FROM [Switchboard Items]

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE VIEW dbo.waqfrmWarranty
AS
SELECT WarrantyID, MachineSerialNumber, CreditMemoNum, 
    CreditMemoAmt, DateOfFailure, Dealer, Customer, RGANum, 
    PartCost, LaborCost, Freight, Model, Problem, Resolution, 
    WarrantyOpen, Travel, Policy, DateEntered, PartReceived, 
    Hours, fkDealerID, Comment
FROM tblWarranty


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO

CREATE VIEW dbo.waqrptRgaClaim
AS
SELECT tblDealers.DealerName, tblDealers.StreetAddress, 
    tblDealers.City, tblDealers.State, tblDealers.Zip, 
    tblWarranty.Customer, tblWarranty.DateOfFailure, 
    tblWarranty.RGANum, tblWarranty.WarrantyID, 
    tblWarranty.Comment, tblWarranty.Problem, 
    tblWarranty.Resolution, tblWarranty.MachineSerialNumber, 
    tblWarranty.DateEntered
FROM tblDealers INNER JOIN
    tblWarranty ON tblDealers.DealerID = tblWarranty.fkDealerID

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE VIEW dbo.waqrptWarrantyPending
AS
SELECT dbo.tblWarranty.Dealer, 
    dbo.tblWarranty.MachineSerialNumber, 
    dbo.tblWarranty.RGANum, 
    dbo.tblWarrantyParts.PartNumReplaced, 
    dbo.tblWarranty.WarrantyOpen, dbo.tblWarranty.Hours
FROM dbo.tblWarranty INNER JOIN
    dbo.tblWarrantyParts ON 
    dbo.tblWarranty.WarrantyID = dbo.tblWarrantyParts.fkWarrantyID
WHERE (dbo.tblWarranty.WarrantyOpen = 1)


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE dlqfrmDealerGoals
@DealerID int
AS
SELECT fkDealerID, [Year], Model, Goal
FROM tblDealersGoals
WHERE fkDealerID = @DealerID
ORDER BY [Year] DESC, Model





GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqfrmDealerGoals]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE Procedure dlqrptDealerContractAlpha
/*
	(
		@parameter1 datatype = default value,
		@parameter2 datatype OUTPUT
	)
*/
As
SELECT DealerName, ContactName, StreetAddress, City, State, Zip, CurrentDealer, ContractExpires
FROM tblDealers
WHERE (CurrentDealer = 1)
ORDER BY DealerName, ContactName	

	return 






GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptDealerContractAlpha]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE Procedure dlqrptDealerContractDate
/*
	(
		@parameter1 datatype = default value,
		@parameter2 datatype OUTPUT
	)
*/
As
SELECT DealerName, ContactName, StreetAddress, City, State, Zip, CurrentDealer, ContractExpires
FROM tblDealers
WHERE (CurrentDealer = 1)
ORDER BY ContractExpires, DealerName, ContactName	

	return 






GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptDealerContractDate]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE dlqrptDealerListAddress
AS
SELECT * FROM "dlqrptDealerListAddressView"
ORDER BY "dlqrptDealerListAddressView".DealerName, "dlqrptDealerListAddressView".ContactName


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptDealerListAddress]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE dlqrptDealerListAllDealers
AS
SELECT * FROM "dlqrptDealerListAllDealersView"
ORDER BY "dlqrptDealerListAllDealersView".DealerName, "dlqrptDealerListAllDealersView".ContactName


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptDealerListAllDealers]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE dlqrptDealerListFax
AS
SELECT * FROM "dlqrptDealerListFaxView"
ORDER BY "dlqrptDealerListFaxView".DealerName, "dlqrptDealerListFaxView".ContactName


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptDealerListFax]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE dlqrptDealerListPhone
AS
SELECT * FROM "dlqrptDealerListPhoneView"
ORDER BY "dlqrptDealerListPhoneView".DealerName, "dlqrptDealerListPhoneView".ContactName


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptDealerListPhone]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE dlqrptMaillingLabels
AS
SELECT * FROM "dlqrptMaillingLabelsView"
ORDER BY "dlqrptMaillingLabelsView".DealerName


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptMaillingLabels]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE dlqrptMaillingLabelServicePartsMan
AS
SELECT * FROM "dlqrptMaillingLabelServicePartsManView"
ORDER BY "dlqrptMaillingLabelServicePartsManView".DealerName


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptMaillingLabelServicePartsMan]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE dlqrptMaillingService
AS
SELECT * FROM "dlqrptMaillingServiceView"
ORDER BY "dlqrptMaillingServiceView".DealerName


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptMaillingService]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE dmqrptDealerDemoReport
AS
SELECT * FROM "dmqrptDealerDemoReportView"
ORDER BY "dmqrptDealerDemoReportView".CustomerName


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dmqrptDealerDemoReport]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE ldpqrptDirectMailLead
	@sFromDate varchar(20),
	@sToDate varchar(20),
	@sDealerName varchar(50)
AS

DECLARE @sSQL varchar(1000)
DECLARE @sWhere varchar(200)
SELECT @sWhere = ''

SELECT @sSQL = '
SELECTDealerName, Salesman, Location, CompanyName, LeadDate, Contact, Address, City, State, Zip, ApplicationNotes, Result, ContactTitle, Phone
FROM tblPurLeads'

IF @sFromDate IS NOT NULL OR @sToDate IS NOT NULL OR @sDealerName IS NOT NULL
  BEGIN
	IF @sFromDate IS NOT NULL AND @sToDate  IS NOT NULL 
	  BEGIN
		SELECT @sWhere = 'LeadDate Between ''' + @sFromDate + ''' And ''' + @sToDAte + ''' '
	  END
  	ELSE IF @sFromDate IS NOT NULL
	  BEGIN
		SELECT @sWhere =  'LeadDate > ' + '''' + @sFromDate + ''''	
	  END
	ELSE IF @sToDate IS NOT NULL
	  BEGIN
		SELECT @sWhere =  'LeadDate > ' + '''' + @sToDate + ''''	
	  END
	IF @sDealerName IS NOT NULL
	  BEGIN
		IF @sWhere <> ''
		  BEGIN
			SELECT @sWhere = @sWhere + ' AND '
		  END
		SELECT @sWhere = @sWhere + ' DealerName = ''' + @sDealerName + ''''
	  END
	SELECT @sSQL = @sSQL + ' WHERE ' + @sWhere
  END

EXEC (@sSQL)



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ldpqrptDirectMailLead]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE ldqrptDirectMailLead
	@sFromDate varchar(20),
	@sToDate varchar(20),
	@sDealerName varchar(50)
AS

DECLARE @sSQL varchar(1000)
DECLARE @sWhere varchar(200)
SELECT @sWhere = ''

SELECT @sSQL = '
SELECT tblLeads.DealerName, tblLeads.Salesman, tblLeads.Location, tblLeads.CompanyName, tblLeads.LeadDate, tblLeads.Contact, tblLeads.Address, tblLeads.City, tblLeads.State, tblLeads.Zip, tblLeads.ApplicationNotes, tblLeads.Result, tblLeads.ContactTitle, tblLeads.Phone
FROM tblLeads'

IF @sFromDate IS NOT NULL OR @sToDate IS NOT NULL OR @sDealerName IS NOT NULL
  BEGIN
	IF @sFromDate IS NOT NULL AND @sToDate  IS NOT NULL 
	  BEGIN
		SELECT @sWhere = 'LeadDate Between ''' + @sFromDate + ''' And ''' + @sToDAte + ''' '
	  END
  	ELSE IF @sFromDate IS NOT NULL
	  BEGIN
		SELECT @sWhere =  'LeadDate > ' + '''' + @sFromDate + ''''	
	  END
	ELSE IF @sToDate IS NOT NULL
	  BEGIN
		SELECT @sWhere =  'LeadDate > ' + '''' + @sToDate + ''''	
	  END
	IF @sDealerName IS NOT NULL
	  BEGIN
		IF @sWhere <> ''
		  BEGIN
			SELECT @sWhere = @sWhere + ' AND '
		  END
		SELECT @sWhere = @sWhere + ' DealerName = ''' + @sDealerName + ''''
	  END
	SELECT @sSQL = @sSQL + ' WHERE ' + @sWhere
  END

EXEC (@sSQL)


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ldqrptDirectMailLead]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE  ldqrptLeadsReports  AS

SELECT DealerName, CompanyName, LeadDate, Contact, State, Phone, Result, City, 
	ActiveInactive, substring(Phone, 1, 3) AS AreaCode
FROM tblLeads
ORDER BY DealerName



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ldqrptLeadsReports]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE ldqrptMaillingLabels
AS
SELECT * FROM "ldqrptMaillingLabelsView"
ORDER BY "ldqrptMaillingLabelsView".Zip


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ldqrptMaillingLabels]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE ldqrptMaillingLabelsDate @Enter_Starting_Date varchar (255)
, @Enter_Ending_Date varchar (255)
AS 
SELECT tblLeads.Contact, tblLeads.CompanyName, tblLeads.Address, tblLeads.City, tblLeads.State, tblLeads.Zip, tblLeads.LeadDate
FROM tblLeads
WHERE (((tblLeads.Address) Is Not Null) AND ((tblLeads.State) Is Not Null) AND ((tblLeads.LeadDate) Between @Enter_Starting_Date And @Enter_Ending_Date))
ORDER BY tblLeads.Zip


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ldqrptMaillingLabelsDate]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE ldqrptResponseMethod
AS
SELECT * FROM "ldqrptResponseMethodView"
ORDER BY "ldqrptResponseMethodView".DealerName


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ldqrptResponseMethod]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE lsqrptStatesDealerList
AS
SELECT * FROM "lsqrptStatesDealerListView"
ORDER BY "lsqrptStatesDealerListView".DealerName, "lsqrptStatesDealerListView".LastDateMailed DESC


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[lsqrptStatesDealerList]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE lsqrptStatesLastMailed
AS
SELECT * FROM "lsqrptStatesLastMailedView"
ORDER BY "lsqrptStatesLastMailedView".State, "lsqrptStatesLastMailedView".LastDateMailed DESC


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[lsqrptStatesLastMailed]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE lsqrptStatesNotMailed
AS
SELECT * FROM "lsqrptStatesNotMailedView"
ORDER BY "lsqrptStatesNotMailedView".Flier, "lsqrptStatesNotMailedView".State


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[lsqrptStatesNotMailed]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE nwqrptAddressListing
AS
SELECT * FROM "nwqrptAddressListingView"
ORDER BY "nwqrptAddressListingView".DealerName, "nwqrptAddressListingView".Name


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[nwqrptAddressListing]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE nwqrptMailingLabels
AS
SELECT * FROM "nwqrptMailingLabelsView"
ORDER BY "nwqrptMailingLabelsView".DealerName


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[nwqrptMailingLabels]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE orqrptEndUserLabels
AS
SELECT * FROM "orqrptEndUserLabelsView"
ORDER BY "orqrptEndUserLabelsView".Name


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orqrptEndUserLabels]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE orqrptEndUserState @Please_Enter_State varchar (255)
AS 
SELECT tblOrders.Model, tblOrders.SaleDate, tblOrders.Name, tblOrders.Address, tblOrders.City, tblOrders.State, tblOrders.Zip, tblOrders.ContactName, tblOrders.Phone
FROM tblOrders
WHERE (((tblOrders.State)=@Please_Enter_State))
ORDER BY tblOrders.SaleDate DESC


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orqrptEndUserState]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE orqrptPrepSheet
AS
SELECT * FROM "orqrptPrepSheetView"
ORDER BY "orqrptPrepSheetView".Model, "orqrptPrepSheetView".PromisedDate, "orqrptPrepSheetView".OrderNumber


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orqrptPrepSheet]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptDealerSales 
	@sDealerName varchar(40),
	@sFromDate varchar(20),
	@sToDate varchar(20)
AS
DECLARE @sSQL varchar(800)
SELECT @sSQL = 'SELECT tblOrders.Dealer, tblOrders.Model, tblOrders.OrderDate, 
		    tblOrders.ShippedDate, tblOrders.Quantity, 
		    tblOrders.SalePrice, tblOrders.OrderNumber, tblOrders.SerialNumber
	FROM tblOrders INNER JOIN
		    tblDealers ON 
		    tblOrders.Dealer = tblDealers.DealerName
	WHERE (tblDealers.CurrentDealer = 1) AND SalePrice <> 0 '
IF @sDealerName IS NOT NULL 
  BEGIN
	SELECT @sSQL = @sSQL + 'AND tblOrders.Dealer = ''' +  @sDealerName + ''' '
  END
IF @sFromDate IS NOT NULL And @sToDate IS NOT NULL
  BEGIN
	SELECT @sSQL = @sSQL + 'AND tblOrders.OrderDate Between ''' + @sFromDate + ''' And ''' + @sToDate + ''' '
  END
ELSE IF @sFromDate IS NOT NULL 
  BEGIN
	SELECT @sSQL = @sSQL + 'AND tblOrders.OrderDate > ''' + @sFromDate + ''' '
  END
ELSE IF @sToDate IS NOT NULL 
  BEGIN
	SELECT @sSQL = @sSQL + 'AND tblOrders.OrderDate < ''' + @sToDate + ''' '
  END
SELECT @sSQL = @sSQL + 'GROUP BY tblOrders.Dealer, tblOrders.Model, tblOrders.OrderDate, tblOrders.ShippedDate, tblOrders.Quantity, 
				tblOrders.SalePrice, tblOrders.OrderNumber, tblOrders.SerialNumber '
SELECT @sSQL = @sSQL + 'ORDER BY tblOrders.Dealer, tblOrders.Model, tblOrders.OrderDate '
EXEC (@sSQL)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptDealerSales]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE orsprptMajorAccountSales
	@sMAName varchar(40),
	@sFromDate varchar(20),
	@sToDate varchar(20)
AS

DECLARE @sSQL varchar(1500)
DECLARE @sWhere varchar(300)
SELECT @sWhere = ''

SELECT @sSQL = '
SELECT dbo.tblMajorAccounts.MACompName, dbo.tblMajorAccounts.MAHeadqAddress, dbo.tblMajorAccounts.MACity, dbo.tblMajorAccounts.MAState, 
    dbo.tblMajorAccounts.MAZip, dbo.tblOrders.SaleDate, dbo.tblOrders.Name, dbo.tblOrders.Address, dbo.tblOrders.City, dbo.tblOrders.State, dbo.tblOrders.Zip, 
    dbo.tblOrders.Dealer, dbo.tblOrders.Model, dbo.tblOrders.SalePrice, dbo.tblOrders.OrderDate
FROM dbo.tblMajorAccounts INNER JOIN dbo.tblOrders ON 
    dbo.tblMajorAccounts.MajorAccountID = dbo.tblOrders.MajorAccountID
'
IF @sMAName IS NOT NULL OR @sFromDate IS NOT NULL OR @sToDate IS NOT NULL
  BEGIN
	IF @sFromDate IS NOT NULL And @sToDate IS NOT NULL
	  BEGIN
		SELECT @sWhere = 'tblOrders.OrderDate Between ''' + @sFromDate + ''' And ''' + @sToDate + ''' '
	  END
	ELSE IF @sFromDate IS NOT NULL 
	  BEGIN
		SELECT @sWhere = 'tblOrders.OrderDate > ''' + @sFromDate + ''' '
	  END
	ELSE IF @sToDate IS NOT NULL 
	  BEGIN
		SELECT @sWhere = 'tblOrders.OrderDate < ''' + @sToDate + ''' '
	  END
	IF @sMAName IS NOT NULL 
	  BEGIN
		IF @sWhere <> ''
		  BEGIN
			SELECT @sWhere = @sWhere + ' AND '
		  END
		SELECT @sWhere = 'dbo.tblMajorAccounts.MACompName = ''' +  @sMAName + ''' '
	  END
	SELECT @sSQL = @sSQL + ' WHERE ' + @sWhere
END

EXEC (@sSQL)



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptMajorAccountSales]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE Procedure dbo.orsprptSalesModel
	(
		@sFromDate varchar(20),
		@sToDate varchar(20)
	)

AS 

DECLARE @sSQL nvarchar(1000)

SELECT @sSQL = 'SELECT Model, Dealer, Sum(Quantity) AS Quantity, Sum(SalePrice) AS SalePrice, 
	Sum(CostPrice) AS CostPrice, Sum(Margin) AS Margin
	FROM tblOrders
	WHERE SalePrice <> 0'

-- check dates
IF @sFromDate IS NOT NULL OR @sToDate IS NOT NULL
  BEGIN
	IF @sFromDate IS NOT NULL AND @sToDate  IS NOT NULL 
	  BEGIN
		SELECT @sSQL = @sSQL + ' AND OrderDate Between ''' + @sFromDate + ''' And ''' + @sToDAte + ''' '
	  END
  	ELSE IF @sFromDate IS NOT NULL
	  BEGIN
		SELECT @sSQL = @sSQL + ' AND OrderDate > ' + '''' + @sFromDate + ''''	
	  END
	ELSE IF @sToDate IS NOT NULL
	  BEGIN
		SELECT @sSQL = @sSQL + ' AND OrderDate > ' + '''' + @sToDate + ''''	
	  END
  END

SELECT @sSQL = @sSQL + ' GROUP BY Model, Dealer'
EXEC (@sSQL)

return 




















GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptSalesModel]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE paqrptListPrices
AS
SELECT * FROM "paqrptListPricesView"
ORDER BY "paqrptListPricesView".RPSPartNum


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[paqrptListPrices]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE paqrptPartListFinish
AS
SELECT * FROM "paqrptPartListFinishView"
ORDER BY "paqrptPartListFinishView".RPSPartNum, "paqrptPartListFinishView".PartName


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[paqrptPartListFinish]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE paqryExportDealerParts
AS
SELECT tblParts.RPSPartNum, tblParts.PartName, tblParts.DealerNet AS "Dealer Net Price", tblParts.SuggestedList AS "Suggested List Price", tblParts.QtyReq AS "Quantity Required", tblParts.Notes, tblParts.PartID
FROM tblParts
WHERE (((tblParts.DealerNet)>0))
ORDER BY tblParts.PartName



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[paqryExportDealerParts]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE parptRPSPartsPricing
AS
SELECT * FROM "parptRPSPartsPricingView"
ORDER BY "parptRPSPartsPricingView".RPSPartNum


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[parptRPSPartsPricing]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE parqryDealerPartsList
AS
SELECT * FROM "parqryDealerPartsListView"
ORDER BY "parqryDealerPartsListView".RPSPartNum


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[parqryDealerPartsList]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE pasprptBillOfMaterials
	@sVendorName varchar(50),
	@sModel varchar(10)

 AS

DECLARE @sSQL varchar(1500)
DECLARE @sWhere varchar(500)
SELECT @sWhere = ''

SELECT @sSQL = 
'SELECT dbo.tblParts.RPSPartNum, dbo.tblParts.PartName, dbo.tblParts.VendorName, dbo.tblParts.CostEach, dbo.tblParts.Quantity, 
	CONVERT (smallmoney, CONVERT (smallmoney, dbo.tblParts.CostEach) * CONVERT (int, dbo.tblParts.Quantity)) AS ExtCost 
FROM dbo.tblParts INNER JOIN dbo.tblPartsModels ON dbo.tblParts.PartID = dbo.tblPartsModels.fkPartID '

-- Check model
IF @sModel IS NOT NULL
  BEGIN
	SELECT @sWhere = ' dbo.tblParts.Model = ' + @sModel + ' '
  END

-- Check vendor
IF @sVendorName IS NOT NULL
  BEGIN
	IF @sWhere <> '' 
	  BEGIN
		SELECT @sWhere = @sWhere + ' AND '
	  END
	SELECT @sWhere = @sWhere +  ' dbo.tblParts.VendorName = ''' + @sVendorName + ''''
	SELECT @sWhere = @sWhere + ' AND dbo.tblParts.RPSPartNum IN ('
	SELECT @sWhere = @sWhere + '
		SELECT dbo.tblPOPart.RPSPartNum
		FROM dbo.tblPO INNER JOIN
		    dbo.tblPOPart ON dbo.tblPO.POID = dbo.tblPOPart.fkPOID INNER JOIN
		    dbo.tblPOPartDetail ON dbo.tblPOPart.POPartID = dbo.tblPOPartDetail.fkPOPartID
		WHERE (dbo.tblPOPartDetail.RequestedShipDate IS NOT NULL) AND 
		    (dbo.tblPOPartDetail.ReceivedDate IS NULL)  )'
  END

IF @sWhere <> ''
  BEGIN
	SELECT @sSQL = @sSQL + ' WHERE ' + @sWhere
  END

EXEC (@sSQL)











GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[pasprptBillOfMaterials]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE pasprsubBillOfMaterialsPParts
	@sVendorName varchar(50)
 AS

DECLARE @sSQL varchar(1500)
DECLARE @sWhere varchar(500)
SELECT @sWhere = ''

SELECT @sSQL = '
SELECT dbo.tblPOPart.RPSPartNum, dbo.tblPO.POID, dbo.tblPOPart.VendorPartNumber, dbo.tblPO.Vendor, 
	dbo.tblPOPartDetail.RequestedShipDate, dbo.tblPOPartDetail.CostEach, dbo.tblPOPartDetail.Value, 
	dbo.tblPOPartDetail.Quantity
FROM dbo.tblPO INNER JOIN dbo.tblPOPart ON dbo.tblPO.POID = dbo.tblPOPart.fkPOID INNER JOIN
	dbo.tblPOPartDetail ON dbo.tblPOPart.POPartID = dbo.tblPOPartDetail.fkPOPartID
WHERE (dbo.tblPOPartDetail.RequestedShipDate IS NOT NULL) AND 
	(dbo.tblPOPartDetail.ReceivedDate IS NULL)'

-- Check vendor
IF @sVendorName IS NOT NULL
  BEGIN
	SELECT @sSQL = @sSQL +  ' dbo.tblParts.VendorName = ''' + @sVendorName + ''''
  END

EXEC (@sSQL)











GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[pasprsubBillOfMaterialsPParts]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE ppqrptPOCosts
	@sToDate varchar(20),
	@sPOID varchar(20)
AS
DECLARE @sSQL varchar(1200)
DECLARE @sWhere varchar(300)
SELECT @sWhere = '(dbo.tblPOPartDetail.Quantity <> 0)'
IF @sToDate IS NOT NULL
  BEGIN
	SELECT @sWhere = @sWhere + ' And dbo.tblPOPartDetail.RequestedShipDate < ''' + @sToDate + ''''
  END
IF @sPOID IS NOT NULL
  BEGIN
	SELECT @sWhere = @sWhere + ' And dbo.tblPO.POID = ' + @sPOID
  END
SELECT @sSQL = 
	'SELECT dbo.tblPO.POID, dbo.tblPOPart.RPSPartNum, dbo.tblPOPart.VendorPartNumber, dbo.tblPOPart.PartDescription, 
		SUM(dbo.tblPOPartDetail.CostEach) AS CostEach 
	FROM dbo.tblPO INNER JOIN dbo.tblPOPart ON dbo.tblPO.POID = dbo.tblPOPart.fkPOID AND 
		dbo.tblPO.POID = dbo.tblPOPart.fkPOID INNER JOIN dbo.tblPOPartDetail ON dbo.tblPOPart.POPartID = 
		dbo.tblPOPartDetail.fkPOPartID 
	WHERE ' + @sWhere + '
	GROUP BY dbo.tblPO.POID, dbo.tblPOPart.RPSPartNum, 	dbo.tblPOPart.VendorPartNumber, dbo.tblPOPart.PartDescription'
EXEC (@sSQL)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ppqrptPOCosts]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE ppqrptVendorRelease
AS
SELECT * FROM "ppqrptVendorReleaseView"
ORDER BY "ppqrptVendorReleaseView".RPSPartNum, "ppqrptVendorReleaseView".RequestedShipDate


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ppqrptVendorRelease]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE ppsprptCashFlow
	@sVendorName varchar(40),
	@sFromDate varchar(20),
	@sToDate varchar(20)
 AS

DECLARE @sSQL varchar(1200)
DECLARE @sWhere varchar(300)
SELECT @sSQL = 'SELECT dbo.tblPOPartDetail.RequestedShipDate, dbo.tblPOPart.VendorPartNumber, dbo.tblPO.Vendor, 
		dbo.tblPOPart.RPSPartNum, dbo.tblPOPartDetail.CostEach, dbo.tblPOPartDetail.Quantity, dbo.tblPOPartDetail.Value, 
		dbo.tblPOPartDetail.ReceivedDate, dbo.tblPO.POID 
		FROM dbo.tblPO INNER JOIN dbo.tblPOPart ON dbo.tblPO.POID = dbo.tblPOPart.fkPOID INNER JOIN dbo.tblPOPartDetail 
			ON dbo.tblPOPart.POPartID = dbo.tblPOPartDetail.fkPOPartID 
		WHERE ReceivedDate IS NULL '

-- check dates
IF @sFromDate IS NOT NULL OR @sToDate IS NOT NULL OR @sVendorName IS NOT NULL
  BEGIN
	IF @sFromDate IS NOT NULL AND @sToDate  IS NOT NULL 
	  BEGIN
		SELECT @sWhere = ' AND dbo.tblPOPartDetail.RequestedShipDate Between ''' + @sFromDAte + ''' And ''' + @sToDAte + ''' '
	  END
  	ELSE IF @sFromDate IS NOT NULL
	  BEGIN
		SELECT @sWhere = ' AND dbo.tblPOPartDetail.RequestedShipDate > ' + '''' + @sFromDate + ''''	
	  END
	ELSE IF @sToDate IS NOT NULL
	  BEGIN
		SELECT @sWhere = ' AND dbo.tblPOPartDetail.RequestedShipDate > ' + '''' + @sToDate + ''''	
	  END
	IF @sVendorName IS NOT NULL
	  BEGIN
		IF @sWhere IS NULL
		  BEGIN
			SELECT @sWhere = ''
		  END
		SELECT @sWhere = @sWhere + ' AND Vendor = ''' + @sVendorName + ''''
	  END
	SELECT @sSQL = @sSQL + @sWhere 
  END


EXEC (@sSQL)









GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ppsprptCashFlow]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE ppsprptProdReleases
	@sToDate varchar(20)
AS
DECLARE @sSQL varchar(1000)
SELECT @sSQL = '
SELECT dbo.tblPO.POID, dbo.tblPO.Vendor, dbo.tblPOPart.VendorPartNumber, dbo.tblPOPart.RPSPartNum, dbo.tblPOPart.PartDescription, 
	dbo.tblPOPartDetail.RequestedShipDate, dbo.tblPOPartDetail.Quantity, dbo.tblPOPartDetail.Value, dbo.tblPOPartDetail.ReceivedDate, 
	dbo.tblPOPartDetail.CostEach, dbo.tblPOPartDetail.CostEach * dbo.tblPOPartDetail.Quantity AS ExtCost,
	DATEPART(yyyy, dbo.tblPOPartDetail.RequestedShipDate) AS ShipYear, DATEPART(wk, dbo.tblPOPartDetail.RequestedShipDate) AS ShipWeek
FROM dbo.tblPO INNER JOIN dbo.tblPOPart ON dbo.tblPO.POID = dbo.tblPOPart.fkPOID 
	INNER JOIN dbo.tblPOPartDetail ON dbo.tblPOPart.POPartID = dbo.tblPOPartDetail.fkPOPartID 
WHERE (dbo.tblPOPartDetail.ReceivedDate IS NULL)'
IF @sToDate IS NOT NULL 
  BEGIN
	SELECT @sSQL = @sSQL + ' AND (dbo.tblPOPartDetail.RequestedShipDate < ''' + @sToDate + ''')'
  END
EXEC (@sSQL)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ppsprptProdReleases]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE ppsprptReceivingReport
	@sVendorName varchar(40), 
	@sPOID varchar(40), 
	@iOpenFlag varchar(2)
AS

DECLARE @sSQL varchar(1300)
DECLARE @sWhere varchar(300)
SELECT @sWhere = ''

SELECT @sSQL = 
'SELECT dbo.tblPO.Vendor, dbo.tblPO.POID, dbo.tblPOPart.VendorPartNumber, dbo.tblPOPart.RPSPartNum, 
	dbo.tblPOPart.PartDescription, dbo.tblPOPartDetail.RequestedShipDate, dbo.tblPOPartDetail.Quantity, 
	dbo.tblPOPartDetail.ReceivedDate, dbo.tblPOPartDetail.QuantityReceived 
FROM dbo.tblPO INNER JOIN dbo.tblPOPart ON dbo.tblPO.POID = dbo.tblPOPart.fkPOID AND dbo.tblPO.POID = dbo.tblPOPart.fkPOID 
	INNER JOIN dbo.tblPOPartDetail ON dbo.tblPOPart.POPartID = dbo.tblPOPartDetail.fkPOPartID '

IF @iOpenFlag = 1 
  BEGIN
	SELECT @sWhere = 'ReceivedDate IS NULL'
  END

IF @sVendorName IS NOT NULL
  BEGIN
	IF @sWhere <> ''
	  BEGIN
		SELECT @sWhere = @sWhere + ' AND '
	  END
	SELECT @sWhere = @sWhere + 'Vendor = ''' + @sVendorName + ''''
  END

IF @sPOID IS NOT NULL
  BEGIN
	IF @sWhere <> ''
	  BEGIN
		SELECT @sWhere = @sWhere + ' AND '
	  END
	SELECT @sWhere = @sWhere + 'POID = ' + @sPOID
  END

IF @sWhere <> ''
  BEGIN
	SELECT @sSQL = @sSQL + ' WHERE ' + @sWhere
  END

EXEC (@sSQL)




GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ppsprptReceivingReport]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE ppsprptVendorRelease
	@sPOID varchar(20),
	@sRPSPartNum varchar(40),
	@sVendorPartNum varchar(40),
	@iOpenFlag varchar(2)
AS

DECLARE @sSQL varchar(1300)
DECLARE @sWhere varchar(300)

SELECT @sSQL = '
SELECT dbo.tblPO.POID, dbo.tblPO.Vendor, dbo.tblPOPart.VendorPartNumber, dbo.tblPOPart.RPSPartNum, 
	dbo.tblPOPart.PartDescription, dbo.tblPOPartDetail.RequestedShipDate, dbo.tblPOPartDetail.Quantity, 
	dbo.tblPOPartDetail.Value, dbo.tblPOPartDetail.ReceivedDate 
FROM dbo.tblPO INNER JOIN dbo.tblPOPart ON dbo.tblPO.POID = dbo.tblPOPart.fkPOID AND 
	dbo.tblPO.POID = dbo.tblPOPart.fkPOID INNER JOIN dbo.tblPOPartDetail ON dbo.tblPOPart.POPartID = dbo.tblPOPartDetail.fkPOPartID 
WHERE (dbo.tblPOPartDetail.RequestedShipDate IS NOT NULL)'

IF @sPOID IS NOT NULL
  BEGIN
	SELECT @sSQL = @sSQL + ' AND dbo.tblPO.POID = ' + @sPOID
  END

IF @sRPSPartNum IS NOT NULL
  BEGIN
	SELECT @sSQL = @sSQL + ' AND dbo.tblPOPart.RPSPartNum = ''' + @sRPSPartNum + ''''
  END

IF @sVendorPartNum IS NOT NULL
  BEGIN
	SELECT @sSQL = @sSQL + ' AND dbo.tblPOPart.VendorPartNumber = ''' + @sVendorPartNum + ''''
  END

IF @iOpenFlag = 1
  BEGIN
	SELECT @sSQL = @sSQL + ' AND dbo.tblPOPartDetail.ReceivedDate IS NULL'
  END

EXEC (@sSQL)


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ppsprptVendorRelease]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE ppsprptVendorShipSched
	@sVendorName varchar(50),
	@sFromDAte varchar(20),
	@sToDAte varchar(20)
AS

DECLARE @sSQL varchar(1300)

SELECT @sSQL = '
SELECT dbo.tblPO.POID, dbo.tblPOPart.VendorPartNumber, 
    dbo.tblPO.Vendor, dbo.tblPOPart.RPSPartNum, 
    dbo.tblPOPartDetail.Quantity, dbo.tblPOPartDetail.CostEach, 
    dbo.tblPOPartDetail.Value, dbo.tblPOPart.PartDescription, 
    dbo.tblPOPartDetail.ReceivedDate, dbo.tblPOPartDetail.RequestedShipDate
FROM dbo.tblPO INNER JOIN
    dbo.tblPOPart ON dbo.tblPO.POID = dbo.tblPOPart.fkPOID AND 
    dbo.tblPO.POID = dbo.tblPOPart.fkPOID INNER JOIN
    dbo.tblPOPartDetail ON 
    dbo.tblPOPart.POPartID = dbo.tblPOPartDetail.fkPOPartID
WHERE (dbo.tblPOPartDetail.ReceivedDate IS NULL)'


IF @sVendorName IS NOT NULL
  BEGIN
	SELECT @sSQL = @sSQL + ' AND Vendor = ''' + @sVendorName + ''''
  END

IF @sFromDate IS NOT NULL And @sToDate IS NOT NULL
  BEGIN
	SELECT @sSQL = @sSQL + 'AND RequestedShipDate Between ''' + @sFromDate + ''' And ''' + @sToDate + ''' '
  END
ELSE IF @sFromDate IS NOT NULL 
  BEGIN
	SELECT @sSQL = @sSQL + 'AND RequestedShipDate > ''' + @sFromDate + ''' '
  END
ELSE IF @sToDate IS NOT NULL 
  BEGIN
	SELECT @sSQL = @sSQL + 'AND RequestedShipDate < ''' + @sToDate + ''' '
  END

EXEC (@sSQL)



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ppsprptVendorShipSched]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE qrptOpenPOsDetailedB
AS
SELECT * FROM "qrptOpenPOsDetailedBView"
ORDER BY "qrptOpenPOsDetailedBView".VendorPOID


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[qrptOpenPOsDetailedB]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE qselDealer
AS
SELECT DealerName
FROM tblDealers
GROUP BY DealerName
ORDER BY DealerName




GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[qselDealer]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE qselDealerID
AS
SELECT DealerID, DealerName, StreetAddress, City
FROM tblDealers
ORDER BY DealerName



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[qselDealerID]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



Create Procedure qselFinishDesc
/*
	(
		@parameter1 datatype = default value,
		@parameter2 datatype OUTPUT
	)
*/
As
	SELECT FinishDesc
	FROM tlkuFinish
	GROUP BY FinishDesc
	return 




GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[qselFinishDesc]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE qselModel
AS
SELECT Model, Description
FROM tblModels
ORDER BY Model



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[qselModel]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



Create Procedure qselOrderID
/*
	(
		@parameter1 datatype = default value,
		@parameter2 datatype OUTPUT
	)
*/
As
	SELECT OrderID, OrderNumber, OrderDate, Dealer
	FROM tblOrders
	ORDER BY tblOrders.OrderNumber;
	return 




GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[qselOrderID]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



Create Procedure qselPartID
/*
	(
		@parameter1 datatype = default value,
		@parameter2 datatype OUTPUT
	)
*/
As
	SELECT PartID, RPSPartNum, PartName
	FROM tblParts
	ORDER BY RPSPartNum
	return 




GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[qselPartID]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO



CREATE Procedure qselPurchaseOrder
/*
	(
		@parameter1 datatype = default value,
		@parameter2 datatype OUTPUT
	)
*/
As
	SELECT POID
	FROM tblPO
	ORDER BY POID

	return 






GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[qselPurchaseOrder]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE qselResponseID
AS
SELECT ResposneID, ResponseMethod
FROM tblResponseMethod
ORDER BY ResponseMethod



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[qselResponseID]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE qselResponseMethod
AS
SELECT ResponseMethod, ResponseMethodNotes
FROM tblResponseMethod
ORDER BY ResponseMethod



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[qselResponseMethod]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE qselRPSPartNum
AS
SELECT RPSPartNum, PartName
FROM tblParts
ORDER BY RPSPartNum



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[qselRPSPartNum]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE qselVendorName
AS
SELECT VendorName
FROM tblVendors
ORDER BY VendorName



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[qselVendorName]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO

CREATE Procedure qselVendorPartNum

As
	SELECT VendorPartNumber
	FROM tblPOPart
	GROUP BY VendorPartNumber
	return 





GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[qselVendorPartNum]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE spDisplayTip
	@sUserName varchar(20),
	@iTipID int
AS

IF EXISTS ( SELECT TipID FROM dbo.tblTipDisable WHERE TipID = @iTipID 
	And UserName = @sUserName )

  BEGIN
	SELECT 1 AS flgDisplay
  END
ELSE
  BEGIN
	SELECT 0 AS flgDisplay
  END







GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[spDisplayTip]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE ut_qry33
AS
SELECT *
FROM tblProdSchedItems
ORDER BY Model



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ut_qry33]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE veqrptVendorLabels
AS
SELECT * FROM "veqrptVendorLabelsView"
ORDER BY "veqrptVendorLabelsView".VendorName


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[veqrptVendorLabels]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE waqrptWarrantyRGANums
AS
SELECT * FROM "waqrptWarrantyRGANumsView"
ORDER BY "waqrptWarrantyRGANumsView".RGANum


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[waqrptWarrantyRGANums]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE wasprptDealerReimburse 
	@sDealerName varchar(40),
	@sFromDate varchar(20),
	@sToDate varchar(20),
	@sOpen varchar(2)
AS
DECLARE @sSQL varchar(1200)
DECLARE @sWhere varchar(200)
SELECT @sWhere = ''
SELECT @sSQL = '
SELECT dbo.tblWarranty.Dealer, dbo.tblWarranty.Model, dbo.tblWarranty.MachineSerialNumber, dbo.tblWarranty.CreditMemoNum, 
	dbo.tblWarranty.CreditMemoAmt, dbo.tblWarranty.RGANum, dbo.tblWarranty.DateOfFailure, dbo.tblWarranty.WarrantyOpen 
FROM dbo.tblWarrantyParts INNER JOIN dbo.tblWarranty ON dbo.tblWarrantyParts.fkWarrantyID = dbo.tblWarranty.WarrantyID'
-- check dates
IF @sFromDate IS NOT NULL OR @sToDate IS NOT NULL OR @sDealerName IS NOT NULL OR @sOpen IS NOT NULL
  BEGIN
	-- check dates
	IF @sFromDate IS NOT NULL AND @sToDate  IS NOT NULL 
	  BEGIN
		SELECT @sWhere =' DateOfFailure Between ''' + @sFromDAte + ''' And ''' + @sToDAte + ''' '
	  END
  	ELSE IF @sFromDate IS NOT NULL
	  BEGIN
		SELECT @sWhere = 'DateOfFailure > ' + '''' + @sFromDate + ''''	
	  END
	ELSE IF @sToDate IS NOT NULL
	  BEGIN
		SELECT @sWhere = 'DateOfFailure > ' + '''' + @sToDate + ''''	
	  END
	-- check dealer
	IF @sDealerName IS NOT NULL
	  BEGIN
		IF @sWhere <> ''
		  BEGIN
			SELECT @sWhere = @sWhere + ' AND '
		  END
		SELECT @sWhere = @sWhere + ' Dealer = ''' + @sDealerName + ''''
	  END
	-- check open flag
	IF @sOpen IS NOT NULL
	  BEGIN
		IF @sWhere <> ''
		  BEGIN
			SELECT @sWhere = @sWhere + ' AND '
		  END
		SELECT @sWhere = @sWhere + ' WarrantyOpen = ' + @sOpen
	  END
	SELECT @sSQL = @sSQL + ' WHERE ' + @sWhere 
  END
EXEC (@sSQL)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptDealerReimburse]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE wasprptRgaClaimDates
	@sFromDate varchar(20),
	@sToDate varchar(20)
AS

DECLARE @sSQL varchar(1000)

SELECT @sSQL = 
'SELECT dbo.tblDealers.DealerName, dbo.tblDealers.StreetAddress, 
    dbo.tblDealers.City, 
	dbo.tblDealers.City + ' + quotename(', ','''') + ' + dbo.tblDealers.State + ' + quotename('  ', '''') + ' + dbo.tblDealers.Zip AS CityStateZip,
    dbo.tblWarranty.Customer, 
    dbo.tblWarranty.DateOfFailure, dbo.tblWarranty.RGANum, 
    dbo.tblWarranty.DateEntered, dbo.tblWarranty.Comment, 
    dbo.tblWarranty.WarrantyID, 
    dbo.tblWarranty.MachineSerialNumber, 
    dbo.tblWarranty.Problem, dbo.tblWarranty.Resolution, 
    dbo.tblDealers.State, dbo.tblDealers.Zip
FROM dbo.tblDealers INNER JOIN
    dbo.tblWarranty ON 
    dbo.tblDealers.DealerID = dbo.tblWarranty.fkDealerID'

IF @sFromDate IS NOT NULL AND @sToDate  IS NOT NULL 
  BEGIN
	SELECT @sSQL = @sSQL + ' WHERE DateEntered Between ''' + @sFromDAte + ''' And ''' + @sToDAte + ''' '
  END
ELSE IF @sFromDate IS NOT NULL
  BEGIN
	SELECT @sSQL = @sSQL + ' WHERE DateEntered > ' + '''' + @sFromDate + ''''	
  END
ELSE IF @sToDate IS NOT NULL
  BEGIN
	SELECT @sSQL = @sSQL + ' WHERE DateEntered > ' + '''' + @sToDate + ''''	
  END

EXEC (@sSQL)




GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptRgaClaimDates]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE wasprptWarrantyCosts 
	@sPartName varchar(40),
	@sFromDate varchar(20),
	@sToDate varchar(20),
	@sOpen varchar(2)
AS
DECLARE @sSQL varchar(1200)
DECLARE @sWhere varchar(200)
SELECT @sWhere = ''
SELECT @sSQL = 'SELECT tblWarranty.MachineSerialNumber, tblWarranty.DateOfFailure, tblWarranty.CreditMemoNum,
		tblWarranty.Dealer, tblWarranty.Customer, tblWarranty.RGANum, tblWarranty.LaborCost, tblWarranty.Freight, 
		tblWarranty.Problem, tblWarrantyParts.PartNumReplaced, tblWarrantyParts.PartDescription, tblWarranty.WarrantyID, 
		tblParts.PartName, tblWarrantyParts.PartCost, tblWarranty.Model, tblWarranty.Hours, tblWarranty.WarrantyOpen
	FROM tblParts RIGHT JOIN (tblWarrantyParts INNER JOIN tblWarranty ON 
		tblWarrantyParts.fkWarrantyID = tblWarranty.WarrantyID) ON tblParts.PartID = tblWarrantyParts.PartFileIndex '
-- check dates
IF @sFromDate IS NOT NULL OR @sToDate IS NOT NULL OR @sPartName IS NOT NULL OR @sOpen IS NOT NULL
  BEGIN
	-- check dates
	IF @sFromDate IS NOT NULL AND @sToDate  IS NOT NULL 
	  BEGIN
		SELECT @sWhere =' DateOfFailure Between ''' + @sFromDAte + ''' And ''' + @sToDAte + ''' '
	  END
  	ELSE IF @sFromDate IS NOT NULL
	  BEGIN
		SELECT @sWhere = 'DateOfFailure > ' + '''' + @sFromDate + ''''	
	  END
	ELSE IF @sToDate IS NOT NULL
	  BEGIN
		SELECT @sWhere = 'DateOfFailure > ' + '''' + @sToDate + ''''	
	  END
	-- check part
	IF @sPartName IS NOT NULL
	  BEGIN
		IF @sWhere <> ''
		  BEGIN
			SELECT @sWhere = @sWhere + ' AND '
		  END
		SELECT @sWhere = @sWhere + ' PartNumReplaced = ''' + @sPartName + ''''
	  END
	-- check open flag
	IF @sOpen IS NOT NULL
	  BEGIN
		IF @sWhere <> ''
		  BEGIN
			SELECT @sWhere = @sWhere + ' AND '
		  END
		SELECT @sWhere = @sWhere + ' WarrantyOpen = ' + @sOpen
	  END
	SELECT @sSQL = @sSQL + ' WHERE ' + @sWhere 
  END
EXEC (@sSQL)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptWarrantyCosts]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE wasprptWarrantyTotalCost
	@sDealerName varchar(40),
	@sFromDate varchar(20),
	@sToDate varchar(20),
	@sOpen varchar(2)
AS
DECLARE @sSQL varchar(1200)
DECLARE @sWhere varchar(200)
SELECT @sWhere = ''
SELECT @sSQL = '
SELECT dbo.tblWarranty.Dealer, dbo.tblWarranty.Model, dbo.tblWarranty.MachineSerialNumber, dbo.tblWarranty.DateOfFailure, dbo.tblWarranty.LaborCost, 
	SUM(dbo.tblWarrantyParts.PartCost) AS ExtPartCost, dbo.tblWarranty.Travel, dbo.tblWarranty.Hours
FROM dbo.tblWarranty INNER JOIN dbo.tblWarrantyParts ON dbo.tblWarranty.WarrantyID = dbo.tblWarrantyParts.fkWarrantyID '
-- check dates
IF @sFromDate IS NOT NULL OR @sToDate IS NOT NULL OR @sDealerName IS NOT NULL OR @sOpen IS NOT NULL
  BEGIN
	-- check dates
	IF @sFromDate IS NOT NULL AND @sToDate  IS NOT NULL 
	  BEGIN
		SELECT @sWhere =' DateOfFailure Between ''' + @sFromDAte + ''' And ''' + @sToDAte + ''' '
	  END
  	ELSE IF @sFromDate IS NOT NULL
	  BEGIN
		SELECT @sWhere = 'DateOfFailure > ' + '''' + @sFromDate + ''''	
	  END
	ELSE IF @sToDate IS NOT NULL
	  BEGIN
		SELECT @sWhere = 'DateOfFailure > ' + '''' + @sToDate + ''''	
	  END
	-- check dealer
	IF @sDealerName IS NOT NULL
	  BEGIN
		IF @sWhere <> ''
		  BEGIN
			SELECT @sWhere = @sWhere + ' AND '
		  END
		SELECT @sWhere = @sWhere + ' Dealer = ''' + @sDealerName + ''''
	  END
	-- check open flag
	IF @sOpen IS NOT NULL
	  BEGIN
		IF @sWhere <> ''
		  BEGIN
			SELECT @sWhere = @sWhere + ' AND '
		  END
		SELECT @sWhere = @sWhere + ' WarrantyOpen = ' + @sOpen
	  END
	SELECT @sSQL = @sSQL + ' WHERE ' + @sWhere 
  END
SELECT @sSQL = @sSQL + '
GROUP BY dbo.tblWarranty.Dealer, dbo.tblWarranty.Model, dbo.tblWarranty.MachineSerialNumber, dbo.tblWarranty.DateOfFailure, 
	dbo.tblWarranty.LaborCost, dbo.tblWarranty.Travel, dbo.tblWarranty.Hours'
EXEC (@sSQL)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptWarrantyTotalCost]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE wasprsubRGAParts
	@sID varchar(10)

AS

DECLARE @sSQL varchar(800)

SELECT @sSQL = '
SELECT tblWarrantyParts.PartNumReplaced, tblParts.PartName, tblWarrantyParts.fkWarrantyID
FROM tblParts INNER JOIN tblWarrantyParts ON (tblParts.RPSPartNum=tblWarrantyParts.PartNumReplaced)'
SELECT @sSQL = @sSQL + ' WHERE tblWarrantyParts.fkWarrantyID = ' + @sID

EXEC (@sSQL)




GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprsubRGAParts]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE TRIGGER "tblLeads_UTrig" ON dbo.tblAllLeads FOR UPDATE AS
SET NOCOUNT ON
IF (SELECT Count(*) FROM inserted WHERE NOT (ActiveInactive='A' Or ActiveInactive='I')) > 0
    BEGIN
        RAISERROR 44444 'You must enter either an A for Active or an I for Inactive in this field.'
        ROLLBACK TRANSACTION
    END
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE TRIGGER "tblLeads_ITrig" ON dbo.tblAllLeads FOR INSERT AS
SET NOCOUNT ON
IF (SELECT Count(*) FROM inserted WHERE NOT (ActiveInactive='A' Or ActiveInactive='I')) > 0
    BEGIN
        RAISERROR 44444 'You must enter either an A for Active or an I for Inactive in this field.'
        ROLLBACK TRANSACTION
    END
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

