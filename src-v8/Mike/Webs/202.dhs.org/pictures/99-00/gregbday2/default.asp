<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../../../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "202 Greg's 2nd Birthday Party"
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
		<td>Celebrating Greg's birthday again - the Gritty was closed the last time, so this time we started at the Gritty.</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="sara.jpg" WIDTH="250" HEIGHT="187"></td>
		<td align="center">Sara</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="sara_greg.jpg" WIDTH="250" HEIGHT="181"></td>
		<td align="center">Sara and Greg</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="greg_muff1.jpg" WIDTH="200" HEIGHT="292"></td>
		<td align="center">Greg with a bday drink...</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="greg_muff2.jpg" WIDTH="200" HEIGHT="291"></td>
		<td align="center">Greg doing one of his birthday drinks.</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="greg_kathy.jpg" WIDTH="250" HEIGHT="208"></td>
		<td align="center">Greg harassing Kathy with a balloon.</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center" width="450" class="clsNormalTable">
	<tr>
		<td width="150">&nbsp;</td>
		<td width="150" align="center">Page 1 <a href="page2.asp">2</a></td>
		<td width="150" align="right"><a href="page2.asp">next &gt; &gt;</a></td>
	</tr>
</table>

<!--#include file="../../../_common/gPageFooter.asp"-->

</body>
</html>


