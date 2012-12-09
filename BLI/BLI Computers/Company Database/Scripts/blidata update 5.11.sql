/* blidata update 5.11 */
/* - adds payment columns to sales table */
/* - adds payment table for sales order payments */
/* - updates spConvertStoI to copy payment info too */
/* - updates item list to include source of item list */

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblSale
	DROP CONSTRAINT DF_tblSale_afkQSI
GO
ALTER TABLE dbo.tblSale
	DROP CONSTRAINT DF__Temporary__afkSa__282DF8C2
GO
ALTER TABLE dbo.tblSale
	DROP CONSTRAINT DF__Temporary__afkSa__2A164134
GO
ALTER TABLE dbo.tblSale
	DROP CONSTRAINT DF__Temporary__SaleD__2B0A656D
GO
ALTER TABLE dbo.tblSale
	DROP CONSTRAINT DF__Temporary__SaleS__2BFE89A6
GO
ALTER TABLE dbo.tblSale
	DROP CONSTRAINT DF__Temporary__SaleC__2CF2ADDF
GO
ALTER TABLE dbo.tblSale
	DROP CONSTRAINT DF__Temporary__SaleT__2DE6D218
GO
ALTER TABLE dbo.tblSale
	DROP CONSTRAINT DF__Temporary__SaleP__2EDAF651
GO
ALTER TABLE dbo.tblSale
	DROP CONSTRAINT DF__Temporary__SaleS__2FCF1A8A
GO
ALTER TABLE dbo.tblSale
	DROP CONSTRAINT DF__Temporary__SaleT__30C33EC3
GO
ALTER TABLE dbo.tblSale
	DROP CONSTRAINT DF__Temporary__SaleT__31B762FC
GO
ALTER TABLE dbo.tblSale
	DROP CONSTRAINT DF__Temporary__afkSa__37703C52
GO
ALTER TABLE dbo.tblSale
	DROP CONSTRAINT DF__Temporary__SaleD__3864608B
GO
ALTER TABLE dbo.tblSale
	DROP CONSTRAINT DF_tblSale_SaleNumberToday
GO
ALTER TABLE dbo.tblSale
	DROP CONSTRAINT DF_tblSale_z_apkSale
GO
CREATE TABLE dbo.Tmp_tblSale
	(
 	apkSale int NOT NULL IDENTITY (1, 1),
	afkQSI int NULL CONSTRAINT DF_tblSale_afkQSI DEFAULT (0),
	afkSaleCust int NULL CONSTRAINT DF__Temporary__afkSa__282DF8C2 DEFAULT (0),
	afkSaleEmp int NULL CONSTRAINT DF__Temporary__afkSa__2A164134 DEFAULT (0),
	SaleTitle nvarchar(255) NULL,
	SaleNumber nvarchar(50) NULL,
	SalePONumber nvarchar(50) NULL,
	SaleDate datetime NULL,
	SaleDiscount smallmoney NULL CONSTRAINT DF__Temporary__SaleD__2B0A656D DEFAULT (0),
	SaleSubTotal smallmoney NULL CONSTRAINT DF__Temporary__SaleS__2BFE89A6 DEFAULT (0),
	SalePayments bit NOT NULL CONSTRAINT DF_tblSale_InvPayments DEFAULT (0),
	SalePaymentAmt smallmoney NULL CONSTRAINT DF_tblSale_InvPaymentAmt DEFAULT (0),
	SaleAmountDue smallmoney NULL CONSTRAINT DF_tblSale_InvAmountDue DEFAULT (0),
	SaleCost smallmoney NULL CONSTRAINT DF__Temporary__SaleC__2CF2ADDF DEFAULT (0),
	SaleTax smallmoney NULL CONSTRAINT DF__Temporary__SaleT__2DE6D218 DEFAULT (0),
	SaleProfit smallmoney NULL CONSTRAINT DF__Temporary__SaleP__2EDAF651 DEFAULT (0),
	SaleShipping smallmoney NULL CONSTRAINT DF__Temporary__SaleS__2FCF1A8A DEFAULT (0),
	SaleTotal smallmoney NULL CONSTRAINT DF__Temporary__SaleT__30C33EC3 DEFAULT (0),
	SaleTaxExemptFlag bit NOT NULL CONSTRAINT DF__Temporary__SaleT__31B762FC DEFAULT (0),
	afkSaleTerms int NULL CONSTRAINT DF__Temporary__afkSa__37703C52 DEFAULT (0),
	SaleNote ntext NULL,
	SaleInternalNote ntext NULL,
	SaleDiscountRate real NULL CONSTRAINT DF__Temporary__SaleD__3864608B DEFAULT (0),
	SaleDiscountNote nvarchar(255) NULL,
	SaleNumberToday int NULL CONSTRAINT DF_tblSale_SaleNumberToday DEFAULT (0),
	z_apkSale int NULL CONSTRAINT DF_tblSale_z_apkSale DEFAULT (0)
	) ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblSale ON
