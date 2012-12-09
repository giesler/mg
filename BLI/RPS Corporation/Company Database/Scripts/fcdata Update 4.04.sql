-- fcdata update 4.04

ALTER TABLE dbo.tblVendors ADD
	FabricatedParts bit NULL
GO
ALTER TABLE dbo.tblVendors ADD CONSTRAINT
	DF_tblVendors_FabricatedParts DEFAULT 0 FOR FabricatedParts
GO
ALTER TABLE dbo.tblVendors
	DROP COLUMN upsize_ts
GO

update tblVendors
set FabricatedParts = 0
go

update tblVendors
set FabricatedParts = 1
where VendorID = 88 or VendorID = 141 or VendorID = 199
go

drop table tblParts_Backup
go

ALTER TABLE dbo.tblDBProperties ADD CONSTRAINT
	PK_tblDBProperties PRIMARY KEY CLUSTERED 
	(
	PropertyName
	) ON [PRIMARY]

GO

CREATE TABLE dbo.Tmp_tblDBProperties
	(
	PropertyName nvarchar(59) NOT NULL,
	PropertyValue nvarchar(20) NULL
	)  ON [PRIMARY]
GO
IF EXISTS(SELECT * FROM dbo.tblDBProperties)
	 EXEC('INSERT INTO dbo.Tmp_tblDBProperties (PropertyName, PropertyValue)
		SELECT PropertyName, PropertyValue FROM dbo.tblDBProperties TABLOCKX')
GO
DROP TABLE dbo.tblDBProperties
GO
EXECUTE sp_rename N'dbo.Tmp_tblDBProperties', N'tblDBProperties', 'OBJECT'
GO
ALTER TABLE dbo.tblDBProperties ADD CONSTRAINT
	PK_tblDBProperties PRIMARY KEY CLUSTERED 
	(
	PropertyName
	) ON [PRIMARY]

GO


insert tblDBProperties (PropertyName, PropertyValue)
values ('Parts.DNetMult', 1.85)
go

insert tblDBProperties (PropertyName, PropertyValue)
values ('Parts.DNetMultFab', 2.2)
go

insert tblDBProperties (PropertyName, PropertyValue)
values ('Parts.DNetMin', 2.95)
go

insert tblDBProperties (PropertyName, PropertyValue)
values ('Parts.SugListMult', 1.6666)
go



insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('4.04','2000-9-18 00:00:00','- Vendors: Added ''Fabricated Parts'' check box for calculating prices in Parts
- Parts: Updated ''Parts Pricing'' form to allow changing of multipliers and min Dealer Net
')
go

update tblDBProperties
set PropertyValue = '4.04'
where PropertyName = 'DBStructVersion'
go
