<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../../../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "Sara's Bday at the Gritty"
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

<table border="0" cellpadding="10" align="center" class="clsNormalTable" width="85%">
	<tr>
		<td>October 6 we went to the Gritty for Sara's birthday</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="lukedoug.jpg" WIDTH="350" HEIGHT="246"></td>
		<td align="center">Luke and Doug</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="kathy.jpg" WIDTH="300" HEIGHT="273"></td>
		<td align="center">Kathy</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="sara1.jpg" WIDTH="300" HEIGHT="284"></td>
		<td align="center">Sara presenting her free beer</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="mike.jpg" WIDTH="300" HEIGHT="299"></td>
		<td align="center">Mike</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="jes_chris.jpg" WIDTH="350" HEIGHT="231"></td>
		<td align="center">Jessica and Chris</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center" class="clsNormalTable">
	<tr>
		<td width="150">&nbsp;</td>
		<td width="150" align="center">Page 1 <a href="page2.asp">2</a> <a href="page3.asp">3</a></td>
		<td width="150" align="right"><a href="page2.asp">next &gt; &gt;</a></td>
	</tr>
</table>

<!--#include file="../../../_common/gPageFooter.asp"-->

</body>
</html>