GO
IF EXISTS(SELECT * FROM dbo.tblSale)
	 EXEC('INSERT INTO dbo.Tmp_tblSale(apkSale, afkQSI, afkSaleCust, afkSaleEmp, SaleTitle, SaleNumber, SalePONumber, SaleDate, SaleDiscount, SaleSubTotal, SaleCost, SaleTax, SaleProfit, SaleShipping, SaleTotal, SaleTaxExemptFlag, afkSaleTerms, SaleNote, SaleInternalNote, SaleDiscountRate, SaleDiscountNote, SaleNumberToday, z_apkSale)
		SELECT apkSale, afkQSI, afkSaleCust, afkSaleEmp, SaleTitle, SaleNumber, SalePONumber, SaleDate, SaleDiscount, SaleSubTotal, SaleCost, SaleTax, SaleProfit, SaleShipping, SaleTotal, SaleTaxExemptFlag, afkSaleTerms, SaleNote, SaleInternalNote, SaleDiscountRate, SaleDiscountNote, SaleNumberToday, z_apkSale FROM dbo.tblSale TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblSale OFF
GO
ALTER TABLE dbo.tblSaleItem
	DROP CONSTRAINT tblSaleItem_FK00
GO
DROP TABLE dbo.tblSale
GO
EXECUTE sp_rename 'dbo.Tmp_tblSale', 'tblSale'
GO
ALTER TABLE dbo.tblSale ADD CONSTRAINT
	aaaaatblSale_PK PRIMARY KEY NONCLUSTERED 
	(
	apkSale
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [{4D226C6D-AB3C-4E9A-9796-B9B5B54FA51C}] ON dbo.tblSale
	(
	afkSaleEmp
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [{9475F40C-5A20-4CFE-90C8-A1C03FD2BF1D}] ON dbo.tblSale
	(
	afkSaleCust
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX CustomerNumber ON dbo.tblSale
	(
	afkSaleCust
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX SalesmanID ON dbo.tblSale
	(
	afkSaleEmp
	) ON [PRIMARY]
GO
CREATE TRIGGER tblSale_DTrig ON dbo.tblSale FOR DELETE AS
SET NOCOUNT ON

/* * CASCADE DELETES TO 'tblSaleItem' */
DELETE tblSaleItem FROM deleted, tblSaleItem WHERE deleted.apkSale = tblSaleItem.afkSItemSale
GO
CREATE TRIGGER tblSale_UTrig ON dbo.tblSale FOR UPDATE AS
SET NOCOUNT ON
/* * CASCADE UPDATES TO 'tblSaleDisc' */
IF UPDATE(apkSale)
    BEGIN
       UPDATE tblSaleDisc
       SET tblSaleDisc.afkSDiscSale = inserted.apkSale
       FROM tblSaleDisc, deleted, inserted
       WHERE deleted.apkSale = tblSaleDisc.afkSDiscSale
    END

/* * PREVENT UPDATES IF NO MATCHING KEY IN 'tblEmp' */
IF UPDATE(afkSaleEmp)
    BEGIN
        IF (SELECT COUNT(*) FROM inserted) !=
           (SELECT COUNT(*) FROM tblEmp, inserted WHERE (tblEmp.apkEmp = inserted.afkSaleEmp))
            BEGIN
                RAISERROR 44446 'The record can''t be added or changed. Referential integrity rules require a related record in table ''tblEmp''.'
                ROLLBACK TRANSACTION
            END
    END

/* * PREVENT UPDATES IF NO MATCHING KEY IN 'tblCust' */
IF UPDATE(afkSaleCust)
    BEGIN
        IF (SELECT COUNT(*) FROM inserted) !=
           (SELECT COUNT(*) FROM tblCust, inserted WHERE (tblCust.apkCust = inserted.afkSaleCust))
            BEGIN
                RAISERROR 44446 'The record can''t be added or changed. Referential integrity rules require a related record in table ''tblCust''.'
                ROLLBACK TRANSACTION
            END
    END

/* * CASCADE UPDATES TO 'tblSaleItem' */
IF UPDATE(apkSale)
    BEGIN
       UPDATE tblSaleItem
       SET tblSaleItem.afkSItemSale = inserted.apkSale
       FROM tblSaleItem, deleted, inserted
       WHERE deleted.apkSale = tblSaleItem.afkSItemSale
    END
GO
CREATE TRIGGER tblSale_ITrig ON dbo.tblSale FOR INSERT AS
SET NOCOUNT ON
/* * PREVENT INSERTS IF NO MATCHING KEY IN 'tblEmp' */
IF (SELECT COUNT(*) FROM inserted) !=
   (SELECT COUNT(*) FROM tblEmp, inserted WHERE (tblEmp.apkEmp = inserted.afkSaleEmp))
    BEGIN
        RAISERROR 44447 'The record can''t be added or changed. Referential integrity rules require a related record in table ''tblEmp''.'
        ROLLBACK TRANSACTION
    END

/* * PREVENT INSERTS IF NO MATCHING KEY IN 'tblCust' */
IF (SELECT COUNT(*) FROM inserted) !=
   (SELECT COUNT(*) FROM tblCust, inserted WHERE (tblCust.apkCust = inserted.afkSaleCust))
    BEGIN
        RAISERROR 44447 'The record can''t be added or changed. Referential integrity rules require a related record in table ''tblCust''.'
        ROLLBACK TRANSACTION
    END
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblSaleItem WITH NOCHECK ADD CONSTRAINT
	tblSaleItem_FK00 FOREIGN KEY
	(
	afkSItemSale
	) REFERENCES dbo.tblSale
	(
	apkSale
	)
GO
COMMIT

CREATE TABLE [dbo].[tblSalePay] (
	[apkSPay] [int] IDENTITY (1, 1) NOT NULL ,
	[afkSPaySale] [int] NULL ,
	[afkSPayEmp] [int] NULL ,
	[SPayNote] [ntext] NULL ,
	[SPayDate] [datetime] NULL ,
	[SPayAmt] [smallmoney] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblSalePay] WITH NOCHECK ADD 
	CONSTRAINT [PK_tblSalePay] PRIMARY KEY  NONCLUSTERED 
	(
		[apkSPay]
	)  ON [PRIMARY] 
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[spConvertStoI]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spConvertStoI]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO




CREATE PROCEDURE spConvertStoI
	@Sale_ID int,
	@CurUserID int

 AS

-- VARIABLES
DECLARE @QSI_ID int
DECLARE @Inv_ID int
DECLARE @QSI_I_ID int
DECLARE @Inv_I_ID int
DECLARE @Sale_I_ID int
DECLARE @Sale_SI_ID int
DECLARE @Inv_SI_ID int
DECLARE @ret int

-- hide counts so access isn't confused
SET NOCOUNT ON

-- make sure quote exists and hasn't been converted
--IF NOT EXISTS (SELECT apkSale FROM tblSale WHERE apkSale = @Sale_ID AND afkQSI = 0)
 -- BEGIN
--	SELECT -1--	RETURN
--  END

-- check for 'warn' items
IF EXISTS(SELECT * FROM tblSaleItem WHERE SItemWarn = 1 AND afkSItemSale = @Sale_ID)
  BEGIN
	SELECT -2
	RETURN
  END	

-- insert row into QSI for tracking, update tblQuote with QSI id
IF NOT EXISTS(SELECT * FROM tblQSI WHERE afkSale = @Sale_ID)
  BEGIN
	INSERT INTO tblQSI (afkSale) VALUES (@Sale_ID)
	SELECT @QSI_ID = @@IDENTITY
  END
ELSE
 BEGIN
	SELECT @QSI_ID = apkQSI FROM tblQSI WHERE afkSale = @Sale_ID
  END

-- insert info from quote table into sale table
INSERT INTO tblInvoice (afkQSI, afkInvCust, afkInvEmp, InvTitle, InvDate, InvCost, InvProfit, InvTotal, InvNote, InvInternalNote, InvTaxExemptFlag,
		InvPONumber, InvShipping, InvDiscountRate, InvDiscountNote, InvPayments)
	SELECT @QSI_ID, afkSaleCust, @CurUserID, SaleTitle, GETDATE(), SaleCost, SaleProfit, SaleTotal, SaleNote, SaleInternalNote, SaleTaxExemptFlag,
		SalePONumber, SaleShipping, SaleDiscountRate, SaleDiscountNote, SalePayments
	FROM tblSale
	WHERE apkSale = @Sale_ID
SELECT @Inv_ID = @@IDENTITY

DECLARE @sDate varchar(6)
SELECT @sDate = SUBSTRING(STR(YEAR(GETDATE()),4,0),3,2)
IF MONTH(GETDATE()) < 10
	SELECT @sDate = @sDate + '0' + STR(MONTH(GETDATE()),1,0)
ELSE
	SELECT @sDate = @sDate + STR(MONTH(GETDATE()),2,0)
IF DAY(GETDATE()) < 10
	SELECT @sDate = @sDate + '0' + STR(DAY(GETDATE()),1,0)
ELSE
	SELECT @sDate = @sDate + STR(DAY(GETDATE()),2,0)
UPDATE tblInvoice SET InvNumberDate = @sDate WHERE apkInv = @Inv_ID
DECLARE @iMaxNum int
IF EXISTS(SELECT InvDate FROM tblInvoice WHERE InvNumberDate = @sDate AND InvNumberToday IS NOT NULL AND apkInv <> @Inv_ID)
	SELECT @iMaxNum = MAX(InvNumberToday) + 1 FROM tblInvoice WHERE InvNumberToday IS NOT NULL AND InvNumberDate = @sDate
ELSE
	SELECT @iMaxNum = 1
UPDATE tblInvoice SET InvNumberToday = @iMaxNum WHERE apkInv = @Inv_ID
DECLARE @sInvNum varchar(2)
IF @iMaxNum < 10
	SELECT @sInvNum = '0' + STR(@iMaxNum,1,0)
ELSE
	SELECT @sInvNum = STR(@iMaxNum,2,0)
UPDATE tblInvoice SET InvNumber = STR(InvNumberDate, 6, 0) + '-' + @sInvNum  WHERE apkInv = @Inv_ID

-- update row in QSI
UPDATE tblQSI SET afkInvoice = @Inv_ID WHERE apkQSI = @QSI_ID

------------------------------------------------------------------------------------------------------------------------

-- now do the sale items

DECLARE cSaleItems CURSOR
FOR SELECT apkSItem FROM tblSaleItem WHERE afkSItemSale = @Sale_ID
FOR READ ONLY
OPEN cSaleItems



FETCH NEXT FROM cSaleItems INTO @Sale_I_ID
WHILE @@FETCH_STATUS = 0
BEGIN
	-- add quote item info to sale item table
	INSERT INTO tblInvoiceItem (afkIItemInv, afkIItemEmp, IItemDesc, IItemPrice, IItemCost, IItemQty, IItemExtCost, IItemExtPrice, 
			IItemDetailFlag, IItemType, IItemOrder, IItemNote)
		SELECT @Inv_ID, @CurUserID, SItemDesc, SItemPrice, SItemCost, SItemQty, SItemExtCost, SItemExtPrice, 
			SItemDetailFlag, SItemType, SItemOrder, SItemNote
		FROM tblSaleItem
		WHERE apkSItem = @Sale_I_ID
	SELECT @Inv_I_ID = @@IDENTITY

	-- now update QSI Item table
	IF NOT EXISTS(SELECT * FROM tblQSISubItems WHERE afkSaleOrderSubItem = @Sale_I_ID)
	  BEGIN
		INSERT INTO tblQSISubItems (afkQSI, afkSaleOrderSubItem, afkInvoiceSubItem)
		VALUES (@QSI_ID, @Sale_I_ID, @Inv_I_ID)
		SELECT @QSI_I_ID = @@IDENTITY
		UPDATE tblSaleItem SET afkQSISubItem = @QSI_I_ID WHERE apkSItem = @Sale_I_ID
	  END
	ELSE
	  BEGIN
		SELECT @QSI_I_ID = apkQSISubItem FROM tblQSISubItems WHERE afkSaleOrderSubItem = @Sale_I_ID
		UPDATE tblQSISubItems SET afkInvoiceSubItem = @Inv_I_ID WHERE apkQSISubItem = @QSI_I_ID
	  END
	UPDATE tblInvoiceItem SET afkQSISubItem = @QSI_I_ID WHERE apkIItem = @Inv_I_ID
	
	-- check if we should add a computer
	IF EXISTS(SELECT apkIItem FROM tblInvoiceItem WHERE IItemType = 3 AND apkIItem = @Inv_I_ID)
		INSERT INTO tblCustComp (afkCCompCust, afkCCompInv, afkCCompIItem)
		SELECT afkInvCust, @Inv_ID, @Inv_I_ID FROM tblInvoice WHERE apkInv = @Inv_ID
	
	-- now mark inventory items
	UPDATE tblInvn SET afkInvnIItem = @Inv_I_ID, afkInvnSItem = 0 WHERE afkInvnSItem = @Sale_I_ID
	UPDATE tblSaleSubItem SET afkSSItemInvn = 0 WHERE afkSaleSItem = @Sale_I_ID
	UPDATE tblSaleItem SET SItemWarn = 1 WHERE apkSItem = @Sale_I_ID AND (SItemType = 1 OR SItemType = 3)

	FETCH NEXT FROM cSaleItems INTO @Sale_I_ID
END	-- done with cSaleItems rows
CLOSE cSaleItems
DEALLOCATE cSaleItems

INSERT INTO tblInvoicePay (afkIPayInv, afkIPayEmp, IPayNote, IPayDate, IPayAmt)
	SELECT @Inv_ID, @CurUserID, SPayNote, SPayDate, SPayAmt
	FROM tblSalePay
	WHERE afkSPaySale = @Sale_ID

-- update totals
UPDATE tblInvoice
	SET 	InvSubTotal = (SELECT SUM(IItemExtPrice) FROM tblInvoiceItem WHERE afkIItemInv = @Inv_ID) ,
		InvCost = (SELECT SUM(IItemExtCost) FROM tblInvoiceItem WHERE afkIItemInv = @Inv_ID) - InvShipping, 
		InvPaymentAmt = (SELECT SUM(IPayAmt) FROM tblInvoicePay WHERE afkIPayInv = @Inv_ID),
		InvTax = 0
	WHERE apkInv = @Inv_ID
UPDATE tblInvoice SET	InvTax = 0.051 * InvSubTotal WHERE apkInv = @Inv_ID AND InvTaxExemptFlag = 0
UPDATE tblInvoice SET InvDiscount = InvSubTotal * (InvDiscountRate/100) WHERE apkInv = @Inv_ID
UPDATE tblInvoice
	SET	InvTotal = InvSubTotal + InvTax + InvShipping - InvDiscount, 
		InvProfit = InvSubTotal - InvCost - InvDiscount,
		InvAmountDue = InvSubTotal + InvTax + InvShipping - InvDiscount
	WHERE apkInv = @Inv_ID

SELECT @Inv_ID
RETURN

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[spConvertStoI]  TO [bliuser]
GO


BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblItemList
	DROP CONSTRAINT DF_tblItemList_iItemGroup
GO
ALTER TABLE dbo.tblItemList
	DROP CONSTRAINT DF_tblItemList_iItemPrice
GO
ALTER TABLE dbo.tblItemList
	DROP CONSTRAINT DF_tblItemList_iItemCost
GO
ALTER TABLE dbo.tblItemList
	DROP CONSTRAINT DF_tblItemList_UserID
GO
CREATE TABLE dbo.Tmp_tblItemList
	(
 	iItemID int NOT NULL IDENTITY (1, 1),
	iItemGroup int NOT NULL CONSTRAINT DF_tblItemList_iItemGroup DEFAULT (0),
	iItemDesc nvarchar(50) NULL,
	iItemPrice smallmoney NOT NULL CONSTRAINT DF_tblItemList_iItemPrice DEFAULT (0),
	iItemCost smallmoney NOT NULL CONSTRAINT DF_tblItemList_iItemCost DEFAULT (0),
	iItemSource char(1) NOT NULL CONSTRAINT DF_tblItemList_iItemSource DEFAULT ('I'),
	UserID int NOT NULL CONSTRAINT DF_tblItemList_UserID DEFAULT (0)
	) ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblItemList ON
GO
IF EXISTS(SELECT * FROM dbo.tblItemList)
	 EXEC('INSERT INTO dbo.Tmp_tblItemList(iItemID, iItemGroup, iItemDesc, iItemPrice, iItemCost, UserID)
		SELECT iItemID, iItemGroup, iItemDesc, iItemPrice, iItemCost, UserID FROM dbo.tblItemList TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblItemList OFF
GO
DROP TABLE dbo.tblItemList
GO
EXECUTE sp_rename 'dbo.Tmp_tblItemList', 'tblItemList'
GO
ALTER TABLE dbo.tblItemList ADD CONSTRAINT
	PK_tblItemList PRIMARY KEY NONCLUSTERED 
	(
	iItemID
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblItemList_UserID ON dbo.tblItemList
	(
	UserID
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblItemList_iItemGroup ON dbo.tblItemList
	(
	iItemGroup
	) ON [PRIMARY]
GO
COMMIT


if exists (select * from sysobjects where id = object_id(N'[dbo].[spUpdateItemList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spUpdateItemList]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE Procedure spUpdateItemList
	@iUserID varchar(4) = '0'
As

CREATE TABLE #tmp (
	iGroup	int,
	iDesc	nvarchar(50),
	iSource char(1)
)

CREATE TABLE #rettbl (
	iGroup	int,
	iDesc	nvarchar(50),
	iCost	smallmoney,
	iPrice	smallmoney
)

INSERT INTO #tmp (iGroup, iDesc, iSource)
	SELECT afkInvnGroup AS iGroup, InvnDesc AS iDesc, 'I'
	FROM tblInvn
	UNION
	SELECT afkPItemInvnGroup AS iGroup, PItemDesc AS iDesc, 'P'
	FROM tblPrice

DELETE FROM tblItemList WHERE UserID = @iUserID

INSERT INTO tblItemList (iItemGroup, iItemDesc, UserID)
	SELECT iGroup, iDesc, @iUserID
	FROM #tmp
	GROUP BY iGroup, iDesc
	ORDER BY iGroup, iDesc

UPDATE tblItemList
	SET iItemSource = 'I'
	FROM tblItemList, #tmp
	WHERE tblItemList.iItemGroup  = #tmp.iGroup AND tblItemList.iItemDesc = #tmp.iDesc AND #tmp.iSource = 'I'

UPDATE tblItemList
	SET iItemSource = 'P'
	FROM tblItemList, #tmp
	WHERE tblItemList.iItemGroup  = #tmp.iGroup AND tblItemList.iItemDesc = #tmp.iDesc AND #tmp.iSource = 'P'

UPDATE t
SET t.iItemCost = i.InvnCost
FROM tblItemList t, tblInvn i
WHERE t.IItemGroup = i.afkInvnGroup AND t.iItemDesc = i.InvnDesc AND i.InvnCost <> 0

UPDATE t
SET t.iItemCost = p.PItemBuy
FROM tblItemList t, tblPrice p
WHERE t.iItemGroup = p.afkPItemInvnGroup AND t.iItemDesc = p.PItemDesc AND p.PItemBuy <> 0

UPDATE t
SET t.iItemPrice = p.PItemSell
FROM tblItemList t, tblPrice p
WHERE t.iItemGroup = p.afkPItemInvnGroup AND t.iItemDesc = p.PItemDesc AND p.PItemSell <> 0


SELECT * FROM tblItemList

return 

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[spUpdateItemList]  TO [bliuser]
GO

