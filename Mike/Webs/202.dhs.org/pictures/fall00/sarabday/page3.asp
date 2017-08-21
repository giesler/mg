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

<table border="0" cellpadding="10" align="center" width="450" class="clsNormalTable">
	<tr>
		<td width="150"><a href="page2.asp">&lt; &lt; previous</a></td>
		<td width="150" align="center">Page <a href="default.asp">1</a> <a href="page2.asp">2</a> 3</td>
		<td width="150" align="right">&nbsp;</td>
	</tr>
</table>


<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="luke_chris_doug.jpg" WIDTH="350" HEIGHT="226"></td>
		<td align="center">Luke, Chris, and Doug</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="luke_chris_doug_2.jpg" WIDTH="350" HEIGHT="209"></td>
		<td align="center">Luke, while Chris and Doug drink</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="chris.jpg" WIDTH="300" HEIGHT="308"></td>
		<td align="center">Chris</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center" width="450" class="clsNormalTable">
	<tr>
		<td width="150"><a href="page2.asp">&lt; &lt; previous</a></td>
		<td width="150" align="center">Page <a href="default.asp">1</a> <a href="page2.asp">2</a> 3</td>
		<td width="150" align="right">&nbsp;</td>
	</tr>
</table>

<!--#include file="../../../_common/gPageFooter.asp"-->

</body>
</html>


