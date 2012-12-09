-- fcdata update 3.19
-- - recreates parts and leads views
-- - update rel notes, db version

if exists (select * from sysobjects where id = object_id(N'[dbo].[paqrptPartLabels]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[paqrptPartLabels]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[paqrptPartLabelsMulti]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[paqrptPartLabelsMulti]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[paqrptPartListFinishView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[paqrptPartListFinishView]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[paqrptPartsPerModel2]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[paqrptPartsPerModel2]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblLeads]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[tblLeads]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblPurLeads]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[tblPurLeads]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblTCLeads]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[tblTCLeads]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW paqrptPartLabels
AS
SELECT tblParts.RPSPartNum, tblParts.PartName, tblParts.VendorName, tblParts.VendorPartName, tblParts.ManfPartNum, Tbl_Numbers.Counter, tblParts.Model
FROM Tbl_Numbers, tblParts


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE VIEW paqrptPartLabelsMulti
AS
SELECT dbo.tblParts.RPSPartNum, dbo.tblParts.PartName, 
    dbo.tblParts.VendorName, dbo.tblParts.VendorPartName, 
    dbo.tblParts.ManfPartNum, dbo.Tbl_Numbers.Counter
FROM dbo.Tbl_Numbers INNER JOIN
    dbo.tblParts INNER JOIN
    dbo.tmpLabelQtys ON 
    dbo.tblParts.PartID = dbo.tmpLabelQtys.PartIndex ON 
    dbo.Tbl_Numbers.Counter <= dbo.tmpLabelQtys.Quantity


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW "paqrptPartListFinishView"
AS
SELECT tblParts.RPSPartNum, tblParts.PartName, tblParts.FinishDesc
FROM tblParts
WHERE (((tblParts.FinishDesc) Is Not Null))


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE VIEW paqrptPartsPerModel2
AS
SELECT tblParts.PartID, tblParts.RPSPartNum, tblParts.PartName, tblModels.Model
FROM tblParts, tblModels


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE VIEW dbo.tblLeads
AS
SELECT tblAllLeads.*
FROM tblAllLeads
WHERE (Purchased = 0)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE VIEW dbo.tblPurLeads
AS
SELECT tblAllLeads.*
FROM tblAllLeads
WHERE (Purchased = 1)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE VIEW dbo.tblTCLeads
AS
SELECT tblAllLeads.*
FROM tblAllLeads
WHERE (Purchased = 2)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.19','1999-12-05 00:00:00','- Fixed #Name? from showing up on several label reports.
- Fixed cursor behavior in leads files when leaving the App/Notes field.')
go

update tblDBProperties
set PropertyValue = '3.19'
where PropertyName = 'DBStructVersion'
go

