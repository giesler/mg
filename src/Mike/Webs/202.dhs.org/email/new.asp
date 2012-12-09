<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "202 Email"
mstrOffset		 = "../"
mstrArea			 = "email"
%>
<!-- #include file="../_common/gCommonCode.asp"-->

<html>
<head>
	<!--#include file="../_common/gHeaderCode.asp"-->
	<title><%=mstrPageTitle%></title>	
</head>
<body <%=GetBodyTag%>>

<!--#include file="../_common/gPageHeader.asp"-->

<%
' get user name
Dim rst, strUserName, strUserEmail, strToName, strToEmail, iLoopVar, varPersonID
set rst = Server.CreateObject("ADODB.Recordset")
rst.Open "sp_UserList", cnWebDB, adOpenStatic, adLockReadOnly
rst.Find "PersonID = " + mintPersonId
if not rst.eof then
	strUserName = rst.Fields("Name")
	strUserEmail = rst.Fields("Email")
end if
rst.MoveFirst

if Request("type") = "madlist" then
	strToName = "202 Madison List <madlist@202.dhs.org>"
	strToEmail = "madlist@202.dhs.org"
elseif Request("type") = "list" then	
	strToName = "202 List <list@202.dhs.org>"
	strToEmail = "list@202.dhs.org"
elseif Request("type") = "sel" or Request("PersonID") <> "" then
	varPersonID = Split(Request("PersonID"), ",")
	for iLoopVar = LBound(varPersonID) to UBound(varPersonID)
		rst.Find "PersonID = " + CStr(varPersonID(iLoopVar))
		if not rst.eof then
			if strToEmail <> "" then
				strToEmail = strToEmail + "; "
				strToName  = strToName + ", "
			end if
			strToEmail = strToEmail + rst.Fields("Name") + "<" + rst.Fields("Email") + ">"
			strToName  = strToName  + rst.Fields("Name")
		end if
	next	
end if
rst.close
set rst = nothing

%>

<table class="clsNormalTable" width="400" align="center" cellpadding="4">
	<tr>
		<td>Use the form below to send the email or click <a href="mailto:<%=strToEmail%>">
		 here</a> to use your default	mail program.</td>
	</tr>
</table>

<hr noshade>

<form name="newmsg" action="sendmsg.asp" method="post">
	<input type="hidden" name="refurl" value="<%=Request("refurl")%>">
	<input type="hidden" name="from" value="<%=strUserName%> <<%=strUserEmail%>>">
	<input type="hidden" name="to" value="<%=strToEmail%>">
	<input type="hidden" name="sUserName" value="<%=strUserName%>">
	<input type="hidden" name="sUserEmail" value="<%=strUserEmail%>">
	<input type="hidden" name="sToName" value="<%=strToName%>">
	
<table class="clsNormalTable" width="400" align="center" cellpadding="4">
	<tr>
		<td valign=top>From:</td>
		<td><%=strUserName%> &lt;<%=strUserEmail%>&gt;</td>
	</tr><tr>
		<td valign=top>To:</td>
		<td><%=Server.HTMLEncode(strToName)%></td>
	</tr><tr>
		<td colSpan=2><input type="checkbox" name="bcc" id="bcc">&nbsp;<label for="bcc">BCC email to <%=strUserEmail%></label></td>
	</tr><tr>
		<td valign=top>Subject:</td>
		<td><input type="text" size="45" name="subject"></td>
	</tr><tr>
		<td colSpan=2><TEXTAREA rows=7 cols=45 name=body></TEXTAREA></td>
	</tr><tr>
		<td align="center" colSpan=2><input type="submit" value="Send" class="clsButton"> 
				<input type="button" value="Cancel" onClick="javascript:history.back()" class="clsButton"></td>
	</tr>
</table>
</form>

<!--#include file="../_common/gPageFooter.asp"-->

</body>
</html>