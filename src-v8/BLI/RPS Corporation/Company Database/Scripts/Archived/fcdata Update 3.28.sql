-- fcdata update 3.28
-- - update rel notes, db version
-- - change dealer table

----------------------------------------------------------------------------------------------
-- tc dealers
----------------------------------------------------------------------------------------------

EXECUTE sp_rename 'dbo.tblDealers', 'tblAllDealers'
GO

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblAllDealers
	DROP CONSTRAINT DF__Temporary__Curre__1B0907CE
GO
ALTER TABLE dbo.tblAllDealers
	DROP CONSTRAINT DF__Temporary__Sweep__1BFD2C07
GO
ALTER TABLE dbo.tblAllDealers
	DROP CONSTRAINT DF__Temporary__Labor__1CF15040
GO
CREATE TABLE dbo.Tmp_tblAllDealers
	(
 	DealerID int NOT NULL IDENTITY (1, 1),
	CurrentDealer bit NOT NULL CONSTRAINT DF__Temporary__Curre__1B0907CE DEFAULT (0),
	DealerName nvarchar(100) NULL,
	TollFreeNumber nvarchar(20) NULL,
	ContactName nvarchar(50) NULL,
	ContactTitle nvarchar(50) NULL,
	StreetAddress nvarchar(50) NULL,
	City nvarchar(50) NULL,
	State nvarchar(20) NULL,
	Zip nvarchar(20) NULL,
	Phone nvarchar(20) NULL,
	Fax nvarchar(20) NULL,
	Num nvarchar(20) NULL,
	SweeperDealer bit NOT NULL CONSTRAINT DF__Temporary__Sweep__1BFD2C07 DEFAULT (0),
	CarPhoneNumber nvarchar(20) NULL,
	TerritoryCovered nvarchar(100) NULL,
	NumSalesman nvarchar(20) NULL,
	MajorProducts nvarchar(100) NULL,
	SalesmensNames nvarchar(100) NULL,
	FirstName nvarchar(50) NULL,
	LastName nvarchar(50) NULL,
	HomePhoneNumber nvarchar(20) NULL,
	Notes ntext NULL,
	ContractExpires datetime NULL,
	SalesmanName nvarchar(50) NULL,
	WarrentyAdministrator nvarchar(50) NULL,
	ServiceManagerName nvarchar(50) NULL,
	LaborRate money NULL CONSTRAINT DF__Temporary__Labor__1CF15040 DEFAULT (0),
	PartsManagerName nvarchar(50) NULL,
	OfficeManagerName nvarchar(50) NULL,
	Terms nvarchar(50) NULL,
	DealerType tinyint NOT NULL CONSTRAINT DF_tblAllDealers_DealerType DEFAULT (0)
	) ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblAllDealers ON
