<%@ Page Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="control" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>msn2 home control</title>
    <link href="Styles.css" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="width=device-width" /> 
    <style type="text/css">
        A:link
        {
            text-decoration: none;
            color: navy;
        }

        .headerLink A:link
        {
            text-decoration: none;
            color: white;
        }

        .headerLink A:visited
        {
            text-decoration: none;
            color: white;
        }

        .subheader
        {
            font-size: x-small;
            padding-top: 4px;
            margin-bottom: -2px;
        }

        .topsubheader
        {
            font-size: x-small;
            margin-bottom: -2px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" style="height: 100%">
   <div style="background: black; color: white; padding: 6px; font-weight: bold" class="headerLink">
        <a href="http://www.msn2.net/">MSN2.NET</a>: <a href="http://home.msn2.net">HOME</a> | <a href="http://cams.msn2.net/">CAMS</a> |  <a href="http://control.msn2.net/">CONTROL</a> | 
	    <a href="http://ts.msn2.net/">TS</a>
        <br />
    </div>
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

        <tr style="border-top: solid 2px silver">
            <td class="mainItem">garden drip</td>
            <td rowspan="2">
                <asp:Button runat="server" ID="dripToggleOn" Text=" on " OnClick="dripToggleOn_Click" Enabled="false" /> 
                <asp:Button runat="server" ID="dripToggleOff" Text=" off " OnClick="dripToggleOff_Click" Enabled="false" />
            </td>
        </tr>
        <tr>
            <td><asp:Label runat="server" ID="dripStatus" CssClass="smallNote" /></td>
        </tr>

    </table>
    </form>
</body>
</html>
