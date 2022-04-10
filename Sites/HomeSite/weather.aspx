<%@ Page Language="C#" AutoEventWireup="true" CodeFile="weather.aspx.cs" Inherits="weather" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>weather</title>
    <link href="Styles.css" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="width=device-width" /> 
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Panel runat="server" ID="errorPanel" Visible="false" ForeColor="DarkRed">
                <asp:Label runat="server" ID="error" />
            </asp:Panel>
            <asp:Panel runat="server" ID="weatherPanel" CssClass="weatherPanel">
                <div style="float: left; padding: 2px; margin-right: 4px;">
                    <table>
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
                <div style="float: left; padding: 2px; margin-right: 4px">
                    <table>
                        <tr>
                            <td style="border-bottom: 1px solid silver">inside</td>
                        </tr>
                        <tr>
                            <td>
                                <table style="border: 0;">
                                    <tr>
                                        <td class="tempTitle" style="padding-right: 6px">media rm</td>
                                        <td class="tempTitle" style="padding-right: 6px">master</td>
                                        <td class="tempTitle">network</td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="vertical-align: top">
                                            <asp:Label ID="mediaRoomCurrent" runat="server" CssClass="tempValue" />
                                            <br />
                                            <asp:Label ID="mediaRoomCO" runat="server" CssClass="coValue" />
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="bedroomCurrent" runat="server" CssClass="tempValue" />
                                            <br />
                                            <asp:Label ID="bedroomCO" runat="server" CssClass="coValue" />
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="waterClosetCurrent" runat="server" CssClass="tempValue" />
                                            <br />
                                            <asp:Label ID="waterClosetCO" runat="server" CssClass="coValue" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="float: left; padding: 2px; margin-right: 4px;">
                    <table>
                        <tr>
                            <td colspan="3" style="border-bottom: 1px solid silver"><a href="https://www.wunderground.com/weather/us/wa/bellevue/KWABELLE229" target="_blank">bellevue forecast</a></td>
                        </tr>
                        <tr>
                            <td class="tempTitle">today</td>
                            <td class="tempTitle">tomorrow</td>
                            <td class="tempTitle">
                                <asp:Label runat="server" ID="day2Label" /></td>
                        </tr>
                        <tr>
                            <td style="width: 115px">
                                <asp:Image runat="server" ID="day0icon" ImageAlign="Left" CssClass="icon" />
                                <asp:Label runat="server" ID="day0hi" CssClass="hiTemp" />
                                /
								<asp:Label runat="server" ID="day0low" CssClass="loTemp" />
                                <br />
                                <div class="coValue">
                                    <asp:Label runat="server" ID="day0pop" />&nbsp;/&nbsp;<asp:Label runat="server" ID="day0precip" />
                                </div>
                            </td>
                            <td style="width: 115px">
                                <asp:Image runat="server" ID="day1icon" ImageAlign="Left" CssClass="icon" />
                                <asp:Label runat="server" ID="day1hi" CssClass="hiTemp" />
                                /
								<asp:Label runat="server" ID="day1low" CssClass="loTemp" />
                                <br />
                                <div class="coValue">
                                    <asp:Label runat="server" ID="day1pop" />&nbsp;/&nbsp;<asp:Label runat="server" ID="day1precip" />
                                </div>
                            </td>
                            <td style="width: 115px">
                                <asp:Image runat="server" ID="day2icon" ImageAlign="Left" CssClass="icon" />
                                <asp:Label runat="server" ID="day2hi" CssClass="hiTemp" />
                                /
								<asp:Label runat="server" ID="day2low" CssClass="loTemp" />
                                <br />
                                <div class="coValue">
                                    <asp:Label runat="server" ID="day2pop" />&nbsp;/&nbsp;<asp:Label runat="server" ID="day2precip" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="float: left; padding: 2px" runat="server" id="randleWeatherDiv">
                    <table>
                        <tr>
                            <td colspan="2" style="border-bottom: 1px solid silver;"><a href="https://www.wunderground.com/forecast/us/wa/randle/98377" target="_blank">
                                <asp:Label runat="server" ID="randleHeader" /></a>
                            </td>
                        </tr>
                        <tr>
                            <td class="tempTitle">
                                <asp:Label runat="server" ID="randleDay0Name" /></td>
                            <td class="tempTitle">
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
                </div>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
