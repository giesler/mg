-- rfcdata structure updates

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_POsByJobNumber]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_POsByJobNumber]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_ShipmentSchedule]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_ShipmentSchedule]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_WorkOrder]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_WorkOrder]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.sp_Report_POsByJobNumber
	@ShipFromDate nvarchar(20) = null, 
	@ShipToDate nvarchar(20) = null,
	@SoldName nvarchar(100) = null, 
	@JobNumber nvarchar(100) = null
AS
IF @ShipFromDate IS NULL
	SELECT @ShipFromDate = '1/1/1900'
IF @ShipToDate IS NULL
	SELECT @ShipToDate = '1/1/2100'

SELECT 
	CASE 
		WHEN ( (po IS NOT NULL and po.PO != '') and (po.BlanketPO is null or po.BlanketPO = '')) THEN po.PO  
		WHEN ( (po.PO IS NULL or po.PO = '') and (po.BlanketPO is not null and po.BlanketPO != '')) THEN po.BlanketPO 
		WHEN ( (po.PO IS NOT NULL and po.PO != '') and (po.BlanketPO is NOT NULL and po.BlanketPO != '')) THEN po.PO + CHAR(13) +  CHAR(10) + po.BlanketPO
	END AS [PONumber], 
	po.Ship_City + ', ' + po.Ship_State AS City_State, po.Arrive_Date, po.Ship_Date, po.Ship_Via, po.Comments, po.Ship_Name, 
	poi.Quanity, poi.Material, poi.MatDescription, poi.Unit_Price, poi.Quanity * poi.Unit_Price AS ExtPrice, poi.Phase, cat.Weight
FROM Tbl_PO po
	LEFT OUTER JOIN Tbl_PO_Items poi ON poi.ID = po.ID
	LEFT OUTER JOIN Tbl_Catalog cat ON poi.Material = cat.Material
WHERE po.CancelPO = 0
	AND po.Ship_Date Between @ShipFromDate And @ShipToDate 
	AND CASE 
		WHEN @SoldName IS NULL THEN 1  
		WHEN po.SoldName Like @SoldName THEN 1 
		 ELSE 0 END  = 1
	AND CASE 
		WHEN @JobNumber IS NULL THEN 1  
		WHEN po.Job_Number Like @JobNumber + '%' THEN 1 
		 ELSE 0 END  = 1
ORDER BY po.Ship_Date
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
	@ShipVia nvarchar(100) = null,
	@RefNum nvarchar(100) = null
AS
IF @ShipFromDate IS NULL
	SELECT @ShipFromDate = '1/1/1900'
IF @ShipToDate IS NULL
	SELECT @ShipToDate = '1/1/2100'
SELECT 
	CASE 
		WHEN ( (po IS NOT NULL and po.PO != '') and (po.BlanketPO is null or po.BlanketPO = '')) THEN po.PO  
		WHEN ( (po.PO IS NULL or po.PO = '') and (po.BlanketPO is not null and po.BlanketPO != '')) THEN po.BlanketPO 
		WHEN ( (po.PO IS NOT NULL and po.PO != '') and (po.BlanketPO is NOT NULL and po.BlanketPO != '')) THEN po.PO + CHAR(13) +  CHAR(10) + po.BlanketPO
	END AS [PONumber],
	po.Ship_City + ', ' + po.Ship_State AS City_State, po.ShelfType, po.Arrive_Date, po.Ship_Date, po.Ship_Via, 
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
ORDER BY po.Ship_Date, [PONumber]
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE dbo.sp_Report_WorkOrder
	@POID int = null
