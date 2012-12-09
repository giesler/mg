<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "202 People"
mstrOffset		 = "../"
mstrArea			 = "people"
%>
<!-- #include file="../_common/gCommonCode.asp"-->

<html>
<head>
	<!--#include file="../_common/gHeaderCode.asp"-->
	<title><%=mstrPageTitle%></title>	
</head>
<body <%=GetBodyTag%>>

<!--#include file="../_common/gPageHeader.asp"-->

<br>

<table align="center">
	<tr>
		<td width="300" valign="top"><font class="clsHeadingText"><a href="view.asp">View</a></font><br>
			View the people of 202. <a href="view.asp">&gt;&gt;</a></td>
		<td width="300" valign="top"><font class="clsHeadingText"><a href="../email/default.asp?refurl=<%=Server.URLEncode(Request.ServerVariables("SCRIPT_NAME"))%>">Email</a></font><br>
			Send an email to one or more people. <a href="../email/default.asp?refurl=<%=Server.URLEncode(Request.ServerVariables("SCRIPT_NAME"))%>">&gt;&gt;</a></td>
	</tr>
</table>

<br>

<table width="400" align="center" class="clsNormalTable">
	<tr>
		<td><font size="-1">Know someone else that would like to use this site?  Email <%=NewEmailMsg(mstrOffset, "1", "Mike")%></font></td>
	</tr>
</table>

<!--#include file="../_common/gPageFooter.asp"-->

</body>
</html>