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

<body topmargin="0" leftmargin="0" background="../../_themes/mikes-website/blackbg.gif" bgcolor="#000000" text="#FFFFFF" link="#FFFF00" vlink="#FFFF99" alink="#FFCC99"><!--msnavigation--><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td><!--mstheme--><font face="Arial, Helvetica">

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
<div align="center"><center>

<!--mstheme--></font><table border="1" cellpadding="5" cellspacing="0" width="500" background="Marble.jpg" bordercolordark="#FFFFFF" bordercolorlight="#FFFFFF">
  <tr>
    <td width="100%"><!--mstheme--><font face="Arial, Helvetica"><font face="Arial"><big><strong>Current Family 'Info'</strong></big><br>
    I will attempt to keep this page as up-to-date as possible, though I can't guarantee
    anything.&nbsp; The last time this page was updated was October 7, 1999.&nbsp;
      Click on anyone to jump to them.</font>
    <!--mstheme--></font></td>
  </tr>
</table><!--mstheme--><font face="Arial, Helvetica">
</center></div>

<p align="center"><map name="FPMap0">
          <area href="#rick" shape="polygon" coords="429, 247, 418, 196, 422, 178, 431, 165, 429, 73, 399, 55, 402, 16, 373, 12, 364, 29, 370, 58, 371, 63, 361, 76, 347, 148, 345, 170, 344, 246, 376, 247, 383, 229, 389, 246, 422, 247">
          <area href="#tmk" shape="polygon" coords="357, 78, 351, 70, 356, 48, 352, 34, 334, 30, 323, 42, 323, 61, 326, 68, 340, 76, 346, 129">
          <area href="#matt" shape="polygon" coords="332, 244, 338, 182, 334, 165, 344, 133, 339, 79, 314, 69, 307, 59, 313, 43, 300, 23, 287, 27, 280, 43, 286, 70, 276, 80, 269, 83, 262, 96, 271, 112, 269, 116, 265, 152, 285, 159, 297, 246, 303, 213, 307, 245, 325, 247">
          <area href="#ginni" shape="polygon" coords="273, 80, 264, 71, 270, 50, 260, 39, 248, 47, 250, 53, 246, 92, 257, 99, 259, 85, 266, 81">
          <area href="#ginni" shape="polygon" coords="179, 68, 181, 32, 203, 28, 213, 37, 210, 47, 204, 67, 216, 76, 213, 87, 185, 98, 173, 84, 181, 73">
          <area href="#mike" shape="polygon" coords="195, 230, 197, 176, 185, 159, 184, 101, 215, 90, 220, 78, 213, 69, 221, 48, 234, 42, 248, 52, 241, 96, 267, 105, 264, 108, 239, 104, 228, 118, 226, 129, 230, 141, 231, 152, 207, 168, 207, 187, 203, 230">
          <area href="#tmk" shape="polygon" coords="289, 236, 281, 162, 260, 155, 258, 149, 265, 125, 261, 112, 250, 105, 235, 107, 230, 117, 229, 131, 237, 139, 235, 156, 213, 169, 209, 231">
          <area href="#paula" shape="polygon" coords="140, 244, 179, 227, 176, 194, 180, 181, 181, 100, 168, 86, 177, 72, 175, 50, 165, 42, 155, 45, 146, 59, 136, 54, 136, 41, 149, 30, 151, 15">
          <area href="#chuck" shape="polygon" coords="90, 157, 122, 168, 125, 89, 117, 80, 120, 55, 110, 32, 90, 31, 76, 58, 74, 71, 56, 77, 49, 101, 47, 117, 35, 153, 47, 156, 53, 121, 74, 113, 94, 126, 92, 150, 94, 150">
          <area href="#rob" shape="polygon" coords="9, 239, 15, 168, 43, 161, 51, 121, 72, 114, 91, 128, 81, 163, 116, 175, 120, 196, 137, 209, 134, 229, 134, 242, 119, 248, 3, 245"></map><img polygon=" (332,244) (338,182) (334,165) (344,133) (339,79) (314,69) (307,59) (313,43) (300,23) (287,27) (280,43) (286,70) (276,80) (269,83) (262,96) (271,112) (269,116) (265,152) (285,159) (297,246) (303,213) (307,245) (325,247) #matt" polygon=" (273,80) (264,71) (270,50) (260,39) (248,47) (250,53) (246,92) (257,99) (259,85) (266,81) #ginni" polygon=" (179,68) (181,32) (203,28) (213,37) (210,47) (204,67) (216,76) (213,87) (185,98) (173,84) (181,73) #ginni" polygon=" (195,230) (197,176) (185,159) (184,101) (215,90) (220,78) (213,69) (221,48) (234,42) (248,52) (241,96) (267,105) (264,108) (239,104) (228,118) (226,129) (230,141) (231,152) (207,168) (207,187) (203,230) #mike" polygon=" (289,236) (281,162) (260,155) (258,149) (265,125) (261,112) (250,105) (235,107) (230,117) (229,131) (237,139) (235,156) (213,169) (209,231) #tmk" polygon=" (140,244) (179,227) (176,194) (180,181) (181,100) (168,86) (177,72) (175,50) (165,42) (155,45) (146,59) (136,54) (136,41) (149,30) (151,15) (136,-2) (116,4) (113,16) (113,27) (118,31) (118,37) (124,56) (120,78) (129,88) (125,173) (125,197) (142,215) (141,235) A#paula" polygon=" (90,157) (122,168) (125,89) (117,80) (120,55) (110,32) (90,31) (76,58) (74,71) (56,77) (49,101) (47,117) (35,153) (47,156) (53,121) (74,113) (94,126) (92,150) (94,150) #chuck" polygon=" (9,239) (15,168) (43,161) (51,121) (72,114) (91,128) (81,163) (116,175) (120,196) (137,209) (134,229) (134,242) (119,248) (3,245) #rob" src="cheve97/freegees.jpg" border="0" usemap="#FPMap0" WIDTH="433" HEIGHT="250"><br>
