<%@ Page Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="control" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>msn2 home control</title>
    <link href="Styles.css" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="width=device-width" />
    <script type="text/javascript">
        function toggleItem() {
            document.all['levelPanel'].style.visibility = 'hidden';
            document.all['sendingPanel'].style.visibility = 'visible';
        }
        function toggleLevel(itemName) {
            document.all['levelPanel'].style.visibility = 'visible';
            document.all['levelItem'].value = itemName;
            return false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" style="height: 100%">
        <asp:Panel runat="server" ID="sendingPanel" class="popupPanel">
            <br />
            <br />
            <br />
            <p>sending...</p>
        </asp:Panel>
        <asp:Panel runat="server" ID="levelPanel" class="popupPanel">
            <asp:Button runat="server" ID="level100" OnClick="level100_Click" Text=" 100% " CssClass="popupButton" OnClientClick="toggleItem();" />
            <br />
            <asp:Button runat="server" ID="level90" OnClick="level100_Click" Text=" 90% " CssClass="popupButton" OnClientClick="toggleItem();" />
            <br />
            <asp:Button runat="server" ID="level80" OnClick="level100_Click" Text=" 80% " CssClass="popupButton" OnClientClick="toggleItem();" />
            <br />
            <asp:Button runat="server" ID="level70" OnClick="level100_Click" Text=" 70% " CssClass="popupButton" OnClientClick="toggleItem();" />
            <br />
            <asp:Button runat="server" ID="level60" OnClick="level100_Click" Text=" 60% " CssClass="popupButton" OnClientClick="toggleItem();" />
            <br />
            <asp:Button runat="server" ID="level50" OnClick="level100_Click" Text=" 50% " CssClass="popupButton" OnClientClick="toggleItem();" />
            <br />
            <asp:Button runat="server" ID="level40" OnClick="level100_Click" Text=" 40% " CssClass="popupButton" OnClientClick="toggleItem();" />
            <br />
            <asp:Button runat="server" ID="level30" OnClick="level100_Click" Text=" 30% " CssClass="popupButton" OnClientClick="toggleItem();" />
            <br />
            <asp:Button runat="server" ID="level20" OnClick="level100_Click" Text=" 20% " CssClass="popupButton" OnClientClick="toggleItem();" />
            <br />
            <asp:Button runat="server" ID="level10" OnClick="level100_Click" Text=" 10% " CssClass="popupButton" OnClientClick="toggleItem();" />
            <asp:HiddenField runat="server" ID="levelItem" />
        </asp:Panel>
        <div style="background: black; color: white; padding: 6px; font-weight: bold" class="headerLink">
            <a href="http://www.msn2.net/">MSN2.NET</a>: <a href="http://home.msn2.net">HOME</a> | <a href="http://cams.msn2.net/">CAMS</a> |  <a href="http://control.msn2.net/">CONTROL</a> | 
	    <a href="http://ts.msn2.net/">TS</a>
            <br />
        </div>
        <table style="width: 100%">
            <tr>
                <td class="mainItem">garage 1</td>
                <td rowspan="2">
                    <asp:Button runat="server" ID="toggleGarage1" Text="toggle" OnClick="toggleGarage1_Click" OnClientClick="javascript:toggleItem();" CssClass="toggleButton" /></td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="garage1Status" CssClass="smallNote" /></td>
            </tr>

            <tr style="border-top: solid 2px silver">
                <td class="mainItem">garage 2</td>
                <td rowspan="2">
                    <asp:Button runat="server" ID="toggleGarage2" Text="toggle" OnClick="toggleGarage2_Click" OnClientClick="javascript:toggleItem();" CssClass="toggleButton" /></td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="garage2Status" CssClass="smallNote" /></td>
            </tr>

            <tr style="border-top: solid 2px silver">
                <td class="mainItem">media room</td>
                <td rowspan="2">
                    <asp:Button runat="server" ID="mediaRoomOn" Text=" on " OnClick="mediaRoomOn_Click" OnClientClick="javascript:toggleItem();" CssClass="onOffButton" />
                    <asp:Button runat="server" ID="mediaRoomOff" Text=" off " OnClick="mediaRoomOff_Click" OnClientClick="javascript:toggleItem();" CssClass="onOffButton" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="mediaRoomStatus" CssClass="smallNote" /></td>
            </tr>

            <tr style="border-top: solid 2px silver">
                <td class="mainItem">upstairs hall</td>
                <td rowspan="2">
                    <asp:Button runat="server" ID="upstairsHallOn" Text=" on " OnClick="upstairsHallOn_Click" OnClientClick="javascript:toggleItem();" CssClass="onOffButton" />
                    <asp:Button runat="server" ID="upstairsHallOff" Text=" off " OnClick="upstairsHallOff_Click" OnClientClick="javascript:toggleItem();" CssClass="onOffButton" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="upstairsHallStatus" CssClass="smallNote" /></td>
            </tr>
            <tr style="border-top: solid 2px silver">
                <td class="mainItem">master sink light</td>
                <td rowspan="2">
                    <asp:Button runat="server" ID="masterSinkLightOn" OnClientClick="return toggleLevel('master sink light');" Text=" on " CssClass="onOffButton" />
                    <asp:Button runat="server" ID="masterSinkLightOff" Text=" off " OnClick="masterSinkLightOff_Click" OnClientClick="javascript:toggleItem();" CssClass="onOffButton" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="masterSinkLightStatus" CssClass="smallNote" /></td>
            </tr>
            <tr style="border-top: solid 2px silver">
                <td class="mainItem">master bath fan</td>
                <td rowspan="2">
                    <asp:Button runat="server" ID="masterBathFanOn" Text=" on " OnClick="masterBathFanOn_Click" OnClientClick="javascript:toggleItem();" CssClass="onOffButton" />
                    <asp:Button runat="server" ID="masterBathFanOff" Text=" off " OnClick="masterBathFanOff_Click" OnClientClick="javascript:toggleItem();" CssClass="onOffButton" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" ID="masterBathFanStatus" CssClass="smallNote" /></td>
            </tr>
            <!--
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
         -->
        </table>
    </form>
</body>
</html>
