-- fcdata update 3.20
-- - update rel notes, db version

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.20','1999-12-07 00:00:00','- Fixed Production Parts form not refreshing PO and Part information
- Fixed ''Purchased Leads Import'' to work on all computers')
go

update tblDBProperties
set PropertyValue = '3.20'
where PropertyName = 'DBStructVersion'
go
