<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "Template Page"
mstrOffset		 = "../"
%>
<!-- #include file="../_common/gCommonCode.asp"-->

<object runat=server progid=GOCom.Version id=mobjGoCom></object>

<html>
<head>
	<!--#include file="../_common/gHeaderCode.asp"-->
	<title><%=mstrPageTitle%></title>	
</head>
<body <%=GetBodyTag%>>

<!--#include file="../_common/gPageHeader.asp"-->


<% 
' Content goes here
Response.Write("This is a template page only.")
%>


<!--#include file="../_common/gPageFooter.asp"-->

</body>
</html>