AS
SELECT 
	CASE 
		WHEN ( (po IS NOT NULL and po.PO != '') and (po.BlanketPO is null or po.BlanketPO = '')) THEN po.PO  
		WHEN ( (po.PO IS NULL or po.PO = '') and (po.BlanketPO is not null and po.BlanketPO != '')) THEN po.BlanketPO 
		WHEN ( (po.PO IS NOT NULL and po.PO != '') and (po.BlanketPO is NOT NULL and po.BlanketPO != '')) THEN po.PO + CHAR(13) +  CHAR(10) + po.BlanketPO
	END AS [PONumber],
	 po.Ship_Date, po.Arrive_Date, po.Ship_via, po.Ship_Name, 
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



if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_CashwrapFisher]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_CashwrapFisher]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_CashwrapFisher2]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_CashwrapFisher2]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE dbo.sp_Report_CashwrapFisher
	@whereClause nvarchar(2000)
AS

exec('
SELECT po.Ship_StoreNo AS [Store #], po.ID, 
	CASE 
		WHEN ( (po IS NOT NULL and po.PO != '') and (po.BlanketPO is null or po.BlanketPO = '')) THEN po.PO  
		WHEN ( (po.PO IS NULL or po.PO = '') and (po.BlanketPO is not null and po.BlanketPO != '')) THEN po.BlanketPO 
		WHEN ( (po.PO IS NOT NULL and po.PO != '') and (po.BlanketPO is NOT NULL and po.BlanketPO != '')) THEN po.PO + CHAR(13) +  CHAR(10) + po.BlanketPO
	END AS [PO #],
	 po.Ship_Address1 AS [Mall], po.Ship_City AS [City], po.Ship_State AS [State], 
	SUM(Weight) AS [Weight], po.FTofTruckAmount AS [FT of Truck], NULL AS [Comments], po.Date_Ordered AS [PO Date], 
	po.Ship_Date AS [Ship Date], cat.CashwrapName, Sum(poi.Quanity) AS Quantity 

FROM Tbl_Catalog cat 
	INNER JOIN Tbl_PO_Items poi ON poi.Material = cat.Material 
	INNER JOIN Tbl_PO po ON poi.ID = po.ID  
WHERE cat.Cashwrap = 1 And ' + @whereClause + '
GROUP BY po.Ship_StoreNo, po.ID, 
	CASE 
		WHEN ( (po IS NOT NULL and po.PO != '') and (po.BlanketPO is null or po.BlanketPO = '')) THEN po.PO  
		WHEN ( (po.PO IS NULL or po.PO = '') and (po.BlanketPO is not null and po.BlanketPO != '')) THEN po.BlanketPO 
		WHEN ( (po.PO IS NOT NULL and po.PO != '') and (po.BlanketPO is NOT NULL and po.BlanketPO != '')) THEN po.PO + CHAR(13) +  CHAR(10) + po.BlanketPO
	END ,
	po.Ship_Address1, po.Ship_City, po.Ship_State, 
	po.FTofTruckAmount,  po.Date_Ordered, po.Ship_Date, cat.CashwrapName 

ORDER BY po.Ship_StoreNo, [PO #]')
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE dbo.sp_Report_CashwrapFisher2
	@whereClause nvarchar(2000)
AS

exec('
SELECT po.ID, 
	CASE 
		WHEN ( (po IS NOT NULL and po.PO != '') and (po.BlanketPO is null or po.BlanketPO = '')) THEN po.PO  
		WHEN ( (po.PO IS NULL or po.PO = '') and (po.BlanketPO is not null and po.BlanketPO != '')) THEN po.BlanketPO 
		WHEN ( (po.PO IS NOT NULL and po.PO != '') and (po.BlanketPO is NOT NULL and po.BlanketPO != '')) THEN po.PO + CHAR(13) +  CHAR(10) + po.BlanketPO
	END AS [PO #],
	po.Ship_Name AS [Ship To], po.Ship_City AS City, po.Ship_State AS State, 
    	po.Date_Ordered AS [PO Date], po.Ship_Date AS [Ship Date], po.Ship_Via AS [Carrier], 
	po.[Pro#] AS [Pro #], cat.CashwrapName, Sum(poi.Quanity) AS Quantity
FROM Tbl_Catalog cat 
	INNER JOIN Tbl_PO_Items poi ON poi.Material = cat.Material 
	INNER JOIN Tbl_PO po ON poi.ID = po.ID  
WHERE cat.Cashwrap = 1 And ' + @whereClause + '
GROUP BY po.ID, 
	CASE 
		WHEN ( (po IS NOT NULL and po.PO != '') and (po.BlanketPO is null or po.BlanketPO = '')) THEN po.PO  
		WHEN ( (po.PO IS NULL or po.PO = '') and (po.BlanketPO is not null and po.BlanketPO != '')) THEN po.BlanketPO 
		WHEN ( (po.PO IS NOT NULL and po.PO != '') and (po.BlanketPO is NOT NULL and po.BlanketPO != '')) THEN po.PO + CHAR(13) +  CHAR(10) + po.BlanketPO
	END ,
	po.Ship_Name, po.Ship_City, po.Ship_State, po.Date_Ordered, po.Ship_Date, po.Ship_Via, po.[Pro#], cat.CashwrapName 

ORDER BY [PO #]')
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

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


SELECT 
	CASE 
		WHEN ( (po IS NOT NULL and po.PO != '') and (po.BlanketPO is null or po.BlanketPO = '')) THEN po.PO  
		WHEN ( (po.PO IS NULL or po.PO = '') and (po.BlanketPO is not null and po.BlanketPO != '')) THEN po.BlanketPO 
		WHEN ( (po.PO IS NOT NULL and po.PO != '') and (po.BlanketPO is NOT NULL and po.BlanketPO != '')) THEN po.PO + CHAR(13) +  CHAR(10) + po.BlanketPO
	END AS [PONumber],
	po.Ship_Date, po.Ship_City + ', ' + po.Ship_State AS City_State, 
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


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_IncompletePOs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_IncompletePOs]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.sp_Report_IncompletePOs
	@SoldName nvarchar(100) = null	
AS
SELECT 
	CASE 
		WHEN ( (po IS NOT NULL and po.PO != '') and (po.BlanketPO is null or po.BlanketPO = '')) THEN po.PO  
		WHEN ( (po.PO IS NULL or po.PO = '') and (po.BlanketPO is not null and po.BlanketPO != '')) THEN po.BlanketPO 
		WHEN ( (po.PO IS NOT NULL and po.PO != '') and (po.BlanketPO is NOT NULL and po.BlanketPO != '')) THEN po.PO + CHAR(13) +  CHAR(10) + po.BlanketPO
	END AS [PONumber],
	po.Date_Ordered, po.Ship_Date, po.Arrive_Date, 
	po.Ship_Name, po.Comments, po.Status, po.SoldName 
FROM Tbl_PO po
WHERE po.Status IS NOT NULL AND po.Status <> 'Completed' AND po.CancelPO = 0
	AND CASE 
		WHEN @SoldName IS NULL THEN 1  
		WHEN po.SoldName Like @SoldName THEN 1 
		 ELSE 0 END  = 1
ORDER BY po.Ship_Date, po.Ship_Name
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_MaintOrders]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_MaintOrders]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.sp_Report_MaintOrders
	@ShipFromDate nvarchar(20) = null, 
	@ShipToDate nvarchar(20) = null,
	@SoldName nvarchar(100) = null	
AS
IF @ShipFromDate IS NULL
	SELECT @ShipFromDate = '1/1/1900'
IF @ShipToDate IS NULL
	SELECT @ShipToDate = '1/1/2100'

SELECT 
	CASE 
		WHEN ( (po IS NOT NULL and po.PO != '') and (po.BlanketPO is null or po.BlanketPO = '')) THEN po.PO  
		WHEN ( (po.PO IS NULL or po.PO = '') and (po.BlanketPO is not null and po.BlanketPO != '')) THEN po.BlanketPO 
		WHEN ( (po.PO IS NOT NULL and po.PO != '') and (po.BlanketPO is NOT NULL and po.BlanketPO != '')) THEN po.PO + CHAR(13) +  CHAR(10) + po.BlanketPO
	END AS [PONumber],
	po.Ship_Date, po.Ship_City + ', ' + po.Ship_State AS City_State, po.Date_Ordered, po.SoldName, po.Ship_Via, po.ID, 
	spoi.ExtQty, spoi.ExtPrice, spoi.ExtWeight
FROM Tbl_PO po
	INNER JOIN (SELECT Tbl_PO_Items.ID, 
			Sum([Weight]*[Quanity]) AS ExtWeight, Sum([Unit_Price]*[Quanity]) AS ExtPrice,
			 Sum([Quanity]) AS ExtQty FROM Tbl_Catalog INNER JOIN Tbl_PO_Items ON Tbl_Catalog.Material = Tbl_PO_Items.Material GROUP BY Tbl_PO_Items.ID) spoi ON po.ID = spoi.ID
WHERE po.CancelPO = 0 AND po.Priority = 'Maintence Order'
	AND po.Ship_Date Between @ShipFromDate And @ShipToDate 
	AND CASE 
		WHEN @SoldName IS NULL THEN 1  
		WHEN po.SoldName Like @SoldName THEN 1 
		 ELSE 0 END  = 1
ORDER BY po.Ship_Date
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_HotOrders]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_HotOrders]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.sp_Report_HotOrders
	@ShipFromDate nvarchar(20) = null, 
	@ShipToDate nvarchar(20) = null,
	@SoldName nvarchar(100) = null	
AS
IF @ShipFromDate IS NULL
	SELECT @ShipFromDate = '1/1/1900'
IF @ShipToDate IS NULL
	SELECT @ShipToDate = '1/1/2100'

SELECT 
	CASE 
		WHEN ( (po IS NOT NULL and po.PO != '') and (po.BlanketPO is null or po.BlanketPO = '')) THEN po.PO  
		WHEN ( (po.PO IS NULL or po.PO = '') and (po.BlanketPO is not null and po.BlanketPO != '')) THEN po.BlanketPO 
		WHEN ( (po.PO IS NOT NULL and po.PO != '') and (po.BlanketPO is NOT NULL and po.BlanketPO != '')) THEN po.PO + CHAR(13) +  CHAR(10) + po.BlanketPO
	END AS [PONumber],
	po.Ship_Date, po.Ship_City + ', ' + po.Ship_State AS City_State, po.Date_Ordered, po.SoldName, po.Ship_Via, po.ID,
	spoi.ExtQty, spoi.ExtPrice, spoi.ExtWeight
FROM Tbl_PO po
	INNER JOIN (SELECT Tbl_PO_Items.ID, 
			Sum([Weight]*[Quanity]) AS ExtWeight, Sum([Unit_Price]*[Quanity]) AS ExtPrice,
			 Sum([Quanity]) AS ExtQty FROM Tbl_Catalog INNER JOIN Tbl_PO_Items ON Tbl_Catalog.Material = Tbl_PO_Items.Material GROUP BY Tbl_PO_Items.ID) spoi ON po.ID = spoi.ID
WHERE po.CancelPO = 0 AND po.Priority = 'Hot Order'
	AND po.Ship_Date Between @ShipFromDate And @ShipToDate 
	AND CASE 
		WHEN @SoldName IS NULL THEN 1  
		WHEN po.SoldName Like @SoldName THEN 1 
		 ELSE 0 END  = 1
ORDER BY po.Ship_Date
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_GapOrderConf]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_GapOrderConf]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE dbo.sp_Report_GapOrderConf
	@ShipFromDate nvarchar(20) = null, 
	@ShipToDate nvarchar(20) = null,
	@SoldName nvarchar(100) = null,
	@StoreSelName nvarchar(100) = null,
	@OrderFromDate nvarchar(20) = null, 
	@OrderToDate nvarchar(20) = null
AS
IF @ShipFromDate IS NULL
	SELECT @ShipFromDate = '1/1/1900'
IF @ShipToDate IS NULL
	SELECT @ShipToDate = '1/1/2100'
IF @OrderFromDate IS NULL
	SELECT @OrderFromDate = '1/1/1900'
IF @OrderToDate IS NULL
	SELECT @OrderToDate = '1/1/2100'

SELECT 
	CASE 
		WHEN ( (po IS NOT NULL and po.PO != '') and (po.BlanketPO is null or po.BlanketPO = '')) THEN po.PO  
		WHEN ( (po.PO IS NULL or po.PO = '') and (po.BlanketPO is not null and po.BlanketPO != '')) THEN po.BlanketPO 
		WHEN ( (po.PO IS NOT NULL and po.PO != '') and (po.BlanketPO is NOT NULL and po.BlanketPO != '')) THEN po.PO + CHAR(13) +  CHAR(10) + po.BlanketPO
	END AS [PONumber],
	po.Ship_City + ', ' + po.Ship_State AS City_State, po.ShelfType, po.Arrive_Date, po.Ship_Date, po.Ship_Via, 
	po.Date_Ordered, po.Comments, po.Priority
FROM Tbl_PO po
WHERE po.CancelPO = 0
	AND po.Ship_Date Between @ShipFromDate And @ShipToDate 
	AND po.Date_Ordered Between @OrderFromDate And @OrderToDate 
	AND CASE 
		WHEN @SoldName IS NULL THEN 1  
		WHEN po.SoldName Like @SoldName THEN 1 
		 ELSE 0 END  = 1
	AND CASE 
		WHEN @StoreSelName IS NULL THEN 1  
		WHEN po.Store IN (SELECT Store FROM ztmpStoreSel WHERE StoreSelName = @StoreSelName) THEN 1 
		 ELSE 0 END  = 1
ORDER BY po.Ship_Date
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_OutstandingPOs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_OutstandingPOs]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.sp_Report_OutstandingPOs
	@ShipFromDate nvarchar(20) = null, 
	@ShipToDate nvarchar(20) = null,
	@SoldName nvarchar(100) = null, 
	@Classification nvarchar(100) = null,
	@ReferenceNumber nvarchar(100) = null
AS
IF @ShipFromDate IS NULL
	SELECT @ShipFromDate = '1/1/1900'
IF @ShipToDate IS NULL
	SELECT @ShipToDate = '1/1/2100'
SELECT 
	CASE 
		WHEN ( (po IS NOT NULL and po.PO != '') and (po.BlanketPO is null or po.BlanketPO = '')) THEN po.PO  
		WHEN ( (po.PO IS NULL or po.PO = '') and (po.BlanketPO is not null and po.BlanketPO != '')) THEN po.BlanketPO 
		WHEN ( (po.PO IS NOT NULL and po.PO != '') and (po.BlanketPO is NOT NULL and po.BlanketPO != '')) THEN po.PO + CHAR(13) +  CHAR(10) + po.BlanketPO
	END AS [PONumber],
	po.Ship_Date, po.Ship_City + ', ' + po.Ship_State AS City_State, po.Date_Ordered, po.Ship_Via, po.ID, 
	po.ShelfType, po.Arrive_Date, po.Ship_Name, po.Comments
FROM Tbl_PO po
WHERE po.CancelPO = 0 AND (po.Status IS NOT NULL AND po.Status <> 'Completed')
	AND po.Ship_Date Between @ShipFromDate And @ShipToDate 
	AND CASE 
		WHEN @SoldName IS NULL THEN 1  
		WHEN po.SoldName Like @SoldName THEN 1 
		 ELSE 0 END  = 1
	AND CASE 
		WHEN @Classification IS NULL THEN 1  
		WHEN po.Classification Like @Classification THEN 1 
		 ELSE 0 END  = 1
	AND CASE 
		WHEN @ReferenceNumber IS NULL THEN 1  
		WHEN  po.RefNum Like @ReferenceNumber + '%' THEN 1 
		 ELSE 0 END  = 1
ORDER BY po.Ship_Date
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Report_ProductCatalog]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Report_ProductCatalog]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.sp_Report_ProductCatalog
	@CustID int = null	
