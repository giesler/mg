-- rfcdata structure updates

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_CurrentBuildSummary]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_CurrentBuildSummary]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE dbo.sp_Report_CurrentBuildSummary
	@ShipFromDate nvarchar(20) = null, 
	@ShipToDate nvarchar(20) = null,
	@SoldName nvarchar(100) = null,
	@StoreNo nvarchar(100) = null,
	@RefNum nvarchar(100) = null,
	@BlanketPO nvarchar(100) = null
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
	AND CASE
		WHEN @BlanketPO IS NULL THEN 1
		WHEN po.BlanketPO = @BlanketPO THEN 1
		ELSE 0 END = 1
GROUP BY poi.Material, cat.Description, cat.Inventory, po.SoldName
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_Invoice]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_Invoice]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE dbo.sp_Report_Invoice
	@InvoiceID int
AS
SELECT inv.ID, inv.Date_Ordered, inv.Ship_Date, inv.Arrive_Date, BlanketPO, 
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
		ELSE ISNULL( inv.Shipped_City + ', ' + inv.Shipped_State + '  ', '') + ISNULL( inv.Shipped_Zip, '')
	END AS Ship_Line3,
	CASE
		WHEN inv.Shipped_Address2 IS NULL AND inv.Shipped_Attn IS NULL THEN NULL
		ELSE ISNULL( inv.Shipped_City + ', ' + inv.Shipped_State + '  ', '') + ISNULL( inv.Shipped_Zip, '')
	END AS Ship_Line4,
	
	inv.PO, inv.Delivery, inv.Salesperson, inv.Tax_Rate, inv.Shipped_State,
	inv.Credit, inv.Comments, inv.[Pro #], inv.[Account#], 
	invi.MatDescription, invi.Quantiy, invi.Unit_Price, invi.Quantiy * invi.Unit_Price AS LineExtPrice, 
	invi.Material, catMat.CustMaterial,  invi.apkInvoiceItem, 
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
	LEFT OUTER JOIN Tbl_Catalog cat ON cat.Material = invi.Material
	LEFT OUTER JOIN tblCatalogCustMaterial catMat ON catMat.MaterialId = cat.apkCatalogItem
WHERE inv.ID = @InvoiceID AND (catMat.CustID is null OR catMat.CustID = sld.SoldID)
ORDER BY inv.ID, invi.apkInvoiceItem
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO




insert ztblReleaseNotes (Date, MajorVersion, MinorVersion, Notes)
values ('3/7/2002', 3.31, 0, '- Reports:  Current Build Summary - Added Blanket PO to criteria
- Reports: Shipment Schedule - changed column widths and removed shelf type column
- Reports: Shipment Schedule ($) - changed column widths and removed shelf type column
- Reports: Shipping Schedule - fixed to work with database structure changes
- Reports: Invoices - City and State will now print even with no zip code entered
- Reports: Invoices - Added Cust Material # to report
')
go

