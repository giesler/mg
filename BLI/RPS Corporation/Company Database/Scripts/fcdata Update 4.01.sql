-- fcdata update 4.01

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptMarginByModel]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptMarginByModel]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptMarginByModel
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT ShippedDate, Dealer, Model, OrderNumber, Quantity, SalePrice, CostPrice, SalePrice - CostPrice AS Margin, SerialNumber 
FROM dbo.tblAllOrders 
WHERE (SalePrice <> 0) 
	AND (OrderType = @iOrderType)
	AND (ShippedDate Between @sFromDate and @sToDate)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptMajorAccountSales]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptMajorAccountSales]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptMajorAccountSales
	@sMAName varchar(40) = null,
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @sMAName IS NULL
	SELECT @sMAName = '%'
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT ma.MACompName, ma.MAHeadqAddress, ma.MACity, ma.MAState, 
    ma.MAZip, ao.SaleDate, ao.[Name], ao.Address, ao.City, ao.State, ao.Zip, 
    ao.Dealer, ao.Model, ao.SalePrice, ao.OrderDate
FROM dbo.tblAllMajorAcnts ma INNER JOIN dbo.tblAllOrders ao ON 
    ma.MajorAccountID = ao.MajorAccountID
WHERE ao.OrderType = @iOrderType
	AND ao.OrderDate Between @sFromDate And @sToDate
	AND ma.MACompName like @sMAName

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptSalesDealer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptSalesDealer]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptSalesModel]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptSalesModel]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptSalesModelON]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptSalesModelON]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptSalesDealer
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT Dealer, Model, OrderNumber, ShippedDate, Quantity, SalePrice, CostPrice, SalePrice - CostPrice as Margin, OrderDate 
FROM dbo.tblAllOrders 
WHERE (SalePrice <> 0) 
	AND (OrderType = @iOrderType)
	AND (OrderDate Between @sFromDate and @sToDate)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE dbo.orsprptSalesModel
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS 
-- Check input parameters
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT Model, Dealer, Sum(Quantity) AS Quantity, Sum(SalePrice) AS SalePrice, Sum(CostPrice) AS CostPrice, 
	Sum(SalePrice - CostPrice) AS Margin
FROM tblAllOrders
WHERE SalePrice <> 0
	AND (OrderType = @iOrderType)
	AND (OrderDate Between @sFromDate and @sToDate)
GROUP BY Model, Dealer

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE dbo.orsprptSalesModelON
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS 
-- Check input parameters
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
--- Run query
SELECT Model, Dealer, OrderNumber, SalePrice, CostPrice, SalePrice - CostPrice AS Margin
FROM tblAllOrders
WHERE SalePrice <> 0
	AND (OrderType = @iOrderType)
	AND (OrderDate Between @sFromDate and @sToDate)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO



update [Switchboard Items]
set Argument = 'orrptAcknowledgement', OpenArgs = 'TomCatMode'
where Argument = 'ortrptAcknowledgement'
go

update [Switchboard Items]
set Argument = 'orrptCustomerLabels', OpenArgs = 'TomCatMode'
where Argument = 'ortrptCustomerLabels'
go

update [Switchboard Items]
set Argument = 'orrptCustomerMailLabels', OpenArgs = 'TomCatMode'
where Argument = 'ortrptCustomerMailLabels'
go

update [Switchboard Items]
set Argument = 'orrptDealerSales', OpenArgs = 'TomCatMode'
where Argument = 'ortrptDealerSales'
go

update [Switchboard Items]
set Argument = 'orrptDealerSerial', OpenArgs = 'TomCatMode'
where Argument = 'ortrptDealerSerial'
go

update [Switchboard Items]
set Argument = 'orrptEndUser', OpenArgs = 'TomCatMode'
where Argument = 'ortrptEndUser'
go

update [Switchboard Items]
set Argument = 'orrptEndUserDealer', OpenArgs = 'TomCatMode'
where Argument = 'ortrptEndUserDealer'
go

update [Switchboard Items]
set Argument = 'orrptEndUserLabels', OpenArgs = 'TomCatMode'
where Argument = 'ortrptEndUserLabels'
go

update [Switchboard Items]
set Argument = 'orrptEndUserState', OpenArgs = 'TomCatMode'
where Argument = 'ortrptEndUserState'
go

update [Switchboard Items]
set Argument = 'orrptMajorAccountSales', OpenArgs = 'TomCatMode'
where Argument = 'ortrptMajorAccountSales'
go

update [Switchboard Items]
set Argument = 'orrptMarginByModel', OpenArgs = 'TomCatMode'
where Argument = 'ortrptMarginByModel'
go

update [Switchboard Items]
set Argument = 'orrptOpenOrders', OpenArgs = 'TomCatMode'
where Argument = 'ortrptOpenOrders'
go

update [Switchboard Items]
set Argument = 'orrptPrepSheet', OpenArgs = 'TomCatMode'
where Argument = 'ortrptPrepSheet'
go

update [Switchboard Items]
set Argument = 'orrptProdSchedule', OpenArgs = 'TomCatMode'
where Argument = 'ortrptProdSchedule'
go

update [Switchboard Items]
set Argument = 'orrptSalesDealer', OpenArgs = 'TomCatMode'
where Argument = 'ortrptSalesDealer'
go

update [Switchboard Items]
set Argument = 'orrptSalesModel', OpenArgs = 'TomCatMode'
where Argument = 'ortrptSalesModel'
go

update [Switchboard Items]
set Argument = 'orrptServicingDealers', OpenArgs = 'TomCatMode'
where Argument = 'ortrptServicingDealers'
go

update [Switchboard Items]
set Argument = 'orrptUnregSweepers', OpenArgs = 'TomCatMode'
where Argument = 'ortrptUnregSweepers'
go


insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (16, 6, 'Update Parts Prices', '3', 'pafdlgPrices')
go

update tblProdSchedules
set Year = 2000
where Year = 0
go

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('4.01','2000-8-13 00:00:00','- Parts: Updated ''USA Sug List'' to be calculated from ''USA Dealer Net''
- Orders: Fixed Margin not showing up, also fixed several other Orders reports which weren''t quite working right
- Warranty: Dealer Ref # field should be wide enough on all reports
- Orders: Removed page number from Tom Cat Dealer Sales report
- Parts: Fixed error displayed when you click ''Find Part''
- Parts: Fixed ''Production Schedule'' report
')
go

update tblDBProperties
set PropertyValue = '4.01'
where PropertyName = 'DBStructVersion'
go
