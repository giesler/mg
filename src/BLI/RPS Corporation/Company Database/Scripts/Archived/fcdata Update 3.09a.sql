if exists (select * from sysobjects where id = object_id(N'[dbo].[pasprptBillOfMaterials]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[pasprptBillOfMaterials]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE pasprptBillOfMaterials
	@sVendorName varchar(50),
	@sModel varchar(10)

 AS

DECLARE @sSQL varchar(1500)
DECLARE @sWhere varchar(500)
SELECT @sWhere = ''

SELECT @sSQL = 
'SELECT p.RPSPartNum, p.PartName, p.VendorName, p.CostEach, p.Quantity, 
	CONVERT (smallmoney, CONVERT (smallmoney, p.CostEach) * CONVERT (int, m.Quantity) ) AS ExtCost
FROM dbo.tblParts p, dbo.tblPartsModels m
WHERE p.PartID = m.fkPartID AND m.Quantity > 0 '

-- Check model
IF @sModel IS NOT NULL
	SELECT @sWhere = ' m.Model = ' + @sModel + ' '

-- Check vendor
IF @sVendorName IS NOT NULL
  BEGIN
	IF @sWhere <> '' 
		SELECT @sWhere = @sWhere + ' AND '
	SELECT @sWhere = @sWhere +  'p.VendorName = ''' + @sVendorName + ''''
	SELECT @sWhere = @sWhere + ' AND p.RPSPartNum IN ('
	SELECT @sWhere = @sWhere + '
		SELECT pop.RPSPartNum
		FROM dbo.tblPO po, dbo.tblPOPart pop, dbo.tblPOPartDetail popd
		WHERE po.POID = pop.fkPOID AND pop.POPartID = popd.fkPOPartID
			AND popd.RequestedShipDate IS NOT NULL
			AND popd.ReceivedDate IS NULL )'
  END

IF @sWhere <> ''
  BEGIN
	SELECT @sSQL = @sSQL + ' AND ' + @sWhere
  END

EXEC (@sSQL)












GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[pasprptBillOfMaterials]  TO [fcuser]
GO

