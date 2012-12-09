USE fcdata
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[orsprptDealerSales]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptDealerSales]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE orsprptDealerSales 
	@sDealerName varchar(40),
	@sFromDate varchar(20),
	@sToDate varchar(20)
AS

DECLARE @sSQL varchar(800)

SELECT @sSQL = 'SELECT tblOrders.Dealer, tblOrders.Model, tblOrders.OrderDate, 
		    tblOrders.ShippedDate, tblOrders.Quantity, 
		    tblOrders.SalePrice, tblOrders.OrderNumber, tblOrders.SerialNumber
	FROM tblOrders INNER JOIN
		    tblDealers ON 
		    tblOrders.Dealer = tblDealers.DealerName
	WHERE (tblDealers.CurrentDealer = 1) AND SalePrice <> 0 '

IF @sDealerName IS NOT NULL 
  BEGIN
	SELECT @sSQL = @sSQL + 'AND tblOrders.Dealer = ''' +  @sDealerName + ''' '
  END
IF @sFromDate IS NOT NULL And @sToDate IS NOT NULL
  BEGIN
	SELECT @sSQL = @sSQL + 'AND tblOrders.OrderDate Between ''' + @sFromDate + ''' And ''' + @sToDate + ''' '
  END
ELSE IF @sFromDate IS NOT NULL 
  BEGIN
	SELECT @sSQL = @sSQL + 'AND tblOrders.OrderDate > ''' + @sFromDate + ''' '
  END
ELSE IF @sToDate IS NOT NULL 
  BEGIN
	SELECT @sSQL = @sSQL + 'AND tblOrders.OrderDate < ''' + @sToDate + ''' '
  END

SELECT @sSQL = @sSQL + 'ORDER BY tblOrders.Dealer, tblOrders.Model, tblOrders.OrderDate'

EXEC (@sSQL)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptDealerSales]  TO [fcuser]
GO

