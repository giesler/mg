<html>

<head>
<meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
<meta name="GENERATOR" content="Microsoft FrontPage 4.0">
<meta name="ProgId" content="FrontPage.Editor.Document">
<title>Confirm Page Information</title>
<meta name="Microsoft Theme" content="mikes-website 011, default">
<meta name="Microsoft Border" content="tlb, default">
</head>

<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" background="../_themes/mikes-website/blackbg.gif" bgcolor="#000000" text="#FFFFFF" link="#FFFF00" vlink="#FFFF99" alink="#FFCC99"><!--msnavigation--><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td><!--mstheme--><font face="Arial, Helvetica">

<!--mstheme--></font><table border="0" width="100%" cellpadding="0" cellspacing="0">
  <tr>
    <td width="100" bgcolor="#000000"><!--mstheme--><font face="Arial, Helvetica"><a href="../Default.asp"><img border="0" src="../_borders/mikespage.gif"></a><!--mstheme--></font></td>
    <td bgcolor="#000000" width="20"><!--mstheme--><font face="Arial, Helvetica"><img border="0" src="../_borders/black20x30filler.gif"><!--mstheme--></font></td>
    <td bgcolor="#C00000" background="../_borders/bl-red_fade_top.jpg" width="400" height="30"><!--mstheme--><font face="Arial, Helvetica"><font size="6"><strong></strong></font><!--mstheme--></font></td>
    <td bgcolor="#C00000" align="right" valign="middle"><!--mstheme--><font face="Arial, Helvetica"><a href="http://giesler.org"><img border="0" src="../_borders/giesler.png" alt="Go to giesler.org" width="22" height="30"></a>&nbsp;
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
<p> &nbsp; </p>
<p> &nbsp; </p>
<%
if IsEmpty(Request("Message")) Then
	Response.Redirect("default.asp")
end if

dim oMsg
set oMsg = Server.CreateObject("CDONTS.NewMail")

oMsg.To = "Mike Giesler <mike@giesler.org>"
oMsg.From = "mike.giesler.org"
oMsg.Subject = "Page to " + Request("dest")
oMsg.Body = Request.Form("message")
oMsg.Send
set omsg = nothing

if Request("dest") = "Mobilecomm Pager" Then
	Response.Write("<form action=""http://www.mobilecomm.com/cgi-bin/wwwpage.exe"" method=""POST"">")
	Response.Write("<INPUT TYPE=""hidden"" NAME=""MSSG"" VALUE=""" + Request("Message") + """>")
	Response.Write("<INPUT TYPE=""hidden"" NAME=""Q1"" VALUE=""1"">")
	Response.Write("<INPUT TYPE=""hidden"" NAME=""PIN"" VALUE=""4149418205"">")
Else
	Response.Write("<form action=""http://www.nextel.com/cgi-bin/sendPage.cgi"" method=""POST"">")
	Response.Write("<INPUT TYPE=""hidden"" NAME=""message"" VALUE=""" + Request("Message") + """>")
	Response.Write("<INPUT TYPE=""hidden"" NAME=""type"" VALUE=""single"">")
	Response.Write("<INPUT TYPE=""hidden"" NAME=""SEND"" VALUE=""SEND"">")
	Response.Write("<INPUT TYPE=""hidden"" NAME=""action"" VALUE=""send"">")
	Response.Write("<INPUT TYPE=""hidden"" NAME=""to01"" VALUE=""4147702250"">")
End If
%>
<p>

&nbsp;

<p>&nbsp;</p>
<div align="center">
  <center>
<!--mstheme--></font><table border="0" cellpadding="8" cellspacing="0" bgcolor="#000080">
  <tr>
     <td ALIGN="RIGHT" VALIGN="TOP"><!--mstheme--><font face="Arial, Helvetica"><font size="+0" face="Arial" color="#FFFFFF"><b>Destination:</b></font><!--mstheme--></font></td>
     <td><!--mstheme--><font face="Arial, Helvetica"><%=Request("dest")%><!--mstheme--></font></td>
  </tr>
     <tr>
     <td ALIGN="RIGHT" VALIGN="TOP"><!--mstheme--><font face="Arial, Helvetica"><font SIZE="+0" face="Arial" color="#FFFFFF">
          <b>Message:</b></font><!--mstheme--></font></td>
     <td><!--mstheme--><font face="Arial, Helvetica"><% 	Response.Write(Request("Message")) %><!--mstheme--></font></td>
  </tr>
  <tr>
    <td align="center" colspan="2"><!--mstheme--><font face="Arial, Helvetica"><font face="Arial" color="#FFFFFF">
	<INPUT TYPE="SUBMIT" VALUE="Continue">
	</FORM>
<FORM method="POST" action="default.asp"><INPUT TYPE="SUBMIT" VALUE="Back"></FORM>
 </font><!--mstheme--></font></td>
  </tr>
</table><!--mstheme--><font face="Arial, Helvetica">
  </center>
</div>
<p> &nbsp; </p>

&nbsp;<!--mstheme--></font><!--msnavigation--></td></tr><!--msnavigation--></table><!--msnavigation--><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td><!--mstheme--><font face="Arial, Helvetica">

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