<font size="2"><i>Christmas Eve, 1997</i></font></p>

<p>&nbsp;</p>

<!--msthemeseparator--><p align="center"><img src="../../_themes/mikes-website/horizontal%20rule.gif" width="600" height="10"></p>

<p><a name="tmk"></a></p>
<div align="center"><center>

<!--mstheme--></font><table border="0" cellpadding="5" cellspacing="0" width="500">
  <tr>
    <td colspan="2"><!--mstheme--><font face="Arial, Helvetica"><font face="Arial"><big><strong>Tony &amp; Mary Kay</strong></big></font><!--mstheme--></font></td>
  </tr>
  <tr>
    <td><!--mstheme--><font face="Arial, Helvetica"><p align="center"><img src="thanks97/parentslo.jpg" WIDTH="150" HEIGHT="126"><font face="Arial"><br>
      <font size="2"><i>
      Thanksgiving 97</i></font></font><!--mstheme--></font></td>
    <td align="center"><!--mstheme--><font face="Arial, Helvetica">
      <div align="center">
        <!--mstheme--></font><table border="1" cellpadding="5" cellspacing="0" background="Marble.jpg" bordercolordark="#FFFFFF" bordercolorlight="#FFFFFF">
          <tr>
            <td><!--mstheme--><font face="Arial, Helvetica"><font face="Arial">Tony and Mary Kay are living in Racine (of course)
    at the same old house.&nbsp; Tony is working at Twin Disc.&nbsp; Mary Kay works at Act 2,
    a clothing resale store.</font><!--mstheme--></font></td>
          </tr>
        </table><!--mstheme--><font face="Arial, Helvetica">
      </div>
      &nbsp;<!--mstheme--></font></td>
  </tr>
</table><!--mstheme--><font face="Arial, Helvetica">
</center></div>

<p>&nbsp;</p>
<!--msthemeseparator--><p align="center"><img src="../../_themes/mikes-website/horizontal%20rule.gif" width="600" height="10"></p>

<p><a name="rick"></a></p>
<div align="center"><center>

