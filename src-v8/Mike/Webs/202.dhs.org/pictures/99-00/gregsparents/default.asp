<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../../../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "202 Party at Greg's Parents"
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
		<td>Greg had a party at his parent's house while they were out of town.  Two nights of partying.</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="doug_sara.jpg" WIDTH="200" HEIGHT="280"></td>
		<td align="center">Doug and Sara outside on the porch</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="sarahl.jpg" WIDTH="200" HEIGHT="285"></td>
		<td align="center">Sarah L. with the hat</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="greg.jpg" WIDTH="200" HEIGHT="307"></td>
		<td align="center">Greg... with his shirt off...</td>
	</tr>
</table>

<br>

<table border="0" cellpadding="10" align="center">
	<tr>
		<td align="center"><img SRC="bed1.jpg" WIDTH="300" HEIGHT="215"></td>
		<td align="center">Sara, Kathy, and Doug in bed</td>
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


