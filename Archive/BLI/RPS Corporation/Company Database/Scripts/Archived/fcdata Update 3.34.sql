-- fcdata update 3.34
-- - update rel notes, db version

if exists (select * from sysobjects where id = object_id(N'[dbo].[spGetSwitchboard]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spGetSwitchboard]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[spGetSwitchboardItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spGetSwitchboardItem]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[spSecurityCheck]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSecurityCheck]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[spSetSwitchboardHistory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spSetSwitchboardHistory]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE spGetSwitchboard
	@iSwitchboard int = 0
AS

select ItemNumber, ItemText
from dbo.[Switchboard Items]
where SwitchboardID = @iSwitchboard
order by ItemNumber
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[spGetSwitchboard]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE spGetSwitchboardItem
	@iSwitchboardID int = 0,
	@iItemNumber int = 0
AS

select ItemText, Command, Argument
from dbo.[Switchboard Items]
where SwitchboardID = @iSwitchboardID and ItemNumber = @iItemNumber


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[spGetSwitchboardItem]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE spSecurityCheck 
	@sUserID nvarchar(50),
	@iSwitchboardID int,
	@iItemNumber int
AS

SELECT AccessType 
FROM dbo.tblSecurity
WHERE UserID = @sUserID AND SwID IN (
	SELECT [ID]
	FROM dbo.[Switchboard Items]
	WHERE SwitchboardID = @iSwitchboardID AND ItemNumber = @iItemNumber )


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[spSecurityCheck]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE spSetSwitchboardHistory
	@sUser nvarchar(40) = null,
	@iSwitchboard int = 0,
	@iItemNumber int = 0
AS

set nocount on

insert tblMenuHistory (HistoryTime, HistoryUser, HistorySwitchID, HistorySwitchItem)
values (GETDATE(), @sUser, @iSwitchboard, @iItemNumber)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[spSetSwitchboardHistory]  TO [fcuser]
GO


if exists (select * from sysobjects where id = object_id(N'[dbo].[tblParts_ITrig]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [dbo].[tblParts_ITrig]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblParts_UTring]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [dbo].[tblParts_UTring]
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
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
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

-- check if we need to update sub parts
if update(CostEach)
  begin
	update a
	set a.SubCost = i.CostEach, a.SubExtCost = i.CostEach * a.SubQty
	from dbo.tblPartsSubParts a, inserted i
	where i.RPSPartNum = a.SubNum
  end
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
CREATE TABLE dbo.tblPartsFinish
	(
 	PartFinishID int NOT NULL IDENTITY (1, 1),
	FinishPartID int NOT NULL,
	FinishOrder smallint NULL CONSTRAINT DF_tblPartsFinish_FinishOrder DEFAULT (1),
	FinishVendorID int NULL,
	FinishDescription nvarchar(255) NULL,
	FinishReadyToUse bit NOT NULL CONSTRAINT DF_tblPartsFinish_FinishReadyToUse DEFAULT (0),
	FinishPackaging ntext NULL
	) ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.tblPartsFinish ADD CONSTRAINT
	PK_tblPartsFinish PRIMARY KEY NONCLUSTERED 
	(
	PartFinishID
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblPartsFinish_FinishPartID ON dbo.tblPartsFinish
	(
	FinishPartID
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblPartsFinish_FinishVendorID ON dbo.tblPartsFinish
	(
	FinishVendorID
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblPartsFinish_FinishOrder ON dbo.tblPartsFinish
	(
	FinishOrder
	) ON [PRIMARY]
GO
COMMIT


insert tblPartsFinish (FinishPartID, FinishOrder, FinishVendorID, FinishDescription)
select PartID, 1, FinishVendorID, FinishDesc
from tblParts
where FinishVendorID is not null or FinishDesc is not null
go

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
EXECUTE sp_rename 'dbo.tblPartsFinish.PartFinishID', 'Tmp_FinishID', 'COLUMN'
GO
EXECUTE sp_rename 'dbo.tblPartsFinish.FinishPartID', 'Tmp_PartID_1', 'COLUMN'
GO
EXECUTE sp_rename 'dbo.tblPartsFinish.FinishOrder', 'Tmp_FOrder_2', 'COLUMN'
GO
EXECUTE sp_rename 'dbo.tblPartsFinish.FinishVendorID', 'Tmp_VendorID_3', 'COLUMN'
GO
EXECUTE sp_rename 'dbo.tblPartsFinish.FinishDescription', 'Tmp_FDescription_4', 'COLUMN'
GO
EXECUTE sp_rename 'dbo.tblPartsFinish.FinishReadyToUse', 'Tmp_FReadyToUse_5', 'COLUMN'
GO
EXECUTE sp_rename 'dbo.tblPartsFinish.FinishPackaging', 'Tmp_FPackaging_6', 'COLUMN'
GO
EXECUTE sp_rename 'dbo.tblPartsFinish.Tmp_FinishID', 'FinishID', 'COLUMN'
GO
EXECUTE sp_rename 'dbo.tblPartsFinish.Tmp_PartID_1', 'PartID', 'COLUMN'
GO
EXECUTE sp_rename 'dbo.tblPartsFinish.Tmp_FOrder_2', 'FOrder', 'COLUMN'
GO
EXECUTE sp_rename 'dbo.tblPartsFinish.Tmp_VendorID_3', 'VendorID', 'COLUMN'
GO
EXECUTE sp_rename 'dbo.tblPartsFinish.Tmp_FDescription_4', 'FDescription', 'COLUMN'
GO
EXECUTE sp_rename 'dbo.tblPartsFinish.Tmp_FReadyToUse_5', 'FReadyToUse', 'COLUMN'
GO
EXECUTE sp_rename 'dbo.tblPartsFinish.Tmp_FPackaging_6', 'FPackaging', 'COLUMN'
GO
COMMIT

if exists (select * from sysobjects where id = object_id(N'[dbo].[pasprptDealerPartsList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[pasprptDealerPartsList]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[pasprptListPrices]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[pasprptListPrices]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[paqrptListPricesView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[paqrptListPricesView]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE pasprptDealerPartsList
	@iType int = null
AS
select tP.RPSPartNum, tP.PartName, tP.DealerNet, tP.SuggestedList, tP.Note, tP.PartName, tP.RPSPNSort
from dbo.tblParts tP, dbo.tblModels tM, dbo.tblPartsModels tPM
where tP.PartID = tPM.fkPartID and tPM.Model = tM.Model and tM.ModelType = @iType and tP.CostEach > 0 and 
	tP.RPSPartNum not like 'V%' And tP.RPSPartNum not like 'E%' and tP.RPSPartNum not like 'H%' and tP.HideOnReports = 0
	and  (tPM.Quantity > 0 or tPM.Optional > 0)
group by tP.RPSPartNum, tP.PartName, tP.DealerNet, tP.SuggestedList, tP.Note, tP.PartName, tP.RPSPNSort
order by tP.RPSPNSort


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[pasprptDealerPartsList]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE pasprptListPrices
	@iType int = 0
AS

select tP.RPSPartNum, tP.PartName, tP.SuggestedList, tP.Note, tP.RPSPNSort
from dbo.tblParts tP, dbo.tblModels tM, dbo.tblPartsModels tPM
where tP.PartID = tPM.fkPartID and tPM.Model = tM.Model and tM.ModelType = @iType and tP.CostEach > 0 and 
	tP.RPSPartNum not like 'V%' And tP.RPSPartNum not like 'E%' and tP.RPSPartNum not like 'H%' and tP.HideOnReports = 0
	and (tPM.Quantity > 0 or tPM.Optional > 0)
group by tP.RPSPartNum, tP.PartName, tP.SuggestedList, tP.Note, tP.RPSPNSort
order by tP.RPSPNSort


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[pasprptListPrices]  TO [fcuser]
GO

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
	Location ntext NULL,
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
	 TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblParts ON
GO
IF EXISTS(SELECT * FROM dbo.tblParts)
	 EXEC('INSERT INTO dbo.Tmp_tblParts(PartID, PartName, Location, VendorName, VendorPartName, ManfPartNum, RPSPartNum, CostEach, QuoteDate, VendorCost, DealerNet, SuggestedList, Note, ShelfNo, Section, PartLevel, HideOnReports, EffectiveDate, SNEffective, FinishDesc, FinishVendorID, RunQuantity, LeadTimeDays, DrawingNum, RevisionNum, RPSPNSort, FreightLine, PartCode, USADate, USADealerNet, USASuggestedList, SecondaryProcess)
		SELECT PartID, PartName, CONVERT(ntext, Location), VendorName, VendorPartName, ManfPartNum, RPSPartNum, CostEach, QuoteDate, VendorCost, DealerNet, SuggestedList, Note, ShelfNo, Section, PartLevel, HideOnReports, EffectiveDate, SNEffective, FinishDesc, FinishVendorID, RunQuantity, LeadTimeDays, DrawingNum, RevisionNum, RPSPNSort, FreightLine, PartCode, USADate, USADealerNet, USASuggestedList, SecondaryProcess FROM dbo.tblParts TABLOCKX')
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
-- check if we need to update sub parts
if update(CostEach)
  begin
	update a
	set a.SubCost = i.CostEach, a.SubExtCost = i.CostEach * a.SubQty
	from dbo.tblPartsSubParts a, inserted i
	where i.RPSPartNum = a.SubNum
  end
GO
COMMIT

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblProspects
	DROP CONSTRAINT DF_tblProspects_Active
GO
CREATE TABLE dbo.Tmp_tblProspects
	(
 	ProspectID int NOT NULL IDENTITY (1, 1),
	CompanyName nvarchar(100) NULL,
	FirstName nvarchar(50) NULL,
	LastName nvarchar(50) NULL,
	Title nvarchar(50) NULL,
	StreetAddress nvarchar(100) NULL,
	StreetZipCode nvarchar(10) NULL,
	POBox nvarchar(20) NULL,
	POBoxZipCode nvarchar(10) NULL,
	City nvarchar(50) NULL,
	State char(20) NULL,
	Phone nvarchar(30) NULL,
	Fax nvarchar(30) NULL,
	Principle nvarchar(50) NULL,
	SalesMgr nvarchar(50) NULL,
	Other nvarchar(50) NULL,
	QualityLevel nvarchar(10) NULL,
	InterestLevel nvarchar(50) NULL,
	ScrubbersSold int NULL,
	Territory nvarchar(255) NULL,
	EquipmentCarried ntext NULL,
	Notes ntext NULL,
	RecordUpdateDate smalldatetime NULL,
	Active bit NOT NULL CONSTRAINT DF_tblProspects_Active DEFAULT (1),
	ActiveBy nvarchar(100) NULL,
	ActiveDate smalldatetime NULL
	) ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblProspects ON
GO
IF EXISTS(SELECT * FROM dbo.tblProspects)
	 EXEC('INSERT INTO dbo.Tmp_tblProspects(ProspectID, CompanyName, FirstName, LastName, StreetAddress, StreetZipCode, POBox, POBoxZipCode, City, State, Phone, Fax, Principle, SalesMgr, Other, QualityLevel, InterestLevel, ScrubbersSold, Territory, EquipmentCarried, Notes, RecordUpdateDate, Active, ActiveBy, ActiveDate)
		SELECT ProspectID, CompanyName, FirstName, LastName, StreetAddress, StreetZipCode, POBox, POBoxZipCode, City, State, Phone, Fax, Principle, SalesMgr, Other, QualityLevel, InterestLevel, ScrubbersSold, Territory, EquipmentCarried, Notes, RecordUpdateDate, Active, ActiveBy, ActiveDate FROM dbo.tblProspects TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblProspects OFF
GO
DROP TABLE dbo.tblProspects
GO
EXECUTE sp_rename 'dbo.Tmp_tblProspects', 'tblProspects'
GO
ALTER TABLE dbo.tblProspects ADD CONSTRAINT
	PK_tblProspects PRIMARY KEY NONCLUSTERED 
	(
	ProspectID
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblProspects_CompanyName ON dbo.tblProspects
	(
	CompanyName
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblProspects_State ON dbo.tblProspects
	(
	State
	) ON [PRIMARY]
GO
COMMIT

update [Switchboard Items] 
set ItemNumber = ItemNumber + 1
where ItemNumber > 2 and SwitchboardID = 14
go

insert [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (14, 3, 'Customer Mailling Labels by Date', 4, 'orrptCustomerMailLabels')
go

update [Switchboard Items] 
set ItemNumber = ItemNumber + 1
where ItemNumber > 2 and SwitchboardID = 33
go

insert [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (33, 3, 'Customer Mailling Labels by Date', 4, 'ortrptCustomerMailLabels')
go

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptCustomerMailLabels]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptCustomerMailLabels]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE orsprptCustomerMailLabels
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0

SELECT [Name] AS CName, Address, City, State, Zip, ContactName
FROM tblAllOrders
WHERE [Name] IS NOT NULL AND City IS NOT NULL and State is not null AND OrderType = @iOrderType 
	AND OrderDate between @sFromDAte and @sToDAte
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptCustomerMailLabels]  TO [fcuser]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptAcknowledgement]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptAcknowledgement]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptCustomerLabels]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptCustomerLabels]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptCustomerMailLabels]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptCustomerMailLabels]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptDealerSales]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptDealerSales]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptDealerSerial]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptDealerSerial]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptEndUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptEndUser]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptEndUserDealer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptEndUserDealer]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptEndUserState]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptEndUserState]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptMajorAccountSales]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptMajorAccountSales]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptMarginByModel]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptMarginByModel]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptOpenOrders]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptOpenOrders]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptPrepSheet]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptPrepSheet]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptProdSchedule]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptProdSchedule]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptSalesDealer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptSalesDealer]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptSalesModel]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptSalesModel]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptSalesModelON]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptSalesModelON]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptServicingDealers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptServicingDealers]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptUnregSweepers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptUnregSweepers]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptAcknowledgement
	@iOrderID varchar(40) = null,
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(2) = 0
AS
-- Check input parameters
IF @iOrderID IS NULL
	SELECT @iOrderID = '%'
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
-- Run query
SELECT ao.OrderID, ao.Dealer, ao.OrderDate, ao.PurchaseOrder, ao.ShipName, ao.StreetAddress, ao.CityStateZip, 
	ao.Quantity, ao.Model, md.Description, md.Price, ao.Options, ao.PromisedDate, ao.ShipVia, ao.OrderNumber, 
	ao.CollectPrepaid, ao.Terms
