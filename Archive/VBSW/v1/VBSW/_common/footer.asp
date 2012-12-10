<% if Request.QueryString("print") <> "1" then ' hides stuff for print only view %>

			<!-- END CONTENT HERE -->
		</td>
	</tr>
</table>
<!-- end main table -->
<table width="100%" border="0" cellpadding="0" cellspacing="0" align="center" class="clsHeader">
	<tr>
		<td align="center"><font size="-1"><a href="<%=sOffset%>default.asp">Home</a> | <a href="<%=sOffset%>download">Download</a> | <a href="<%=sOffset%>docs">Documentation</a> | <a href="<%=sOffset%>feedback">Feedback / Help</a> | <a href="<%=sOffset%>about">About</a></font></td>
	</tr>
</table>

<table width="100%" border="0" cellpadding="0" cellspacing="0" align="center">
	<tr>
		<td width="33%" align="left"><a href="http://giesler.org"><img src="<%=sOffset%>_common/image/giesler.org.gif" WIDTH="22" HEIGHT="28" border=0></a></td>
		<td width="34%" align="center"><font size=-2><a href="http://giesler.org">giesler.org</a><br>&copy;2000, Mike Giesler</font></td>
		<td width="33%" align="right"><a href="http://www.microsoft.com/windows2000"><img src="<%=sOffset%>_common/image/w2k.gif" WIDTH="100" HEIGHT="33" border=0></a></td>
	</tr>
</table>

<% end if %>

<% if Request.cookies("eSAMI")("debug") = "True" then %>
<br><br><hr noshade>
<table bgcolor="#dcffff" align="center" cellpadding="3" border="1" cellspacing="0">
	<tr><td colSpan="2"><font size="-1">Debug Information</font></td></tr>
	<tr><td valign="top"><font size="-1">Script name:<br> <%=Request.ServerVariables("SCRIPT_NAME")%><br><br><%=TailCut(Request.ServerVariables("SCRIPT_NAME"), "/")%></td>
<td>
<table align="center" cellpadding="4" border="1" cellspacing="0">
<%
dim sFTmp, vFArr, iFCurArr
vFArr = Split(Request.QueryString,"&")
for iFCurArr = 0 to UBound(vFArr)
	sFTmp = vFArr(iFCurArr)
	if Instr(sFTmp, "=") > 0 then 
		rw("<tr><td><font size=-1>" + Left(sFTmp, Instr(sFTmp, "=") - 1) + "</td>")
		rw("<td><font size=-1>" + rq(Left(sFTmp, Instr(sFTmp, "=") - 1)) + "</td></tr>")
	end if
next
%>
</table>
</td></tr></table>
<% 
end if 
%>

</body>
</html>

