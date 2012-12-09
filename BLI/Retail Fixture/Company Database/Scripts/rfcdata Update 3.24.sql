-- rfcdata structure updates


ALTER TABLE dbo.Tbl_Catalog
	DROP CONSTRAINT Tbl_Catalog_FK00
GO

ALTER TABLE dbo.Tbl_Catalog
	DROP CONSTRAINT DF__Temporary__afkCa__117F9D94
GO
ALTER TABLE dbo.Tbl_Catalog
	DROP CONSTRAINT DF__Temporary__Price__1273C1CD
GO
ALTER TABLE dbo.Tbl_Catalog
	DROP CONSTRAINT DF__Temporary__Weigh__1367E606
GO
ALTER TABLE dbo.Tbl_Catalog
	DROP CONSTRAINT DF__Temporary__Medit__145C0A3F
GO
ALTER TABLE dbo.Tbl_Catalog
	DROP CONSTRAINT DF__Temporary__Inven__15502E78
GO
ALTER TABLE dbo.Tbl_Catalog
	DROP CONSTRAINT DF__Temporary__Headi__164452B1
GO
ALTER TABLE dbo.Tbl_Catalog
	DROP CONSTRAINT DF__Temporary__CatOr__173876EA
GO
ALTER TABLE dbo.Tbl_Catalog
	DROP CONSTRAINT DF__Temporary__Commi__182C9B23
GO
ALTER TABLE dbo.Tbl_Catalog
	DROP CONSTRAINT DF__Temporary__Activ__1920BF5C
GO
ALTER TABLE dbo.Tbl_Catalog
	DROP CONSTRAINT DF__TemporaryU__Hide__1A14E395
GO
ALTER TABLE dbo.Tbl_Catalog
	DROP CONSTRAINT DF__Temporary__SubIt__1B0907CE
GO
ALTER TABLE dbo.Tbl_Catalog
	DROP CONSTRAINT DF__Temporary__Cashw__1BFD2C07
