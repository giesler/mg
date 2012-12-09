<%@ Language=VBScript %>
<%
dim sTitle, iOffset, bLogin, sOnLoad, bAutoLogin
if instr(Request.ServerVariables("PATH_INFO"), "dqdemo") > 0 then 
	sTitle = "Welcome to SAMI DQ"
elseif instr(Request.ServerVariables("PATH_INFO"), "bcdemo") > 0 then 
	sTitle = "Welcome to SAMI Background Checking"
	bAutoLogin = true
else
	sTitle = "Welcome to eSAMI.com"
end if
iOffset = 1
sOnLoad = "onLoad=""document.a.Login.focus();"""
bLogin = true
%>

<!-- #include file="../_include/header.inc"-->

<%

dim sMsg, sTmp, oBC, sLogin, sPassword
if bAutoLogin then
	sLogin = "demo"
	sPassword = "demo"
else
	sLogin = Request.Form("Login")
	sPassword = Request.Form("Password")
end if

if Request.Form("Login") <> "" or bAutoLogin then
	sTmp = dl("OrganizationContact", "ContactID", "Login = '" + sLogin + "' And Password = '" + sPassword + "'")
	if sTmp <> "" then
		set oBC = server.CreateObject("MSWC.BrowserType")
		Response.Cookies("eSAMI")("Browser") = oBC.Browser
		Response.Cookies("eSAMI")("Version") = oBC.Version
		Response.Cookies("eSAMI")("ContactID") = sTmp
		Response.Cookies("eSAMI")("OrganizationID") = dl("OrganizationContact", "OrganizationID", "Login = '" + sLogin + "'")
		if Request.Form("refurl") <> "" then
			Response.Redirect(Request.Form("refurl"))
		else
			Response.Redirect("../default.asp")
		end if
	else
		sMsg = "Invalid login/password."
	end if
else
	sMsg = "<br><br>Please enter your login and password for access to SAMI."
end if	

%>
<br><br>
<b><font color="Mediumblue">You are about to enter America's only fully 
integrated and web enabled Third Party Administrative Service.</font>
<br><br></b>
<%=sMsg%><br>

<!--
<table align=center border=1 cellpadding=5 cellspacing=0><tr bgcolor=#ffff99><td>
<b>Note:</b>  The graphics and colors are in the process of being converted to the new layout/design.  Please excuse the dust and keep in mind it is being worked on.
</td></tr></table>
-->

<form method="post" name=a>
<input type="hidden" name="refurl" value="<%=rq("refurl")%>">
<table align="center" border=0 cellpadding=3 cellspacing=0>
	<tr>
		<td>Login:</td>
		<td><input type="text" name="Login"></td>
	</tr><tr>
		<td>Password:</td>
		<td><input type="password" name="Password"></td>
	</tr><tr>
		<td colSpan=2 align="center"><input type="submit" value="Login"></td>
	</tr>
</table>


</form>

<!-- #include file="../_include/footer.inc"-->