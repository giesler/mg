<%@ Language=VBScript %>
<%
Option Explicit
dim strOffset
strOffset = "../../"

dim strID, cn, strErrs, strEmail, blnUpdated
strID = Request.QueryString("id")
if strID = "" then response.Redirect("../")

if Request.Form("email") <> "" then
	set cn = Server.CreateObject("ADODB.Connection")
	cn.Open Application("cnString")
	cn.Execute "sp_Web_UpdateEmail 0x" & strID & ", '" & Replace(Request("Email"), "'", "''") & "'"
	cn.Close
	blnUpdated = true

	if Request.Form("email") <> "" then
		set cn = Server.CreateObject("ADODB.Connection")
		cn.Open Application("cnString")
		cn.Execute "sp_Web_UpdateEmail 0x" & strID & ", '" & Replace(Request("Email"), "'", "''") & "'"
		cn.Close
		blnUpdated = true
	end if

end if

strEmail = Request("email")

%>
<html>
	<head>
		<title>Adult Media Swapper - Modify Account</title>
		<link rel="stylesheet" type="text/css" href="../../ams.css">
			<% if not blnUpdated then %>
			<script language="javascript">

function checkform() {

	if (f.email.value == '') {
		alert('You must enter your email address.');
		f.email.focus();
		return false;
	}
	if (f.email.value.length < 4) {
		alert('Your email must be at least 4 characters!');
		f.email.focus();
		return false;
	}
	return true;
}
			</script>
			<% end if %>
	</head>
	<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" link="#E22000" vlink="#bf0400" alink="#ef1c19">
		<!-- #include file="../../_header.asp"-->
		<h3>
			Modify Email
		</h3>
		<% if blnUpdated then %>
		<p>
			Your email address has been updated.
			<br>
			<a href="default.asp?id=<%=strID%>">Return to Account home</a>
		</p>
		<% else %>
		<% if strErrs <> "" then %>
		<p class="req">
			<%=strErrs%>
		</p>
		<% end if %>
		<p>
			<form method="post" name="f" id="f" action="email.asp?id=<%=strID%>">
				<blockquote>
					<table bgcolor="#f0f0f0" width="200" cellpadding="3">
						<tr>
							<td class="clsText">
								Email:
							</td>
							<td>
								<input type="text" name="email" size="30" value="<%=strEmail%>" maxlength="32" ID="Text1">
							</td>
						</tr>
						<tr>
							<td colspan="2" align="right">
								<input type="submit" value="Change" onclick="return checkform()">
							</td>
						</tr>
					</table>
				</blockquote>
			</form>
		</p>
		<% end if %>
		<!-- #include file="../../_footer.asp"-->
	</body>
</html>
