
<%
' Figure out toolbar text
dim strHScript, strHToolbarText

if mblnLoginPage then
	strHToolbarText = "&nbsp;"
else
	strHScript = Request.ServerVariables("SCRIPT_NAME")
	if strHScript = "/menu.asp" then
		strHToolbarText = strHToolBarText + "Home | "
	else
		strHToolbarText = strHToolBarText + "<a href=""" + mstrOffset + "default.asp"">Home</a> | "
	end if

	if Instr(strHScript, "/archives/") > 0 then
		if Instr(strHScript, "/archives/default.asp") > 0 then
			strHToolbarText = strHToolBarText + "<b>Archives</b> | "
		else
			strHToolbarText = strHToolBarText + "<b><a href=""" + mstrOffset + "archives/default.asp"">Archives</a></b> | "
		end if
	else
		strHToolbarText = strHToolBarText + "<a href=""" + mstrOffset + "archives/default.asp"">Archives</a> | "
	end if

	if strHScript = "/email/default.asp" then
		strHToolbarText = strHToolBarText + "Email | "
	else
		strHToolbarText = strHToolBarText + "<a href=""" + mstrOffset + "email/default.asp?refurl=" + Server.URLEncode(Request.ServerVariables("SCRIPT_NAME")) + """>Email</a> | "
	end if

	if strHScript = "/people/default.asp" then
		strHToolbarText = strHToolBarText + "People | "
	else
		strHToolbarText = strHToolBarText + "<a href=""" + mstrOffset + "people/default.asp"">People</a> | "
	end if

	if strHScript = "/pictures/default.asp" then
		strHToolbarText = strHToolBarText + "Pictures | "
	else
		strHToolbarText = strHToolBarText + "<a href=""" + mstrOffset + "pictures/default.asp"">Pictures</a> | "
	end if

	if strHScript = "/quotes/default.asp" then
		strHToolbarText = strHToolBarText + "Quotes"
	else
		strHToolbarText = strHToolBarText + "<a href=""" + mstrOffset + "quotes/default.asp"">Quotes</a>"
	end if
end if

' Get links for this page
dim strAreaLink, strAreaButton, varAreaLinks(5,2), intAreaLinks, intAreaLinkLoop
dim strIcon

select case mstrArea
	case "archives"
		strIcon = "archives.gif"
		strAreaLink = "archives/"
		intAreaLinks = 3
		varAreaLinks(0,0) = "archives/default.asp"
		varAreaLinks(0,1) = "Archive Home"
		varAreaLinks(1,0) = "archives/morebeer.mp3"
		varAreaLinks(1,1) = "More Beer"
		varAreaLinks(2,0) = "archives/bac.asp"
		varAreaLinks(2,1) = "BAC"
	case "email"
		strIcon = "email.gif"
		strAreaLink = "email/"
		intAreaLinks = 1
		varAreaLinks(0,0) = "email/default.asp"
		varAreaLinks(0,1) = "Email Home"
	case "people"
		strIcon = "people.gif"
		strAreaLink = "people/"
		intAreaLinks = 2
		varAreaLinks(0,0) = "people/default.asp"
		varAreaLinks(0,1) = "People Home"
		varAreaLinks(1,0) = "people/view.asp"
		varAreaLinks(1,1) = "View People"
	case "pictures"
		strIcon = "pictures.gif"
		strAreaLink = "pictures/"
		intAreaLinks = 1
		varAreaLinks(0,0) = "pictures/default.asp"
		varAreaLinks(0,1) = "Pictures Home"
	case "quotes"
		strIcon = "quotes.gif"
		strAreaLink = "quotes/"
		intAreaLinks = 4
		varAreaLinks(0,0) = "quotes/default.asp"
		varAreaLinks(0,1) = "Quotes Home"
		varAreaLinks(1,0) = "quotes/add.asp"
		varAreaLinks(1,1) = "Add New Quote"
		varAreaLinks(2,0) = "quotes/view.asp?qty=5"
		varAreaLinks(2,1) = "View Quotes"
		varAreaLinks(3,0) = "quotes/default.asp"
		varAreaLinks(3,1) = "Search Quotes"
	case else
		strIcon = ""
		strAreaLink = ""
		intAreaLinks = 0
end select

%>

<!-- begin page header -->

<% if mblnDebug then %>
<table border="0" width="100%" bgColor="#C00000" cellspacing="0" cellpadding="0" align="center">
	<tr><td align="center"><b>B&nbsp;&nbsp;&nbsp;E&nbsp;&nbsp;&nbsp;T&nbsp;&nbsp;&nbsp;A</b></td></tr>
</table>
<% end if %>

<!-- Header table -->
<table border="0" cellpadding="0" cellspacing="0" width="100%">
	<tr>
<% if strIcon <> "" then %>
		<td width=40><a href="<%=mstrOffset + strAreaLink%>"><img src="<%=mstrOffset + "images/" + strIcon%>" border=0 width=32 height=32></a></td>
<% end if %>
		<td><font class="clsHeaderText"><%=mstrPageTitle%></font></td>
		<td><img src="<%=mstrOffset%>images/trans.gif" height="50" width="10"></td>
<% if not mblnLoginPage then %>
		<td align="right">
			<font class="clsNoteText">Hi <b><%=mstrName%></b><br><a href="<%=mstrOffset%>people/person/?id=<%=Request.Cookies("PersonID")%>">Change my info</a>&nbsp;|&nbsp;<a href="<%=mstrOffset%>logout.asp">Logout</a></font>
		</td>
<% end if %>
	</tr>
</table>
<!-- End header table -->

<!-- start toolbar -->
<table border="0" cellpadding="0" cellspacing="0" width="100%" class="clsToolbar">
	<tr>
		<td align="center"><font size="-1"><%=strHToolbarText%></font></td>
	</tr>
</table>
<!-- end toolbar -->

<!-- start main table -->

<table border="0" cellpadding="0" cellspacing="0" width="100%">
	<tr>
<% if not mblnLoginPage then %>
		<td class="clsToolbar" valign="top">
			<table>
				<tr>
					<td><%
							for intAreaLinkLoop = 0 to intAreaLinks-1
								Response.Write("<p><font class=""clsNoteText"">")
								Response.Write("<a href=""" + mstrOffset + varAreaLinks(intAreaLinkLoop,0) + """>")
								Response.Write(varAreaLinks(intAreaLinkLoop,1))
								Response.Write("</a></font></p>")
							next 
						%></td>
				</tr>
			</table>
		</td>
<% end if %>
		<td width="100%">
			<table border="0" cellpadding="0" cellspacing="0" align="center" width="95%">
				<tr>
					<td colspan="3"><img src="<%=mstrOffset%>images/trans.gif" width="5" height="5"></td>
				</tr>
				<tr>
					<td><img src="<%=mstrOffset%>images/trans.gif" width="5" height="350"></td>					
					<td width="100%" valign="top">

<!-- end page header -->