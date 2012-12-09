<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "VBSW Download"
mstrOffset		 = "../"
mstrArea			 = "download"
%>

<!-- #include file="../_common/gCommonCode.asp"-->

<%
dim strFile, blnUnSub
blnUnSub = false

if Request("version") <> "" then
	select case Request("version")
		case "100"
			strFile = "vbsw_1-00.zip"
		case "101"
			strFile = "vbsw_1-01.zip"
		case "200BETA"
			strFile = "vbsw_2-00-BETA.zip"
		case "200"
			strFile = "vbsw_2-00.zip"
	end select
	mstrOnLoad = "dlfile();"
	
	dim strSQL
	strSQL = "sp_VBSWAddDownload '" & Request.ServerVariables("REMOTE_ADDR") & "', "
	strSQL = strSQL & "'" & Request.ServerVariables("HTTP_USER_AGENT") & "', "
	strSQL = strSQL & "'" & Request("email") & "', " & IIf(Request("subscribe")="1", "1", "0") & ", "
	strSQL = strSQL & "'" & Request("version") & "'"
	cnWebDB.Execute strSQL
	
	if Request("subscribe") = "1" then
		strSQL = "sp_VBSWMailListAction 1, '" & Request("email") & "', "
		strSQL = strSQL & "'" & Request.ServerVariables("REMOTE_ADDR") & "'"
		cnWebDB.Execute strSQL
	end if
	
	if mstrEmail = "" and Request("email") <> "" then
		Response.Cookies("email") = Request("email")
		mstrEmail = Request("email")
	end if
else

	' want to see if this person is not currently subscribed
	if mstrEmail <> "" then
		dim rsSub
		set rsSub = Server.CreateObject("ADODB.Recordset")
		rsSub.Open "sp_VBSWMaillistStatus '" & mstrEmail & "'", cnWebDB, adOpenForwardOnly, adLockReadOnly
		if rsSub.Fields("Subscribed").Value = false then
			blnUnSub = true
		end if
	end if
	
end if

%>

<html>
<head>
	<!--#include file="../_common/gHeaderCode.asp"-->
	<title><%=mstrPageTitle%></title>	

<script language="JavaScript"><!--
function dlfile() {
	window.location = '<%=strFile%>';
}
function submitForm() {
	alert('checking version...');
	if (document.form.version.value == '200BETA')
		return (window.prompt('Are you sure you want to download the beta version?  It has not been fully tested.'));
}
// --></script>

</head>
<body <%=GetBodyTag%>>

<!--#include file="../_common/gPageHeader.asp"-->


<% if strFile = "" then %>

<table class="clsNoteTable" align="center">
	<tr>
		<td <%=GetTBodyTag(0)%>><img src="<%=mstrOffset%>images/warn.gif"></td>
		<td <%=GetTBodyTag(0)%>>Please read the <a href="<%=mstrOffset%>docs/">documentation</a><br>before downloading!</td>
	</tr>
</table>

<br>

<p>
Please fill out the form below to download VBSW.  <!-- Your email address is only required for Beta versions.  -->VBSW is free, however, 
<a href="<%=mstrOffset%>about/">donations</a> or even simple <a href="<%=mstrOffset%>feedback/">feedback</a> would be greatly appreciated.
</p>

<form name="form" id="form" method="post" action="default.asp">

<table <%=GetTTag%>>
	<tr>
		<td <%=GetTHeaderTag()%> colspan="2">Download VBSW</td>
	</tr><tr>
		<td <%=GetTBodyTag(1)%>>Email Address:</td>
		<td <%=GetTBodyTag(0)%>>
			<% if mstrEmail <> "" then %>
				<%=Request.Cookies("email")%> (<a href="<%=Request.ServerVariables("SCRIPT_NAME")%>?<%=AddQSVar(Request.QueryString, "ChangeEmail", mstrEmail)%>">change</a>)
				<input type="hidden" name="email" size="40" value="<%=mstrEmail%>">
			<% else %>
				<input type="text" name="email" size="40" value="<%=Request.QueryString("ChangeEmail")%>">
			<% end if %>
		</td>
	</tr>
	<% if mstrEmail = "" or blnUnSub then %>
	<tr>
		<td <%=GetTBodyTag(1)%>>Subscribe:</td>
		<td <%=GetTBodyTag(0)%>>
			<table>
				<tr>
					<td valign="top"><input type="checkbox" name="subscribe" id="subscribe" value="1"> </td>
					<td><font size="-2"><label for="subscribe">Check to be notified when new versions are released.<br><i>Your address will not be sold or released to anyone.</i></font></td>
				</tr>
			</table>
		</td>
	</tr>
	<% end if %>
	<tr>
		<td <%=GetTBodyTag(1)%>>Select Version:</td>
		<td <%=GetTBodyTag(0)%>>
			<input type="radio" name="version" value="200" id="v200" checked> <label for="v200">2.00 (Current Version) <i>(139k)</i></label><br>
			<input type="radio" name="version" value="101" id="v101"> <label for="v101">1.01 <i>(132k)</i></label><br>
			<input type="radio" name="version" value="100" id="v100"> <label for="v100">1.00 <i>(130k)</i></label><br>
		</td>
	</tr><tr>
		<td <%=GetTFooterTag()%> colSpan="2">
			<table width="100%">
				<tr>
					<td align="center">
						<input type="submit" value="Download">&nbsp;
						<input type="reset" value="Cancel">
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>
			
</form>

<% else %>

<p>
You should receive the file shortly.  If not, click <a href="<%=strFile%>">here</a> to start the download.
</p>

<a href="<%=mstrOffset%>docs/">Go to documentation</a>

<% end if %>

<!--#include file="../_common/gPageFooter.asp"-->

</body>
</html>