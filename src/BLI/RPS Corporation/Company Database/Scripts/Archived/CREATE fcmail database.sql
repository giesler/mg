CREATE DATABASE fcmail
go

USE fcmail
go

ALTER TABLE [dbo].[tblMailContact] DROP CONSTRAINT FK_tblMailContact_tblMailTitle
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[spDuplicateNames]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spDuplicateNames]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[StoredProcedure1]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[StoredProcedure1]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[duplicate contacts]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[duplicate contacts]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[new titles]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[new titles]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblMailCompany]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblMailCompany]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblMailContact]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblMailContact]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblMailExclude]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblMailExclude]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblMailInclude]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblMailInclude]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[tblMailTitle]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblMailTitle]
GO

if not exists (select * from master..syslogins where name = N'fcuser')
BEGIN
	declare @logindb nvarchar(132), @loginlang nvarchar(132) select @logindb = N'fcdata', @loginlang = N'us_english'
	if @logindb is null or not exists (select * from master..sysdatabases where name = @logindb)
		select @logindb = N'master'
	if @loginlang is null or (not exists (select * from master..syslanguages where name = @loginlang) and @loginlang <> N'us_english')
		select @loginlang = @@language
	exec sp_addlogin N'fcuser', null, @logindb, @loginlang
END
GO

if not exists (select * from sysusers where name = N'fcuser' and uid < 16382)
	EXEC sp_grantdbaccess N'fcuser', N'fcuser'
GO

exec sp_addrolemember N'db_datareader', N'fcuser'
GO

exec sp_addrolemember N'db_datawriter', N'fcuser'
GO

CREATE TABLE [dbo].[tblMailCompany] (
	[CompID] [int] IDENTITY (1, 1) NOT NULL ,
	[MailState] [nvarchar] (2) NULL ,
	[AreaCode] [nvarchar] (3) NULL ,
	[MailCounty] [nvarchar] (30) NULL ,
	[ProductDesc] [nvarchar] (200) NULL ,
	[MailZip] [nvarchar] (10) NULL ,
	[PlantSqFt] [float] NULL ,
	[Employment] [float] NULL ,
	[MailAddress] [nvarchar] (30) NULL ,
	[Fax] [nvarchar] (12) NULL ,
	[CompName] [nvarchar] (30) NULL ,
	[SICDesc] [nvarchar] (95) NULL ,
	[SICCode] [float] NULL ,
	[Division] [nvarchar] (30) NULL ,
	[Phone800] [nvarchar] (12) NULL ,
	[HarrisID] [float] NULL ,
	[Phone] [nvarchar] (12) NULL ,
	[MailCity] [nvarchar] (30) NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblMailContact] (
	[ContactID] [int] IDENTITY (1, 1) NOT NULL ,
	[fkCompID] [int] NOT NULL ,
	[ContactName] [nvarchar] (50) NULL ,
	[fkContactTitleID] [int] NULL ,
	[DoNotMail] [bit] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblMailExclude] (
	[TitleID] [int] NOT NULL ,
	[Title] [nvarchar] (20) NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblMailInclude] (
	[TitleID] [int] NOT NULL ,
	[Title] [nvarchar] (20) NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tblMailTitle] (
	[apkTitle] [int] IDENTITY (1, 1) NOT NULL ,
	[TitleDesc] [nvarchar] (40) NULL ,
	[TitleBatch] [tinyint] NULL ,
	[TitleDelete] [bit] NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblMailCompany] WITH NOCHECK ADD 
	CONSTRAINT [PK_tblMailCompany] PRIMARY KEY  NONCLUSTERED 
	(
		[CompID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblMailContact] WITH NOCHECK ADD 
	CONSTRAINT [DF_tblContact_DoNotMail] DEFAULT (0) FOR [DoNotMail],
	CONSTRAINT [PK_tblMailContacts] PRIMARY KEY  NONCLUSTERED 
	(
		[ContactID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[tblMailTitle] WITH NOCHECK ADD 
	CONSTRAINT [PK_tblTitle] PRIMARY KEY  NONCLUSTERED 
	(
		[apkTitle]
	)  ON [PRIMARY] 
GO

 CREATE  INDEX [IX_tblMailCompany_MailState] ON [dbo].[tblMailCompany]([MailState]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_tblMailCompany_MailZip] ON [dbo].[tblMailCompany]([MailZip]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_tblMailCompany_Phone] ON [dbo].[tblMailCompany]([Phone]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_tblMailCompany_MailCounty] ON [dbo].[tblMailCompany]([MailCounty]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_tblContact_fkCompID] ON [dbo].[tblMailContact]([fkCompID]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_tblMailContact_fkContactTitleID] ON [dbo].[tblMailContact]([fkContactTitleID]) ON [PRIMARY]
GO

 CREATE  INDEX [IX_tblMailTitle] ON [dbo].[tblMailTitle]([TitleDesc]) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblMailContact] ADD 
	CONSTRAINT [FK_tblMailContact_tblMailTitle] FOREIGN KEY 
	(
		[fkContactTitleID]
	) REFERENCES [dbo].[tblMailTitle] (
		[apkTitle]
	)
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO

CREATE VIEW dbo.[duplicate contacts]
AS
SELECT fkCompID, ContactName, COUNT(ContactID) 
    AS Expr1
FROM tblContact
GROUP BY fkCompID, ContactName
HAVING (COUNT(ContactID) > 1)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO

CREATE VIEW dbo.[new titles]
AS
SELECT ContactTitle, COUNT(ContactID) AS [Count]
FROM tblContact
GROUP BY ContactTitle

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE spDuplicateNames
AS
SELECT --fkCompID, ContactName, COUNT(ContactID), 
MIN(ContactID) 
    AS ContactID
FROM tblContact
GROUP BY fkCompID, ContactName
HAVING (COUNT(ContactID) > 1)

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

SET QUOTED_IDENTIFIER  ON    SET ANSI_NULLS  ON 
GO

CREATE Procedure StoredProcedure1
/*
	(
		@parameter1 datatype = default value,
		@parameter2 datatype OUTPUT
	)
*/
As

INSERT INTO tblContacts ( fkCompID, ContactName, ContactTitle, ContactCat )
SELECT CompID, VPFULL, VPTITLE, 'VP' AS Expr1
FROM fch99


	return 


GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

