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
		<td width="150"><a href="page2.asp">&lt; &lt; previous</a></td>
		<td width="150" align="center">Page <a href="default.asp">1</a> <a href="page2.asp">2</a> 3 <a href="page4.asp">4</a></td>
		<td width="150" align="right"><a href="page4.asp">next &gt; &gt;</a></td>
	</tr>
</table>


<br>

<table border="0" cellpadding=10 align="center">
	<tr>
		<td align="center"><img SRC="sara_frisbee.jpg" WIDTH="206" HEIGHT="264"></td>
		<td align="center">Sara attempting to catch a frisbee</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center">
	<tr>
		<td align="center"><img SRC="kara_chair_doug.jpg" WIDTH="181" HEIGHT="342"></td>
		<td align="center">Kara and Doug dancing with a chair between them.</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center">
	<tr>
		<td align="center"><img SRC="joe.jpg" WIDTH="256" HEIGHT="205"></td>
		<td align="center">Joe</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center" width="450" class="clsNormalTable">
	<tr>
		<td width="150"><a href="page2.asp">&lt; &lt; previous</a></td>
		<td width="150" align="center">Page <a href="default.asp">1</a> <a href="page2.asp">2</a> 3 <a href="page4.asp">4</a></td>
		<td width="150" align="right"><a href="page4.asp">next &gt; &gt;</a></td>
	</tr>
</table>

<!--#include file="../../../_common/gPageFooter.asp"-->

</body>
</html>

