
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html dir="ltr" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
    <meta http-equiv="cache-control" content="no-cache" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta name="viewport" content="width=device-width" />
    <link href="Styles.css" rel="stylesheet" type="text/css" />
    <title>MS2N Home</title>
    <link rel="SHORTCUT ICON" href="favicon.ico" />
    <script language="javascript" type="text/javascript">
        function OnReloadPage() {
            try {
                window.location.reload()
                this.all.refreshError.innerText = ''
            }
            catch {
                this.all.refreshError.innerText = $_
                setTimeout(OnReloadPage, 10000)
            }
        }

        setTimeout(OnReloadPage, 300000)
    </script>
</head>

<body class="bodyStyle">
    <div class="headerLink" style="padding: 4px; border-bottom-color: silver;">
        <a href="/" style="color: deepskyblue">home</a> |
        <a href="/cams/">cams</a>
        <!-- | <a href="http://control.ms2n.net/">control</a> ffffffffff -->
    </div>
    <div style="clear: both; background-color: black;">
        <a class="smallthumb" href="/cams/?c=gdw" target="_blank">
            <img class="smallthumb" src="https://svcs.ms2n.net:8443/getimg.aspx?c=gdw&amp;h=128&amp;id=th" style="height: 128px; width: 200px" alt="cam1"/></a>
        <a class="smallthumb" href="/cams/?c=dwety" target="_blank">
            <img class="smallthumb" src="https://svcs.ms2n.net:8443/getimg.aspx?c=dwety&amp;h=128&amp;id=th" style="height: 128px; width: 200px" alt="cam2" /></a>
        <!--<a class="smallthumb" href="/cams/?c=side" target="_blank">
            <img class="smallthumb" src="/getimg.aspx?c=side&amp;h=64&amp;id=th" style="height: 64px; width: 85px" /></a>
        <a class="smallthumb" href="/cams/?c=gdoor" target="_blank">
            <img class="smallthumb" src="/getimg.aspx?c=gdoor&amp;h=64&amp;id=th" style="height: 64px; width: 85px" /></a>