<!--mstheme--></font><table border="0" cellpadding="5" cellspacing="0" width="500">
  <tr>
    <td colspan="2"><!--mstheme--><font face="Arial, Helvetica"><font face="Arial"><big><strong>Rick</strong></big></font><!--mstheme--></font></td>
  </tr>
  <tr>
    <td><!--mstheme--><font face="Arial, Helvetica"><p align="center"><font face="Arial"><img src="easter98/rick.jpg" WIDTH="200" HEIGHT="163"><br>
      <font size="2"><i>
      Easter 98</i></font></font><!--mstheme--></font></td>
    <td align="center"><!--mstheme--><font face="Arial, Helvetica">
      <div align="center">
        <!--mstheme--></font><table border="1" cellpadding="5" cellspacing="0" background="Marble.jpg" bordercolordark="#FFFFFF" bordercolorlight="#FFFFFF">
          <tr>
            <td><!--mstheme--><font face="Arial, Helvetica"><font face="Arial">Rick is living in Milwaukee.&nbsp; He graduated from
      UWM with a degree in Architecture, and will be continuing school there as
      a graduate student.&nbsp; He is a lot attendant for the city of Milwaukee,
      so if you go to Summerfest or something, look for him for cheap parking. <% Response.Write("<BR><BR><I>Current age: " & CalcAge("June 19, 1970")) & "</I>"%> </font><!--mstheme--></font></td>
          </tr>
        </table><!--mstheme--><font face="Arial, Helvetica">
      </div>
    <!--mstheme--></font></td>
  </tr>
</table><!--mstheme--><font face="Arial, Helvetica">
</center></div>

<p>&nbsp;</p>
<!--msthemeseparator--><p align="center"><img src="../../_themes/mikes-website/horizontal%20rule.gif" width="600" height="10"></p>

<p><a name="rob"></a></p>
<div align="center"><center>

<!--mstheme--></font><table border="0" cellpadding="5" cellspacing="0" width="500">
  <tr>
    <td colspan="2"><!--mstheme--><font face="Arial, Helvetica"><font face="Arial"><big><strong>Rob</strong></big></font><!--mstheme--></font></td>
  </tr>
  <tr>
    <td><!--mstheme--><font face="Arial, Helvetica"><p align="center"><font face="Arial"><img src="easter98/rob.jpg" WIDTH="200" HEIGHT="214"><br>
      <i><font size="2">
      Easter 98</font></i></font><!--mstheme--></font></td>
    <td align="center"><!--mstheme--><font face="Arial, Helvetica">
      <div align="center">
        <!--mstheme--></font><table border="1" cellpadding="5" cellspacing="0" background="Marble.jpg" bordercolordark="#FFFFFF" bordercolorlight="#FFFFFF">
          <tr>
            <td><!--mstheme--><font face="Arial, Helvetica"><font face="Arial">Rob is living in Racine.&nbsp; He is working at
    Modine and taking a class or two at Gateway in Racine. <% Response.Write("<BR><BR><I>Current age: " & CalcAge("Nov 17, 1971")) & "</I>"%> </font><!--mstheme--></font></td>
          </tr>
        </table><!--mstheme--><font face="Arial, Helvetica">
      </div>
    <!--mstheme--></font></td>
  </tr>
</table><!--mstheme--><font face="Arial, Helvetica">
</center></div>

<p>&nbsp;</p>
<!--msthemeseparator--><p align="center"><img src="../../_themes/mikes-website/horizontal%20rule.gif" width="600" height="10"></p>

<p><a name="ginni"></a></p>
<div align="center"><center>

