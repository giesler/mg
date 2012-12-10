-- rfcdata structure updates

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

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_WorkOrder]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_WorkOrder]
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
	poi.Quanity, cat.Weight, poi.MatDescription, poi.apkPOItem, cat.Weight * poi.Quanity AS ExtWeight, po.BlanketPO
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

insert ztblReleaseNotes (Date, MajorVersion, MinorVersion, Notes)
values ('1/30/2002', 3.29, 0, '- Invoices:  Changed to print PO and/or Blanket PO depending on what was entered
- Work Order: Changed to print PO and/or Blanket PO depending on what was entered
- PO Summary: Added blanket PO to criteria form
- PO Form: Changed PO field to allow you to clear it and enter a blanket PO
')
go