FROM dbo.tblAllOrders ao, dbo.tblModels md
WHERE ao.Model = md.Model AND OrderType = @iOrderType
	AND (ao.OrderDate Between @sFromDate And @sToDate)
	AND ao.OrderID like @iOrderID
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptAcknowledgement]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptCustomerLabels
	@iOrderType varchar(3) = 0
AS
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
SELECT [Name] AS CName, Address, City, State, Zip, ContactName
FROM tblAllOrders
WHERE [Name] IS NOT NULL AND City IS NOT NULL AND OrderType = @iOrderType
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptCustomerLabels]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE orsprptCustomerMailLabels
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0

SELECT [Name] AS CName, Address, City, State, Zip, ContactName
FROM tblAllOrders
WHERE [Name] IS NOT NULL AND City IS NOT NULL and State is not null AND OrderType = @iOrderType 
	AND OrderDate between @sFromDAte and @sToDAte
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptCustomerMailLabels]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptDealerSales 
	@sDealerName varchar(40) = null,
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @sDealerName IS NULL
	SELECT @sDealerName = '%'
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
-- Run query
SELECT DISTINCT ao.Dealer, ao.Model, ao.OrderDate, ao.ShippedDate, ao.Quantity, ao.SalePrice, ao.OrderNumber, ao.SerialNumber
FROM tblAllOrders ao INNER JOIN tblAllDealers dl ON ao.Dealer = dl.DealerName
WHERE (dl.CurrentDealer = 1) AND SalePrice <> 0 AND ao.OrderType = @iOrderType
	AND (ao.Dealer like @sDealerName)
	AND (ao.OrderDate Between @sFromDate And @sToDate)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptDealerSales]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptDealerSerial
	@sDealerName varchar(40) = null,
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @sDealerName IS NULL
	SELECT @sDealerName = '%'
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT ao.Dealer, ao.Model, ao.SerialNumber, ao.Name, ao.SaleDate, ao.Quantity, ao.SalePrice, ao.ShippedDate
FROM tblAllOrders ao
WHERE ao.OrderType = @iOrderType
	AND (ao.Dealer like @sDealerName)
	AND (ao.ShippedDate Between @sFromDate and @sToDate)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptDealerSerial]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptEndUser
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT PartialList,[Name], ContactName, Address, City, State, Zip, SaleDate, TypeOfBusiness, Phone, SICCode 
FROM tblAllOrders 
WHERE PartialList='X'
	AND OrderType = @iOrderType
