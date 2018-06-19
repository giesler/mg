
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html dir="ltr" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
    <meta http-equiv="cache-control" content="no-cache" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="refresh" content="1500" />
    <meta name="viewport" content="width=device-width" />
    <link href="Styles.css" rel="stylesheet" type="text/css" />
    <title>MS2N Home</title>
    <link rel="SHORTCUT ICON" href="favicon.ico" />
</head>

<body class="bodyStyle" style="color: black">

    <div class="headerLink" style="padding: 4px">
        <a href="http://www.ms2n.net/">ms2n.net</a>: <a href="http://home.ms2n.net" style="color:deepskyblue">home</a> | <a href="http://cams.ms2n.net/">cams</a> |  <a href="http://control.ms2n.net/">control</a>
    </div>
    <div style="width: 100%; clear: both; background-color: black">
        <a class="smallthumb" href="http://cams.ms2n.net/?c=dw1" target="_top">
            <img class="smallthumb" src="http://cams.ms2n.net/getimg.aspx?c=dw1&amp;h=64&amp;id=th" style="height: 64px; width: 85px" /></a>
        <a class="smallthumb" href="http://cams.ms2n.net/?c=front" target="_top">
            <img class="smallthumb" src="http://cams.ms2n.net/getimg.aspx?c=front&amp;h=64&amp;id=th" style="height: 64px; width: 85px" /></a>
        <a class="smallthumb" href="http://cams.ms2n.net/?c=side" target="_top">
            <img class="smallthumb" src="http://cams.ms2n.net/getimg.aspx?c=side&amp;h=64&amp;id=th" style="height: 64px; width: 85px" /></a>
        <a class="smallthumb" href="http://cams.ms2n.net/?c=gdoor" target="_top">
            <img class="smallthumb" src="http://cams.ms2n.net/getimg.aspx?c=gdoor&amp;h=64&amp;id=th" style="height: 64px; width: 85px" /></a>
        <a class="smallthumb" href="http://cams.ms2n.net/?c=CoopTop" target="_top">
            <img class="smallthumb" src="http://cams.ms2n.net/getimg.aspx?c=CoopTop&amp;h=64&amp;id=th" style="height: 64px; width: 102px" /></a>
<!--        <a class="smallthumb" href="http://cams.ms2n.net/?c=CoopDoor" target="_top">
            <img class="smallthumb" src="http://cams.ms2n.net/getimg.aspx?c=CoopDoor&amp;h=64&amp;id=th" style="height: 64px; width: 113px" /></a>
