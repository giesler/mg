<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HomeDash.aspx.cs" Inherits="HomeDash" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin: 0px">
    <form id="form1" runat="server">
        <div style="float: left">
        <table style="padding: 5px; width: 500px">
            <tr>
                <td style="width: 200px; align-content: center">
                    outside
                </td>
                <td style="width: 150px; align-content: center">
                    media room
                </td>
                <td style="width: 150px; align-content: center">
                    bedroom
                </td>
            </tr>
            <tr>
                <td style="align-content: center">
                    <table style="padding: 16px; border: 1px solid black">
                        <tr>
                            <td>
                                <asp:Image ID="outsideImage" runat="server" ImageAlign="Left" />
                                <asp:Label ID="outsideCurrent" runat="server" Font-Size="XX-Large" />
                                <asp:Label ID="outsideCurrentDecimal" runat="server" Font-Size="X-Small" />
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="outsideHigh" runat="server" /><asp:Label ID="outsideHighDecimal" runat="server" Font-Size="XX-Small" />
                                <br />
                                <asp:Label ID="outsideLow" runat="server" /><asp:Label ID="outsideLowDecimal" runat="server" Font-Size="XX-Small" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="align-content: center">
                    <table style="padding: 20px; border: 1px solid black">
                        <tr>
                            <td>
                                <asp:Label ID="mediaRoomCurrent" runat="server" Font-Size="XX-Large" />
                                <asp:Label ID="mediaRoomCurrentDecimal" runat="server" Font-Size="X-Small" />
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="mediaRoomHigh" runat="server" /><asp:Label ID="mediaRoomHighDecimal" runat="server" Font-Size="XX-Small" />
                                <br />
                                <asp:Label ID="mediaRoomLow" runat="server" /><asp:Label ID="mediaRoomLowDecimal" runat="server" Font-Size="XX-Small" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="align-content: center">
                    <table style="padding: 20px; border: 1px solid black">
                        <tr>
                            <td>
                                <asp:Label ID="bedroomCurrent" runat="server" Font-Size="XX-Large" />
                                <asp:Label ID="bedroomCurrentDecimal" runat="server" Font-Size="X-Small" />
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="bedroomHigh" runat="server" /><asp:Label ID="bedroomHighDecimal" runat="server" Font-Size="XX-Small" />
                                <br />
                                <asp:Label ID="bedroomLow" runat="server" /><asp:Label ID="bedroomLowDecimal" runat="server" Font-Size="XX-Small" />
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
                                <td colspan="3" style="border-bottom: 1px solid black">kirkland forecast</td>
                            </tr>
                            <tr>
                                <td>today</td>
                                <td>tomorrow</td>
                                <td><asp:Label runat="server" ID="day2Label" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Image runat="server" ID="day0icon" ImageAlign="Left" />
                                    <asp:Label runat="server" ID="day0hi" /> / <asp:Label runat="server" ID="day0low" /> <br />
                                    <div style="font-size: x-small"><asp:Label runat="server" ID="day0pop" />&nbsp;/&nbsp;<asp:Label runat="server" ID="day0precip"/></div>
                                </td>
                                <td>
                                    <asp:Image runat="server" ID="day1icon" ImageAlign="Left" />
                                    <asp:Label runat="server" ID="day1hi" /> / <asp:Label runat="server" ID="day1low" /> <br />
                                    <div style="font-size: x-small"><asp:Label runat="server" ID="day1pop" />&nbsp;/&nbsp;<asp:Label runat="server" ID="day1precip"/></div>
                                </td>
                                <td>
                                    <asp:Image runat="server" ID="day2icon" ImageAlign="Left" />
                                    <asp:Label runat="server" ID="day2hi" /> / <asp:Label runat="server" ID="day2low" /> <br />
                                    <div style="font-size: x-small"><asp:Label runat="server" ID="day2pop" />&nbsp;/&nbsp;<asp:Label runat="server" ID="day2precip" /></div>
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
                                <td colspan="2" style="border-bottom: 1px solid black"><asp:Label runat="server" ID="randleHeader" /></td>
                            </tr>
                            <tr>
                                <td><asp:Label runat="server" ID="randleDay0Name" /></td>
                                <td><asp:Label runat="server" ID="randleDay1Name" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Image runat="server" ID="randleDay0Icon" ImageAlign="Left" />
                                    <asp:Label runat="server" ID="randleDay0Hi" /> / <asp:Label runat="server" ID="randleDay0Low" /> <br />
                                    <div style="font-size: x-small"><asp:Label runat="server" ID="randleDay0pop" />&nbsp;/&nbsp;<asp:Label runat="server" ID="randleDay0precip"/></div>
                                </td>
                                <td>
                                    <asp:Image runat="server" ID="randleDay1Icon" ImageAlign="Left" />
                                    <asp:Label runat="server" ID="randleDay1Hi" /> / <asp:Label runat="server" ID="randleDay1Low" /> <br />
                                    <div style="font-size: x-small"><asp:Label runat="server" ID="randleDay1pop" />&nbsp;/&nbsp;<asp:Label runat="server" ID="randleDay1precip"/></div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>

    </form>
</body>
</html>
