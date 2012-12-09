<%@ Language=VBScript %>
<%
Option Explicit
dim strOffset
strOffset = "../../"

dim cn, rs, strTemp
set cn = Server.CreateObject("ADODB.Connection")
set rs = Server.CreateObject("ADODB.Recordset")
cn.Open Application("cnString")

' open rs to get news items
rs.Open "sp_Web_GetNews 10", cn

%>
<html>
	<head>
		<title>Adult Media Swapper - News</title>
		<link rel="stylesheet" type="text/css" href="../../ams.css">
	</head>
	<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" link="#E22000" vlink="#bf0400" alink="#ef1c19">
		<!-- #include file="../../_header.asp"-->
		<h3>
			News
		</h3>
		<% 
		do while not rs.EOF
			strTemp = "<p><font size=""-1"" color=""gray""><i>"
			strTemp = strTemp & FormatDateTime(rs.Fields("NewsDateTime"), 1) & "</i></font><br>"
			strTemp = strTemp & rs.Fields("NewsContent") & "</p>"
			Response.Write(strTemp)
			rs.MoveNext
		loop
		
		rs.Close
		cn.Close
		%>
		<!-- #include file="../../_footer.asp"-->
	</body>
</html>