ORDER BY [Name];
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptEndUser]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptEndUserDealer
-- also used by orrptEndUserLabels
	@sDealer varchar(50) = null,
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @sDealer IS NULL
	SELECT @sDealer = '%'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT Dealer, Model, [Name], SaleDate, Address, City, State, Zip, ContactName, Phone, [city] + ', ' + [state] + '  ' + [zip] AS AddressLine
FROM tblAllOrders 
WHERE ([Name] IS NOT NULL) 
	AND OrderType = @iOrderType
	AND Dealer like @sDealer
ORDER BY [Name], SaleDate DESC
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptEndUserDealer]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptEndUserState
	@sState varchar(50) = null,
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @sState IS NULL
	SELECT @sState = '%'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT Model, SaleDate, [Name], Address, City, State, Zip, ContactName, Phone
FROM tblAllOrders
WHERE (State like @sState)
	AND OrderType = @iOrderType
ORDER BY SaleDate DESC
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptEndUserState]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptMajorAccountSales
	@sMAName varchar(40) = null,
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @sMAName IS NULL
	SELECT @sMAName = '%'
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT ma.MACompName, ma.MAHeadqAddress, ma.MACity, ma.MAState, 
    ma.MAZip, ao.SaleDate, ao.[Name], ao.Address, ao.City, ao.State, ao.Zip, 
    ao.Dealer, ao.Model, ao.SalePrice, ao.OrderDate
