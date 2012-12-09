<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "VBSW Feedback"
mstrOffset		 = "../"
mstrArea			 = "feedback"

if Request("send") = "1" then
	dim obj
	set obj = Server.CreateObject("CDONTS.NewMail")
	obj.To = "vbsw@giesler.org"
	obj.From = Request.Form("Name") + "<" + Request.Form("Email") + ">"
	obj.Subject = "VBSW Site Feedback: " + Request.Form("Issue")
	obj.Body = Request.Form("body")
	obj.Send
	set obj = nothing
	if mstrEmail = "" and Request("email") <> "" then
		Response.Cookies("email") = Request("email")
		mstrEmail = Request("email")
	end if
end if
%>

<!-- #include file="../_common/gCommonCode.asp"-->

<html>
<head>
	<!--#include file="../_common/gHeaderCode.asp"-->
	<title><%=mstrPageTitle%></title>	
</head>
<body <%=GetBodyTag%>>

<!--#include file="../_common/gPageHeader.asp"-->

<% if Request("send") <> "1" then %>

<p>
Need help?  Have a suggestion?  Find a bug?  Fill out the form below.  
You can also email directly to <a href="mailto:vbsw@giesler.org">vbsw@giesler.org</a>.
</p>

<form method="post" action="default.asp">
<input type="hidden" name="send" value="1">

<table <%=GetTTag()%>>
	<tr>
		<td colSpan=2 <%=GetTHeaderTag()%>>Feedback / Help</td>
	</tr><tr>
		<td <%=GetTBodyTag(1)%>>Your Name:</td>
		<td <%=GetTBodyTag(0)%>><input type="text" name="name" size=33></td>
	</tr><tr>
		<td <%=GetTBodyTag(1)%>>Your Email:</td>
		<td <%=GetTBodyTag(0)%>>
			<% if mstrEmail <> "" then %>
				<%=Request.Cookies("email")%> (<a href="<%=Request.ServerVariables("SCRIPT_NAME")%>?<%=AddQSVar(Request.QueryString, "ChangeEmail", mstrEmail)%>">change</a>)
				<input type="hidden" name="email" size="40" value="<%=mstrEmail%>">
			<% else %>
				<input type="text" name="email" size="40" value="<%=Request.QueryString("ChangeEmail")%>">
			<% end if %>		
	</tr><tr>
		<td <%=GetTBodyTag(1)%>>Issue:</td>
		<td <%=GetTBodyTag(0)%>>
			<input type="radio" name="issue" id="issue1" value="Help" checked> <label for="issue1">Help</label><br>
			<input type="radio" name="issue" id="issue2" value="Suggestion"> <label for="issue2">Suggestion</label><br>
			<input type="radio" name="issue" id="issue3" value="Bug Report"> <label for="issue3">Bug Report</label><br>
			<input type="radio" name="issue" id="issue4" value="Other"> <label for="issue4">Other</label><br>
		</td>
	</tr><tr>
		<td colSpan=2 <%=GetTBodyTag(1)%>>
			<textarea rows=5 cols=40 name="body"></textarea>
		</td>
	</tr><tr>
		<td <%=GetTFooterTag()%> colSpan="2">
			<table width="100%">
				<tr>
					<td align="center">
						<input type="submit" value="Send">&nbsp;
						<input type="reset" value="Cancel">
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>

</form>

<% else %>

<p>
Thanks for your feedback.
</p>

<% end if %>

<!--#include file="../_common/gPageFooter.asp"-->

</body>
</html>