<!--mstheme--></font><table border="0" cellpadding="5" cellspacing="0" width="500">
  <tr>
    <td colspan="2"><!--mstheme--><font face="Arial, Helvetica"><font face="Arial"><big><strong>Ginni</strong></big></font><!--mstheme--></font></td>
  </tr>
  <tr>
    <td><!--mstheme--><font face="Arial, Helvetica"><p align="center"><font face="Arial"><img src="easter98/ginni.jpg" WIDTH="200" HEIGHT="195"><br>
      <font size="2"><i>
      Easter 98</i></font></font><!--mstheme--></font></td>
    <td align="center"><!--mstheme--><font face="Arial, Helvetica">&nbsp;
      <div align="center">
        <!--mstheme--></font><table border="1" cellpadding="5" cellspacing="0" background="Marble.jpg" bordercolordark="#FFFFFF" bordercolorlight="#FFFFFF">
          <tr>
            <td><!--mstheme--><font face="Arial, Helvetica"><font face="Arial">Ginni lives a few blocks away from
      Tony and Mary Kay in Racine. &nbsp; She has two kids - Zach and Elleah -
      Elleah was just born (October 6). She is married
    to Matt, pictured below.&nbsp; They got married in Las Vegas last year, and
      early in June (this year) got married in a church ceremony.  <% Response.Write("<BR><BR><I>Current age: " & CalcAge("April 2, 1972")) & "</I>"%>     </font><!--mstheme--></font></td>
          </tr>
        </table><!--mstheme--><font face="Arial, Helvetica">
      </div>
      
      <p><font face="Arial"><img src="easter98/mattz.jpg" alt="mattz.jpg (8110 bytes)" WIDTH="200" HEIGHT="185">
 </font><!--mstheme--></font></td>
  </tr>
</table><!--mstheme--><font face="Arial, Helvetica">
</center></div>

<p>&nbsp;</p>
<!--msthemeseparator--><p align="center"><img src="../../_themes/mikes-website/horizontal%20rule.gif" width="600" height="10"></p>

<p><a name="paula"></a></p>
<div align="center"><center>

<!--mstheme--></font><table border="0" cellpadding="5" cellspacing="0" width="500">
  <tr>
    <td colspan="2"><!--mstheme--><font face="Arial, Helvetica"><font face="Arial"><big><strong>Paula</strong></big></font><!--mstheme--></font></td>
  </tr>
  <tr>
    <td><!--mstheme--><font face="Arial, Helvetica"><p align="center"><font face="Arial"><img src="cheve97/paulatom.jpg" width="200" height="206"><br>
      <font size="2"><i>
      Christmas 97</i></font></font><!--mstheme--></font></td>
    <td align="center"><!--mstheme--><font face="Arial, Helvetica">
      <div align="center">
        <!--mstheme--></font><table border="1" cellpadding="5" cellspacing="0" background="Marble.jpg" bordercolordark="#FFFFFF" bordercolorlight="#FFFFFF">
          <tr>
            <td><!--mstheme--><font face="Arial, Helvetica"><font face="Arial">Paula and Tom live in North Carolina.&nbsp; Paula
      graduated from San Diego State with a degree in
    Accounting.&nbsp; Tom was serving in the Navy. </font><!--mstheme--></font></td>
          </tr>
        </table><!--mstheme--><font face="Arial, Helvetica">
      </div>
    <!--mstheme--></font></td>
  </tr>
</table><!--mstheme--><font face="Arial, Helvetica">
</center></div>

<p>&nbsp;</p>
<!--msthemeseparator--><p align="center"><img src="../../_themes/mikes-website/horizontal%20rule.gif" width="600" height="10"></p>

<p><a name="chuck"></a></p>
<div align="center">

<!--mstheme--></font><table border="0" cellpadding="6" cellspacing="0" width="500">
  <tr>
    <td colspan="2"><!--mstheme--><font face="Arial, Helvetica"><font face="Arial"><big><strong>Chuck</strong></big></font><!--mstheme--></font></td>
  </tr>
  <center>

  <tr>
    <td><!--mstheme--><font face="Arial, Helvetica"><p align="center"><font face="Arial"><img src="thanks97/chucklo.jpg" WIDTH="150" HEIGHT="159"><br>
      <font size="2"><i>
      Thanksgiving 97</i></font></font><!--mstheme--></font></td>
    <td align="center"><!--mstheme--><font face="Arial, Helvetica">
      <div align="center">
        <!--mstheme--></font><table border="1" cellpadding="5" cellspacing="0" background="Marble.jpg" bordercolordark="#FFFFFF" bordercolorlight="#FFFFFF">
          <tr>
            <td><!--mstheme--><font face="Arial, Helvetica"><font face="Arial">Chuck is going to the University of
      Minnesota-Minneapolis.&nbsp; She is majoring in Art.&nbsp; She is the assistant
    director of the art gallery on the UM campus. <% Response.Write("<BR><BR><I>Current age: " & CalcAge("Nov 30, 1975")) & "</I>"%> </font><!--mstheme--></font></td>
          </tr>
        </table><!--mstheme--><font face="Arial, Helvetica">
      </div>
    <!--mstheme--></font></td>
  </tr>
