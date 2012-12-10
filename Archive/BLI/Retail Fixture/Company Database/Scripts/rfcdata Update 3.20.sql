-- rfcdata structure updates

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_ShipmentSchedule]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_ShipmentSchedule]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.sp_Report_ShipmentSchedule
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
ORDER BY po.Ship_Date, po.PO
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tlkuStoreType]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tlkuStoreType]
GO


ALTER TABLE dbo.tlkuStore ADD
	SoldID int NULL
GO


insert ztblReleaseNotes (Date, MajorVersion, MinorVersion, Notes)
values ('7/31/2001', 3.20, 0, '- Added Email, Username, and Password to Sold To list in ''Edit Lists''
- Update ''Shipment Schedule Wheat'' to include only wheatboard items in the item qty on each line.  Note an item must be marked as ''Wheatboard'' in 
the catalog in order to show up on this report.
- Added ''Sold To'' to the ''Store Name'' dropdown in ''Edit Lists''
')
go

