-- fcdata update 3.45
-- - update rel notes, db version




insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.45','2000-5-2 00:00:00','- Prod Parts:  Fixed prompt for ''POID''.
- TC Warranty:  Fixed Dealer drop down to show TC dealers
- TC Warranty:  Fixed ''Dealer Ref #'' field not allowing input
- Warranty (both):  Part Cost is now pulled from Purchasing\Dealer Net
')
go

update tblDBProperties
set PropertyValue = '3.45'
where PropertyName = 'DBStructVersion'
go
