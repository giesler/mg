-- rfcdata structure updates

BEGIN TRANSACTION
ALTER TABLE dbo.Tbl_PO_Items
	DROP CONSTRAINT Tbl_PO_Items_FK01
GO
DROP INDEX dbo.Tbl_PO.ID
GO
DROP INDEX dbo.Tbl_PO.ID1
GO
CREATE NONCLUSTERED INDEX ID ON dbo.Tbl_PO
	(
	ID2
	) ON [PRIMARY]
GO
CREATE UNIQUE CLUSTERED INDEX ID1 ON dbo.Tbl_PO
	(
	ID
	) ON [PRIMARY]
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.Tbl_PO_Items WITH NOCHECK ADD CONSTRAINT
	Tbl_PO_Items_FK01 FOREIGN KEY
	(
	ID
	) REFERENCES dbo.Tbl_PO
	(
	ID2
	)
GO
ALTER TABLE dbo.Tbl_PO_Items
	NOCHECK CONSTRAINT Tbl_PO_Items_FK01
GO


ALTER TABLE dbo.Tbl_Invoice
	DROP CONSTRAINT aaaaaTbl_Invoice_PK
GO
ALTER TABLE dbo.Tbl_Invoice ADD CONSTRAINT
	aaaaaTbl_Invoice_PK PRIMARY KEY CLUSTERED 
	(
	ID
	) ON [PRIMARY]

GO


insert ztblReleaseNotes (Date, MajorVersion, MinorVersion, Notes)
values ('9/20/2000', 3.01, 0, '- Added ''Recent'' buttons to Invoice and PO menus to only show invoices/POs within the last year
- ')
go