GO
IF EXISTS(SELECT * FROM dbo.tblAllDealers)
	 EXEC('INSERT INTO dbo.Tmp_tblAllDealers(DealerID, CurrentDealer, DealerName, TollFreeNumber, ContactName, ContactTitle, StreetAddress, City, State, Zip, Phone, Fax, Num, SweeperDealer, CarPhoneNumber, TerritoryCovered, NumSalesman, MajorProducts, SalesmensNames, FirstName, LastName, HomePhoneNumber, Notes, ContractExpires, SalesmanName, WarrentyAdministrator, ServiceManagerName, LaborRate, PartsManagerName, OfficeManagerName, Terms)
		SELECT DealerID, CurrentDealer, DealerName, TollFreeNumber, ContactName, ContactTitle, StreetAddress, City, State, Zip, Phone, Fax, Num, SweeperDealer, CarPhoneNumber, TerritoryCovered, NumSalesman, MajorProducts, SalesmensNames, FirstName, LastName, HomePhoneNumber, Notes, ContractExpires, SalesmanName, WarrentyAdministrator, ServiceManagerName, LaborRate, PartsManagerName, OfficeManagerName, Terms FROM dbo.tblAllDealers TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblAllDealers OFF
GO
ALTER TABLE dbo.tblDealerAdvert
	DROP CONSTRAINT tblDealerAdvert_FK00
GO
ALTER TABLE dbo.tblDealersGoals
	DROP CONSTRAINT tblDealersGoals_FK00
GO
DROP TABLE dbo.tblAllDealers
GO
EXECUTE sp_rename 'dbo.Tmp_tblAllDealers', 'tblAllDealers'
GO
ALTER TABLE dbo.tblAllDealers ADD CONSTRAINT
	aaaaatblDealers_PK PRIMARY KEY NONCLUSTERED 
	(
	DealerID
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX City ON dbo.tblAllDealers
	(
	City
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX Num ON dbo.tblAllDealers
	(
	Num
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX NumSalesman ON dbo.tblAllDealers
	(
	NumSalesman
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX phone ON dbo.tblAllDealers
	(
	Phone
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX Sort1 ON dbo.tblAllDealers
	(
	DealerName
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblDealersGoals WITH NOCHECK ADD CONSTRAINT
	tblDealersGoals_FK00 FOREIGN KEY
	(
	fkDealerID
	) REFERENCES dbo.tblAllDealers
	(
	DealerID
	)
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblDealerAdvert WITH NOCHECK ADD CONSTRAINT
	tblDealerAdvert_FK00 FOREIGN KEY
	(
	fkDealerID
	) REFERENCES dbo.tblAllDealers
	(
	DealerID
	)
GO
COMMIT

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblDealers]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[tblDealers]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblTCDealers]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[tblTCDealers]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO

CREATE VIEW dbo.tblDealers
AS
SELECT tblAllDealers.*
FROM tblAllDealers
WHERE (DealerType = 0)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO

CREATE VIEW dbo.tblTCDealers
AS
SELECT tblAllDealers.*
FROM tblAllDealers
WHERE (DealerType = 1)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

drop view dbo.dlqrptDealerListAddressView
go

drop view dbo.dlqrptDealerListAllDealersView
go

drop view dbo.dlqrptDealerListFaxView
go

drop view dbo.dlqrptDealerListPhoneView
go

drop view dbo.dlqrptMaillingLabelsView
go

drop view dbo.dlqrptMaillingLabelServicePartsManView
go

drop view dbo.dlqrptMaillingServiceView
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (1, 14, 'Tom Cat Dealers', 1, '36')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (36, 0, 'Tom Cat Dealers', 0, '')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (36, 1, 'Open TC Dealers listing', 3, 'dltfrmDealers')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (36, 2, 'TC Dealer Reports', 1, '37')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (37, 0, 'Tom Cat Dealer Reports', 0, '')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (37, 1, 'Dealer List - Contact Name and Phone Number', 4, 'dltrptDealerListPhone')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (37, 2, 'Dealer List - Contact Name and Address', 4, 'dltrptDealerListAddress')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (37, 3, 'Dealer List - Contact Name and Fax Numbers', 4, 'dltrptDealerListFax')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (37, 4, 'Current Dealers Mailing labels', 4, 'dltrptMailingLabels')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (37, 5, 'Current Dealers End Contract Date - By Date', 4, 'dltrptDealerContractDate')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (37, 6, 'Current Dealers End Contract Date - By Dealer', 4, 'dltrptDealerContractAlpha')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (37, 7, 'Current Dealers Mailing labels incuding Phone Number', 4, 'dltrptMailingLabelsPhone')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (37, 8, 'Mailling labels for specific titles', 4, 'dltrptMailingLabelsNames')
go


if exists (select * from sysobjects where id = object_id(N'[dbo].[dlqfrmDealerGoals]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqfrmDealerGoals]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[dlqrptDealerContractAlpha]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqrptDealerContractAlpha]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[dlqrptDealerContractDate]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqrptDealerContractDate]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[dlqrptDealerListAddress]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqrptDealerListAddress]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[dlqrptDealerListAllDealers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqrptDealerListAllDealers]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[dlqrptDealerListFax]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqrptDealerListFax]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[dlqrptDealerListPhone]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqrptDealerListPhone]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[dlqrptMaillingLabels]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqrptMaillingLabels]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[dlqrptMaillingLabelServicePartsMan]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqrptMaillingLabelServicePartsMan]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[dlqrptMaillingService]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[dlqrptMaillingService]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO



CREATE PROCEDURE dlqfrmDealerGoals
@DealerID int
AS
SELECT fkDealerID, [Year], Model, Goal
FROM tblDealersGoals
WHERE fkDealerID = @DealerID
ORDER BY [Year] DESC, Model





GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqfrmDealerGoals]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE Procedure dlqrptDealerContractAlpha
	@iDealerType int = 0
AS

SELECT DealerName, ContactName, StreetAddress, City, State, Zip, CurrentDealer, ContractExpires
FROM tblAllDealers
WHERE CurrentDealer = 1 AND DealerType = @iDealerType
ORDER BY DealerName, ContactName	

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptDealerContractAlpha]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE Procedure dlqrptDealerContractDate
	@iDealerType int = 0
AS

SELECT DealerName, ContactName, StreetAddress, City, State, Zip, CurrentDealer, ContractExpires
FROM tblAllDealers
WHERE CurrentDealer = 1 AND DealerType = @iDealerType
ORDER BY ContractExpires, DealerName, ContactName	

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptDealerContractDate]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE dlqrptDealerListAddress
	@iDealerType int = 0
AS

SELECT DealerName, ContactName, StreetAddress, City, State, Zip, CurrentDealer, ContractExpires
FROM tblAllDealers
WHERE CurrentDealer = 1 AND DealerType = @iDealerType
ORDER BY DealerName, ContactName

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptDealerListAddress]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE dlqrptDealerListAllDealers
	@iDealerType int = 0
AS

SELECT DealerName, ContactName, City, State, Phone, TollFreeNumber
FROM tblAllDealers
WHERE DealerType = @iDealerType
ORDER BY DealerName, ContactName

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptDealerListAllDealers]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE dlqrptDealerListFax
	@iDealerType int = 0
AS
SELECT DealerName, ContactName, City, State, Fax, TerritoryCovered, CurrentDealer
FROM tblAllDealers
WHERE CurrentDealer = 1 AND DealerType = @iDealerType
ORDER BY DealerName, ContactName




GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptDealerListFax]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE dlqrptDealerListPhone
	@iDealerType int = 0
AS

SELECT DealerName, ContactName, City, State, Phone, TollFreeNumber, CurrentDealer
FROM tblAllDealers
WHERE CurrentDealer = 1 AND DealerType = @iDealerType
ORDER BY DealerName, ContactName
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptDealerListPhone]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE dlqrptMaillingLabels
	@iDealerType int = 0
