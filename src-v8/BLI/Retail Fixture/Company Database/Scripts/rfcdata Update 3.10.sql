-- rfcdata structure updates

delete from ztblReleaseNotes where id = 83
go

update ztblReleaseNotes set MajorVersion = 3.08 where id = 89
go

update ztblReleaseNotes set MajorVersion = 3.09 where id = 90
go

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Invoice]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Invoice]
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
SELECT inv.ID, inv.Date_Ordered, inv.Ship_Date, inv.Arrive_Date, 
	inv.Sold_Name + CHAR(13)+CHAR(10) + ISNULL(slda.Attention + CHAR(13)+CHAR(10), '') 
		+ ISNULL(inv.Sold_Address1 + CHAR(13)+CHAR(10), '')
		+ ISNULL(inv.Sold_Address2 + CHAR(13)+CHAR(10), '') + inv.Sold_City + ', ' 
		+ inv.Sold_State + '  ' + inv.Sold_Zip AS Sold_Address_Info, 
	ISNULL(inv.Shipped_Name, '') + ISNULL('#' + inv.Shipped_StoreNo, '') + CHAR(13)+CHAR(10)
		+ ISNULL(inv.Shipped_Attn + CHAR(13)+CHAR(10), '')
		+ ISNULL(inv.Shipped_Address1 + CHAR(13)+CHAR(10), '')
		+ ISNULL(inv.Shipped_Address2 + CHAR(13)+CHAR(10), '')
		+ inv.Shipped_City + ', ' + inv.Shipped_State + '  ' + inv.Shipped_Zip AS Ship_Address_Info, 
	
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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



insert ztblReleaseNotes (Date, MajorVersion, MinorVersion, Notes)
values ('10/13/2000', 3.10, 0, '- Invoices now print the new Attention field
- POs: Cursor now moves from ''Reference #'' to ''Qty''
- The ''Sold Attention'' now defaults to the first name in the list for the sold to customer.
')
go

