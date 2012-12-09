<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../../../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "202 At The Union - Sept 23"
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

<table border="0" cellpadding="10" align="center" width="450" class="clsNormalTable">
	<tr>
		<td width="150"><a href="default.asp">&lt; &lt; previous</a></td>
		<td width="150" align="center">Page <a href="default.asp">1</a> 2 <a href="page3.asp">3</a></td>
		<td width="150" align="right"><a href="page3.asp">next &gt; &gt;</a></td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="group1.jpg" WIDTH="350" HEIGHT="208"></td>
		<td align="center">Group picture</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="peter.jpg" WIDTH="250" HEIGHT="252"></td>
		<td align="center">Peter</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="jessica.jpg" WIDTH="250" HEIGHT="366"></td>
		<td align="center">Jessica</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="chris.jpg" WIDTH="250" HEIGHT="340"></td>
		<td align="center">Chris</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center" width="450" class="clsNormalTable">
	<tr>
		<td width="150"><a href="default.asp">&lt; &lt; previous</a></td>
		<td width="150" align="center">Page <a href="default.asp">1</a> 2 <a href="page3.asp">3</a></td>
		<td width="150" align="right"><a href="page3.asp">next &gt; &gt;</a></td>
	</tr>
</table>

<!--#include file="../../../_common/gPageFooter.asp"-->

</body>
</html>

