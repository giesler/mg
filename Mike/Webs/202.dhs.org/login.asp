<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "202 Login Page"
mstrOffset		 = ""
mblnLoginPage  = true
mstrOnLoad     = "document.frmLogin.Login.focus();"
%>
<!-- #include file="_common/gCommonCode.asp"-->

<%
on error resume next
' see if login attempt
dim strErrMsg, rs
strErrMsg = ""
if GetInput("submit") = "1" then
	set rs = Server.CreateObject("ADODB.Recordset")
	rs.Open "sp_PersonLogin '" + GetInput("Login") + "', '" + HashText(GetInput("Password")) + "'", cnWebDB, adOpenForwardOnly, adLockReadOnly
	if err then
		strErrMsg = "There was an error attempting to validate your login information.  The authentication system may not be available.  "
		strErrMsg = strErrMsg + "Please try again later.<br><font size=""-2"">" + Err.Source + "<br>" + Err.description + "</font><br>"
	elseif rs.EOF then
		strErrMsg = "You specified an invalid user name or password.<br>"
	else
		Response.Cookies("PersonID") = rs("PersonID")
		Response.Cookies("Name") = rs("Name")
		if Request.Form("refurl") <> "" then
			Response.Redirect(Request.Form("refurl"))
		else
			Response.Redirect("default.asp")
		end if
	end if
	rs.Close
	set rs = nothing
end if

%>

<html>
<head>
	<!--#include file="_common/gHeaderCode.asp"-->
	<title><%=mstrPageTitle%></title>	
</head>
<script language="JavaScript"><!--

function DoLogin() {
	document.all.item("spanLoginTable").style.display = "none";
	document.all.item("spanLoggingIn").style.display = "";
}

//--></script>

<body <%=GetBodyTag%>>

<!--#include file="_common/gPageHeader.asp"-->

<% if strErrMsg <> "" then %>
<p class="clsErrorText">
<%=strErrMsg%>
</p>
<% end if %>

<br><br>

<form name="frmLogin" method="post">
<input type="hidden" name="submit" value="1">
<input type="hidden" name="refurl" value="<%=GetInput("refurl")%>">
<table align="center" class="clsNormalTable" cellspacing="5" width=250>
	<tr>
		<td>
			<span id="spanLoginTable">
			<table align="center">
				<tr>
					<td>User ID</td><td><input name="Login" class="clsInputBox"></td>
				</tr><tr>
					<td>Password</td><td><input name="Password" type="Password" class="clsInputBox"></td>
				</tr><tr>
					<td align=right colspan=2><input type=submit value=Login class="clsButton" onClick="javascript:DoLogin()"></td>
				</tr>
			</table>
			</span>
			<span id="spanLoggingIn" style="display: none">
			<table align="center">
				<tr>
					<td colspan=3>&nbsp;</td>
				</tr><tr>
					<td>&nbsp;&nbsp;</td><td align="center">Logging in...</td><td>&nbsp;&nbsp;</td>
				</tr><tr>
					<td colSpan=3>&nbsp;</td>
				</tr>
			</table>
			</span>
		</td>
	</tr>
</table>
</form>

<!--#include file="_common/gPageFooter.asp"-->

</body>
</html>
