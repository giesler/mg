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

<table border="0" cellpadding=10 align="center" class="clsNormalTable" width="85%">
	<tr>
		<td>The first 202 related party to be busted.  No pictures of the cops though...  And it was just a noise violation...</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center">
	<tr>
		<td align="center"><img SRC="nicolle_sara.jpg" WIDTH="306" HEIGHT="186"></td>
		<td align="center">Nicolle and Sara.</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center">
	<tr>
		<td align="center"><img SRC="lee.jpg" WIDTH="206" HEIGHT="290"></td>
		<td align="center">Lee</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center">
	<tr>
		<td align="center"><img SRC="doug_talk_lee.jpg" WIDTH="306" HEIGHT="207"></td>
		<td align="center">Doug apparently boring Lee to death.  Resh, Reid, Nicolle, and Kara in the background</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center">
	<tr>
		<td align="center"><img SRC="resh_reid.jpg" WIDTH="206" HEIGHT="278"></td>
		<td align="center">Resh dancing with Reid</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center" class="clsNormalTable">
	<tr>
		<td width="150">&nbsp;</td>
		<td width="150" align="center">Page 1 <a href="page2.asp">2</a> <a href="page3.asp">3</a></td>
		<td width="150" align="right"><a href="page2.asp">next &gt; &gt;</a></td>
	</tr>
</table>

<!--#include file="../../../_common/gPageFooter.asp"-->

</body>
</html>

