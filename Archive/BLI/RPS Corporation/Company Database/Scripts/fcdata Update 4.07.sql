-- fcdata update 4.07


ALTER TABLE dbo.tblPartsModels ADD
	SpareParts smallint NOT NULL CONSTRAINT DF_tblPartsModels_SpareParts DEFAULT 0
GO
DROP INDEX dbo.tblPartsModels.fkPartID
GO
CREATE CLUSTERED INDEX fkPartID ON dbo.tblPartsModels
	(
	fkPartID
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[paspfrmPartsModels]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[paspfrmPartsModels]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE paspfrmPartsModels
	@iPartID int = 0
AS
select PartModelID, Model, Quantity, Optional, SpareParts
from dbo.tblPartsModels
where fkPartID = @iPartID
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[paspfrmPartsModels]  TO [fcuser]
GO

update [Switchboard Items]
set ItemNumber = ItemNumber + 1
where SwitchboardID = 17 and ItemNumber > 11
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (17, 12, 'Production Parts listing by Model - Option Parts', '4', 'parptProdPartsByModelOpt')
go

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[orsprptDealerOrders]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptDealerOrders]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE orsprptDealerOrders
	@sDealerName varchar(40) = null,
	@bNullShipDate varchar(20) = 0,
	@iOrderType varchar(3) = 0
AS

-- Run query
SELECT dl.DealerName, ao.Model, ao.OrderDate, ao.PromisedDate, ao.PurchaseOrder
FROM tblAllOrders ao 
	INNER JOIN tblAllDealers dl ON ao.fkDealerID = dl.DealerID
WHERE (dl.CurrentDealer = 1) AND ao.OrderType = @iOrderType
	AND CASE
			WHEN @sDealerName IS NULL THEN 1
			WHEN dl.DealerName = @sDealerName THEN 1
			ELSE 0 END = 1
	AND CASE
			WHEN @bNullShipDate = 0 THEN 1
			WHEN ao.ShippedDate IS NULL THEN 1 ELSE 0
		END = 1
ORDER BY dl.DealerName, ao.PurchaseOrder
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

ALTER TABLE dbo.tblAllLeads ADD
	LeadAction nvarchar(30) NULL,
	Prospect nvarchar(30) NULL
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[orsprptSalesRepCommission]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptSalesRepCommission]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE dbo.orsprptSalesRepCommission
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null
AS 
-- Check input parameters
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
--- Run query
SELECT o.Model, o.Dealer, o.Quantity, o.SalePrice, o.OrderNumber, o.OrderType, r.DealerRepName
FROM tblAllOrders o
	INNER JOIN tblAllDealers d ON d.DealerID = o.fkDealerID
	INNER JOIN tlkuDealerRep r ON r.DealerRepID = d.DealerRepID
WHERE o.SalePrice <> 0
	AND (o.OrderDate Between @sFromDate and @sToDate)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptSalesRepCommission]  TO [fcuser]
GO

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('4.07','2000-10-29','- Fixed Database Security Admin program not working
- TC Warranty:  Fixed ''Dealer Ref #'' and ''Problem'' fields not working
- FC/TC Warranty:  Fixed other problems with adding warranty records and parts
- Parts:  Added ''Spare Parts'' field to Model dialog
- Parts:  Added ''Prod Parts listed by Model - Option Parts'' report
- Parts:  Fixed error when entering a new vendor on a part
- TC Leads:  Fixed Lead Page Printout report showing FC dealers in criteria screen
- FC/TC Orders:  Updated ''Dealer Orders'' report to allow choosing to include only orders with null ship date
- All Leads:  Added ''Action'' drop down and ''Prospect'' dropdown.  Also updated ''Current Record'' printout
- Orders:  Fixed ''Salesman Commission Report''
')
go

update tblDBProperties
set PropertyValue = '4.07'
where PropertyName = 'DBStructVersion'
go

