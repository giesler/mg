-- fcdata update 3.32
-- - update rel notes, db version
-- - change dealer table

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblPartsModels
	DROP CONSTRAINT DF__Temporary__fkPar__76969D2E
GO
ALTER TABLE dbo.tblPartsModels
	DROP CONSTRAINT DF__Temporary__Model__778AC167
GO
ALTER TABLE dbo.tblPartsModels
	DROP CONSTRAINT DF__Temporary__Quant__787EE5A0
GO
ALTER TABLE dbo.tblPartsModels
	DROP CONSTRAINT DF__tblPartsM__Optio__0EF836A4
GO
CREATE TABLE dbo.Tmp_tblPartsModels
	(
 	PartModelID int NOT NULL IDENTITY (1, 1),
	fkPartID int NULL CONSTRAINT DF__Temporary__fkPar__76969D2E DEFAULT (0),
	Model nvarchar(20) NULL CONSTRAINT DF__Temporary__Model__778AC167 DEFAULT (''),
	Quantity float(53) NULL CONSTRAINT DF__Temporary__Quant__787EE5A0 DEFAULT (0),
	Optional smallint NOT NULL CONSTRAINT DF__tblPartsM__Optio__0EF836A4 DEFAULT (0)
	) ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblPartsModels ON
GO
IF EXISTS(SELECT * FROM dbo.tblPartsModels)
	 EXEC('INSERT INTO dbo.Tmp_tblPartsModels(PartModelID, fkPartID, Model, Quantity, Optional)
		SELECT PartModelID, fkPartID, Model, Quantity, CONVERT(smallint, Optional) FROM dbo.tblPartsModels TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblPartsModels OFF
GO
DROP TABLE dbo.tblPartsModels
GO
EXECUTE sp_rename 'dbo.Tmp_tblPartsModels', 'tblPartsModels'
GO
ALTER TABLE dbo.tblPartsModels ADD CONSTRAINT
	tblPartsModels_PK PRIMARY KEY NONCLUSTERED 
	(
	PartModelID
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX fkPartID ON dbo.tblPartsModels
	(
	fkPartID
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
ALTER TABLE dbo.tblPartsModels ADD CONSTRAINT
	IX_tblPartsModels UNIQUE NONCLUSTERED 
	(
	Model,
	fkPartID
	) ON [PRIMARY]
GO
COMMIT


BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblParts
	DROP CONSTRAINT DF__Temporary__RPSPu__693CA210
GO
ALTER TABLE dbo.tblParts
	DROP CONSTRAINT DF__TemporaryUp__Qty__6A30C649
GO
ALTER TABLE dbo.tblParts
	DROP CONSTRAINT DF__Temporary__Total__6B24EA82
GO
ALTER TABLE dbo.tblParts
	DROP CONSTRAINT DF__Temporary__SubPa__6C190EBB
GO
ALTER TABLE dbo.tblParts
	DROP CONSTRAINT DF__Temporary__SubFl__6D0D32F4
GO
ALTER TABLE dbo.tblParts
	DROP CONSTRAINT DF__Temporary__Shelf__6E01572D
GO
ALTER TABLE dbo.tblParts
	DROP CONSTRAINT DF__Temporary__Level__6EF57B66
GO
ALTER TABLE dbo.tblParts
	DROP CONSTRAINT DF__Temporary__HideO__6FE99F9F
GO
ALTER TABLE dbo.tblParts
	DROP CONSTRAINT DF__Temporary__Optio__70DDC3D8
GO
ALTER TABLE dbo.tblParts
	DROP CONSTRAINT DF__Temporary__RunQu__71D1E811
GO
CREATE TABLE dbo.Tmp_tblParts
	(
 	PartID int NOT NULL IDENTITY (1, 1),
	PartName nvarchar(150) NULL,
	Location nvarchar(150) NULL,
	VendorName nvarchar(100) NULL,
	VendorPartName nvarchar(150) NULL,
	ManfPartNum nvarchar(50) NULL,
	RPSPartNum nvarchar(50) NULL,
	CostEach smallmoney NULL,
	QuoteDate smalldatetime NULL,
	VendorCost smallmoney NULL,
	DealerNet smallmoney NULL CONSTRAINT DF_tblParts_DealerNet DEFAULT (0),
	SuggestedList smallmoney NULL CONSTRAINT DF_tblParts_SuggestedList DEFAULT (0),
	Note nvarchar(255) NULL,
	ShelfNo float(53) NULL CONSTRAINT DF__Temporary__Shelf__6E01572D DEFAULT (0),
	Section nvarchar(10) NULL,
	PartLevel float(53) NULL CONSTRAINT DF__Temporary__Level__6EF57B66 DEFAULT (0),
	HideOnReports bit NOT NULL CONSTRAINT DF__Temporary__HideO__6FE99F9F DEFAULT (0),
	EffectiveDate smalldatetime NULL,
	SNEffective nvarchar(50) NULL,
	FinishDesc nvarchar(50) NULL,
	FinishVendorID int NULL,
	RunQuantity int NULL CONSTRAINT DF__Temporary__RunQu__71D1E811 DEFAULT (0),
	LeadTimeDays nvarchar(50) NULL,
	DrawingNum nvarchar(50) NULL,
	RevisionNum nvarchar(50) NULL,
	RPSPNSort char(30) NULL,
	FreightLine nvarchar(50) NULL,
	PartCode nvarchar(50) NULL
	) ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblParts ON
GO
IF EXISTS(SELECT * FROM dbo.tblParts)
	 EXEC('INSERT INTO dbo.Tmp_tblParts(PartID, PartName, Location, VendorName, VendorPartName, ManfPartNum, RPSPartNum, CostEach, QuoteDate, VendorCost, DealerNet, SuggestedList, Note, ShelfNo, Section, PartLevel, HideOnReports, EffectiveDate, SNEffective, FinishDesc, FinishVendorID, RunQuantity, LeadTimeDays, DrawingNum, RevisionNum, RPSPNSort)
		SELECT PartID, PartName, Location, VendorName, VendorPartName, ManfPartNum, RPSPartNum, CONVERT(smallmoney, CostEach), CONVERT(smalldatetime, QuoteDate), CONVERT(smallmoney, VendorCost), CONVERT(smallmoney, DealerNet), CONVERT(smallmoney, SuggestedList), Note, ShelfNo, Section, PartLevel, HideOnReports, CONVERT(smalldatetime, EffectiveDate), SNEffective, FinishDesc, FinishVendorID, RunQuantity, LeadTimeDays, DrawingNum, RevisionNum, RPSPNSort FROM dbo.tblParts TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblParts OFF
GO
DROP TABLE dbo.tblParts
GO
EXECUTE sp_rename 'dbo.Tmp_tblParts', 'tblParts'
GO
ALTER TABLE dbo.tblParts ADD CONSTRAINT
	tblParts_PK PRIMARY KEY NONCLUSTERED 
	(
	PartID
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX DrawingNum ON dbo.tblParts
	(
	DrawingNum
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX ManfPartNum ON dbo.tblParts
	(
	ManfPartNum
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX partname ON dbo.tblParts
	(
	PartName
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX RevisionNum ON dbo.tblParts
	(
	RevisionNum
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX rpspn ON dbo.tblParts
	(
	RPSPartNum
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX Vendorname ON dbo.tblParts
	(
	VendorName
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblParts_RPSPNSort ON dbo.tblParts
	(
	RPSPNSort
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE TRIGGER tblParts_ITrig  ON dbo.tblParts 
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
CREATE TRIGGER tblParts_UTring ON dbo.tblParts
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
COMMIT


-- backup current parts table
CREATE TABLE dbo.tblParts_BACKUP
	(
 	PartID int NOT NULL IDENTITY (1, 1),
	PartName nvarchar(150) NULL,
	Location nvarchar(150) NULL,
	VendorName nvarchar(100) NULL,
	VendorPartName nvarchar(150) NULL,
	ManfPartNum nvarchar(50) NULL,
	RPSPartNum nvarchar(50) NULL,
	CostEach smallmoney NULL,
	QuoteDate smalldatetime NULL,
	VendorCost smallmoney NULL,
	DealerNet smallmoney NULL CONSTRAINT DF_tblParts1_DealerNet DEFAULT (0),
	SuggestedList smallmoney NULL CONSTRAINT DF_tblParts1_SuggestedList DEFAULT (0),
	Note nvarchar(255) NULL,
	ShelfNo float(53) NULL CONSTRAINT DF__Temporary1__Shelf__6E01572D DEFAULT (0),
	Section nvarchar(10) NULL,
	PartLevel float(53) NULL CONSTRAINT DF__Temporary1__Level__6EF57B66 DEFAULT (0),
	HideOnReports bit NOT NULL CONSTRAINT DF__Temporary1__HideO__6FE99F9F DEFAULT (0),
	EffectiveDate smalldatetime NULL,
	SNEffective nvarchar(50) NULL,
	FinishDesc nvarchar(50) NULL,
	FinishVendorID int NULL,
	RunQuantity int NULL CONSTRAINT DF__Temporary1__RunQu__71D1E811 DEFAULT (0),
	LeadTimeDays nvarchar(50) NULL,
	DrawingNum nvarchar(50) NULL,
	RevisionNum nvarchar(50) NULL,
	RPSPNSort char(30) NULL,
	FreightLine nvarchar(50) NULL,
	PartCode nvarchar(50) NULL, 
	SecondaryProcess bit NOT NULL CONSTRAINT DF_tblParts_SecondaryProcess DEFAULT (0)
	) ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.tblParts_BACKUP ON
GO
IF EXISTS(SELECT * FROM dbo.tblParts)
	 EXEC('INSERT INTO dbo.tblParts_BACKUP(PartID, PartName, Location, VendorName, VendorPartName, ManfPartNum, RPSPartNum, CostEach, QuoteDate, VendorCost, DealerNet, SuggestedList, Note, ShelfNo, Section, PartLevel, HideOnReports, EffectiveDate, SNEffective, FinishDesc, FinishVendorID, RunQuantity, LeadTimeDays, DrawingNum, RevisionNum, RPSPNSort)
		SELECT PartID, PartName, Location, VendorName, VendorPartName, ManfPartNum, RPSPartNum, CONVERT(smallmoney, CostEach), CONVERT(smalldatetime, QuoteDate), CONVERT(smallmoney, VendorCost), CONVERT(smallmoney, DealerNet), CONVERT(smallmoney, SuggestedList), Note, ShelfNo, Section, PartLevel, HideOnReports, CONVERT(smalldatetime, EffectiveDate), SNEffective, FinishDesc, FinishVendorID, RunQuantity, LeadTimeDays, DrawingNum, RevisionNum, RPSPNSort FROM dbo.tblParts TABLOCKX')
GO
SET IDENTITY_INSERT dbo.tblParts_BACKUP OFF
GO

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblParts ADD
	USADate smalldatetime NULL
GO
COMMIT

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
CREATE TABLE dbo.tblFreightLines
	(
 	FreightLineID int NOT NULL IDENTITY (1, 1),
	FreightLineText nvarchar(50) NULL
	) ON [PRIMARY]
GO
ALTER TABLE dbo.tblFreightLines ADD CONSTRAINT
	PK_tblFreightLines PRIMARY KEY NONCLUSTERED 
	(
	FreightLineID
	) ON [PRIMARY]
GO
COMMIT


BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
CREATE TABLE dbo.tblPartCodes
	(
 	PartCodeID int NOT NULL IDENTITY (1, 1),
	PartCodeText nvarchar(50) NULL
	) ON [PRIMARY]
GO
ALTER TABLE dbo.tblPartCodes ADD CONSTRAINT
	PK_tblPartCodes PRIMARY KEY NONCLUSTERED 
	(
	PartCodeID
	) ON [PRIMARY]
GO
COMMIT

insert tblPartCodes (PartCodeText) values ('Purchased')
go

insert tblPartCodes (PartCodeText) values ('Fabricated')
go

insert tblFreightLines (FreightLineText) values ('UPS')
go

update [Switchboard Items] set ItemText = 'Add/edit/view drop down list values', Argument = 'pafdlgEditDropDowns'
where SwitchboardID = 16 and ItemNumber = 5
go

if exists (select * from sysobjects where id = object_id(N'[dbo].[paspfrmPartsModels]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[paspfrmPartsModels]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[paspfrmPartsSubParts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[paspfrmPartsSubParts]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[paspfrmPartsVendors]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[paspfrmPartsVendors]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[pasprptBillOfMaterials]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[pasprptBillOfMaterials]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[pasprptPartLabels]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[pasprptPartLabels]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE paspfrmPartsModels
	@iPartID int = 0
AS

select PartModelID, Model, Quantity, Optional
from dbo.tblPartsModels
where fkPartID = @iPartID

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[paspfrmPartsModels]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE paspfrmPartsSubParts
	@iPartID int = 0
AS

select SubPartID, SubNum, SubDescription, SubCost, SubExtCost, SubSource, SubSourcePartNum, SubQty 
from dbo.tblPartsSubParts 
where fkPartID = @iPartID
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[paspfrmPartsSubParts]  TO [fcuser]
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

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE pasprptBillOfMaterials
	@sVendorName varchar(50),
	@sModel varchar(10)
 AS
DECLARE @sSQL varchar(1500)
DECLARE @sWhere varchar(500)
SELECT @sWhere = ''
SELECT @sSQL = 
'SELECT p.RPSPartNum, p.PartName, p.VendorName, p.CostEach,  
	CONVERT (smallmoney, CONVERT (smallmoney, p.CostEach) * CONVERT (int, m.Quantity) ) AS ExtCost, p.RPSPNSort
FROM dbo.tblParts p, dbo.tblPartsModels m
WHERE p.PartID = m.fkPartID AND m.Quantity > 0 '
-- Check model
IF @sModel IS NOT NULL
	SELECT @sWhere = ' m.Model = ''' + @sModel + ''' '
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
IF @iQty IS NULL
	SELECT @iQty = 1
IF @sModel is null
	SELECT pa.RPSPartNum, pa.PartName, pa.VendorName, pa.VendorPartName, pa.ManfPartNum, nu.Counter, pa.RPSPNSort
	FROM dbo.tblParts pa, dbo.tbl_Numbers nu
	WHERE nu.Counter <= @iQty		-- allows multiple labels
		and pa.RPSPartNum Between @sStartPart And @sEndPart
else
	SELECT DISTINCT pa.RPSPartNum, pa.PartName, pa.VendorName, pa.VendorPartName, pa.ManfPartNum, nu.Counter, pa.RPSPNSort
	FROM dbo.tblParts pa, dbo.tbl_Numbers nu, dbo.tblPartsModels pm
	WHERE nu.Counter <= @iQty		-- allows multiple labels
		and pa.RPSPartNum Between @sStartPart And @sEndPart
		and pm.Model Like @sModel
	



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[pasprptPartLabels]  TO [fcuser]
GO


--insert [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
--values (31, 2, 'Tom Cat Prospects by State', 4, 'tcrptProspectListState')
--go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.32','2000-2-16 00:00:00','- Redid parts screen with new fields, layout
- Bill of Materials and Prod Parts works with HD models now.
')
go

update tblDBProperties
set PropertyValue = '3.32'
where PropertyName = 'DBStructVersion'
go