FROM dbo.tblAllMajorAccounts ma INNER JOIN dbo.tblAllOrders ao ON 
    ma.MajorAccountID = ao.MajorAccountID
WHERE ao.OrderType = @iOrderType
	AND ao.OrderDate Between @sFromDate And @sToDate
	AND ma.MACompName like @sMAName

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptMajorAccountSales]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptMarginByModel
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT ShippedDate, Dealer, Model, OrderNumber, Quantity, SalePrice, CostPrice, Margin, SerialNumber 
FROM dbo.tblAllOrders 
WHERE (SalePrice <> 0) 
	AND (OrderType = @iOrderType)
	AND (ShippedDate Between @sFromDate and @sToDate)


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptMarginByModel]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptOpenOrders
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT  ShippedDate, Model, Dealer, OrderNumber, Quantity, SalePrice, CostPrice, Margin
FROM dbo.tblAllOrders
WHERE (ShippedDate Is Null)
	AND (OrderType = @iOrderType)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptOpenOrders]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptPrepSheet 
	@sModel varchar(40) = null,
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @sModel IS NULL
	SELECT @sModel = '%'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
-- Run query
SELECT Model, PromisedDate, OrderNumber, Dealer, Battery, AmpCharger, ShipVia, Notes, [Size], ShippedDate
FROM tblAllOrders
WHERE (Model<>0) 
	AND (ShippedDate Is Null)
	AND Model like @sModel
	AND OrderType = @iOrderType
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptPrepSheet]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptProdSchedule
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT  PromisedDate, ShipVia, Dealer, Quantity, OrderNumber, Model, Notes, ShippedDate
FROM tblAllOrders
WHERE (ShippedDate Is Null)
	AND OrderType = @iOrderType
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptProdSchedule]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptSalesDealer
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT Dealer, Model, OrderNumber, ShippedDate, Quantity, SalePrice, CostPrice, Margin, OrderDate 
FROM dbo.tblAllOrders 
WHERE (SalePrice <> 0) 
	AND (OrderType = @iOrderType)
	AND (OrderDate Between @sFromDate and @sToDate)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptSalesDealer]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE dbo.orsprptSalesModel
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS 
-- Check input parameters
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT Model, Dealer, Sum(Quantity) AS Quantity, Sum(SalePrice) AS SalePrice, Sum(CostPrice) AS CostPrice, Sum(Margin) AS Margin
FROM tblAllOrders
WHERE SalePrice <> 0
	AND (OrderType = @iOrderType)
	AND (OrderDate Between @sFromDate and @sToDate)
