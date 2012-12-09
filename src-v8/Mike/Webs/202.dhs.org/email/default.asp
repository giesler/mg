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

<script Language="JavaScript"><!--

function ChangeSpan(intEnable) {
	if (intEnable == 1)
		personlist.style.display = "";
	else
		personlist.style.display = "none";
}

// --></script>

</head>
<body <%=GetBodyTag%>>

<!--#include file="../_common/gPageHeader.asp"-->

<br>

<form name="multemail" action="new.asp?refurl=<%=Server.urlencode(Request("refurl"))%>" method="post">

<table align="center" cellpadding="4" class="clsNormalTable">
	<tr>
		<td colSpan=2>Send email to:</td>
	</tr><tr>
		<td valign="top"><input type="radio" name="type" id="madlist" value="madlist"  onClick="ChangeSpan(0)" checked></td>
		<td><label for="madlist">madlist@202.dhs.org<br><font class="clsNoteText">Only people subscribed to the Madison list</font></label></td>
	</tr><tr>
		<td valign="top"><input type="radio" name="type" id="list" value="list" onClick="ChangeSpan(0)"></td>
		<td><label for="list">list@202.dhs.org<br><font class="clsNoteText">All members of the 202 site that have selected to be subscribed to the 202 list.</font></label></td>
	</tr><tr>
		<td valign="top"><input type="radio" name="type" id="sel" value="sel" onClick="ChangeSpan(1)"></td>
		<td><label for="sel">Select specific people:<span id="personlist" style="display: none">
			<table>
				<tr>
					<td valign="top">
						<table>
<%
dim rst, iLoopVar, iCol1
iLoopVar = 0
set rst = Server.CreateObject("ADODB.Recordset")
rst.Open "sp_UserList", cnWebDB, adOpenStatic, adLockReadOnly
rst.movelast
rst.movefirst
iCol1 = Int(rst.RecordCount / 2) + rst.RecordCount Mod 2
do while not rst.EOF
	iLoopVar = iLoopVar + 1
%>
		<tr>
			<td><input type="checkbox" name="PersonID" value="<%=rst.Fields("PersonID")%>" id="PersonID_<%=CStr(rst.Fields("PersonID"))%>">
					<label for="PersonID_<%=CStr(rst.Fields("PersonID"))%>"><%=rst.Fields("Name")%></label>
			</td>
		</tr>
		<% if iLoopVar = iCol1 then %>
						</table>
					</td><td valign="top">
						<table>
		<% end if
				rst.MoveNext
loop
rst.Close
set rst = nothing
%>
					</td>
				</tr>
			</table></span>
			</table>
		</td>
	</tr>
</table>

<table width="400" align="center">
	<tr>
		<td align="right"><input type="submit" name="Continue" value="Continue >>" class="clsButton">
			<input type="button" name="Cancel" value="Cancel" onClick="javascript:history.back()" class="clsButton"></td>
	</tr>
</table>

</form>

<!--#include file="../_common/gPageFooter.asp"-->

</body>
</html>