GO
CREATE TABLE dbo.Tmp_Tbl_Catalog
	(
	apkCatalogItem int NOT NULL IDENTITY (1, 1),
	afkCatalogHeading int NULL,
	Material nvarchar(50) NULL,
	CustMaterial nvarchar(50) NULL,
	Description nvarchar(50) NULL,
	Price money NULL,
	Weight real NULL,
	Medite bit NULL,
	Inventory int NULL,
	Heading_Number int NULL,
	CatOrder smallint NULL,
	[Committed] int NULL,
	Active bit NULL,
	Hide bit NULL,
	SubItems bit NULL,
	Cashwrap bit NULL,
	CashwrapName nvarchar(100) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Tbl_Catalog ADD CONSTRAINT
	DF__Temporary__afkCa__117F9D94 DEFAULT (0) FOR afkCatalogHeading
GO
ALTER TABLE dbo.Tmp_Tbl_Catalog ADD CONSTRAINT
	DF__Temporary__Price__1273C1CD DEFAULT (0) FOR Price
GO
ALTER TABLE dbo.Tmp_Tbl_Catalog ADD CONSTRAINT
	DF__Temporary__Weigh__1367E606 DEFAULT (0) FOR Weight
GO
ALTER TABLE dbo.Tmp_Tbl_Catalog ADD CONSTRAINT
	DF__Temporary__Medit__145C0A3F DEFAULT (0) FOR Medite
GO
ALTER TABLE dbo.Tmp_Tbl_Catalog ADD CONSTRAINT
	DF__Temporary__Inven__15502E78 DEFAULT (0) FOR Inventory
GO
ALTER TABLE dbo.Tmp_Tbl_Catalog ADD CONSTRAINT
	DF__Temporary__Headi__164452B1 DEFAULT (0) FOR Heading_Number
GO
ALTER TABLE dbo.Tmp_Tbl_Catalog ADD CONSTRAINT
	DF__Temporary__CatOr__173876EA DEFAULT (0) FOR CatOrder
GO
ALTER TABLE dbo.Tmp_Tbl_Catalog ADD CONSTRAINT
	DF__Temporary__Commi__182C9B23 DEFAULT (0) FOR [Committed]
GO
ALTER TABLE dbo.Tmp_Tbl_Catalog ADD CONSTRAINT
	DF__Temporary__Activ__1920BF5C DEFAULT (0) FOR Active
GO
ALTER TABLE dbo.Tmp_Tbl_Catalog ADD CONSTRAINT
	DF__TemporaryU__Hide__1A14E395 DEFAULT (0) FOR Hide
GO
ALTER TABLE dbo.Tmp_Tbl_Catalog ADD CONSTRAINT
	DF__Temporary__SubIt__1B0907CE DEFAULT (0) FOR SubItems
GO
ALTER TABLE dbo.Tmp_Tbl_Catalog ADD CONSTRAINT
	DF__Temporary__Cashw__1BFD2C07 DEFAULT (0) FOR Cashwrap
GO
SET IDENTITY_INSERT dbo.Tmp_Tbl_Catalog ON
GO
IF EXISTS(SELECT * FROM dbo.Tbl_Catalog)
	 EXEC('INSERT INTO dbo.Tmp_Tbl_Catalog (apkCatalogItem, afkCatalogHeading, Material, Description, Price, Weight, Medite, Inventory, Heading_Number, CatOrder, [Committed], Active, Hide, SubItems, Cashwrap, CashwrapName)
		SELECT apkCatalogItem, afkCatalogHeading, Material, Description, Price, Weight, Medite, Inventory, Heading_Number, CatOrder, [Committed], Active, Hide, SubItems, Cashwrap, CashwrapName FROM dbo.Tbl_Catalog TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_Tbl_Catalog OFF
GO
ALTER TABLE dbo.Tbl_Inventory_Entry_Items
	DROP CONSTRAINT Tbl_Inventory_Entry_Items_FK00
GO
ALTER TABLE dbo.Tbl_Invoice_Items
	DROP CONSTRAINT Tbl_Invoice_Items_FK00
GO
ALTER TABLE dbo.Tbl_PO_Items
	DROP CONSTRAINT Tbl_PO_Items_FK00
GO
ALTER TABLE dbo.tblCatalogSubItems
	DROP CONSTRAINT tblCatalogSubItems_FK00
GO
DROP TABLE dbo.Tbl_Catalog
GO
EXECUTE sp_rename N'dbo.Tmp_Tbl_Catalog', N'Tbl_Catalog', 'OBJECT'
GO
ALTER TABLE dbo.Tbl_Catalog ADD CONSTRAINT
	aaaaaTbl_Catalog_PK PRIMARY KEY NONCLUSTERED 
	(
	apkCatalogItem
	) WITH FILLFACTOR = 90 ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX CatOrder ON dbo.Tbl_Catalog
	(
	CatOrder
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX Material ON dbo.Tbl_Catalog
	(
	Material
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX Tbl_CatalogHeading_Number ON dbo.Tbl_Catalog
	(
	Heading_Number
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
ALTER TABLE dbo.Tbl_Catalog WITH NOCHECK ADD CONSTRAINT
	Tbl_Catalog_FK00 FOREIGN KEY
	(
	Heading_Number
	) REFERENCES dbo.Tbl_Catalog_Headings
	(
	Heading_Number
	)
GO
ALTER TABLE dbo.Tbl_Catalog
	NOCHECK CONSTRAINT Tbl_Catalog_FK00
GO
CREATE TRIGGER "Tbl_Catalog_UTrig" ON dbo.Tbl_Catalog FOR UPDATE AS
SET NOCOUNT ON
/* * CASCADE UPDATES TO 'tblCatalogSubItems' */
IF UPDATE(apkCatalogItem)
    BEGIN
       UPDATE tblCatalogSubItems
       SET tblCatalogSubItems.afkItemNum = inserted.apkCatalogItem
       FROM tblCatalogSubItems, deleted, inserted
       WHERE deleted.apkCatalogItem = tblCatalogSubItems.afkItemNum
    END
GO
CREATE TRIGGER "Tbl_Catalog_DTrig" ON dbo.Tbl_Catalog FOR DELETE AS
SET NOCOUNT ON
/* * CASCADE DELETES TO 'tblCatalogSubItems' */
DELETE tblCatalogSubItems FROM deleted, tblCatalogSubItems WHERE deleted.apkCatalogItem = tblCatalogSubItems.afkItemNum
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblCatalogSubItems WITH NOCHECK ADD CONSTRAINT
	tblCatalogSubItems_FK00 FOREIGN KEY
	(
	afkItemNum
	) REFERENCES dbo.Tbl_Catalog
	(
	apkCatalogItem
	)
GO
ALTER TABLE dbo.tblCatalogSubItems
	NOCHECK CONSTRAINT tblCatalogSubItems_FK00
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.Tbl_PO_Items WITH NOCHECK ADD CONSTRAINT
	Tbl_PO_Items_FK00 FOREIGN KEY
	(
	Material
	) REFERENCES dbo.Tbl_Catalog
	(
	Material
	)
GO
ALTER TABLE dbo.Tbl_PO_Items
	NOCHECK CONSTRAINT Tbl_PO_Items_FK00
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.Tbl_Invoice_Items WITH NOCHECK ADD CONSTRAINT
	Tbl_Invoice_Items_FK00 FOREIGN KEY
	(
	Material
	) REFERENCES dbo.Tbl_Catalog
	(
	Material
	)
GO
ALTER TABLE dbo.Tbl_Invoice_Items
	NOCHECK CONSTRAINT Tbl_Invoice_Items_FK00
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.Tbl_Inventory_Entry_Items WITH NOCHECK ADD CONSTRAINT
	Tbl_Inventory_Entry_Items_FK00 FOREIGN KEY
	(
	Material
	) REFERENCES dbo.Tbl_Catalog
	(
	Material
	)
GO
ALTER TABLE dbo.Tbl_Inventory_Entry_Items
	NOCHECK CONSTRAINT Tbl_Inventory_Entry_Items_FK00
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Form_InvoiceItemTotals]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Form_InvoiceItemTotals]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Form_POItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Form_POItem]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Form_POItemAdd]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Form_POItemAdd]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Form_POItemSubItemAdd]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Form_POItemSubItemAdd]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Form_POItemSubItemDelete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Form_POItemSubItemDelete]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Form_POItemSubItemEdit]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Form_POItemSubItemEdit]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Form_POItemSubItems]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Form_POItemSubItems]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Form_POItemTotals]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Form_POItemTotals]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Form_POItemUpdate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Form_POItemUpdate]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Form_POItems]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Form_POItems]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_CurrentBuildSummary]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_CurrentBuildSummary]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_CurrentInventory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_CurrentInventory]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_CustShipmentSchedule]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_CustShipmentSchedule]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_GapOrderConf]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_GapOrderConf]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_HotOrders]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_HotOrders]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_IncompletePOs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_IncompletePOs]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_Invoice]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_Invoice]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_InvoiceSummary]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_InvoiceSummary]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_MaintOrders]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_MaintOrders]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_OutstandingPOs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_OutstandingPOs]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_POsByJobNumber]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_POsByJobNumber]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_ProductCatalog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_ProductCatalog]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_ShipmentSchedule]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_ShipmentSchedule]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_ShipmentSummary]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_ShipmentSummary]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_TotalSalesByClass]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_TotalSalesByClass]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_WisconsinTaxes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_WisconsinTaxes]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_WorkOrder]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_WorkOrder]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Util_GetSoldToAddressInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Util_GetSoldToAddressInfo]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Util_GetStoreInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Util_GetStoreInfo]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Web_Cat_Groups]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Web_Cat_Groups]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Web_Cat_Items]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Web_Cat_Items]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Web_Login]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Web_Login]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Web_PO_Find]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Web_PO_Find]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Web_PO_Open]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Web_PO_Open]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Web_PO_View]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Web_PO_View]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Web_PO_View_Items]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Web_PO_View_Items]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE dbo.sp_Form_InvoiceItemTotals
	@ID int = 0
AS
SELECT Count(invi.apkInvoiceItem) AS ItemCount, 
	Sum(cat.Weight * invi.Quantiy) AS ExtWeight, 
	Sum(invi.Unit_Price * invi.Quantiy) AS ExtPrice 
FROM Tbl_Catalog cat
	INNER JOIN Tbl_Invoice_Items invi ON cat.Material = invi.Material 
WHERE invi.ID = @ID
GROUP BY invi.ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE dbo.sp_Form_POItem
	@POItemID int = null
AS

select poi.Material, poi.MatDescription Description, poi.Quanity Quantity, poi.Unit_Price Price, poi.Unit_Quantiy Unit, poi.Phase, 
	poi.SubItems, cat.CustMaterial, cat.Weight SingleItemWeight
from Tbl_PO_Items poi
	left outer join Tbl_Catalog cat on cat.Material = poi.Material
where poi.apkPOItem = @POItemID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE dbo.sp_Form_POItemAdd
	@POID int = null,
	@RFMaterial nvarchar(50) = null
AS

DECLARE @ItemID int, @SubItems bit

SET NOCOUNT ON

-- insert item into PO item table
insert into tbl_po_items (id, material, matdescription, unit_price, Unit_Quantiy, subitems)
select @POID, @RFMaterial, cat.Description, cat.Price, 'EA', cat.SubItems
from Tbl_Catalog cat
where cat.Material = @RFMaterial

-- get the ID
select @ItemID = @@identity

