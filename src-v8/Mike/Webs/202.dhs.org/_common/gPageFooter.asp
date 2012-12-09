
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

<br>

<%
' ------------------------------------------------------------------------
' All debug stuff below
' ------------------------------------------------------------------------

if Request.Cookies("debug") = "1" then %>
<table bgColor="black">
	<tr>
		<td valign=top>
		<table cellspacing=0 cellpadding=0><tr><td><font color="white" size=-1><b>HTTP Headers</font></td></tr>
<%
dim varTempFoot, intTempFoot

varTempFoot = Split(Request.ServerVariables("ALL_HTTP"), "HTTP_")
for intTempFoot = LBound(varTempFoot) to UBound(varTempFoot)
	Response.Write("<tr><td><font color=""white"" size=""-2"">HTTP_" + CStr(varTempFoot(intTempFoot)) + "</font></td></tr>")
next

%>	
		</table>	
		</td>
		<td valign=top>
<table cellspacing=0 cellpadding=0><tr><td><font color="white" size=-1><b>Session Data</font></td></tr>
<%

'varTempFoot = Split(mobjSession.SessionData("<br>"), "<br>")
'for intTempFoot = LBound(varTempFoot) to UBound(varTempFoot)
'	Response.Write("<tr><td><font color=""white"" size=""-2"">" + CStr(varTempFoot(intTempFoot)) + "</font></td></tr>")
'next

%>	
		</table>		
		</td>
	</tr>
<%
end if %>

</table>

<!-- end page footer -->

<%
' Close connection if opened
if cnWebDB.State = adStateOpen then cnWebDB.Close
%>