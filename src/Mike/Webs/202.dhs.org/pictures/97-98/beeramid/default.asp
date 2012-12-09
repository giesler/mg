<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../../../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "202 Beeramid 98"
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

<table border="0" cellpadding=10>
	<tr>
		<td width="200" align="center"><img SRC="fridge.jpg" WIDTH="150" HEIGHT="224"></td>
		<td width="400" align="center">The fridge before the night.</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10>
	<tr>
		<td width="200" align="center"><img SRC="beeramid.jpg" WIDTH="250" HEIGHT="375"></td>
		<td width="400" align="center">The almost full beeramid the next day.</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10>
	<tr>
		<td width="200" align="center"><img SRC="beeramid_arun.jpg" WIDTH="250" HEIGHT="375"></td>
		<td width="400" align="center">Arun topping off the first beeramid.</td>
	</tr>
</table>

<!--#include file="../../../_common/gPageFooter.asp"-->

</body>
</html>