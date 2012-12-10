-- rfcdata structure updates


DROP INDEX dbo.Tbl_PO.PO
GO
CREATE NONCLUSTERED INDEX PO ON dbo.Tbl_PO
	(
	PO
	) ON [PRIMARY]
GO



insert ztblReleaseNotes (Date, MajorVersion, MinorVersion, Notes)
values ('1/15/2002', 3.28, 0, '- POs:  Changed so you are allowed to enter either a PO or Blanket PO
')
go

