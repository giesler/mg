-- rfcdata structure updates

update Tbl_PO
set SoldTo_Address1 = tblSold.SoldAddress1,
	SoldTo_Address2 = tblSold.SoldAddress2,
	SoldTo_City = tblSold.SoldCity,
	SoldTo_State = tblSold.SoldState,
	SoldTo_Zip = tblSold.SoldZip
from Tbl_PO inner join tblSold on tblSold.apkSold = Tbl_PO.SoldID
where Tbl_PO.SoldTo_Address1 is null
go

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


ALTER TABLE dbo.Tbl_PO_Items
	DROP CONSTRAINT aaaaaTbl_PO_Items_PK
GO
ALTER TABLE dbo.Tbl_PO_Items ADD CONSTRAINT
	Tbl_PO_Items_PK PRIMARY KEY CLUSTERED 
	(
	apkPOItem
	) WITH FILLFACTOR = 90 ON [PRIMARY]

GO


ALTER TABLE dbo.Tbl_Invoice_Items
	DROP CONSTRAINT aaaaaTbl_Invoice_Items_PK
GO
ALTER TABLE dbo.Tbl_Invoice_Items ADD CONSTRAINT
	Tbl_Invoice_Items_PK PRIMARY KEY CLUSTERED 
	(
	apkInvoiceItem
	) WITH FILLFACTOR = 90 ON [PRIMARY]

GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_CurrentBuildSummary]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_CurrentBuildSummary]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_ShipmentSchedule]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_ShipmentSchedule]
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



insert ztblReleaseNotes (Date, MajorVersion, MinorVersion, Notes)
values ('8/11/2001', 3.23, 0, '- PO Form: Moved ''Sold To Name'' and ''Sold To Attention'' to front tab, changed tab order to not go to second tab by default, it must be clicked.
- PO Form:  Changed exit behavior for ''Job #'' to go to Store # even when enter is pressed (already did for tab)
- Invoices:  Changed Sold to Address to use the address entered on the PO form
- Changed PO items and invoice items to maintain the order they were entered (if this is still seen, let me know an example PO)
- Removed ''Ref #'' from ''Current Build Summary'' report
- Added ''Ref #'' as criteria for the following reports:  Current Build Summary, and all Shipment Schedule reports
')
go

