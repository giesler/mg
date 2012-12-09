<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "VBSW Mail List"
mstrOffset		 = "../"
mstrArea			 = "maillist"
%>

<!-- #include file="../_common/gCommonCode.asp"-->

<%
dim strSubChk, strUnSubChk, rs, strCurStatus

if Request("mlaction") = "1" then
	dim strSQL
	strSQL = "sp_VBSWMailListAction " & IIf(Request("action")="Subscribe", "1", "0")
	strSQL = strSQL & ", '" & DQ(Request("email")) & "', "
	strSQL = strSQL & "'" & Request.ServerVariables("REMOTE_ADDR") & "'"
	cnWebDB.Execute strSQL
	Response.Cookies("email") = Request("email")
	Response.Cookies("email").expires = #January 1, 2002#
else
	if Request("unsub") <> "" then
		strUnSubChk = "checked"
		Response.Cookies("Email") = mstrEmail
		mstrEmail = Request("unsub")
	else
		set rs = Server.CreateObject("ADODB.Recordset")
		rs.Open "sp_VBSWMaillistStatus '" & mstrEmail & "'", cnWebDB, adOpenForwardOnly, adLockReadOnly
		if rs.Fields("Subscribed").Value = true then
			strUnSubChk = "checked"
			strCurStatus = "You are currently subscribed."
		else
			strSubChk = "checked"
			strCurStatus = "You are not currently subscribed."
		end if
		rs.Close
		set rs = nothing
	end if
end if
		
%>

<html>
<head>
	<!--#include file="../_common/gHeaderCode.asp"-->
	<title><%=mstrPageTitle%></title>	
</head>
<body <%=GetBodyTag%>>

<!--#include file="../_common/gPageHeader.asp"-->

<% if Request("mlaction") <> "1" then %>

<p>
You can subscribe and unsubscribe to the mailling list below.  If you are subscribed, you will 
receive notification of new versions or any critical issues.  Your name will not be sold or 
provided to anyone.  Simply enter your email address, and click 'Subscribe' or 'Unsubscribe'.
</p>

<form method="post" action="default.asp">
<input type="hidden" name="mlaction" value="1">

<table <%=GetTTag()%>>
	<tr>
		<td colSpan=2 <%=GetTHeaderTag()%>>Mail List Subscription</td>
	</tr><tr>
		<td <%=GetTBodyTag(0)%>>Your Email:</td>
		<td <%=GetTBodyTag(0)%>>
			<% if mstrEmail <> "" then %>
				<%=Request.Cookies("email")%> (<a href="<%=Request.ServerVariables("SCRIPT_NAME")%>?<%=AddQSVar(Request.QueryString, "ChangeEmail", mstrEmail)%>">change</a>)
				<input type="hidden" name="email" size="40" value="<%=mstrEmail%>">
				<br>
				<font size="-1"><i><%=strCurStatus%></i></font>
			<% else %>
				<input type="text" name="email" size="40" value="<%=Request.QueryString("ChangeEmail")%>">
			<% end if %>
		</td>
	</tr><tr>
		<td <%=GetTBodyTag(1)%>>Action:</td>
		<td <%=GetTBodyTag(0)%>>
			<input type="radio" name="action" id="action1" value="Subscribe" <%=strSubChk%>> <label for="action1">Subscribe</label><br>
			<input type="radio" name="action" id="action2" value="Unsubscribe" <%=strUnSubChk%>> <label for="action2">Unsubscribe</label><br>
		</td>
	</tr><tr>
		<td <%=GetTFooterTag()%> colSpan="2">
			<table width="100%">
				<tr>
					<td align="center">
						<input type="submit" value="  OK  ">&nbsp;
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
<% if Request("action") = "Subscribe" then %>
You have been subscribed to the VBSW mailling list.
<% else %>
You have been unsubscribed from the VBSW mailling list.
<% end if %>
</p>

<% end if %>

<!--#include file="../_common/gPageFooter.asp"-->

</body>
</html>