-- check for and insert sub items
select @SubItems = SubItems from Tbl_Catalog where Material = @RFMaterial
if (@SubItems = 1)
	insert into tblPOSubItems (afkItemNum, ItemQty, ItemDesc, ItemPrice)
	select @ItemID, ItemQty, ItemDesc, ItemPrice
	from tblCatalogSubItems
	where afkItemNum in (select apkCatalogItem from Tbl_Catalog where Material = @RFMaterial)

-- return the current item id
select @ItemID POItemID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE dbo.sp_Form_POItemSubItemAdd
	(
		@POItemID int = null,
		@Description nvarchar(50) = null,
		@Quantity float(8) = null,
		@Price smallmoney
	)
AS
SET NOCOUNT ON

insert into tblPOSubItems (afkItemNum, ItemDesc, ItemQty, ItemPrice)
values (@POItemID, @Description, @Quantity, @Price)

select @@identity POSubItemID

RETURN 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE dbo.sp_Form_POItemSubItemDelete
	(
		@POSubItemID int = null
	)
AS
SET NOCOUNT ON

delete
from tblPOSubItems
where
	apkItem		= @POSubItemID

RETURN 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE dbo.sp_Form_POItemSubItemEdit
	(
		@POSubItemID int = null,
		@Description nvarchar(50) = null,
		@Quantity float(8) = null,
		@Price smallmoney
	)
AS
SET NOCOUNT ON

update tblPOSubItems
set
	ItemDesc	= @Description,
	ItemQty		= @Quantity,
	ItemPrice	= @Price
where
	apkItem		= @POSubItemID

RETURN 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE dbo.sp_Form_POItemSubItems
	@POItemID int
AS

select apkItem, ItemDesc, ItemQty, ItemPrice
from tblPOSubItems
where afkItemNum = @POItemID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE dbo.sp_Form_POItemTotals
	@ID int = 0
AS
SELECT Count(apkPOItem) AS ItemCount, 
	Sum([Weight]*[Quanity]) AS ExtWeight, 
	Sum([Unit_Price]*[Quanity]) AS ExtPrice 
FROM Tbl_PO_Items poi
	INNER JOIN Tbl_Catalog cat ON cat.Material = poi.Material
WHERE poi.ID = @ID 
GROUP BY poi.ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE dbo.sp_Form_POItemUpdate
	(
		@POItemID int = null,
		@Description nvarchar(50) = null,
		@Phase nvarchar(50) = null,
		@Quantity int = null,
		@Price smallmoney = null,
		@Unit nvarchar(50) = null,
		@SubItems bit = null	
	)
AS

SET NOCOUNT ON

-- Update main table with updated values
update Tbl_PO_Items
set
	MatDescription	= @Description,
	Phase			= @Phase,
	Quanity			= @Quantity,
	Unit_Price		= @Price,
	Unit_Quantiy	= @Unit,
	SubItems		= @SubItems
where
	apkPOItem		= @POItemID

-- make sure to clear out any sub items if set to 0
if (@SubItems = 0)
	delete from tblPOSubItems where afkItemNum = @POItemID
	
RETURN 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE dbo.sp_Form_POItems
	@POID int
AS

SELECT poi.apkPOItem, poi.Material RFMaterial, cat.CustMaterial CustMaterial, poi.MatDescription Description, poi.Phase, cat.Weight * poi.Quanity Weight, poi.SubItems,
	poi.Unit_Price Price, poi.Unit_Quantiy Unit, poi.Quanity Quantity, poi.Quanity * poi.Unit_Price Total
FROM	Tbl_PO_Items poi
	LEFT OUTER JOIN Tbl_Catalog cat ON poi.Material = cat.Material
WHERE poi.ID = @POID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO



CREATE PROCEDURE dbo.sp_Report_CurrentBuildSummary
	@ShipFromDate nvarchar(20) = null, 
	@ShipToDate nvarchar(20) = null,
	@SoldName nvarchar(100) = null,
	@StoreNo nvarchar(100) = null,
	@RefNum nvarchar(100) = null
AS
IF @ShipFromDate IS NULL
	SELECT @ShipFromDate = '1/1/1900'
IF @ShipToDate IS NULL
	SELECT @ShipToDate = '1/1/2100'
SELECT poi.Material, cat.Description, cat.Inventory, Sum(poi.Quanity) AS SumOfQuanity, po.SoldName
FROM Tbl_Catalog cat
	INNER JOIN Tbl_PO_Items poi ON poi.Material = cat.Material
	INNER JOIN Tbl_PO po ON po.ID = poi.ID
WHERE po.Credit = 0 AND po.Ship_Date Between @ShipFromDate And @ShipToDate AND po.CancelPO = 0
	AND (po.Status <> 'Complete' Or po.Status Is Null)
	AND CASE 
		WHEN @SoldName IS NULL THEN 1  
		WHEN po.SoldName Like @SoldName THEN 1 
		 ELSE 0 END  = 1
	AND CASE
		WHEN @StoreNo IS NULL THEN 1
		WHEN po.Ship_StoreNo = @StoreNo THEN 1
		ELSE 0 END = 1
	AND CASE
		WHEN @RefNum IS NULL THEN 1
		WHEN po.RefNum = @RefNum THEN 1
		ELSE 0 END = 1
GROUP BY poi.Material, cat.Description, cat.Inventory, po.SoldName
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE dbo.sp_Report_CurrentInventory
	@Active bit = null,
	@Hide bit = null,
	@CustID int = null
AS
SELECT cat.Material, cat.Description, cat.Inventory, cat.Hide, cat.Active, cat.Price * cat.Inventory AS ExtPrice
FROM Tbl_Catalog_Headings cath 
	INNER JOIN Tbl_Catalog cat ON cath.apkHeading = cat.afkCatalogHeading
WHERE CASE 
		WHEN @Active IS NULL THEN 1  
		WHEN cat.Active = @Active THEN 1 
		 ELSE 0 END  = 1
	and CASE
		WHEN @Hide IS NULL THEN 1  
		WHEN cat.Active = @Hide THEN 1 
		 ELSE 0 END  = 1
	and CASE
		WHEN @CustID IS NULL THEN 1  
		WHEN cath.afkHeadCust = @CustID THEN 1 
		 ELSE 0 END  = 1
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.sp_Report_CustShipmentSchedule
	@ShipFromDate nvarchar(20) = null, 
	@ShipToDate nvarchar(20) = null,
	@SoldName nvarchar(100) = null	
AS
IF @ShipFromDate IS NULL
	SELECT @ShipFromDate = '1/1/1900'
IF @ShipToDate IS NULL
	SELECT @ShipToDate = '1/1/2100'
SELECT po.Ship_Date, po.PO, po.Ship_City + ', ' + po.Ship_State AS City_State, 
	po.Arrive_Date, po.Ship_via, po.Comments, po.Priority, po.ShelfType, po.Ship_Name, po.SoldName
