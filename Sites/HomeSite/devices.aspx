<%@ Page Language="C#" AutoEventWireup="true" CodeFile="devices.aspx.cs" Inherits="devices" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>devices</title>
    <link href="https://home.giesler.org/Styles.css" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="width=device-width" /> 
    <script type="text/javascript">
        function toggleItem() {
            document.all['sendingPanel'].style.visibility = 'visible';
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        <asp:Panel runat="server" ID="errorPanel" Visible="false" ForeColor="DarkRed">
            <asp:Label runat="server" ID="error" />
        </asp:Panel>
        <asp:Panel runat="server" ID="weatherPanel" CssClass="weatherPanel">
            <div style="width: 100%; border-bottom: solid silver 1px">
                status
            </div>
            <asp:Panel runat="server" ID="sendingPanel" CssClass="popupPanel">
                <br />
                <br />
                <img src="loading.gif" height="30" />
            </asp:Panel>
            <div>
                <smaller>
                    <table cellpadding="2" cellspacing="0">
                        <tr>
                            <td rowspan="2" style="vertical-align:top; padding-right: 6px;"><small>locks:</small></td>
                            <td style="padding-right: 6px"><small>porch:</small></td>
                            <td>
                                <small><asp:LinkButton Text="kitchen" runat="server" id="patioKitchenLockAction" OnClick="patioKitchenLockActionClick" OnClientClick="toggleItem();" /></small> | 
                                <small><asp:LinkButton Text="garage" runat="server" id="patioGarageLockAction" OnClick="patioGarageLockActionClick" OnClientClick="toggleItem();" /></small>
                            </td>
                        </tr>
                        <tr>
                            <td><small>garage:</small></td>
                            <td>
                                <small><asp:LinkButton Text="backyard" runat="server" id="garageBackLockAction" OnClick="garageBackLockActionClick" OnClientClick="toggleItem();" /></small> |
                                <small><asp:LinkButton Text="tack room" runat="server" id="garageInsideLockAction" OnClick="garageInsideLockActionClick" OnClientClick="toggleItem();" /></small>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="2" style="vertical-align:top; padding-right: 6px;"><small>doors:</small> </td>
                            <td style="padding-right: 13px"><small>garage: </small></td>
                            <td>
                                <small>
                                    <asp:LinkButton Text="north" runat="server" id="garageDoorNorthAction" OnClick="garageDoorNorthAction_Click" OnClientClick="toggleItem();" /> | 
                                    <asp:LinkButton Text="center" runat="server" id="garageDoorCenterAction" OnClick="garageDoorCenterAction_Click" OnClientClick="toggleItem();" /> | 
                                    <asp:LinkButton Text="south" runat="server" id="garageDoorSouthAction" OnClick="garageDoorSouthAction_Click" OnClientClick="toggleItem();" />
                                </small>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-right: 13px"><small>outside: </small></td>
                            <td>
                                <asp:Label runat="server" ID="shedDoor" Text="shed" Font-Size="Medium" />
                            </td>
                        </tr>
                    </table>
                </smaller>
            </div>
        </asp:Panel>
        </div>
    </form>
</body>
</html>
