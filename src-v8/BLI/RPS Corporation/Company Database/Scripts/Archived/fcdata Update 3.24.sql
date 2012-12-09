-- fcdata update 3.24
-- - update rel notes, db version

alter table dbo.tblParts drop column upsize_ts 
go

alter table dbo.tblParts add RPSPNSort char(30) NULL
go

create nonclustered index IX_tblParts_RPSPNSort ON dbo.tblParts (RPSPNSort)
go

update dbo.tblParts
set RPSPNSort = substring(RPSPartNum, 1, charindex('-',RPSPartNum)-1) + replicate('0', 10 - charindex('-',RPSPartNum)-1)
	+ '-' + substring(RPSPartNum, charindex('-', RPSPartNum) + 1, 21) + replicate('0', 21 - len(substring(RPSPartNum, charindex('-', RPSPartNum) + 1, 21)))
where RPSPartNum is not null and charindex('-',RPSPartNum) > 0
go

update dbo.tblParts
set RPSPNSort = RPSPartNum + replicate('0',30 - len(RPSPartNum))
where RPSPartNum is not null and charindex('-',RPSPartNum) = 0
go

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE TRIGGER tblParts_ITrig  ON [tblParts] 
FOR INSERT 
AS
SET NOCOUNT ON
IF (SELECT COUNT(*) FROM inserted WHERE RPSPartNum IS NOT NULL and charindex('-',RPSPartNum) > 0) > 0
  begin
	update a
	set a.RPSPNSort = substring(a.RPSPartNum, 1, charindex('-', a.RPSPartNum)-1) + replicate('0', 10 - charindex('-', a.RPSPartNum)-1)
		+ '-' + substring(a.RPSPartNum, charindex('-', a.RPSPartNum) + 1, 21) + replicate('0', 21 - len(substring(a.RPSPartNum, charindex('-', a.RPSPartNum) + 1, 21)))
	from dbo.tblParts a, inserted
	where inserted.RPSPartNum is not null and charindex('-',inserted.RPSPartNum) > 0 and inserted.RPSPartNum = a.RPSPartNum
  end
IF (SELECT COUNT(*) FROM inserted WHERE RPSPartNum IS NOT NULL and charindex('-',RPSPartNum)= 0) > 0
  begin
	update a
	set a.RPSPNSort = a.RPSPartNum + replicate('0',30 - len(a.RPSPartNum))
	from dbo.tblParts a, inserted
	where a.RPSPartNum is not null and charindex('-',a.RPSPartNum) = 0 and inserted.RPSPartNum = a.RPSPartNum
  end


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE TRIGGER tblParts_UTring ON tblParts
FOR UPDATE
AS
SET NOCOUNT ON
IF (SELECT COUNT(*) FROM inserted WHERE RPSPartNum IS NOT NULL and charindex('-',RPSPartNum) > 0) > 0
  begin
	update a
	set a.RPSPNSort = substring(a.RPSPartNum, 1, charindex('-', a.RPSPartNum)-1) + replicate('0', 10 - charindex('-', a.RPSPartNum)-1)
		+ '-' + substring(a.RPSPartNum, charindex('-', a.RPSPartNum) + 1, 21) + replicate('0', 21 - len(substring(a.RPSPartNum, charindex('-', a.RPSPartNum) + 1, 21)))
	from dbo.tblParts a, inserted
	where inserted.RPSPartNum is not null and charindex('-',inserted.RPSPartNum) > 0 and inserted.RPSPartNum = a.RPSPartNum
  end
IF (SELECT COUNT(*) FROM inserted WHERE RPSPartNum IS NOT NULL and charindex('-',RPSPartNum)= 0) > 0
  begin
	update a
	set a.RPSPNSort = a.RPSPartNum + replicate('0',30 - len(a.RPSPartNum))
	from dbo.tblParts a, inserted
	where a.RPSPartNum is not null and charindex('-',a.RPSPartNum) = 0 and inserted.RPSPartNum = a.RPSPartNum
  end
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[parqryDealerPartsList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[parqryDealerPartsList]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[paqrptListPrices]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[paqrptListPrices]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[paqrptPartListFinish]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[paqrptPartListFinish]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[parptRPSPartsPricing]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[parptRPSPartsPricing]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblOrderPrepByList]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblOrderPrepByList]
GO

