<html>
	<head>
		<meta http-equiv="Content-Language" content="en-us">
		<meta name="GENERATOR" content="Microsoft FrontPage 4.0">
		<meta name="ProgId" content="FrontPage.Editor.Document">
		<title>Thanks</title>
		<style fprolloverstyle>
			A:hover {color: #FF0000; font-family: Arial}</style>
		<meta name="Microsoft Theme" content="none, default">
		<meta name="Microsoft Border" content="none, default">
	</head>
	<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0">
		<%

if IsEmpty(Request("Message")) Then
	Response.Redirect("default.asp")
end if

dim objMail
set objMail = Server.CreateObject("BLIMAIL.SMTP")
objMail.SendMail "kyle", Request.Form("email"), "mike@giesler.org", "mike.giesler.org email: " + Request.Form("subject"), "From: " + Request.Form("From") + vbCrLf + Request.Form("message"), "text"
set objMail = nothing

%>
		<p>
			&nbsp;
		</p>
		<p>
			Thanks for you email.
		</p>
		<p>
			&nbsp;
		</p>
		<p>
			Have a nice day.
		</p>
		<p>
			&nbsp;
		</p>
		<p>
			Back to my <a href="../Default.asp">homepage</a>
		.</body>
</html>
