<%
dim sTitle, iOffset, sSubTitle, sOnLoad, sFile
sTitle = "Download"
sSubTitle = "VB Setup Wrapper"
iOffset = 1

select case Request.Form("Version")
	case "1.00"
		sFile = "vbsw_1-00.zip"
	case "1.01"
		sFile = "vbsw_1-01.zip"
	case else
		sFile = "vbsw_1-01.zip"
end select

if Request.Form("dl") = "1" then
	sOnLoad = "onLoad=""dlfile();"""
end if

%>

<SCRIPT LANGUAGE=Javascript>
<!--
function dlfile() {
	window.location = "<%=sFile%>";
}
-->
</SCRIPT>

<!-- #include file="../_common/header.inc"-->

<%
on error resume next
if Request.Form("dl") = "1" then
	dim sSQL, oBC
	sSQL = "insert tblDownload (IPAddress, Browser, Email, Version) "
	sSQL = sSQL + "values ('" + Request.ServerVariables("REMOTE_ADDR") + "', '"
	set oBC = Server.CreateObject("MSWC.BrowserType")
	sSQL = sSQL + oBC.Browser + " " + oBC.Version + "', '" 
	sSQL = sSQL + Request.Form("Email") + "', '" + Request.Form("Version") + "')"
	set oBC = nothing
	RunSQL(sSQL)
%>

<br>
You should receive the file shortly.  If not, click <a href="<%=sFile%>">here</a> to start the download.<br>

<a href="../docs/">Go to documentation</a>

<%
else
%>
<br>

<form method=post action="default.asp">
<input type="hidden" name="dl" value="1">
<table border=0 class=clsRpt align="center" width="300">
	<tr class=clsRptBody1>
		<td colSpan=2>Entering your email is not required.  However, if you do, you will receive an email when significant updates are available.<br><font size=-1>Your email will not be sold or released to anyone.</font></td>
	</tr><tr class=clsRptBody2>
		<td>Email address:</td><td><input type="text" name="email" size=50></td>
	</tr><tr class=clsRptBody1>
		<td>Version:</td><td><select name="Version"><option selected value="1.01">1.01 [Current Version]</option><option value="1.00">1.00</option></select></td>
	</tr><tr class=clsRptBottom>
		<td align="center" colSpan=2><input type=submit value="Download Now (~130 kb)"></td>
	</tr>
</table>
</form>

<br>
<%
end if
%>

<!-- #include file="../_common/footer.inc"-->