GROUP BY Model, Dealer
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptSalesModel]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE dbo.orsprptSalesModelON
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS 
-- Check input parameters
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT Model, Dealer, OrderNumber, SalePrice, CostPrice, Margin
FROM tblAllOrders
WHERE SalePrice <> 0
	AND (OrderType = @iOrderType)
	AND (OrderDate Between @sFromDate and @sToDate)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptSalesModelON]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptServicingDealers
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT ao.Model, ao.SerialNumber, ao.[Name], ao.Phone, ao.City, ao.State, ao.SaleDate, ao.Dealer, dl.DealerName 
FROM dbo.tblAllOrders ao INNER JOIN dbo.tblAllDealers dl ON ao.fkServDealerID = dl.DealerID
WHERE OrderType = @iOrderType

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptServicingDealers]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptUnregSweepers
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT DISTINCT ao.Dealer, ao.SerialNumber, ao.PurchaseOrder, ao.OrderDate, ao.ShippedDate, ao.[Name], ao.Model 
FROM dbo.tblAllDealers dl RIGHT OUTER JOIN dbo.tblAllOrders ao ON dl.DealerName = ao.Dealer 
WHERE (dl.CurrentDealer = 1) 
	AND (ao.ShippedDate IS NOT NULL) 
	AND (ao.[Name] IS NULL)
	AND OrderType = @iOrderType

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptUnregSweepers]  TO [fcuser]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[paspfrmPartsFinish]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[paspfrmPartsFinish]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE paspfrmPartsFinish
	@iPartID int = 0
