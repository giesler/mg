<html>
<script LANGUAGE="VBScript" RUNAT="SERVER">
Function CalcAge(bday) 
    bdayDate = CDate(bday)
    off = 0
    If CDate(DatePart("m", bday) & "/" & DatePart("d", bday) & "/" & Year(Now)) > Now Then off = 1
    calcage = DateDiff("yyyy", bday, Now) - off
End Function
</script>


<head>
<title>Current Family Info</title>
<style fprolloverstyle>A:hover {color: #FF0000; font-family: Arial}</style>
<meta name="Microsoft Theme" content="mikes-website 011, default">
<meta name="Microsoft Border" content="tlb, default">
</head>

<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" background="../../_themes/mikes-website/blackbg.gif" bgcolor="#000000" text="#FFFFFF" link="#FFFF00" vlink="#FFFF99" alink="#FFCC99"><!--msnavigation--><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td><!--mstheme--><font face="Arial, Helvetica">

<!--mstheme--></font><table border="0" width="100%" cellpadding="0" cellspacing="0">
  <tr>
    <td width="100" bgcolor="#000000"><!--mstheme--><font face="Arial, Helvetica"><img border="0" src="../../_borders/mikespage.gif" width="100" height="30"><!--mstheme--></font></td>
    <td bgcolor="#000000" width="20"><!--mstheme--><font face="Arial, Helvetica"><img border="0" src="../../_borders/black20x30filler.gif" width="20" height="30"><!--mstheme--></font></td>
    <td bgcolor="#C00000" background="../../_borders/bl-red_fade_top.jpg" width="400" height="30"><!--mstheme--><font face="Arial, Helvetica"><font size="6"><strong><img src="_derived/curinfo.asp_cmp_mikes-website010_bnr.gif" width="400" height="30" border="0" alt="Current Family Info"></strong></font><!--mstheme--></font></td>
    <td bgcolor="#C00000" align="right" valign="middle"><!--mstheme--><font face="Arial, Helvetica"><a href="http://giesler.org"><img border="0" src="../../_borders/giesler.org.gif" width="18" height="25" alt="Go to giesler.org" align="middle"></a>&nbsp;
    <!--mstheme--></font></td>
  </tr>
</table><!--mstheme--><font face="Arial, Helvetica">

<!--mstheme--></font></td></tr><!--msnavigation--></table><!--msnavigation--><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td valign="top" width="1%"><!--mstheme--><font face="Arial, Helvetica">

<div align="center">
  <center>
  <!--mstheme--></font><table border="0" cellpadding="0" cellspacing="0" width="100" bgcolor="#C00000" height="100%">
    <tr>
      <td valign="top" align="center" bgcolor="#000000" width="100" height="20"><!--mstheme--><font face="Arial, Helvetica"><img border="0" src="../../_borders/black100x20filler.gif" width="100" height="20"><!--mstheme--></font></td>
    </tr>
    <tr>
      <td background="../../_borders/bl-red_fade_side.jpg" valign="top" align="center" bgcolor="#000000" width="100" height="400"><!--mstheme--><font face="Arial, Helvetica">
        <script language="JavaScript"><!--
MSFPhover = 
  (((navigator.appName == "Netscape") && 
  (parseInt(navigator.appVersion) >= 3 )) || 
  ((navigator.appName == "Microsoft Internet Explorer") && 
  (parseInt(navigator.appVersion) >= 4 ))); 
function MSFPpreload(img) 
{
  var a=new Image(); a.src=img; return a; 
}
// --></script><script language="JavaScript"><!--
if(MSFPhover) { MSFPnav1n=MSFPpreload("../../_derived/home_cmp_mikes-website010_hbtn.gif"); MSFPnav1h=MSFPpreload("../../_derived/home_cmp_mikes-website010_hbtn_a.gif"); }
// --></script><a href="../../" language="JavaScript" onmouseover="if(MSFPhover) document['MSFPnav1'].src=MSFPnav1h.src" onmouseout="if(MSFPhover) document['MSFPnav1'].src=MSFPnav1n.src"><img src="../../_derived/home_cmp_mikes-website010_hbtn.gif" width="80" height="20" border="0" alt="Home" align="middle" name="MSFPnav1"></a> <script language="JavaScript"><!--
if(MSFPhover) { MSFPnav2n=MSFPpreload("../../_derived/up_cmp_mikes-website010_hbtn.gif"); MSFPnav2h=MSFPpreload("../../_derived/up_cmp_mikes-website010_hbtn_a.gif"); }
// --></script><a href="index.html" language="JavaScript" onmouseover="if(MSFPhover) document['MSFPnav2'].src=MSFPnav2h.src" onmouseout="if(MSFPhover) document['MSFPnav2'].src=MSFPnav2n.src"><img src="../../_derived/up_cmp_mikes-website010_hbtn.gif" width="80" height="20" border="0" alt="Up" align="middle" name="MSFPnav2"></a> <script language="JavaScript"><!--
if(MSFPhover) { MSFPnav3n=MSFPpreload("../../aboutme/_derived/index.html_cmp_mikes-website010_hbtn.gif"); MSFPnav3h=MSFPpreload("../../aboutme/_derived/index.html_cmp_mikes-website010_hbtn_a.gif"); }
// --></script><a href="../../aboutme/index.html" language="JavaScript" onmouseover="if(MSFPhover) document['MSFPnav3'].src=MSFPnav3h.src" onmouseout="if(MSFPhover) document['MSFPnav3'].src=MSFPnav3n.src"><img src="../../aboutme/_derived/index.html_cmp_mikes-website010_hbtn.gif" width="80" height="20" border="0" alt="About Me" align="middle" name="MSFPnav3"></a> <script language="JavaScript"><!--
if(MSFPhover) { MSFPnav4n=MSFPpreload("../_derived/index.html_cmp_mikes-website010_hbtn.gif"); MSFPnav4h=MSFPpreload("../_derived/index.html_cmp_mikes-website010_hbtn_a.gif"); }
// --></script><a href="../index.html" language="JavaScript" onmouseover="if(MSFPhover) document['MSFPnav4'].src=MSFPnav4h.src" onmouseout="if(MSFPhover) document['MSFPnav4'].src=MSFPnav4n.src"><img src="../_derived/index.html_cmp_mikes-website010_hbtn.gif" width="80" height="20" border="0" alt="Family" align="middle" name="MSFPnav4"></a> <script language="JavaScript"><!--
if(MSFPhover) { MSFPnav5n=MSFPpreload("../../friends/_derived/index.html_cmp_mikes-website010_hbtn.gif"); MSFPnav5h=MSFPpreload("../../friends/_derived/index.html_cmp_mikes-website010_hbtn_a.gif"); }
// --></script><a href="../../friends/index.html" language="JavaScript" onmouseover="if(MSFPhover) document['MSFPnav5'].src=MSFPnav5h.src" onmouseout="if(MSFPhover) document['MSFPnav5'].src=MSFPnav5n.src"><img src="../../friends/_derived/index.html_cmp_mikes-website010_hbtn.gif" width="80" height="20" border="0" alt="Friends" align="middle" name="MSFPnav5"></a> <script language="JavaScript"><!--
if(MSFPhover) { MSFPnav6n=MSFPpreload("../../sara/_derived/index.html_cmp_mikes-website010_hbtn.gif"); MSFPnav6h=MSFPpreload("../../sara/_derived/index.html_cmp_mikes-website010_hbtn_a.gif"); }
// --></script><a href="../../sara/index.html" language="JavaScript" onmouseover="if(MSFPhover) document['MSFPnav6'].src=MSFPnav6h.src" onmouseout="if(MSFPhover) document['MSFPnav6'].src=MSFPnav6n.src"><img src="../../sara/_derived/index.html_cmp_mikes-website010_hbtn.gif" width="80" height="20" border="0" alt="Sara" align="middle" name="MSFPnav6"></a> <script language="JavaScript"><!--
if(MSFPhover) { MSFPnav7n=MSFPpreload("../../camping/_derived/index.html_cmp_mikes-website010_hbtn.gif"); MSFPnav7h=MSFPpreload("../../camping/_derived/index.html_cmp_mikes-website010_hbtn_a.gif"); }
// --></script><a href="../../camping/index.html" language="JavaScript" onmouseover="if(MSFPhover) document['MSFPnav7'].src=MSFPnav7h.src" onmouseout="if(MSFPhover) document['MSFPnav7'].src=MSFPnav7n.src"><img src="../../camping/_derived/index.html_cmp_mikes-website010_hbtn.gif" width="80" height="20" border="0" alt="Camping" align="middle" name="MSFPnav7"></a> <script language="JavaScript"><!--
if(MSFPhover) { MSFPnav8n=MSFPpreload("../../pictures/_derived/index.html_cmp_mikes-website010_hbtn.gif"); MSFPnav8h=MSFPpreload("../../pictures/_derived/index.html_cmp_mikes-website010_hbtn_a.gif"); }
// --></script><a href="../../pictures/index.html" language="JavaScript" onmouseover="if(MSFPhover) document['MSFPnav8'].src=MSFPnav8h.src" onmouseout="if(MSFPhover) document['MSFPnav8'].src=MSFPnav8n.src"><img src="../../pictures/_derived/index.html_cmp_mikes-website010_hbtn.gif" width="80" height="20" border="0" alt="Pictures" align="middle" name="MSFPnav8"></a> <script language="JavaScript"><!--
if(MSFPhover) { MSFPnav9n=MSFPpreload("../../stuff/_derived/index.html_cmp_mikes-website010_hbtn.gif"); MSFPnav9h=MSFPpreload("../../stuff/_derived/index.html_cmp_mikes-website010_hbtn_a.gif"); }
// --></script><a href="../../stuff/index.html" language="JavaScript" onmouseover="if(MSFPhover) document['MSFPnav9'].src=MSFPnav9h.src" onmouseout="if(MSFPhover) document['MSFPnav9'].src=MSFPnav9n.src"><img src="../../stuff/_derived/index.html_cmp_mikes-website010_hbtn.gif" width="80" height="20" border="0" alt="Stuff" align="middle" name="MSFPnav9"></a> <script language="JavaScript"><!--
if(MSFPhover) { MSFPnav10n=MSFPpreload("../../email/_derived/index.html_cmp_mikes-website010_hbtn.gif"); MSFPnav10h=MSFPpreload("../../email/_derived/index.html_cmp_mikes-website010_hbtn_a.gif"); }
// --></script><a href="../../email/index.html" language="JavaScript" onmouseover="if(MSFPhover) document['MSFPnav10'].src=MSFPnav10h.src" onmouseout="if(MSFPhover) document['MSFPnav10'].src=MSFPnav10n.src"><img src="../../email/_derived/index.html_cmp_mikes-website010_hbtn.gif" width="80" height="20" border="0" alt="Email" align="middle" name="MSFPnav10"></a>
        <p>&nbsp;</p>
      <!--mstheme--></font></td>
    </tr>
    <tr>
      <td valign="middle" align="center" width="100" height="200"><!--mstheme--><font face="Arial, Helvetica"><a href="../../stuff/toc.html"><img border="0" src="../../_borders/border_toc-ad.gif" width="48" height="31"></a><!--mstheme--></font></td>
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

<p>&nbsp;</p>
<p align="center"><font face="Arial" size="2"><em><strong>Click anybody below to jump to them.
    </strong></em></font>
<p align="center"><map name="FPMap9">
<area href="#dadmarsha" shape="polygon" coords="171, 112, 186, 112, 187, 136, 194, 144, 213, 132, 226, 137, 228, 153, 226, 166, 233, 174, 236, 204, 229, 229, 234, 256, 215, 256, 201, 206, 182, 190, 175, 166, 157, 169, 146, 164, 155, 145, 169, 140, 166, 112">
<area href="#rob" shape="polygon" coords="232, 157, 247, 144, 244, 118, 257, 110, 271, 118, 269, 144, 287, 153, 287, 168, 276, 175, 268, 195, 269, 165, 259, 162, 245, 171, 244, 186, 243, 199, 240, 200, 234, 160, 240, 148, 250, 145, 244, 121">
<area href="#mel" shape="polygon" coords="294, 161, 296, 145, 303, 132, 321, 134, 320, 152, 318, 164, 324, 171, 321, 195, 307, 206, 277, 197, 277, 179, 291, 168, 295, 155">
<area href="#katie" shape="polygon" coords="147, 149, 142, 135, 116, 138, 117, 163, 127, 176, 114, 181, 108, 198, 123, 204, 144, 200, 154, 191, 153, 169, 142, 161, 147, 144">
<area href="#matt" coords="106, 191, 105, 170, 84, 167, 88, 192, 45, 218, 14, 273, 8, 276, 7, 285, 36, 280, 108, 278, 114, 223, 118, 210, 105, 200, 102, 185" shape="polygon">
<area href="#rick" shape="polygon" coords="112, 281, 123, 203, 159, 197, 159, 170, 176, 170, 178, 192, 194, 200, 217, 275, 228, 306, 214, 306, 206, 271, 116, 276">
<area href="#kevin" shape="polygon" coords="267, 168, 253, 167, 249, 172, 248, 197, 239, 210, 234, 230, 240, 252, 243, 268, 278, 267, 312, 265, 301, 206, 274, 197">
<area href="#mike" shape="polygon" coords="333, 167, 352, 171, 342, 196, 379, 208, 400, 266, 388, 270, 321, 265, 303, 208, 326, 198, 329, 168">
<area href="trees.html" coords="32, 27, 86, 9, 253, 8, 326, 20, 357, 36, 391, 15, 395, 100, 382, 140, 288, 84, 215, 56, 122, 82, 56, 103, 16, 65, 23, 36" shape="polygon"></map><img border="0" src="reception/family.jpg" usemap="#FPMap9"></p>
<p align="center"><font face="Arial"><em>May 1999</em></font></p>

<p><a name="dadmarsha"></a></p>
<div align="center">
  <center><!--mstheme--></font><table border="1" cellpadding="6" cellspacing="0" width="95%" bgcolor="#000080" bordercolordark="#FFFFFF" bordercolorlight="#FFFFFF">
  <tr>
    <td colspan="2"><!--mstheme--><font face="Arial, Helvetica"><font face="Arial"><strong><big>Dick</big><big> </big><big>and
      Marsha</big></strong></font><!--mstheme--></font></td>
  </tr>
  <tr>
    <td><!--mstheme--><font face="Arial, Helvetica">
      <p align="center"><img border="0" src="wedding/dad_marsha.jpg"><!--mstheme--></font></td>
    <td align="center"><!--mstheme--><font face="Arial, Helvetica">They are living in Libertyville, Illinois.&nbsp; Dick (my
      Dad) is taking several computer related training classes and starting
      consulting.&nbsp; Marsha is a real estate agent.<!--mstheme--></font></td>
  </tr>
</table><!--mstheme--><font face="Arial, Helvetica"></center>
</div>

<p><a name="rick"></a></p>
<div align="center"><center>

<!--mstheme--></font><table border="1" cellpadding="6" cellspacing="0" width="95%" bgcolor="#000080" bordercolordark="#FFFFFF" bordercolorlight="#FFFFFF">
  <tr>
    <td colspan="2"><!--mstheme--><font face="Arial, Helvetica"><font face="Arial"><big><strong>Rick</strong></big></font><!--mstheme--></font></td>
  </tr>
  <tr>
    <td><!--mstheme--><font face="Arial, Helvetica"><p align="center"><font face="Arial"><img src="../freegee/easter98/rick.jpg" WIDTH="200" HEIGHT="163"><br>
      Easter 98</font><!--mstheme--></font></td>
    <td align="center"><!--mstheme--><font face="Arial, Helvetica"><font face="Arial">Rick is living in Milwaukee.&nbsp; He graduated
    from UWM with a degree in Architecture, and will be continuing school there as a graduate
    student.&nbsp; He is a lot attendant for the city of Milwaukee, so if you go to Summerfest
    or something, look for him for cheap parking. <% Response.Write("<BR><BR><I>Current age: " & CalcAge("June 19, 1970")) & "</I>"%> </font><!--mstheme--></font></td>
  </tr>
</table><!--mstheme--><font face="Arial, Helvetica">
</center></div>

<p><a name="rob"></a></p>
<div align="center"><center>

<!--mstheme--></font><table border="1" cellpadding="6" cellspacing="0" width="95%" bgcolor="#000080" bordercolordark="#FFFFFF" bordercolorlight="#FFFFFF">
  <tr>
    <td colspan="2"><!--mstheme--><font face="Arial, Helvetica"><font face="Arial"><big><strong>Rob</strong></big></font><!--mstheme--></font></td>
  </tr>
  <tr>
    <td><!--mstheme--><font face="Arial, Helvetica"><p align="center"><font face="Arial"><img src="../freegee/easter98/rob.jpg" WIDTH="200" HEIGHT="214"><br>
      Easter 98</font><!--mstheme--></font></td>
    <td align="center"><!--mstheme--><font face="Arial, Helvetica"><font face="Arial">Rob is living in Racine.&nbsp; He is working at
    Modine and taking a class or two at Gateway in Racine. <% Response.Write("<BR><BR><I>Current age: " & CalcAge("Nov 17, 1971")) & "</I>"%> </font><!--mstheme--></font></td>
  </tr>
</table><!--mstheme--><font face="Arial, Helvetica">
</center></div>

<p><a name="mel"></a></p>
<div align="center"><center>

<!--mstheme--></font><table border="1" cellpadding="6" cellspacing="0" width="95%" bgcolor="#000080" bordercolordark="#FFFFFF" bordercolorlight="#FFFFFF">
  <tr>
    <td colspan="2"><!--mstheme--><font face="Arial, Helvetica"><font face="Arial"><strong><big>Mel</big></strong></font><!--mstheme--></font></td>
  </tr>
  <tr>
    <td><!--mstheme--><font face="Arial, Helvetica">
      <p align="center"><i><font face="Arial" color="#00FF00">Picture not yet
      available.</font></i><!--mstheme--></font></td>
    <td align="center"><!--mstheme--><font face="Arial, Helvetica"><font face="Arial">Mel (or Ryan) is a graduate student at
      Colorado State University studying Manufacturing Technology for his
      Masters.</font><!--mstheme--></font></td>
  </tr>
</table><!--mstheme--><font face="Arial, Helvetica">
</center></div>

<p><a name="katie"></a></p>
<div align="center"><center>

<!--mstheme--></font><table border="1" cellpadding="6" cellspacing="0" width="95%" bgcolor="#000080" bordercolordark="#FFFFFF" bordercolorlight="#FFFFFF">
  <tr>
    <td colspan="2"><!--mstheme--><font face="Arial, Helvetica"><font face="Arial"><strong><big>Katie</big></strong></font><!--mstheme--></font></td>
  </tr>
  <tr>
    <td><!--mstheme--><font face="Arial, Helvetica">
      <p align="center"><i><font face="Arial" color="#00FF00">Picture not yet
      available.</font></i><!--mstheme--></font></td>
    <td align="center"><!--mstheme--><font face="Arial, Helvetica"><font face="Arial">Katie is a Junior at UW-Stevens Point
      majoring in International Resource Management and Wildlife Ecology.</font><!--mstheme--></font></td>
  </tr>
</table><!--mstheme--><font face="Arial, Helvetica">
</center></div>

<p><a name="mike"></a></p>
<div align="center"><center>

<!--mstheme--></font><table border="1" cellpadding="6" cellspacing="0" width="95%" bgcolor="#000080" bordercolordark="#FFFFFF" bordercolorlight="#FFFFFF">
  <tr>
    <td colspan="2"><!--mstheme--><font face="Arial, Helvetica"><font face="Arial"><big><strong>Mike</strong></big></font><!--mstheme--></font></td>
  </tr>
  <tr>
    <td><!--mstheme--><font face="Arial, Helvetica"><p align="center"><font face="Arial"><img src="../freegee/easter98/mike.jpg" WIDTH="200" HEIGHT="189"><br>
      Easter 98</font><!--mstheme--></font></td>
    <td align="center"><!--mstheme--><font face="Arial, Helvetica"><font face="Arial">For my information, look at my own <a href="../../aboutme/index.html">page</a>, it will
    have the most recent info about me. <% Response.Write("<BR><BR><I>Current age: " & CalcAge("March 15, 1977")) & "</I>"%> </font><!--mstheme--></font></td>
  </tr>
</table><!--mstheme--><font face="Arial, Helvetica">
</center></div>

<p><a name="matt"></a></p>
<div align="center"><center>

<!--mstheme--></font><table border="1" cellpadding="6" cellspacing="0" width="95%" bgcolor="#000080" bordercolordark="#FFFFFF" bordercolorlight="#FFFFFF">
  <tr>
    <td colspan="2"><!--mstheme--><font face="Arial, Helvetica"><font face="Arial"><big><strong>Matt</strong></big></font><!--mstheme--></font></td>
  </tr>
  <tr>
    <td><!--mstheme--><font face="Arial, Helvetica"><p align="center"><font face="Arial"><img src="../freegee/easter98/matt.jpg" WIDTH="200" HEIGHT="196"><br>
      Easter 98</font><!--mstheme--></font></td>
    <td align="center"><!--mstheme--><font face="Arial, Helvetica"><font face="Arial">Matt is going to school in LaCrosse.&nbsp; He hasn't
    decided on a major yet, though he is leaning towards biochem&nbsp; For this
      summer he is living at home and working at Chi-Chis and Kohls food store. <% Response.Write("<BR><BR><I>Current age: " & CalcAge("Dec 13, 1978")) & "</I>"%> </font><!--mstheme--></font></td>
  </tr>
</table><!--mstheme--><font face="Arial, Helvetica">
</center></div>

<p><a name="kevin"></a></p>

<div align="center">
  <center><!--mstheme--></font><table border="1" cellpadding="6" cellspacing="0" width="95%" bgcolor="#000080" bordercolordark="#FFFFFF" bordercolorlight="#FFFFFF">
  <tr>
    <td colspan="2"><!--mstheme--><font face="Arial, Helvetica"><font face="Arial"><strong><big>Kevin</big></strong></font><!--mstheme--></font></td>
  </tr>
  <tr>
    <td><!--mstheme--><font face="Arial, Helvetica">
      <p align="center"><img border="0" src="Kev_99.jpg"><br>
      Fall 99<!--mstheme--></font></td>
    <td align="center"><!--mstheme--><font face="Arial, Helvetica"><font face="Arial">Kevin is a freshman at Libertyville
      High School.</font><!--mstheme--></font></td>
  </tr>
</table><!--mstheme--><font face="Arial, Helvetica"></center>
</div>

<p>&nbsp;</p>

<p>&nbsp;</p>
<!--mstheme--></font><!--msnavigation--></td></tr><!--msnavigation--></table><!--msnavigation--><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td><!--mstheme--><font face="Arial, Helvetica">

<div align="center">
  <center>
  <!--mstheme--></font><table border="0" cellpadding="0" cellspacing="0" width="100%" height="30">
    <tr>
      <td width="100%" valign="middle" align="center" bgcolor="#C00000"><!--mstheme--><font face="Arial, Helvetica">
        <div align="center">
          <!--mstheme--></font><table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
              <td valign="middle" align="center" width="100"><!--mstheme--><font face="Arial, Helvetica"><a href="http://www.microsoft.com/windows2000" target="_blank"><img border="0" src="../../_borders/w2ksplash.gif" width="31" height="20" alt="Powered by Windows 2000"></a><!--mstheme--></font></td>
              <td valign="middle" align="center"><!--mstheme--><font face="Arial, Helvetica"><font size="1"> <a href="http://mike.giesler.org">mike.giesler.org</a>,
                a <a href="http://giesler.org">giesler.org</a> site</font><!--mstheme--></font></td>
              <td valign="middle" align="right" width="100"><!--mstheme--><font face="Arial, Helvetica"><img border="0" src="../../_borders/border_copyright.gif" width="80" height="27"><!--mstheme--></font></td>
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