AS
SELECT DealerName, ContactName AS Name, ContactName AS PersonName, StreetAddress, City, State, Zip, CurrentDealer, Phone, TollFreeNumber
FROM tblAllDealers
WHERE CurrentDealer = 1 AND DealerType = @iDealerType
ORDER BY DealerName




GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptMaillingLabels]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE dlqrptMaillingLabelServicePartsMan
	@iDealerType int = 0
AS

SELECT dl.DealerName, nm.PersonName, dl.StreetAddress, dl.City, dl.State, dl.Zip, dl.CurrentDealer, dl.Phone, dl.ContactName
FROM tmpNames nm INNER JOIN tblAllDealers dl ON (nm.DealerIndex=dl.DealerID)
WHERE dl.CurrentDealer=1 AND dl.DealerType = @iDealerType
ORDER BY dl.DealerName



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptMaillingLabelServicePartsMan]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE dlqrptMaillingService
	@iDealerType int = 0
AS
SELECT DealerName, ServiceManagerName AS PersonName, StreetAddress, City, State, Zip, CurrentDealer, Phone, ContactName
FROM tblAllDealers
WHERE CurrentDealer=1 AND DealerType = @iDealerType
ORDER BY DealerName



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[dlqrptMaillingService]  TO [fcuser]
GO



----------------------------------------------------------------------------------------------
-- tc major accounts
----------------------------------------------------------------------------------------------

EXECUTE sp_rename 'dbo.tblMajorAccounts', 'tblAllMajorAcnts'
GO

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblAllMajorAcnts
	DROP CONSTRAINT DF__Temporary__fkMAD__4BAC3F29
GO
ALTER TABLE dbo.tblAllMajorAcnts
	DROP CONSTRAINT DF__Temporary__MANum__4CA06362
GO
ALTER TABLE dbo.tblAllMajorAcnts
	DROP CONSTRAINT DF__Temporary__MAApp__4D94879B
GO
ALTER TABLE dbo.tblAllMajorAcnts
	DROP CONSTRAINT DF__Temporary__MADen__4E88ABD4
GO
ALTER TABLE dbo.tblAllMajorAcnts
	DROP CONSTRAINT DF__Temporary__MAAcc__4F7CD00D
