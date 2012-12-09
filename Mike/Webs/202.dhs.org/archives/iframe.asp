<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "202 Archives"
mstrOffset		 = "../"
mstrArea			 = "archives"
%>
<!-- #include file="../_common/gCommonCode.asp"-->

<%
' validate we have a URL
dim strURL
strURL = Request("URL")
if strURL = "" then Response.Redirect("default.asp")
%>

<html>
<head>
	<!--#include file="../_common/gHeaderCode.asp"-->
	<title><%=mstrPageTitle%></title>	
</head>
<body <%=GetBodyTag%>>

<!--#include file="../_common/gPageHeader.asp"-->

<table width="95%" class="clsNormalTable" align="center">
	<tr>
		<td align="center">Event Invite</td>
	</tr><tr>
		<td>
			<iframe src="<%=strURL%>" width="100%" height="500" align="center">
			<p>
			Your browser does not support embedded frames.  Click <a href="<%=strURL%>" target="_new">here</a> to view the page.
			</p>
			</iframe>
		</td>
	</tr>
</table>

<!--#include file="../_common/gPageFooter.asp"-->

</body>
</html>
