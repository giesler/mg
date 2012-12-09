-- rfcdata structure updates

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Invoice]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Invoice]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE dbo.sp_Invoice
	@InvoiceID int
AS
SELECT inv.ID, inv.Date_Ordered, inv.Ship_Date, inv.Arrive_Date, inv.Sold_Name, inv.Sold_Address1, 
	inv.Sold_Address2, inv.Shipped_Name, inv.Shipped_Address1, inv.Shipped_Address2, 
	inv.Shipped_City + ', ' + inv.Shipped_State + '  ' + inv.Shipped_Zip AS Shipped_Address,
	inv.Shipped_Attn, inv.Shipped_Phone, inv.PO, inv.Delivery, inv.Salesperson, inv.Tax_Rate, 
	inv.Credit, inv.Comments, inv.[Pro #], inv.Shipped_State, inv.[Account#], inv.Shipped_StoreNo, 
	ISNULL(inv.Shipped_Name,'') + ISNULL(' #' + inv.Shipped_StoreNo,'') AS Shipped_Line1, 
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


insert ztblReleaseNotes (Date, MajorVersion, MinorVersion, Notes)
values ('9/25/2000', 3.06, 0, '- Fixed problem with the top ship to line not always printing on invoices
- Added text indicating loading progress on PO screen and invoices screen
')
go

