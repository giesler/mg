-- Update 3.13 for fcdata
-- oct 20, 1999
update [Switchboard Items]
set Argument = 'ldprptDealerLeads'
where SwitchboardID = 25 and ItemNumber = 1
go

update [Switchboard Items]
set Argument = 'ldprptDealerLeadsCity'
where SwitchboardID = 25 and ItemNumber = 2
go

update [Switchboard Items]
set Argument = 'ldprptDealerLeadsPhone'
where SwitchboardID = 25 and ItemNumber = 3
go

update [Switchboard Items]
set ItemText = 'Purchased Leads Report', Argument = 'ldprptDirectMailLead'
where SwitchboardID = 25 and ItemNumber = 4
go

update [Switchboard Items]
set ItemText = 'Purchased Leads Labels', Argument = 'ldprptDirectMailLeadLabels'
where SwitchboardID = 25 and ItemNumber = 5
go

update [Switchboard Items]
set ItemText = 'Mailling Labels for Purchased Leads', Argument = 'ldprptMaillingLabels'
where SwitchboardID = 25 and ItemNumber = 6
go

update [Switchboard Items]
set Argument = 'ldprptReponseMethod'
where SwitchboardID = 25 and ItemNumber = 7
go


BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.[Switchboard Items]
	DROP CONSTRAINT DF__Temporary__ItemN__1273C1CD
GO
CREATE TABLE dbo.[Tmp_Switchboard Items]
	(
 	ID int NOT NULL IDENTITY (1, 1),
	SwitchboardID int NOT NULL,
	ItemNumber smallint NOT NULL CONSTRAINT DF__Temporary__ItemN__1273C1CD DEFAULT (0),
	ItemText nvarchar(255) NULL,
	Command smallint NULL,
	Argument nvarchar(255) NULL,
	Tooltip nvarchar(100) NULL,
	OpenArgs nvarchar(100) NULL
	) ON [PRIMARY]
GO
SET IDENTITY_INSERT dbo.[Tmp_Switchboard Items] OFF
GO
IF EXISTS(SELECT * FROM dbo.[Switchboard Items])
	 EXEC('INSERT INTO dbo.[Tmp_Switchboard Items](SwitchboardID, ItemNumber, ItemText, Command, Argument, Tooltip, OpenArgs)
		SELECT SwitchboardID, ItemNumber, ItemText, Command, Argument, Tooltip, OpenArgs FROM dbo.[Switchboard Items] TABLOCKX')
GO
DROP TABLE dbo.[Switchboard Items]
GO
EXECUTE sp_rename 'dbo.[Tmp_Switchboard Items]', 'Switchboard Items'
GO
ALTER TABLE dbo.[Switchboard Items] ADD CONSTRAINT
	[aaaaaSwitchboard Items_PK] PRIMARY KEY NONCLUSTERED 
	(
	SwitchboardID,
	ItemNumber
	) ON [PRIMARY]
GO
COMMIT

if exists (select * from sysobjects where id = object_id(N'[dbo].[vSwitchboard]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].[vSwitchboard]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE VIEW dbo.vSwitchboard
AS
SELECT [Switchboard Items].*
FROM [Switchboard Items]

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO
