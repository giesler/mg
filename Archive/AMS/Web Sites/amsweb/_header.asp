<table width="100%" cellpadding="0" cellspacing="0" background="<%=strOffset%>img/top_wash.gif">
	<tr>
		<td width="108" rowspan="2">
			<a href="<%=strOffset%>"><img src="<%=strOffset%>img/logo_wash.jpg" width="130" height="120" border="0"></a>
		</td>
		<td width="500" height="100">
			<img src="<%=strOffset%>img/header.jpg" width="500" height="100">
		</td>
		<td align="right" height="100">
			<a href="<%=strOffset%>download/"><img src="<%=strOffset%>img/download_now.jpg" width="140" height="100" border="0"></a>
		</td>
		<td width="5" height="100">
			<img src="<%=strOffset%>img/trans.gif" width="5" height="100">
		</td>
	</tr>
	<tr>
		<td colspan="3" height="20" bgcolor="white">
			<img src="<%=strOffset%>img/trans.gif" width="5" height="5">
		</td>
	</tr>
</table>
<table width="100%" align="center" cellpadding="0" cellspacing="0">
	<tr>
		<td width="8" bgcolor="white">
		</td>
		<td width="125" bgcolor="white" valign="top">
			<%

dim strCurPage, blnAtRoot, strOriginalPage
strCurPage = Replace(Lcase(Request.ServerVariables("PATH_INFO")), "default.asp", "")
strOriginalPage = strCurPage

' compensate for root page request
blnAtRoot = (strCurPage = "/" or strCurPage = "/amsweb/")

if instr(strCurPage, "?") > 0 then
	strCurPage = left(strCurPage, Instr(strCurPage, "?")-1)
end if

function TreeHTML(parent, out, level)

	dim child, i, strCurURL

	' first output parent
	if level > 0 then
		strCurURL = parent.Attributes.getNamedItem("url").Text
		
		out = out & "<tr>"
		
		for i = 1 to level-1
			out = out & "<td width=""10""><img src=""" & strOffset & "img/trans.gif"" width=""10"" height=""5""></td>"
		next 
	    
	    if instr(strCurPage, strCurURL) > 0 then
			if parent.hasChildNodes then
				out = out & "<td><a class=""clsMenu"" href=""" & strOffset & strCurURL & """>"
				out = out & "<img src=""" & strOffset & "img/minus.gif"" border=""0""></a>&nbsp;</td>"
			else
				out = out & "<td><img src=""" & strOffset & "img/trans.gif"">&nbsp;</td>"
			end if
		else
			out = out & "<td>"
			if parent.hasChildNodes then
				out = out & "<a class=""clsMenu"" href=""" & strOffset & strCurURL & """>"
				out = out & "<img border=""0"" src=""" & strOffset & "img/plus.gif""></a>&nbsp;</td>"
			else
				out = out & "<img border=""0"" src=""" & strOffset & "img/trans.gif""></a>&nbsp;</td>"
			end if
		end if
	    
	    ' see if in area now, unless we are at root node
		out = out & "<td colspan=""" & (5-level) & """>"
		if blnAtRoot and strCurURL = "" then
			out = out & "<b class=""clsMenu"">" & Replace(parent.Attributes.getNamedItem("name").Text, " ", "&nbsp;") & "</b><br>"
	    elseif right(strCurPage, len(strCurURL)) = strCurURL and strCurURL <> "" then
			out = out & "<b class=""clsMenu"">" & Replace(parent.Attributes.getNamedItem("name").Text, " ", "&nbsp;") & "</b><br>"
		else
			' add hyperlink
			out = out & "<a class=""clsMenu"" href=""" & strOffset & strCurURL & """>"
			' Add name
			out = out & Replace(parent.Attributes.getNamedItem("name").Text, " ", "&nbsp;") & "</a><br>"
		end if

		out = out & "</td></tr>"

	end if
	
	' if children, go through them
	if parent.hasChildNodes and (instr(strCurPage, strCurURL) > 0 or level=0)   then
		for each child in parent.childNodes
			TreeHTML child, out, level+1
		next
	end if

end function

dim objXML, out, objXMLDocEl

on error resume next
set objXMLDocEl = Application("XMLMenu")
' load xml doc if not loaded
if objXMLDocEl is nothing or err then
  on error goto 0
  set objXML = Server.CreateObject("MSXML2.FreeThreadedDOMDocument")
  objXML.async = false
  objXML.load server.MapPath(strOffset & "menu.xml")
  set Application("XMLMenu") = objXML.documentElement
  set objXMLDocEl = Application("XMLMenu")
end if 
on error goto 0

TreeHTML objXMLDocEl, out, 0
%>
			<table cellpadding="0" cellspacing="0" border="0">
				<%=out%>
			</table>
			<br>
			<img src="img/trans.gif" height="100" width="1">
		</td>
		<td width="8" bgcolor="white">
			<img src="<%=strOffset%>img/trans.gif" height="8" width="8">
		</td>
		<td bgcolor="White" valign="top">
<!-- main content -->