AS
SELECT cath.Heading_Desc, cat.Material, ccm.CustMaterial, cat.Description, cat.Price, cat.Weight, cath.Heading_Number, cat.CatOrder
FROM Tbl_Catalog_Headings cath
	INNER JOIN Tbl_Catalog cat ON cath.apkHeading = cat.afkCatalogHeading 
	LEFT OUTER JOIN tblCatalogCustMaterial ccm on cat.apkCatalogItem = ccm.MaterialID
WHERE cat.Hide = 0
	AND CASE 
		WHEN @CustID IS NULL THEN 1  
		WHEN cath.afkHeadCust = @CustID THEN 1 
		 ELSE 0 END  = 1
	AND CASE
		WHEN @CustID IS NULL THEN 1
		WHEN ccm.CustID IS NULL THEN 1
		WHEN ccm.CustID = @CustID THEN 1
		ELSE 0 END = 1
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

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
	@ShipVia nvarchar(100) = null,
	@RefNum nvarchar(100) = null
AS
IF @ShipFromDate IS NULL
	SELECT @ShipFromDate = '1/1/1900'
IF @ShipToDate IS NULL
	SELECT @ShipToDate = '1/1/2100'
SELECT 
	CASE 
		WHEN ( (po IS NOT NULL and po.PO != '') and (po.BlanketPO is null or po.BlanketPO = '')) THEN po.PO  
		WHEN ( (po.PO IS NULL or po.PO = '') and (po.BlanketPO is not null and po.BlanketPO != '')) THEN po.BlanketPO 
		WHEN ( (po.PO IS NOT NULL and po.PO != '') and (po.BlanketPO is NOT NULL and po.BlanketPO != '')) THEN po.PO + CHAR(13) +  CHAR(10) + po.BlanketPO
	END AS [PONumber],
	po.Ship_City + ', ' + po.Ship_State AS City_State, po.ShelfType, po.Arrive_Date, po.Ship_Date, po.Ship_Via, 
	po.Comments, po.Ship_Name,po.Ship_StoreNo,  spoi2.ExtPrice, spoi2.ExtQty, spoi2.ExtWeight, po.FTofTruckAmount
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
ORDER BY po.Ship_Date, [PONumber]
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO




insert ztblReleaseNotes (Date, MajorVersion, MinorVersion, Notes)
values ('2/14/2002', 3.30, 0, '- Reports: Cashwrap Shipments for Fisher - increased timeout, made cancellable, fixed so it works with blanket POs
- Reports: Cashwrap Shipments with ship info - increased timeout, made cancellable, fixed so it works with blanket POs
- Reports: POs by Job Number - can now displays blanket POs
- Reports: Work Order: can now displays blanket POs, added Arrive Date
- Reports: Shipment Schedule: can now displays blanket POs, added Store #
- Reports: Shipment Schedule with Amt: can now displays blanket POs, added Store #
- Reports: Shipment Schedule Ship Via: can now displays blanket POs, added Store #
- Reports: Shipment Schedule Wheat: can now displays blanket POs, added Store #
- Reports: Cust Shipment Schedule: can now displays blanket POs
- Reports: Incomplete POs: can now displays blanket POs
- Reports: Maintenance Orders: can now displays blanket POs
- Reports: Hot Orders: can now displays blanket POs
- Reports: Gap Order Confirmation: can now displays blanket POs
- Reports: Outstanding POs: can now displays blanket POs
- Reports: Product Catalog: Added cust material #
')
go

