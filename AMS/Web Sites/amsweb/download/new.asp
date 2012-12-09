<%@ Language=VBScript %>
<%
Option Explicit
dim strOffset
strOffset = "../"

dim blnValidValues, strErrs, strSQL
blnValidValues = false
strErrs = ""

' Check if this is a repost
if Request.Form("p") <> "" then
	if Len(Request.Form("un")) < 4 then
		strErrs = strErrs & "Your username must be at least 4 characters!<br>"
	end if
	if len(Request.Form("pwd")) < 5 then
		strErrs = strErrs & "Your password must be at least 5 characters!<br>"
	end if
	if Request.Form("pwd") <> Request.Form("cpwd") then
		strErrs = strErrs & "Your confirmation password does not match the first password you entered!<br>"
	end if
	if Request.Form("age") = "" then
		strErrs = strErrs & "You must select your age!<br>"
	end if
	if Request.Form("email") <> "" then
		if Instr(Request.Form("email"), "@") = 0 then
			strErrs = strErrs & "The email address you entered does not appear to be valid.  Please enter a valid email address.<br>"
		end if
	end if
	if Request.Form("referemail") <> "" then
		if Request.Form("email") = "" then
			strErrs = strErrs & "In order to award points to the person that referred you to AMS, you must include your email address.<br>"
		elseif Instr(Request.Form("email"), "@") = 0 then
			strErrs = strErrs & "The referral address you entered does not appear to be valid.  Please enter a valid email address.<br>"
		end if
	end if
	if strErrs = "" then
		blnValidValues = true
	else
		blnValidValues = false
	end if
end if

' Check age
if blnValidValues and Request.Form("age") = "0" then
	Response.Redirect("http://www.google.com")
end if

