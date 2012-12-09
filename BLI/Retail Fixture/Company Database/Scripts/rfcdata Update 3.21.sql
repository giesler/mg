-- rfcdata structure updates

ALTER TABLE dbo.tblSoldAttention ADD
	Username nvarchar(50) NULL,
	Password nvarchar(50) NULL,
	Email nvarchar(50) NULL
GO


ALTER TABLE dbo.tblSold
	DROP COLUMN Username, Password, Email
GO


insert ztblReleaseNotes (Date, MajorVersion, MinorVersion, Notes)
values ('8/7/2001', 3.21, 0, '- Moved username, password, email to Sold To Attention instead of Sold To
')
go

