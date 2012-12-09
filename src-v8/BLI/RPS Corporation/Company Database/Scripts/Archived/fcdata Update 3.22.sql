-- fcdata update 3.22
-- - add col for optional part
-- - update rel notes, db version

alter table dbo.tblPartsModels
	add Optional bit NOT NULL DEFAULT 0
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.22','1999-12-08 00:00:00','- Added ''Optional'' field to Part Model quantity list.
- Fixed Prod Parts problem when adding new parts.
- Added text boxes to the toolbar in Parts, Orders, Prod Parts, and Warranty that allow you to type an ID, ie Order #, then hit enter to jump to that record.  (As oppposed to opening ''Find'' each time.
- Fixed a few problems with the autoupdate feature.')
go

update tblDBProperties
set PropertyValue = '3.22'
where PropertyName = 'DBStructVersion'
go
