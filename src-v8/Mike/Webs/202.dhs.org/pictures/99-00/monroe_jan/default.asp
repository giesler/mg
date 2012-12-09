<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../../../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "202 - 'P' is for Party"
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

<table border="0" cellpadding=10 width="400" align="center" class="clsNormalTable">
	<tr>
		<td>Sara, Joe, Kristen, and Glenn had a party at their Monroe Street apartment Jan 21.  Below are pictures...</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center">
	<tr>
		<td align="center"><img SRC="jim_sara.jpg" WIDTH="256" HEIGHT="249"></td>
		<td align="center">Jim and Sara in the kitchen.</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center">
	<tr>
		<td align="center"><img SRC="group.jpg" WIDTH="356" HEIGHT="232"></td>
		<td align="center">Left to right, Greg, Jenalyn, Kristen, Joe, Sara, Jim</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center">
	<tr>
		<td align="center"><img SRC="nate_xxx.jpg" WIDTH="256" HEIGHT="212"></td>
		<td align="center">Kimberly and John</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center">
	<tr>
		<td align="center"><img SRC="jim_joe.jpg" WIDTH="256" HEIGHT="202"></td>
		<td align="center">Jim and Joe</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center" width="450" class="clsNormalTable">
	<tr>
		<td width="150">&nbsp;</td>
		<td width="150" align="center">Page 1 <a href="page2.asp">2</a> <a href="page3.asp">3</a> <a href="page4.asp">4</a></td>
		<td width="150" align="right"><a href="page2.asp">next &gt; &gt;</a></td>
	</tr>
</table>

<!--#include file="../../../_common/gPageFooter.asp"-->

</body>
</html>


