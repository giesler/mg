
-- TODO: 'ShelfType' must be set in Tbl_PO, see CustShipmentSched report

use rfcdata
go

sp_change_users_login 'auto_fix', 'rfcuser'
go

ALTER TABLE dbo.Tbl_Invoice_Items ADD
	POItemID int NULL
GO

CREATE NONCLUSTERED INDEX IX_tblSold_SoldName ON dbo.tblSold
	(
	SoldName
	) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX IX_Tbl_Invoice_Sold_Name ON dbo.Tbl_Invoice
	(
	Sold_Name
	) ON [PRIMARY]
GO

ALTER TABLE dbo.Tbl_PO ADD
	ShelfType nvarchar(50) NULL
GO


CREATE TABLE dbo.ztmpStoreSel
	(
 	StoreSelID int NOT NULL IDENTITY (1, 1),
	Store nvarchar(100) NULL,
	StoreSelName nvarchar(100) NULL
	) ON [PRIMARY]
GO
ALTER TABLE dbo.ztmpStoreSel ADD CONSTRAINT
	PK_ztmpStoreSel PRIMARY KEY NONCLUSTERED 
	(
	StoreSelID
	) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX IX_Tbl_PO_Ship_Store_No ON dbo.Tbl_PO
	(
	Ship_StoreNo
	) ON [PRIMARY]
GO


UPDATE Tbl_PO
SET ShelfType = st1.ShelfType
FROM Tbl_PO
	INNER JOIN (SELECT tbl1.ID, 
				CASE LEFT(poi.Material,2)
					WHEN 'MD' THEN 'MD'
					ELSE LEFT(poi.Material,1)
				END AS ShelfType
			FROM Tbl_PO_Items poi
				INNER JOIN (	SELECT po.ID, MIN(poi.apkPOItem) AS apkPOItem
						FROM Tbl_PO po
							INNER JOIN Tbl_PO_Items poi ON Poi.ID = po.id
						GROUP BY po.ID
					) tbl1 ON poi.apkPOItem = tbl1.apkPOItem
		) st1 ON st1.ID = Tbl_PO.ID
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Invoice]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Invoice]
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

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Util_GetStoreInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Util_GetStoreInfo]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE sp_Invoice
	@InvoiceID int
AS

