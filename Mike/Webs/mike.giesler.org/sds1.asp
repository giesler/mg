<html>
<head>
<meta name="Microsoft Theme" content="mikes-website 011, default">
<body background="_themes/mikes-website/blackbg.gif" bgcolor="#000000" text="#FFFFFF" link="#FFFF00" vlink="#FFFF99" alink="#FFCC99"><!--mstheme--><font face="Arial, Helvetica">

<title>To SDS</title>

<%
Public varInt
Set varInt = Request.QueryString("i")

Function curIter 
if varInt = "" then
   varInt = 0
end if
if varInt = 21 then
	varInt = -1
end if
curIter = varInt
End Function

Function curString

select case varInt
   case 0
      curString = "Hi."
   case 1
      curString = "I"
   case 2
      curString = "just"
   case 3
      curString = "thought"
   case 4
      curString = "I'd"
   case 5
      curString = "say"
   case 6
      curString = "er"
   case 7
      curString = "type"
   case 8
      curString = "how"
   case 9
      curString = "very"
   case 10
      curString = "very"
   case 11
      curString = "much"
   case 12
      curString = "I"
   case 13
      curString = "love"
   case 14
      curString = "you"
   case 15
      curString = "!"
   case 16
      curString = "Smile!"
   case 17
      curString = ";)"
   case 18
      curString = "See"
   case 19
      curString = "u"
   case 20
      curString = "soon!"
   case -1
      curString = "(<a href='sds1.asp'>repeat</a>)<font size=-1><BR><a href='http://mg.dhs.org'>mg.dhs.org</a></font>"
end select
curStr = varStr
End Function
%>
<% if CurIter() <> -1 then %>
<meta http-equiv="REFRESH" content="1; URL=sds1.asp?i=<%Response.Write (CurIter() + 1) %> &gt;
<% end if %>
&lt;/HEAD&gt;
&lt;body bgcolor=" text="#ffffff" link="#FF0000" vlink="#FF0000" alink="#FFFF00" grey">

<!--mstheme--></font><table border="0" cellpadding="0" cellspacing="0" width="100%" height="98%">
<tr><td width="100%"><!--mstheme--><font face="Arial, Helvetica"><center>
<!--mstheme--></font><table border="1" cellpadding="18" cellspacing="0" bgcolor="Maroon" bordercolordark="#FFFFFF" bordercolorlight="#FFFFFF">
<tr><td><!--mstheme--><font face="Arial, Helvetica"><center><b><font face="Arial" size="+8" color="White">
<%Response.Write (curString()) %>
<!--mstheme--></font></td></tr></table><!--mstheme--><font face="Arial, Helvetica">
<!--mstheme--></font></td></tr></table><!--mstheme--><font face="Arial, Helvetica">

<!--mstheme--></font></body>
</html>

