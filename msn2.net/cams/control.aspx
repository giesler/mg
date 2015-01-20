<%@ Page Language="C#" AutoEventWireup="true" CodeFile="control.aspx.cs" Inherits="control" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>msn2 home control</title>
    <link href="Styles.css" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="width=device-width" /> 
</head>
<body>
    <form id="form1" runat="server" style="height: 100%">
    <asp:Panel runat="server" ID="topLinks" CssClass="linkPanel">
        <a href="/">HOME</a> | 
        <a href="Login.aspx">SIGN OUT</a>
    </asp:Panel>
    <table style="width: 100%">
        <tr>
            <td class="mainItem">garage</td>
            <td rowspan="2"><asp:Button runat="server" ID="toggleGarage" Text="toggle" OnClick="toggleGarage_Click" /></td>
        </tr>
        <tr>
            <td><asp:Label runat="server" ID="garageStatus" CssClass="smallNote" /></td>
        </tr>
                
        <tr style="border-top: solid 2px silver">
            <td class="mainItem">media room</td>
            <td rowspan="2"><asp:Button runat="server" ID="mediaRoomOn" Text=" on " OnClick="mediaRoomOn_Click"/> 
                 <asp:Button runat="server" ID="mediaRoomOff" Text=" off " OnClick="mediaRoomOff_Click" />
            </td>
        </tr>
        <tr>
            <td><asp:Label runat="server" ID="mediaRoomStatus" CssClass="smallNote" /></td>
        </tr>

        <tr style="border-top: solid 2px silver">
            <td class="mainItem">upstairs hall</td>
            <td rowspan="2"><asp:Button runat="server" ID="upstairsHallOn" Text=" on " OnClick="upstairsHallOn_Click" />
                 <asp:Button runat="server" ID="upstairsHallOff" Text=" off " OnClick="upstairsHallOff_Click" />
            </td>
        </tr>
        <tr>
            <td><asp:Label runat="server" ID="upstairsHallStatus" CssClass="smallNote" /></td>
        </tr>

    </table>
    </form>
</body>
</html>
