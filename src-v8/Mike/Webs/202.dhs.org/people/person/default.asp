<%@ Language=VBScript
    EnableSessionState=False %>

<%
Option Explicit
%>
<!--#include file="../../_common/gDeclares.asp"-->
<% 
' Page level variables
mstrPageTitle = "202 People"
mstrOffset		 = "../../"
mstrArea			 = "people"
%>
<!-- #include file="../../_common/gCommonCode.asp"-->

<html>
<head>
	<!--#include file="../../_common/gHeaderCode.asp"-->
	<title><%=mstrPageTitle%></title>	
</head>
<body <%=GetBodyTag%>>

<!--#include file="../../_common/gPageHeader.asp"-->

<%

Dim rst, id, rstP
id = Request("id")
if id = "" then id = Request.Cookies("PersonID")
Set rst = Server.CreateObject("ADODB.Recordset")
rst.Open "sp_PersonInfo " + id, cnWebDB, adOpenStatic, adLockReadOnly
set rstP = Server.CreateObject("ADODB.Recordset")
rstP.Open "sp_PersonDetail " + id, cnWebDB, adOpenStatic, adLockReadOnly
if rst.BOF and rst.EOF then Response.Redirect("../default.asp")
if not rst.Fields("OnList") then
	if cstr(Request.Cookies("PersonID")) <> id then
		Response.Write("The selected user is not active.")
		Response.End
	end if
end if
%>

<table align="center" cellpadding="4" border="0">
	<tr>
		<td colSpan="2">
			<table width="100%">
				<tr>
					<td>
						<font class="clsHeadingText"><%=rst.Fields("Name")%></font>
						<p>
						<%=NewEmailMsg(mstrOffset, id, rst.Fields("Email"))%>
						</p>
					</td>
				<% if cstr(Request.Cookies("PersonID")) = id then %>
					<td align="right">
						<table class="clsNoteTable"><tr><td>
						<a href="settings.asp">Change my settings</a><br>
						<a href="details.asp?type=add">Add contact info</a>
						</td></tr></table>
					</td>
				<% end if %>
				</tr>
			</table>
		</td>
	</tr><tr>
		<td valign="top">			
			<% if rst.Fields("HiResPic") <> "" then %>
				<img src="pics/<%=rst.Fields("HiResPic")%>" border="0">
			<% else %>
				<font size="-2"><i>no picture<br>available</i></font>
			<% end if %>
			<% If not rst.Fields("OnList") and cstr(Request.Cookies("PersonID")) = id Then %>
			<table width="200" class="clsNormalTable"><tr><td><font class="clsErrorText" size="-1">No one else can see your email and other information because you are not listed.  
			Click 'Change my settings' above to update your listing status.</font></td></tr></table>
			<% End If %>
		</td><td valign="top">
			<% if rstP.BOF and rstP.EOF then %>
			<table class="clsNormalTable">
				<tr>
					<td><%=rst.Fields("Name")%> has not entered any additional information.</td>
				</tr>
			</table>
			<% end if

			do while not rstP.EOF
			%>
			<table class="clsNormalTable" width="100%" cellpadding="4">
			<tr>
				<% if rst.Fields("PersonID") <> id and rstP.Fields("Title") <> "" then %>
					<td colspan="2"><b><font size="-1"><%=Nb(rstP.Fields("Title"))%></font></b>
					<% if cstr(Request.Cookies("PersonID")) = id then %>
						&nbsp;<font size="-2"><a href="details.asp?type=edit&amp;detailid=<%=rstP.Fields("DetailID")%>">edit</a> | <a href="details.asp?type=delete&amp;detailid=<%=rstP.Fields("DetailID")%>">delete</a></font>
					<% end if %>
				</td>
				<% end if %>
			</tr>
			<% if rstP.Fields("Text") <> "" then %>
			<tr>
				<td><font size="-1"><%=NW(Nb(rstP.Fields("Text")))%></font></td>
			</tr>
			<% end if %>
			<% if rstP.Fields("Comment") <> "" then %>
			<tr>
				<td><font size="-1"><i><%=NW(Nb(rstP.Fields("Comment")))%></i></font></td>
			</tr>
			<% end if %>
			</table><br>
			<%
				rstP.MoveNext
			loop
			rstP.Close
			set rstP = nothing
			%>
		</td>
	</tr>
</table>

<%
rst.Close
set rst = nothing
%>

<!--#include file="../../_common/gPageFooter.asp"-->

</body>
</html>