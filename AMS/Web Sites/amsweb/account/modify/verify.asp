<%@ Language=VBScript %>
<%
Option Explicit
dim strOffset
strOffset = "../../"
dim cn, rs, blnBadLogin, strID, strEmail, strValidationID
if Request("id") = "" then
	Response.Redirect("../")
end if
strID = Request("id")

set cn = Server.CreateObject("ADODB.Connection")
cn.Open Application("cnString")
set rs = cn.Execute("sp_Web_GetVerifyInfo 0x" & strID)
if rs.BOF and rs.EOF then
	blnBadLogin = true
else
	strEmail = rs.Fields("Email")
	strValidationID = rs.Fields("ValidationID")
	
	' send an email if we have an email address
	if Len(strEmail) > 0 then
		'send email	
		dim objMail, strBody
		strBody = "<html><body>"
		strBody = strBody & "<h3>AMS Email Verification</h3>"
		strBody = strBody & "<p>This message is simply validating your email address.</p>"
		strBody = strBody & "<p>Simply click <a href="""
		strBody = strBody & Application("VerifyURL") & "?id=" & strId & "&vid=" & Mid(strValidationID, 3)
		strBody = strBody & """>here</a> to validate this is your correct email address.</p>"
		strBody = strBody & "</body></html>"
	
		set objMail = Server.CreateObject("BLIMAIL.SMTP")
		objMail.SendMail Application("strSMTPServer"), "maillist@adultmediaswapper.com", strEmail, "AMS Email Verification", strBody, "text/html"
		set objMail = nothing
	end if
	
end if
rs.Close
cn.Close

if blnBadLogin then response.Redirect("../?err=login")


%>
<html>
	<head>
		<title>Adult Media Swapper - Verify Email Address</title>
		<link rel="stylesheet" type="text/css" href="../../ams.css">
	</head>
	<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" link="#E22000" vlink="#bf0400" alink="#ef1c19">
		<!-- #include file="../../_header.asp"-->
		<h3>
			Verify Email Address
		</h3>
		<% if Len(strEmail) = 0 then %>
		<p>
			You must set your email address before we can send you an email to verify it. 
			Please click <a href="../modify/?id=<%=strId%>">here</a> to update your email 
			address.
		</p>
		<% else %>
		<p>
			A verification email has been sent to <b>
				<%=strEmail%>
			</b>.
			<br>
			<a href="default.asp?id=<%=strID%>">Return to Account home</a>
		</p>
		<% end if %>
		<!-- #include file="../../_footer.asp"-->
	</body>
</html>