-->
        <a class="smallthumb" href="https://www.wsdot.wa.gov/aviation/WebCam/Packwood.htm" target="_blank">
            <img class="smallthumb" src="https://images.wsdot.wa.gov/airports/packwood5.jpg" style="height: 128px; width: 223px;" alt="packwood" /></a>
    </div>
    <asp:Panel runat="server" ID="errorPanel" Visible="false" ForeColor="DarkRed">
        <asp:Label runat="server" ID="error" />
    </asp:Panel>
    <asp:Panel runat="server" ID="weatherPanel" CssClass="weatherPanel">
        <div style="float: left; padding-top: 4px; margin: 8px">
            <table cellpadding="2px">
                <tr>
                    <td colspan="2" style="border-bottom: 1px solid silver">outside</td>
                </tr>
                <tr>
                    <td>
                        <asp:Image ID="outsideImage" runat="server" ImageAlign="Left" CssClass="weatherImg" />
                        &nbsp;<asp:Label ID="outsideCurrent" runat="server" Font-Size="XX-Large" />
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
        <div style="float: left; padding: 2px; padding-top: 6px; margin: 8px">
            <table>
                <tr>
                    <td style="border-bottom: 1px solid silver">inside</td>
                </tr>
                <tr>
                    <td>
                        <table style="border-spacing: 0px">
                            <tr>
                                <td>media room</td>
                                <td>&nbsp;</td>
                                <td>north mstr</td>
                                <td>&nbsp;</td>
                                <td>wtr clst</td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="mediaRoomCurrent" runat="server" Font-Size="X-Large" /><br />
                                    <asp:Label ID="mediaRoomCO" runat="server" Font-Size="X-Small" />
                                </td>
                                <td>&nbsp;</td>
                                <td align="center">
                                    <asp:Label ID="bedroomCurrent" runat="server" Font-Size="X-Large" /><br />
                                    <asp:Label ID="bedroomCO" runat="server" Font-Size="X-Small" />
                                </td>
                                <td>&nbsp;</td>
                                <td align="center">
                                    <asp:Label ID="waterClosetCurrent" runat="server" Font-Size="X-Large" /><br />
                                    <asp:Label ID="waterClosetCO" runat="server" Font-Size="X-Small" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div style="float: left; margin: 8px">
            <table>
                <tr>
                    <td>
                        <table style="padding: 2px">
                            <tr>
                                <td colspan="3" style="border-bottom: 1px solid silver"><a href="https://www.wunderground.com/weather/us/wa/bellevue/KWABELLE229" target="_blank">bellevue forecast</a></td>
                            </tr>
                            <tr>
                                <td>today</td>
                                <td>tomorrow</td>
                                <td>
                                    <asp:Label runat="server" ID="day2Label" /></td>
                            </tr>
                            <tr>
                                <td style="width: 115px">
                                    <asp:Image runat="server" ID="day0icon" ImageAlign="Left" CssClass="icon" />
                                    <asp:Label runat="server" ID="day0hi" CssClass="hiTemp" />
                                    /
									<asp:Label runat="server" ID="day0low" CssClass="loTemp" />
                                    <br />
                                    <div style="font-size: medium">
                                        <asp:Label runat="server" ID="day0pop" />&nbsp;/&nbsp;<asp:Label runat="server" ID="day0precip" />
                                    </div>
                                </td>
                                <td style="width: 115px">
                                    <asp:Image runat="server" ID="day1icon" ImageAlign="Left" CssClass="icon" />
                                    <asp:Label runat="server" ID="day1hi" CssClass="hiTemp" />
                                    /
									<asp:Label runat="server" ID="day1low" CssClass="loTemp" />
                                    <br />
                                    <div style="font-size: medium">
                                        <asp:Label runat="server" ID="day1pop" />&nbsp;/&nbsp;<asp:Label runat="server" ID="day1precip" />
                                    </div>
                                </td>
                                <td style="width: 115px">
                                    <asp:Image runat="server" ID="day2icon" ImageAlign="Left" CssClass="icon" />
                                    <asp:Label runat="server" ID="day2hi" CssClass="hiTemp" />
                                    /
									<asp:Label runat="server" ID="day2low" CssClass="loTemp" />
                                    <br />
                                    <div style="font-size: medium">
                                        <asp:Label runat="server" ID="day2pop" />&nbsp;/&nbsp;<asp:Label runat="server" ID="day2precip" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div style="float: left; margin: 8px;">
            <table>
                <tr>
                    <td>
                        <table style="padding: 2px">
                            <tr>
                                <td colspan="2" style="border-bottom: 1px solid silver"><a href="https://www.wunderground.com/forecast/us/wa/randle/98377" target="_blank">
                                    <asp:Label runat="server" ID="randleHeader" /></a>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="randleDay0Name" /></td>
                                <td>
                                    <asp:Label runat="server" ID="randleDay1Name" /></td>
                            </tr>
                            <tr>
                                <td style="width: 115px">
                                    <asp:Image runat="server" ID="randleDay0Icon" ImageAlign="Left" CssClass="icon" />
                                    <asp:Label runat="server" ID="randleDay0Hi" CssClass="hiTemp" />
                                    /
									<asp:Label runat="server" ID="randleDay0Low" CssClass="loTemp" />
                                    <br />
                                    <div class="precip" style="display:none">
                                        <asp:Label runat="server" ID="randleDay0pop" />&nbsp;/&nbsp;<asp:Label runat="server" ID="randleDay0precip" />
                                    </div>
                                </td>
                                <td style="width: 115px">
                                    <asp:Image runat="server" ID="randleDay1Icon" ImageAlign="Left" CssClass="icon" />
                                    <asp:Label runat="server" ID="randleDay1Hi" CssClass="hiTemp" />
                                    /
									<asp:Label runat="server" ID="randleDay1Low" CssClass="loTemp" />
                                    <br />
                                    <div class="precip" style="display:none">
                                        <asp:Label runat="server" ID="randleDay1pop" />&nbsp;/&nbsp;<asp:Label runat="server" ID="randleDay1precip" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <div style="clear: both; padding-left: 6px">
        <div style="float: left; padding-right: 5px; padding-bottom: 10px; padding-left: 3px; margin: 8px">
            <div style="width: 100%; border-bottom: solid silver 1px">
                home
            </div>
                <a href="https://mysecurity.eufylife.com/#/camera" target="_blank">Eufy</a> | 
                <a href="https://myhs.homeseer.com/" target="_blank">MyHS</a> | 
            <a href="https://www.ecobee.com/consumerportal/" target="_blank">ecobee</a> | 
            <a href="https://my.netatmo.com/app/station" target="_blank">Netatmo</a> | 
				<a href="https://app.rach.io/" target="_blank">Rachio</a>
        </div>
    </div>
    <div style="vertical-align: bottom; color: darkred">
        <div id="refreshError"></div>
    </div>
</body>
</html>
