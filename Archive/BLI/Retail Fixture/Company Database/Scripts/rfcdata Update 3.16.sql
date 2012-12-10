-- rfcdata structure updates

update Tbl_PO
set SoldName = 'JOHNSON OUTDOORS, INC.', SoldID = 11
where SoldName = 'JOHNSON OUTDOORS'
go

update Tbl_Invoice
set Sold_Name = 'JOHNSON OUTDOORS, INC.'
where Sold_Name = 'JOHNSON OUTDOORS'
go

insert ztblReleaseNotes (Date, MajorVersion, MinorVersion, Notes)
values ('11/12/2000', 3.16, 0, '- Updated ''Johnson Outdoors'' sold tos to ''Johnson Outdoors, Inc.''.
')
go

