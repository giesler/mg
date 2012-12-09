-- fcdata update script 3.14  -  Oct 20
--  - adds table for prospects
--  - update menus for Tom Cat stuff
--  - updates sp ldsprptDealerLeads
--  - create sp  ldsprptResponseMethod
--  - create security and dbproperties tables
--  - create view tblTCLeads

use blidata
go

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
CREATE TABLE dbo.tblProspects
	(
 	ProspectID int NOT NULL IDENTITY (1, 1),
	CompanyName nvarchar(100) NULL,
	FirstName nvarchar(50) NULL,
	LastName nvarchar(50) NULL,
	StreetAddress nvarchar(100) NULL,
	StreetZipCode nvarchar(10) NULL,
	POBox nvarchar(20) NULL,
	POBoxZipCode nvarchar(10) NULL,
	City nvarchar(50) NULL,
	State char(20) NULL,
	Phone nvarchar(30) NULL,
	Fax nvarchar(30) NULL,
	Territory nvarchar(255) NULL,
	EquipmentCarried ntext NULL,
	Notes ntext NULL,
	RecordUpdateDate smalldatetime NULL
	) ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.tblProspects ADD CONSTRAINT
	PK_tblProspects PRIMARY KEY NONCLUSTERED 
	(
	ProspectID
	) ON [PRIMARY]
GO
COMMIT

----------------------------------------------------------
-- now update tblAllLeads for other lead types
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblAllLeads
	DROP CONSTRAINT DF__Temporary__Activ__3F466844
GO
ALTER TABLE dbo.tblAllLeads
	DROP CONSTRAINT DF__Temporary__Purch__403A8C7D
GO
CREATE TABLE dbo.Tmp_tblAllLeads
	(
 	LeadID int NOT NULL IDENTITY (1, 1),
	DealerName nvarchar(50) NULL,
	Location nvarchar(50) NULL,
	LeadDate datetime NULL,
	Salesman nvarchar(50) NULL,
	CompanyName nvarchar(100) NULL,
	Contact nvarchar(100) NULL,
	ContactTitle nvarchar(50) NULL,
	Address nvarchar(50) NULL,
	City nvarchar(20) NULL,
	State nvarchar(20) NULL,
	Zip nvarchar(20) NULL,
	Phone nvarchar(30) NULL,
	SIC nvarchar(20) NULL,
	Size nvarchar(20) NULL,
	ApplicationNotes ntext NULL,
	ResponseMethod nvarchar(20) NULL,
	Purchase nvarchar(50) NULL,
	Code nvarchar(50) NULL,
	ActiveInactive nchar(10) NULL CONSTRAINT DF__Temporary__Activ__3F466844 DEFAULT ('A'),
	Result ntext NULL,
	SendInfoOn nvarchar(50) NULL,
	DealerNumber nvarchar(50) NULL,
	Purchased tinyint NOT NULL CONSTRAINT DF__Temporary__Purch__403A8C7D DEFAULT (0),
	upsize_ts timestamp NULL
	) ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblAllLeads ON
