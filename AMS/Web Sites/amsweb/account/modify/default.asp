<%@ Language=VBScript %>
<%
Option Explicit
dim strOffset
strOffset = "../../"
dim cn, rs, blnBadLogin, strID, strLogin, strEmail, strSubsribe
if Request("id") = "" then
	Response.Redirect("../")
end if
strID = Request("id")

set cn = Server.CreateObject("ADODB.Connection")
cn.Open Application("cnString")
set rs = cn.Execute("sp_Web_GetAccountByID 0x" & strID)
if rs.BOF and rs.EOF then
	blnBadLogin = true
else
	strLogin = rs.Fields("Login")
	strEmail = rs.Fields("Email")
	strSubsribe = CInt(rs.Fields("Subsribe"))
end if
rs.Close
cn.Close

if blnBadLogin then response.Redirect("../?fail=1")


%>
<html>
	<head>
		<title>Adult Media Swapper - Modify Account</title>
		<link rel="stylesheet" type="text/css" href="../../ams.css">
	</head>
	<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" link="#E22000" vlink="#bf0400" alink="#ef1c19">
		<!-- #include file="../../_header.asp"-->
		<h3>
			Modify Account Information
		</h3>
		<p>
			<b>
				<%=strLogin%>
				, </b>what would you like to do?
		</p>
		<p>
			<a href="email.asp?id=<%=strID%>&email=<%=Server.URLEncode(strEmail)%>">Change the 
				email address AMS has on file (<b><%=strEmail%></b>)</a>
		</p>
		<p>
			<a href="pw.asp?id=<%=strID%>">Change my AMS password</a>
		</p>
		<p>
			<a href="subscribe.asp?id=<%=strID%>&subscribe=<%=strSubsribe%>">Change my mailling 
				preference</a>
		</p>
		<!-- #include file="../../_footer.asp"-->
	</body>
</html>
