<%@ Language=VBScript %>
<%
Option Explicit
dim strOffset
strOffset = "../../"

dim blnGoodLogin, strErrs, blnSaved, rs, cn
blnGoodLogin = false
strErrs = ""
blnSaved = false

' see if we are logging in
if Request.Form("l") <> "" then
	set cn = Server.CreateObject("ADODB.Connection")
	cn.Open Application("cnString")
	set rs = cn.execute("sp_webgetaccountinfo '" & Replace(Request.Form("un"),"'","''") & "', '" & Replace(Request.Form("pwd"),"'","''") & "'")
	if rs.bof and rs.eof then
		' the login and password weren't found
		Response.Redirect("../modify/?badlogin")
	else	
		blnGoodLogin = true
	end if
else
	Response.Redirect("../modify/")
end if


%>
<html>
<head>
<title>Adult Media Swapper - Modify Account</title>
<link rel="stylesheet" type="text/css" href="../../ams.css">
</head>
<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" link=#E22000 vlink=#bf0400 alink=#ef1c19>

<!-- #include file="../../_header.asp"-->

<h3>Modify Account</h3>

<p>
Change any of the values below, then click 'Save' to save changes.  To change your password, enter your 
password in both fields.
</p>

<form method="post" name="f" id="f">
<input type="hidden" name="p" value="1">

<blockquote>
<table bgcolor="#f0f0f0" width="300" cellpadding="3">
	<tr>
		<td class="clsText">User name: <font class="req">*</font></td>
		<td><input type="hidden" name="un" value="<%=rs.Fields("Login")%>"><%=rs.Fields("Login")%></td>
	</tr><tr>
		<td class="clsText">Password: <font class="req">*</font></td>
		<td><input type="password" name="pwd" maxlength=32></td>
	</tr><tr>
		<td class="clsText">Confirm Password: <font class="req">*</font></td>
		<td><input type="password" name="cpwd" maxlength=32></td>
	</tr><tr>
		<td class="clsText">Email Address: </td>
		<td><input type="text" name="email" value="<%=rs.Fields("email")%>" maxlength=64></td>
	</tr><tr>
		<td colspan="2"><img src="../../img/trans.gif" height="10"></td>
	</tr><tr>
		<td colspan="2" align="right"><input type="submit" value="Save" onclick="return checkform()" id=submit1 name=submit1></td>
	</tr>
</table>

</blockquote>
</form>

<!-- #include file="../../_footer.asp"-->

</body>
</html>
