-- fcdata update 3.27
-- - update rel notes, db version

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptDealerSales]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptDealerSales]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptDealerSales 
	@sDealerName varchar(40) = null,
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iOrderType varchar(3) = 0
AS
-- Check input parameters
IF @sDealerName IS NULL
	SELECT @sDealerName = '%'
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iOrderType IS NULL
	SELECT @iOrderType = 0
-- Run query
SELECT DISTINCT ao.Dealer, ao.Model, ao.OrderDate, ao.ShippedDate, ao.Quantity, ao.SalePrice, ao.OrderNumber, ao.SerialNumber
FROM tblAllOrders ao INNER JOIN tblDealers ON ao.Dealer = tblDealers.DealerName
WHERE (tblDealers.CurrentDealer = 1) AND SalePrice <> 0 AND ao.OrderType = @iOrderType
	AND (ao.Dealer like @sDealerName)
	AND (ao.OrderDate Between @sFromDate And @sToDate)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptDealerSales]  TO [fcuser]
GO



insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.27','2000-1-6 00:00:00','- Updated ''Dealer Sales'' report in Orders.')
go

update tblDBProperties
set PropertyValue = '3.27'
where PropertyName = 'DBStructVersion'
go