FROM Tbl_PO po
WHERE po.CancelPO = 0 
	AND po.ID IN (SELECT poi.ID
			FROM Tbl_PO_Items poi INNER JOIN Tbl_Catalog cat ON cat.Material = poi.Material WHERE cat.Medite = 0)
	AND po.Ship_Date Between @ShipFromDate And @ShipToDate 
	AND CASE 
		WHEN @SoldName IS NULL THEN 1  
		WHEN po.SoldName Like @SoldName THEN 1 
		 ELSE 0 END  = 1
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE dbo.sp_Report_GapOrderConf
	@ShipFromDate nvarchar(20) = null, 
	@ShipToDate nvarchar(20) = null,
	@SoldName nvarchar(100) = null,
	@StoreSelName nvarchar(100) = null,
	@OrderFromDate nvarchar(20) = null, 
	@OrderToDate nvarchar(20) = null
AS
IF @ShipFromDate IS NULL
	SELECT @ShipFromDate = '1/1/1900'
IF @ShipToDate IS NULL
	SELECT @ShipToDate = '1/1/2100'
IF @OrderFromDate IS NULL
	SELECT @OrderFromDate = '1/1/1900'
IF @OrderToDate IS NULL
	SELECT @OrderToDate = '1/1/2100'
SELECT po.Ship_City + ', ' + po.Ship_State AS City_State, po.PO, po.ShelfType, po.Arrive_Date, po.Ship_Date, po.Ship_Via, 
	po.Date_Ordered, po.Comments, po.Priority
FROM Tbl_PO po
WHERE po.CancelPO = 0
	AND po.Ship_Date Between @ShipFromDate And @ShipToDate 
	AND po.Date_Ordered Between @OrderFromDate And @OrderToDate 
	AND CASE 
		WHEN @SoldName IS NULL THEN 1  
		WHEN po.SoldName Like @SoldName THEN 1 
		 ELSE 0 END  = 1
	AND CASE 
		WHEN @StoreSelName IS NULL THEN 1  
		WHEN po.Store IN (SELECT Store FROM ztmpStoreSel WHERE StoreSelName = @StoreSelName) THEN 1 
		 ELSE 0 END  = 1
ORDER BY po.Ship_Date
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.sp_Report_HotOrders
	@ShipFromDate nvarchar(20) = null, 
	@ShipToDate nvarchar(20) = null,
	@SoldName nvarchar(100) = null	
AS
IF @ShipFromDate IS NULL
	SELECT @ShipFromDate = '1/1/1900'
IF @ShipToDate IS NULL
	SELECT @ShipToDate = '1/1/2100'
SELECT po.Ship_Date, po.Ship_City + ', ' + po.Ship_State AS City_State, po.Date_Ordered, po.SoldName, po.Ship_Via, po.ID, po.PO, 
	spoi.ExtQty, spoi.ExtPrice, spoi.ExtWeight
FROM Tbl_PO po
	INNER JOIN (SELECT Tbl_PO_Items.ID, 
			Sum([Weight]*[Quanity]) AS ExtWeight, Sum([Unit_Price]*[Quanity]) AS ExtPrice,
			 Sum([Quanity]) AS ExtQty FROM Tbl_Catalog INNER JOIN Tbl_PO_Items ON Tbl_Catalog.Material = Tbl_PO_Items.Material GROUP BY Tbl_PO_Items.ID) spoi ON po.ID = spoi.ID
WHERE po.CancelPO = 0 AND po.Priority = 'Hot Order'
	AND po.Ship_Date Between @ShipFromDate And @ShipToDate 
	AND CASE 
		WHEN @SoldName IS NULL THEN 1  
		WHEN po.SoldName Like @SoldName THEN 1 
		 ELSE 0 END  = 1
ORDER BY po.Ship_Date
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.sp_Report_IncompletePOs
	@SoldName nvarchar(100) = null	
AS
SELECT po.Status, po.PO, po.Date_Ordered, po.Ship_Date, po.Arrive_Date, 
	po.Ship_Name, po.Comments, po.Status, po.SoldName 
FROM Tbl_PO po
WHERE po.Status IS NOT NULL AND po.Status <> 'Completed' AND po.CancelPO = 0
	AND CASE 
		WHEN @SoldName IS NULL THEN 1  
		WHEN po.SoldName Like @SoldName THEN 1 
		 ELSE 0 END  = 1
ORDER BY po.Ship_Date, po.Ship_Name
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE dbo.sp_Report_Invoice
	@InvoiceID int
