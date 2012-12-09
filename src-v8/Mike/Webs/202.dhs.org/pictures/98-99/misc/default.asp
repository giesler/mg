<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../../../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "202 Misc Pictures 99"
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
		<td align="center"><img SRC="joe_mike_chair.jpg" WIDTH="300" HEIGHT="210"></td>
	</tr><tr>
		<td align="center">Joe and Mike on the blue spin chair.  Feb 19.</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center">
	<tr>
		<td align="center"><img SRC="luke_horn.jpg" WIDTH="200" HEIGHT="330"></td>
		<td align="center">Luke playing Sara's horn.  Mar 29.</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center">
	<tr>
		<td align="center"><img SRC="joe_hall.jpg" WIDTH="200" HEIGHT="329"></td>
		<td align="center">Joe passed out in the 3rd floor hallway.  April 26.</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center">
	<tr>
		<td align="center"><img SRC="mike_comp.jpg" WIDTH="200" HEIGHT="156"></td>
		<td align="center">Mike by his computer.  April 22.</td>
	</tr>
</table>

<!--#include file="../../../_common/gPageFooter.asp"-->

</body>
</html>


