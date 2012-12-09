<%@ Language=VBScript %>
<%
Option Explicit
dim strOffset
strOffset = "../../"
dim cn, rs, strID, strValidateID, blnValidateOK

if Request("id") = "" then
	Response.Redirect("../")
end if
strID = Request("id")
if Request("vid") = "" then
	Response.Redirect("../?err=vid")
end if
strValidateID = Request("vid")

set cn = Server.CreateObject("ADODB.Connection")
cn.Open Application("cnString")
set rs = cn.Execute("sp_Web_SetVerifyEmail 0x" & strID & ", 0x" & strValidateID)
if rs.BOF and rs.EOF then
	blnValidateOK = false
elseif rs.Fields(0).Value = "0" then
	blnValidateOK = true
else
	blnValidateOK = true
end if
rs.Close
cn.Close

%>
<html>
	<head>
		<title>Adult Media Swapper - Email Address Verification</title>
		<link rel="stylesheet" type="text/css" href="../../ams.css">
	</head>
	<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" link="#E22000" vlink="#bf0400" alink="#ef1c19">
		<!-- #include file="../../_header.asp"-->
		<h3>
			Email Address Verification
		</h3>
		<% if blnValidateOK then %>
		<p>
			<b>Your email account has been verified.</b>
		</p>
		<p>
			If you would like to modify other account information, click <a href="../modify/?id=<%=strId%>">
				here</a>.
		</p>
		<% else %>
		<p>
			<b>The validation of your email address failed. Please try again by clicking <a href="../modify/verify.asp?id=<%=strId%>">
					here</a></b>
		</p>
		<% end if %>
		<!-- #include file="../../_footer.asp"-->
	</body>
</html>
