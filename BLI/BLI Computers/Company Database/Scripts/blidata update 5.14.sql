-- blidata Update 5.14 (October 2x, 1999 - mpg)
--  - set key on version table
--  - add version rows for past versions (5.00 - 5.13)
--  - add release notes for current version
--  - update quote, sale order, invoice tables to allow longer item descriptions

use blidata
go

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblVersion]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblVersion]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblVersion]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblVersion]
GO

set nocount on
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
values ('5.00','1999-08-15 00:00:00','- Lots of stuff works better, or works period.
- Some things don''t work.')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('5.01','1999-08-20 00:00:00','- Added Quantity to quote sub items.  When converted to a sales order, the correct quantity of items is added to the sale order sub item.
- Changed list views to show the selected item when popup forms and such are opened.
- Fixed ''Find'' dialog to work with SQL, improved it as well.')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('5.02','1999-08-30 00:00:00','- Fixed ''Jump To'' in Labor and Vendors
- Fixed ''New Customer'' form.
- Added debugging messages to Convert Quote to Sales Order code.')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('5.03','1999-09-05 00:00:00','- Added debug message form and more debug messages to the Quote to Sales Order code.')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('5.04','1999-09-10 00:00:00','- Added ''Check Inventory'' on Quotes to check available inventory for quote items.
- Updated ''Checking'' to use a tree view and list view.  Also slightly modified it to handle multiple accounts.
- Changed all places where items are shown in a list view or tree view with warning icons to also display red text instead of black text.
- Redesigned the ''Customer'' form using a treeview and listview.
- Redesigned the ''Customer'' pop up form to use a treeview and listview.
- Checking ''Main'' for phone numbers, ''Mail'' for addresses, or ''Primary'' for contacts will now clear the check on any other phone numbers/addresses/contacts for that customer automatically.
- To delete items in listviews simply hit ''Delete''.  (Some forms don''t have ''Remove'' buttons, so you must use <delete>.  Others work the same using either method.)')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('5.05','1999-09-15 00:00:00','- Changed envelope printing to allow several options - alignment, preview, etc.
- Fixed envelope printing button on Quotes and Invoices.')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('5.06','1999-09-20 00:00:00','- When you add a custom item in a quote, the custom item will be added to the price listing if a check box on the addition form is checked (which it is by default).
- ''Cancel'' now works on the quote custom item form.
- Added ''Edit Connection'' to the ''Utilities'' form from the main menu.  You can adjust connection parameters in this dialog.
- Updated envelope margins.
- Slightly modified version checking procedure to see if the change makes the autoupdate work on Nick''s computer.
- Updated checking to display new checks when no check number is entered.
- Fixed drop downs in deposits and checks to work.  The list of names comes from a combination of previous payments, customers, and vendors.
- Added ''Custom View'' to Checking transaction views.  Clicking it will open a popup form for you to enter info.
- Clicking column headers in Checking will sort based on that field.  But there is a limitation - it sorts based on the text, so numeric fields don''t sort in numeric order.  The date is the exception - that is sorted differently when selected.
- Added an ''Account Summary'' report to Management Reports.  It allows choosing a bunch of criteria from Checking.')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('5.07','1999-09-25 00:00:00','- Changed Labor sheets form to use treeview/listview controls.
- Changed security settings to allow SQL server security to be used - so if any drop downs don''t display anything, I missed them (all tables get ''dbo.'' added to the beginning of their names)
- Now using NT security for user validation and data access.  This may also fix Greg''s convert problem (converting Quote -> Sales Order).')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('5.08','1999-09-26 00:00:00','- Changed the quote conversion code to use its own connection and explicitly set each opened recordset to use server side recordsets.  [The ADO 2.1/2.5 couldn''t have been the problem - 2.1 was already being used...]  The ADO error collection is also printed to debug msgs.')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('5.09','1999-09-30 00:00:00','- Changed the conversion from quotes -> sales orders -> invoices to used stored procedures (which run on SQL) instead of code.  Damn your computer Greg.')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('5.10','1999-10-01 00:00:00','- Fixed computer information not showing up on computer invoice items.
- Changing the ''Shipping'' or ''Discount %'' on invoices and sales orders now updates the totals as it should.
- The password check for secure areas is now only run every 10 minutes - so if you use more than one ''secure'' thing in 10 minutes, you will not be prompted again.
- Added a report to ''Inventory Reports'' for Requested Inventory.  All items on sales orders that have not been assigned (are not in inventory) are printed along with the last order information for each item.
- Added code that checks all sales orders with previously unavailable inventory for now available inventory after adding a new order.  There is a check box below the save button on the order if you don''t want to run the check (which takes almost no time - it is all done on the server, so unless you have a good reason, leave it checking sales order items.)
- The quote ''Check Inventory'' was including all inventory, even sold and reserved.  Fixed.
- Added the same procedure to the invoice item removal function, so if you remove an item from an invoice, if that frees up inventory items they will be assigned right away.
- Voiding invoices and sales orders removes items, so you don''t have to manually.  It is also quote improved - if this invoice was converted from a sales order, the sales order is reactivated (and you can then void it if you wish).')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('5.11','1999-10-05 00:00:00','- Added ''Last 50 Checks'' and ''Last 50 Deposits'' to checking.
- Fixed in checking not being able to edit a check after adding it.
- Added a check to make sure you don''t add duplicate check numbers.
- Updated payment part of Invoices to use ''<add>'' feature and delete key functionality.
- Added payment tracking ability to Sales Orders (same as in invoices)
- Converting a sales order to an invoice also copies payment information to the invoice.
- Fixed an error if adding new items to an order and clicking ''no'' to adding to the price list.
- The source of an item on a quote is now shown with a cog icon for inventory items and a paper icon for price list items.  Price list items can be edited by double clicking them.
- Fixed checks not showing up and not adjusting account balance when added in checking.')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('5.12','1999-10-09 00:00:00','- Changed the code when selecting a customer in Invoices a bit to hopefully prevent the error Greg was getting.
- Changed the BLI icons to use icon files - they look better on nonstandard backgrounds.')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('5.13','1999-10-21 00:00:00','- Fixed "Check Number" showing up as dates on Account Report.
- Enabled entering item quantities on inventory items and computers for quotes.
- Fixed an error when converting a sales order to a quote when no payments were on the Sales Order')
go

-- Add 'Price Option' field to tblQuoteItem
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT FK_tblQuoteItem_tblQuote
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_afkQSISubItem
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_afkQItemSale
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_afkQItemEmp
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_QItemPrice
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_QItemCost
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_QItemQty
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_QItemExtCost
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_QItemExtPrice
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_QItemDetailFlag
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_QItemType
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_QItemOrder
GO
CREATE TABLE dbo.Tmp_tblQuoteItem
	(
 	apkQItem int NOT NULL IDENTITY (1, 1),
	afkQSISubItem int NULL CONSTRAINT DF_tblQuoteItem_afkQSISubItem DEFAULT (0),
	afkQItemQuote int NULL CONSTRAINT DF_tblQuoteItem_afkQItemSale DEFAULT (0),
	afkQItemEmp int NULL CONSTRAINT DF_tblQuoteItem_afkQItemEmp DEFAULT (0),
	QItemDesc nvarchar(200) NULL,
	QItemPrice smallmoney NULL CONSTRAINT DF_tblQuoteItem_QItemPrice DEFAULT (0),
	QItemCost smallmoney NULL CONSTRAINT DF_tblQuoteItem_QItemCost DEFAULT (0),
	QItemQty real NULL CONSTRAINT DF_tblQuoteItem_QItemQty DEFAULT (0),
	QItemExtCost smallmoney NULL CONSTRAINT DF_tblQuoteItem_QItemExtCost DEFAULT (0),
	QItemExtPrice smallmoney NULL CONSTRAINT DF_tblQuoteItem_QItemExtPrice DEFAULT (0),
	QItemDetailFlag bit NOT NULL CONSTRAINT DF_tblQuoteItem_QItemDetailFlag DEFAULT (0),
	QItemType int NULL CONSTRAINT DF_tblQuoteItem_QItemType DEFAULT (0),
	QItemOrder int NULL CONSTRAINT DF_tblQuoteItem_QItemOrder DEFAULT (0),
	QItemPriceOption tinyint NOT NULL CONSTRAINT DF_tblQuoteItem_QItemPriceOption DEFAULT (0),
	QItemNote ntext NULL
	) ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblQuoteItem ON
GO
IF EXISTS(SELECT * FROM dbo.tblQuoteItem)
	 EXEC('INSERT INTO dbo.Tmp_tblQuoteItem(apkQItem, afkQSISubItem, afkQItemQuote, afkQItemEmp, QItemDesc, QItemPrice, QItemCost, QItemQty, QItemExtCost, QItemExtPrice, QItemDetailFlag, QItemType, QItemOrder, QItemNote)
		SELECT apkQItem, afkQSISubItem, afkQItemQuote, afkQItemEmp, QItemDesc, QItemPrice, QItemCost, QItemQty, QItemExtCost, QItemExtPrice, QItemDetailFlag, QItemType, QItemOrder, QItemNote FROM dbo.tblQuoteItem TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblQuoteItem OFF
GO
ALTER TABLE dbo.tblQuoteSubItem
	DROP CONSTRAINT FK_tblQuoteSubItem_tblQuoteItem
GO
DROP TABLE dbo.tblQuoteItem
GO
EXECUTE sp_rename 'dbo.Tmp_tblQuoteItem', 'tblQuoteItem'
GO
ALTER TABLE dbo.tblQuoteItem ADD CONSTRAINT
	PK_tblQuoteItem PRIMARY KEY NONCLUSTERED 
	(
	apkQItem
	) ON [PRIMARY]
GO
ALTER TABLE dbo.tblQuoteItem WITH NOCHECK ADD CONSTRAINT
	FK_tblQuoteItem_tblQuote FOREIGN KEY
	(
	afkQItemQuote
	) REFERENCES dbo.tblQuote
	(
	apkQuote
	)
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblQuoteSubItem WITH NOCHECK ADD CONSTRAINT
	FK_tblQuoteSubItem_tblQuoteItem FOREIGN KEY
	(
	afkQuoteQItem
	) REFERENCES dbo.tblQuoteItem
	(
	apkQItem
	)
GO
COMMIT


BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT FK_tblQuoteItem_tblQuote
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_afkQSISubItem
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_afkQItemSale
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_afkQItemEmp
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_QItemPrice
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_QItemCost
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_QItemQty
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_QItemExtCost
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_QItemExtPrice
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_QItemDetailFlag
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_QItemType
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_QItemOrder
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_QItemPriceOption
GO
CREATE TABLE dbo.Tmp_tblQuoteItem
	(
 	apkQItem int NOT NULL IDENTITY (1, 1),
	afkQSISubItem int NULL CONSTRAINT DF_tblQuoteItem_afkQSISubItem DEFAULT (0),
	afkQItemQuote int NULL CONSTRAINT DF_tblQuoteItem_afkQItemSale DEFAULT (0),
	afkQItemEmp int NULL CONSTRAINT DF_tblQuoteItem_afkQItemEmp DEFAULT (0),
	QItemDesc nvarchar(200) NULL,
	QItemPrice smallmoney NULL CONSTRAINT DF_tblQuoteItem_QItemPrice DEFAULT (0),
	QItemCost smallmoney NULL CONSTRAINT DF_tblQuoteItem_QItemCost DEFAULT (0),
	QItemQty real NULL CONSTRAINT DF_tblQuoteItem_QItemQty DEFAULT (0),
	QItemExtCost smallmoney NULL CONSTRAINT DF_tblQuoteItem_QItemExtCost DEFAULT (0),
	QItemExtPrice smallmoney NULL CONSTRAINT DF_tblQuoteItem_QItemExtPrice DEFAULT (0),
	QItemDetailFlag bit NOT NULL CONSTRAINT DF_tblQuoteItem_QItemDetailFlag DEFAULT (0),
	QItemType int NULL CONSTRAINT DF_tblQuoteItem_QItemType DEFAULT (0),
	QItemOrder int NULL CONSTRAINT DF_tblQuoteItem_QItemOrder DEFAULT (0),
	QItemPriceOption tinyint NOT NULL CONSTRAINT DF_tblQuoteItem_QItemPriceOption DEFAULT (0),
	QItemCustomPrice smallmoney NULL,
	QItemCustomMarkup float(53) NULL,
	QItemNote ntext NULL
	) ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblQuoteItem ON
GO
IF EXISTS(SELECT * FROM dbo.tblQuoteItem)
	 EXEC('INSERT INTO dbo.Tmp_tblQuoteItem(apkQItem, afkQSISubItem, afkQItemQuote, afkQItemEmp, QItemDesc, QItemPrice, QItemCost, QItemQty, QItemExtCost, QItemExtPrice, QItemDetailFlag, QItemType, QItemOrder, QItemPriceOption, QItemNote)
		SELECT apkQItem, afkQSISubItem, afkQItemQuote, afkQItemEmp, QItemDesc, QItemPrice, QItemCost, QItemQty, QItemExtCost, QItemExtPrice, QItemDetailFlag, QItemType, QItemOrder, QItemPriceOption, QItemNote FROM dbo.tblQuoteItem TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblQuoteItem OFF
GO
ALTER TABLE dbo.tblQuoteSubItem
	DROP CONSTRAINT FK_tblQuoteSubItem_tblQuoteItem
GO
DROP TABLE dbo.tblQuoteItem
GO
EXECUTE sp_rename 'dbo.Tmp_tblQuoteItem', 'tblQuoteItem'
GO
ALTER TABLE dbo.tblQuoteItem ADD CONSTRAINT
	PK_tblQuoteItem PRIMARY KEY NONCLUSTERED 
	(
	apkQItem
	) ON [PRIMARY]
GO
ALTER TABLE dbo.tblQuoteItem WITH NOCHECK ADD CONSTRAINT
	FK_tblQuoteItem_tblQuote FOREIGN KEY
	(
	afkQItemQuote
	) REFERENCES dbo.tblQuote
	(
	apkQuote
	)
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblQuoteSubItem WITH NOCHECK ADD CONSTRAINT
	FK_tblQuoteSubItem_tblQuoteItem FOREIGN KEY
	(
	afkQuoteQItem
	) REFERENCES dbo.tblQuoteItem
	(
	apkQItem
	)
GO
COMMIT



-- add db properties table
-- create security and dbproperties tables
if exists (select * from sysobjects where id = object_id(N'[dbo].[tblDBProperties]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblDBProperties]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblSecurity]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblSecurity]
GO

CREATE TABLE [dbo].[tblDBProperties] (
	[PropertyName] [nvarchar] (20) NOT NULL ,
	[PropertyValue] [nvarchar] (20) NULL 
) ON [PRIMARY]
GO

INSERT INTO tblDBProperties (PropertyName, PropertyValue)
VALUES ('DBStructVersion','5.14')
GO

INSERT INTO tblDBProperties (PropertyName, PropertyValue)
VALUES ('LaborRate','87.5')
GO

INSERT INTO tblDBProperties (PropertyName, PropertyValue)
VALUES ('TaxRate','5.1')
GO

INSERT INTO tblDBProperties (PropertyName, PropertyValue)
VALUES ('CostMarkup','25')
GO


-- longer item descriptions
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT FK_tblQuoteItem_tblQuote
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_afkQSISubItem
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_afkQItemSale
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_afkQItemEmp
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_QItemPrice
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_QItemCost
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_QItemQty
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_QItemExtCost
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_QItemExtPrice
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_QItemDetailFlag
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_QItemType
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_QItemOrder
GO
ALTER TABLE dbo.tblQuoteItem
	DROP CONSTRAINT DF_tblQuoteItem_QItemPriceOption
GO
CREATE TABLE dbo.Tmp_tblQuoteItem
	(
 	apkQItem int NOT NULL IDENTITY (1, 1),
	afkQSISubItem int NULL CONSTRAINT DF_tblQuoteItem_afkQSISubItem DEFAULT (0),
	afkQItemQuote int NULL CONSTRAINT DF_tblQuoteItem_afkQItemSale DEFAULT (0),
	afkQItemEmp int NULL CONSTRAINT DF_tblQuoteItem_afkQItemEmp DEFAULT (0),
	QItemDesc ntext NULL,
	QItemPrice smallmoney NULL CONSTRAINT DF_tblQuoteItem_QItemPrice DEFAULT (0),
	QItemCost smallmoney NULL CONSTRAINT DF_tblQuoteItem_QItemCost DEFAULT (0),
	QItemQty real NULL CONSTRAINT DF_tblQuoteItem_QItemQty DEFAULT (0),
	QItemExtCost smallmoney NULL CONSTRAINT DF_tblQuoteItem_QItemExtCost DEFAULT (0),
	QItemExtPrice smallmoney NULL CONSTRAINT DF_tblQuoteItem_QItemExtPrice DEFAULT (0),
	QItemDetailFlag bit NOT NULL CONSTRAINT DF_tblQuoteItem_QItemDetailFlag DEFAULT (0),
	QItemType int NULL CONSTRAINT DF_tblQuoteItem_QItemType DEFAULT (0),
	QItemOrder int NULL CONSTRAINT DF_tblQuoteItem_QItemOrder DEFAULT (0),
	QItemPriceOption tinyint NOT NULL CONSTRAINT DF_tblQuoteItem_QItemPriceOption DEFAULT (0),
	QItemCustomPrice smallmoney NULL,
	QItemCustomMarkup float(53) NULL,
	QItemNote ntext NULL
	) ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblQuoteItem ON
GO
IF EXISTS(SELECT * FROM dbo.tblQuoteItem)
	 EXEC('INSERT INTO dbo.Tmp_tblQuoteItem(apkQItem, afkQSISubItem, afkQItemQuote, afkQItemEmp, QItemDesc, QItemPrice, QItemCost, QItemQty, QItemExtCost, QItemExtPrice, QItemDetailFlag, QItemType, QItemOrder, QItemPriceOption, QItemCustomPrice, QItemCustomMarkup, QItemNote)
		SELECT apkQItem, afkQSISubItem, afkQItemQuote, afkQItemEmp, CONVERT(ntext, QItemDesc), QItemPrice, QItemCost, QItemQty, QItemExtCost, QItemExtPrice, QItemDetailFlag, QItemType, QItemOrder, QItemPriceOption, QItemCustomPrice, QItemCustomMarkup, QItemNote FROM dbo.tblQuoteItem TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblQuoteItem OFF
GO
ALTER TABLE dbo.tblQuoteSubItem
	DROP CONSTRAINT FK_tblQuoteSubItem_tblQuoteItem
GO
DROP TABLE dbo.tblQuoteItem
GO
EXECUTE sp_rename 'dbo.Tmp_tblQuoteItem', 'tblQuoteItem'
GO
ALTER TABLE dbo.tblQuoteItem ADD CONSTRAINT
	PK_tblQuoteItem PRIMARY KEY NONCLUSTERED 
	(
	apkQItem
	) ON [PRIMARY]
GO
ALTER TABLE dbo.tblQuoteItem WITH NOCHECK ADD CONSTRAINT
	FK_tblQuoteItem_tblQuote FOREIGN KEY
	(
	afkQItemQuote
	) REFERENCES dbo.tblQuote
	(
	apkQuote
	)
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblQuoteSubItem WITH NOCHECK ADD CONSTRAINT
	FK_tblQuoteSubItem_tblQuoteItem FOREIGN KEY
	(
	afkQuoteQItem
	) REFERENCES dbo.tblQuoteItem
	(
	apkQItem
	)
GO
COMMIT


BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblSaleItem
	DROP CONSTRAINT tblSaleItem_FK00
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblSaleItem
	DROP CONSTRAINT DF__Temporary__afkSI__7FEAFD3E
GO
ALTER TABLE dbo.tblSaleItem
	DROP CONSTRAINT DF__Temporary__afkSI__00DF2177
GO
ALTER TABLE dbo.tblSaleItem
	DROP CONSTRAINT DF__Temporary__SItem__01D345B0
GO
ALTER TABLE dbo.tblSaleItem
	DROP CONSTRAINT DF__Temporary__SItem__02C769E9
GO
ALTER TABLE dbo.tblSaleItem
	DROP CONSTRAINT DF__Temporary__SItem__03BB8E22
GO
ALTER TABLE dbo.tblSaleItem
	DROP CONSTRAINT DF__Temporary__SItem__04AFB25B
GO
ALTER TABLE dbo.tblSaleItem
	DROP CONSTRAINT DF__Temporary__SItem__05A3D694
GO
ALTER TABLE dbo.tblSaleItem
	DROP CONSTRAINT DF__Temporary__SItem__0697FACD
GO
ALTER TABLE dbo.tblSaleItem
	DROP CONSTRAINT DF__Temporary__SItem__078C1F06
GO
ALTER TABLE dbo.tblSaleItem
	DROP CONSTRAINT DF__Temporary__SItem__0880433F
GO
ALTER TABLE dbo.tblSaleItem
	DROP CONSTRAINT DF_tblSaleItem_SItemWarn
GO
ALTER TABLE dbo.tblSaleItem
	DROP CONSTRAINT DF_tblSaleItem_SItemReserved
GO
ALTER TABLE dbo.tblSaleItem
	DROP CONSTRAINT DF_tblSaleItem_IItemAutoCalcPrice
GO
CREATE TABLE dbo.Tmp_tblSaleItem
	(
 	apkSItem int NOT NULL IDENTITY (1, 1),
	afkQSISubItem int NULL,
	afkSItemSale int NULL CONSTRAINT DF__Temporary__afkSI__7FEAFD3E DEFAULT (0),
	afkSItemEmp int NULL CONSTRAINT DF__Temporary__afkSI__00DF2177 DEFAULT (0),
	SItemDesc ntext NULL,
	SItemPrice smallmoney NULL CONSTRAINT DF__Temporary__SItem__01D345B0 DEFAULT (0),
	SItemCost smallmoney NULL CONSTRAINT DF__Temporary__SItem__02C769E9 DEFAULT (0),
	SItemQty real NULL CONSTRAINT DF__Temporary__SItem__03BB8E22 DEFAULT (0),
	SItemExtCost smallmoney NULL CONSTRAINT DF__Temporary__SItem__04AFB25B DEFAULT (0),
	SItemExtPrice smallmoney NULL CONSTRAINT DF__Temporary__SItem__05A3D694 DEFAULT (0),
	SItemDetailFlag bit NOT NULL CONSTRAINT DF__Temporary__SItem__0697FACD DEFAULT (0),
	SItemType int NULL CONSTRAINT DF__Temporary__SItem__078C1F06 DEFAULT (0),
	SItemOrder int NULL CONSTRAINT DF__Temporary__SItem__0880433F DEFAULT (0),
	SItemNote ntext NULL,
	SItemWarn bit NOT NULL CONSTRAINT DF_tblSaleItem_SItemWarn DEFAULT (0),
	SItemReserved bit NOT NULL CONSTRAINT DF_tblSaleItem_SItemReserved DEFAULT (0),
	z_apkSItem int NULL,
	SStemAutoCalcPrice bit NOT NULL CONSTRAINT DF_tblSaleItem_IItemAutoCalcPrice DEFAULT (1)
	) ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblSaleItem ON
GO
IF EXISTS(SELECT * FROM dbo.tblSaleItem)
	 EXEC('INSERT INTO dbo.Tmp_tblSaleItem(apkSItem, afkQSISubItem, afkSItemSale, afkSItemEmp, SItemDesc, SItemPrice, SItemCost, SItemQty, SItemExtCost, SItemExtPrice, SItemDetailFlag, SItemType, SItemOrder, SItemNote, SItemWarn, SItemReserved, z_apkSItem, SStemAutoCalcPrice)
		SELECT apkSItem, afkQSISubItem, afkSItemSale, afkSItemEmp, CONVERT(ntext, SItemDesc), SItemPrice, SItemCost, SItemQty, SItemExtCost, SItemExtPrice, SItemDetailFlag, SItemType, SItemOrder, SItemNote, SItemWarn, SItemReserved, z_apkSItem, SStemAutoCalcPrice FROM dbo.tblSaleItem TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblSaleItem OFF
GO
ALTER TABLE dbo.tblSaleSubItem
	DROP CONSTRAINT FK_tblSaleSubItem_tblSaleItem
GO
DROP TABLE dbo.tblSaleItem
GO
EXECUTE sp_rename 'dbo.Tmp_tblSaleItem', 'tblSaleItem'
GO
ALTER TABLE dbo.tblSaleItem ADD CONSTRAINT
	aaaaatblSaleItem_PK PRIMARY KEY NONCLUSTERED 
	(
	apkSItem
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [{9FFF2CF5-8C4C-4745-9322-71BD25EC281E}] ON dbo.tblSaleItem
	(
	afkSItemSale
	) ON [PRIMARY]
GO
ALTER TABLE dbo.tblSaleItem WITH NOCHECK ADD CONSTRAINT
	tblSaleItem_FK00 FOREIGN KEY
	(
	afkSItemSale
	) REFERENCES dbo.tblSale
	(
	apkSale
	)
GO
CREATE TRIGGER "tblSaleItem_UTrig" ON dbo.tblSaleItem FOR UPDATE AS
SET NOCOUNT ON
/* * PREVENT UPDATES IF NO MATCHING KEY IN 'tblSale' */
IF UPDATE(afkSItemSale)
    BEGIN
        IF (SELECT COUNT(*) FROM inserted) !=
           (SELECT COUNT(*) FROM tblSale, inserted WHERE (tblSale.apkSale = inserted.afkSItemSale))
            BEGIN
                RAISERROR 44446 'The record can''t be added or changed. Referential integrity rules require a related record in table ''tblSale''.'
                ROLLBACK TRANSACTION
            END
    END
GO
CREATE TRIGGER "tblSaleItem_ITrig" ON dbo.tblSaleItem FOR INSERT AS
SET NOCOUNT ON
/* * PREVENT INSERTS IF NO MATCHING KEY IN 'tblSale' */
IF (SELECT COUNT(*) FROM inserted) !=
   (SELECT COUNT(*) FROM tblSale, inserted WHERE (tblSale.apkSale = inserted.afkSItemSale))
    BEGIN
        RAISERROR 44447 'The record can''t be added or changed. Referential integrity rules require a related record in table ''tblSale''.'
        ROLLBACK TRANSACTION
    END
GO
CREATE Trigger tblSaleItem_DTrig
On dbo.tblSaleItem
For DELETE
As

/* * DELETE subItems */
DELETE FROM tblSaleSubItem
WHERE afkSaleSItem IN (
	SELECT apkSItem
	FROM deleted )

/* * UPDATE MARKED ITEMS IN tblInvn */
UPDATE tblInvn SET afkInvnSItem = 0
WHERE afkInvnSItem IN (
	SELECT apkSItem 
	FROM deleted )
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblSaleSubItem WITH NOCHECK ADD CONSTRAINT
	FK_tblSaleSubItem_tblSaleItem FOREIGN KEY
	(
	afkSaleSItem
	) REFERENCES dbo.tblSaleItem
	(
	apkSItem
	)
GO
COMMIT

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblInvoiceItem
	DROP CONSTRAINT FK_tblInvoiceItem_tblInvoice
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblInvoiceItem
	DROP CONSTRAINT DF_tblInvoiceItem_afkIItemInv
GO
ALTER TABLE dbo.tblInvoiceItem
	DROP CONSTRAINT DF_tblInvoiceItem_afkIItemEmp
GO
ALTER TABLE dbo.tblInvoiceItem
	DROP CONSTRAINT DF_tblInvoiceItem_IItemPrice
GO
ALTER TABLE dbo.tblInvoiceItem
	DROP CONSTRAINT DF_tblInvoiceItem_IItemCost
GO
ALTER TABLE dbo.tblInvoiceItem
	DROP CONSTRAINT DF_tblInvoiceItem_IItemQty
GO
ALTER TABLE dbo.tblInvoiceItem
	DROP CONSTRAINT DF_tblInvoiceItem_IItemExtCost
GO
ALTER TABLE dbo.tblInvoiceItem
	DROP CONSTRAINT DF_tblInvoiceItem_IItemExtPrice
GO
ALTER TABLE dbo.tblInvoiceItem
	DROP CONSTRAINT DF_tblInvoiceItem_IItemDetailFlag
GO
ALTER TABLE dbo.tblInvoiceItem
	DROP CONSTRAINT DF_tblInvoiceItem_IItemType
GO
ALTER TABLE dbo.tblInvoiceItem
	DROP CONSTRAINT DF_tblInvoiceItem_IItemOrder
GO
ALTER TABLE dbo.tblInvoiceItem
	DROP CONSTRAINT DF_tblInvoiceItem_IItemAutoCalcPrice
GO
CREATE TABLE dbo.Tmp_tblInvoiceItem
	(
 	apkIItem int NOT NULL IDENTITY (1, 1),
	afkQSISubItem int NULL,
	afkIItemInv int NULL CONSTRAINT DF_tblInvoiceItem_afkIItemInv DEFAULT (0),
	afkIItemEmp int NULL CONSTRAINT DF_tblInvoiceItem_afkIItemEmp DEFAULT (0),
	IItemDesc ntext NULL,
	IItemPrice smallmoney NULL CONSTRAINT DF_tblInvoiceItem_IItemPrice DEFAULT (0),
	IItemCost smallmoney NULL CONSTRAINT DF_tblInvoiceItem_IItemCost DEFAULT (0),
	IItemQty real NULL CONSTRAINT DF_tblInvoiceItem_IItemQty DEFAULT (0),
	IItemExtCost smallmoney NULL CONSTRAINT DF_tblInvoiceItem_IItemExtCost DEFAULT (0),
	IItemExtPrice smallmoney NULL CONSTRAINT DF_tblInvoiceItem_IItemExtPrice DEFAULT (0),
	IItemDetailFlag bit NOT NULL CONSTRAINT DF_tblInvoiceItem_IItemDetailFlag DEFAULT (0),
	IItemType int NULL CONSTRAINT DF_tblInvoiceItem_IItemType DEFAULT (0),
	IItemOrder int NULL CONSTRAINT DF_tblInvoiceItem_IItemOrder DEFAULT (0),
	IItemNote ntext NULL,
	z_apkSItem int NULL,
	IItemAutoCalcPrice bit NOT NULL CONSTRAINT DF_tblInvoiceItem_IItemAutoCalcPrice DEFAULT (1)
	) ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblInvoiceItem ON
GO
IF EXISTS(SELECT * FROM dbo.tblInvoiceItem)
	 EXEC('INSERT INTO dbo.Tmp_tblInvoiceItem(apkIItem, afkQSISubItem, afkIItemInv, afkIItemEmp, IItemDesc, IItemPrice, IItemCost, IItemQty, IItemExtCost, IItemExtPrice, IItemDetailFlag, IItemType, IItemOrder, IItemNote, z_apkSItem, IItemAutoCalcPrice)
		SELECT apkIItem, afkQSISubItem, afkIItemInv, afkIItemEmp, CONVERT(ntext, IItemDesc), IItemPrice, IItemCost, IItemQty, IItemExtCost, IItemExtPrice, IItemDetailFlag, IItemType, IItemOrder, IItemNote, z_apkSItem, IItemAutoCalcPrice FROM dbo.tblInvoiceItem TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblInvoiceItem OFF
GO
ALTER TABLE dbo.tblCustComp
	DROP CONSTRAINT FK_tblCustComp_tblInvoiceItem
GO
DROP TABLE dbo.tblInvoiceItem
GO
EXECUTE sp_rename 'dbo.Tmp_tblInvoiceItem', 'tblInvoiceItem'
GO
ALTER TABLE dbo.tblInvoiceItem ADD CONSTRAINT
	PK_tblInvoiceItem PRIMARY KEY NONCLUSTERED 
	(
	apkIItem
	) ON [PRIMARY]
GO
ALTER TABLE dbo.tblInvoiceItem WITH NOCHECK ADD CONSTRAINT
	FK_tblInvoiceItem_tblInvoice FOREIGN KEY
	(
	afkIItemInv
	) REFERENCES dbo.tblInvoice
	(
	apkInv
	)
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblCustComp WITH NOCHECK ADD CONSTRAINT
	FK_tblCustComp_tblInvoiceItem FOREIGN KEY
	(
	afkCCompIItem
	) REFERENCES dbo.tblInvoiceItem
	(
	apkIItem
	)
GO
COMMIT


insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('5.14','1999-10-22 00:00:00','- Fixed r:### showing up in transaction column for new deposits. 
- Fixed terms not displaying on invoices.  (#Error appeared)  Also updated the wording slightly.
- Added ''Options'' tab to quote items.  You can choose the price calculation method.  Auto calc, custom price, default markup, or custom markup
- The tax rate, default labor rate, and default markup rate are stored in a table so they can be changed easily
- Quote, Sales Order, and Invoice items now much longer text to be entered in description (especially useful for labor).
- Added ''Export'' command in the File menu of report previews.  You can export to a Microsoft Snapshot format that then requires Microsoft Snapshot Viewer to see')
go


