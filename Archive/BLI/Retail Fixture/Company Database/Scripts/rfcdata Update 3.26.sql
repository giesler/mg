-- rfcdata structure updates

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_TotalSalesByClass]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_TotalSalesByClass]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



CREATE PROCEDURE dbo.sp_Report_TotalSalesByClass
	@ShipFromDate nvarchar(20) = null, 
	@ShipToDate nvarchar(20) = null,
	@SoldName nvarchar(100) = null,
	@Classification nvarchar(50) = null,
	@StoreSelName nvarchar(100) = null,
	@RefNum nvarchar(100) = null
AS
SET NOCOUNT ON
IF @ShipFromDate IS NULL
	SELECT @ShipFromDate = '1/1/1900'
IF @ShipToDate IS NULL
	SELECT @ShipToDate = '1/1/2100'

-- convert string date fields to actual dates
DECLARE @dShipFromDate datetime
SELECT @dShipFromDate = CONVERT(datetime, @ShipFromDate)
DECLARE @dShipToDate datetime
SELECT @dShipToDate = CONVERT(datetime, @ShipToDate)


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
		WHERE inv.Ship_Date Between @dShipFromDate And @dShipToDate 
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
			AND CASE
				WHEN @RefNum IS NULL THEN 1
				WHEN po.RefNum = @RefNum THEN 1
				ELSE 0 END = 1
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
		WHERE inv.Ship_Date Between @dShipFromDate And @dShipToDate 
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
			AND CASE
				WHEN @RefNum IS NULL THEN 1
				WHEN po.RefNum = @RefNum THEN 1
				ELSE 0 END = 1
		GROUP BY invi.Material
		) tbl1 ON tbl1.Material = #tmpTotals.Material
DELETE FROM #tmpTotals WHERE Quantity IS NULL
SELECT * FROM #tmpTotals ORDER BY Material

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO





insert ztblReleaseNotes (Date, MajorVersion, MinorVersion, Notes)
values ('10/10/2001', 3.26, 0, '- Reports:  Added Ref Num to Total Sales by Class report
- Reports:  Added Ref Num to Cashwraps reports
')
go

