

<% if bBetaMode then %>
<table border="0" width="600" bgColor="#C00000" cellspacing="0" cellpadding="0" align="center">
	<tr><td align="center"><b>B&nbsp;&nbsp;&nbsp;E&nbsp;&nbsp;&nbsp;T&nbsp;&nbsp;&nbsp;A</b></td></tr>
</table>
<% end if %>

<table border="0" width="600" bgcolor="midnightblue" cellspacing="0" cellpadding="0" align="center">
  <tr>
    <td width="100%" align="left">
<table border="0" width="100%">
<tr><td align="left"><h2><%=sPageTitle%></h2></td>
<td align="right">
<%
if Request.Cookies("202")("UserID") = "" then
	Response.Write("<h5>Please login.</h5>")
else
	Response.Write("<h5>You are logged in as " + Request.Cookies("202")("UserLogin") + "<br>")
	Response.Write("<a href=""" & sOffset & "people/user/default.asp?id=" + CStr(Request.Cookies("202")("UserID")) + """>Change my info</a> | ")
	Response.Write("<a href=""" & soffset & "logout.asp"">Logout</a></h5>")
end if
%>
</td></tr></table>
  </td>
  </tr>
</table>

<table border=0 width="600" align="center" bgColor = "#666666" cellpadding="2" cellspacing="0">
<tr>
	<td align=center><font size="-1">
<% if Request.Cookies("202")("LoggedIn") = False then %>
		&nbsp;
<% else
			sScript = Request.ServerVariables("SCRIPT_NAME")
			if sScript = "/menu.asp" then
				Response.Write("Home | ")
			else
				Response.Write("<a href=""" + sOffset + "menu.asp"">Home</a> | ")
			end if

			if sScript = "/archives/default.asp" then
				Response.Write("Archives | ")
			else
				Response.Write("<a href=""" + sOffset + "archives/default.asp"">Archives</a> | ")
			end if

			if sScript = "/email/default.asp" then
				Response.Write("Email | ")
			else
				Response.Write("<a href=""" + sOffset + "email/default.asp?refurl=" + Server.URLEncode(Request.ServerVariables("SCRIPT_NAME")) + """>Email</a> | ")
			end if

			if sScript = "/people/default.asp" then
				Response.Write("People | ")
			else
				Response.Write("<a href=""" + sOffset + "people/default.asp"">People</a> | ")
			end if

			if sScript = "/pictures/default.asp" then
				Response.Write("Pictures | ")
			else
				Response.Write("<a href=""" + sOffset + "pictures/default.asp"">Pictures</a> | ")
			end if

			if sScript = "/quotes/default.asp" then
				Response.Write("Quotes")
			else
				Response.Write("<a href=""" + sOffset + "quotes/default.asp"">Quotes</a>")
			end if
 end if %>
	</font></td>
</tr>
</table>

<table border="0" width="600" bgcolor="midnightblue" cellpadding="0" align="center" cellspacing="0">
  <tr>
    <td width="100%" valign="top">
			<br>

