<%@ Language=VBScript %>
<%
Option Explicit
dim strOffset
strOffset = "../../"

dim strID, cn, strErrs, blnUpdated
strID = Request.QueryString("id")
if strID = "" then response.Redirect("../")

if Request.Form("subscribe") <> "" then
	set cn = Server.CreateObject("ADODB.Connection")
	cn.Open Application("cnString")
	cn.Execute "sp_Web_SetSubscribe 0x" & strID & ", " & Request.Form("subscribe")
	cn.Close
	blnUpdated = true
end if
%>
<html>
	<head>
		<title>Adult Media Swapper - Modify Account</title>
		<link rel="stylesheet" type="text/css" href="../../ams.css">
	</head>
	<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" link="#E22000" vlink="#bf0400" alink="#ef1c19">
		<!-- #include file="../../_header.asp"-->
		<h3>
			Mailling Preference
		</h3>
		<% if blnUpdated then %>
		<p>
			<% if Request.Form("subscribe") = "1" then %>
			You have been subscribed to the AMS mailling list.
			<% else %>
			You have been unsubscribed from the AMS mailling list.
			<% end if %>
			<br>
			<a href="default.asp?id=<%=strID%>">Return to Account home</a>
		</p>
		<% else %>
		<p>
			<form method="post" name="f" id="f" action="subscribe.asp?id=<%=strID%>">
				<blockquote>
					<table bgcolor="#f0f0f0" width="400" cellpadding="3">
						<tr>
							<td class="clsText">
								<% if request.QueryString("subscribe") = "0" then %>
								<input type="hidden" value="1" name="subscribe"> You are not currently 
								subscribed to receive the AMS mailling list. Click 'Subscribe' to subscribe to 
								the AMS mailling list. Your email address will not be shared or used for any 
								other purpose.
								<% else %>
								<input type="hidden" value="0" name="subscribe"> You are currently subscribed 
								to receive the AMS mailling list. Click 'Unsubscribe' to unsubscribe from the 
								AMS mailling list. Note - your email address will never be shared or used for 
								any other purpose.
								<% end if %>
							</td>
						</tr>
						<tr>
							<td align="right">
								<% if request.QueryString("subscribe") = "0" then %>
								<input type="submit" value="Subscribe">
								<% else %>
								<input type="submit" value="Unsubsribe">
								<% end if %>
								<input type="button" value="Cancel" onclick="javascript:history.back()">
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
