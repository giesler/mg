-- fcdata update 3.38
-- - update rel notes, db version


update dbo.tblWarrantyParts
set PartNumReplaced = SUBSTRING(PartNumReplaced,4,len(PartNumReplaced) - 3)
where SUBSTRING(PartNumReplaced,1,3) = 'RPS'
go

update dbo.tblWarrantyParts
set PartNumReplaced = '8-574'
where PartNumReplaced = '-8-574'
go


update wp
set PartFileIndex = PartID
from dbo.tblWarrantyParts wp, dbo.tblParts pa
where wp.PartNumReplaced = pa.RPSPartNum and wp.PartFileIndex <> pa.PartID
go


delete from dbo.tblWarrantyParts where PartNumReplaced is null
go


delete dbo.tblWarrantyParts
from dbo.tblWarrantyParts w left outer join dbo.tblParts p on w.PartNumReplaced = p.RPSPartNum
where p.RPSPartNum is null
go

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblWarrantyParts
	DROP CONSTRAINT FK_tblWarrantyParts_tblWarranty
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblWarrantyParts
	DROP CONSTRAINT DF__Temporary__fkWar__395884C4
GO
ALTER TABLE dbo.tblWarrantyParts
	DROP CONSTRAINT DF__Temporary__PartC__3A4CA8FD
GO
ALTER TABLE dbo.tblWarrantyParts
	DROP CONSTRAINT DF__Temporary__PartF__3B40CD36
GO
CREATE TABLE dbo.Tmp_tblWarrantyParts
	(
 	WarrantyPartID int NOT NULL IDENTITY (1, 1),
	fkWarrantyID int NULL CONSTRAINT DF__Temporary__fkWar__395884C4 DEFAULT (0),
	PartCost money NULL CONSTRAINT DF__Temporary__PartC__3A4CA8FD DEFAULT (0),
	PartID int NULL CONSTRAINT DF__Temporary__PartF__3B40CD36 DEFAULT (0)
	) ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblWarrantyParts ON
GO
IF EXISTS(SELECT * FROM dbo.tblWarrantyParts)
	 EXEC('INSERT INTO dbo.Tmp_tblWarrantyParts(WarrantyPartID, fkWarrantyID, PartCost, PartID)
		SELECT WarrantyPartID, fkWarrantyID, PartCost, PartFileIndex FROM dbo.tblWarrantyParts TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblWarrantyParts OFF
GO
DROP TABLE dbo.tblWarrantyParts
GO
EXECUTE sp_rename 'dbo.Tmp_tblWarrantyParts', 'tblWarrantyParts'
GO
ALTER TABLE dbo.tblWarrantyParts ADD CONSTRAINT
	aaaaatblWarrantyParts_PK PRIMARY KEY NONCLUSTERED 
	(
	WarrantyPartID
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX fkWarrantyID ON dbo.tblWarrantyParts
	(
	fkWarrantyID
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblWarrantyParts_PartID ON dbo.tblWarrantyParts
	(
	PartID
	) ON [PRIMARY]
GO
ALTER TABLE dbo.tblWarrantyParts WITH NOCHECK ADD CONSTRAINT
	FK_tblWarrantyParts_tblWarranty FOREIGN KEY
	(
	fkWarrantyID
	) REFERENCES dbo.tblAllWarranty
	(
	WarrantyID
	)
GO
COMMIT

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
EXECUTE sp_rename 'dbo.tblWarrantyParts.fkWarrantyID', 'Tmp_WarrantyID', 'COLUMN'
GO
EXECUTE sp_rename 'dbo.tblWarrantyParts.Tmp_WarrantyID', 'WarrantyID', 'COLUMN'
GO
COMMIT

if exists (select * from sysobjects where id = object_id(N'[dbo].[paspfrmPartsFinish]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[paspfrmPartsFinish]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[waspfrmWarrantyParts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[waspfrmWarrantyParts]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE paspfrmPartsFinish
	@iPartID int = 0
AS
select pf.FinishID, pf.FOrder, ve.VendorName, pf.FDescription, pf.FReadyToUse, pf.FCost, pf.FPackaging
from dbo.tblPartsFinish pf left outer join dbo.tblVendors ve on pf.VendorID = ve.VendorID
where pf.PartID = @iPartID 
order by pf.FOrder

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[paspfrmPartsFinish]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE waspfrmWarrantyParts
	@WarrantyID int = 0
as

select wa.WarrantyPartID, pa.RPSPartNum, pa.PartName, wa.PartCost
from dbo.tblWarrantyParts wa, dbo.tblParts pa
where wa.PartID = pa.PartID and wa.WarrantyID = @WarrantyID
order by pa.RPSPNSort
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[waspfrmWarrantyParts]  TO [fcuser]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblParts_ITrig]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [dbo].[tblParts_ITrig]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblParts_UTring]') and OBJECTPROPERTY(id, N'IsTrigger') = 1)
drop trigger [dbo].[tblParts_UTring]
GO

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.38','2000-3-16 00:00:00','- Fixed Warranty Parts not displaying
- Fixed Sub Parts not being displayed when first added
- After entering a sub part number, the description, source, source PN, and cost are automatcially filled in
- Added a ''Finish Page'' report to printout from Parts
')
go

update tblDBProperties
set PropertyValue = '3.38'
where PropertyName = 'DBStructVersion'
go
