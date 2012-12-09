-- blisql Update 5.15
-- - create tblPriceSubItems
-- - update spUpdateItemList
-- - add indexes to tblItemList
-- - update release notes

-- tblPriceSubItems
if exists (select * from sysobjects where id = object_id(N'[dbo].[tblPriceSubItems]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblPriceSubItems]
GO

CREATE TABLE [dbo].[tblPriceSubItems] (
	[apkPSubItem] [int] IDENTITY (1, 1) NOT NULL ,
	[afkPItem] [int] NOT NULL ,
	[afkPItemSub] [int] NOT NULL
) ON [PRIMARY]
GO

CREATE TRIGGER tblPrice_DTrig ON tblPrice
FOR DELETE 
AS
delete from tblPriceSubItem where tblPriceSubItem.afkPItem = deleted.apkPItem
GO

-- spUpdateItemList
if exists (select * from sysobjects where id = object_id(N'[dbo].[spUpdateItemList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spUpdateItemList]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE Procedure spUpdateItemList
	@iUserID varchar(4) = '0'
As

SET NOCOUNT ON

CREATE TABLE #tmp (
	iGroup	int,
	iDesc	nvarchar(50),
	iSource char(1)
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

return 
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[spUpdateItemList]  TO [bliuser]
GO

-- tblItemList
CREATE NONCLUSTERED INDEX IX_tblItemList_iItemDesc ON dbo.tblItemList (iItemDesc)
GO
CREATE NONCLUSTERED INDEX IX_tblItemList_iItemSource ON dbo.tblItemList (iItemSource)
GO
CREATE NONCLUSTERED INDEX IX_tblinvn_GrpDescCost ON dbo.tblinvn (afkinvngroup, invndesc, invncost)
GO
CREATE NONCLUSTERED INDEX IX_tblitemlist_GrpDesc ON dbo.tblitemlist (iitemgroup, iitemdesc)
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
	UPDATE tblSale SET afkQSI = @QSI_ID WHERE apkSale = @Sale_ID
  END
ELSE
 BEGIN
	SELECT @QSI_ID = apkQSI FROM tblQSI WHERE afkSale = @Sale_ID
  END

-- insert info from quote table into sale table
INSERT INTO tblInvoice (afkQSI, afkInvCust, afkInvEmp, InvTitle, InvDate, InvCost, InvProfit, InvTotal, InvNote, InvInternalNote, InvTaxExemptFlag,
		InvPONumber, InvShipping, InvDiscountRate, InvDiscountNote, InvPayments, InvPaymentAmt)
	SELECT @QSI_ID, afkSaleCust, @CurUserID, SaleTitle, GETDATE(), SaleCost, SaleProfit, SaleTotal, SaleNote, SaleInternalNote, SaleTaxExemptFlag,
		SalePONumber, SaleShipping, SaleDiscountRate, SaleDiscountNote, SalePayments, 0
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
		InvPaymentAmt = ISNULL((SELECT SUM(IPayAmt) FROM tblInvoicePay WHERE afkIPayInv = @Inv_ID),0),
		InvTax = 0
	WHERE apkInv = @Inv_ID
UPDATE tblInvoice SET InvDiscount = InvSubTotal * (InvDiscountRate/100) WHERE apkInv = @Inv_ID
UPDATE tblInvoice SET	InvTax = 0.051 * (InvSubTotal - InvDiscount) WHERE apkInv = @Inv_ID AND InvTaxExemptFlag = 0
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


ALTER TABLE dbo.tblInvoice ADD CONSTRAINT DF_tblInvoice_InvDate DEFAULT (GETDATE()) FOR InvDate
GO

ALTER TABLE dbo.tblSale ADD CONSTRAINT DF_tblSale_SaleDate DEFAULT (GETDATE()) FOR SaleDate
GO

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('5.15','1999-11-23 00:00:00','- Added ''Sub Items'' to price list items.  Only items with no other sub items may be added (ie, you can''t ''nest'' levels of items).
- Improved the query item list creation procedure running time.
- Added a ''Select a filter'' drop down to the bottom of the invoices screen to allow a variety of selections.
- Fixed tax calculation on invoices and sales orders in the front end and updated conversion procedures.
- Fixed cancelling labor purchases.
- Fixed printing form views when clicking the Print button in report preview.
- The ''Print'' button on Labor Sheets is disabled for now.  Use print preview.
- Added ''Invoice Item Type Totals'' report to Management Reports.
- Changed the area code on reports to 262.
- Changed the customer drop down box on Invoices, Sales Orders, Quotes, and Labor Sheets to be a pop up dialog.
- Changed the date field on Invoices, Sales Orders, Quotes, and Labor Sheets to use the windows date picker control.
- The ''Sub Items'' mentioned above do not yet add associated items on quotes, sales orders, or invoices.  They will eventually.') 
go

update tblDBProperties
set PropertyValue = '5.15'
where PropertyName = 'DBStructVersion'
go