CREATE TABLE [dbo].[tblOrderPrepByList] (
	[PrepByName] [nvarchar] (50) NOT NULL ,
	[PrepByDefault] [bit] NOT NULL 
) ON [PRIMARY]
GO

insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (13, 5, 'Add/edit/view prepared by names', 3, 'orfrmPrepByNamesEdit')
go

insert into tblOrderPrepByList (PrepByName, PrepByDefault)
values ('Irene Mopps', 0)
go

insert into tblOrderPrepByList (PrepByName, PrepByDefault)
values ('Vicki Plachter', 1)
go

if exists (select * from sysobjects where id = object_id(N'[dbo].[paqrptPartListFinish]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[paqrptPartListFinish]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[paqryExportDealerParts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[paqryExportDealerParts]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[pasprptBillOfMaterials]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[pasprptBillOfMaterials]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[pasprptPartLabels]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[pasprptPartLabels]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[pasprptProductionSchedule]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[pasprptProductionSchedule]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[pasprptRPSPartsPricing]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[pasprptRPSPartsPricing]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[pasprsubBillOfMaterialsPParts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[pasprsubBillOfMaterialsPParts]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE paqrptPartListFinish
AS
SELECT * FROM "paqrptPartListFinishView"
ORDER BY "paqrptPartListFinishView".RPSPartNum, "paqrptPartListFinishView".PartName


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[paqrptPartListFinish]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE paqryExportDealerParts
AS
SELECT tblParts.RPSPartNum, tblParts.PartName, tblParts.DealerNet AS "Dealer Net Price", tblParts.SuggestedList AS "Suggested List Price", tblParts.QtyReq AS "Quantity Required", tblParts.Notes, tblParts.PartID
FROM tblParts
WHERE (((tblParts.DealerNet)>0))
ORDER BY tblParts.PartName



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[paqryExportDealerParts]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE pasprptBillOfMaterials
	@sVendorName varchar(50),
	@sModel varchar(10)
 AS
DECLARE @sSQL varchar(1500)
DECLARE @sWhere varchar(500)
SELECT @sWhere = ''
SELECT @sSQL = 
'SELECT p.RPSPartNum, p.PartName, p.VendorName, p.CostEach, p.Quantity, 
	CONVERT (smallmoney, CONVERT (smallmoney, p.CostEach) * CONVERT (int, m.Quantity) ) AS ExtCost, p.RPSPNSort
FROM dbo.tblParts p, dbo.tblPartsModels m
WHERE p.PartID = m.fkPartID AND m.Quantity > 0 '
-- Check model
IF @sModel IS NOT NULL
	SELECT @sWhere = ' m.Model = ' + @sModel + ' '
-- Check vendor
IF @sVendorName IS NOT NULL
  BEGIN
	IF @sWhere <> '' 
		SELECT @sWhere = @sWhere + ' AND '
	SELECT @sWhere = @sWhere +  'p.VendorName = ''' + @sVendorName + ''''
	SELECT @sWhere = @sWhere + ' AND p.RPSPartNum IN ('
	SELECT @sWhere = @sWhere + '
		SELECT pop.RPSPartNum
		FROM dbo.tblPO po, dbo.tblPOPart pop, dbo.tblPOPartDetail popd
		WHERE po.POID = pop.fkPOID AND pop.POPartID = popd.fkPOPartID
			AND popd.RequestedShipDate IS NOT NULL
			AND popd.ReceivedDate IS NULL )'
  END
IF @sWhere <> ''
  BEGIN
	SELECT @sSQL = @sSQL + ' AND ' + @sWhere
  END
EXEC (@sSQL)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[pasprptBillOfMaterials]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE pasprptPartLabels 
	@sStartPart varchar(50) = null,
	@sEndPart varchar(50) = null,
	@sModel varchar(50) = null,
	@iQty int = 1
AS
IF @sStartPart IS NULL
	SELECT @sStartPart = '000000000000000000'
IF @sEndPart IS NULL
	SELECT @sEndPart = 'zzzzzzzzzzzzzzzzzzzzzz'
IF @sModel IS NULL
	SELECT @sModel = '%'
IF @iQty IS NULL
	SELECT @iQty = 1
SELECT pa.RPSPartNum, pa.PartName, pa.VendorName, pa.VendorPartName, pa.ManfPartNum, nu.Counter, pa.Model, pa.RPSPNSort
FROM dbo.tblParts pa, dbo.tbl_Numbers nu
WHERE nu.Counter <= @iQty		-- allows multiple labels
	and pa.RPSPartNum Between @sStartPart And @sEndPart
	and pa.Model Like @sModel

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[pasprptPartLabels]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE pasprptProductionSchedule
	@sFromMonth char(2) = null,
	@sFromYear char(4) = null,
	@sToMonth char(2) = null,
	@sToYear char(4) = null,
	@sVendorName varchar(50) = null
AS
IF @sFromMonth IS NULL
	SELECT @sFromMonth = '00'
IF @sFromYear IS NULL
	SELECT @sFromYear = '0000'
IF @sToMonth IS NULL
	SELECT @sToMonth = '99'
IF @sToYear IS NULL
	SELECT @sToYear = '2050'
IF @sVendorName IS NULL
	SELECT @sVendorName = '%'
DECLARE @sFrom char(6)
DECLARE @sTo char(6)
SELECT @sFrom = @sFromYear + CAST(CAST(REPLICATE('0', 2 - DATALENGTH(CAST(RTRIM(@sFromMonth) as varchar(2)))) AS varchar(3)) +  CAST(@sFromMonth as varchar(2)) AS char(2))
SELECT @sTo = @sToYear + CAST(CAST(REPLICATE('0', 2 - DATALENGTH(CAST(RTRIM(@sToMonth) as varchar(2)))) AS varchar(3)) +  CAST(@sToMonth as varchar(2)) AS char(2))
select pa.RPSPartNum, Sum(si.Quantity * pm.Quantity) AS QtyReqd, pa.VendorName, pa.RPSPNSort
from dbo.tblParts pa, dbo.tblProdSchedules ps, dbo.tblProdSchedItems si, dbo.tblPartsModels pm
where pm.Model = si.Model
	and pa.PartID = pm.fkPartID
	and ps.ScheduleID = si.ScheduleID
	and pa.HideOnReports = 0
	and CAST(CAST([Year] AS char(4)) + CAST(CAST(REPLICATE('0', 2 - DATALENGTH(CAST([Month] as varchar(2)))) AS varchar(3)) +  CAST([Month] as varchar(2)) AS char(2))  AS int)
		Between @sFrom And @sTo
	and pa.VendorName Like @sVendorName
group by pa.RPSPNSort, pa.RPSPartNum, pa.VendorName
having Sum(si.Quantity * pm.Quantity) > 0
order by pa.RPSPNSort

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[pasprptProductionSchedule]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE pasprptRPSPartsPricing 
AS

select tP.RPSPartNum, tP.VendorName, tP.ManfPartNum, tP.SuggestedList, tP.DealerNet, tP.PartName, tP.HideOnReports, tP.RPSPNSort
from dbo.tblParts tP
where tP.RPSPartNum not like 'V%' And tP.RPSPartNum not like 'E%' and tP.RPSPartNum not like 'H%' and tP.HideOnReports = 0
order by tP.RPSPNSort

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[pasprptRPSPartsPricing]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE pasprsubBillOfMaterialsPParts
	@sVendorName varchar(50)
 AS

DECLARE @sSQL varchar(1500)
DECLARE @sWhere varchar(500)
SELECT @sWhere = ''

SELECT @sSQL = '
SELECT dbo.tblPOPart.RPSPartNum, dbo.tblPO.POID, dbo.tblPOPart.VendorPartNumber, dbo.tblPO.Vendor, 
	dbo.tblPOPartDetail.RequestedShipDate, dbo.tblPOPartDetail.CostEach, dbo.tblPOPartDetail.Value, 
	dbo.tblPOPartDetail.Quantity
FROM dbo.tblPO INNER JOIN dbo.tblPOPart ON dbo.tblPO.POID = dbo.tblPOPart.fkPOID INNER JOIN
	dbo.tblPOPartDetail ON dbo.tblPOPart.POPartID = dbo.tblPOPartDetail.fkPOPartID
WHERE (dbo.tblPOPartDetail.RequestedShipDate IS NOT NULL) AND 
	(dbo.tblPOPartDetail.ReceivedDate IS NULL)'

-- Check vendor
IF @sVendorName IS NOT NULL
  BEGIN
	SELECT @sSQL = @sSQL +  ' dbo.tblParts.VendorName = ''' + @sVendorName + ''''
  END

EXEC (@sSQL)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[pasprsubBillOfMaterialsPParts]  TO [fcuser]
GO


if exists (select * from sysobjects where id = object_id(N'[dbo].[paqrptListPricesView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[paqrptListPricesView]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[paqrptPartLabels]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[paqrptPartLabels]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[paqrptPartLabelsMulti]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[paqrptPartLabelsMulti]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[paqrptPartListFinishView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[paqrptPartListFinishView]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[paqrptPartsPerModel2]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[paqrptPartsPerModel2]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[parptRPSPartsPricingView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[parptRPSPartsPricingView]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[parqryDealerPartsListView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[parqryDealerPartsListView]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE VIEW paqrptListPricesView
AS
SELECT tblParts.RPSPartNum, tblParts.PartName, tblParts.SuggestedList, tblParts.Note, tblParts.HideOnReports, tblParts.RPSPNSort
FROM tblParts
WHERE (((tblParts.RPSPartNum) Not Like 'V%' And (tblParts.RPSPartNum) Not Like 'E%' And (tblParts.RPSPartNum) Not Like 'H%') And ((tblParts.SuggestedList)>0.01) And ((tblParts.HideOnReports)=0))

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE VIEW paqrptPartLabels
AS
SELECT tblParts.RPSPartNum, tblParts.PartName, tblParts.VendorName, tblParts.VendorPartName, tblParts.ManfPartNum, Tbl_Numbers.Counter, tblParts.Model
FROM Tbl_Numbers, tblParts
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE VIEW paqrptPartLabelsMulti
AS
SELECT dbo.tblParts.RPSPartNum, dbo.tblParts.PartName, 
    dbo.tblParts.VendorName, dbo.tblParts.VendorPartName, 
    dbo.tblParts.ManfPartNum, dbo.Tbl_Numbers.Counter, dbo.tblParts.RPSPNSort
FROM dbo.Tbl_Numbers INNER JOIN
    dbo.tblParts INNER JOIN
    dbo.tmpLabelQtys ON 
    dbo.tblParts.PartID = dbo.tmpLabelQtys.PartIndex ON 
    dbo.Tbl_Numbers.Counter <= dbo.tmpLabelQtys.Quantity

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE VIEW paqrptPartListFinishView
AS
SELECT tblParts.RPSPartNum, tblParts.PartName, tblParts.FinishDesc, tblParts.RPSPNSort
FROM tblParts
WHERE (((tblParts.FinishDesc) Is Not Null))

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE VIEW paqrptPartsPerModel2
AS
SELECT tblParts.PartID, tblParts.RPSPartNum, tblParts.PartName, tblModels.Model
FROM tblParts, tblModels
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "parptRPSPartsPricingView"
AS
SELECT tblParts.RPSPartNum, tblParts.VendorName, tblParts.ManfPartNum, tblParts.SuggestedList, tblParts.DealerNet, tblParts.PartName, tblParts.HideOnReports
FROM tblParts
WHERE (((tblParts.RPSPartNum) Not Like 'V%' And (tblParts.RPSPartNum) Not Like 'E%' And (tblParts.RPSPartNum) Not Like 'H%') And ((tblParts.HideOnReports)=0))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE VIEW parqryDealerPartsListView
AS
SELECT tblParts.RPSPartNum, tblParts.PartName, tblParts.DealerNet, tblParts.SuggestedList, tblParts.Note, tblParts.RPSPNSort
FROM tblParts
WHERE (((tblParts.RPSPartNum) Not Like 'V%' And (tblParts.RPSPartNum) Not Like 'E%' And (tblParts.RPSPartNum) Not Like 'H%') And ((tblParts.DealerNet)<>0 And (tblParts.DealerNet) Is Not Null) And ((tblParts.HideOnReports)=0))



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

update [Switchboard Items] set ItemNumber = ItemNumber + 1 
where ItemNumber > 11
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (14, 12, 'Sales by Model with Order Number', 4, 'orrptSalesModelON')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.24','1999-12-10 00:00:00','- Fixed Parts model quantities not working in a new part.
- Fixed Prod Parts showing old part info when adding a new PO.
- Changed reports and such that sort on RPS Part Number to sort correctly.
- Added a ''Prepared by'' drop down box to the Acknowledgement Report criteria.  The names can be edited from a new option on the Orders menu.
- Added ''Open/Closed/both'' criteria to Cash Flow report in Prod Parts.

')
go

update tblDBProperties
set PropertyValue = '3.24'
where PropertyName = 'DBStructVersion'
go
