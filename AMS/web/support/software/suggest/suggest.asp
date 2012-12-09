<%@ Language=VBScript %>
<%
Option Explicit
dim strOffset
strOffset = "../../../"

' Do a db insert
dim cn

set cn = Server.CreateObject("ADODB.Connection")
cn.Open Application("cnString")
cn.Execute "sp_websuggest '" & Replace(Request.Form("type"),"'","''") & "', '" & Replace(Request.Form("txt"),"'","''") & "', '" & Replace(Request.Form("email"),"'","''") & "'"
cn.Close
set cn = nothing
	
%>
<html>
<head>
<title>Adult Media Swapper - Thanks</title>
<link rel="stylesheet" type="text/css" href="../../../ams.css">
</head>
<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" link=#E22000 vlink=#bf0400 alink=#ef1c19>

<!-- #include file="../../../_header.asp"-->

<h3>Suggestion</h3>

<p>
Thanks!  Your feedback is appreciated.
</p>

<!-- #include file="../../../_footer.asp"-->

</body>
</html>