</table><!--mstheme--><font face="Arial, Helvetica">
</center></div>

<p>&nbsp;</p>
<!--msthemeseparator--><p align="center"><img src="../../_themes/mikes-website/horizontal%20rule.gif" width="600" height="10"></p>

<p><a name="mike"></a></p>
<div align="center">

<!--mstheme--></font><table border="0" cellpadding="5" cellspacing="0" width="500">
  <tr>
    <td colspan="2"><!--mstheme--><font face="Arial, Helvetica"><font face="Arial"><big><strong>Mike</strong></big></font><!--mstheme--></font></td>
  </tr>
  <center>

  <tr>
    <td><!--mstheme--><font face="Arial, Helvetica"><p align="center"><font face="Arial"><img src="easter98/mike.jpg" WIDTH="200" HEIGHT="189"><br>
      <font size="2"><i>
      Easter 98</i></font></font><!--mstheme--></font></td>
    <td align="center"><!--mstheme--><font face="Arial, Helvetica">
      <div align="center">
        <!--mstheme--></font><table border="1" cellpadding="5" cellspacing="0" background="Marble.jpg" bordercolordark="#FFFFFF" bordercolorlight="#FFFFFF">
          <tr>
            <td><!--mstheme--><font face="Arial, Helvetica"><font face="Arial">For my information, look at my own <a href="../../aboutme/index.html">page</a>, it will
    have the most recent info about me. <% Response.Write("<BR><BR><I>Current age: " & CalcAge("March 15, 1977")) & "</I>"%> </font><!--mstheme--></font></td>
          </tr>
        </table><!--mstheme--><font face="Arial, Helvetica">
      </div>
    <!--mstheme--></font></td>
  </tr>
</table><!--mstheme--><font face="Arial, Helvetica">
</center></div>

<p>&nbsp;</p>
<!--msthemeseparator--><p align="center"><img src="../../_themes/mikes-website/horizontal%20rule.gif" width="600" height="10"></p>

<p><a name="matt"></a></p>
<div align="center">

<!--mstheme--></font><table border="0" cellpadding="5" cellspacing="0" width="500">
  <tr>
    <td colspan="2"><!--mstheme--><font face="Arial, Helvetica">
      <p align="left"><font face="Arial"><big><strong>Matt</strong></big></font></p>
    <!--mstheme--></font></td>
  </tr>
  <center>

  <tr>
    <td><!--mstheme--><font face="Arial, Helvetica"><p align="center"><font face="Arial"><img src="easter98/matt.jpg" WIDTH="200" HEIGHT="196"><br>
      <font size="2"><i>
      Easter 98</i></font></font><!--mstheme--></font></td>
    <td align="center"><!--mstheme--><font face="Arial, Helvetica">
      <div align="center">
        <!--mstheme--></font><table border="1" cellpadding="5" cellspacing="0" background="Marble.jpg" bordercolordark="#FFFFFF" bordercolorlight="#FFFFFF">
          <tr>
            <td><!--mstheme--><font face="Arial, Helvetica"><font face="Arial">Matt is going to school in LaCrosse.&nbsp; He hasn't
    decided on a major yet, though he is leaning towards biochem&nbsp; For this
      summer he is living at home and working at Chi-Chis and Kohls food store. <% Response.Write("<BR><BR><I>Current age: " & CalcAge("Dec 13, 1978")) & "</I>"%> </font><!--mstheme--></font></td>
          </tr>
        </table><!--mstheme--><font face="Arial, Helvetica">
      </div>
    <!--mstheme--></font></td>
  </tr>
</table><!--mstheme--><font face="Arial, Helvetica">
</center></div>

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
