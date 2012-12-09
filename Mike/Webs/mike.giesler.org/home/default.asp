<html>
	<head>
						<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<title>Frequent Links</title>
		<style> <!-- a {color:#FFFF00;  TEXT-DECORATION: none;} 
						a:visited {color:#FFFF00;} 
						a:hover {color:red; TEXT-DECORATION: underline;}
						input        { font-size: 8pt; font-family: Verdana }
--></style>
		<script language="JavaScript"><!--

var garrLayoutParts = new Array();

//--Object declaration
function objPart(strPartName, strState, strOrder)
{
	this.name	= strPartName;
	this.state	= strState;
}

garrLayoutParts[0] = new objPart("Google", "expanded")
garrLayoutParts[1] = new objPart("AltaVista", "collapsed")
garrLayoutParts[2] = new objPart("Yahoo", "collapsed")
garrLayoutParts[3] = new objPart("Lycos", "collapsed")
garrLayoutParts[4] = new objPart("Excite", "collapsed")
garrLayoutParts[5] = new objPart("YellowPages", "collapsed")
garrLayoutParts[6] = new objPart("Dictionary", "collapsed")
garrLayoutParts[8] = new objPart("AskJeeves", "collapsed")



function ShowHide(id) {
	var oContent = document.all.item(id + "Content");
	var oImage   = document.all.item(id + "Image");
	var oDocFocus = document.all.item(id + "Focus");
	
	if (!oContent || !oImage) return;
	
	// find array element
	for (var i = 0; i < garrLayoutParts.length; i++) {
		if (id == garrLayoutParts[i].name)
			break;
	}
	
	if (garrLayoutParts[i].state == "expanded") {
		oContent.style.display = "none";
		oImage.src = "expand.gif"
		garrLayoutParts[i].state = "collapsed"
	} else {
		oContent.style.display = "";
		oImage.src = "collapse.gif"
		garrLayoutParts[i].state = "expanded"
		if (oDocFocus) oDocFocus.focus();
		
	}

}

// --></script>
			<meta name="Microsoft Border" content="tlb">
</head>
	<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" background="_themes/mikes-website/blackbg.gif" bgcolor="#000000" text="#FFFFFF" link="#FFFF00" vlink="#FFFF99" alink="#FFCC99" onLoad="javascript:frmGoogle.q.focus()"><!--msnavigation--><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td>

<table border="0" width="100%" cellpadding="0" cellspacing="0">
  <tr>
    <td width="100" bgcolor="#000000"><a href="../Default.asp"><img border="0" src="../_borders/mikespage.gif"></a></td>
    <td bgcolor="#000000" width="20"><img border="0" src="../_borders/black20x30filler.gif"></td>
    <td bgcolor="#C00000" background="../_borders/bl-red_fade_top.jpg" width="400" height="30"><font size="6"><strong>Frequent Links</strong></font></td>
    <td bgcolor="#C00000" align="right" valign="middle"><a href="http://giesler.org"><img border="0" src="../_borders/giesler.png" alt="Go to giesler.org" width="22" height="30"></a>&nbsp;
    </td>
  </tr>
</table>

</td></tr><!--msnavigation--></table><!--msnavigation--><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td valign="top" width="1%">

<div align="center">
  <center>
  <table border="0" cellpadding="0" cellspacing="0" width="100" bgcolor="#C00000" height="100%">
    <tr>
      <td valign="top" align="center" bgcolor="#000000" width="100" height="20"><img border="0" src="../_borders/black100x20filler.gif" width="100" height="20"></td>
    </tr>
    <tr>
      <td background="../_borders/bl-red_fade_side.jpg" valign="top" align="center" bgcolor="#000000" width="100" height="400">
        <nobr>[&nbsp;<a href="../">Home</a>&nbsp;]</nobr> <nobr>[&nbsp;<a href="../stuff/index.html">Up</a>&nbsp;]</nobr> <nobr>[&nbsp;<a href="../aboutme/index.html">About&nbsp;Me</a>&nbsp;]</nobr> <nobr>[&nbsp;<a href="../family/index.html">Family</a>&nbsp;]</nobr> <nobr>[&nbsp;<a href="../friends/index.html">Friends</a>&nbsp;]</nobr> <nobr>[&nbsp;<a href="../sara/index.html">Sara</a>&nbsp;]</nobr> <nobr>[&nbsp;<a href="../camping/index.html">Camping</a>&nbsp;]</nobr> <nobr>[&nbsp;<a href="../pictures/index.html">Pictures</a>&nbsp;]</nobr> <nobr>[&nbsp;<a href="../email/index.html">Email</a>&nbsp;]</nobr>
        <p>&nbsp;</p>
      </td>
    </tr>
    <tr>
      <td valign="middle" align="center" width="100" height="200"><a href="../stuff/toc.html"><img border="0" src="../_borders/border_toc-ad.gif" width="48" height="31"></a></td>
    </tr>
    <tr>
      <td width="100" valign="bottom" align="center">
        <p>&nbsp;</p>
      </td>
    </tr>
  </table>
  </center>
</div>

</td><td valign="top" width="24"></td><!--msnavigation--><td valign="top"> 
		 
		
					<p align="center">
						&nbsp;
					</p>
					<div align="center">
						<center>
							</font>
							<table border="0" cellpadding="5" cellspacing="0">
								<tr>
									<td rowSpan="5" valign="top">
										</font>
										<table border="0" cellpadding="0" cellspacing="0">
											<tr>
												<td width="200" align="center" valign="top">
													</font>
													<table cellpadding="0" cellspacing="3" border="0">
														<tr>
															<td>
																</font>
																<table width="200" cellpadding="0" cellspacing="0" bgColor="#000000">
																	<tr>
																		<td>
																			</font>
																			<table cellpadding="0" cellspacing="0">
																				<tr>
																					<td bgcolor="black" width="100%" align="center" height="20" background="wash2020.gif">
																						<font face="Verdana" size="2" color="#FFFFFF">Google</font> </font>
																					</td>
																					<td align="right" height="20" background="wash2020.gif">
																						<img src="collapse.gif" id="GoogleImage" onclick="ShowHide('Google')" WIDTH="9" HEIGHT="9"><img src="trans.gif" width="4">
																						</font>
																					</td>
																				</tr>
																			</table>
																			<span id="GoogleContent">
																				<form method="GET" action="http://www.google.com/custom" name="frmGoogle" id="frmGoogle">
																					<input type="hidden" name="cof" VALUE="GIMP:#FFFF40;T:#FFFFFF;BIMG:http://mike.giesler.org/_themes/mikes-website/blackbg.gif;ALC:#FFCC99;GFNT:#FFFFC0;LC:#FFFF00;BGC:#000000;AH:center;VLC:#FFFF99;GL:2;GALT:#C0FFFF;AWFID:48f9c49ef451ce28;">
																					</font>
																					<table cellpadding="0" cellspacing="0">
																						<tr>
																							<td width="100%" align="center" height="20" bgcolor="#000000">
																								<input TYPE="text" name="q" size="31" maxlength="255" value style="font-family: Verdana; font-size: 8pt" id="GoogleFocus">
																								<input type="submit" name="sa" VALUE="Google Search"> </font>
																							</td>
																						</tr>
																					</table>
																				</form>
																			</span></font>
																		</td>
																	</tr>
																</table>
																</font>
															</td>
														</tr>
														<tr>
															<td>
																</font>
																<table width="200" cellpadding="0" cellspacing="0" bgColor="#000000">
																	<tr>
																		<td>
																			</font>
																			<table cellpadding="0" cellspacing="0">
																				<tr>
																					<td bgcolor="black" width="100%" align="center" height="20" background="wash2020.gif">
																						<a href="http://www.av.com"><img src="av_logo.gif" border="0"></a> </font>
																					</td>
																					<td align="right" height="20" background="wash2020.gif">
																						<img src="expand.gif" id="AltaVistaImage" onclick="ShowHide('AltaVista')" WIDTH="9" HEIGHT="9"><img src="trans.gif" width="4">
																						</font>
																					</td>
																				</tr>
																			</table>
																			<span id="AltaVistaContent" style="display: none">
																				<form action="http://altavista.digital.com/cgi-bin/query" method="GET" name="frmAltaVista" id="frmAltaVista">
																					<input type="hidden" name="pg" value="q"> </font>
																					<table cellpadding="0" cellspacing="0" width="100%" align="center">
																						<tr>
																							<td width="100%" align="center" height="20" bgcolor="#000000">
																								<input type="text" size="15" maxlength="200" name="q" style="font-family: Verdana; font-size: 8pt" id="AltaVistaFocus">
																								</font>
																							</td>
																						</tr>
																					</table>
																				</form>
																			</span></font>
																		</td>
																	</tr>
																</table>
																</font>
															</td>
														</tr>
														<tr>
															<td>
																</font>
																<table width="200" cellpadding="0" cellspacing="0" bgColor="#000000">
																	<tr>
																		<td>
																			</font>
																			<table cellpadding="0" cellspacing="0">
																				<tr>
																					<td bgcolor="black" width="100%" align="center" height="20" background="wash2020.gif">
																						<a href="http://www.yahoo.com"><img src="iyahoo.gif" align="top" border="0" width="102" height="20"></a>
																						</font>
																					</td>
																					<td align="right" height="20" background="wash2020.gif">
																						<img src="expand.gif" id="YahooImage" onclick="ShowHide('Yahoo')" WIDTH="9" HEIGHT="9"><img src="trans.gif" width="4">
																						</font>
																					</td>
																				</tr>
																			</table>
																			<span id="YahooContent" style="display: none">
																				<form action="http://search.yahoo.com/bin/search" method="GET" name="frmYahoo" id="frmYahoo">
																					</font>
																					<table cellpadding="0" cellspacing="0" width="100%" align="center">
																						<tr>
																							<td width="100%" align="center" height="20" bgcolor="#000000">
																								<input type="text" size="15" name="p" style="font-family: Verdana; font-size: 8pt" id="YahooFocus">
																								</font>
																							</td>
																						</tr>
																					</table>
																				</form>
																			</span></font>
																		</td>
																	</tr>
																</table>
																</font>
															</td>
														</tr>
														<tr>
															<td>
																</font>
																<table width="200" cellpadding="0" cellspacing="0" bgColor="#000000">
																	<tr>
																		<td>
																			</font>
																			<table cellpadding="0" cellspacing="0">
																				<tr>
																					<td bgcolor="black" width="100%" align="center" height="20" background="wash2020.gif">
																						<a href="http://www.lycos.com"><img src="lycos.gif" border="0" width="64" height="20"></a>
																						</font>
																					</td>
																					<td align="right" height="20" background="wash2020.gif">
																						<img src="expand.gif" id="LycosImage" onclick="ShowHide('Lycos')" WIDTH="9" HEIGHT="9"><img src="trans.gif" width="4">
																						</font>
																					</td>
																				</tr>
																			</table>
																			<span id="LycosContent" style="display: none">
																				<form action="http://www.lycos.com/cgi-bin/pursuit" method="GET" name="frmLycos" id="frmLycos">
																					</font>
																					<table cellpadding="0" cellspacing="0" width="100%" align="center">
																						<tr>
																							<td width="100%" align="center" height="20" bgcolor="#000000">
																								<input type="text" size="15" name="query" style="font-family: Verdana; font-size: 8pt" id="LycosFocus">
																								</font>
																							</td>
																						</tr>
																					</table>
																				</form>
																			</span></font>
																		</td>
																	</tr>
																</table>
																</font>
															</td>
														</tr>
														<tr>
															<td>
																</font>
																<table width="200" cellpadding="0" cellspacing="0" bgColor="#000000">
																	<tr>
																		<td>
																			</font>
																			<table cellpadding="0" cellspacing="0">
																				<tr>
																					<td bgcolor="black" width="100%" align="center" height="20" background="wash2020.gif">
																						<a href="http://www.excite.com"><img src="excite.gif" border="0" width="51" height="20"></a>
																						</font>
																					</td>
																					<td align="right" height="20" background="wash2020.gif">
																						<img src="expand.gif" id="ExciteImage" onclick="ShowHide('Excite')" WIDTH="9" HEIGHT="9"><img src="trans.gif" width="4">
																						</font>
																					</td>
																				</tr>
																			</table>
																			<span id="ExciteContent" style="display: none">
																				<form action="http://www.excite.com/search.gw" method="get" name="frmExcite" id="frmExcite">
																					<input type="hidden" name="trace" value="a"> </font>
																					<table cellpadding="0" cellspacing="0" width="100%" align="center">
																						<tr>
																							<td width="100%" align="center" height="20" bgcolor="#000000">
																								<input type="text" size="15" name="search" style="font-family: Verdana; font-size: 8pt" id="ExciteFocus">
																								</font>
																							</td>
																						</tr>
																					</table>
																				</form>
																			</span></font>
																		</td>
																	</tr>
																</table>
																</font>
															</td>
														</tr>
														<tr>
															<td>
																</font>
																<table width="200" cellpadding="0" cellspacing="0" bgColor="#000000">
																	<tr>
																		<td>
																			</font>
																			<table cellpadding="0" cellspacing="0">
																				<tr>
																					<td bgcolor="black" width="100%" align="center" height="20" background="wash2020.gif">
																						<font face="Verdana" size="2" color="#FFFFFF">Kirkland Yellow Pages</font> </font>
																					</td>
																					<td align="right" height="20" background="wash2020.gif">
																						<img src="expand.gif" id="YellowPagesImage" onclick="ShowHide('YellowPages')" WIDTH="9" HEIGHT="9"><img src="trans.gif" width="4">
																						</font>
																					</td>
																				</tr>
																			</table>
																			<span id="YellowPagesContent" style="display: none">
																				<form NAME="BizBasicSearchForm" METHOD="GET" ACTION="http://search.yellowpages.msn.com/yellowpages/results">
																					<input TYPE="HIDDEN" NAME="BizBasicSearchForm"> <input NAME="RSC" TYPE="HIDDEN" VALUE="2">
																					<input NAME="HUSERFU" TYPE="HIDDEN" VALUE="0"> <input NAME="KWDT" TYPE="HIDDEN" VALUE="4">
																					<input NAME="HCKWD" TYPE="HIDDEN" VALUE> <input type="hidden" name="UIS" value="72">
																					<input NAME="CSZ" TYPE="HIDDEN" VALUE="KIRKLAND" SIZE="15" MAXLENGTH="255" VALUE>
																					<input NAME="STATE" TYPE="HIDDEN" VALUE="WA"> </font>
																					<table cellpadding="0" cellspacing="0" width="100%" align="center">
																						<tr>
																							<td width="100%" align="center" height="20" bgcolor="#000000">
																								<input type="text" size="15" name="KWD" style="font-family: Verdana; font-size: 8pt" id="YellowPagesFocus">
																								</font>
																							</td>
																						</tr>
																					</table>
																					<!--								<form method="get" action="http://yp.yahoo.com/py/ypBrowse.py?" name="frmYellowPages" id="frmYellowPages">								<input type="hidden" name="city" value="Redmond"><input type="hidden" name="state" value="WA">								<input type="hidden" name="zip" value="98052"><input type="hidden" name="stp" value="a">								</font><table cellpadding="0" cellspacing="0" width="100%" align="center">									<tr>										<td width="100%" align="center" height="20" bgcolor="#000000">											<input type="text" name="stx" size="15" maxlength="65" style="font-family: Verdana; font-size: 8pt" id="YellowPagesFocus">										</font></td>								  </tr>								</table>-->
																				</form>
																			</span></font>
																		</td>
																	</tr>
																</table>
																</font>
															</td>
														</tr>
														<tr>
															<td>
																</font>
																<table width="200" cellpadding="0" cellspacing="0" bgColor="#000000">
																	<tr>
																		<td>
																			</font>
																			<table cellpadding="0" cellspacing="0">
																				<tr>
																					<td bgcolor="black" width="100%" align="center" height="20" background="wash2020.gif">
																						<font face="Verdana" size="2" color="#FFFFFF">Dictionary</font> </font>
																					</td>
																					<td align="right" height="20" background="wash2020.gif">
																						<img src="expand.gif" id="DictionaryImage" onclick="ShowHide('Dictionary')" WIDTH="9" HEIGHT="9"><img src="trans.gif" width="4">
																						</font>
																					</td>
																				</tr>
																			</table>
																			<span id="DictionaryContent" style="display: none">
																				<form method="post" action="http://www.m-w.com/cgi-bin/dictionary" name="dict" id="frmDictionary">
																					<input type="hidden" name="book" value="Dictionary"> </font>
																					<table cellpadding="0" cellspacing="0" width="100%" align="center">
																						<tr>
																							<td width="100%" align="center" height="20" bgcolor="#000000">
																								<input type="text" name="va" size="15" style="font-family: Verdana; font-size: 8pt" id="DictionaryFocus">
																								</font>
																							</td>
																						</tr>
																					</table>
																				</form>
																			</span></font>
																		</td>
																	</tr>
																</table>
																</font>
															</td>
														</tr>
														<tr>
															<td>
																</font>
																<table width="200" cellpadding="0" cellspacing="0" bgColor="#000000">
																	<tr>
																		<td>
																			</font>
																			<table cellpadding="0" cellspacing="0">
																				<tr>
																					<td bgcolor="black" width="100%" align="center" height="20" background="wash2020.gif">
																						<font size="2" face="Verdana">Ask Jeeves</font></a> </font>
																					</td>
																					<td align="right" height="20" background="wash2020.gif">
																						<img src="expand.gif" id="AskJeevesImage" onclick="ShowHide('AskJeeves')" WIDTH="9" HEIGHT="9"><img src="trans.gif" width="4">
																						</font>
																					</td>
																				</tr>
																			</table>
																			<span id="AskJeevesContent" style="display: none">
																				<form METHOD="GET" ACTION="http://www.ask.com/main/askjeeves.asp" name="frmAskJeeves">
																					<input NAME="origin" TYPE="hidden" VALUE="0"> <input NAME="site_name" TYPE="hidden" VALUE="Jeeves">
																					<input NAME="metasearch" TYPE="hidden" VALUE="yes"> </font>
																					<table cellpadding="0" cellspacing="0" width="100%" align="center">
																						<tr>
																							<td width="100%" align="center" height="20" bgcolor="#000000">
																								<input NAME="ask" SIZE="20" MAXSIZE="255" style="font-size: 8pt; font-family: Verdana" id="AskJeevesFocus">
																								<input BORDER="0" SRC="Ask.gif" TYPE="image" name="Ask!" WIDTH="36" HEIGHT="23">
																								</font>
																							</td>
																						</tr>
																					</table>
																				</form>
																			</span></font>
																		</td>
																	</tr>
																</table>
																</font>
															</td>
														</tr>
													</table>
													</font>
												</td>
											</tr>
										</table>
										</font>
									</td>
									<td valign="top" colSpan="2">
										</font>
										<table border="0" width="100%" cellpadding="0" cellspacing="0">
											<tr>
												<td bgcolor="black" width="100%" align="center" height="20" background="wash2020.gif">
													<font face="Verdana" size="2" color="#FFFFFF"><!-- | 
							<a href="#" onclick="WeatherMain('SEA')">Seattle</a> |
							<a href="#" onclick="WeatherMain('MKE')">Milwaukee</a>-->
														<a href="#" onClick="WeatherMain('WAKI')">Kirkland</a> | <a href="#" onclick="WeatherMain('MSN')">
															Madison</a> Weather</font></font>
												</td>
											</tr>
											<tr>
												<td align="center" bgcolor="#000000">
													<div id="WebPartWPQ2">
														<div id="msnbcWeaWPQ2">
															<br>
															<br>
															<table border="0">
																<tr>
																	<td>
																		<img src="clogo.gif" height="30">
																	</td>
																	<td>
																		&nbsp;
																	</td>
																	<td>
																		<font size="-1">Loading...</font>
																	</td>
																	<td>
																		<img src="trans.gif" height="37" width="1">
																	</td>
																</tr>
															</table>
															<br>
															<br>
														</div>
														<script id="msnbcWeaDataWPQ2" language="javascript"></script>
														<script id="msnbcWeaRendWPQ2" language="javascript">
function GetDayOfWeek(sDay) { 
	switch (sDay) { 
		case '1': return 'Sun'; 
		case '2': return 'Mon'; 
		case '3': return 'Tue'; 
		case '4': return 'Wed'; 
		case '5': return 'Thu'; 
		case '6': return 'Fri'; 
		case '7': return 'Sat'; 
		default: return '???'; 
	} 
} 

function RenderWeather(oWea) { 
	var sTmp = ''; 
	var aFore = '';
	var aUpdate = oWea.swLastUp.split(" "); 
	var bFore = (oWea.swFore!=''); 
	
	if (bFore) {
		aFore = oWea.swFore.split("|");	
	} 
	
	sTmp += '<table cellpadding=0 cellspacing=3 border=0>'; 
	sTmp += '<tr>'; sTmp += '<td rowSpan=3 valign=top>'; 
	
	// Current condition 
	sTmp += '<img src=http://www.msnbc.com/m/wea/i/'+oWea.swCIcon+'_w.gif><br>'; 
	sTmp += '<font style=font-size:150%;color:ff9900;font-weight:bold;>'+oWea.swTemp+'&#176;</font>'; 
	sTmp += '</td>'; 
	
	// Text descriptions 
	sTmp += '<td rowSpan=3 valign=top>'; 
	sTmp += '<font style=font-size:90%;><b>'+oWea.swCity+'</b></font><br>'; 
	sTmp += '<font style=font-size:80%;>'; 
	sTmp += '<b>Wind</b>: '+oWea.swWindS+' '+oWea.swWindD+'<br>'; 
	sTmp += '<b>Real Feel</b>: '+oWea.swReal+'&#176;<br>'; 
	sTmp += '<b>Humidity</b>: '+oWea.swHumid+'%<br>'; 
	//sTmp += '<b>Baro</b>: '+oWea.swBaro+'"<br><b>UV</b>: '+oWea.swUV+'<br><b>Visibility</b>: '+oWea.swVis; 
	sTmp += '</font><font style=font-size:60%;>' + oWea.swLastUp + '</font>'; 
	sTmp += '</td>'; 
	sTmp += '<td rowSpan=3 valign=top>&nbsp;</td>'; 
	
	// Forecast 
	for (var i=0; i < 5; i++) { 
		sTmp += '<td align=center>'; 
		sTmp += '<font style=font-size:70%;>'+GetDayOfWeek(aFore[i])+'</font>'; 
		sTmp += '</td>'; 
	} 
	
	sTmp += '</tr><tr>'; 
	
	for (var i=0; i < 5; i++) { 
		sTmp += '<td>'; 
		sTmp += '<img width=44 height=60 src=http://www.msnbc.com/m/wea/i/'+aFore[i+10]+'_w.gif>'; 
		sTmp += '</td>'; 
	}
	
	sTmp += '</tr><tr>'; 
	
	for (var i=0; i < 5; i++) { 
		sTmp += '<td bgcolor=maroon align=center>'; 
		sTmp += '<font style=font-size:70%;>'+aFore[i+15]; 
		sTmp += '</td>'; 
	} 
	
	sTmp += '</tr><tr>'; 
	
	// other links 
	sTmp += '<td colSpan=3 align="center">'; 
	
	if (oWea.swCity == 'Seattle') { 
		sTmp += '<font size="1"><a href="http://www.wunderground.com/US/WA/Seattle.html">WUnder</a> | '; 
		sTmp += '<a href="http://weather.yahoo.com/forecast/Seattle_WA_US_f.html">Yahoo</a> | ';
		sTmp += '<font color="#00FF00"><a href="http://www.intellicast.com/Local/USLocalWide.asp?loc=ksea&seg=LocalWeather&prodgrp=RadarImagery&product=Radar&prodnav=none&pid=none">Radar</a></font></font></font>'; 
	} else if (oWea.swCity == 'Kirkland') { 
		sTmp += '<font size="1"><a href="http://www.wunderground.com/US/WA/Kirkland.html">WUnder</a> | '; 
		sTmp += '<a href="http://weather.yahoo.com/forecast/Kirkland_WA_US_f.html">Yahoo</a> | '; 
		sTmp += '<font color="#00FF00"><a href="http://www.intellicast.com/Local/USLocalWide.asp?loc=ksea&seg=LocalWeather&prodgrp=RadarImagery&product=Radar&prodnav=none&pid=none">Radar</a></font></font></font>'; 
	} else { 
		sTmp += '<font size="1"><a href="http://www.wunderground.com/US/WI/Madison.html">WUnder</a> | '; 
		sTmp += '<a href="http://weather.yahoo.com/forecast/Madison_WI_US_f.html">Yahoo</a> | '; 
		sTmp += '<font color="#00FF00"><a href="http://www.intellicast.com/LocalWeather/World/UnitedStates/Midwest/Wisconsin/Madison/RadarLoop/">Radar</a></font></font></font>'; 
	} 
	
	sTmp += '</td>'; 
	
	for (var i=0; i < 5; i++) { 
		sTmp += '<td bgcolor=navy align=center>'; 
		sTmp += '<font style=font-size:70%;>'+aFore[i+25]; 
		sTmp += '</td>'; 
	} 
	
	sTmp += '</tr>'; 
	// sTmp += '<tr><td align=center><font style=font-size:60%;>'+GetDayOfWeek(aFore[1])+'</td><td align=center><font style=font-size:60%;>'+GetDayOfWeek(aFore[2])+'</td><td align=center><font style=font-size:60%;>'+GetDayOfWeek(aFore[3])+'</td><td align=center><font style=font-size:60%;>'+GetDayOfWeek(aFore[4])+'</td></tr>'; 
	// sTmp += '<tr><td><img width=44 height=60 src=http://www.msnbc.com/m/wea/i/'+aFore[11]+'_w.gif></td><td><img width=44 height=60 src=http://www.msnbc.com/m/wea/i/'+aFore[12]+'_w.gif></td><td><img width=44 height=60 src=http://www.msnbc.com/m/wea/i/'+aFore[13]+'_w.gif></td><td><img width=44 height=60 src=http://www.msnbc.com/m/wea/i/'+aFore[14]+'_w.gif></td></tr>'; 
	// sTmp += '<tr><td align=center><font style=font-size:60%;>'+aFore[16]+'/'+aFore[26]+'</td><td align=center><font style=font-size:60%;>'+aFore[17]+'/'+aFore[27]+'</td><td align=center><font style=font-size:60%;>'+aFore[18]+'/'+aFore[28]+'</td><td align=center><font style=font-size:60%;>'+aFore[19]+'/'+aFore[29]+'</td></tr>'; 
	// sTmp += '<tr><td colSpan=5 align=center style=font-size:60%;><i>Last updated '+oWea.swLastUp+'</td></tr>'; sTmp += '</table>'; 
	
	return sTmp; 
} 
														</script>
														<!-- #include file="weather_js.asp"-->
														<script id="msnbcWeaMainWPQ2" language="javascript"><%=WeatherFunctions()%></script>
													</div>
												</td>
											</tr>
										</table>
										</font>
									</td>
								</tr>
								<tr>
									<td valign="top">
										<table border="0" width="100%" cellpadding="0" cellspacing="0">
											<tr>
												<td bgcolor="black" width="100%" align="center" height="20" background="wash2020.gif">
													<font face="Verdana" size="2" color="#FFFFFF">Microsoft</font></font>
												</td>
											</tr>
											<tr>
												<td width="100%" align="center" bgcolor="#000000">
													<font face="Verdana" size="2"><a href="http://www.microsoft.com/ms.htm">Home</a> | <a href="http://search.support.microsoft.com/kb/c.asp?SA=PER&amp;LNG=ENG">
															Support</a>&nbsp;<a href="http://msdn.microsoft.com/default.asp">
															<br>
															MSDN</a>: <a href="http://premium.microsoft.com/msdn/library/">Lib</a> | <a href="http://search.microsoft.com/us/dev/default.asp">
															Search</a> | <a href="http://msdn.microsoft.com/bugs/">Bugs</a>
														<br>
														<a href="http://www.betaplace.com">BetaPlace</a></font> <font face="Verdana" size="2">
														| <a href="http://beta.windowsupdate.com">WU</a> | <a href="http://windowsbeta.microsoft.com/Default.asp">
															XP</a> | <a href="http://beta.visualstudio.net">VS</a>
														<br>
														<a href="http://www.microsoft.com/windows2000">Win2k</a> | <a href="http://www.microsoft.com/technet">
															TechNet</a> | <a href="http://www.microsoft.com/commerceserver">Commerce</a>
														<br>
														<a href="http://www.msn.com">MSN</a> | <a href="http://www.hotmail.msn.com">Hotmail</a>
														| <a href="http://moneycentral.msn.com">Money</a> | <a href="http://slate.msn.com">Slate</a></font></font>
												</td>
											</tr>
										</table>
									</td>
									<td valign="top" align="center">
										</font>
										<table border="0" width="100%" cellpadding="0" cellspacing="0">
											<tr>
												<td bgcolor="black" width="100%" align="center" height="20" background="wash2020.gif">
													<font face="Verdana" size="2" color="#FFFFFF">News</font></font>
												</td>
											</tr>
											<tr>
												<td width="100%" align="center" bgcolor="#000000">
													<font size="2" face="Verdana"><a href="http://abcnews.go.com/">ABC</a> | <a href="http://www.cnn.com">
															CNN</a> | <a href="http://www.theonion.com">Onion</a>
														<br>
														<a href="http://www.usatoday.com">USA Today</a> | <a href="http://www.msnbc.com">MSNBC</a>
														<br>
														<a href="http://slashdot.org">SlashDot</a> | <a href="http://www.activewin.com">ActiveWin</a>
														<br>
														<a href="http://news.com">news.com</a> | <a href="http://www.15seconds.com">15secs</a>
														<br>
													</font>
												</td>
											</tr>
										</table>
										</font>
									</td>
								</tr>
								<tr>
									<td valign="middle" align="center">
										<table border="0" width="100%" cellpadding="0" cellspacing="0">
											<tr>
												<td bgcolor="black" width="100%" align="center" height="20" background="wash2020.gif">
													<font face="Verdana" size="2" color="#FFFFFF">Other Sites</font></font>
												</td>
											</tr>
											<tr>
												<td width="100%" align="center" bgcolor="#000000">
													<font face="Verdana"><small>Y!: <a href="http://my.yahoo.com">My</a> | <a href="http://local.yahoo.com">
																Local</a> | <a href="http://maps.yahoo.com">Maps</a> | <a href="http://mail.yahoo.com">
																Mail</a>
															<br>
															<a href="http://cws.internet.com/new.html">New Apps</a>
															<br>
															<br>
															<a href="http://mail.giesler.org">mail.giesler.org</a> | <a href="https://mail.microsoft.com/exchange">
																mail.ms.com</a></small></font>
												</td>
											</tr>
										</table>
									</td>
									<td align="center" valign="middle" align="center">
										</font>
										<table border="0" width="100%" cellpadding="0" cellspacing="0">
											<tr>
												<td bgcolor="black" width="100%" align="center" height="20" background="wash2020.gif">
													<font face="Verdana" size="2" color="#FFFFFF">TV</font></font>
												</td>
											</tr>
											<tr>
												<td width="100%" align="center" bgcolor="#000000">
													<font size="2" face="Verdana"><a href="http://msn.gist.com/tv/grid.jsp?page=1">TV 
															Listings</a>
														<br>
														<a href="http://espn.go.com">ESPN</a> | <a href="http://www.foxsports.com">FoxSports</a>
														<br>
														<a href="http://www.fox.com">Fox</a> | <a href="http://www.nbc.com">NBC</a> | <a href="http://www.abc.com">
															ABC</a> | <a href="http://www.cbs.com">CBS</a>
														<br>
														<a href="http://www.comedycentral.com">CC</a> | <a href="http://www.mtv.com">MTV</a>
														| <a href="http://www.discovery.com">Discovery</a> </font></font>
												</td>
											</tr>
										</table>
										</font>
									</td>
								</tr>
								<tr>
									<td valign="top" align="center">
										<table border="0" width="100%" cellpadding="0" cellspacing="0">
											<tr>
												<td bgcolor="black" width="100%" align="center" height="20" background="wash2020.gif">
													<font face="Verdana" size="2" color="#FFFFFF">UW</font></font>
												</td>
											</tr>
											<tr>
												<td width="100%" align="center" bgcolor="#000000">
													<font size="2" face="Verdana"><a href="http://www.wisc.edu" style="font-family: Verdana">
															Home</a> | <a href="http://www.uwbadgers.com/">Sports</a>
														<br>
														<a href="http://www.cs.wisc.edu">CS Home</a> | <a href="http://www.wisc.edu/union/">
															Union</a>
														<br>
														<a href="http://www.badgerherald.com">BH</a> | <a href="http://www.cardinal.wisc.edu">
															Cardinal</a> | <a href="http://uwalumni.com/email">Alum Email</a></font>
													</font>
												</td>
											</tr>
										</table>
									</td>
									<td valign="top" align="center">
										<table border="0" width="100%" cellpadding="0" cellspacing="0">
											<tr>
												<td bgcolor="black" width="100%" align="center" height="20" background="wash2020.gif">
													<font face="Verdana" size="2" color="#FFFFFF">Seattle</font></font>
												</td>
											</tr>
											<tr>
												<td width="100%" align="center" bgcolor="#000000">
													<font size="2" face="Verdana"><a href="http://seattletimes.nwsource.com/">Seattle Times</a>
														| <a href="http://seattlep-i.nwsource.com/">Seattle PI</a>
														<br>
														<a href="http://seattle.citysearch.com/">CitySearch</a>
														<br>
														<a href="http://www.wsdot.wa.gov/PugetSoundTraffic/">Puget Sound Traffic</a>
														<br>
													</font>
												</td>
											</tr>
										</table>
									</td>
								</tr>
							</table>
						</center>
					</div>
					<p>
						<br>
					</p>
					<p>
						&nbsp;
					</p>
					&nbsp;&nbsp;</font> 
					
				 
		
	<!--msnavigation--></td></tr><!--msnavigation--></table><!--msnavigation--><table border="0" cellpadding="0" cellspacing="0" width="100%"><tr><td>

<div align="center">
  <center>
  <table border="0" cellpadding="0" cellspacing="0" width="100%" height="30">
    <tr>
      <td width="100%" valign="middle" align="center" bgcolor="#C00000">
        <div align="center">
          <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
              <td valign="middle" align="center" width="100"><a href="http://www.microsoft.com/windows2000" target="_blank"><img border="0" src="../_borders/w2ksplash.gif" width="31" height="20" alt="Powered by Windows 2000"></a></td>
              <td valign="middle" align="center"><font size="1"> <a href="http://mike.giesler.org">mike.giesler.org</a>,
                a <a href="http://giesler.org">giesler.org</a> site</font></td>
              <td valign="middle" align="right" width="100"><img border="0" src="../_borders/border_copyright.gif" width="80" height="27"></td>
            </tr>
          </table>
        </div>
      </td>
    </tr>
  </table>
  </center>
</div>

</td></tr><!--msnavigation--></table></body>
</html>
