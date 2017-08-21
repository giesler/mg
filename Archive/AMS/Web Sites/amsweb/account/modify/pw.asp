<%@ Language=VBScript %>
<%
Option Explicit
dim strOffset
strOffset = "../../"

dim strID, cn, strErrs, blnUpdated
strErrs = ""
strID = Request.QueryString("id")
if strID = "" then response.Redirect("../")

if Request.Form("pwd") <> "" then
	if len(Request.Form("pwd")) < 5 then
		strErrs = strErrs & "Your password must be at least 5 characters!<br>"
	end if
	if Request.Form("pwd") <> Request.Form("cpwd") then
		strErrs = strErrs & "Your confirmation password does not match the first password you entered!<br>"
	end if

	if strErrs = "" then	
		set cn = Server.CreateObject("ADODB.Connection")
		cn.Open Application("cnString")
		cn.Execute "sp_Web_UpdatePassword 0x" & strID & ", '" & Replace(Request("pwd"), "'", "''") & "'"
		cn.Close
		blnUpdated = true
	end if
end if
%>
<html>
	<head>
		<title>Adult Media Swapper - Modify Account</title>
		<link rel="stylesheet" type="text/css" href="../../ams.css">
			<% if not blnUpdated then %>
			<script language="javascript">

function checkform() {
	if (f.pwd.value == '') {
		alert('You must enter a password.');
		f.pwd.focus();
		return false;
	}
	if (f.pwd.value.length < 5) {
		alert('Your password must be at least 5 characters!');
		f.pwd.focus();
		return false;
	}
	if (f.cpwd.value == '') {
		alert('You must enter your password twice.');
		f.cpwd.focus();
		return false;
	}
	if (f.pwd.value != f.cpwd.value) {
		alert('Your confirmation password does not match the initial password you entered.');
		f.pwd.focus();
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
			Modify Password
		</h3>
		<% if blnUpdated then %>
		<p>
			Your password has been updated.
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
			<form method="post" name="f" id="f" action="pw.asp?id=<%=strID%>">
				<blockquote>
					<table bgcolor="#f0f0f0" width="250" cellpadding="3">
						<tr>
							<td class="clsText">
								New&nbsp;Password:
							</td>
							<td>
								<input type="Password" name="pwd" size="30" maxlength="32">
							</td>
						</tr>
						<tr>
							<td class="clsText">
								Confirm&nbsp;Password:
							</td>
							<td>
								<input type="Password" name="cpwd" size="30" maxlength="32">
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
