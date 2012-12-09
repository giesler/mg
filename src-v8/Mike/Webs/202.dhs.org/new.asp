<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "202 New Stuff"
mstrOffset		 = ""
%>
<!-- #include file="_common/gCommonCode.asp"-->

<html>
<head>
	<!--#include file="_common/gHeaderCode.asp"-->
	<title><%=mstrPageTitle%></title>	
</head>
<body <%=GetBodyTag%>>

<!--#include file="_common/gPageHeader.asp"-->

<p>
Return <a href="default.asp">Home</a>
</p>

<table class="clsNormalTable" width="95%" align="center" cellpadding=4>
	<tr><td><center><font size="+1">What's New?</font></center><br>
		<b>Apr 7</b><br>New <a href="pictures/">pictures</a> and changed <a href="quotes/">quotes</a> so you can view only some at a time.  You can also enable/disable drunk features <a href="people/person/settings.asp">here</a>.  And in the <a href="archives/">archives</a>, 'More Beer'.<br><br>
		<b>Mar 23</b><br>Sorry if you tried logging in in the past day or two and it failed.  202 staff was programming drunk.  Or not.<br><br>
		<b>Mar 2</b><br>Added 'Email' link to top of all pages.<br><br>
		<b>Feb 16</b><br>Added lots of <a href="pictures/default.asp">pictures</a>, changed the <a href="people/default.asp">People</a> section to allow you to
				email multiple people.  You can also see the <a href="pictures/99-00/bevwall/default.asp">beverage wall</a> results.<br><br>
	</td></tr>
</table>

<!--#include file="_common/gPageFooter.asp"-->

</body>
</html>
