<html>

<head>
<meta http-equiv="Content-Language" content="en-us">
<meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
<meta name="GENERATOR" content="Microsoft FrontPage 4.0">
<meta name="ProgId" content="FrontPage.Editor.Document">
<title>Thanks</title>
<style fprolloverstyle>A:hover {color: #FF0000; font-family: Arial}</style>
<meta name="Microsoft Theme" content="mikes-website 011, default">
<meta name="Microsoft Border" content="tlb, default">
</head>

<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" background="../_themes/mikes-website/blackbg.gif" bgcolor="#000000" text="#FFFFFF" link="#FFFF00" vlink="#FFFF99" alink="#FFCC99"><!--msnavigation--><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td><!--mstheme--><font face="Arial, Helvetica">

<!--mstheme--></font><table border="0" width="100%" cellpadding="0" cellspacing="0">
  <tr>
    <td width="100" bgcolor="#000000"><!--mstheme--><font face="Arial, Helvetica"><img border="0" src="../_borders/mikespage.gif" width="100" height="30"><!--mstheme--></font></td>
    <td bgcolor="#000000" width="20"><!--mstheme--><font face="Arial, Helvetica"><img border="0" src="../_borders/black20x30filler.gif" width="20" height="30"><!--mstheme--></font></td>
    <td bgcolor="#C00000" background="../_borders/bl-red_fade_top.jpg" width="400" height="30"><!--mstheme--><font face="Arial, Helvetica"><font size="6"><strong></strong></font><!--mstheme--></font></td>
    <td bgcolor="#C00000" align="right" valign="middle"><!--mstheme--><font face="Arial, Helvetica"><a href="http://giesler.org"><img border="0" src="../_borders/giesler.org.gif" width="18" height="25" alt="Go to giesler.org" align="middle"></a>&nbsp;
    <!--mstheme--></font></td>
  </tr>
</table><!--mstheme--><font face="Arial, Helvetica">

<!--mstheme--></font></td></tr><!--msnavigation--></table><!--msnavigation--><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td valign="top" width="1%"><!--mstheme--><font face="Arial, Helvetica">

<div align="center">
  <center>
  <!--mstheme--></font><table border="0" cellpadding="0" cellspacing="0" width="100" bgcolor="#C00000" height="100%">
    <tr>
      <td valign="top" align="center" bgcolor="#000000" width="100" height="20"><!--mstheme--><font face="Arial, Helvetica"><img border="0" src="../_borders/black100x20filler.gif" width="100" height="20"><!--mstheme--></font></td>
    </tr>
    <tr>
      <td background="../_borders/bl-red_fade_side.jpg" valign="top" align="center" bgcolor="#000000" width="100" height="400"><!--mstheme--><font face="Arial, Helvetica">
        
        <p>&nbsp;</p>
      <!--mstheme--></font></td>
    </tr>
    <tr>
      <td valign="middle" align="center" width="100" height="200"><!--mstheme--><font face="Arial, Helvetica"><a href="../stuff/toc.html"><img border="0" src="../_borders/border_toc-ad.gif" width="48" height="31"></a><!--mstheme--></font></td>
    </tr>
    <tr>
      <td width="100" valign="bottom" align="center"><!--mstheme--><font face="Arial, Helvetica">
        <p>&nbsp;</p>
      <!--mstheme--></font></td>
    </tr>
  </table><!--mstheme--><font face="Arial, Helvetica">
  </center>
</div>

<!--mstheme--></font></td><td valign="top" width="24"></td><!--msnavigation--><td valign="top"><!--mstheme--><font face="Arial, Helvetica">

<%

if IsEmpty(Request("Message")) Then
	Response.Redirect("default.asp")
end if

dim oMsg
set oMsg = Server.CreateObject("CDONTS.NewMail")

oMsg.To = "Mike Giesler <mike@giesler.org>"
oMsg.From = Request.Form("Name") + "<" + Request.Form("Email") + ">"
oMsg.Subject = "mike.giesler.org email: " + Request("subject")
oMsg.Body = Request.Form("message")
oMsg.Send
set omsg = nothing

%>

<p>&nbsp;</p>
<p>Thanks for you email.</p>
<p>&nbsp;</p>
<p>Have a nice day.</p>
<p>&nbsp;</p>
<p>Back to my <a href="../Default.asp">homepage</a>.<!--mstheme--></font><!--msnavigation--></td></tr><!--msnavigation--></table><!--msnavigation--><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td><!--mstheme--><font face="Arial, Helvetica">

<div align="center">
  <center>
  <!--mstheme--></font><table border="0" cellpadding="0" cellspacing="0" width="100%" height="30">
    <tr>
      <td width="100%" valign="middle" align="center" bgcolor="#C00000"><!--mstheme--><font face="Arial, Helvetica">
        <div align="center">
          <!--mstheme--></font><table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
              <td valign="middle" align="center" width="100"><!--mstheme--><font face="Arial, Helvetica"><a href="http://www.microsoft.com/windows2000" target="_blank"><img border="0" src="../_borders/w2ksplash.gif" width="31" height="20" alt="Powered by Windows 2000"></a><!--mstheme--></font></td>
              <td valign="middle" align="center"><!--mstheme--><font face="Arial, Helvetica"><font size="1"> <a href="http://mike.giesler.org">mike.giesler.org</a>,
                a <a href="http://giesler.org">giesler.org</a> site</font><!--mstheme--></font></td>
              <td valign="middle" align="right" width="100"><!--mstheme--><font face="Arial, Helvetica"><img border="0" src="../_borders/border_copyright.gif" width="80" height="27"><!--mstheme--></font></td>
            </tr>
          </table><!--mstheme--><font face="Arial, Helvetica">
        </div>
      <!--mstheme--></font></td>
    </tr>
  </table><!--mstheme--><font face="Arial, Helvetica">
  </center>
</div>

<!--mstheme--></font></td></tr><!--msnavigation--></table></body>

</html>
