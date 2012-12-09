<%@ Language=VBScript %>
<% Response.Buffer = false %>

<!-- #include file="utils.asp"-->

<html>
<head>
<title>Location</title>
<link rel="stylesheet" type="text/css" href="styles.css">
</head>
<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0">

<form method="get" id="form1" name="form1">

<input type="hidden" name="sitesubmit" value="1">

<table class="clsWiz" align="center" width="100%" height="100%">
	<tr class="clsWizTop">
		<td><font size="-1">Select another Location</font></td>
	</tr><tr class="clsWizBody1">
		<td valign="top" align="center"><table border="1" cellpadding="0" cellspacing="0"><tr><td><img src="tv.gif" WIDTH="154" HEIGHT="240" alt="This demo does not allow you to change the location"></td></tr></table></td>
	</tr><tr class="clsWizBottom">
		<td align="center"><input type="button" value="Close" onClick="javascript:window.close()" id="button1" name="button1">
	</tr>
</table>

</body>
</html>
