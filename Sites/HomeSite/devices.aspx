<%@ Page Language="C#" AutoEventWireup="true" CodeFile="devices.aspx.cs" Inherits="devices" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>devices</title>
    <link href="https://home.giesler.org/Styles.css" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="width=device-width" /> 
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
            <div>
                <smaller>
                    <table cellpadding="2" cellspacing="0">
                        <tr>
                            <td rowspan="2" style="vertical-align:top; padding-right: 6px;"><small>locks:</small></td>
                            <td style="padding-right: 6px"><small>porch:</small></td>
                            <td>
                                <small><asp:LinkButton Text="kitchen" runat="server" id="patioKitchenLockAction" OnClick="patioKitchenLockActionClick" /></small> | 
                                <small><asp:LinkButton Text="garage" runat="server" id="patioGarageLockAction" OnClick="patioGarageLockActionClick" /></small>
                            </td>
                        </tr>
                        <tr>
                            <td><small>garage:</small></td>
                            <td>
                                <small><asp:LinkButton Text="backyard" runat="server" id="garageBackLockAction" OnClick="garageBackLockActionClick" /></small> |
                                <small><asp:LinkButton Text="tack room" runat="server" id="garageInsideLockAction" OnClick="garageInsideLockActionClick" /></small>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="2" style="vertical-align:top; padding-right: 6px;"><small>doors:</small> </td>
                            <td style="padding-right: 13px"><small>garage: </small></td>
                            <td>
                                <small>
                                    <asp:LinkButton Text="north" runat="server" id="garageDoorNorthAction" OnClick="garageDoorNorthAction_Click" /> | 
                                    <asp:LinkButton Text="center" runat="server" id="garageDoorCenterAction" OnClick="garageDoorCenterAction_Click" /> | 
                                    <asp:LinkButton Text="south" runat="server" id="garageDoorSouthAction" OnClick="garageDoorSouthAction_Click" />
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
