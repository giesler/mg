<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../../../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "202 Greg's Birthday Party"
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
		<td>Celebrating Greg's birthday on Marrion.</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="group.jpg" WIDTH="350" HEIGHT="219"></td>
		<td align="center">A whole bunch of people.  Duh.</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="pete_luke_trester.jpg" WIDTH="300" HEIGHT="175"></td>
		<td align="center">Peter, Luke, and Trester</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="doug_chris.jpg" WIDTH="250" HEIGHT="207"></td>
		<td align="center">Doug and Chris</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="couch_group.jpg" WIDTH="250" HEIGHT="207"></td>
		<td align="center">Sara, Luke, Kathy, and Mike in a big group hug</td>
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


