-- fcdata update 3.29
-- - update rel notes, db version
-- - change dealer table


BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblModels
	DROP CONSTRAINT DF__Temporary__Model__5441852A
GO
ALTER TABLE dbo.tblModels
	DROP CONSTRAINT DF__Temporary__Price__5535A963
GO
CREATE TABLE dbo.Tmp_tblModels
	(
 	ModelID int NOT NULL IDENTITY (1, 1),
	Model nvarchar(20) NULL CONSTRAINT DF__Temporary__Model__5441852A DEFAULT (''),
	Description ntext NULL,
	Price money NULL CONSTRAINT DF__Temporary__Price__5535A963 DEFAULT (0),
	ModelType tinyint NOT NULL CONSTRAINT DF_tblModels_ModelType DEFAULT (0)
	) ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblModels ON
GO
IF EXISTS(SELECT * FROM dbo.tblModels)
	 EXEC('INSERT INTO dbo.Tmp_tblModels(ModelID, Model, Description, Price)
		SELECT ModelID, Model, Description, Price FROM dbo.tblModels TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblModels OFF
GO
DROP TABLE dbo.tblModels
GO
EXECUTE sp_rename 'dbo.Tmp_tblModels', 'tblModels'
GO
ALTER TABLE dbo.tblModels ADD CONSTRAINT
	aaaaatblModels_PK PRIMARY KEY NONCLUSTERED 
	(
	ModelID
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX ModelID ON dbo.tblModels
	(
	ModelID
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX ModelType ON dbo.tblModels
	(
	ModelType
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
COMMIT


update [Switchboard Items]
set ItemNumber = ItemNumber + 1
where SwitchboardID = 17 and (ItemNumber > 10)
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (17, 11, 'Production Parts listing by Model (21 Series)', 4, 'parptProdPartsbyModel21')
go

if exists (select * from sysobjects where id = object_id(N'[dbo].[pasprptRPSPartsPricing]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[pasprptRPSPartsPricing]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE pasprptRPSPartsPricing 
	@iType int = 0
AS

select tP.RPSPartNum, tP.VendorName, tP.ManfPartNum, tP.SuggestedList, tP.DealerNet, tP.PartName, tP.RPSPNSort
from dbo.tblParts tP, dbo.tblModels tM, dbo.tblPartsModels tPM
where tP.PartID = tPM.fkPartID and tPM.Model = tM.Model and tM.ModelType = @iType and tPM.Quantity > 0 and
	tP.RPSPartNum not like 'V%' And tP.RPSPartNum not like 'E%' and tP.RPSPartNum not like 'H%' and tP.HideOnReports = 0
group by tP.RPSPartNum, tP.VendorName, tP.ManfPartNum, tP.SuggestedList, tP.DealerNet, tP.PartName, tP.RPSPNSort
order by tP.RPSPNSort

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[pasprptRPSPartsPricing]  TO [fcuser]
GO

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.29','2000-2-11 00:00:00','- Added Production Parts by Model for 21 Series report
- Updated ''RPS Parts Pricing'' report to print a Tom Cat or Factory Cat list
- Added a ''type'' field to the Model list.
- Changed drop downs in Orders and TC Orders to only show appropriate dealers, major accounts, and models
')
go

update tblDBProperties
set PropertyValue = '3.29'
where PropertyName = 'DBStructVersion'
go