AS
SELECT inv.ID, inv.Date_Ordered, inv.Ship_Date, inv.Arrive_Date, 
	-- Sold to address
	inv.Sold_Name AS Sold_Line1, 
	CASE	WHEN slda.Attention IS NOT NULL THEN 'ATTN: ' + slda.Attention
		ELSE	po.SoldTo_Address1
	END AS Sold_Line2,
	CASE
		WHEN slda.Attention IS NOT NULL THEN po.SoldTo_Address1
		WHEN po.SoldTo_Address2 IS NOT NULL THEN po.SoldTo_Address2
		ELSE ISNULL(po.SoldTo_City, '') + ', ' + ISNULL(po.SoldTo_State, '') + '  ' + ISNULL(po.SoldTo_Zip, '')
	END AS Sold_Line3,
	CASE
		WHEN slda.Attention IS NULL AND po.SoldTo_Address2 IS NULL THEN NULL
		ELSE ISNULL(po.SoldTo_City, '') + ', ' + ISNULL(po.SoldTo_State, '') + '  ' + ISNULL(po.SoldTo_Zip, '')
	END AS Sold_Line4,
	
	-- Shipping address
	ISNULL(inv.Shipped_Name, '') + ISNULL('  #' + inv.Shipped_StoreNo, '') AS Ship_Line1,
	CASE
		WHEN inv.Shipped_Address2 IS NULL AND inv.Shipped_Attn IS NOT NULL THEN inv.Shipped_Attn
		ELSE inv.Shipped_Address1
	END AS Ship_Line2, 
	CASE
		WHEN inv.Shipped_Address2 IS NULL AND inv.Shipped_Attn IS NOT NULL THEN inv.Shipped_Address1
		WHEN inv.Shipped_Address2 IS NOT NULL THEN inv.Shipped_Address2
		ELSE inv.Shipped_City + ', ' + inv.Shipped_State + '  ' + inv.Shipped_Zip 
	END AS Ship_Line3,
	CASE
		WHEN inv.Shipped_Address2 IS NULL AND inv.Shipped_Attn IS NULL THEN NULL
		ELSE inv.Shipped_City + ', ' + inv.Shipped_State + '  ' + inv.Shipped_Zip 
	END AS Ship_Line4,
	
	inv.PO, inv.Delivery, inv.Salesperson, inv.Tax_Rate, inv.Shipped_State,
	inv.Credit, inv.Comments, inv.[Pro #], inv.[Account#], 
	invi.MatDescription, invi.Quantiy, invi.Unit_Price, invi.Quantiy * invi.Unit_Price AS LineExtPrice, 
	invi.Material, invi.apkInvoiceItem, 
	t1.ExtPrice AS ExtTotal, ISNULL(t1.Tax_Rate,0)/100 * t1.ExtPrice AS TaxPrice, 
	ISNULL(t1.Tax_Rate/100 * t1.ExtPrice,0) + t1.ExtPrice AS TotalPrice, 
	gl.Invoice_Thank, sld.SoldDiscount
FROM 	Tbl_Invoice inv
	LEFT OUTER JOIN Tbl_Invoice_Items invi ON invi.ID = inv.ID
	INNER JOIN 	(SELECT inv.ID, SUM(Quantiy * Unit_Price) AS ExtPrice, ISNULL(inv.Tax_Rate,0) AS Tax_Rate
			FROM Tbl_Invoice inv INNER JOIN Tbl_Invoice_Items invi ON inv.ID = invi.ID
			GROUP BY inv.ID, inv.Tax_Rate) as t1 ON t1.ID = inv.ID
	CROSS JOIN Tbl_Globals gl
	INNER JOIN Tbl_PO po ON po.ID = inv.ID
	LEFT OUTER JOIN tblSold sld ON po.SoldID = sld.SoldID
	LEFT OUTER JOIN tblSoldAttention slda ON slda.SoldAttentionID = po.SoldAttentionID
WHERE inv.ID = @InvoiceID
ORDER BY inv.ID, invi.apkInvoiceItem
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.sp_Report_InvoiceSummary
	@ShipFromDate nvarchar(20) = null, 
	@ShipToDate nvarchar(20) = null,
	@SoldName nvarchar(100) = null, 
	@Classification nvarchar(100) = null,
	@ReferenceNumber nvarchar(100) = null
AS
IF @ShipFromDate IS NULL
	SELECT @ShipFromDate = '1/1/1900'
IF @ShipToDate IS NULL
	SELECT @ShipToDate = '1/1/2100'
SELECT inv.Ship_Date, inv.ID, inv.Shipped_City, inv.Shipped_State, inv.PO, sinvi.ExtPrice
FROM Tbl_Invoice inv
	INNER JOIN (SELECT ID, Sum([Unit_Price]*[Quantiy]) AS ExtPrice FROM Tbl_Invoice_Items GROUP BY ID)  sinvi ON sinvi.ID = inv.ID
	INNER JOIN Tbl_PO po ON po.ID = inv.ID
WHERE inv.Ship_Date Between @ShipFromDate And @ShipToDate 
	AND CASE 
		WHEN @SoldName IS NULL THEN 1  
		WHEN po.SoldName Like @SoldName THEN 1 
		 ELSE 0 END  = 1
	AND CASE 
		WHEN @Classification IS NULL THEN 1  
		WHEN po.Classification Like @Classification THEN 1 
		 ELSE 0 END  = 1
	AND CASE 
		WHEN @ReferenceNumber IS NULL THEN 1  
		WHEN  po.RefNum Like @ReferenceNumber + '%' THEN 1 
		 ELSE 0 END  = 1
ORDER BY inv.Ship_Date, inv.ID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.sp_Report_MaintOrders
	@ShipFromDate nvarchar(20) = null, 
	@ShipToDate nvarchar(20) = null,
	@SoldName nvarchar(100) = null	
AS
IF @ShipFromDate IS NULL
	SELECT @ShipFromDate = '1/1/1900'
IF @ShipToDate IS NULL
	SELECT @ShipToDate = '1/1/2100'
SELECT po.Ship_Date, po.Ship_City + ', ' + po.Ship_State AS City_State, po.Date_Ordered, po.SoldName, po.Ship_Via, po.ID, po.PO, 
	spoi.ExtQty, spoi.ExtPrice, spoi.ExtWeight
FROM Tbl_PO po
	INNER JOIN (SELECT Tbl_PO_Items.ID, 
			Sum([Weight]*[Quanity]) AS ExtWeight, Sum([Unit_Price]*[Quanity]) AS ExtPrice,
			 Sum([Quanity]) AS ExtQty FROM Tbl_Catalog INNER JOIN Tbl_PO_Items ON Tbl_Catalog.Material = Tbl_PO_Items.Material GROUP BY Tbl_PO_Items.ID) spoi ON po.ID = spoi.ID
WHERE po.CancelPO = 0 AND po.Priority = 'Maintence Order'
	AND po.Ship_Date Between @ShipFromDate And @ShipToDate 
	AND CASE 
		WHEN @SoldName IS NULL THEN 1  
		WHEN po.SoldName Like @SoldName THEN 1 
		 ELSE 0 END  = 1
ORDER BY po.Ship_Date
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.sp_Report_OutstandingPOs
	@ShipFromDate nvarchar(20) = null, 
	@ShipToDate nvarchar(20) = null,
	@SoldName nvarchar(100) = null, 
	@Classification nvarchar(100) = null,
	@ReferenceNumber nvarchar(100) = null
AS
IF @ShipFromDate IS NULL
	SELECT @ShipFromDate = '1/1/1900'
IF @ShipToDate IS NULL
	SELECT @ShipToDate = '1/1/2100'
SELECT po.Ship_Date, po.Ship_City + ', ' + po.Ship_State AS City_State, po.Date_Ordered, po.Ship_Via, po.ID, po.PO, 
	po.ShelfType, po.Arrive_Date, po.Ship_Name, po.Comments
FROM Tbl_PO po
WHERE po.CancelPO = 0 AND (po.Status IS NOT NULL AND po.Status <> 'Completed')
	AND po.Ship_Date Between @ShipFromDate And @ShipToDate 
	AND CASE 
		WHEN @SoldName IS NULL THEN 1  
		WHEN po.SoldName Like @SoldName THEN 1 
		 ELSE 0 END  = 1
	AND CASE 
		WHEN @Classification IS NULL THEN 1  
		WHEN po.Classification Like @Classification THEN 1 
		 ELSE 0 END  = 1
	AND CASE 
		WHEN @ReferenceNumber IS NULL THEN 1  
		WHEN  po.RefNum Like @ReferenceNumber + '%' THEN 1 
		 ELSE 0 END  = 1
ORDER BY po.Ship_Date
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.sp_Report_POsByJobNumber
	@ShipFromDate nvarchar(20) = null, 
	@ShipToDate nvarchar(20) = null,
	@SoldName nvarchar(100) = null, 
	@JobNumber nvarchar(100) = null
AS
IF @ShipFromDate IS NULL
	SELECT @ShipFromDate = '1/1/1900'
IF @ShipToDate IS NULL
	SELECT @ShipToDate = '1/1/2100'
SELECT po.PO, po.Ship_City + ', ' + po.Ship_State AS City_State, po.Arrive_Date, po.Ship_Date, po.Ship_Via, po.Comments, po.Ship_Name, 
	poi.Quanity, poi.Material, poi.MatDescription, poi.Unit_Price, poi.Quanity * poi.Unit_Price AS ExtPrice, poi.Phase, cat.Weight
FROM Tbl_PO po
	LEFT OUTER JOIN Tbl_PO_Items poi ON poi.ID = po.ID
	LEFT OUTER JOIN Tbl_Catalog cat ON poi.Material = cat.Material
WHERE po.CancelPO = 0
	AND po.Ship_Date Between @ShipFromDate And @ShipToDate 
	AND CASE 
		WHEN @SoldName IS NULL THEN 1  
		WHEN po.SoldName Like @SoldName THEN 1 
		 ELSE 0 END  = 1
	AND CASE 
		WHEN @JobNumber IS NULL THEN 1  
		WHEN po.Job_Number Like @JobNumber + '%' THEN 1 
		 ELSE 0 END  = 1
ORDER BY po.Ship_Date
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.sp_Report_ProductCatalog
	@CustID int = null	
AS
SELECT cath.Heading_Desc, cat.Material, cat.Description, cat.Price, cat.Weight, cath.Heading_Number, cat.CatOrder
FROM Tbl_Catalog_Headings cath
	INNER JOIN Tbl_Catalog cat ON cath.apkHeading = cat.afkCatalogHeading 
WHERE cat.Hide = 0
	AND CASE 
		WHEN @CustID IS NULL THEN 1  
		WHEN cath.afkHeadCust = @CustID THEN 1 
		 ELSE 0 END  = 1
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.sp_Report_ShipmentSchedule
	@ShipFromDate nvarchar(20) = null, 
	@ShipToDate nvarchar(20) = null,
	@SoldName nvarchar(100) = null,
	@Medite bit = null,
	@StoreSelName nvarchar(100) = null,
	@ShipVia nvarchar(100) = null,
	@RefNum nvarchar(100) = null
AS
IF @ShipFromDate IS NULL
	SELECT @ShipFromDate = '1/1/1900'
IF @ShipToDate IS NULL
	SELECT @ShipToDate = '1/1/2100'
SELECT po.PO, po.Ship_City + ', ' + po.Ship_State AS City_State, po.ShelfType, po.Arrive_Date, po.Ship_Date, po.Ship_Via, 
	po.Comments, po.Ship_Name, spoi2.ExtPrice, spoi2.ExtQty, spoi2.ExtWeight, po.FTofTruckAmount
FROM Tbl_PO po
	INNER JOIN (SELECT Tbl_PO_Items.ID, 
				Sum([Weight]*[Quanity]) AS ExtWeight, Sum([Unit_Price]*[Quanity]) AS ExtPrice,
				 Sum([Quanity]) AS ExtQty 
			FROM Tbl_Catalog INNER JOIN Tbl_PO_Items ON Tbl_Catalog.Material = Tbl_PO_Items.Material 
			WHERE 
				CASE
					WHEN @Medite IS NULL THEN 1  
						-- if medite not null, we want to only sum invoice items that are wheatboard
					WHEN  Medite=@Medite THEN 1 
					 ELSE 0 END  = 1
			GROUP BY Tbl_PO_Items.ID) 
		spoi2 ON po.ID = spoi2.ID
WHERE po.CancelPO = 0
	AND po.Ship_Date Between @ShipFromDate And @ShipToDate 
	AND CASE 
		WHEN @SoldName IS NULL THEN 1  
		WHEN po.SoldName Like @SoldName THEN 1 
		 ELSE 0 END  = 1
	AND CASE 
		WHEN @Medite IS NULL THEN 1  
			-- if medite not null, we want to check if any item on invoice is a wheatboard or any item not a wheatboard
		WHEN  EXISTS(select ID from Tbl_PO_Items poi INNER JOIN Tbl_Catalog cat ON cat.Material=poi.Material  where poi.ID=po.ID and cat.Medite=@Medite) THEN 1 
		 ELSE 0 END  = 1
	AND CASE 
		WHEN @StoreSelName IS NULL THEN 1  
		WHEN po.Store IN (SELECT Store FROM ztmpStoreSel WHERE StoreSelName = @StoreSelName) THEN 1 
		 ELSE 0 END  = 1
	AND CASE 
		WHEN @ShipVia IS NULL THEN 1  
		WHEN po.Ship_Via Like @ShipVia THEN 1 
		 ELSE 0 END  = 1
	AND CASE
		WHEN @RefNum IS NULL THEN 1
		WHEN po.RefNum = @RefNum THEN 1
		ELSE 0 END = 1
ORDER BY po.Ship_Date, po.PO
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE dbo.sp_Report_ShipmentSummary
	@ShipFromDate nvarchar(20) = null, 
	@ShipToDate nvarchar(20) = null,
	@SoldName nvarchar(100) = null	
AS
IF @ShipFromDate IS NULL
	SELECT @ShipFromDate = '1/1/1900'
IF @ShipToDate IS NULL
	SELECT @ShipToDate = '1/1/2100'
SELECT po.Store AS StoreGroup, Count(po.PO) AS CountOfPO, Sum([Quanity]*[Unit_Price]) AS Amount 
FROM Tbl_PO po
	INNER JOIN Tbl_PO_Items poi ON po.ID = poi.ID
WHERE po.CancelPO = 0 
	AND po.Ship_Date Between @ShipFromDate And @ShipToDate 
	AND CASE 
		WHEN @SoldName IS NULL THEN 1  
		WHEN po.SoldName Like @SoldName THEN 1 
		 ELSE 0 END  = 1
GROUP BY po.Store
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE dbo.sp_Report_TotalSalesByClass
	@ShipFromDate nvarchar(20) = null, 
	@ShipToDate nvarchar(20) = null,
	@SoldName nvarchar(100) = null,
	@Classification nvarchar(50) = null,
	@StoreSelName nvarchar(100) = null
AS
SET NOCOUNT ON
IF @ShipFromDate IS NULL
	SELECT @ShipFromDate = '1/1/1900'
IF @ShipToDate IS NULL
	SELECT @ShipToDate = '1/1/2100'

-- convert string date fields to actual dates
DECLARE @dShipFromDate datetime
SELECT @dShipFromDate = CONVERT(datetime, @ShipFromDate)
DECLARE @dShipToDate datetime
SELECT @dShipToDate = CONVERT(datetime, @ShipToDate)


-- Create temp table to retun results
CREATE TABLE #tmpTotals (
	Material nvarchar(50),
	Description nvarchar(50),
	Quantity int,
	Stores int,
	Price money )