-->    </div>
    <asp:Panel runat="server" ID="errorPanel" Visible="false" Width="100%" ForeColor="DarkRed" >
        <asp:Label runat="server" ID="error" />
        </asp:Panel>
    <div style="float: left">
        <div style="float: left;">
            <table style="padding: 2px">
                <tr>
                    <td colspan="2" style="border-bottom: 1px solid black">outside</td>
                </tr>
                <tr>
                    <td>
                        <asp:Image ID="outsideImage" runat="server" ImageAlign="Left" CssClass="weatherImg" />
                        <asp:Label ID="outsideCurrent" runat="server" Font-Size="XX-Large" />
                        &nbsp;
                    </td>
                    <td>
                        <asp:Label ID="outsideHigh" CssClass="hiTemp" runat="server" />
                        <br />
                        <asp:Label ID="outsideLow" CssClass="loTemp" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <div style="float: left;">
            <table style="padding: 2px">
                <tr>
                    <td style="border-bottom: 1px solid black">inside</td>
                </tr>
                <tr>
                    <td>
                        <table style="border-spacing: 0px">
                            <tr>
                                <td>media room</td>
                                <td>&nbsp;</td>
                                <td>mstr bdrm</td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="mediaRoomCurrent" runat="server" Font-Size="X-Large" /></td>
                                <td>&nbsp;</td>
                                <td align="center">
                                    <asp:Label ID="bedroomCurrent" runat="server" Font-Size="X-Large" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div style="float: left">
        <table>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td colspan="3" style="border-bottom: 1px solid black"><a href="https://www.wunderground.com/forecast/us/wa/kirkland" target="_blank">kirkland forecast</a></td>
                        </tr>
                        <tr>
                            <td>today</td>
                            <td>tomorrow</td>
                            <td>
                                <asp:Label runat="server" ID="day2Label" /></td>
                        </tr>
                        <tr>
                            <td style="width: 110px">
                                <asp:Image runat="server" ID="day0icon" ImageAlign="Left" CssClass="icon" />
                                <asp:Label runat="server" ID="day0hi" CssClass="hiTemp" />
                                /
									<asp:Label runat="server" ID="day0low" CssClass="loTemp" />
                                <br />
                                <div style="font-size: x-small">
                                    <asp:Label runat="server" ID="day0pop" />&nbsp;/&nbsp;<asp:Label runat="server" ID="day0precip" />
                                </div>
                            </td>
                            <td style="width: 110px">
                                <asp:Image runat="server" ID="day1icon" ImageAlign="Left" CssClass="icon" />
                                <asp:Label runat="server" ID="day1hi" CssClass="hiTemp" />
                                /
									<asp:Label runat="server" ID="day1low" CssClass="loTemp" />
                                <br />
                                <div style="font-size: x-small">
                                    <asp:Label runat="server" ID="day1pop" />&nbsp;/&nbsp;<asp:Label runat="server" ID="day1precip" />
                                </div>
                            </td>
                            <td style="width: 110px">
                                <asp:Image runat="server" ID="day2icon" ImageAlign="Left" CssClass="icon" />
                                <asp:Label runat="server" ID="day2hi" CssClass="hiTemp" />
                                /
									<asp:Label runat="server" ID="day2low" CssClass="loTemp" />
                                <br />
                                <div style="font-size: x-small">
                                    <asp:Label runat="server" ID="day2pop" />&nbsp;/&nbsp;<asp:Label runat="server" ID="day2precip" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div style="float: left">
        <table>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td colspan="2" style="border-bottom: 1px solid black"><a href="https://www.wunderground.com/forecast/us/wa/randle/98377" target="_blank"><asp:Label runat="server" ID="randleHeader" /></a>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="randleDay0Name" /></td>
                            <td>
                                <asp:Label runat="server" ID="randleDay1Name" /></td>
                        </tr>
                        <tr>
                            <td style="width: 110px">
                                <asp:Image runat="server" ID="randleDay0Icon" ImageAlign="Left" CssClass="icon" />
                                <asp:Label runat="server" ID="randleDay0Hi" CssClass="hiTemp" />
                                /
									<asp:Label runat="server" ID="randleDay0Low" CssClass="loTemp" />
                                <br />
                                <div class="precip">
                                    <asp:Label runat="server" ID="randleDay0pop" />&nbsp;/&nbsp;<asp:Label runat="server" ID="randleDay0precip" />
                                </div>
                            </td>
                            <td style="width: 110px">
                                <asp:Image runat="server" ID="randleDay1Icon" ImageAlign="Left" CssClass="icon" />
                                <asp:Label runat="server" ID="randleDay1Hi" CssClass="hiTemp" />
                                /
									<asp:Label runat="server" ID="randleDay1Low" CssClass="loTemp" />
                                <br />
                                <div class="precip">
                                    <asp:Label runat="server" ID="randleDay1pop" />&nbsp;/&nbsp;<asp:Label runat="server" ID="randleDay1precip" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </div>
	<div style="clear: both">
        <div style="float: left; padding-right: 5px; padding-bottom: 10px; padding-left: 3px;">
            <div style="width: 100%; border-bottom: solid black 1px">
                home
            </div>
            <div class="subheader">sites</div>
            <a href="http://dome.msn2.net" target="_blank">Dome</a>
            <div class="subheader">services</div>
            <a href="http://192.168.1.55/WEB/had.htm" target="_blank">ISY</a> |
				<a href="https://home.nest.com/" target="_blank">Nest</a>
            <br />
            <a href="https://my.netatmo.com/app/station" target="_blank">Netatmo</a> | 
				<a href="https://app.rach.io/" target="_blank">Rachio</a><br />
            <a href="http://192.168.1.60:8081/" target="_blank">HA Bridge</a> | 
				<a href="http://hplaserjet/" target="_blank">HP</a>
            <div class="subheader">media</div>
            <a href="http://online.tivo.com" target="_blank">TiVo</a> |
				<a href="https://app.plex.tv/web/app#" target="_blank">Plex</a> |
				<a href="http://xfinitytv.comcast.net" target="_blank">Xfinity</a>
            <div class="subheader">network</div>
            Main: <a href="http://msn2w1.sp.msn2.net/" target="_blank">router</a> |
				<a href="http://msn2s1.sp.msn2.net/" target="_blank">switch</a><br />
            <a href="http://10.0.0.1" target="_blank">Modem</a>
            <div class="subheader">other</div>
            <a href="http://www.onedrive.com" target="_blank">OneDrive</a> | 
				<a href="http://www.outlook.com/owa/giesler.org/" target="_blank">OWA</a>
        </div>
        <div style="float: left; padding-right: 5px; padding-bottom: 10px; padding-left: 3px;">
            <div style="width: 100%; border-bottom: solid black 1px">
                top links
            </div>
            <div class="topsubheader">search</div>
            <a href="http://www.bing.com/" target="_blank">Bing</a> | 
				<a href="http://www.google.com/" target="_blank">Google</a> | 
				<a href="http://en.wikipedia.org/wiki/Main_Page" target="_blank">Wikipedia</a> | 
				<a href="http://www.yahoo.com/" target="_blank">Yahoo</a><br />
            <div class="subheader">weather</div>
            <a href="http://www.wunderground.com/US/WA/Kirkland.html" target="_blank">WUnder</a> |
				<a href="http://www.wunderground.com/wundermap/?lat=47.68687057&lon=-122.18956757&zoom=8&pin=Kirkland%2c%20WA&rad=1&wxsn=0&svr=0&cams=0&sat=0&riv=0&mm=0&hur=0" target="_blank">Radar</a> |
				<a href="http://www.accuweather.com/en/us/kirkland-wa/98033/minute-weather-forecast/341298" target="_blank">MinuteCast</a>
            <div class="subheader">food</div>
            <a href="http://www.allrecipes.com/" target="_blank">All Recipies</a> |
				<a href="http://www.epicurious.com/" target="_blank">Epicurious</a> |
				<a href="http://www.seriouseats.com/" target="_blank">Serious Eats</a><br />
            <a href="http://thekitchn.com" target="_blank">The Kitchn</a> |
				<a href="http://food52.com" target="_blank">Food 52</a> |
				<a href="http://eater.com" target="_blank">Eater</a>
            <div class="subheader">shopping</div>
            <a href="http://smile.amazon.com/?tag=charity48-20" target="_blank">Amazon</a> |
				<a href="http://www.newegg.com/" target="_blank">NewEgg</a> |
				<a href="https://www.wunderlist.com/webapp" target="_blank">Wunderlist</a>
            <div class="subheader">tech</div>
            <a href="http://www.gizmodo.com/" target="_blank">Gizmodo</a> |
				<a href="http://www.engadget.com/" target="_blank">Engadget</a> |
				<a href="http://www.lifehacker.com/" target="_blank">Lifehacker</a> |
				<a href="http://www.slashdot.org/" target="_blank">/.</a>
        </div>
        <div style="float: left; padding-right: 5px; padding-bottom: 10px; padding-left: 3px;">
            <div style="width: 100%; border-bottom: solid black 1px">
                news
            </div>
            <div class="topsubheader">top</div>
            <a href="http://www.bbc.co.uk/" target="_blank">BBC</a> |
				<a href="http://news.google.com/" target="_blank">Google</a> |
				<a href="http://t.msn.com/" target="_blank">MSN</a> | 
				<a href="http://www.nbcnews.com/" target="_blank">NBC</a> | 
				<a href="http://www.nytimes.com/" target="_blank">NYT</a>
            <div class="subheader">local</div>
            <a href="http://seattlepi.nwsource.com/" target="_blank">PI</a> | 
				<a href="http://www.seattletimes.com/" target="_blank">Times</a> |
				<a href="http://www.kirklandreporter.com/" target="_blank">Kirkland</a>
            <div class="subheader">other</div>
            <a href="http://www.npr.org/" target="_blank">NPR</a> | 
				<a href="http://www.newscientist.com/news/" target="_blank">New Scientist </a>|
				<a href="http://www.vox.com/" target="_blank">Vox</a>
            <div class="subheader">opinion</div>
            <a href="http://www.salon.com/" target="_blank">Salon</a> | 
				<a href="http://slate.com/" target="_blank">Slate</a> | 
				<a href="http://www.theatlantic.com/" target="_blank">Atlantic</a> |
				<a href="http://www.huffingtonpost.com/" target="_blank">HuffPo</a>
            <div class="subheader">politics</div>
            <a href="http://mediamatters.org/" target="_blank">Media Matters</a> | 
				<a href="http://washingtonvotes.org/" target="_blank">WA Votes </a>
            <div class="subheader">social</div>
            <a href="http://facebook.com/?sk=h_chr" target="_blank">Facebook</a> |
				<a href="http://www.linkedin.com/" target="_blank">LinkedIn</a> |
				<a href="http://www.twitter.com/" target="_blank">Twitter</a> |
				<a href="http://www.instagram.com/" target="_blank">Instagram</a>
        </div>
    </div>
    <div style="float: left; padding-right: 5px; padding-bottom: 10px; padding-left: 3px;">
        <div style="width: 100%; border-bottom: solid black 1px">
            local
        </div>
        <div class="topsubheader">traffic</div>
        <a href="http://www.wsdot.wa.gov/traffic/seattle/flowmaps/bridges.htm" target="_blank">Maps</a> | 
				<a href="http://www.wsdot.com/traffic/seattle/incidents/default.aspx" target="_blank">Incidents</a> | 
				<a href="http://www.bellevuewa.gov/trafficmap/" target="_blank">Bellevue</a>
        <br />
        <div class="subheader">government</div>
        <a href="http://www.kingcounty.gov/" target="_blank">King County</a> | 
				<a href="http://www.ci.kirkland.wa.us/" target="_blank">Kirkland</a>
        <br />
        <div class="subheader">environment</div>
        <a href="http://www.pscleanair.org/airq/status.aspx" target="_blank">Burning</a> | 
				<a href="http://www.seattle.gov/util/EnvironmentConservation/OurWatersheds/index.htm" target="_blank">Water</a><br />
        <div class="subheader">food</div>
        <a href="https://www.dinnerdeliveryplus.com/index.php?module=modRDS&op=restaurants" target="_blank">Dinner Delivery</a> | 
				<a href="http://www.bitesquad.com/search/?search%5Baddress%5D=12627+NE+87th+Pl%2C+Kirkland%2C+WA%2C+United+States&search%5Bdistance%5D=5&search%5BstreetAddress%5D=12627+Northeast+87th+Place&search%5BunitNumber%5D=&search%5Bcity%5D=Kirkland&search%5Bstate%5D=Washington&search%5BzipCode%5D=98033-5966&search%5Blatitude%5D=47.680706&search%5Blongitude%5D=-122.17140240000003&search%5BmaxResults%5D=20&search%5BfirstResult%5D=0&search%5BorderTypes%5D=1" target="_blank">Bite Squad</a> | 
				<a href="http://pagliacci.com/menu" target="_blank">Pagliacci</a>
        <br />
        <div class="subheader">misc</div>
        <a href="http://seattle.citysearch.com/" target="_blank">CitySearch</a> | 
				<a href="http://www.vegseattle.com/" target="_blank">VegSeattle</a>
        <br />
        <div class="subheader">bus stops</div>
        <a href="http://pugetsound.onebusaway.org/where/standard/stop.action?id=1_74342" target="_blank">Honda</a> | 
				<a href="http://pugetsound.onebusaway.org/where/standard/stop.action?id=1_73845" target="_blank">Safeway</a> |
				<a href="http://pugetsound.onebusaway.org/where/standard/stop.action?id=1_73953" target="_blank">McDonalds</a> | 
				<a href="http://pugetsound.onebusaway.org/where/standard/#ll(47.6794,-122.1754)spn(0.0110,0.0187)" target="_blank">more...</a>
    </div>
    <div style="float: left; padding-left: 3px;">
        <div style="width: 100%; border-bottom: solid black 1px">
            media
        </div>
        <div class="topsubheader">tv</div>
        <a href="http://tv.msn.com/" target="_blank">MSN TV </a>
        <br />
        <div class="subheader">movies</div>
        <a href="http://movies.msn.com/movies/finda.aspx?shloc=98033&amp;famsel=-1" target="_blank">Showtimes</a> |
				<a href="http://www.netflix.com/" target="_blank">NetFlix</a> |
				<a href="http://www.cinesift.com/" target="_blank">Cinesift</a>
        <br />
        <div class="subheader">video</div>
        <a href="http://www.youtube.com/" target="_blank">YouTube</a> |
				<a href="http://www.hulu.com/" target="_blank">Hulu</a><br />
        <div class="subheader">radio</div>
        <a href="http://kexp.org/home.asp?noflash=false" target="_blank" title="90.3">KEXP</a> |
				<a href="http://kuow.org/" target="_blank" title="94.9">KUOW</a> |
				<a href="http://www.1037themountain.com/" target="_blank" title="103.7 The Mountain">KMTT</a><br />
        <div class="subheader">other</div>
        <a href="http://www.feedly.com/" target="_blank">Feedly</a> | 
				<a href="http://www.instapaper.com/" target="_blank">Instapaper</a> |
				<a href="https://play.pocketcasts.com/web/podcasts/index#/podcasts/new_releases" target="_blank">Casts</a>
    </div>
</body>
</html>

