<html>
<script LANGUAGE="VBScript" RUNAT="SERVER">

const datStart = "July 24, 1999 5:25 pm"
const datEnd = "August 14, 1999 9:20 pm"

Function CalcLength(time1, time2)

dim days, hours, mins

days = DateDiff("d", time1, time2)
hours = DateDiff("h", time1, time2) - (days * 24)
mins = DateDiff("n", time1, time2) - (days * 24 * 60) - (hours * 60)
if mins < 0 then
	hours = hours - 1
	mins = mins + 60
end if
if hours < 0 then
	days = days - 1
	hours = hours + 24
end if
if days < 0 then
	days = days + 1
	hours = hours - 24
end if
if hours < 0 then
	hours = hours + 1
	mins = mins - 60
end if

if hours < 0 or mins < 0 or days < 0 then
	prefix = "-"
	days = abs(days)
	hours = abs(hours)
	mins = abs(mins)
else
	prefix = ""
end if

if mins < 10 and mins > -10 then
	mins = "0" & mins
end if
if hours < 10 and hours > -10 then
	hours = "0" & hours
end if
if days < 10 and days > -10 then
	days = "0" & days
end if

CalcLength = prefix & days & ":" & hours & ":" & mins

end function

function outputSpacedDate(datIn)

dim strOut

for i = 1 to len(datIn)
	if i = len(datIn) then
		strOut = strOut & mid(datin,i,1)
	else
		strOut = strOut & mid(datin,i,1) & "&nbsp;"
	end if
next

Response.Write strOut

end function

</script>

<meta http-equiv=Refresh content=60>

<head>
<title>suds</title>
<meta name="Microsoft Theme" content="mikes-website 011, default">
</head>

<body topMargin=0 leftMargin=0 rightMargin=0 bottomMargin=0 background="_themes/mikes-website/blackbg.gif" bgcolor="#000000" text="#FFFFFF" link="#FFFF00" vlink="#FFFF99" alink="#FFCC99"><!--mstheme--><font face="Arial, Helvetica">

<center><font face=Arial size=-2>
<% outputSpacedDate("mpg") %> &nbsp; &amp; &nbsp; <% outputSpacedDate("sds") %>
</font></center>

<!--mstheme--></font><table width="95%" height="95%">
	<tr align = middle valign = center>
	<td align = middle valign = center><!--mstheme--><font face="Arial, Helvetica">
	
	<!--mstheme--></font><table cellpadding=5>
		<tr><td colspan=2 align=middle><!--mstheme--><font face="Arial, Helvetica">
			<font face=Arial size=-2>total</font><br></FONT>
			<!--mstheme--></font><table border=1 cellpadding=3 cellspacing=0 align=center borderColor=silver bordercolordark="#FFFFFF" bordercolorlight="#FFFFFF"><tr><td><!--mstheme--><font face="Arial, Helvetica"><font face=Arial size=-1>
			<% Response.Write(CalcLength(datStart, datEnd)) %>
			</font><!--mstheme--></font></td></tr></table><!--mstheme--><font face="Arial, Helvetica">
		<!--mstheme--></font></td></tr>
		<tr><td align=middle><!--mstheme--><font face="Arial, Helvetica">
			<font face=Arial size=-2>elapsed</font><br>
			<!--mstheme--></font><table border=1 cellpadding=3 cellspacing=0 align=center borderColor=silver bordercolordark="#FFFFFF" bordercolorlight="#FFFFFF"><tr><td><!--mstheme--><font face="Arial, Helvetica"><font face=Arial size=-1>
			<% Response.Write(CalcLength(datStart, Now)) %>
			</font><!--mstheme--></font></td></tr></table><!--mstheme--><font face="Arial, Helvetica">
		<!--mstheme--></font></td><td align=middle><!--mstheme--><font face="Arial, Helvetica">
			<font face=Arial size=-2>remaining</font><br>
			<!--mstheme--></font><table border=1 cellpadding=3 cellspacing=0 align=center borderColor=silver bordercolordark="#FFFFFF" bordercolorlight="#FFFFFF"><tr><td><!--mstheme--><font face="Arial, Helvetica"><font face=Arial size=-1>
			<% Response.Write(CalcLength(Now, datEnd)) %>
			</font><!--mstheme--></font></td></tr></table><!--mstheme--><font face="Arial, Helvetica">
			
		<!--mstheme--></font></td></tr>
	</table><!--mstheme--><font face="Arial, Helvetica">      
	
<!--mstheme--></font></td></tr>
</table><!--mstheme--><font face="Arial, Helvetica">

<center><font face=Arial size=-2>
<% outputSpacedDate(datStart) %> &nbsp; - &nbsp; <% outputSpacedDate(datEnd) %> &nbsp;
<% outputSpacedDate("CST") %>
</font></center>

<!--mstheme--></font></body>
</html>


