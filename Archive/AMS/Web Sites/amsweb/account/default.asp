<%@ Language=VBScript %>
<%
Option Explicit
dim strOffset
strOffset = "../"

' check if a login attempt
dim cn, rs, blnBadLogin, id
if Request("un") <> "" then
	set cn = Server.CreateObject("ADODB.Connection")
	cn.Open Application("cnString")
	set rs = cn.Execute("sp_Web_GetAccountInfo '" & Replace(Request.Form("un"),"'","''") & "', '" & Replace(Request.Form("pwd"),"'","''") & "'")
	if rs.BOF and rs.EOF then
		blnBadLogin = true
	else
		id = rs.Fields("UserIDString").Value
	end if
	rs.Close
	cn.Close
	
	if not blnBadLogin then 
		response.Redirect("modify/?id=" & mid(id, 3))
	end if

end if
%>
<html>
	<head>
		<title>Adult Media Swapper - Account Info</title>
		<link rel="stylesheet" type="text/css" href="../ams.css">
			<script language="javascript">

function checkform() {

	if (f.un.value == '') {
		alert('You must enter a username.');
		f.un.focus();
		return false;
	}
	if (f.un.value.length < 4) {
		alert('Your username must be at least 4 characters!');
		f.un.focus();
		return false;
	}
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
	return true;
}
			</script>
	</head>
	<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" link="#E22000" vlink="#bf0400" alink="#ef1c19">
		<!-- #include file="../_header.asp"-->
		<h3>
			Account Information
		</h3>
		<p>
			Here is where you can change your account preferences or sign up for advanced 
			services.
		</p>
		<p>
			If you need to create a new account, please do so <a href="../download/new.asp">here</a>.
		</p>
		<p>
			Registering for Adult Media Swapper is completely <b>FREE.</b> AMS does not 
			share any account information with outside advertisers. Please see our <a href="../about/privacy/">
				Privacy Policy</a> for more details.
		</p>
		<p>
			Login below to update your account information.
		</p>
		<% if Request("err") = "login" then %>
		<p class="req">
			Please login to access this area
		</p>
		<% elseif Request("err") = "vid" then %>
		<p class="req">
			Please login to send a validation message.
		</p>
		<% end if %>
		<% if blnBadLogin then %>
		<p class="req">
			The username and password entered was not recognized.
		</p>
		<% end if %>
		<form method="post" name="f" id="f" action="default.asp">
			<blockquote>
				<table bgcolor="#f0f0f0" width="200" cellpadding="3">
					<tr>
						<td class="clsText">
							User&nbsp;name:
						</td>
						<td>
							<input type="text" name="un" size="30" value="" maxlength="32" id="Text1">
						</td>
					</tr>
					<tr>
						<td class="clsText">
							Password:
						</td>
						<td>
							<input type="password" size="30" name="pwd" maxlength="32" id="Password1">
						</td>
					</tr>
					<tr>
						<td colspan="2" align="right">
							<input type="submit" value="Continue >>" onclick="return checkform()" id="Submit1" name="Submit1">
						</td>
					</tr>
				</table>
			</blockquote>
		</form>
		<!-- #include file="../_footer.asp"-->
	</body>
</html>
