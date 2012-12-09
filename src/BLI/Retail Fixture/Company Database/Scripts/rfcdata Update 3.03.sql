-- rfcdata structure updates

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Form_InvoiceItemTotals]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Form_InvoiceItemTotals]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Form_POItemTotals]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Form_POItemTotals]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_CurrentInventory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_CurrentInventory]
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

insert ztblReleaseNotes (Date, MajorVersion, MinorVersion, Notes)
values ('9/21/2000', 3.03, 0, '- Fixed ''PO Summary Report'' problem
- Fixed ''Current Inventory'' reports
- Fixed ''Browse Recent'' date not being editable in PO and Invoice menus
- Fixed several problems in Invoices
- Fixed problem when creating a new invoice from a PO
- Misc other changes/updates
')
go

