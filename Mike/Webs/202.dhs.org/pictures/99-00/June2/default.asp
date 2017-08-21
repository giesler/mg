<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../../../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "202 At The Union"
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

<table border="0" cellpadding="10" width="400" align="center" class="clsNormalTable">
	<tr>
		<td>A few pictures from an evening at the Union.</td>
	</tr>
</table>


<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="lake.jpg" WIDTH="300" HEIGHT="209"></td>
		<td align="center">The slightly flooded Lake Mendota at the Union</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="jes_mike.jpg" WIDTH="300" HEIGHT="193"></td>
		<td align="center">Jessica and Mike</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="shades.jpg" WIDTH="350" HEIGHT="231"></td>
		<td align="center">Greg, Mike, and Sara in shades in the Union</td>
	</tr>
</table>

<!--#include file="../../../_common/gPageFooter.asp"-->

</body>
</html>