-- Insert cat rows
INSERT #tmpTotals (Material, Description)
SELECT Material, Description
FROM Tbl_Catalog
-- Update material rows with qty, price
UPDATE #tmpTotals
SET Quantity = tbl1.ExtQty, Price = tbl1.ExtPrice
FROM #tmpTotals
	INNER JOIN (
		SELECT invi.Material, Sum(ISNULL(invi.Quantiy,0)) AS ExtQty, Sum(ISNULL(invi.Quantiy,0) * ISNULL(invi.Unit_Price,0)) AS ExtPrice
		FROM Tbl_Invoice_Items invi
			INNER JOIN Tbl_Invoice inv ON inv.ID = invi.ID
			INNER JOIN Tbl_PO po ON inv.ID = po.ID
		WHERE inv.Ship_Date Between @dShipFromDate And @dShipToDate 
			AND CASE 
				WHEN @SoldName IS NULL THEN 1  
				WHEN inv.Sold_Name Like @SoldName THEN 1 
				 ELSE 0 END  = 1
			AND CASE 
				WHEN @Classification IS NULL THEN 1  
				WHEN po.Classification Like @Classification THEN 1 
				 ELSE 0 END  = 1
			AND CASE 
				WHEN @StoreSelName IS NULL THEN 1  
				WHEN po.Store IN (SELECT Store FROM ztmpStoreSel WHERE StoreSelName = @StoreSelName) THEN 1 
				 ELSE 0 END  = 1
		GROUP BY invi.Material
		) tbl1 ON tbl1.Material = #tmpTotals.Material
