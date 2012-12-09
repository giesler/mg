-- fcdata update 3.33
-- - update rel notes, db version
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblParts
	DROP CONSTRAINT DF_tblParts_DealerNet
GO
ALTER TABLE dbo.tblParts
	DROP CONSTRAINT DF_tblParts_SuggestedList
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
	DROP CONSTRAINT DF__Temporary__RunQu__71D1E811
GO
ALTER TABLE dbo.tblParts
	DROP CONSTRAINT DF_tblParts_SecondaryProcess_1
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
	PartCode nvarchar(50) NULL,
	USADate smalldatetime NULL,
	USADealerNet smallmoney NULL,
	USASuggestedList smallmoney NULL,
	SecondaryProcess bit NOT NULL CONSTRAINT DF_tblParts_SecondaryProcess_1 DEFAULT (0)
	) ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblParts ON
GO
IF EXISTS(SELECT * FROM dbo.tblParts)
	 EXEC('INSERT INTO dbo.Tmp_tblParts(PartID, PartName, Location, VendorName, VendorPartName, ManfPartNum, RPSPartNum, CostEach, QuoteDate, VendorCost, DealerNet, SuggestedList, Note, ShelfNo, Section, PartLevel, HideOnReports, EffectiveDate, SNEffective, FinishDesc, FinishVendorID, RunQuantity, LeadTimeDays, DrawingNum, RevisionNum, RPSPNSort, FreightLine, PartCode, USADate, SecondaryProcess)
		SELECT PartID, PartName, Location, VendorName, VendorPartName, ManfPartNum, RPSPartNum, CostEach, QuoteDate, VendorCost, DealerNet, SuggestedList, Note, ShelfNo, Section, PartLevel, HideOnReports, EffectiveDate, SNEffective, FinishDesc, FinishVendorID, RunQuantity, LeadTimeDays, DrawingNum, RevisionNum, RPSPNSort, FreightLine, PartCode, USADate, SecondaryProcess FROM dbo.tblParts TABLOCKX')
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

--insert [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
--values (31, 2, 'Tom Cat Prospects by State', 4, 'tcrptProspectListState')
--go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.33','2000-2-25 00:00:00','- Fixed USA Sug List and Dealer Net on Parts Sales page.
- Fixed sub parts total not updating correctly.
')
go

update tblDBProperties
set PropertyValue = '3.33'
where PropertyName = 'DBStructVersion'
go