AS
select pf.FinishID, pf.FOrder, ve.VendorName, pf.FDescription, pf.FReadyToUse
from dbo.tblPartsFinish pf left outer join dbo.tblVendors ve on pf.VendorID = ve.VendorID
where pf.PartID = @iPartID 
order by pf.FOrder
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[paspfrmPartsFinish]  TO [fcuser]
GO



insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.34','2000-3-11 00:00:00','- Significantly improved performance of Switchboard pages
- Updated Parts ''List Prices'' report to allow Tom Cat/Factory Cat choice.
- Fixed ''List Prices'' and ''Dealer Parts Price List'' reports to include options parts
- Fixed ''List Prices'' and ''Dealer Parts Price List'' to not include parts with a cost each of 0
- Changed default terms in Orders and TC Orders to ''2%/15 Net 45''
- Added ''Vendor Info'' button below vendor list in Parts
- Increased ''Internal Notes'' field length
- Fixed ''Servicing Dealer'' drop down in Orders and TC Orders
- Fixed Tom Cat Orders Acknowledgement report
- Added Title field to Tom Cat Prospects
- ''Last Updated Date'' in Prospects is updated any time History items are added/edited/deleted
- Added Customer Mailling Labels by Date to Orders and TC Orders
- Fixed Tom Cat Orders Margin and Sales reports problems
- Parts that are listed as sub parts update the sub part cost each when their cost each is updated
- The Finishing tab in Parts has been redesigned
')
go

update tblDBProperties
set PropertyValue = '3.34'
where PropertyName = 'DBStructVersion'
go
