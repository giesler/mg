-- fcdata update 3.36
-- - update rel notes, db version


insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.36','2000-3-11 00:00:00','- Fixed Part form printout showing Dealer names instead of Vendors
- Removed ''Factory Cat Parts'' from Parts labels in Parts\Reports
')
go

update tblDBProperties
set PropertyValue = '3.36'
where PropertyName = 'DBStructVersion'
go
