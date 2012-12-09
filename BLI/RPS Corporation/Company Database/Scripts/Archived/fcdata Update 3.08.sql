if exists (select * from sysobjects where id = object_id(N'[dbo].[ldqrptDirectMailLead]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ldqrptDirectMailLead]
GO

if exists (select * from sysobjects where id = object_id(N'[dbo].[ldsprptDealerLeads]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ldsprptDealerLeads]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO


CREATE PROCEDURE ldqrptDirectMailLead
	@sFromDate varchar(20),
	@sToDate varchar(20),
	@sDealerName varchar(50),
	@sCompanyName varchar(50),
	@iPurchased int
AS

DECLARE @sSQL varchar(1000)
DECLARE @sWhere varchar(200)
SELECT @sWhere = ''

SELECT @sSQL = '
SELECT DealerName, Salesman, Location, CompanyName, LeadDate, Contact, Address, City, State, Zip, ApplicationNotes, Result, ContactTitle, Phone
FROM tblAllLeads
WHERE Purchased = ' + STR(@iPurchased,1,0)

IF @sFromDate IS NOT NULL OR @sToDate IS NOT NULL OR @sDealerName IS NOT NULL OR @sCompanyName IS NOT NULL
  BEGIN
	IF @sFromDate IS NOT NULL AND @sToDate  IS NOT NULL 
		SELECT @sWhere = 'LeadDate Between ''' + @sFromDate + ''' And ''' + @sToDAte + ''' '
  	ELSE IF @sFromDate IS NOT NULL
		SELECT @sWhere =  'LeadDate > ' + '''' + @sFromDate + ''''	
	ELSE IF @sToDate IS NOT NULL
		SELECT @sWhere =  'LeadDate < ' + '''' + @sToDate + ''''	
	IF @sDealerName IS NOT NULL
	  BEGIN
		IF @sWhere <> ''
			SELECT @sWhere = @sWhere + ' AND '
		SELECT @sWhere = @sWhere + ' DealerName = ''' + @sDealerName + ''''
	  END
	IF @sCompanyName IS NOT NULL
	  BEGIN
		IF @sWhere <> ''
			SELECT @sWhere = @sWhere + ' AND '
		SELECT @sWhere = @sWhere + ' CompanyName Like ''%' + @sCompanyName + '%'''
	  END
	SELECT @sSQL = @sSQL + ' AND ' + @sWhere
  END

EXEC (@sSQL)





GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ldqrptDirectMailLead]  TO [fcuser]
GO

SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

CREATE PROCEDURE ldsprptDealerLeads
	@sFromDate varchar(20),
	@sToDate varchar(20),
	@sActiveInactive varchar(5),
	@sDealerName varchar(100)
AS

DECLARE @sSQL varchar(1500)
DECLARE @sWhere varchar(300)
SELECT @sWhere = ''

SELECT @sSQL = '
SELECT DealerName, CompanyName, LeadDate, Contact, State, 
    Phone, Result, City, ActiveInactive, SUBSTRING(Phone, 1, 3) 
    AS AreaCode
FROM dbo.tblAllLeads
WHERE Purchased = 0
'

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
		SELECT @sWhere = 'ActiveInactive = ''' +  @sActiveInactive + ''''
	  END
	IF @sDealerName IS NOT NULL 
	  BEGIN
		IF @sWhere <> ''
			SELECT @sWhere = @sWhere + ' AND '
		SELECT @sWhere = 'DealerName = ''' +  @sDealerName + ''''
	  END
	SELECT @sSQL = @sSQL + ' AND ' + @sWhere
END

EXEC (@sSQL)



GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

GRANT  EXECUTE  ON [dbo].[ldsprptDealerLeads]  TO [fcuser]
GO