GO
IF EXISTS(SELECT * FROM dbo.tblAllLeads)
	 EXEC('INSERT INTO dbo.Tmp_tblAllLeads(LeadID, DealerName, Location, LeadDate, Salesman, CompanyName, Contact, ContactTitle, Address, City, State, Zip, Phone, SIC, Size, ApplicationNotes, ResponseMethod, Purchase, Code, ActiveInactive, Result, SendInfoOn, DealerNumber, Purchased)
		SELECT LeadID, DealerName, Location, LeadDate, Salesman, CompanyName, Contact, ContactTitle, Address, City, State, Zip, Phone, SIC, Size, ApplicationNotes, ResponseMethod, Purchase, Code, ActiveInactive, Result, SendInfoOn, DealerNumber, CONVERT(tinyint, Purchased) FROM dbo.tblAllLeads TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblAllLeads OFF
GO
DROP TABLE dbo.tblAllLeads
GO
EXECUTE sp_rename 'dbo.Tmp_tblAllLeads', 'tblAllLeads'
GO
ALTER TABLE dbo.tblAllLeads ADD CONSTRAINT
	tblLeads_PK PRIMARY KEY NONCLUSTERED 
	(
	LeadID
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX active ON dbo.tblAllLeads
	(
	ActiveInactive
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX Code ON dbo.tblAllLeads
	(
	Code
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [company name] ON dbo.tblAllLeads
	(
	CompanyName
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [Date of Lead] ON dbo.tblAllLeads
	(
	LeadDate
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [Dealer Name] ON dbo.tblAllLeads
	(
	DealerName
	) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX phone ON dbo.tblAllLeads
	(
	Phone
	) ON [PRIMARY]
GO
CREATE TRIGGER "tblLeads_UTrig" ON dbo.tblAllLeads FOR UPDATE AS
SET NOCOUNT ON
IF (SELECT Count(*) FROM inserted WHERE NOT (ActiveInactive='A' Or ActiveInactive='I')) > 0
    BEGIN
        RAISERROR 44444 'You must enter either an A for Active or an I for Inactive in this field.'
        ROLLBACK TRANSACTION
    END
GO
CREATE TRIGGER "tblLeads_ITrig" ON dbo.tblAllLeads FOR INSERT AS
SET NOCOUNT ON
IF (SELECT Count(*) FROM inserted WHERE NOT (ActiveInactive='A' Or ActiveInactive='I')) > 0
    BEGIN
        RAISERROR 44444 'You must enter either an A for Active or an I for Inactive in this field.'
        ROLLBACK TRANSACTION
    END
GO
COMMIT


-- Main Menu
update dbo.[Switchboard Items]
set ItemNumber = 14
where ItemNumber = 12 and SwitchboardID = 1
go

update dbo.[Switchboard Items]
set ItemNumber = 15
where ItemNumber = 13 and SwitchboardID = 1
go

insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (1, 12, 'Tom Cat Leads', 1, '28')
go

insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (1, 13, 'Tom Cat Prospects', 1, '30')
go

-- Tom Cat Leads main menu
insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (28, 0, 'Tom Cat Leads', 0, '')
go

insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (28, 1, 'Add/Edit/View Tom Cat Leads', 3, 'ldtfrmLeads')
go

insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (28, 2, 'Tom Cat Leads Reports', 1, '29')
go

-- Tom Cat Leads reports
insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (29, 0, 'Tom Cat Leads Reports', 0, '')
go

insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (29, 1, 'Program Results - By Dealer', 4, 'ldtrptDealerLeads')
go

insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (29, 2, 'Program Results - By City', 4, 'ldtrptDealerLeadsCity')
go

insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (29, 3, 'Program Results - By Phone', 4, 'ldtrptDealerLeadsPhone')
go

insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (29, 4, 'Lead Page printout', 4, 'ldtrptDirectMailLead')
go

insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (29, 5, 'Lead labels', 4, 'ldtrptDirectMailLeadLabels')
go

insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (29, 6, 'Mailling Labels', 4, 'ldtrptMaillingLabels')
go

insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (29, 7, 'Response Method results', 4, 'ldtrptResponseMethod')
go

-- Tom Cat Prospects main menu
insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (30, 0, 'Tom Cat Prospects', 0, '')
go

insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (30, 1, 'Add/Edit/View Prospects', 3, 'tcfrmProspects')
go

insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (30, 2, 'Prospect Reports', 1, '31')
go

-- Tom Cat Prospects reports
insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (31, 0, 'Tom Cat Prospects Reports', 0, '')
go

insert into dbo.[Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (31, 1, 'Tom Cat Prospects', 4, 'tcrptProspectList')
go


-- update sp ldsprptDealerLeads
if exists (select * from sysobjects where id = object_id(N'[dbo].[ldsprptDealerLeads]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ldsprptDealerLeads]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE ldsprptDealerLeads
	@sFromDate varchar(20),
	@sToDate varchar(20),
	@sActiveInactive varchar(5),
	@sDealerName varchar(100),
	@iPurchased varchar(2)
AS
DECLARE @sSQL varchar(1500)
DECLARE @sWhere varchar(300)
SELECT @sWhere = ''
SELECT @sSQL = '
SELECT DealerName, CompanyName, LeadDate, Contact, State, 
    Phone, Result, City, ActiveInactive, SUBSTRING(Phone, 1, 3) 
    AS AreaCode, LeadID
FROM dbo.tblAllLeads
WHERE Purchased = ' + @iPurchased

IF @sFromDate IS NOT NULL OR @sToDate IS NOT NULL OR @sActiveInactive IS NOT NULL OR @sDealerName IS NOT NULL
  BEGIN
	IF @sFromDate IS NOT NULL And @sToDate IS NOT NULL
		SELECT @sWhere = 'LeadDate Between ''' + @sFromDate + ''' And ''' + @sToDate + ''' '
	ELSE IF @sFromDate IS NOT NULL 
		SELECT @sWhere = 'LeadDate > ''' + @sFromDate + ''' '
	ELSE IF @sToDate IS NOT NULL 
		SELECT @sWhere = 'LeadDate < ''' + @sToDate + ''' '
	IF @sActiveInactive IS NOT NULL 
	  BEGIN
		IF @sWhere <> ''
			SELECT @sWhere = @sWhere + ' AND '
		SELECT @sWhere = @sWhere + 'ActiveInactive = ''' +  @sActiveInactive + ''''
	  END
	IF @sDealerName IS NOT NULL 
	  BEGIN
		IF @sWhere <> ''
			SELECT @sWhere = @sWhere + ' AND '
		SELECT @sWhere = @sWhere + 'DealerName = ''' +  @sDealerName + ''''
	  END
	SELECT @sSQL = @sSQL + ' AND ' + @sWhere
END
EXEC (@sSQL)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ldsprptDealerLeads]  TO [fcuser]
GO


-- create new sp ldsprptResponseMethod
if exists (select * from sysobjects where id = object_id(N'[dbo].[ldsprptResponseMethod]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ldsprptResponseMethod]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE ldsprptResponseMethod
	@sFromDate varchar(20),
	@sToDate varchar(20),
	@sResponseMethod varchar(25),
	@iPurchased varchar(5)
AS

DECLARE @sSQL varchar(1500)
DECLARE @sWhere varchar(300)
SELECT @sWhere = ''
SELECT @sSQL = '
SELECT DealerName, CompanyName, LeadDate, Contact, State, Phone, Result, City, ResponseMethod
FROM tblAllLeads
WHERE Purchased = ' + @iPurchased

IF @sFromDate IS NOT NULL OR @sToDate IS NOT NULL OR @sResponseMethod IS NOT NULL
  BEGIN
	IF @sFromDate IS NOT NULL And @sToDate IS NOT NULL
		SELECT @sWhere = 'LeadDate Between ''' + @sFromDate + ''' And ''' + @sToDate + ''' '
	ELSE IF @sFromDate IS NOT NULL 
		SELECT @sWhere = 'LeadDate > ''' + @sFromDate + ''' '
	ELSE IF @sToDate IS NOT NULL 
		SELECT @sWhere = 'LeadDate < ''' + @sToDate + ''' '
	IF @sResponseMethod IS NOT NULL 
	  BEGIN
		IF @sWhere <> ''
			SELECT @sWhere = @sWhere + ' AND '
		SELECT @sWhere = @sWhere + 'ResponseMethod = ''' +  @sResponseMethod + ''''
	  END
	SELECT @sSQL = @sSQL + ' AND ' + @sWhere
END
EXEC (@sSQL)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ldsprptResponseMethod]  TO [fcuser]
GO

-- create security and dbproperties tables
if exists (select * from sysobjects where id = object_id(N'[dbo].[tblDBProperties]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblDBProperties]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblSecurity]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblSecurity]
GO

CREATE TABLE [dbo].[tblDBProperties] (
	[PropertyName] [nvarchar] (20) NOT NULL ,
	[PropertyValue] [nvarchar] (20) NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblSecurity] (
	[SecurityID] [int] IDENTITY (1, 1) NOT NULL ,
	[UserID] [nvarchar] (50) NOT NULL ,
	[SwID] [int] NOT NULL ,
	[AccessType] [int] NOT NULL ,
	[s] [char] (10) NULL 
) ON [PRIMARY]
GO

INSERT INTO tblDBProperties (PropertyName, PropertyValue)
VALUES ('DBStructVersion','3.14')
GO

INSERT INTO tblDBProperties (PropertyName, PropertyValue)
VALUES ('LaborRate','87.5')
GO

INSERT INTO tblDBProperties (PropertyName, PropertyValue)
VALUES ('TaxRate','5.1')
GO

INSERT INTO tblDBProperties (PropertyName, PropertyValue)
VALUES ('CostMarkup','25')
GO

CREATE VIEW dbo.tblTCLeads
AS
SELECT tblAllLeads.*
FROM tblAllLeads
WHERE (Purchased = 2)
GO