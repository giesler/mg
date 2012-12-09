<%

' Common vars for all pages
dim mstrPageTitle			' Page title that will be displayed at top of each page
dim mstrOffset				' Offset from root, in form '../' with trailing /
dim mblnLoginPage			' if page is a login page, so login validation doesn't happen
dim mblnDialog				' if page is a dialog/popup - use different styles on these pages
dim mblnShowMenu			' whether or not to use a different menu on this page
dim mstrPrintTitle		' printer friendly view title
dim mstrUserName			' currently logged in user name
dim mstrSessionId			' current session
dim mstrOnLoad				' any javascript to run onLoad for this page
dim mstrArea					' area of the website user is in

' User information
dim mstrEmail					' email address
if Request("ChangeEmail") <> "" then
	Response.Cookies("Email") = ""
	mstrEmail = ""
else
	mstrEmail = Request.Cookies("Email")
end if

' Default vars if nothing defined
mstrPageTitle  = "giesler.org"
mstrOffset     = ""
mblnLoginPage  = False
mblnDialog     = False
mblnShowMenu   = False
mstrPrintTitle = mstrPageTitle
mstrSessionId  = Request.Cookies("SessionId")
mstrOnLoad		 = ""
mstrArea			 = "default"

' Common declares used for all pages
dim gstrBodyTag, gstrTableTag
gstrBodyTag = "bgcolor=white text=black link=Red vlink=#ff0033 alink=Aqua leftmargin=""0"" topmargin=""0"" marginwidth=""0"" marginheight=""0"""
gstrTableTag = "border=""0"" cellpadding=""4"" cellspacing=""0"" class=""clsTable"" bgcolor=""black"""
	
' Debugging flag
dim mblnDebug
mblnDebug = False

%>
