<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "202 Settings Change"
mstrOffset		 = "../../"
mstrArea			 = "people"
'mstrOnLoad     = "document.frmUserInfo.Email.focus();"
%>
<!-- #include file="../../_common/gCommonCode.asp"-->

<html>
<head>
	<!--#include file="../../_common/gHeaderCode.asp"-->
	<title><%=mstrPageTitle%></title>	
</head>
<body <%=GetBodyTag%>>

<!--#include file="../../_common/gPageHeader.asp"-->

<%

dim rst, strSQL, id, strErrMsg, rstProps
dim strEmail, strList, strMadList, strListed
strErrMsg = ""
id = cstr(Request.Cookies("PersonID"))
set rst = Server.CreateObject("ADODB.Recordset")
rst.Open "sp_PersonInfo " + id, cnWebDB, adOpenStatic, adLockReadOnly
set rstProps = Server.CreateObject("ADODB.Recordset")
rstProps.Open "sp_PersonProperty " + id, cnWebDB, adOpenStatic, adLockReadOnly

If Request("submit") = "1" Then
	' check for valid data
	if Request("OldPassword") <> "" or Request("NewPassword") <> "" then
		if rst.Fields("Password") <> HashText(Request("OldPassword")) then
			strErrMsg = strErrMsg + "The old password you specified is incorrect.<br>"
		end if
		if Request("NewPassword") <> Request("ConfirmPassword") then
			strErrMsg = strErrMsg + "The confirmation password did not match the new password.<br>"
		elseif Request("NewPassword") = "" then
			strErrMsg = strErrMsg + "You cannot specify a blank password.<br>"
		end if
	end if
	if Request("Email") = "" then
		strErrMsg = strErrMsg + "Your email address cannot be blank.<br>"
	end if			
	' update user properties
	if strErrMsg = "" then
'		strSQL = "sp_PersonPropertyUpdate " + id + ", null, 'Listed', " + Request("Listed")
'		cnWebDB.Execute strSQL
'		strSQL = "sp_PersonPropertyUpdate " + id + ", null, 'madlist@202.dhs.org', " + Request("MadList")
'		cnWebDB.Execute strSQL
'		strSQL = "sp_PersonPropertyUpdate " + id + ", null, 'list@202.dhs.org', " + Request("List")
'		cnWebDB.Execute strSQL
		if Request("NewPassword") <> "" then
			strSQL = "'" + HashText(Request("NewPassword")) + "'"
		else
			strSQL = "null"
		end if
		strSQL = "sp_PersonUpdate " + id + ", " + strSQL + ", '" + DQ(Request("Email")) + "'"
		cnWebDB.Execute strSQL
'		UpdateLists
		Response.Redirect("default.asp?id=" + id)
	end if
	strEmail = Request("Email")
	strList = Request("List")
	strMadList = Request("MadList")
	strListed = Request("Listed")
else
	strEmail = rst.Fields("Email")
	if rstProps.BOF and rstProps.EOF then
		strListed = 0
		strList = 0
		strMadList = 0
	else
		rstProps.MoveFirst
		rstProps.Find "PropertyName = 'Listed'"
		if rstProps.EOF then
			strListed = 0
		elseif rstProps.Fields("PropertyValue") = "1" then
			strListed = 1
		else
			strListed = 0
		end if
		rstProps.MoveFirst
		rstProps.Find "PropertyName = 'list@202.dhs.org'"
		if rstProps.EOF then
			strList = 0
		elseif rstProps.Fields("PropertyValue") = "1" then
			strList = 1
		else
			strList = 0
		end if
		rstProps.MoveFirst
		rstProps.Find "PropertyName = 'madlist@202.dhs.org'"
		if rstProps.EOF then
			strMadList = 0
		elseif rstProps.Fields("PropertyValue") = "1" then
			strMadList = 1
		else
			strMadList = 0
		end if
	end if
end if

%>

<p>
Change any information below, then click 'Save'.  If you do not want to change your password, 
you can leave the password fields blank.
</p>

<table class="clsNoteTable" align="center">
	<tr>
		<td>Fields marked with a <font color="red">*</font> cannot be modified online during the 202 transition time.  Please email Mike to change this information.</td>
	</tr>
</table>

<% if strErrMsg <> "" then %>
<p class="clsErrorText">
<%=strErrMsg%>
</p>
<% end if %>

<form name="frmUserInfo" method="POST" action="settings.asp">
<input type="hidden" name="submit" value="1">

<table class="clsNormalTable" cellpadding=4 align=center>
	<tr>
		<td>Email:</td>
		<td><input type="hidden" name=Email value="<%=strEmail%>" class="clsInputBox"><%=strEmail%> <font color="red">*</font></td>
	</tr><tr>
		<td>Old Password:</td>
		<td><input type="password" name="OldPassword" class="clsInputBox"></td>
	</tr><tr>
		<td>New Password:</td>
		<td><input type="password" name="NewPassword" class="clsInputBox"></td>
	</tr><tr>
		<td>Confirm New Password:</td>
		<td><input type="password" name="ConfirmPassword" class="clsInputBox"></td>
	</tr><tr>
		<td>List Me:</td>
		<td><!--<select name="Listed" class="clsInputBox">
				<option value="1" <%=IIf(strListed = 1,"SELECTED","")%>>Yes</option>
				<option value="0" <%=IIf(strListed = 0,"SELECTED","")%>>No</option>
			</select>--><%=IIf(strListed, "Yes", "No")%> <font color="red">*</font></td>
	</tr><tr>
		<td>madlist@202.dhs.org:</td>
		<td><!--<select name="MadList" class="clsInputBox">
				<option value="1" <%=IIf(strMadList = 1,"SELECTED","")%>>Subscribed</option>
				<option value="0" <%=IIf(strMadList = 0,"SELECTED","")%>>Not Subscribed</option>
			</select>--><%=IIf(strMadList=1, "Subscribed", "Not subscribed")%> <font color="red">*</font></td>
	</tr><tr>
		<td>list@202.dhs.org:</td>
		<td><!--<select name="List" class="clsInputBox">
				<option value="1" <%=IIf(strList = 1,"SELECTED","")%>>Subscribed</option>
				<option value="0" <%=IIf(strList = 0,"SELECTED","")%>>Not Subscribed</option>
			</select>--><%=IIf(strList=1, "Subscribed", "Not subscribed")%> <font color="red">*</font></td>
	</tr>
</table>
<br>
<center><input type="submit" name="save" value="Save" class="clsButton">
<input type="Button" name="cancel" value="Cancel" onClick="javascript:history.back()" class="clsButton">
</center>

</form>
<%
rst.Close
set rst = nothing
rstProps.close
set rstProps = nothing
%>

<!--#include file="../../_common/gPageFooter.asp"-->

</body>
</html>