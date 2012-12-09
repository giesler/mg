-- rfcdata structure updates

-- fix bad #
update tbl_po set ship_storeno = '7973' where id = 10330
go


ALTER TABLE dbo.Tbl_PO
	DROP CONSTRAINT FK_Tbl_PO_tblSold
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.Tbl_PO
	DROP CONSTRAINT FK_Tbl_PO_tblSoldAttention
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.Tbl_PO
	DROP CONSTRAINT DF__TemporaryUps__ID__440B1D61
GO
ALTER TABLE dbo.Tbl_PO
	DROP CONSTRAINT DF__Temporary__Credi__44FF419A
GO
ALTER TABLE dbo.Tbl_PO
	DROP CONSTRAINT DF__Temporary__Label__45F365D3
GO
ALTER TABLE dbo.Tbl_PO
	DROP CONSTRAINT DF__Temporary__Prior__46E78A0C
GO
ALTER TABLE dbo.Tbl_PO
	DROP CONSTRAINT DF__Temporary__OldID__47DBAE45
GO
ALTER TABLE dbo.Tbl_PO
	DROP CONSTRAINT DF__Temporary__CANCE__48CFD27E
GO
ALTER TABLE dbo.Tbl_PO
	DROP CONSTRAINT DF_Tbl_PO_FTofTruck