SELECT inv.ID, inv.Date_Ordered, inv.Ship_Date, inv.Arrive_Date, inv.Sold_Name, inv.Sold_Address1, 
	inv.Sold_Address2, inv.Shipped_Name, inv.Shipped_Address1, inv.Shipped_Address2, 
	inv.Shipped_City + ', ' + inv.Shipped_State + '  ' + inv.Shipped_Zip AS Shipped_Address,
	inv.Shipped_Attn, inv.Shipped_Phone, inv.PO, inv.Delivery, inv.Salesperson, inv.Tax_Rate, 
	inv.Credit, inv.Comments, inv.[Pro #], inv.Shipped_State, inv.[Account#], inv.Shipped_StoreNo, 
	inv.Shipped_Name + ' #' + inv.Shipped_StoreNo AS Shipped_Line1, 
	inv.Sold_City + ', ' + inv.Sold_State + '  ' + inv.Sold_Zip AS Sold_Address, 
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
	LEFT OUTER JOIN tblSold sld ON sld.SoldName = inv.Sold_Name
WHERE inv.ID = @InvoiceID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE sp_Report_CurrentBuildSummary
	@ShipFromDate nvarchar(20) = null, 
	@ShipToDate nvarchar(20) = null,
	@SoldName nvarchar(100) = null
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

CREATE PROCEDURE sp_Report_CurrentInventory
	@Active bit = null,
	@Hide bit = null,
	@CustID int = null
AS

SELECT cat.Material, cat.Description, cat.Inventory, cat.Hide, cat.Active, cat.Price * cat.Inventory AS ExtPrice
FROM Tbl_Catalog_Headings cath 
	INNER JOIN Tbl_Catalog cat ON cath.Heading_Number = cat.Heading_Number
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

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE sp_Report_CustShipmentSchedule
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
FROM Tbl_Catalog cat
	INNER JOIN Tbl_PO_Items poi ON poi.Material = cat.Material
	INNER JOIN Tbl_PO po ON po.ID = poi.ID
WHERE po.CancelPO = 0 AND cat.Medite = 0
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

CREATE PROCEDURE sp_Report_GapOrderConf
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

CREATE PROCEDURE sp_Report_HotOrders
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

CREATE PROCEDURE sp_Report_IncompletePOs
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
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE sp_Report_InvoiceSummary
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

CREATE PROCEDURE sp_Report_MaintOrders
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

CREATE PROCEDURE sp_Report_OutstandingPOs
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

CREATE PROCEDURE sp_Report_POsByJobNumber
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

CREATE PROCEDURE sp_Report_ProductCatalog
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE sp_Report_ShipmentSchedule
	@ShipFromDate nvarchar(20) = null, 
	@ShipToDate nvarchar(20) = null,
	@SoldName nvarchar(100) = null,
	@Medite bit = null,
	@StoreSelName nvarchar(100) = null,
	@ShipVia nvarchar(100) = null
AS

IF @ShipFromDate IS NULL
	SELECT @ShipFromDate = '1/1/1900'
IF @ShipToDate IS NULL
	SELECT @ShipToDate = '1/1/2100'


SELECT po.PO, po.Ship_City + ', ' + po.Ship_State AS City_State, po.ShelfType, po.Arrive_Date, po.Ship_Date, po.Ship_Via, 
	po.Comments, po.Ship_Name, spoi2.ExtPrice, spoi2.ExtQty, spoi2.ExtWeight
FROM Tbl_PO po
	INNER JOIN (SELECT DISTINCT poi.ID, cat.Medite
			FROM Tbl_PO_Items poi 
				LEFT OUTER JOIN Tbl_Catalog cat ON cat.Material = poi.Material
			WHERE LEFT(poi.Material, 3) <> 'FRT')
		spoi ON spoi.ID = po.ID
	INNER JOIN (SELECT Tbl_PO_Items.ID, 
				Sum([Weight]*[Quanity]) AS ExtWeight, Sum([Unit_Price]*[Quanity]) AS ExtPrice,
				 Sum([Quanity]) AS ExtQty 
			FROM Tbl_Catalog INNER JOIN Tbl_PO_Items ON Tbl_Catalog.Material = Tbl_PO_Items.Material 
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
		WHEN spoi.Medite = @Medite THEN 1 
		 ELSE 0 END  = 1
	AND CASE 
		WHEN @StoreSelName IS NULL THEN 1  
		WHEN po.Store IN (SELECT Store FROM ztmpStoreSel WHERE StoreSelName = @StoreSelName) THEN 1 
		 ELSE 0 END  = 1
	AND CASE 
		WHEN @ShipVia IS NULL THEN 1  
		WHEN po.Ship_Via Like @ShipVia THEN 1 
		 ELSE 0 END  = 1
ORDER BY po.Ship_Date






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE sp_Report_ShipmentSummary
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

CREATE PROCEDURE sp_Report_TotalSalesByClass
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
		WHERE inv.Ship_Date Between @ShipFromDate And @ShipToDate 
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
		WHERE inv.Ship_Date Between @ShipFromDate And @ShipToDate 
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

CREATE PROCEDURE sp_Report_WisconsinTaxes
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

CREATE PROCEDURE sp_Report_WorkOrder
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
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE sp_Util_GetStoreInfo
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

delete from tbl_po_items
where Material is null
go

delete from tbl_po
where id in (select id from tbl_po where Ship_Name is null and Arrive_Date is null)
go

insert ztblReleaseNotes (Date, MajorVersion, MinorVersion, Notes)
values ('9/20/2000', 3, 0, '- Database has been modified to use SQL Server for data storage.  Too many changes to list.')
go

delete from tbl_invoice
where id = 0
go


CREATE TABLE dbo.Tmp_Tbl_Inventory_Entry
	(
	[Date] datetime NULL,
	Entry_Number int NOT NULL,
	Entry_Initials nvarchar(50) NULL
	)  ON [PRIMARY]
GO
IF EXISTS(SELECT * FROM dbo.Tbl_Inventory_Entry)
	 EXEC('INSERT INTO dbo.Tmp_Tbl_Inventory_Entry ([Date], Entry_Number, Entry_Initials)
		SELECT [Date], Entry_Number, Entry_Initials FROM dbo.Tbl_Inventory_Entry TABLOCKX')
GO
ALTER TABLE dbo.Tbl_Inventory_Entry_Items
	DROP CONSTRAINT Tbl_Inventory_Entry_Items_FK01
GO
DROP TABLE dbo.Tbl_Inventory_Entry
GO
EXECUTE sp_rename N'dbo.Tmp_Tbl_Inventory_Entry', N'Tbl_Inventory_Entry', 'OBJECT'
GO
ALTER TABLE dbo.Tbl_Inventory_Entry ADD CONSTRAINT
	aaaaaTbl_Inventory_Entry_PK PRIMARY KEY NONCLUSTERED 
	(
	Entry_Number
	) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX [Date] ON dbo.Tbl_Inventory_Entry
	(
	[Date]
	) ON [PRIMARY]
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.Tbl_Inventory_Entry_Items WITH NOCHECK ADD CONSTRAINT
	Tbl_Inventory_Entry_Items_FK01 FOREIGN KEY
	(
	Entry_Number
	) REFERENCES dbo.Tbl_Inventory_Entry
	(
	Entry_Number
	)
GO
ALTER TABLE dbo.Tbl_Inventory_Entry_Items
	NOCHECK CONSTRAINT Tbl_Inventory_Entry_Items_FK01
GO
COMMIT

BEGIN TRANSACTION
ALTER TABLE dbo.Tbl_Temp_Invoice_Build ADD
	NTUser nvarchar(50) NULL
GO
COMMIT

DELETE FROM Tbl_Temp_Invoice_Build
go

delete from tblUser where UserName is null
go
