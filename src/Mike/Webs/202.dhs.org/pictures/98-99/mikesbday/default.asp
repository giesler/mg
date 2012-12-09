<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../../../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "202 Mike's Birthday 99"
mstrOffset		 = "../../../"
mstrArea			 = "pictures"
%>
<!-- #include file="../../../_common/gCommonCode.asp"-->

<html>
<head>
	<!--#include file="../../../_common/gHeaderCode.asp"-->
	<title><%=mstrPageTitle%></title>	
</head>
<body <%=GetBodyTag%>>

<!--#include file="../../../_common/gPageHeader.asp"-->

<br>

<table border="0" cellpadding=10 align="center">
	<tr>
		<td align="center"><img SRC="mbday_fabfour.jpg" WIDTH="406" HEIGHT="285"></td>
	</tr><tr>
		<td align="center">The fab four at the Gritty for Mike's birthday.</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center">
	<tr>
		<td align="center"><img SRC="mbday_joe.jpg" WIDTH="256" HEIGHT="222"></td>
		<td align="center">Joe singing "It's your birthday" to Mike.</td>
	</tr>
</table>

<!--#include file="../../../_common/gPageFooter.asp"-->

</body>
</html>


