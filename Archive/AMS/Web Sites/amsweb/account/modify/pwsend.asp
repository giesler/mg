<%@ Language=VBScript %>
<%
Option Explicit
dim strOffset, strErrs, cn, rs, objMail, blnDone, blnBadEmail
strOffset = "../../"
blnDone = false
blnBadEmail = false

' Check for form post, if so try to lookup and send email
if Request.Form("p") <> "" then
	set cn = Server.CreateObject("ADODB.Connection")
	cn.Open Application("cnString")
	set rs = cn.Execute("sp_webgetpassword '" & Replace(Request.Form("un"),"'","''") & "'")
	if rs.bof and rs.eof then
		blnBadEmail = true
	else
		set objMail = Server.CreateObject("CDONTS.NewMail")
		objMail.From = "AMS"
		objMail.To = rs.Fields("email")
		objMail.Subject = "AMS Account Information"
		objMail.Body = "Your AMS password is: " & rs.Fields("Password")
		objMail.Send
		set objMail = nothing
		blnDone = true
	end if
	rs.close
	set rs = nothing
	cn.Close
	set cn = nothing
end if
%>
<html>
<head>
<title>Adult Media Swapper - Retreive Password</title>
<link rel="stylesheet" type="text/css" href="../../ams.css">
</head>
<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" link=#E22000 vlink=#bf0400 alink=#ef1c19>

<!-- #include file="../../_header.asp"-->

<h3>Retreive Password</h3>

<% if blnDone then %>

<p>
Your password has been sent to your email address on file.
</p>

<p>
<a href="../modify/">Modify account information</a>
</p>

<% else %>

<p>
If you entered your email address when you registered with AMS, and can no longer remember your password, 
you can retreive your password by email.  Enter your user name below, then click 'Send Password'.
</p>

<% 
if blnBadEmail then
%>
<p class="req">
The user name you entered, '<i><%=Request.Form("un")%></i>' was either not found or did not have an email 
address on file.  If you didn't enter your email address when you registered, there is no way to retreive 
your password.
</p>
<%
end if
%>

<form method="post">
<input type="hidden" name="p" value="1">
<blockquote>
<table bgcolor="#f0f0f0" cellpadding="3">
	<tr>
		<td class="clsText">User Name:</td>
		<td><input type="text" name="un" value="<%=Request.Form("un")%>" maxlength=32></td>
	</tr><tr>
		<td colspan="2" align="right"><input type="submit" value="Send Password"></td>
	</tr>
</table>
</blockquote>
</form>

<% end if %>

<!-- #include file="../../_footer.asp"-->

</body>
</html>
