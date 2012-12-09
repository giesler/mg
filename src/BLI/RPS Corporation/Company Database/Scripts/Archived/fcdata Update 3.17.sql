-- fcdata Update 3.17
-- - add sp for prod sched report
-- - update rel notes, db version

if exists (select * from sysobjects where id = object_id(N'[dbo].[pasprptProductionSchedule]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[pasprptProductionSchedule]
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

select pa.RPSPartNum, Sum(si.Quantity * pm.Quantity) AS QtyReqd, pa.VendorName
from dbo.tblParts pa, dbo.tblProdSchedules ps, dbo.tblProdSchedItems si, dbo.tblPartsModels pm
where pm.Model = si.Model
	and pa.PartID = pm.fkPartID
	and ps.ScheduleID = si.ScheduleID
	and pa.HideOnReports = 0
	and CAST(CAST([Year] AS char(4)) + CAST(CAST(REPLICATE('0', 2 - DATALENGTH(CAST([Month] as varchar(2)))) AS varchar(3)) +  CAST([Month] as varchar(2)) AS char(2))  AS int)
		Between @sFrom And @sTo
	and pa.VendorName Like @sVendorName
group by pa.RPSPartNum, pa.VendorName
having Sum(si.Quantity * pm.Quantity) > 0
order by pa.RPSPartNum

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[pasprptProductionSchedule]  TO [fcuser]
GO

ALTER TABLE dbo.tblWarranty ADD CONSTRAINT
	DF_tblWarranty_DateEntered DEFAULT (GETDATE()) FOR DateEntered
GO

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.17','1999-12-01 00:00:00','- Fixed Production Schedule screen and report in Parts
- Added grand totals to Production Releases report in Production Parts.
- When adding records in warranty the current date is entered automatically.
- Changed the tab order a bit on orders to go between pages automatically.
- Changed the tab order for App/Notes and Results on all Leads forms (Leads, Purchased Leads, Tom Cat Leads)')
go

update tblDBProperties
set PropertyValue = '3.17'
where PropertyName = 'DBStructVersion'
go
