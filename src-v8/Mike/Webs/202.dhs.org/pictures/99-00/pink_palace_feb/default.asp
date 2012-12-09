<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../../../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "202 - Pink Palace Party"
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
		<td>Yet another Pink Palace Party... complete with Jello shots</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center">
	<tr>
		<td align="center"><img SRC="kara.jpg" WIDTH="206" HEIGHT="343"></td>
		<td align="center">Kara</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center">
	<tr>
		<td align="center"><img SRC="greg.jpg" WIDTH="206" HEIGHT="318"></td>
		<td align="center">Greg happy as can be</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center">
	<tr>
		<td align="center"><img SRC="resh.jpg" WIDTH="206" HEIGHT="342"></td>
		<td align="center">Resh after a Jello shot</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center">
	<tr>
		<td align="center"><img SRC="resh2.jpg" WIDTH="206" HEIGHT="311"></td>
		<td align="center">Resh again</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center" width="450" class="clsNormalTable">
	<tr>
		<td width="150">&nbsp;</td>
		<td width="150" align="center">Page 1 <a href="page2.asp">2</a></td>
		<td width="150" align="right"><a href="page2.asp">next &gt; &gt;</a></td>
	</tr>
</table>

<!--#include file="../../../_common/gPageFooter.asp"-->

</body>
</html>

