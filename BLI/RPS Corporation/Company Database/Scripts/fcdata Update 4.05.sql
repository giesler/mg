-- fcdata update 4.05

update tblParts
set CostEach = 0, DealerNet = 0, SuggestedList = 0
where CostEach is null
go

ALTER TABLE dbo.tblWarrantyParts
	DROP CONSTRAINT FK_tblWarrantyParts_tblWarranty
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblWarrantyParts WITH NOCHECK ADD CONSTRAINT
	FK_tblWarrantyParts_tblWarranty FOREIGN KEY
	(
	WarrantyID
	) REFERENCES dbo.tblAllWarranty
	(
	WarrantyID
	) 
	
GO


DROP INDEX dbo.tblAllOrders.IX_tblAllOrders_OrderDate
GO
CREATE CLUSTERED INDEX IX_tblAllOrders_OrderDate ON dbo.tblAllOrders
	(
	OrderDate
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO

DROP INDEX dbo.tblParts.rpspn
GO
CREATE UNIQUE CLUSTERED INDEX rpspn ON dbo.tblParts
	(
	RPSPartNum
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
COMMIT


CREATE TABLE dbo.tblDealerPartSales
	(
	DealerSalesID int NOT NULL IDENTITY (1, 1),
	DealerID int NULL,
	SalesQuarter int NULL,
	SalesYear int NULL,
	SalesAmount money NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.tblDealerPartSales ADD CONSTRAINT
	PK_tblDealerPartSales PRIMARY KEY NONCLUSTERED 
	(
	DealerSalesID
	) ON [PRIMARY]

GO
CREATE CLUSTERED INDEX IX_tblDealerPartSales ON dbo.tblDealerPartSales
	(
	DealerID
	) ON [PRIMARY]
GO
ALTER TABLE dbo.tblDealerPartSales ADD CONSTRAINT
	IX_tblDealerPartSales_QuarterYear UNIQUE NONCLUSTERED 
	(
	SalesQuarter,
	SalesYear,
	DealerID
	) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX IX_tblDealerPartSales_Time ON dbo.tblDealerPartSales
	(
	SalesQuarter,
	SalesYear
	) ON [PRIMARY]
GO
ALTER TABLE dbo.tblDealerPartSales ADD CONSTRAINT
	FK_tblDealerPartSales_tblAllDealers FOREIGN KEY
	(
	DealerID
	) REFERENCES dbo.tblAllDealers
	(
	DealerID
	) 
	
GO


EXECUTE sp_rename N'dbo.tblDealerPartSales.DealerSalesID', N'Tmp_PartSalesID', 'COLUMN'
GO
EXECUTE sp_rename N'dbo.tblDealerPartSales.Tmp_PartSalesID', N'PartSalesID', 'COLUMN'
GO


ALTER TABLE dbo.tblDealersGoals
	DROP CONSTRAINT DF__Temporary__fkDea__37A5467C
GO
ALTER TABLE dbo.tblDealersGoals
	DROP CONSTRAINT DF__Temporary__Model__398D8EEE
GO
ALTER TABLE dbo.tblDealersGoals
	DROP CONSTRAINT DF__TemporaryU__Goal__3A81B327
GO
CREATE TABLE dbo.Tmp_tblDealersGoals
	(
	GoalID int NOT NULL IDENTITY (1, 1),
	DealerID int NOT NULL,
	GoalYear nvarchar(4) NOT NULL,
	Model smallint NOT NULL,
	ModelID int NULL,
	Goal smallint NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_tblDealersGoals ADD CONSTRAINT
	DF__Temporary__fkDea__37A5467C DEFAULT (0) FOR DealerID
GO
ALTER TABLE dbo.Tmp_tblDealersGoals ADD CONSTRAINT
	DF__Temporary__Model__398D8EEE DEFAULT (0) FOR Model
GO
ALTER TABLE dbo.Tmp_tblDealersGoals ADD CONSTRAINT
	DF__TemporaryU__Goal__3A81B327 DEFAULT (0) FOR Goal
GO
SET IDENTITY_INSERT dbo.Tmp_tblDealersGoals OFF
GO
IF EXISTS(SELECT * FROM dbo.tblDealersGoals)
	 EXEC('INSERT INTO dbo.Tmp_tblDealersGoals (DealerID, GoalYear, Model, Goal)
		SELECT fkDealerID, [Year], Model, Goal FROM dbo.tblDealersGoals TABLOCKX')
GO
DROP TABLE dbo.tblDealersGoals
GO
EXECUTE sp_rename N'dbo.Tmp_tblDealersGoals', N'tblDealersGoals', 'OBJECT'
GO
CREATE CLUSTERED INDEX IX_tblDealerGoals_DealerID ON dbo.tblDealersGoals
	(
	DealerID
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
ALTER TABLE dbo.tblDealersGoals ADD CONSTRAINT
	PK_tblDealersGoals PRIMARY KEY NONCLUSTERED 
	(
	GoalID
	) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX IX_tblDealersGoals_GoalYear ON dbo.tblDealersGoals
	(
	GoalYear
	) ON [PRIMARY]
GO
ALTER TABLE dbo.tblDealersGoals WITH NOCHECK ADD CONSTRAINT
	tblDealersGoals_FK00 FOREIGN KEY
	(
	DealerID
	) REFERENCES dbo.tblAllDealers
	(
	DealerID
	) 
	
GO
ALTER TABLE dbo.tblDealersGoals ADD CONSTRAINT
	FK_tblDealersGoals_tblModels FOREIGN KEY
	(
	ModelID
	) REFERENCES dbo.tblModels
	(
	ModelID
	) 
	
GO

update tblDealersGoals
set tblDealersGoals.ModelID = m.ModelID
from tblDealersGoals, tblModels m 
where tblDealersGoals.Model = m.Model and len(m.Model) = 2
go


EXECUTE sp_rename N'dbo.tblDealersGoals', N'tblDealerGoals', 'OBJECT'
GO
ALTER TABLE dbo.tblDealerGoals
	DROP CONSTRAINT DF__Temporary__Model__398D8EEE
GO
ALTER TABLE dbo.tblDealerGoals
	DROP COLUMN Model
GO


ALTER TABLE dbo.tblDealerGoals ADD CONSTRAINT
	IX_tblDealerGoals_DealerYearModel UNIQUE NONCLUSTERED 
	(
	DealerID,
	GoalYear,
	ModelID
	) ON [PRIMARY]

GO


ALTER TABLE dbo.tblAllWarranty
	DROP CONSTRAINT DF_tblWarranty_DateEntered
GO
ALTER TABLE dbo.tblAllWarranty
	DROP CONSTRAINT DF_tblAllWarranty_WarrantyType
GO
CREATE TABLE dbo.Tmp_tblAllWarranty
	(
	WarrantyID int NOT NULL IDENTITY (1, 1),
	MachineSerialNumber nvarchar(50) NULL,
	DateOfFailure datetime NULL,
	CreditMemoNum nvarchar(50) NULL,
	CreditMemoAmt money NULL,
	CreditMemoDate datetime NULL,
	Dealer nvarchar(50) NULL,
	Customer nvarchar(50) NULL,
	RGANum float(53) NULL,
	PartCost float(53) NULL,
	LaborCost float(53) NULL,
	Freight float(53) NULL,
	Problem ntext NULL,
	Model nvarchar(20) NULL,
	Resolution ntext NULL,
	WarrantyOpen bit NULL,
	Travel money NULL,
	Policy money NULL,
	DateEntered datetime NULL,
	PartReceived bit NULL,
	Hours real NULL,
	fkDealerID smallint NULL,
	Comment ntext NULL,
	WarrantyType tinyint NOT NULL,
	DealerRefNum nvarchar(50) NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_tblAllWarranty ADD CONSTRAINT
	DF_tblWarranty_DateEntered DEFAULT (getdate()) FOR DateEntered
GO
ALTER TABLE dbo.Tmp_tblAllWarranty ADD CONSTRAINT
	DF_tblAllWarranty_WarrantyType DEFAULT (0) FOR WarrantyType
GO
SET IDENTITY_INSERT dbo.Tmp_tblAllWarranty ON
GO
IF EXISTS(SELECT * FROM dbo.tblAllWarranty)
	 EXEC('INSERT INTO dbo.Tmp_tblAllWarranty (WarrantyID, MachineSerialNumber, DateOfFailure, CreditMemoNum, CreditMemoAmt, Dealer, Customer, RGANum, PartCost, LaborCost, Freight, Problem, Model, Resolution, WarrantyOpen, Travel, Policy, DateEntered, PartReceived, Hours, fkDealerID, Comment, WarrantyType, DealerRefNum)
		SELECT WarrantyID, MachineSerialNumber, DateOfFailure, CreditMemoNum, CreditMemoAmt, Dealer, Customer, RGANum, PartCost, LaborCost, Freight, Problem, Model, Resolution, WarrantyOpen, Travel, Policy, DateEntered, PartReceived, Hours, fkDealerID, Comment, WarrantyType, DealerRefNum FROM dbo.tblAllWarranty TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblAllWarranty OFF
GO
ALTER TABLE dbo.tblWarrantyParts
	DROP CONSTRAINT FK_tblWarrantyParts_tblWarranty
GO
DROP TABLE dbo.tblAllWarranty
GO
EXECUTE sp_rename N'dbo.Tmp_tblAllWarranty', N'tblAllWarranty', 'OBJECT'
GO
ALTER TABLE dbo.tblAllWarranty ADD CONSTRAINT
	PK_tblWarranty PRIMARY KEY NONCLUSTERED 
	(
	WarrantyID
	) WITH FILLFACTOR = 90 ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX IX_tblWarranty_Dealer ON dbo.tblAllWarranty
	(
	Dealer
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblWarranty_RGANum ON dbo.tblAllWarranty
	(
	RGANum
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblAllWarranty_WarrantyType ON dbo.tblAllWarranty
	(
	WarrantyType
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblAllWarranty_DealerRefNum ON dbo.tblAllWarranty
	(
	DealerRefNum
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblWarrantyParts WITH NOCHECK ADD CONSTRAINT
	FK_tblWarrantyParts_tblWarranty FOREIGN KEY
	(
	WarrantyID
	) REFERENCES dbo.tblAllWarranty
	(
	WarrantyID
	) 
	
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[dlspfrmDealerGoals]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlspfrmDealerGoals]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[dlspfrmDealersPartSales]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlspfrmDealersPartSales]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE dbo.dlspfrmDealerGoals
	@DealerID int = null
AS

SELECT dg.*, tm.Model, tm.Description
FROM tblDealerGoals dg
	INNER JOIN tblModels tm ON tm.ModelID = dg.ModelID
WHERE dg.DealerID = @DealerID
ORDER BY dg.GoalYear DESC, tm.Model ASC
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[dlspfrmDealerGoals]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE dbo.dlspfrmDealersPartSales
	@DealerID int = null
AS

SELECT *
FROM tblDealerPartSales
WHERE DealerID = @DealerID
ORDER BY SalesYear DESC, SalesQuarter DESC
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[dlspfrmDealersPartSales]  TO [fcuser]
GO


DROP INDEX dbo.tblAllWarranty.IX_tblWarranty_RGANum
GO
CREATE CLUSTERED INDEX IX_tblWarranty_RGANum ON dbo.tblAllWarranty
	(
	RGANum
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[wasprptWarrantyTotalCost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprptWarrantyTotalCost]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE wasprptWarrantyTotalCost
	@sDealerName varchar(40) = null,
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@sOpen varchar(2) = null,
	@sFromDate2 varchar(20) = null,
	@sToDate2 varchar(20) = null,
	@sFromDate3 varchar(20) = null,
	@sToDate3 varchar(20) = null,
	@iWarrantyType int = 0
AS

-- Run query
SELECT wa.Dealer, wa.Model, wa.MachineSerialNumber, wa.CreditMemoDate, wa.CreditMemoAmt, wa.DateOfFailure, wa.LaborCost, 
	wp.ExtPartCost, wa.Travel, wa.Hours
FROM dbo.tblAllWarranty wa 
	INNER JOIN (SELECT WarrantyID, SUM(PartCost) AS ExtPartCost FROM dbo.tblWarrantyParts GROUP BY WarrantyID) wp 
		ON wa.WarrantyID = wp.WarrantyID 
WHERE 
	CASE 	WHEN @sFromDate IS NULL AND @sToDate IS NULL THEN 1
		WHEN @sFromDate IS NULL AND wa.DateOfFailure < @sToDate THEN 1
		WHEN @sFromDate < wa.DateOfFailure AND @sToDate IS NULL THEN 1
		WHEN wa.DateOfFailure Between @sFromDate and @sToDate THEN 1
		ELSE 0 END = 1 AND
	CASE 	WHEN @sFromDate2 IS NULL AND @sToDate2 IS NULL THEN 1
		WHEN @sFromDate2 IS NULL AND wa.DateEntered < @sToDate2 THEN 1
		WHEN @sFromDate2 < wa.DateEntered AND @sToDate2 IS NULL THEN 1
		WHEN wa.DateEntered Between @sFromDate2 and @sToDate2 THEN 1
		ELSE 0 END = 1 AND
	CASE 	WHEN @sFromDate3 IS NULL AND @sToDate3 IS NULL THEN 1
		WHEN @sFromDate3 IS NULL AND wa.CreditMemoDate < @sToDate3 THEN 1
		WHEN @sFromDate3 < wa.CreditMemoDate AND @sToDate2 IS NULL THEN 1
		WHEN wa.CreditMemoDate Between @sFromDate3 and @sToDate3 THEN 1
		ELSE 0 END = 1 AND
	CASE	WHEN @sDealerName IS NULL THEN 1
		WHEN wa.Dealer LIKE @sDealerName THEN 1
		ELSE 0 END = 1 AND
	CASE 	WHEN @sOpen IS NULL THEN 1
		WHEN wa.WarrantyOpen = @sOpen THEN 1
		ELSE 0 END = 1 AND
	CASE	WHEN @iWarrantyType IS NULL THEN 1
		WHEN wa.WarrantyTYpe = @iWarrantyType THEN 1
		ELSE 0 END = 1	

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptWarrantyTotalCost]  TO [fcuser]
GO


ALTER TABLE dbo.tblAllLeads
	DROP CONSTRAINT tblLeads_PK
GO
ALTER TABLE dbo.tblAllLeads ADD CONSTRAINT
	tblLeads_PK PRIMARY KEY CLUSTERED 
	(
	LeadID
	) WITH FILLFACTOR = 90 ON [PRIMARY]

GO

update tblAllOrders
set fkDealerID = dl.DealerID
from tblAllOrders, tblAlldealers dl
where tblAllOrders.Dealer = dl.DealerName
	and (tblAllOrders.fkDealerID is null or tblAllOrders.fkDealerID = 0)
go


insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('4.05','2000-9-25 00:00:00','- Parts:  Dealer Net price is not calculated for parts with a cost each of 0.
- Parts:  All parts with a blank Cost Each now have a Cost Each of 0.
- Warranty: Fixed problem deleting records (also TC Warranty)
- Lists: Added Tom Cat dealers to allow choosing Tom Cat dealers
- Warranty: Added ''Credit Memo Date'' field (also TC Warranty)
- Warranty: Added ''Credit Memo Date'' criteria to ''Total Cost of Warranty'' report.  Also added CM Amount to the report.
- Dealers: Added Part Sales by Quarter tab to allow inputting sales
- Orders: Improved the loading time
- Leads: Improved the loading time
')
go

update tblDBProperties
set PropertyValue = '4.05'
where PropertyName = 'DBStructVersion'
go