-- Update material rows with store cnt
UPDATE #tmpTotals
SET Stores = tbl1.StoreCnt
FROM #tmpTotals
	INNER JOIN (
		SELECT invi.Material, Count(ISNULL(inv.Shipped_StoreNo,0)) AS StoreCnt
		FROM Tbl_Invoice_Items invi
			INNER JOIN Tbl_Invoice inv ON inv.ID = invi.ID
			INNER JOIN Tbl_PO po on po.ID = inv.ID
		WHERE inv.Ship_Date Between @dShipFromDate And @dShipToDate 
			AND CASE 
				WHEN @SoldName IS NULL THEN 1  
				WHEN inv.Sold_Name Like @SoldName THEN 1 
				 ELSE 0 END  = 1
			AND CASE 
				WHEN @Classification IS NULL THEN 1  
				WHEN po.Classification Like @Classification THEN 1 
				 ELSE 0 END  = 1
			AND CASE 
				WHEN @StoreSelName IS NULL THEN 1  
				WHEN po.Store IN (SELECT Store FROM ztmpStoreSel WHERE StoreSelName = @StoreSelName) THEN 1 
				 ELSE 0 END  = 1
		GROUP BY invi.Material
		) tbl1 ON tbl1.Material = #tmpTotals.Material
DELETE FROM #tmpTotals WHERE Quantity IS NULL
SELECT * FROM #tmpTotals ORDER BY Material
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE dbo.sp_Report_WisconsinTaxes
	@ShipFromDate nvarchar(20) = null, 
	@ShipToDate nvarchar(20) = null,
	@SoldName nvarchar(100) = null
AS
IF @ShipFromDate IS NULL
	SELECT @ShipFromDate = '1/1/1900'
IF @ShipToDate IS NULL
	SELECT @ShipToDate = '1/1/2100'
SELECT inv.Ship_Date, inv.Shipped_City, inv.Tax_Rate, SUM(sinvi.ExtPrice) AS ExtPrice, 
	SUM(sinvi.ExtPrice * (inv.Tax_Rate/100)) AS TaxAmount,
	SUM(sinvi.ExtPrice * (inv.Tax_Rate/100) + sinvi.ExtPrice) AS TotalAmount
FROM Tbl_Invoice inv
	INNER JOIN (SELECT ID, Sum(Unit_Price * Quantiy) AS ExtPrice
			FROM Tbl_Invoice_Items
			GROUP BY ID
			) sinvi ON sinvi.ID = inv.ID
WHERE inv.Shipped_State = 'WI'
	AND inv.Ship_Date Between @ShipFromDate And @ShipToDate 
	AND CASE 
		WHEN @SoldName IS NULL THEN 1  
		WHEN inv.Sold_Name Like @SoldName THEN 1 
		 ELSE 0 END  = 1
GROUP BY inv.Ship_Date, inv.Shipped_City, inv.Tax_Rate
ORDER BY inv.Ship_Date, inv.Shipped_City
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE dbo.sp_Report_WorkOrder
	@POID int = null
AS
SELECT po.PO, po.Ship_Date, po.Arrive_Date, po.Ship_via, po.Ship_Name, 
	po.Ship_City + ', ' + po.Ship_State AS City_State, po.Comments, po.ID, po.Date_Ordered, po.Ship_StoreNo,
	poi.Quanity, cat.Weight, poi.MatDescription, poi.apkPOItem, cat.Weight * poi.Quanity AS ExtWeight
FROM Tbl_PO po
	LEFT OUTER JOIN Tbl_PO_Items poi ON po.ID = poi.ID
	LEFT OUTER JOIN Tbl_Catalog cat ON poi.Material = cat.Material
WHERE CASE 
		WHEN @POID IS NULL THEN 1  
		WHEN po.ID Like @POID THEN 1 
		 ELSE 0 END  = 1
ORDER BY poi.ID, poi.apkPOItem
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE dbo.sp_Util_GetSoldToAddressInfo
	@SoldID int
AS
select SoldAddress1, SoldAddress2, SoldCity, SoldState, SoldZip
from tblSold
where apkSold = @SoldID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE dbo.sp_Util_GetStoreInfo
	@StoreNumber nvarchar(50)
AS
SELECT Ship_Name, Ship_Address1, Ship_Address2, Ship_City, Ship_State, Ship_Zip, Ship_Attn, Ship_Phone,
	svia.Ship_Via
FROM Tbl_PO po
	LEFT OUTER JOIN Tbl_Ship_Via svia ON svia.State = po.Ship_State
WHERE po.Date_Ordered = (SELECT Max(Date_Ordered) FROM Tbl_PO WHERE Ship_StoreNo = @StoreNumber)
	and po.Ship_StoreNo = @StoreNumber
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE dbo.sp_Web_Cat_Groups
	(
		@SoldID int = null
	)
AS

SET NOCOUNT ON

select apkHeading CatGroupID, Heading_Desc CatGroupDesc
from Tbl_Catalog_Headings
where afkHeadCust = @SoldID
order by Heading_Desc

RETURN 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE dbo.sp_Web_Cat_Items
	(
		@SoldID int = null,
		@CatGroupID int = null,
		@CatGroupDesc nvarchar(100) OUTPUT
	)
AS

SET NOCOUNT ON

-- get the catalog group name
select @CatGroupDesc = Heading_Desc 
from Tbl_Catalog_Headings
where apkHeading = @CatGroupID and afkHeadCust = @SoldID

-- get the catalog items in this group
select Material, Description, Price, Weight
from Tbl_Catalog cat inner join Tbl_Catalog_Headings grp on cat.afkCatalogHeading = grp.apkHeading
where afkCatalogHeading = @CatGroupID and grp.afkHeadCust = @SoldID

