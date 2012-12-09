-- blisql Update 5.16
-- - add acnt receive sp

if exists (select * from sysobjects where id = object_id(N'[dbo].[sprptAcntReceive]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sprptAcntReceive]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE sprptAcntReceive
AS
SELECT tc.CustName, ti.InvNumber, ti.InvDate, ti.InvTotal 
FROM tblCust tc INNER JOIN tblInvoice ti ON tc.apkCust = ti.afkInvCust 
WHERE (ti.InvPaymentReceivedFlag = 0) ORDER BY ti.InvDate
GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[sprptAcntReceive]  TO [bliuser]
GO

insert into tblVersion (VersionNumber, VersionDate, VersionRelNotes)
values ('5.16','2000-1-4 00:00:00','- Added Accounts Receivable report to the Unpaid Invoices screen.') 
go

update tblDBProperties
set PropertyValue = '5.16'
where PropertyName = 'DBStructVersion'
go

