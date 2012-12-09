-- rfcdata structure updates

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_CustShipmentSchedule]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_CustShipmentSchedule]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.sp_Report_CustShipmentSchedule
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
FROM Tbl_PO po
WHERE po.CancelPO = 0 
	AND po.ID IN (SELECT poi.ID
			FROM Tbl_PO_Items poi INNER JOIN Tbl_Catalog cat ON cat.Material = poi.Material WHERE cat.Medite = 0)
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


CREATE TABLE dbo.tblSoldAttention
	(
	SoldNameID int NOT NULL,
	SoldAttentionID int NOT NULL IDENTITY (1, 1),
	Attention nvarchar(100) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.tblSoldAttention ADD CONSTRAINT
	PK_tblSoldAttention PRIMARY KEY NONCLUSTERED 
	(
	SoldAttentionID
	) ON [PRIMARY]

GO
CREATE CLUSTERED INDEX IX_tblSoldAttention_SoldNameID ON dbo.tblSoldAttention
	(
	SoldNameID
	) ON [PRIMARY]
GO
ALTER TABLE dbo.tblSoldAttention ADD CONSTRAINT
	FK_tblSoldAttention_tblSold FOREIGN KEY
	(
	SoldNameID
	) REFERENCES dbo.tblSold
	(
	apkSold
	)
GO


ALTER TABLE dbo.Tbl_PO ADD
	FTofTruck bit NULL,
	FTofTruckAmount nvarchar(50) NULL
GO
ALTER TABLE dbo.Tbl_PO ADD CONSTRAINT
	DF_Tbl_PO_FTofTruck DEFAULT 0 FOR FTofTruck
GO

update Tbl_PO
set FTofTruck = 0
where FTofTruck is null
go



ALTER TABLE dbo.tblSold ADD
	SoldID  AS apkSold
GO

ALTER TABLE dbo.Tbl_PO ADD
	SoldAttentionID int NULL
GO
ALTER TABLE dbo.Tbl_PO ADD CONSTRAINT
	FK_Tbl_PO_tblSoldAttention FOREIGN KEY
	(
	SoldAttentionID
	) REFERENCES dbo.tblSoldAttention
	(
	SoldAttentionID
	)
GO


ALTER TABLE dbo.Tbl_PO ADD
	SoldID int NULL
GO
ALTER TABLE dbo.Tbl_PO ADD CONSTRAINT
	FK_Tbl_PO_tblSold FOREIGN KEY
	(
	SoldID
	) REFERENCES dbo.tblSold
	(
	apkSold
	)
GO


EXECUTE sp_rename N'dbo.tblSoldAttention.SoldNameID', N'Tmp_SoldID', 'COLUMN'
GO
EXECUTE sp_rename N'dbo.tblSoldAttention.Tmp_SoldID', N'SoldID', 'COLUMN'
GO

update Tbl_PO
set SoldID = tblSold.SoldID
from Tbl_PO INNER JOIN tblSold ON tblSold.SoldName = Tbl_PO.SoldName
WHERE Tbl_PO.SoldID is null
go


update Tbl_PO
set SoldID = 1, SoldName = 'Fisher Development, Inc.'
where SoldName = 'Fisher Development Inc.'
go

update Tbl_PO
set SoldID = 6, SoldName = 'JOCKEY INTERNATIONAL'
where SoldName = 'JOCKEY OUTLET INTERNATIONAL'
go

update Tbl_PO
set SoldID = 2, SoldName = 'THE GAP'
where SoldName = 'GAP KIDS  #9167'
go

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_CurrentBuildSummary]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_CurrentBuildSummary]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_ShipmentSchedule]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_ShipmentSchedule]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE dbo.sp_Report_CurrentBuildSummary
	@ShipFromDate nvarchar(20) = null, 
	@ShipToDate nvarchar(20) = null,
	@SoldName nvarchar(100) = null,
	@StoreNo nvarchar(100) = null
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
GROUP BY poi.Material, cat.Description, cat.Inventory, po.SoldName
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
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
ORDER BY po.Ship_Date, po.PO
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO




insert ztblReleaseNotes (Date, MajorVersion, MinorVersion, Notes)
values ('10/6/2000', 3.08, 0, '- Fixed Cust Shipment Schedule report showing duplicates
- Added ''FT of Truck'' to PO screen and Shipment Schedule Ship Via report
- Added ''Store #'' criteria to Current Build Summary report
- On the toolbar of every report is an icon that says ''OfficeLinks'' when you put the mouse over it.  Click it to export a report to Excel.
')
go

