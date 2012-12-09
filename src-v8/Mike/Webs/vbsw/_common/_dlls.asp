<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "DLL Versions"
mstrOffset		= "../"
%>
<!-- #include file="gCommonCode.asp"-->

<object runat=server progid=GOCom.Version id=mobjGoCom></object>

<html>
<head>
	<!--#include file="gHeaderCode.asp"-->
	<title><%=mstrPageTitle%></title>	
</head>
<body <%=GetBodyTag%>>

<!--#include file="gPageHeader.asp"-->

<table cellspacing=10><tr><td>

<table name="tbl1" id="tblid1">
	<tr>
		<td colSpan=2><b>GOCom</b></td>
	</tr><tr>
		<td>Version:</td><td name="sver" id ="idver"><%=mobjGoCom.Version %></td>
	</tr><tr>		
		<td>Running on:</td><td><%=mobjGoCom.ComputerName %></td>
	</tr><tr>		
		<td>Connection String:</td><td><%=mobjGoCom.ConnectionString %></td>
	</tr>
</table>

<!--#include file="gPageFooter.asp"-->

</body>
</html>
