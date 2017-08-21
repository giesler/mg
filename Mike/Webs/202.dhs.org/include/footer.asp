<br><br>
    </td>
  </tr>
  <tr>
    <td width="100%" align="middle" bgcolor=#666666>

<table border=0 width="100%" align="middle" cellpadding="2" cellspacing="0">
<tr>
	<td align=center><font size="-1">
<% if Request.Cookies("202")("LoggedIn") = False then %>
		&nbsp;
<% else
			dim sScript
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

  </td>
  </tr>
</table>

<% if bBetaMode then %>
<table border="0" width="600" bgColor="#C00000" cellspacing="0" cellpadding="0" align="center">
	<tr><td align="center"><b>B&nbsp;&nbsp;&nbsp;E&nbsp;&nbsp;&nbsp;T&nbsp;&nbsp;&nbsp;A</b></td></tr>
</table>
<% end if %>

</body>
</html>

<%
cnn.close
%>
