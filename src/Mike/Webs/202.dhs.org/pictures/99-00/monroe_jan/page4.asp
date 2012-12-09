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

<table border="0" cellpadding=10 align="center" width="450" class="clsNormalTable">
	<tr>
		<td width="150"><a href="page3.asp">&lt; &lt; previous</a></td>
		<td width="150" align="center">Page <a href="default.asp">1</a> <a href="page2.asp">2</a> <a href="page3.asp">3</a> 4</td>
		<td width="150" align="right">&nbsp;</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center">
	<tr>
		<td align="center"><img SRC="joe_shoe.jpg" WIDTH="256" HEIGHT="247"></td>
		<td align="center">Joe eating Mike's shoe</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center">
	<tr>
		<td align="center"><img SRC="joe_mike_sara.jpg" WIDTH="306" HEIGHT="180"></td>
		<td align="center">Joe with his 'bottle' (a coaster), Mike, and Sara</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center">
	<tr>
		<td align="center"><img SRC="doug_kara.jpg" WIDTH="206" HEIGHT="150"></td>
		<td align="center">Kara and Doug</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center" width="450" class="clsNormalTable">
	<tr>
		<td width="150"><a href="page3.asp">&lt; &lt; previous</a></td>
		<td width="150" align="center">Page <a href="default.asp">1</a> <a href="page2.asp">2</a> <a href="page3.asp">3</a> 4</td>
		<td width="150" align="right">&nbsp;</td>
	</tr>
</table>

<!--#include file="../../../_common/gPageFooter.asp"-->

</body>
</html>


