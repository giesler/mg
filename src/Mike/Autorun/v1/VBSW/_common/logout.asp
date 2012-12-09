<%@ Language=VBScript %>
<%
Response.Cookies("eSAMI") = ""

dim sTitle, iOffset, bLogin, sOnLoad
sTitle = "SAMI Logout"
iOffset = 1
bLogin = true

if Request.QueryString("intro") = "1" then 
	Response.Redirect("/intro.asp")
end if
%>
<!-- #include file="../_include/header.inc"-->

<table border=0 cellspacing=10><tr><td>
You have been logged out.  To complete the logout you should exit your web browser.<br>
<br>
<a href="login.asp">Login</a>
</td></tr></table>

<!-- #include file="../_include/footer.inc"-->