GO
CREATE TABLE dbo.Tmp_tblAllMajorAcnts
	(
 	MajorAccountID int NOT NULL IDENTITY (1, 1),
	fkMADealerID int NULL CONSTRAINT DF__Temporary__fkMAD__4BAC3F29 DEFAULT (0),
	MACompName nvarchar(50) NULL,
	MAHeadqAddress nvarchar(50) NULL,
	MACity nvarchar(30) NULL,
	MAState nvarchar(2) NULL,
	MAZip nvarchar(10) NULL,
	MAPurContact nvarchar(50) NULL,
	MAManageContact nvarchar(50) NULL,
	MAPhone nvarchar(20) NULL,
	MAFax nvarchar(20) NULL,
	MANumLocations int NULL CONSTRAINT DF__Temporary__MANum__4CA06362 DEFAULT (0),
	MALocations nvarchar(20) NULL,
	MASerialNums ntext NULL,
	MAInitialPO ntext NULL,
	MAApproved bit NOT NULL CONSTRAINT DF__Temporary__MAApp__4D94879B DEFAULT (0),
	MADenied bit NOT NULL CONSTRAINT DF__Temporary__MADen__4E88ABD4 DEFAULT (0),
	MAAccountNum nvarchar(15) NULL,
	MAAccountNumKey int NULL CONSTRAINT DF__Temporary__MAAcc__4F7CD00D DEFAULT (0),
	MANotes ntext NULL,
	MADateApproved datetime NULL,
	MAType tinyint NOT NULL CONSTRAINT DF_tblAllMajorAcnts_MAType DEFAULT (0)
	) ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblAllMajorAcnts ON
