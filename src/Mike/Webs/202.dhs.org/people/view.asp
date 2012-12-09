<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "202 People"
mstrOffset		 = "../"
mstrArea			 = "people"
%>
<!-- #include file="../_common/gCommonCode.asp"-->

<html>
<head>
	<!--#include file="../_common/gHeaderCode.asp"-->
	<title><%=mstrPageTitle%></title>	
</head>
<body <%=GetBodyTag%>>

<!--#include file="../_common/gPageHeader.asp"-->

<table align="center">
	<tr>
		<td valign="top">
			<table cellpadding="4">
<%
Dim rst, iLoopVar, iCol1
set rst = Server.CreateObject("ADODB.Recordset")
rst.Open "sp_UserList", cnWebDB, adOpenStatic, adLockReadOnly
iLoopVar = 0
iCol1 = Int(rst.RecordCount / 2) + rst.RecordCount Mod 2

Do while not rst.EOF
	iLoopVar = iLoopVar + 1
%>
				<tr><td><table width="100%" class="clsNormalTable">

				<tr>
					<td valign="top">
						<font class="clsHeadingText"><%=rst.Fields("Name")%></font>
					</td>
				</tr><tr>
					<td>
						<table cellpadding="4"><tr><td>
						<% if rst.Fields("LoResPic") <> "" then %>
							<a href="person/default.asp?id=<%=rst.Fields("PersonID")%>"><img src="person/pics/<%=rst.Fields("LoResPic")%>" border="0"></a>
						<% else %>
							&nbsp;
						<% end if %>
						</td><td>
						<p>
						<%=NewEmailMsg(mstrOffset, rst.Fields("PersonID"), "send email >>")%>
						<br><br>
						<a href="person/default.asp?id=<%=rst.Fields("PersonID")%>">more info &gt;&gt;</a>
						</p>
						</td></tr></table>
					</td>
				</tr>
				</table></td></tr>
			<% if iLoopVar = iCol1 then %>
			</table>
		</td><td valign="top">
			<table cellpadding="4">
			<% end if %>
  <%
  rst.MoveNext
loop
rst.Close
set rst = nothing
%>
		</td>
	</tr>
</table>

<!--#include file="../_common/gPageFooter.asp"-->

</body>
</html>
