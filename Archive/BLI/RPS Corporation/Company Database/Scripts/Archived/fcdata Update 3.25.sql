-- fcdata update 3.25
-- - update rel notes, db version

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (1, 17, 'Tom Cat ISSA List', 1, '35')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (35, 0, 'Tom Cat ISSA List', 0, '')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (35, 0, 'Add/edit/view entries', 3, 'isfrmISSAList')
go


-- update tblISSA from dts task
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
CREATE TABLE dbo.Tmp_tblISSA
	(
 	ID int NOT NULL IDENTITY (1, 1),
	Code nvarchar(50) NULL,
	Title nvarchar(50) NULL,
	FirstName nvarchar(50) NULL,
	LastName nvarchar(50) NULL,
	Suffix nvarchar(20) NULL,
	Company nvarchar(75) NULL,
	Division nvarchar(75) NULL,
	Address nvarchar(75) NULL,
	Address2 nvarchar(75) NULL,
	City nvarchar(75) NULL,
	State nvarchar(20) NULL,
	Zip nvarchar(15) NULL,
	StateFullName nvarchar(50) NULL,
	Country nvarchar(50) NULL,
	Phone nvarchar(30) NULL,
	Fax nvarchar(30) NULL,
	TollFree nvarchar(30) NULL,
	Email nvarchar(50) NULL
	) ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblISSA ON
GO
IF EXISTS(SELECT * FROM dbo.tblISSA)
	 EXEC('INSERT INTO dbo.Tmp_tblISSA(ID, Code, Title, FirstName, LastName, Suffix, Company, Division, Address, Address2, City, State, Zip, StateFullName, Country, Phone, Fax, TollFree, Email)
		SELECT ID, Code, Title, FirstName, LastName, Suffix, Company, Division, Address, Address2, City, State, Zip, StateFullName, Country, Phone, Fax, TollFree, Email FROM dbo.tblISSA TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblISSA OFF
GO
DROP TABLE dbo.tblISSA
GO
EXECUTE sp_rename 'dbo.Tmp_tblISSA', 'tblISSA'
GO
ALTER TABLE dbo.tblISSA ADD CONSTRAINT
	PK_tblISSA PRIMARY KEY NONCLUSTERED 
	(
	ID
	) ON [PRIMARY]
GO
COMMIT

if exists (select * from sysobjects where id = object_id(N'[dbo].[wasprptWarrantyTotalCost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprptWarrantyTotalCost]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE wasprptWarrantyTotalCost
	@sDealerName varchar(40),
	@sFromDate varchar(20),
	@sToDate varchar(20),
	@sOpen varchar(2),
	@sFromDate2 varchar(20) = null,
	@sToDate2 varchar(20) = null
AS
DECLARE @sSQL varchar(1400)
DECLARE @sWhere varchar(300)
SELECT @sWhere = ''
SELECT @sSQL = '
SELECT dbo.tblWarranty.Dealer, dbo.tblWarranty.Model, dbo.tblWarranty.MachineSerialNumber, dbo.tblWarranty.DateOfFailure, dbo.tblWarranty.LaborCost, 
	SUM(dbo.tblWarrantyParts.PartCost) AS ExtPartCost, dbo.tblWarranty.Travel, dbo.tblWarranty.Hours
FROM dbo.tblWarranty INNER JOIN dbo.tblWarrantyParts ON dbo.tblWarranty.WarrantyID = dbo.tblWarrantyParts.fkWarrantyID '
SELECT @sWhere = ''
-- check dates
IF @sFromDate IS NOT NULL OR @sToDate IS NOT NULL OR @sDealerName IS NOT NULL OR @sOpen IS NOT NULL OR @sFromDate2 is not null or @sToDate2 is not null
  BEGIN
	-- check dates
	IF @sFromDate IS NOT NULL AND @sToDate  IS NOT NULL 
	  BEGIN
		SELECT @sWhere =' DateOfFailure Between ''' + @sFromDAte + ''' And ''' + @sToDAte + ''' '
	  END
  	ELSE IF @sFromDate IS NOT NULL
	  BEGIN
		SELECT @sWhere = 'DateOfFailure > ' + '''' + @sFromDate + ''''	
	  END
	ELSE IF @sToDate IS NOT NULL
	  BEGIN
		SELECT @sWhere = 'DateOfFailure < ' + '''' + @sToDate + ''''	
	  END
	-- check dates2
	IF @sFromDate2 IS NOT NULL AND @sToDate2 IS NOT NULL 
	  BEGIN
		IF @sWhere <> ''
			SELECT @sWhere = @sWhere + ' AND '
		SELECT @sWhere = @sWhere + ' DateEntered Between ''' + @sFromDAte2 + ''' And ''' + @sToDAte2 + ''' '
	  END
  	ELSE IF @sFromDate2 IS NOT NULL
	  BEGIN
		IF @sWhere <> ''
			SELECT @sWhere = @sWhere + ' AND '
		SELECT @sWhere = @sWhere + 'DateEntered > ' + '''' + @sFromDate2 + ''''	
	  END
	ELSE IF @sToDate2 IS NOT NULL
	  BEGIN
		IF @sWhere <> ''
			SELECT @sWhere = @sWhere + ' AND '
		SELECT @sWhere = @sWhere +  'DateEntered < ' + '''' + @sToDate2 + ''''	
	  END
	-- check dealer
	IF @sDealerName IS NOT NULL
	  BEGIN
		IF @sWhere <> ''
			SELECT @sWhere = @sWhere + ' AND '
		SELECT @sWhere = @sWhere + ' Dealer = ''' + @sDealerName + ''''
	  END
	-- check open flag
	IF @sOpen IS NOT NULL
	  BEGIN
		IF @sWhere <> ''
		  BEGIN
			SELECT @sWhere = @sWhere + ' AND '
		  END
		SELECT @sWhere = @sWhere + ' WarrantyOpen = ' + @sOpen
	  END
	SELECT @sSQL = @sSQL + ' WHERE ' + @sWhere 
  END
SELECT @sSQL = @sSQL + '
GROUP BY dbo.tblWarranty.Dealer, dbo.tblWarranty.Model, dbo.tblWarranty.MachineSerialNumber, dbo.tblWarranty.DateOfFailure, 
	dbo.tblWarranty.LaborCost, dbo.tblWarranty.Travel, dbo.tblWarranty.Hours'
EXEC (@sSQL)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptWarrantyTotalCost]  TO [fcuser]
GO


ALTER TABLE fcmail.dbo.tblMailContact ADD
	ListID smallint NULL CONSTRAINT DF_tblMailContact_ListID DEFAULT (0)
GO

CREATE NONCLUSTERED INDEX IX_tblMailContact_ListID ON fcmail.dbo.tblMailContact
	(
	ListID
	) ON [PRIMARY]
GO

UPDATE fcmail.dbo.tblMailContact SET ListID = 1
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[mlvContact]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[mlvContact]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE VIEW dbo.mlvContact
AS
SELECT ContactID, fkCompID, ContactName, fkContactTitleID, 
    DoNotMail, ListID
FROM fcmail.dbo.tblMailContact

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.25','1999-12-30 00:00:00','- Added Tom Cat ISSA file.
- Added ability to run ''Total Costs of Warranty'' report by Date Entered.
')
go

update tblDBProperties
set PropertyValue = '3.25'
where PropertyName = 'DBStructVersion'
go
