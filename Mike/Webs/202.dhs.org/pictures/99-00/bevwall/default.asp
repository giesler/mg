<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../../../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "202 Beverage Wall"
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
		<td>After months of hard work, the beverage wall was completed.  Stats are below.</td>
	</tr>
</table>

<br>

<table border="0" cellpadding=10 align="center" width=600>
	<tr>
		<td width="100" class="clsNoteTable" valign="top"><font size="-1"><b>Contributors</b><br>Arun<br>Doug<br>Greg<br>
			Joe<br>Kara<br>Luke<br>Mike<br>Resh<br>Sara<br><br>
			<i>Mike's Family</i><br>Chuck<br>Ginni<br>Mary Kay<br>Matt<br>Rick<br>Rob
			<br>Tom<br>Tony<br>Terrance<br><br>
			<i>Other People</i><br>Brian<br>Sasha<br></font>
		</td>
		<td width="375" align="center" valign="top">
			<img src="bevwall1.jpg" width="356" height="231"><br><br>
			<img src="bevwall2.jpg" width="356" height="190"><br><br>
			<img src="bevwall3.jpg" width="356" height="202"><br>
		</td>
		<td width="125" class="clsNoteTable" valign="top"><font size="-1"><b>Totals</b><br>
			<table width="100%" cellpadding=1>
				<tr><td valign=top><font size="-1">1 </font></td><td><font size="-1">Miller Lite (86)</font></td></tr>
				<tr><td valign=top><font size="-1">2 </font></td><td><font size="-1">MGD Light (36)</font></td></tr>
				<tr><td valign=top><font size="-1">3 </font></td><td><font size="-1">Coke (33)</font></td></tr>
				<tr><td valign=top><font size="-1">4 </font></td><td><font size="-1">Sprite (31)</font></td></tr>
				<tr><td valign=top><font size="-1">5 </font></td><td><font size="-1">Red Dog (27)</font></td></tr>
				<tr><td valign=top><font size="-1">6 </font></td><td><font size="-1">MGD (21)</font></td></tr>
				<tr><td valign=top><font size="-1">7 </font></td><td><font size="-1">Bud Light (12)</font></td></tr>
				<tr><td valign=top><font size="-1">7 </font></td><td><font size="-1">Budweiser (12)</font></td></tr>
				<tr><td valign=top><font size="-1">7 </font></td><td><font size="-1">High Life (12)</font></td></tr>
				<tr><td valign=top><font size="-1">7 </font></td><td><font size="-1">Icehouse (12)</font></td></tr>
				<tr><td valign=top><font size="-1">8 </font></td><td><font size="-1">Old Style (12)</font></td></tr>
				<tr><td valign=top><font size="-1">9 </font></td><td><font size="-1">Barq's (11)</font></td></tr>
				<tr><td valign=top><font size="-1">10 </font></td><td><font size="-1">Mug (4)</font></td></tr>
				<tr><td valign=top><font size="-1">11 </font></td><td><font size="-1">Pepsi (2)</font></td></tr>
			</table>
			<br>
			Grand Total: <b>311</b>
			</font>
		</td>
	</tr>
</table>

<!--#include file="../../../_common/gPageFooter.asp"-->

</body>
</html>


