<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "202 Detail Change"
mstrOffset		 = "../../"
mstrArea			 = "people"
if Request("type") = "edit" or Request("type") = "add" then
	mstrOnLoad     = "document.frmDetail.Title.focus();"
end if
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
Dim rst, strSQL, id, strTitle, strText, strComment
id = CStr(Request.Cookies("PersonID"))

If Request("submit") = "1" then
	If Request("type") = "add" then
		strSQL = "sp_PersonDetailAdd " + id + ", '" + DQ(Request("Title")) + "', "
		strSQL = strSQL + "'" + DQ(Request("Text")) + "', '" + DQ(Request("Comment")) + "'"
		cnWebDB.Execute strSQL
		Response.Redirect("default.asp?id=" + id)
	Elseif Request("type") = "edit" then
		strSQL = "sp_PersonDetailUpdate " + Request("DetailID") + ", " + id + ", "
		strSQL = strSQL + "'" + DQ(Request("Title")) + "', "
		strSQL = strSQL + "'" + DQ(Request("Text")) + "', '" + DQ(Request("Comment")) + "'"
		cnWebDB.Execute strSQL
		Response.Redirect("default.asp?id=" + id)
	Elseif Request("type") = "delete" then
		strSQL = "sp_PersonDetailDelete " + Request("DetailID") + ", " + id
		cnWebDB.Execute strSQL
		Response.Redirect("default.asp?id=" + id)
	End If
Else
	If Request("type") <> "add" then
		Set rst = Server.CreateObject("ADODB.Recordset")
		rst.Open "sp_PersonDetail " + id + ", " + Request("DetailID"), cnWebDB, adOpenStatic, adLockReadOnly
		strTitle = rst.Fields("Title")
		strText = rst.Fields("Text")
		strComment = rst.Fields("Comment")
		rst.Close
		set rst = nothing
	End If

%>

<br>
<form name="frmDetail" method="POST">
<input type="hidden" name="type" value="<%=Request("type")%>">
<input type="hidden" name="DetailID" value="<%=Request("DetailID")%>">
<input type="hidden" name="submit" value="1">

<% if Request("type") <> "delete" then %>

<table class="clsNormalTable" cellpadding=4 align=center>
	<tr>
		<td>Title</td>
		<td align=right><font size=-2>ie. phone, address, pager, icq, another email, etc.</font></td>
	</tr><tr>
		<td colspan=2><input type="text" name="Title" value="<%=strTitle%>" class="clsInputBox"></td>
	</tr><tr>
		<td>Text</td>
		<td align=right><font size=-2>ie. 608-264-1565, 1650 Kronshage Drive, etc.</font></td>
	</tr><tr>
		<td colspan=2><textarea rows=4 cols=40 name="Text" class="clsInputBox"><%=strText%></textarea></td>
	</tr><tr>
		<td>Comment</td>
		<td align=right><font size=-2>ie. rarely used, emergency (out of beer), etc.</font></td>
	</tr><tr>
		<td colspan=2><textarea rows=4 cols=40 name="Comment" class="clsInputBox"><%=strComment%></textarea></td>
	</tr>
</table>
<br>
<center><input type="submit" name="save" value="Save" class="clsButton">
<input type="button" name="cancel" value="Cancel" class="clsButton" onClick="javascript:history.back()">
</center>

<% else %>

<p>
Are you sure you want to delete this entry?
</p>

<table class="clsNormalTable" cellpadding="4" align="center">
<tr>
	<% if strTitle <> "" then %>
		<td colspan="2"><b><font size="-1"><%=Nb(strTitle)%></font></b></td>
	<% end if %>
</tr>
<% if strText <> "" then %>
<tr>
	<td><font size="-1"><%=NW(Nb(strText))%></font></td>
</tr>
<% end if %>
<% if strComment <> "" then %>
<tr>
	<td><font size="-1"><i><%=NW(Nb(strComment))%></i></font></td>
</tr>
<% end if %>
</table><br>
<br>
<center><input type="submit" name="delete" value="Delete" class="clsButton">
<input type="button" name="cancel" value="Cancel" class="clsButton" onClick="javascript:history.back()">
</center>

<% end if %>

</form>


<%
End If
%>

<!--#include file="../../_common/gPageFooter.asp"-->

</body>
</html>