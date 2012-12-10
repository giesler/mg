<%@ Language=VBScript %>
<%
Option Explicit
dim strOffset
strOffset = "../../"
%>
<html>
<head>
<title>Adult Media Swapper - Modify Account</title>
<link rel="stylesheet" type="text/css" href="../../ams.css">
</head>
<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" link=#E22000 vlink=#bf0400 alink=#ef1c19>

<!-- #include file="../../_header.asp"-->

<h3>Modify Account Information</h3>

<% if false then %>
<p>
If you already have an AMS account, enter your AMS username and password below to modify your account 
information.  If you need a new account, click <a href="../../download/new.asp">here</a>.

<% if instr(Request.QueryString, "badlogin") > 0 then %>
<p class="req">
Your username or password is incorrect.  Please try loggging on again.  If you forgot your password, 
click <a href="pwsend.asp">here</a>.
</p>
<% end if %>

<form method="post" action="modify.asp">
<input type="hidden" name="l" value="1">
<blockquote>
<table bgcolor="#f0f0f0" cellpadding="3">
	<tr>
		<td class="clsText">User name:</td>
		<td><input type="text" name="un" value="<%=Request.Form("un")%>" maxlength=32></td>
	</tr><tr>
		<td class="clsText">Password:</td>
		<td><input type="password" name="pwd" maxlength=32></td>
	</tr><tr>
		<td colspan="2" align="right"><input type="submit" value="Login"></td>
	</tr><tr>
		<td colspan="2" class="clsText"><a href="pwsend.asp">Forgot your password?</a></td>
	</tr>
</table>
</blockquote>
</form>

<% end if %>

<p>
This area is currently under construction.
</p>

<!-- #include file="../../_footer.asp"-->

</body>
</html>