' Do a db insert
dim cn, rs, strSub
if blnValidValues then

	if Request.Form("sub") = "on" then
		strSub = "1"
	else
		strSub = "0"
	end if
	set cn = Server.CreateObject("ADODB.Connection")
	cn.Open Application("cnString")
	strSQL = "sp_webinsertuser '" & Replace(Request.Form("un"),"'","''") & "', "
	strSQL = strSQL & "'" & Replace(Request.Form("pwd"),"'","''") & "', "
	strSQL = strSQL & "'" & Replace(Request.Form("email"),"'","''") & "', "
	strSQL = strSQL & ChkNull(Request.Form("age")) & ", " & ChkNull(Request.Form("gen")) & ", " 
	strSQL = strSQL & ChkNull(Request.Form("sexpref")) & ", "
	strSQL = strSQL & "'" & Replace(Request.ServerVariables("HTTP_USER_AGENT"), "'", "''") & "', "
	strSQL = strSQL & strSub & ", "		' whether or not user subscribed
	strSQL = strSQL & "'" & Request.ServerVariables("REMOTE_ADDR") & "',"		' signup ip address
	strSQL = strSQL & "'" & Replace(Request.Form("referemail"),"'","''") & "'"
	set rs = cn.Execute(strSQL)
	if rs.Fields(0) = "1" then
		strErrs = strErrs & "Your username is already taken.  Please enter a new username.<br>"
		blnValidValues = false
	end if

	' if valid, send a mail if email was set
	if blnValidValues and Request.Form("email") <> "" then

		dim objMail, strBody
		strBody = "<html><body>"
		strBody = strBody & "<h3>Welcome to Adult Media Swapper</h3>"
		strBody = strBody & "<p>Welcome <b>" & Request.Form("un") & "</b>!</p>"
		strBody = strBody & "<p>Click <a href=""http://adultmediaswapper.com/support/software/tutorial/"">here</a> for a short tutorial on using AMS.</p>"
		strBody = strBody & "<p>Click <a href=""" & Application("VerifyURL") & "?id=" & Mid(rs.Fields(1).Value,3) & "&vid=" & Mid(rs.Fields(2).Value, 3) & """>here</a>"
		strBody = strBody & " to verify your email address"
		if Request.Form("referemail") <> "" then
			strBody = strBody & "<br><i>You must verify your email address to give credit to the person that referred you to AMS.</i>"
		end if
		strBody = strBody & "</p>"
		strBody = strBody & "</body></html>"
	
		set objMail = Server.CreateObject("BLIMAIL.SMTP")
		objMail.SendMail Application("strSMTPServer"), "maillist@adultmediaswapper.com", Request.Form("email"), "Welcome to AMS", strBody, "text/html"
		set objMail = nothing

	end if

	cn.Close
	set cn = nothing
	
end if

%>
<html>
	<head>
		<title>Adult Media Swapper - Signup</title>
		<link rel="stylesheet" type="text/css" href="../ams.css">
			<% if not blnValidValues then %>
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
	if (f.age.value == '') {
		alert('You must select your age.');
		f.age.focus();
		return false;
	}
	if (f.sub.checked && f.email.value == '') {
		alert('Please enter your email address to subscribe to the AMS mailling list.');
		f.email.focus();
		return false;
	}
	if (f.email.value == '' && f.referemail.value != '') {
		alert('Please enter your email address for confirmation - otherwise the person referring you will not get credit.');
		f.email.focus();
		return false;
	}
	return true;
}
			</script>
			<% end if %>
	</head>
	<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" link="#E22000" vlink="#bf0400" alink="#ef1c19">
		<!-- #include file="../_header.asp"-->
		<% if blnValidValues then %>
		<h3>
			Signup
		</h3>
		<p>
			Your new username is <b>
				<%=Request.Form("un")%>
			</b>
			<br>
			Your new password is not displayed for security reasons.
		</p>
		<p>
			<a href="get/">Click here to download AMS</a>
		</p>
		<% else %>
		<h3>
			New Members
		</h3>
		<p>
			In order to use AMS you need a username and password. Fields marked with a <font class="req">
				*</font> are required. Your email address is not required, however it is 
			recommended. It will not be sold or shared with anyone. By entering your email 
			address, you will be able to retreive your username and password if you forget 
			them, and subsribe to the AMS mailling list if you wish.
		</p>
		<p>
			If someone referred you here, be sure to enter their email address exactly as 
			they sent it to you.
		</p>
		<% 
if strErrs <> "" then
	Response.Write("<p class=""req"">" & strErrs & "</p>")
end if
%>
		<form method="post" name="f" id="f">
			<input type="hidden" name="p" value="1">
			<blockquote>
				<table bgcolor="#f0f0f0" width="400" cellpadding="3">
					<tr>
						<td class="clsText">
							User name:&nbsp;<font class="req">*</font>
						</td>
						<td>
							<input type="text" name="un" size="30" value="<%=Request.Form("un")%>" maxlength="32">
						</td>
					</tr>
					<tr>
						<td class="clsText">
							Password:&nbsp;<font class="req">*</font>
						</td>
						<td>
							<input type="password" size="30" name="pwd" maxlength="32">
						</td>
					</tr>
					<tr>
						<td class="clsText">
							Confirm Password:&nbsp;<font class="req">*</font>
						</td>
						<td>
							<input type="password" size="30" name="cpwd" maxlength="32">
						</td>
					</tr>
					<tr>
						<td class="clsText">
							Email Address:
						</td>
						<td>
							<input type="text" size="30" name="email" value="<%=Request.Form("email")%>" maxlength="64">
						</td>
					</tr>
					<tr>
						<td class="clsText">
							Referred by:
						</td>
						<td>
							<input type="text" size="30" name="referemail" value="<%=Request.Form("referemail")%>" maxlength="64">
						</td>
					</tr>
					<tr>
						<td class="clsText">
							Age:&nbsp;<font class="req">*</font>
						</td>
						<td>
							<select name="age">
								<option value="">
									- select -</option>
								<option value='"0" <%=IfEqualSel("age","0")%>'>
									Under 18</option>
								<option value='"18" <%=IfEqualSel("age","18")%>'>
									18-24</option>
								<option value='"25" <%=IfEqualSel("age","25")%>'>
									25-34</option>
								<option value='"35" <%=IfEqualSel("age","35")%>'>
									35-44</option>
								<option value='"45" <%=IfEqualSel("age","45")%>'>
									45-60</option>
								<option value='"60" <%=IfEqualSel("age","60")%>'>
									60+</option>
							</select>
						</td>
					</tr>
					<tr>
						<td class="clsText">
							Gender:
						</td>
						<td>
							<select name="gen">
								<option value="">
									- select -</option>
								<option value='"1" <%=IfEqualSel("gen","m")%>'>
									man</option>
								<option value='"2" <%=IfEqualSel("gen","f")%>'>
									woman</option>
							</select>
						</td>
					</tr>
					<tr>
						<td class="clsText">
							Sexual preference:
						</td>
						<td>
							<select name="spref">
								<option value="">
									- select -</option>
								<option value='"1" <%=IfEqualSel("spref","het")%>'>
									heterosexual</option>
								<option value='"2" <%=IfEqualSel("spref","hom")%>'>
									homosexual</option>
								<option value='"3" <%=IfEqualSel("spref","bi")%>'>
									bisexual</option>
							</select>
						</td>
					</tr>
					<tr>
						<td colspan="2">
							<table>
								<tr>
									<td valign="top">
										<input type="checkbox" name="sub" checked>
									</td>
									<td class="clsText">
										Check here if you would like to subscribe to the AMS mailling list. Your email 
										address will <i>not</i> be shared or used for any other purpose.
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td colspan="2">
							<img src="../../img/trans.gif" height="10">
						</td>
					</tr>
					<tr>
						<td colspan="2" align="right">
							<input type="submit" value="Continue >>" onclick="return checkform()">
						</td>
					</tr>
				</table>
			</blockquote>
		</form>
		<% end if %>
		<!-- #include file="../_footer.asp"-->
	</body>
</html>
<%

' --- Server side functions
function IfEqualSel(sSelectName, sFormVal)
	if Request.Form(sSelectName) = sFormVal then
		IfEqualSel = "selected"
	end if
end function

' replace a blank value with the string 'null'
function ChkNull(sIn)
	if sIn = "" then 
		ChkNull = "null"
	else
		ChkNull = sIn
	end if
end function

%>
