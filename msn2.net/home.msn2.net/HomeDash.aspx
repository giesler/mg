<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HomeDash.aspx.cs" Inherits="HomeDash" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; margin: 0px">
    <form id="form1" runat="server">
        <table style="padding: 5px; width: 500px">
            <tr>
                <td style="width: 200px; align-content: center">
                    OUTSIDE
                </td>
                <td style="width: 150px; align-content: center">
                    MEDIA ROOM
                </td>
                <td style="width: 150px; align-content: center">
                    BEDROOM
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
    </form>
</body>
</html>
