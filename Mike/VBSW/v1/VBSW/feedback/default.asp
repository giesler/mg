<%
dim sTitle, iOffset, sSubTitle
sTitle = "Feedback"
sSubTitle = "VB Setup Wrapper"
iOffset = 1

%>

<!-- #include file="../_common/header.inc"-->

<% 
'on error resume next
if Request.Form("f") = "1" then 
	dim obj, sSQL
	set obj = Server.CreateObject("CDONTS.NewMail")
	obj.To = "vbsw@giesler.org"
	obj.From = Request.Form("Name") + "<" + Request.Form("Email") + ">"
	obj.Subject = "VBSW Site Feedback: " + Request.Form("Issue")
	obj.Body = Request.Form("body")
	obj.Send
	sSQL = "insert tblFeedback (Name, Email, Issue, Body) values ("
	sSQL = sSQL + qt(Request.Form("Name")) + ", " + qt(Request.Form("Email")) + ", "
	sSQL = sSQL + qt(Request.Form("Issue")) + ", " + qt(Request.Form("Body")) + ")"
	RunSQL(sSQL)
	%>

Your message has been sent.  If you are asking for help, expect a reply within 24 hours or so.<br>
<br>

<a href="../">Return Home</a>
	
<%
else 
%>
Need help?  Have a suggestion?  Find a bug?  Fill out the form below.  You should 
get a reply within 24 hours.  You can also send an email directly to <a href="mailto:vbsw@giesler.org">vbsw@giesler.org</a>.

<form method="post" action="default.asp">
<input type="hidden" name="f" value="1">
<table class=clsRpt align="center">
	<tr class=clsRptTop>
		<td colSpan=2>Feedback / Help</td>
	</tr><tr class=clsRptBody1>
		<td>Issue:</td>
		<td><select name=issue><option>Help</option><option>Suggestion</option><option>Bug Report</option><option>Other</option></select></td>
	</tr><tr class=clsRptBody2>
		<td>Your name:</td>
		<td><input type="text" size=40 name="Name"></td>
	</tr><tr class=clsRptBody1>
		<td>Your email:</td>
		<td><input type="text" size=40 name="Email"></td>
	</tr><tr class=clsRptBody2>
		<td colSpan=2><textarea name="body" rows=5 cols=40></textarea></td>
	</tr><tr class=clsRptBottom>
		<td colSpan=2 align=center><input type="submit" value="Send"></td>
	</tr>
</table>		

<% end if %>

<!-- #include file="../_common/footer.inc"-->
