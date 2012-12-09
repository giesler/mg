-- fcdata update 4.11

update dbo.tblSwitchboard set Argument = 'pstfrmPartSales'
where SwitchboardID = 45 and ItemNumber = 1
go


CREATE TABLE dbo.tblWarrantyTech
	(
	WarrantyTechID int NOT NULL IDENTITY (1, 1),
	ReferenceNum int NOT NULL,
	TechDate smalldatetime NOT NULL,
	DealerID int NULL,
	ModelID int NULL,
	SerialNumber nvarchar(100) NULL,
	Problem ntext NULL,
	Resolution ntext NULL,
	WarrantyTechType tinyint NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.tblWarrantyTech ADD CONSTRAINT
	DF_tblWarrantyTech_WarrantyTechType DEFAULT 0 FOR WarrantyTechType
GO
ALTER TABLE dbo.tblWarrantyTech ADD CONSTRAINT
	PK_tblWarrantyTech PRIMARY KEY NONCLUSTERED 
	(
	WarrantyTechID
	) ON [PRIMARY]

GO
ALTER TABLE dbo.tblWarrantyTech ADD CONSTRAINT
	IX_tblWarrantyTech_ReferenceNum UNIQUE CLUSTERED 
	(
	ReferenceNum
	) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX IX_tblWarrantyTech_TechDate ON dbo.tblWarrantyTech
	(
	TechDate
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblWarrantyTech_DealerID ON dbo.tblWarrantyTech
	(
	DealerID
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblWarrantyTech_ModelID ON dbo.tblWarrantyTech
	(
	ModelID
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblWarrantyTech_SerialNumber ON dbo.tblWarrantyTech
	(
	SerialNumber
	) ON [PRIMARY]
GO
ALTER TABLE dbo.tblWarrantyTech ADD CONSTRAINT
	FK_tblWarrantyTech_tblAllDealers FOREIGN KEY
	(
	DealerID
	) REFERENCES dbo.tblAllDealers
	(
	DealerID
	)
GO
ALTER TABLE dbo.tblWarrantyTech ADD CONSTRAINT
	FK_tblWarrantyTech_tblModels FOREIGN KEY
	(
	ModelID
	) REFERENCES dbo.tblModels
	(
	ModelID
	)
GO


ALTER TABLE dbo.tblWarrantyTech
	DROP CONSTRAINT IX_tblWarrantyTech_ReferenceNum
GO
ALTER TABLE dbo.tblWarrantyTech ADD CONSTRAINT
	IX_tblWarrantyTech_ReferenceNum UNIQUE CLUSTERED 
	(
	ReferenceNum,
	WarrantyTechType
	) ON [PRIMARY]

GO

insert into dbo.tblSwitchboard (SwitchboardID, ItemNumber, ItemText, Command, Argument, OpenArgs)
values (46, 0, 'FC Warranty Tech', 0, '', '')
go

insert into dbo.tblSwitchboard (SwitchboardID, ItemNumber, ItemText, Command, Argument, OpenArgs)
values (46, 1, 'Add/edit/view Warranty Tech records', 3, 'wtfrmWarrantyTech', '')
go

insert into dbo.tblSwitchboard (SwitchboardID, ItemNumber, ItemText, Command, Argument, OpenArgs)
values (46, 2, 'Warranty Tech Report by Date', 4, 'wtrptDate', '')
go

insert into dbo.tblSwitchboard (SwitchboardID, ItemNumber, ItemText, Command, Argument, OpenArgs)
values (46, 3, 'Warranty Tech Report by Dealer', 4, 'wtrptDealer', '')
go

insert into dbo.tblSwitchboard (SwitchboardID, ItemNumber, ItemText, Command, Argument, OpenArgs)
values (46, 3, 'Warranty Tech Report by Model', 4, 'wtrptModel', '')
go

-----------------
insert into dbo.tblSwitchboard (SwitchboardID, ItemNumber, ItemText, Command, Argument, OpenArgs)
values (47, 0, 'TC Warranty Tech', 0, '', '')
go

insert into dbo.tblSwitchboard (SwitchboardID, ItemNumber, ItemText, Command, Argument, OpenArgs)
values (47, 1, 'Add/edit/view Warranty Tech records', 3, 'wttfrmWarrantyTech', '')
go

insert into dbo.tblSwitchboard (SwitchboardID, ItemNumber, ItemText, Command, Argument, OpenArgs)
values (47, 2, 'Warranty Tech Report by Date', 4, 'wtrptDate', 'TomCatMode')
go

insert into dbo.tblSwitchboard (SwitchboardID, ItemNumber, ItemText, Command, Argument, OpenArgs)
values (47, 3, 'Warranty Tech Report by Dealer', 4, 'wtrptDealer', 'TomCatMode')
go

insert into dbo.tblSwitchboard (SwitchboardID, ItemNumber, ItemText, Command, Argument, OpenArgs)
values (47, 3, 'Warranty Tech Report by Model', 4, 'wtrptModel', 'TomCatMode')
go

--------------------

insert into dbo.tblSwitchboard (SwitchboardID, ItemNumber, ItemText, Command, Argument, OpenArgs)
values (41, 10, 'Tom Cat Warranty Tech', 1, '47', '')
go

insert into dbo.tblSwitchboard (SwitchboardID, ItemNumber, ItemText, Command, Argument, OpenArgs)
values (1, 16, 'Warranty Tech', 1, '46', '')
go


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[wtsprptWarrantyTech]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wtsprptWarrantyTech]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE dbo.wtsprptWarrantyTech
	@FromDate varchar(20) = null,
	@ToDate varchar(20) = null,
	@DealerName varchar(100) = null,
	@ModelID int = null,
	@WarrantyTechType varchar(2) = null
AS

SELECT wt.ReferenceNum, wt.TechDate, dl.DealerName, md.Model, wt.SerialNumber, wt.Problem, wt.Resolution
FROM dbo.tblWarrantyTech wt
	LEFT OUTER JOIN dbo.tblAllDealers dl ON dl.DealerID = wt.DealerID
	LEFT OUTER JOIN dbo.tblModels md ON md.ModelID = wt.ModelID
WHERE 	
	CASE 	WHEN @WarrantyTechType IS NULL THEN 1
		WHEN WarrantyTechType = @WarrantyTechType THEN 1
		ELSE 0 END = 1 AND
	-- Sale Date criteria
	CASE
		WHEN @FromDate IS NULL AND @ToDate IS NULL THEN 1
		WHEN @FromDate IS NULL AND @ToDate IS NOT NULL
			AND TechDate < @ToDate THEN 1
		WHEN @FromDate IS NOT NULL AND @ToDate IS NULL
			AND TechDate > @FromDate THEN 1
		WHEN @FromDate IS NOT NULL AND @ToDate IS NOT NULL
			AND TechDate Between @FromDate and @ToDate THEN 1
		ELSE 0 END = 1 AND
	-- Model
	CASE	WHEN @ModelID IS NULL THEN 1
		WHEN md.ModelID = @ModelID THEN 1
		ELSE 0 END = 1 AND
	-- Dealer Name
	CASE	WHEN @DealerName IS NULL THEN 1
		WHEN dl.DealerName = @DealerName THEN 1
		ELSE 0 END = 1
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[wtsprptWarrantyTech]  TO [fcuser]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[wasprptWarrantyTotalCost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprptWarrantyTotalCost]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



CREATE PROCEDURE dbo.wasprptWarrantyTotalCost
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
	wp.ExtPartCost, wa.Travel, wa.Hours, wa.RGANum
FROM dbo.tblAllWarranty wa 
	INNER JOIN (SELECT WarrantyID, SUM(PartCost) AS ExtPartCost FROM dbo.tblWarrantyParts GROUP BY WarrantyID) wp 
		ON wa.WarrantyID = wp.WarrantyID 
WHERE 
	wa.NewPart = 0 AND 
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

update dbo.tblSwitchboard
set ItemNumber = ItemNumber + 1
where SwitchboardID = 23 and ItemNumber > 2
go

insert into dbo.tblSwitchboard (SwitchboardID, ItemNumber, ItemText, Command, Argument, OpenArgs)
values (23, 3, 'Warranty by Model', 4, 'warptModel', '')
go

update dbo.tblSwitchboard
set ItemNumber = ItemNumber + 1
where SwitchboardID = 40 and ItemNumber > 2
go

insert into dbo.tblSwitchboard (SwitchboardID, ItemNumber, ItemText, Command, Argument, OpenArgs)
values (40, 3, 'Warranty by Model', 4, 'warptModel', 'TomCatMode')
go

update dbo.tblSwitchboard
set	ItemText = ItemNumber
where SwitchboardID in (23, 40) and ItemNumber in (2, 3, 4)
go

update dbo.tblSwitchboard
set 	ItemText = 'Warranty by Model',
	Argument = 'warptModel'
where SwitchboardID = 23 and ItemNumber = 2
go

update dbo.tblSwitchboard
set 	ItemText = 'Warranty by Part Number',
	Argument = 'warptPartNum'
where SwitchboardID = 23 and ItemNumber = 3
go

update dbo.tblSwitchboard
set 	ItemText = 'Warranty by Serial Number',
	Argument = 'warptSerialNum'
where SwitchboardID = 23 and ItemNumber = 4
go

update dbo.tblSwitchboard
set 	ItemText = 'Warranty by Model',
	Argument = 'warptModel'
where SwitchboardID = 40 and ItemNumber = 2
go

update dbo.tblSwitchboard
set 	ItemText = 'Warranty by Part Number',
	Argument = 'warptPartNum'
where SwitchboardID = 40 and ItemNumber = 3
go

update dbo.tblSwitchboard
set 	ItemText = 'Warranty by Serial Number',
	Argument = 'warptSerialNum'
where SwitchboardID = 40 and ItemNumber = 4
go

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[wasprptWarrantyCosts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprptWarrantyCosts]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE dbo.wasprptWarrantyCosts 
	@DealerName varchar(40) = null,
	@PartID varchar(50) = null,
	@Open varchar(2) = null,
	@FailFromDate varchar(20) = null,
	@FailToDate varchar(20) = null,
	@EnterFromDate varchar(20) = null,
	@EnterToDate varchar(20) = null,
	@CMFromDate varchar(20) = null,
	@CMToDate varchar(20) = null,
	@iWarrantyType int = 0
AS
-- Run query
SELECT     wa.MachineSerialNumber, wa.DateOfFailure, wa.CreditMemoNum, wa.Dealer, wa.Customer, wa.RGANum, wa.LaborCost, wa.Freight, wa.Problem, 
                      wa.WarrantyID, pa.RPSPartNum, pa.PartName, wp.PartCost, wa.Model, wa.Hours, wa.WarrantyOpen, pa.RPSPNSort, wa.DealerRefNum
FROM         dbo.tblAllWarranty wa
	LEFT OUTER JOIN dbo.tblWarrantyParts wp ON wp.WarrantyID = wa.WarrantyID
	LEFT OUTER JOIN dbo.tblParts pa ON pa.PartID = wp.PartID
WHERE 
	wa.NewPart = 0 AND 
	CASE 	WHEN @FailFromDate IS NULL AND @FailToDate IS NULL THEN 1
		WHEN @FailFromDate IS NULL AND wa.DateOfFailure < @FailToDate THEN 1
		WHEN @FailFromDate < wa.DateOfFailure AND @FailToDate IS NULL THEN 1
		WHEN wa.DateOfFailure Between @FailFromDate and @FailToDate THEN 1
		ELSE 0 END = 1 AND
	CASE 	WHEN @EnterFromDate IS NULL AND @EnterToDate IS NULL THEN 1
		WHEN @EnterFromDate IS NULL AND wa.DateEntered < @EnterToDate THEN 1
		WHEN @EnterFromDate < wa.DateEntered AND @EnterToDate IS NULL THEN 1
		WHEN wa.DateEntered Between @EnterFromDate and @EnterToDate THEN 1
		ELSE 0 END = 1 AND
	CASE 	WHEN @CMFromDate IS NULL AND @CMToDate IS NULL THEN 1
		WHEN @CMFromDate IS NULL AND wa.CreditMemoDate < @CMToDate THEN 1
		WHEN @CMFromDate < wa.CreditMemoDate AND @CMToDate IS NULL THEN 1
		WHEN wa.CreditMemoDate Between @CMFromDate and @CMToDate THEN 1
		ELSE 0 END = 1 AND
	CASE	WHEN @DealerName IS NULL THEN 1
		WHEN wa.Dealer LIKE @DealerName THEN 1
		ELSE 0 END = 1 AND
	CASE	WHEN @PartID IS NULL THEN 1
		WHEN pa.PartID = @PartID THEN 1
		ELSE 0 END = 1 AND
	CASE 	WHEN @Open IS NULL THEN 1
		WHEN wa.WarrantyOpen = @Open THEN 1
		ELSE 0 END = 1 AND
	CASE	WHEN @iWarrantyType IS NULL THEN 1
		WHEN wa.WarrantyTYpe = @iWarrantyType THEN 1
		ELSE 0 END = 1
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptWarrantyCosts]  TO [fcuser]
GO


ALTER TABLE dbo.tblPartSales
	DROP CONSTRAINT FK_tblPartSales_tblAllDealers
GO


ALTER TABLE dbo.tblPartSales
	DROP CONSTRAINT DF_tblPartSales_AmountParts
GO
ALTER TABLE dbo.tblPartSales
	DROP CONSTRAINT DF_tblPartSales_AmountFreight
GO
ALTER TABLE dbo.tblPartSales
	DROP CONSTRAINT DF_tblPartSales_AmountMisc
GO
ALTER TABLE dbo.tblPartSales
	DROP CONSTRAINT DF_tblPartSales_PartSaleType
GO
CREATE TABLE dbo.Tmp_tblPartSales
	(
	PartSaleID int NOT NULL IDENTITY (1, 1),
	DealerID int NULL,
	SaleDate smalldatetime NULL,
	DealerPO nvarchar(100) NULL,
	InvoiceNum nvarchar(100) NULL,
	AmountParts smallmoney NULL,
	AmountFreight smallmoney NULL,
	AmountMisc smallmoney NULL,
	PartSaleType tinyint NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_tblPartSales ADD CONSTRAINT
	DF_tblPartSales_AmountParts DEFAULT (0) FOR AmountParts
GO
ALTER TABLE dbo.Tmp_tblPartSales ADD CONSTRAINT
	DF_tblPartSales_AmountFreight DEFAULT (0) FOR AmountFreight
GO
ALTER TABLE dbo.Tmp_tblPartSales ADD CONSTRAINT
	DF_tblPartSales_AmountMisc DEFAULT (0) FOR AmountMisc
GO
ALTER TABLE dbo.Tmp_tblPartSales ADD CONSTRAINT
	DF_tblPartSales_PartSaleType DEFAULT (0) FOR PartSaleType
GO
SET IDENTITY_INSERT dbo.Tmp_tblPartSales ON
GO
IF EXISTS(SELECT * FROM dbo.tblPartSales)
	 EXEC('INSERT INTO dbo.Tmp_tblPartSales (PartSaleID, DealerID, SaleDate, DealerPO, AmountParts, AmountFreight, AmountMisc, PartSaleType)
		SELECT PartSaleID, DealerID, SaleDate, DealerPO, AmountParts, AmountFreight, AmountMisc, PartSaleType FROM dbo.tblPartSales TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblPartSales OFF
GO
DROP TABLE dbo.tblPartSales
GO
EXECUTE sp_rename N'dbo.Tmp_tblPartSales', N'tblPartSales', 'OBJECT'
GO
ALTER TABLE dbo.tblPartSales ADD CONSTRAINT
	PK_tblPartSales PRIMARY KEY CLUSTERED 
	(
	PartSaleID
	) ON [PRIMARY]

GO
CREATE NONCLUSTERED INDEX IX_tblPartSales_DealerID ON dbo.tblPartSales
	(
	DealerID
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblPartSales_SaleDate ON dbo.tblPartSales
	(
	SaleDate
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblPartSales_PartSaleType ON dbo.tblPartSales
	(
	PartSaleType
	) ON [PRIMARY]
GO
ALTER TABLE dbo.tblPartSales WITH NOCHECK ADD CONSTRAINT
	FK_tblPartSales_tblAllDealers FOREIGN KEY
	(
	DealerID
	) REFERENCES dbo.tblAllDealers
	(
	DealerID
	)
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[psrptPartSales]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[psrptPartSales]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE dbo.psrptPartSales
	@FromDate varchar(20) = null,
	@ToDate varchar(20) = null,
	@DealerName varchar(100) = null,
	@PartSaleType varchar(2) = null
AS

SELECT dl.DealerName, ps.SaleDate, ps.DealerPO, ps.InvoiceNum, ps.AmountParts, ps.AmountFreight, ps.AmountMisc, 
	ps.AmountParts + ps.AmountFreight + ps.AmountMisc AS AmountTotal
FROM dbo.tblPartSales ps
	INNER JOIN dbo.tblAllDealers dl ON dl.DealerID = ps.DealerID
WHERE 	
	CASE 	WHEN @PartSaleType IS NULL THEN 1
		WHEN PartSaleType = @PartSaleType THEN 1
		ELSE 0 END = 1 AND
	-- Sale Date criteria
	CASE
		WHEN @FromDate IS NULL AND @ToDate IS NULL THEN 1
		WHEN @FromDate IS NULL AND @ToDate IS NOT NULL
			AND SaleDate < @ToDate THEN 1
		WHEN @FromDate IS NOT NULL AND @ToDate IS NULL
			AND SaleDate > @FromDate THEN 1
		WHEN @FromDate IS NOT NULL AND @ToDate IS NOT NULL
			AND SaleDate Between @FromDate and @ToDate THEN 1
		ELSE 0 END = 1 AND
	-- Dealer Name
	CASE	WHEN @DealerName IS NULL THEN 1
		WHEN dl.DealerName = @DealerName THEN 1
		ELSE 0 END = 1
ORDER BY DealerName, SaleDate
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[psrptPartSales]  TO [fcuser]
GO


ALTER TABLE dbo.tblWarrantyTech
	DROP CONSTRAINT FK_tblWarrantyTech_tblAllDealers
GO
ALTER TABLE dbo.tblPartSales
	DROP CONSTRAINT FK_tblPartSales_tblAllDealers
GO
ALTER TABLE dbo.tblDealerGoals
	DROP CONSTRAINT tblDealersGoals_FK00
GO
ALTER TABLE dbo.tblDealerAdvert
	DROP CONSTRAINT tblDealerAdvert_FK00
GO
ALTER TABLE dbo.tblAllDealers
	DROP CONSTRAINT aaaaatblDealers_PK
GO
DROP INDEX dbo.tblAllDealers.Sort1
GO
ALTER TABLE dbo.tblAllDealers ADD CONSTRAINT
	tblDealers_PK PRIMARY KEY NONCLUSTERED 
	(
	DealerID
	) WITH FILLFACTOR = 90 ON [PRIMARY]

GO
CREATE CLUSTERED INDEX IX_tblAllDealers_DealerName ON dbo.tblAllDealers
	(
	DealerName
	) ON [PRIMARY]
GO

ALTER TABLE dbo.tblDealerAdvert WITH NOCHECK ADD CONSTRAINT
	tblDealerAdvert_FK00 FOREIGN KEY
	(
	DealerID
	) REFERENCES dbo.tblAllDealers
	(
	DealerID
	)
GO

ALTER TABLE dbo.tblDealerGoals WITH NOCHECK ADD CONSTRAINT
	tblDealersGoals_FK00 FOREIGN KEY
	(
	DealerID
	) REFERENCES dbo.tblAllDealers
	(
	DealerID
	)
GO

ALTER TABLE dbo.tblPartSales WITH NOCHECK ADD CONSTRAINT
	FK_tblPartSales_tblAllDealers FOREIGN KEY
	(
	DealerID
	) REFERENCES dbo.tblAllDealers
	(
	DealerID
	)
GO

ALTER TABLE dbo.tblWarrantyTech WITH NOCHECK ADD CONSTRAINT
	FK_tblWarrantyTech_tblAllDealers FOREIGN KEY
	(
	DealerID
	) REFERENCES dbo.tblAllDealers
	(
	DealerID
	)
GO

update dbo.tblAllOrders
set Dealer = 'Mark C. Pope & Associates'
where Dealer = 'Mark C. Pope Co'
go

update dbo.tblAllOrders
set Dealer = 'Allied Industrial Equipment Corporation'
where Dealer = 'Allied Industrial Equipment Corp'
go


update dbo.tblAllOrders
set Dealer = 'A.I.M.S., Inc.'
where Dealer = 'A.I.M.S. Inc'
go

update dbo.tblAllOrders
set Dealer = 'Acme Soap, Inc.'
where Dealer = 'Acme Soap'
go

update dbo.tblAllOrders
set Dealer = 'Acme Soap, Inc.'
where Dealer = 'Acme Soap Inc'
go

update dbo.tblAllOrders
set Dealer = 'All-Lift Ltd.'
where Dealer = 'All-Lift Ltd'
go

update dbo.tblAllOrders
set Dealer = 'Bernie''s Equipment Company, Inc.'
where Dealer = 'Bernie''s Equipment Co., Inc'
go

update dbo.tblAllOrders
set Dealer = 'Brophy, Inc.'
where Dealer = 'Brophy, Inc'
go

update dbo.tblAllOrders
set Dealer = 'Brungart Equipment Company'
where Dealer = 'Brungart Equipment Co'
go

update dbo.tblAllOrders
set Dealer = 'Buchanan Equipment Company'
where Dealer = 'Buchanan Equipment Co.'
go

update dbo.tblAllOrders
set Dealer = 'Caliber Equipment, Inc.'
where Dealer = 'Caliber Equipment Inc'
go

update dbo.tblAllOrders
set Dealer = 'Easy Buff'
where Dealer = 'Easy Bluff'
go

update dbo.tblAllOrders
set Dealer = 'Equipment Engineering'
where Dealer = 'Engineering Equipment'
go

update dbo.tblAllOrders
set Dealer = 'Equipment, Inc.'
where Dealer = 'Equipment  Inc'
go

update dbo.tblAllOrders
set Dealer = 'Ergonomic Handling, Inc.'
where Dealer = 'Ergonomic Handling Inc'
go

update dbo.tblAllOrders
set Dealer = 'Forklifts, Inc.'
where Dealer = 'Forklifts Inc'
go

update dbo.tblAllOrders
set Dealer = 'Grady W. Jones Company'
where Dealer = 'Grady W. Jones co'
go

update dbo.tblAllOrders
set Dealer = 'Industrial Parts & Service Company'
where Dealer = 'Industrial Parts & Service Co'
go

update dbo.tblAllOrders
set Dealer = 'Lift Power, Inc.'
where Dealer = 'Lift Power Inc'
go

update dbo.tblAllOrders
set Dealer = 'Lincoln Service & Equipment'
where Dealer = 'Lincoln Service & Eq'
go

update dbo.tblAllOrders
set Dealer = 'Mar-Co Equipment Company'
where Dealer = 'Mar-Co Equipment Co.'
go

update dbo.tblAllOrders
set Dealer = 'Nationwide Wire & Brush Manufacturing'
where Dealer = 'Nationwide Wire & Brush Mfg'
go

update dbo.tblAllOrders
set Dealer = 'Norlift, Inc.'
where Dealer = 'Norlift, Inc'
go

update dbo.tblAllOrders
set Dealer = 'Peters Industrial Equipment, Inc.'
where Dealer = 'Peters Industrial Equipment Inc'
go

update dbo.tblAllOrders
set Dealer = 'Power Clean Industries'
where Dealer = 'Power Clean, Ind'
go

update dbo.tblAllOrders
set Dealer = 'RPS Corporation'
where Dealer = 'Precision Fleet'
go

update dbo.tblAllOrders
set Dealer = 'Pride Equipment Corporation'
where Dealer = 'Pride Equipment Corp'
go

update dbo.tblAllOrders
set Dealer = 'ProLift Industrial Equipment'
where Dealer = 'ProLift Ind Equipment'
go

update dbo.tblAllOrders
set Dealer = 'SE REP-Darin Park'
where Dealer = 'SE REP- Darin Park'
go

update dbo.tblAllOrders
set Dealer = 'Shipping Utilities, Inc.'
where Dealer = 'Shipping Utilities Inc.'
go

update dbo.tblAllOrders
set Dealer = 'Southern Nevada Equipment Company'
where Dealer = 'Southern Nevada Equipment Co'
go

update dbo.tblAllOrders
set Dealer = 'Southline Equipment Company'
where Dealer = 'Southline Equipment Co.'
go

update dbo.tblAllOrders
set Dealer = 'Stampede Marketing Group, Inc.'
where Dealer = 'Stampede Marketing Group Inc'
go

update dbo.tblAllOrders
set Dealer = 'The Belue Company of Georgia'
where Dealer = 'The Belue Company of GA'
go

update dbo.tblAllOrders
set Dealer = 'Total Line Supply Company'
where Dealer = 'Total Line'
go

update dbo.tblAllOrders
set Dealer = 'Total Line Supply Company'
where Dealer = 'Total Line Supply Co.'
go

update dbo.tblAllOrders
set Dealer = 'Vonachen Service & Supply'
where Dealer = 'Vonachen/Total Line'
go

update dbo.tblAllOrders
set Dealer = 'Wagner Lift Truck Corporation'
where Dealer = 'Wagner Lift Truck Corp.'
go


-- Update dealer id field in Orders table
update o
set fkDealerID = (SELECT MIN(DealerID) AS DealerID FROM dbo.tblDealers WHERE o.Dealer = DealerName GROUP BY DealerName )
from dbo.tblAllOrders o
go

insert into dbo.tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('4.11','2001-1-27','- TC Part Sales: Fixed it showing FC Part Sales
- FC/TC Warranty Tech:  Added to database, also added reports
- FC/TC Warranty:  Added ''By Model'' report
- FC/TC Warranty:  Fixed ''Total Cost of Warranty'' report showing dealers for only FC in criteria drop down
- FC/TC Warranty:  Renamed ''Warranty Report'' to ''Warranty by Serial Number''
- FC/TC Warranty:  Added additional report criteria to ''Warranty Costs'', ''Warranty by Part Number'', and ''Warranty by Serial Number''
- FC/TC Part Sales:  Added ''Invoice #'' field to form and report
- FC/TC Orders:  Dealer Rep Commission report;  Carolina Industrial Equip missing orders on report because there are two dealers with this same name, one of which did not have Darin Park as the sales rep.  Same situation with ''System Clean''.
- FC/TC Orders:  Fixed Dealer Names not matching Dealer Names in the Dealer file causing many orders to not show up on several reports.
- FC/TC Dealers:  When a dealer name is updated, it updates the orders file as well
- FC/TC Dealers:  To delete a dealer, make sure all ''Goals'' and ''Advert Allocs'' are deleted first.
- Security:  Included updated security program to fix error
')
go

update dbo.tblDBProperties
set PropertyValue = '4.11'
where PropertyName = 'DBStructVersion'
go

