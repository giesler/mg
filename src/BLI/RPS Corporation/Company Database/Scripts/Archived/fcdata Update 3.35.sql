-- fcdata update 3.35
-- - update rel notes, db version


if exists (select * from sysobjects where id = object_id(N'[dbo].[pasprptDealerPartsList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[pasprptDealerPartsList]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[pasprptListPrices]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[pasprptListPrices]
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
	and  (tPM.Quantity > 0 or tPM.Optional > 0)
group by tP.RPSPartNum, tP.PartName, tP.USADealerNet, tP.USASuggestedList, tP.Note, tP.PartName, tP.RPSPNSort
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
select tP.RPSPartNum, tP.PartName, tP.USASuggestedList, tP.Note, tP.RPSPNSort
from dbo.tblParts tP, dbo.tblModels tM, dbo.tblPartsModels tPM
where tP.PartID = tPM.fkPartID and tPM.Model = tM.Model and tM.ModelType = @iType and tP.CostEach > 0 and 
	tP.RPSPartNum not like 'V%' And tP.RPSPartNum not like 'E%' and tP.RPSPartNum not like 'H%' and tP.HideOnReports = 0
	and (tPM.Quantity > 0 or tPM.Optional > 0)
group by tP.RPSPartNum, tP.PartName, tP.USASuggestedList, tP.Note, tP.RPSPNSort
order by tP.RPSPNSort

GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[pasprptListPrices]  TO [fcuser]
GO

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.35','2000-3-11 00:00:00','- Servicing Dealers drop down fixed
- Dealer Net and Sug List can be entered in Parts
- Part card printout is updated with new fields and form layout
- List Prices and Dealer Parts List reports now use the data from the fields on the Sales page in Parts
')
go

update tblDBProperties
set PropertyValue = '3.35'
where PropertyName = 'DBStructVersion'
go
