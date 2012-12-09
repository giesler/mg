-- fcdata update 3.23
-- - update rel notes, db version


insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.23','1999-12-10 00:00:00','- Replaced ''Find'' command on toolbar.
- Fixed ''Sub Parts'' in the Parts form.')
go

update tblDBProperties
set PropertyValue = '3.23'
where PropertyName = 'DBStructVersion'
go
