-- fcdata update 3.39
-- - update rel notes, db version


if exists (select * from sysobjects where id = object_id(N'[dbo].[pasprptDealerPartsList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[pasprptDealerPartsList]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE pasprptDealerPartsList
	@iType int = null
AS
select tP.RPSPartNum, tP.PartName, tP.USADealerNet, tP.USASuggestedList, tP.Note, tP.PartName, tP.RPSPNSort
from dbo.tblParts tP, dbo.tblModels tM, dbo.tblPartsModels tPM
where tP.PartID = tPM.fkPartID and tPM.Model = tM.Model and tM.ModelType = @iType and tP.CostEach > 0 and 
	tP.RPSPartNum not like 'V%' And tP.RPSPartNum not like 'E%' and tP.RPSPartNum not like 'H%' and tP.HideOnReports = 0
group by tP.RPSPartNum, tP.PartName, tP.USADealerNet, tP.USASuggestedList, tP.Note, tP.PartName, tP.RPSPNSort
order by tP.RPSPNSort

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[pasprptDealerPartsList]  TO [fcuser]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ppfsubOrderDetail]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ppfsubOrderDetail]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

create proc ppfsubOrderDetail
	@POPartID int = 0
as

select * 
from dbo.tblPOPartDetail 
where fkPOPartID = @POPartID
order by RequestedShipDate 

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ppfsubOrderDetail]  TO [fcuser]
GO

update tblPOPart
set VendorPartNumber = ''
where VendorPartNumber is null
go

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblPOPart ADD CONSTRAINT
	DF_tblPOPart_VendorPartNumber DEFAULT ('') FOR VendorPartNumber
GO
ALTER TABLE dbo.tblPOPart ADD CONSTRAINT
	DF_tblPOPart_RPSPartNum DEFAULT ('') FOR RPSPartNum
GO
COMMIT



insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.39','2000-3-25 00:00:00','- Fixed adding model quantities to parts when the model has a alpha character in it
- Fixed the sort order for parts with more than one char after the second number (ie, 5-123AB)
- Fixed Tom Cat Warranty dealer drop down showing Factory Cat dealers
- Fixed Tom Cat Leads dealer drop down showing Factory Cat dealers
- Dealer Parts Price listing now includes parts with 0 qtys
- The Vendor Release Schedule report now displays the Vendor Name
- Fixed Prod Parts orders not showing in order on the Prod Parts form
- Changed Factory Cat logo on Warranty RGA ticket to the text ''Tom Cat''
- Fixed parts not showing up for RGA tickets
- Fixed problem with some part orders not showing on Vendor Release Schedule
')
go

update tblDBProperties
set PropertyValue = '3.39'
where PropertyName = 'DBStructVersion'
go
