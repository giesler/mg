<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "202 Email"
mstrOffset		 = "../"
mstrArea			 = "email"
%>
<!-- #include file="../_common/gCommonCode.asp"-->

<html>
<head>
	<!--#include file="../_common/gHeaderCode.asp"-->
	<title><%=mstrPageTitle%></title>	
</head>
<body <%=GetBodyTag%>>

<!--#include file="../_common/gPageHeader.asp"-->

<%
if Request.Form("Confirmed") = "yes" then
	Dim oMsg
	set oMsg = Server.CreateObject("CDONTS.NewMail")

	oMsg.To = Request.Form("to")
	oMsg.From = Request.Form("from")
	if Request.Form("bcc") = "on" then
		oMsg.Bcc = Request.Form("to")
	end if
	oMsg.Subject = Request.Form("subject")
	oMsg.Body = Request.Form("body")
	oMsg.Send
	
	set omsg = nothing
%>

<br>

<center>
The email was sent succesfully.<br><br>

<a href="<%=Request.Form("refurl")%>">Continue >></a>
</center>

<%
else   ' we want to confirm the email
%>

Are you sure you want to send the below email message?  <br>
<font size=-1>You must click 'Send' below to actually send the message.</font>

<form name="newmsg" action="sendmsg.asp" method="post">
	<input type="hidden" name="Confirmed" value="yes">
	<input type="hidden" name="refurl" value="<%=Request.Form("refurl")%>">
	<input type="hidden" name="from" value="<%=Request.Form("from")%>">
	<input type="hidden" name="to" value="<%=Request.Form("to")%>">
  <input type="hidden" name="subject" value="<%=DQ(Request.Form("subject"))%>">
  <input type="hidden" name="body" value="<%=DQ(Request.Form("body"))%>">
  <input type="hidden" name="bcc" value="<%=Request.Form("bcc")%>">
<table class="clsNormalTable" width="400" align="center" cellpadding="4">
	<tr>
		<td valign=top>From:</td>
		<td><%=Request.form("sUserName")%> &lt;<%=Request.form("sUserEmail")%>&gt;</td>
	</tr><tr>
		<td valign=top>To:</td>
		<td><%=Server.HTMLEncode(Request.Form("sToName"))%></td>
<% if Request("bcc") = "on" then %>
	</tr><tr>
		<td valign=top>BCC:</td>
		<td><%=Request.form("sUserName")%> &lt;<%=Request.form("sUserEmail")%>&gt;</td>		
<% end if %>
	</tr><tr>
		<td valign=top>Subject:</td>
		<td><%=Request.Form("subject")%></td>
	</tr><tr>
		<td colSpan=2><table width="95%" border=0 align=center class="clsInputBox"><tr><td>
			<%=NW(Request.Form("body"))%></td></tr></table></td>
	</tr><tr>
		<td align="center" colSpan=2><input type="submit" value="Send" id=submit1 name=submit1 class="clsButton">
		 <input type="button" value="Back" onClick="javascript:history.back()" id=button1 name=button1 class="clsButton"></td>
	</tr>
</table>
</form>

<%
end if
%>

<!--#include file="../_common/gPageFooter.asp"-->

</body>
</html>