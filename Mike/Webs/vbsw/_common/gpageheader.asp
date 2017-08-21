
<%
' Figure out toolbar text
dim strHScript, strHToolbarText

strHScript = Request.ServerVariables("SCRIPT_NAME")

if strHScript = "/default.asp" then
	strHToolbarText = strHToolBarText + "Home | "
else
	strHToolbarText = strHToolBarText + "<a href=""" + mstrOffset + "default.asp"">Home</a> | "
end if

if InStr(strHScript, "/download/") > 0 then
	strHToolbarText = strHToolBarText + "Download | "
else
	strHToolbarText = strHToolBarText + "<a href=""" + mstrOffset + "download/"">Download</a> | "
end if

if InStr(strHScript, "/docs/") > 0 then
	strHToolbarText = strHToolBarText + "Documentation | "
else
	strHToolbarText = strHToolBarText + "<a href=""" + mstrOffset + "docs/"">Documentation</a> | "
end if

if InStr(strHScript, "/feedback/") > 0 then
	strHToolbarText = strHToolBarText + "Feedback / Help | "
else
	strHToolbarText = strHToolBarText + "<a href=""" + mstrOffset + "feedback/"">Feedback / Help</a> | "
end if

if InStr(strHScript, "/maillist/") > 0 then
	strHToolbarText = strHToolBarText + "Mail List | "
else
	strHToolbarText = strHToolBarText + "<a href=""" + mstrOffset + "maillist/"">Mail List</a> | "
end if

if InStr(strHScript, "/about/") > 0 then
	strHToolbarText = strHToolBarText + "About"
else
	strHToolbarText = strHToolBarText + "<a href=""" + mstrOffset + "about/"">About</a>"
end if

' Get links for this page
dim strAreaLink, strAreaButton, varAreaLinks(5,2), intAreaLinks, intAreaLinkLoop
dim strIcon

select case mstrArea
	case "download"
		strIcon = "download.gif"
		strAreaLink = "download/"
		intAreaLinks = 1
		varAreaLinks(0,0) = "download/"
		varAreaLinks(0,1) = "Download Home"
	case "docs"
		strIcon = "docs.gif"
		strAreaLink = "docs/"
		intAreaLinks = 3
		varAreaLinks(0,0) = "docs/"
		varAreaLinks(0,1) = "Docs Home"
		varAreaLinks(1,0) = "docs/default.asp"
		varAreaLinks(1,1) = "v2.0x Docs"
		varAreaLinks(2,0) = "docs/docs100.asp"
		varAreaLinks(2,1) = "v1.0x Docs"
	case "feedback"
		strIcon = "feedback.gif"
		strAreaLink = "feedback/"
		intAreaLinks = 1
		varAreaLinks(0,0) = "feedback/"
		varAreaLinks(0,1) = "Feedback<br>/Help Home"
	case "about"
		strIcon = "about.gif"
		strAreaLink = "about/"
		intAreaLinks = 1
		varAreaLinks(0,0) = "about/"
		varAreaLinks(0,1) = "About Home"
	case "maillist"
		strAreaLink = "maillist/"
		intAreaLinks = 1
		varAreaLinks(0,0) = "maillist/"
		varAreaLinks(0,1) = "Mail List Home"
	case else
		strIcon = ""
		strAreaLink = ""
		intAreaLinks = 0
end select

%>

<!-- begin page header -->

<!-- Header table -->
<table border="0" cellpadding="0" cellspacing="0" width="100%">
	<tr>
		<td width=40><a href="<%=mstrOffset%>default.asp"><img src="<%=mstrOffset + "images/wininsticon.gif"%>" border=0 width=32 height=32></a></td>
		<td><font class="clsHeaderText"><%=mstrPageTitle%></font></td>
		<td><img src="<%=mstrOffset%>images/trans.gif" height="50" width="10"></td>
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
		<td width="100%">
			<table border="0" cellpadding="0" cellspacing="0" align="center" width="95%">
				<tr>
					<td colspan="3"><img src="<%=mstrOffset%>images/trans.gif" width="5" height="5"></td>
				</tr>
				<tr>
					<td><img src="<%=mstrOffset%>images/trans.gif" width="5" height="350"></td>					
					<td width="100%" valign="top">

<!-- end page header -->