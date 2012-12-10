-- rfcdata structure updates

delete Tbl_Catalog
from Tbl_Catalog tcat
	LEFT OUTER JOIN Tbl_Catalog_Headings cath ON tcat.afkCatalogHeading = cath.apkHeading
where cath.apkHeading is null
go


update Tbl_PO
set SoldName = 'JOCKEY INTERNATIONAL', SoldID = 6
where SoldName = 'JOCKEY  INT''L,  INC.'
go

update Tbl_PO
set SoldName = 'JOHNSON OUTDOORS', SoldID = 11
where SoldName = 'JOHNSON OUTDOORS, INC.'
go

update Tbl_PO
set SoldName = 'THINGS REMEMBERED INC.', SoldID = 7
where SoldName = 'THINGS REMEMBERED'
go

update Tbl_PO
set SoldName = 'THINGS REMEMBERED INC.', SoldID = 7
where SoldName = 'THINGS REMEMBERED INCORPORATED'
go


update Tbl_Invoice
set Sold_Name = 'JOCKEY INTERNATIONAL'
where Sold_Name = 'JOCKEY  INT''L,  INC.'
go

update Tbl_Invoice
set Sold_Name = 'JOHNSON OUTDOORS'
where Sold_Name = 'JOHNSON OUTDOORS, INC.'
go

update Tbl_Invoice
set Sold_Name = 'THINGS REMEMBERED INC.'
where Sold_Name = 'THINGS REMEMBERED'
go

update Tbl_Invoice
set Sold_Name = 'THINGS REMEMBERED INC.'
where Sold_Name = 'THINGS REMEMBERED INCORPORATED'
go

insert ztblReleaseNotes (Date, MajorVersion, MinorVersion, Notes)
values ('11/7/2000', 3.15, 0, '- Fixed Catalog items not actually deleting when you deleting a catalog heading
- Updated Sold addresses to new consolidated addresses
')
go

