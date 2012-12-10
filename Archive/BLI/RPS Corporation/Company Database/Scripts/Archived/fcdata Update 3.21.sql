-- fcdata update 3.21
-- - update rel notes, db version

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.21','1999-12-08 00:00:00','- Added an auto lookup feature to Production Parts that looks up the RPS Part Num and Description when a vendor part num is entered.
- Changed the model list on the Parts form to hopefully prevent future problems with it.
- Changed the ''Database Update Program'' to check for a file needed by new Parts form')
go

update tblDBProperties
set PropertyValue = '3.21'
where PropertyName = 'DBStructVersion'
go
