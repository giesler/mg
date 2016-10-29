<%@ Language=VBScript %>
<HTML>
<HEAD>
<META NAME="GENERATOR" Content="Microsoft Visual Studio 6.0">
</HEAD>
<BODY>

<% if Request.Cookies("eSAMI")("debug") = "True" then
Response.Cookies("eSAMI")("debug") = "False"
%>
Debug info disabled.
<% else
Response.Cookies("eSAMI")("debug") = "True"
%>
Debug info enabled.
<% end if %>

<br><br>
Go back and refresh manually.

</BODY>
</HTML>
