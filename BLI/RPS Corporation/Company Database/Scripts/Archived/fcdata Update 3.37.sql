-- fcdata update 3.37
-- - update rel notes, db version

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblPartsFinish ADD
	FCost smallmoney NULL
GO
COMMIT

if exists (select * from sysobjects where id = object_id(N'[dbo].[paspfrmPartsFinish]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[paspfrmPartsFinish]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE paspfrmPartsFinish
	@iPartID int = 0
AS
select pf.FinishID, pf.FOrder, ve.VendorName, pf.FDescription, pf.FReadyToUse, pf.FCost
from dbo.tblPartsFinish pf left outer join dbo.tblVendors ve on pf.VendorID = ve.VendorID
where pf.PartID = @iPartID 
order by pf.FOrder

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[paspfrmPartsFinish]  TO [fcuser]
GO

insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (18, 3, 'Production Parts Explorer', 3, 'ppfrmProdPartsExplore')
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.37','2000-3-15 00:00:00','- Fixed sorting order of orders in Production Parts by In House Date
- Removed ''RPS'' from ''RPS Part #'' on Parts labels
- Added ''Cost'' field to Finishing in Parts
- Updated Vendor Cost or Finish Cost will update the ''Current Cost'' in Parts as well as Dealer Net and Sug List
- Added ''Current Cost'' to the Purchasing page in Parts
- Added new Production Parts screen to get feedback
')
go

update tblDBProperties
set PropertyValue = '3.37'
where PropertyName = 'DBStructVersion'
go
