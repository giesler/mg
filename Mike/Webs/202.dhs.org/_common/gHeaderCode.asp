<!-- header code begin -->

<!--<link rel="stylesheet" type="text/css" href="<%=mstrOffset%>_common/css/theme.css">-->

<%
dim strHCTheme
if Request("css") <> "" then
	Response.Cookies("css") = Request("css")
	strHCTheme = Request("css")
elseif Request.Cookies("css") <> "" then
	strHCTheme = Request.Cookies("css")
else
	strHCTheme = "theme.css"
end if
%>

<link rel="stylesheet" type="text/css" href="<%=mstrOffset%>_common/css/<%=strHCTheme%>">

<!-- header code end -->
