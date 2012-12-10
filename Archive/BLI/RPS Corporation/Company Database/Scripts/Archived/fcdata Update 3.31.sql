-- fcdata update 3.31
-- - update rel notes, db version
-- - change dealer table

if exists (select * from sysobjects where id = object_id(N'[dbo].[ppsprptVendorRelease]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ppsprptVendorRelease]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE ppsprptVendorRelease
	@sPOID varchar(20) = null,
	@sRPSPartNum varchar(40) = null,
	@sVendorPartNum varchar(40) = null,
	@iOpenFlag varchar(2) = null
AS

if @sPOID is null
	select @sPOID = '%'
if @sRPSPartNum is null
	select @sRPSPartNum = '%'
if @sVendorPartNum is null
	select @sVendorPartNum = '%'
if @iOpenFlag = 1
	SELECT po.POID, po.Vendor, pp.VendorPartNumber, pp.RPSPartNum, pp.PartDescription, pd.RequestedShipDate,
		pd.Quantity, pd.Value, pd.ReceivedDate
	FROM dbo.tblPO po, dbo.tblPOPart pp, dbo.tblPOPartDetail pd
	WHERE po.POID = pp.fkPOID and pp.POPartID = pd.fkPOPartID and pd.RequestedShipDate is not null
		and po.POID like @sPOID
		and pp.RPSPartNum like @sRPSPartNum
		and pp.VendorPartNumber like @sVendorPartNum
		and pd.ReceivedDate is null
	ORDER BY pp.RPSPartNum, po.POID, pp.VendorPartNumber, pd.RequestedShipDate
else
	SELECT po.POID, po.Vendor, pp.VendorPartNumber, pp.RPSPartNum, pp.PartDescription, pd.RequestedShipDate,
		pd.Quantity, pd.Value, pd.ReceivedDate
	FROM dbo.tblPO po, dbo.tblPOPart pp, dbo.tblPOPartDetail pd
	WHERE po.POID = pp.fkPOID and pp.POPartID = pd.fkPOPartID and pd.RequestedShipDate is not null
		and po.POID like @sPOID
		and pp.RPSPartNum like @sRPSPartNum
		and pp.VendorPartNumber like @sVendorPartNum
	ORDER BY pp.RPSPartNum, po.POID, pp.VendorPartNumber, pd.RequestedShipDate





GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ppsprptVendorRelease]  TO [fcuser]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[pasprptDealerPartsList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[pasprptDealerPartsList]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE pasprptDealerPartsList
	@iType int = null
AS

select tP.RPSPartNum, tP.PartName, tP.DealerNet, tP.SuggestedList, tP.Note, tP.PartName, tP.RPSPNSort
from dbo.tblParts tP, dbo.tblModels tM, dbo.tblPartsModels tPM
where tP.PartID = tPM.fkPartID and tPM.Model = tM.Model and tM.ModelType = @iType and tPM.Quantity > 0 and
	tP.RPSPartNum not like 'V%' And tP.RPSPartNum not like 'E%' and tP.RPSPartNum not like 'H%' and tP.HideOnReports = 0
group by tP.RPSPartNum, tP.PartName, tP.DealerNet, tP.SuggestedList, tP.Note, tP.PartName, tP.RPSPNSort
order by tP.RPSPNSort


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[pasprptDealerPartsList]  TO [fcuser]
GO


drop view dbo.parqryDealerPartsListView
go


delete tblPartsModels where PartModelID = 12150 or PartModelID = 8364 or PartModelID = 13948
go

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblPartsModels ADD CONSTRAINT
	IX_tblPartsModels UNIQUE NONCLUSTERED 
	(
	Model,
	fkPartID
	) ON [PRIMARY]
GO
COMMIT
go

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
CREATE TABLE dbo.tblPartsVendors
	(
 	PartVendorID int NOT NULL IDENTITY (1, 1),
	PartID int NOT NULL,
	VendorID int NOT NULL,
	VendorPartName nvarchar(150) NULL,
	VendorPartNum nvarchar(100) NULL,
	VendorCostEach smallmoney NULL,
	VendorPrimary bit NOT NULL CONSTRAINT DF_tblPartsVendors_VendorPrimary DEFAULT (0)
	) ON [PRIMARY]
GO
ALTER TABLE dbo.tblPartsVendors ADD CONSTRAINT
	PK_tblPartsVendors PRIMARY KEY NONCLUSTERED 
	(
	PartVendorID
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblPartsVendors_PartID ON dbo.tblPartsVendors
	(
	PartID
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblPartsVendors_VendorID ON dbo.tblPartsVendors
	(
	VendorID
	) ON [PRIMARY]
GO
COMMIT



update tblOrders
set Model = '40HD'
where Model = '40' and OrderDate < '1/1/2000'
go

update tblOrders
set Model = '52HD'
where Model = '52' and OrderDate < '1/1/2000'
go

update tblParts set VendorName = 'BWD Automotive, (All Lock)' where VendorName = 'All-Lock Co., Inc.'
go

update tblParts set VendorName = 'C & H Distributors' where VendorName = 'C & H'
go

update tblParts set VendorName = 'Capital Stampings Corp' where VendorName = 'Capitol Stampings Corp'
go

update tblParts set VendorName = 'Crown Battery Mfg. Co., Inc.' where VendorName = 'Crown Battery'
go

update tblParts set VendorName = 'Curtis Instruments, Inc' where VendorName = 'Curtis Instruments PR'
go

update tblParts set VendorName = 'Darcor Limited' where VendorName = 'Darcor'
go

update tblParts set VendorName = 'Deltrol Controls' where VendorName = 'Deltrol'
go

update tblParts set VendorName = 'Electronic Expeditors, Inc.' where VendorName = 'Electronic Expeditors'
go

update tblParts set VendorName = 'Federal Signal/Target Tec.' where VendorName = 'Federal Signal'
go

update tblParts set VendorName = 'Grayline Inc' where VendorName = 'Grayline'
go

update tblParts set VendorName = 'Kranz, Inc.' where VendorName = 'Kranz'
go

update tblParts set VendorName = 'LaCharite Mohr Jones' where VendorName = 'LaCharite'
go

update tblParts set VendorName = 'L.E. Peterson Tool & Die' where VendorName = 'Lyle Peterson'
go

update tblParts set VendorName = 'McMaster Carr Supply Co' where VendorName = 'McMaster Carr'
go

update tblParts set VendorName = 'NAPA  Automotive Parts Co.' where VendorName = 'NAPA'
go

update tblParts set VendorName = 'Nelson Electric Supply Co' where VendorName = 'Nelson   (Electricals)'
go

update tblParts set VendorName = 'Pottinger Steel Works, Inc.' where VendorName = 'Pottinger'
go

update tblParts set VendorName = 'Quick-Cable Corporation' where VendorName = 'Quick Cable'
go

update tblParts set VendorName = 'RadioShack.com' where VendorName = 'Radio Shack'
go

update tblParts set VendorName = 'RPS Corp' where VendorName = 'RPS Corporation'
go

update tblParts set VendorName = 'Superior Tire & Rubber' where VendorName = 'Superior Tire'
go

update tblParts set VendorName = 'Systems Material Handling' where VendorName = 'System Material Handling'
go

insert into tblVendors (VendorName, Active)
values ('All Fasteners', 0)
go

insert into tblVendors (VendorName, Active)
values ('Bauer Welding', 0)
go

insert into tblVendors (VendorName, Active)
values ('Delta Systems, Inc.', 1)
go

insert into tblVendors (VendorName, Active)
values ('Ekco /Wright Bernet', 1)
go

insert into tblVendors (VendorName, Active)
values ('Hedke Marketing', 0)
go

insert into tblVendors (VendorName, Active)
values ('Industrial Engine & Parts', 0)
go

insert into tblVendors (VendorName, Active)
values ('Industrial Filter Manufacturers', 0)
go

insert into tblVendors (VendorName, Active)
values ('Longview Fibre', 0)
go

insert into tblVendors (VendorName, Active)
values ('Neilson Wheel', 0)
go

insert into tblVendors (VendorName, Active)
values ('Savel', 1)
go

insert into tblVendors (VendorName, Active)
values ('Stancor', 1)
go

insert into tblVendors (VendorName, Active)
values ('Tech America', 1)
go

update tblParts set VendorName = 'RPS Corp' where VendorName = 'Dealer Parts'
go

update tblParts set VendorName = 'RPS Corp' where VendorName = 'Hardware'
go

update tblParts
set VendorName = 'RPS Corp'
where VendorName is null
go

insert tblPartsVendors (PartID, VendorID, VendorPartName, VendorPartNum, VendorCostEach, VendorPrimary)
select pa.PartID, ve.VendorID, pa.VendorPartName, pa.ManfPartNum, pa.CostEach, 1
from dbo.tblVendors ve, dbo.tblParts pa
where ve.VendorName = pa.VendorName
go

if exists (select * from sysobjects where id = object_id(N'[dbo].[paspfrmPartsVendors]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[paspfrmPartsVendors]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE paspfrmPartsVendors
	@iPartID int = 0
AS

select ve.VendorName, pv.PartVendorID, pv.VendorPartName, pv.VendorPartNum, pv.VendorCostEach, pv.VendorPrimary
from dbo.tblVendors ve, dbo.tblPartsVendors pv
where ve.VendorID = pv.VendorID
	and pv.PartID = @iPartID
order by pv.VendorPrimary desc, ve.VendorName
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[paspfrmPartsVendors]  TO [fcuser]
GO



--insert [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
--values (31, 2, 'Tom Cat Prospects by State', 4, 'tcrptProspectListState')
--go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.31','2000-2-13 00:00:00','- Added ''Vendor Release Schedule'' tab to Parts screen
- Updated ''Cycle Counts'' reports to add new areas and allow price range to be entered
- Fixed duplicate part showing on Prod Parts by Model
- Bill of Materials now sorts parts by In House date
- Fixed Prep Sheet not working with HD models
- Updated all Model 40 & 52''s before 1/1/2000 to 40HD and 52HD
- Cleaned up Vendors in Parts file to match the Vendors file
- Added capability to add multiple vendors to the Parts file
- Redesigned Parts screen removing extra fields
')
go

update tblDBProperties
set PropertyValue = '3.31'
where PropertyName = 'DBStructVersion'
go