GO
IF EXISTS(SELECT * FROM dbo.tblAllMajorAcnts)
	 EXEC('INSERT INTO dbo.Tmp_tblAllMajorAcnts(MajorAccountID, fkMADealerID, MACompName, MAHeadqAddress, MACity, MAState, MAZip, MAPurContact, MAManageContact, MAPhone, MAFax, MANumLocations, MALocations, MASerialNums, MAInitialPO, MAApproved, MADenied, MAAccountNum, MAAccountNumKey, MANotes, MADateApproved)
		SELECT MajorAccountID, fkMADealerID, MACompName, MAHeadqAddress, MACity, MAState, MAZip, MAPurContact, MAManageContact, MAPhone, MAFax, MANumLocations, MALocations, MASerialNums, MAInitialPO, MAApproved, MADenied, MAAccountNum, MAAccountNumKey, MANotes, MADateApproved FROM dbo.tblAllMajorAcnts TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblAllMajorAcnts OFF
GO
DROP TABLE dbo.tblAllMajorAcnts
GO
EXECUTE sp_rename 'dbo.Tmp_tblAllMajorAcnts', 'tblAllMajorAcnts'
GO
ALTER TABLE dbo.tblAllMajorAcnts ADD CONSTRAINT
	aaaaatblMajorAccounts_PK PRIMARY KEY NONCLUSTERED 
	(
	MajorAccountID
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX fkMADealerID ON dbo.tblAllMajorAcnts
	(
	fkMADealerID
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX MAAccountNum ON dbo.tblAllMajorAcnts
	(
	MAAccountNum
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX MAAccountNumKey ON dbo.tblAllMajorAcnts
	(
	MAAccountNumKey
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX VendorName ON dbo.tblAllMajorAcnts
	(
	MACompName
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblAllMajorAcnts_MAType ON dbo.tblAllMajorAcnts
	(
	MAType
	) ON [PRIMARY]
GO
COMMIT


if exists (select * from sysobjects where id = object_id(N'[dbo].[tblMajorAccounts]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[tblMajorAccounts]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblTCMajorAccounts]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[tblTCMajorAccounts]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO

CREATE VIEW dbo.tblMajorAccounts
AS
SELECT tblAllMajorAcnts.*
FROM tblAllMajorAcnts
WHERE (MAType = 0)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO

CREATE VIEW dbo.tblTCMajorAccounts
AS
SELECT tblAllMajorAcnts.*
FROM tblAllMajorAcnts
WHERE (MAType = 1)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

update [Switchboard Items]
set ItemNumber = ItemNumber - 1
where SwitchboardID = 1 and (ItemNumber = 17 or ItemNumber = 18)
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (1, 18, 'Tom Cat Major Accounts', 1, '38')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (38, 0, 'Tom Cat Major Accounts', 0, '')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (38, 1, 'Add/edit/view TC Major Account information', 3, 'matfrmMajorAccounts')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (38, 2, 'TC Major Account List (report)', 4, 'matrptList')
go


if exists (select * from sysobjects where id = object_id(N'[dbo].[maqrptMajorAccountInfoPage]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[maqrptMajorAccountInfoPage]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE VIEW dbo.maqrptMajorAccountInfoPage
AS
SELECT dbo.tblAllMajorAcnts.*,
    dbo.tblAllDealers.DealerName, 
    dbo.tblAllDealers.StreetAddress, 
    dbo.tblAllDealers.City, 
    dbo.tblAllDealers.State, 
    dbo.tblAllDealers.Zip
FROM dbo.tblAllMajorAcnts INNER JOIN
    dbo.tblAllDealers ON 
    dbo.tblAllMajorAcnts.fkMADealerID = dbo.tblAllDealers.DealerID
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


----------------------------------------------------------------------------------------------
-- tc warranty
----------------------------------------------------------------------------------------------

EXECUTE sp_rename 'dbo.tblWarranty', 'tblAllWarranty'
GO

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblAllWarranty
	DROP CONSTRAINT DF_tblWarranty_DateEntered
GO
CREATE TABLE dbo.Tmp_tblAllWarranty
	(
 	WarrantyID int NOT NULL IDENTITY (1, 1),
	MachineSerialNumber nvarchar(50) NULL,
	DateOfFailure datetime NULL,
	CreditMemoNum nvarchar(50) NULL,
	CreditMemoAmt money NULL,
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
	DateEntered datetime NULL CONSTRAINT DF_tblWarranty_DateEntered DEFAULT (getdate()),
	PartReceived bit NULL,
	Hours real NULL,
	fkDealerID smallint NULL,
	Comment ntext NULL,
	WarrantyType tinyint NOT NULL CONSTRAINT DF_tblAllWarranty_WarrantyType DEFAULT (0)
	) ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblAllWarranty ON
GO
IF EXISTS(SELECT * FROM dbo.tblAllWarranty)
	 EXEC('INSERT INTO dbo.Tmp_tblAllWarranty(WarrantyID, MachineSerialNumber, DateOfFailure, CreditMemoNum, CreditMemoAmt, Dealer, Customer, RGANum, PartCost, LaborCost, Freight, Problem, Model, Resolution, WarrantyOpen, Travel, Policy, DateEntered, PartReceived, Hours, fkDealerID, Comment)
		SELECT WarrantyID, MachineSerialNumber, DateOfFailure, CreditMemoNum, CreditMemoAmt, Dealer, Customer, RGANum, PartCost, LaborCost, Freight, Problem, Model, Resolution, WarrantyOpen, Travel, Policy, DateEntered, PartReceived, Hours, fkDealerID, Comment FROM dbo.tblAllWarranty TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblAllWarranty OFF
GO
ALTER TABLE dbo.tblWarrantyParts
	DROP CONSTRAINT FK_tblWarrantyParts_tblWarranty
GO
DROP TABLE dbo.tblAllWarranty
GO
EXECUTE sp_rename 'dbo.Tmp_tblAllWarranty', 'tblAllWarranty'
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
	) ON [PRIMARY]
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblWarrantyParts WITH NOCHECK ADD CONSTRAINT
	FK_tblWarrantyParts_tblWarranty FOREIGN KEY
	(
	fkWarrantyID
	) REFERENCES dbo.tblAllWarranty
	(
	WarrantyID
	)
GO
COMMIT

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblTCWarranty]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[tblTCWarranty]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblWarranty]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[tblWarranty]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO

CREATE VIEW dbo.tblTCWarranty
AS
SELECT tblAllWarranty.*
FROM tblAllWarranty
WHERE (WarrantyType = 1)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO

CREATE VIEW dbo.tblWarranty
AS
SELECT tblAllWarranty.*
FROM tblAllWarranty
WHERE (WarrantyType = 0)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


if exists (select * from sysobjects where id = object_id(N'[dbo].[waqrptRgaClaim]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[waqrptRgaClaim]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE VIEW dbo.waqrptRgaClaim
AS
SELECT dl.DealerName, dl.StreetAddress, dl.City, dl.State, dl.Zip, 
	wa.Customer, wa.DateOfFailure, wa.RGANum, wa.WarrantyID, 
	wa.Comment, wa.Problem, wa.Resolution, wa.MachineSerialNumber, wa.DateEntered
FROM dbo.tblAllDealers dl INNER JOIN dbo.tblAllWarranty wa ON dl.DealerID = wa.fkDealerID

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


update [Switchboard Items]
set ItemNumber = ItemNumber - 1
where SwitchboardID = 1 and (ItemNumber > 15)
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (1, 20, 'Tom Cat Warranty', 1, '39')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (39, 0, 'Tom Cat Warranty', 0, '')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (39, 1, 'Add/edit/view TC warranty listings', 3, 'watfrmWarranty')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (39, 2, 'Tom Cat Warranty Reports', 1, '40')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (40, 0, 'Tom Cat Warranty Reports', 0, '')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (40, 1, 'Warranty Costs', 4, 'watrptWarrantyCosts')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (40, 2, 'Warranty Report', 4, 'watrptWarrantyReport')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (40, 3, 'Warranty by Part Number', 4, 'watrptWarrantyPN')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (40, 4, 'Warranty Claim RGA Tickets', 4, 'watrptRgaClaimDates')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (40, 5, 'Open warranty claims report', 4, 'watrptWarrantyRGANums')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (40, 6, 'Open warranty Pending Report - by Serial Number', 4, 'watrptWarrantyPendingSN')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (40, 7, 'Open warranty Pending Report - by Dealer', 4, 'watrptWarrantyPendingDealer')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (40, 8, 'Total cost of warranty report', 4, 'watrptWarrantyTotalCost')
go

insert into [Switchboard Items] (SwitchboardID, ItemNumber, ItemText, Command, Argument)
values (40, 9, 'Dealer Warranty Reimbursement', 4, 'watrptDealerReimburse')
go


-- update leads indexes
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
	Purchased tinyint NOT NULL CONSTRAINT DF__Temporary__Purch__403A8C7D DEFAULT (0)
	) ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblAllLeads ON
GO
IF EXISTS(SELECT * FROM dbo.tblAllLeads)
	 EXEC('INSERT INTO dbo.Tmp_tblAllLeads(LeadID, DealerName, Location, LeadDate, Salesman, CompanyName, Contact, ContactTitle, Address, City, State, Zip, Phone, SIC, Size, ApplicationNotes, ResponseMethod, Purchase, Code, ActiveInactive, Result, SendInfoOn, DealerNumber, Purchased)
		SELECT LeadID, DealerName, Location, LeadDate, Salesman, CompanyName, Contact, ContactTitle, Address, City, State, Zip, Phone, SIC, Size, ApplicationNotes, ResponseMethod, Purchase, Code, ActiveInactive, Result, SendInfoOn, DealerNumber, Purchased FROM dbo.tblAllLeads TABLOCKX')
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
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX active ON dbo.tblAllLeads
	(
	ActiveInactive
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX Code ON dbo.tblAllLeads
	(
	Code
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [company name] ON dbo.tblAllLeads
	(
	CompanyName
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [Date of Lead] ON dbo.tblAllLeads
	(
	LeadDate
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [Dealer Name] ON dbo.tblAllLeads
	(
	DealerName
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX phone ON dbo.tblAllLeads
	(
	Phone
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX IX_tblAllLeads_Purchase ON dbo.tblAllLeads
	(
	Purchase
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


if exists (select * from sysobjects where id = object_id(N'[dbo].[tblLeads]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[tblLeads]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblPurLeads]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[tblPurLeads]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblTCLeads]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[tblTCLeads]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE VIEW dbo.tblLeads
AS
SELECT tblAllLeads.*
FROM tblAllLeads
WHERE (Purchased = 0)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE VIEW dbo.tblPurLeads
AS
SELECT tblAllLeads.*
FROM tblAllLeads
WHERE (Purchased = 1)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE VIEW dbo.tblTCLeads
AS
SELECT tblAllLeads.*
FROM tblAllLeads
WHERE (Purchased = 2)
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
CREATE NONCLUSTERED INDEX IX_tblAllOrders ON dbo.tblAllOrders
	(
	OrderType
	) ON [PRIMARY]
GO
COMMIT

if exists (select * from sysobjects where id = object_id(N'[dbo].[waqrptWarrantyRGANums]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[waqrptWarrantyRGANums]
GO

drop view waqrptWarrantyPending
go


if exists (select * from sysobjects where id = object_id(N'[dbo].[wasprptDealerReimburse]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprptDealerReimburse]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[wasprptRgaClaimDates]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprptRgaClaimDates]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[wasprptWarrantyCosts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprptWarrantyCosts]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[wasprptWarrantyPending]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprptWarrantyPending]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[wasprptWarrantyRGA]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprptWarrantyRGA]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[wasprptWarrantyTotalCost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprptWarrantyTotalCost]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[wasprsubRGAParts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[wasprsubRGAParts]
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE wasprptDealerReimburse 
	@sDealerName varchar(40) = null,
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@sOpen varchar(2) = null, 
	@iWarrantyType int = 0
AS

-- Check input parameters
IF @sDealerName IS NULL
	SELECT @sDealerName = '%'
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @sOpen IS NULL
	SELECT @sOpen = '%'
IF @iWarrantyType IS NULL
	SELECT @iWarrantyType = 0

-- Run query
SELECT wa.Dealer, wa.Model, wa.MachineSerialNumber, wa.CreditMemoNum, wa.CreditMemoAmt, 
	wa.RGANum, wa.DateOfFailure, wa.WarrantyOpen
FROM dbo.tblWarrantyParts wp INNER JOIN dbo.tblAllWarranty wa ON wp.fkWarrantyID = wa.WarrantyID
WHERE wa.DateOfFailure Between @sFromDate and @sToDate
	and wa.Dealer Like @sDealerName
	and wa.WarrantyOpen Like @sOpen
	and wa.WarrantyType = @iWarrantyType

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptDealerReimburse]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE wasprptRgaClaimDates
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@iWarrantyType int = 0
AS

-- Check input parameters
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @iWarrantyType IS NULL
	SELECT @iWarrantyType = 0

SELECT dl.DealerName, dl.StreetAddress, dl.City, dl.State, dl.Zip, 
	dl.City + quotename(', ','''') + dl.State + quotename('  ', '''') + dl.Zip AS CityStateZip,
	wa.Customer, wa.DateOfFailure, wa.RGANum, wa.DateEntered, wa.Comment, wa.WarrantyID, 
	wa.MachineSerialNumber, wa.Problem, wa.Resolution
FROM dbo.tblAllDealers dl INNER JOIN dbo.tblAllWarranty wa ON dl.DealerID = wa.fkDealerID
WHERE wa.DateEntered Between @sFromDate And @sToDate
	AND wa.WarrantyType = @iWarrantyType
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptRgaClaimDates]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE wasprptWarrantyCosts 
	@sPartName varchar(40) = null,
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@sOpen varchar(2) = null,
	@iWarrantyType int = 0
AS

-- Check input parameters
IF @sPartName IS NULL
	SELECT @sPartName = '%'
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @sOpen IS NULL
	SELECT @sOpen = '%'
IF @iWarrantyType IS NULL
	SELECT @iWarrantyType = 0

-- Run query

SELECT wa.MachineSerialNumber, wa.DateOfFailure, wa.CreditMemoNum, wa.Dealer, wa.Customer, wa.RGANum, wa.LaborCost, 
	wa.Freight, wa.Problem, wp.PartNumReplaced, wp.PartDescription, wa.WarrantyID, pa.PartName, wp.PartCost, wa.Model, wa.Hours, 
	wa.WarrantyOpen
FROM dbo.tblParts pa RIGHT JOIN (dbo.tblWarrantyParts wp INNER JOIN dbo.tblAllWarranty wa ON 
		wp.fkWarrantyID = wa.WarrantyID) ON pa.PartID = wp.PartFileIndex
WHERE wa.DateOfFailure Between @sFromDate and @sToDate
	and wp.PartNumReplaced like @sPartName
	and wa.WarrantyOpen like @sOpen
	and wa.WarrantyType = @iWarrantyType


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptWarrantyCosts]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE wasprptWarrantyPending
	@iWarrantyType int = 0
AS

SELECT wa.Dealer, wa.MachineSerialNumber, wa.RGANum, wp.PartNumReplaced, wa.WarrantyOpen, wa.Hours
FROM dbo.tblWarranty wa INNER JOIN  dbo.tblWarrantyParts wp ON wa.WarrantyID = wp.fkWarrantyID
WHERE wa.WarrantyOpen = 1 
	and wa.WarrantyType = @iWarrantyType

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptWarrantyPending]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE wasprptWarrantyRGA
	@iWarrantyType int = 0
AS

SELECT wa.RGANum, wa.MachineSerialNumber, wa.DateOfFailure, wa.CreditMemoNum, wa.Dealer, wa.Customer, wa.LaborCost, 
	wa.Freight, wa.Problem, wp.PartNumReplaced, wp.PartDescription, wa.WarrantyID, pa.PartName, wp.PartCost, wa.WarrantyOpen 
FROM dbo.tblParts pa RIGHT OUTER JOIN dbo.tblWarrantyParts wp INNER JOIN dbo.tblAllWarranty wa ON 
	wp.fkWarrantyID = wa.WarrantyID ON pa.PartID = wp.PartFileIndex 
WHERE wa.WarrantyOpen = 1 and wa.WarrantyType = @iWarrantyType

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptWarrantyRGA]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE wasprptWarrantyTotalCost
	@sDealerName varchar(40) = null,
	@sFromDate varchar(20) = null,
	@sToDate varchar(20) = null,
	@sOpen varchar(2) = null,
	@sFromDate2 varchar(20) = null,
	@sToDate2 varchar(20) = null,
	@iWarrantyType int = 0
AS

-- Check input parameters
IF @sDealerName IS NULL
	SELECT @sDealerName = '%'
IF @sFromDate IS NULL
	SELECT @sFromDate = '1/1/1900'
IF @sToDate IS NULL
	SELECT @sToDate = '1/1/2100'
IF @sOpen IS NULL
	SELECT @sOpen = '%'
IF @sFromDate2 IS NULL
	SELECT @sFromDate2 = '1/1/1900'
IF @sToDate2 IS NULL
	SELECT @sToDate2 = '1/1/2100'
IF @iWarrantyType IS NULL
	SELECT @iWarrantyType = 0

-- Run query
SELECT wa.Dealer, wa.Model, wa.MachineSerialNumber, wa.DateOfFailure, wa.LaborCost, 
	SUM(wp.PartCost) AS ExtPartCost, wa.Travel, wa.Hours
FROM dbo.tblAllWarranty wa INNER JOIN dbo.tblWarrantyParts wp ON wa.WarrantyID = wp.fkWarrantyID 
WHERE wa.DateOfFailure Between @sFromDate and @sToDate
	and DateEntered Between @sFromDate2 and @sToDate2
	and wa.Dealer like @sDealerName
	and wa.WarrantyOpen like @sOpen
	and wa.WarrantyType = @iWarrantyType
GROUP BY wa.Dealer, wa.Model, wa.MachineSerialNumber, wa.DateOfFailure, wa.LaborCost, wa.Travel, wa.Hours


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprptWarrantyTotalCost]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE wasprsubRGAParts
	@sID varchar(10) = null
AS

IF @sID = null
	SELECT @sID = 0

SELECT wp.PartNumReplaced, pa.PartName, wp.fkWarrantyID
FROM dbo.tblParts pa INNER JOIN dbo.tblWarrantyParts wp ON (pa.RPSPartNum = wp.PartNumReplaced)
WHERE wp.fkWarrantyID = @sID




GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[wasprsubRGAParts]  TO [fcuser]
GO


BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.tblProdSchedItems
	DROP CONSTRAINT DF__Temporary__Sched__0E6E26BF
GO
ALTER TABLE dbo.tblProdSchedItems
	DROP CONSTRAINT DF__Temporary__Model__0F624AF8
GO
ALTER TABLE dbo.tblProdSchedItems
	DROP CONSTRAINT DF__Temporary__Quant__10566F31
GO
CREATE TABLE dbo.Tmp_tblProdSchedItems
	(
 	ScheduleID int NOT NULL CONSTRAINT DF__Temporary__Sched__0E6E26BF DEFAULT (0),
	Model nvarchar(20) NOT NULL CONSTRAINT DF__Temporary__Model__0F624AF8 DEFAULT (0),
	Quantity smallint NOT NULL CONSTRAINT DF__Temporary__Quant__10566F31 DEFAULT (0),
	SchedItemID int NOT NULL IDENTITY (1, 1)
	) ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.Tmp_tblProdSchedItems ON
GO
IF EXISTS(SELECT * FROM dbo.tblProdSchedItems)
	 EXEC('INSERT INTO dbo.Tmp_tblProdSchedItems(ScheduleID, Model, Quantity, SchedItemID)
		SELECT ScheduleID, Model, Quantity, SchedItemID FROM dbo.tblProdSchedItems TABLOCKX')
GO
SET IDENTITY_INSERT dbo.Tmp_tblProdSchedItems OFF
GO
DROP TABLE dbo.tblProdSchedItems
GO
EXECUTE sp_rename 'dbo.Tmp_tblProdSchedItems', 'tblProdSchedItems'
GO
ALTER TABLE dbo.tblProdSchedItems ADD CONSTRAINT
	tblProdSchedItems_PK PRIMARY KEY NONCLUSTERED 
	(
	ScheduleID,
	Model,
	SchedItemID
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX SchedItemID ON dbo.tblProdSchedItems
	(
	SchedItemID
	) WITH FILLFACTOR = 90 ON [PRIMARY]
GO
COMMIT


insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('3.28','2000-2-3 00:00:00','- Added Tom Cat Dealers
- Added Tom Cat Major Accounts
- Improved performance of Orders, Tom Cat Orders, Leads, Purchased Leads, and Tom Cat Leads
- Added Tom Cat Warranty')
go

update tblDBProperties
set PropertyValue = '3.28'
where PropertyName = 'DBStructVersion'
go
