<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../../../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "202 March Madness"
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
		<td>A couple pictures from the March Madness games.</td>
	</tr>
</table>


<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="greg-drink.jpg" WIDTH="250" HEIGHT="256"></td>
		<td align="center">Greg drinking something.  March 25</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="jenglenn.jpg" WIDTH="300" HEIGHT="220"></td>
		<td align="center">Jenalyn and Glenn.  March 25</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="sara_greg_ff.jpg" WIDTH="200" HEIGHT="314"></td>
		<td align="center">Sara and Greg at Mad Dogs watching the Final Four game.  April 1</td>
	</tr>
</table>

<!--#include file="../../../_common/gPageFooter.asp"-->

</body>
</html>