RETURN 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE dbo.sp_Web_Login
	(
		@username nvarchar(100) = null,
		@password nvarchar(100) = null
	)
AS

SET NOCOUNT ON

-- Attempt to find a rec in logins
select tSA.SoldID, tS.SoldName, tSA.SoldAttentionID, tSA.Attention
from tblSoldAttention tSA inner join tblSold tS on tS.apkSold = tSA.SoldID
where UserName = @username and Password = @password

RETURN 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE dbo.sp_Web_PO_Find
	(
		@SoldID int = null,
		@Page int = 0,
		@RecsPerPage int = 10,
		@OrderBy nvarchar(50) = 'Date_Ordered',
		@PO nvarchar(50) = null,
		@ShipVia nvarchar(50) = null,
		@ShipName nvarchar(50) = null,
		@ShipCity nvarchar(50) = null,
		@ShipState nvarchar(50) = null,
		@RecordCount int OUTPUT
	)

AS

SET NOCOUNT ON

-- check params
if @OrderBy is null
	select @OrderBy = 'Date_Ordered'

-- Create a temp table for the results
create table #tmpPO (
	RecNumber int IDENTITY,
	ID int
)

-----------------------------------------------------------------------------
-- select any recent POs and insert into table
DECLARE @SoldIDString nvarchar(10), @SQL nvarchar(2500)
select @SoldIDString = @SoldID

-- portion to append to temp table
select @SQL = 'insert into #tmpPO (ID) '

-- simply select the ID
select @SQL = @SQL + 'select po.ID from Tbl_PO po '

-- only return POs for the current customer
select @SQL = @SQL + 'where po.SoldID = ' + @SoldIDString

-- PO
if (@PO is not null and @PO <> '')
	select @SQL = @SQL + ' and po.PO like ''%' + @PO + '%'''

-- Ship Via
if (@ShipVia is not null and @ShipVia <> '')
	select @SQL = @SQL + ' and po.Ship_Via like ''%' + @ShipVia + '%'''

-- Ship Name
if (@ShipName is not null and @ShipName <> '')
	select @SQL = @SQL + ' and po.Ship_Name like ''%' + @ShipName + '%'''

-- Ship City
if (@ShipCity is not null and @ShipCity <> '')
	select @SQL = @SQL + ' and po.Ship_City like ''%' + @ShipCity + '%'''
	
-- Ship State
if (@ShipState is not null and @ShipState <> '')
	select @SQL = @SQL + ' and po.Ship_State like ''%' + @ShipState + '%'''
	

-- sort recs
select @SQL = @SQL + ' order by ' + @OrderBy

-- Now run the SQL to populate the temp table
exec(@SQL)
-----------------------------------------------------------------------------


-- get the total for output
select @RecordCount = @@ROWCOUNT


-- now get the recs starting at @Page * @RecsPerPage
select po.ID, po.Job_Number, po.PO, po.Date_Ordered, po.Ship_Date, po.Ship_Via, po.Ship_Name, po.Ship_City, po.Ship_State
from Tbl_PO po inner join #tmpPO tPO on tPO.ID = po.ID
where tPO.RecNumber > @Page * @RecsPerPage and tPO.RecNumber <= (@Page + 1) * @RecsPerPage
order by tPO.RecNumber



RETURN 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE dbo.sp_Web_PO_Open
	(
		@SoldID int = null,
		@Page int = 0,
		@RecsPerPage int = 10,
		@OrderBy nvarchar(50) = 'Date_Ordered',
		@RecordCount int OUTPUT
	)

AS
--SET NOCOUNT ON


-- check params
if @OrderBy is null
	select @OrderBy = 'Date_Ordered'

-- Create a temp table for the results
create table #tmpPO (
	RecNumber int IDENTITY,
	ID int
)

-- select any recent POs and insert into table
DECLARE @SoldIDString nvarchar(10)
select @SoldIDString = @SoldID
exec(
	'insert into #tmpPO (ID) 
	select po.ID from Tbl_PO po where po.Date_Ordered >= GETDATE()-30 and po.SoldID = ' + @SoldIDString + ' order by ' + @OrderBy
	)

-- get the total for output
select @RecordCount = @@ROWCOUNT --count(*) from #tmpPO


-- now get the recs starting at @Page * @RecsPerPage
select po.ID, po.Job_Number, po.PO, po.Date_Ordered, po.Ship_Date, po.Ship_Via, po.Ship_Name, po.Ship_City, po.Ship_State
from Tbl_PO po inner join #tmpPO tPO on tPO.ID = po.ID
where tPO.RecNumber > @Page * @RecsPerPage and tPO.RecNumber <= (@Page + 1) * @RecsPerPage
order by tPO.RecNumber



RETURN 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE dbo.sp_Web_PO_View
	(
		@SoldID int = null,
		@POID int = null
	)
AS

SET NOCOUNT ON

-- We first want to return the PO info
select ID POID, 
	s.SoldName SoldName, SoldTo_Address1 SoldAddress1, SoldTo_Address2 SoldAddress2, 
	SoldTo_City SoldCity, SoldTo_State SoldState, SoldTo_Zip SoldZip,
	Ship_Name ShipName, Ship_Address1 ShipAddress1, Ship_Address2 ShipAddress2, 
	Ship_City ShipCity, Ship_State ShipState, Ship_Zip ShipZip, 
	PO, Job_Number JobNumber, Date_Ordered DateOrdered, Arrive_Date ArriveDate, 
	[Pro#] ProNum, Ship_Via ShipVia, Ship_Attn ShipAttn, Ship_Phone ShipPhone, 
	Salesperson, Purchaser
from Tbl_PO po
	inner join tblSold s on s.apkSold = po.SoldID
where po.ID = @POID

RETURN 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE dbo.sp_Web_PO_View_Items
	(
		@SoldID int = null,
		@POID int = null
	)
AS

SET NOCOUNT ON

-- now we want to select PO items
select poi.apkPOItem POItemID, 
	poi.Material RFMaterial, cat.CustMaterial, poi.MatDescription Description, poi.Quanity Quantity, 
	poi.Unit_Price Price, poi.Unit_Quantiy Unit, 
	poi.Quanity * poi.Unit_Price Total
from Tbl_PO_Items poi
	inner join Tbl_PO po ON po.ID = poi.ID
	left outer join Tbl_Catalog cat ON poi.Material = cat.Material
where po.SoldID = @SoldID and poi.ID = @POID

RETURN 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



insert ztblReleaseNotes (Date, MajorVersion, MinorVersion, Notes)
values ('8/13/2001', 3.24, 0, '- Catalog: Added ''Cust Material #'' to RFC Catalog
- Catalog:  Added search for cust material #
- POs:  Added cust material # for PO items
- POs:  Added a highlight to the currently active text field
- POs:  Changed enter key behavior after entering a sold to attention name to move to the correct field
')
go

