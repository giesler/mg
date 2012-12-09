<%@ Language=VBScript %>
<%
Option Explicit
dim strOffset
strOffset = "../../../"
%>
<html>
<head>
<title>Adult Media Swapper - Suggestions</title>
<link rel="stylesheet" type="text/css" href="../../../ams.css">
</head>
<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" link=#E22000 vlink=#bf0400 alink=#ef1c19>

<!-- #include file="../../../_header.asp"-->

<h3>Suggestions</h3>
 
<p>
We welcome your comments and suggestions.  Although we do read every comment or suggestion sent to us, we 
cannot guarantee a response.  You do not need to enter an email address, but if you would like a response from 
us, be sure to fill it in.
</p>

<form action="suggest.asp" method="post">

<blockquote>
<table bgcolor="#f0f0f0" width="400" cellpadding="3">
	<tr>
		<td class="clsText">Issue Type:</td>
		<td><select name="type"><option>Suggestion</option><option>Comment</option></select></td>
	</tr><tr>
		<td class="clsText">Email:</td>
		<td><input name="email" type="text" maxlength="64" size="32"></td>
	</tr><tr>
		<td colSpan="2"><textarea name="txt" rows="6" cols="50"></textarea></td>
	</tr><tr>
		<td colSpan="2" align="right"><input type="submit" value="Send"></td>
	</tr>
</table>
</form>

<!-- #include file="../../../_footer.asp"-->

</body>
</html>
