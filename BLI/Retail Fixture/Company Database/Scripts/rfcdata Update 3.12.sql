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
SELECT inv.ID, inv.Date_Ordered, inv.Ship_Date, inv.Arrive_Date, 
	-- Sold to address
	inv.Sold_Name AS Sold_Line1, 
	CASE	WHEN slda.Attention IS NOT NULL THEN 'ATTN: ' + slda.Attention
		ELSE	inv.Sold_Address1
	END AS Sold_Line2,
	CASE
		WHEN slda.Attention IS NOT NULL THEN inv.Sold_Address1
		ELSE inv.Sold_Address2
	END AS Sold_Line3,
	CASE
		WHEN slda.Attention IS NULL AND inv.Sold_Address2 IS NULL THEN NULL
		ELSE inv.Sold_City + ', ' + inv.Sold_State + '  ' + inv.Sold_Zip 
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
GO


insert ztblReleaseNotes (Date, MajorVersion, MinorVersion, Notes)
values ('10/19/2000', 3.12, 0, '- Fixed invoices not printing on Martha''s computer problem
- Fixed ''Finish'' button in POs to save the record before printing
- Changed line 2 of ''Sold'' address info on invoices to be the ''Attention'' name whenever it is set (otherwise, uses Address Line 1)
')
go