GO
CREATE TABLE dbo.Tmp_Tbl_PO
	(
	ID2 int NOT NULL IDENTITY (1, 1),
	ID int NULL,
	PO nvarchar(50) NULL,
	Date_Ordered datetime NULL,
	Ship_Date datetime NULL,
	Arrive_Date datetime NULL,
	Job_Number nvarchar(20) NULL,
	Ship_via nvarchar(50) NULL,
	Ship_Name nvarchar(50) NULL,
	Ship_StoreNo nvarchar(20) NULL,
	Ship_Address1 nvarchar(60) NULL,
	Ship_Address2 nvarchar(60) NULL,
	Ship_City nvarchar(35) NULL,
	Ship_State nvarchar(20) NULL,
	Ship_Zip nvarchar(20) NULL,
	Ship_Attn nvarchar(50) NULL,
	Ship_Phone nvarchar(20) NULL,
	Comments ntext NULL,
	Status nvarchar(20) NULL,
	Salesperson nvarchar(25) NULL,
	Purchaser nvarchar(40) NULL,
	Credit bit NULL,
	Labels smallint NULL,
	pro# nvarchar(60) NULL,
	Priority nvarchar(20) NULL,
	OldID int NULL,
	Store nvarchar(50) NULL,
	SoldName nvarchar(50) NULL,
	Classification nvarchar(20) NULL,
	CANCELPO bit NULL,
	Account# nvarchar(50) NULL,
	RefNum nvarchar(30) NULL,
	ShelfType nvarchar(5) NULL,
	FTofTruck bit NULL,
	FTofTruckAmount nvarchar(10) NULL,
	SoldAttentionID int NULL,
	SoldID int NULL,
	StoreTypeID int NULL,
	BlanketPO nvarchar(50) NULL,
	SoldTo_Address1 nvarchar(60) NULL,
	SoldTo_Address2 nvarchar(60) NULL,
	SoldTo_City nvarchar(35) NULL,
	SoldTo_State nvarchar(20) NULL,
	SoldTo_Zip nvarchar(20) NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Tbl_PO ADD CONSTRAINT
	DF__TemporaryUps__ID__440B1D61 DEFAULT (0) FOR ID
GO
ALTER TABLE dbo.Tmp_Tbl_PO ADD CONSTRAINT
	DF__Temporary__Credi__44FF419A DEFAULT (0) FOR Credit
GO
ALTER TABLE dbo.Tmp_Tbl_PO ADD CONSTRAINT
	DF__Temporary__Label__45F365D3 DEFAULT (0) FOR Labels
GO
ALTER TABLE dbo.Tmp_Tbl_PO ADD CONSTRAINT
	DF__Temporary__Prior__46E78A0C DEFAULT ('Normal Order') FOR Priority
GO
ALTER TABLE dbo.Tmp_Tbl_PO ADD CONSTRAINT
	DF__Temporary__OldID__47DBAE45 DEFAULT (0) FOR OldID
GO
ALTER TABLE dbo.Tmp_Tbl_PO ADD CONSTRAINT
	DF__Temporary__CANCE__48CFD27E DEFAULT (0) FOR CANCELPO
GO
ALTER TABLE dbo.Tmp_Tbl_PO ADD CONSTRAINT
	DF_Tbl_PO_FTofTruck DEFAULT (0) FOR FTofTruck
GO
SET IDENTITY_INSERT dbo.Tmp_Tbl_PO ON
GO
IF EXISTS(SELECT * FROM dbo.Tbl_PO)
	 EXEC('INSERT INTO dbo.Tmp_Tbl_PO (ID2, ID, PO, Date_Ordered, Ship_Date, Arrive_Date, Job_Number, Ship_via, Ship_Name, Ship_StoreNo, Ship_Address1, Ship_Address2, Ship_City, Ship_State, Ship_Zip, Ship_Attn, Ship_Phone, Comments, Status, Salesperson, Purchaser, Credit, Labels, pro#, Priority, OldID, Store, SoldName, Classification, CANCELPO, Account#, RefNum, ShelfType, FTofTruck, FTofTruckAmount, SoldAttentionID, SoldID, StoreTypeID)
		SELECT ID2, ID, PO, Date_Ordered, Ship_Date, Arrive_Date, CONVERT(nvarchar(20), Job_Number), Ship_via, Ship_Name, CONVERT(nvarchar(20), Ship_StoreNo), Ship_Address1, Ship_Address2, CONVERT(nvarchar(35), Ship_City), CONVERT(nvarchar(20), Ship_State), CONVERT(nvarchar(20), Ship_Zip), Ship_Attn, CONVERT(nvarchar(20), Ship_Phone), Comments, CONVERT(nvarchar(20), Status), CONVERT(nvarchar(25), Salesperson), CONVERT(nvarchar(40), Purchaser), Credit, Labels, pro#, CONVERT(nvarchar(20), Priority), OldID, Store, SoldName, CONVERT(nvarchar(20), Classification), CANCELPO, Account#, CONVERT(nvarchar(30), RefNum), CONVERT(nvarchar(5), ShelfType), FTofTruck, CONVERT(nvarchar(10), FTofTruckAmount), SoldAttentionID, SoldID, StoreTypeID FROM dbo.Tbl_PO TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_Tbl_PO OFF
GO
ALTER TABLE dbo.Tbl_PO_Items
	DROP CONSTRAINT Tbl_PO_Items_FK01
GO
DROP TABLE dbo.Tbl_PO
GO
EXECUTE sp_rename N'dbo.Tmp_Tbl_PO', N'Tbl_PO', 'OBJECT'
GO
CREATE UNIQUE CLUSTERED INDEX ID1 ON dbo.Tbl_PO
	(
	ID
	) ON [PRIMARY]
GO
ALTER TABLE dbo.Tbl_PO ADD CONSTRAINT
	aaaaaTbl_PO_PK PRIMARY KEY NONCLUSTERED 
	(
	ID2
	) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX Arrive_Date ON dbo.Tbl_PO
	(
	Arrive_Date
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX Date_Ordered ON dbo.Tbl_PO
	(
	Date_Ordered
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX ID ON dbo.Tbl_PO
	(
	ID2
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX Job_Number ON dbo.Tbl_PO
	(
	Job_Number
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX OldID ON dbo.Tbl_PO
	(
	OldID
	) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX PO ON dbo.Tbl_PO
	(
	PO
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX RefNum ON dbo.Tbl_PO
	(
	RefNum
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX Ship_Date ON dbo.Tbl_PO
	(
	Ship_Date
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX Ship_Name ON dbo.Tbl_PO
	(
	Ship_Name
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_Tbl_PO_Ship_Store_No ON dbo.Tbl_PO
	(
	Ship_StoreNo
	) ON [PRIMARY]
GO
ALTER TABLE dbo.Tbl_PO WITH NOCHECK ADD CONSTRAINT
	FK_Tbl_PO_tblSoldAttention FOREIGN KEY
	(
	SoldAttentionID
	) REFERENCES dbo.tblSoldAttention
	(
	SoldAttentionID
	)
GO
ALTER TABLE dbo.Tbl_PO WITH NOCHECK ADD CONSTRAINT
	FK_Tbl_PO_tblSold FOREIGN KEY
	(
	SoldID
	) REFERENCES dbo.tblSold
	(
	apkSold
	)
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

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_Util_GetSoldToAddressInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_Util_GetSoldToAddressInfo]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE dbo.sp_Util_GetSoldToAddressInfo
	@SoldID int
AS
select SoldAddress1, SoldAddress2, SoldCity, SoldState, SoldZip
from tblSold
where apkSold = @SoldID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



insert ztblReleaseNotes (Date, MajorVersion, MinorVersion, Notes)
values ('8/4/2001', 3.22, 0, '- Modified database to only work on monitors with at least 800x600 screen resolution
- Added ''Blanket PO'' field to PO page
- Added ''Sold To'' address information to PO page
')
go

