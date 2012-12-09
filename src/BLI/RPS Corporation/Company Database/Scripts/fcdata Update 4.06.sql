-- fcdata update 4.06


ALTER TABLE dbo.tblParts ADD
	Weight nvarchar(100) NULL
GO


DROP INDEX dbo.tblParts.rpspn
GO
DROP INDEX dbo.tblParts.IX_tblParts_RPSPNSort
GO
CREATE UNIQUE NONCLUSTERED INDEX rpspn ON dbo.tblParts
	(
	RPSPartNum
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE CLUSTERED INDEX IX_tblParts_RPSPNSort ON dbo.tblParts
	(
	RPSPNSort
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO


update dbo.[Switchboard Items]
set Argument = 'lsrptStatesDealerlist'
where ID = 49
go

update dbo.[Switchboard Items]
set Argument = 'lsrptStatesLastMailed'
where ID = 48
go



update [Switchboard Items]
set ItemNumber = ItemNumber + 1
where SwitchboardID = 14 and ItemNumber > 3
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (14, 4, 'Dealer Orders', '4', 'orrptDealerOrders')
go


update [Switchboard Items]
set ItemNumber = ItemNumber + 1
where SwitchboardID = 33 and ItemNumber > 3
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument, OpenArgs)
values (33, 4, 'Dealer Orders', '4', 'orrptDealerOrders', 'TomCatMode')
go


EXECUTE sp_rename N'dbo.tblNewsletter', N'tblAllNewsletter', 'OBJECT'
GO
ALTER TABLE dbo.tblAllNewsletter ADD
	NewsletterType tinyint NULL
GO
ALTER TABLE dbo.tblAllNewsletter ADD CONSTRAINT
	DF_tblAllNewsletter_NewsletterType DEFAULT 0 FOR NewsletterType
GO
ALTER TABLE dbo.tblAllNewsletter
	DROP CONSTRAINT aaaaatblNewsletter_PK
GO
ALTER TABLE dbo.tblAllNewsletter ADD CONSTRAINT
	tblNewsletter_PK PRIMARY KEY NONCLUSTERED 
	(
	NewsletterID
	) WITH FILLFACTOR = 90 ON [PRIMARY]

GO
ALTER TABLE dbo.tblAllNewsletter
	DROP COLUMN upsize_ts
GO

update dbo.tblAllNewsletter
set NewsletterType=0
go

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[orsprptDealerOrders]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[orsprptDealerOrders]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE orsprptDealerOrders
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
SELECT dl.DealerName, ao.Model, ao.OrderDate, ao.PromisedDate, ao.PurchaseOrder
FROM tblAllOrders ao 
	INNER JOIN tblAllDealers dl ON ao.fkDealerID = dl.DealerID
WHERE (dl.CurrentDealer = 1) AND ao.OrderType = @iOrderType
	AND (ao.Dealer like @sDealerName)
	AND (ao.OrderDate Between @sFromDate And @sToDate)
ORDER BY dl.DealerName, ao.PurchaseOrder
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[orsprptDealerOrders]  TO [fcuser]
GO

update [Switchboard Items]
set ItemNumber = ItemNumber + 1
where SwitchboardID = 41 and ItemNumber > 4
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (41, 5, 'Tom Cat Newsletters', '1', '43')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (43, 0, 'Tom Cat Newsletters', '0', '')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (43, 1, 'Add/edit/view listings', '3', 'nwtfrmNewsletter')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (43, 2, 'Address Listing', '4', 'nwtrptAddressListing')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (43, 3, 'Mailling Labels', '4', 'nwtrptMailingLabels')
go


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[nwqrptAddressListing]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[nwqrptAddressListing]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[nwqrptMailingLabels]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[nwqrptMailingLabels]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[nwqrptAddressListingView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[nwqrptAddressListingView]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[nwqrptMailingLabelsView]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[nwqrptMailingLabelsView]
GO



insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('4.06','2000-10-19','- Lists:  Fixed reports not showing dealer names
- TC Leads:  Shows Tom Cat dealers, not Factory Cat dealers
- Parts:  Added ''Weight'' field to Misc tab.  Also improved load time
- Fixed Filter by Form not working in FC&TC Orders, Parts, Lists, BSCAI, ISSA, FC&TC Major Accounts, Maillist, Prospects, and Vendors
- FC&TC Warranty:  Fixed dealer lists showing all FC dealers to show appropriate dealer lists
- Leads:  Pulls Contact name and City to Salesman and Location fields when a dealer name is selected
- Lists:  Added ''Notes'' field to Dealer report
- Lists:  Swapped two reports that were labelled wrong on the menu
- FC&TC Orders:  Added ''Dealer Orders'' report
- Added ''Tom Cat Newsletters'' section
- FC&TC Warranty:  Fixed deleting records problem
')
go

update tblDBProperties
set PropertyValue = '4.06'
where PropertyName = 'DBStructVersion'
go

