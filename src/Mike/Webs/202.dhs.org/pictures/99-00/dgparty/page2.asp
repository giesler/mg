<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../../../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "202 Doug and Greg's Feb Party"
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

<table border="0" cellpadding=10 align="center" width="450" class="clsNormalTable">
	<tr>
		<td width="150"><a href="default.asp">&lt; &lt; previous</a></td>
		<td width="150" align="center">Page <a href="default.asp">1</a> 2 <a href="page3.asp">3</a></td>
		<td width="150" align="right"><a href="page3.asp">next &gt; &gt;</a></td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center">
	<tr>
		<td align="center"><img SRC="matt_nicolle.jpg" WIDTH="106" HEIGHT="250"></td>
		<td align="center">Matt and Nicolle dancing</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center">
	<tr>
		<td align="center"><img SRC="greg_trester.jpg" WIDTH="206" HEIGHT="204"></td>
		<td align="center">Greg making sure no one is in the bathroom for Trester.</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center">
	<tr>
		<td align="center"><img SRC="peter_trester.jpg" WIDTH="306" HEIGHT="185"></td>
		<td align="center">Peter and Trester on the couch, Kara looking uh...</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center">
	<tr>
		<td align="center"><img SRC="sara_lush.jpg" WIDTH="306" HEIGHT="233"></td>
		<td align="center">Lee is just a bit amazed at Sara's drinking ability</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center" width="450" class="clsNormalTable">
	<tr>
		<td width="150"><a href="default.asp">&lt; &lt; previous</a></td>
		<td width="150" align="center">Page <a href="default.asp">1</a> 2 <a href="page3.asp">3</a></td>
		<td width="150" align="right"><a href="page3.asp">next &gt; &gt;</a></td>
	</tr>
</table>

<!--#include file="../../../_common/gPageFooter.asp"-->

</body>
</html>

