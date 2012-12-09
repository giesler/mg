
<!-- begin page footer -->

					</td>
					<td><img src="<%=mstrOffset%>images/trans.gif" width="5" height="350"></td>
				</tr><tr>
					<td colspan="3"><img src="<%=mstrOffset%>images/trans.gif" width="5" height="5"></td>
				</tr>
			</table>
		</td>
	</tr>
</table>
<!-- end main table -->

<!-- start toolbar -->
<table cellpadding="0" cellspacing="0" width="100%" class="clsToolbar">
	<tr>
		<td align="center"><font size="-1"><%=strHToolbarText%></font></td>
	</tr>
</table>
<!-- end toolbar -->

<table width="100%" border="0" cellpadding="0" cellspacing="0" align="center">
	<tr>
		<td width="33%" align="left"><a href="http://giesler.org"><img src="<%=mstrOffset%>images/giesler.org.gif" WIDTH="22" HEIGHT="28" border=0></a></td>
		<td width="34%" align="center"><font size=-2><a href="http://giesler.org">giesler.org</a><br>&copy;2001, Mike Giesler, <a href="mailto:mike@giesler.org">mike@giesler.org</a></font></td>
		<td width="33%" align="right"><a href="http://www.microsoft.com/windows2000"><img src="<%=mstrOffset%>images/w2k.gif" WIDTH="100" HEIGHT="33" border=0></a></td>
	</tr>
</table>

<!-- end page footer -->

<%
' Close connection if opened
if cnWebDB.State = adStateOpen then cnWebDB.Close
%>