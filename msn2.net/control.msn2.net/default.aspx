<%@ Page Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="control" EnableViewState="false" EnableSessionState="False" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MSN2 Control</title>
    <link href="http://home.msn2.net/Styles.css" rel="stylesheet" type="text/css" />
    <link rel="SHORTCUT ICON" href="favicon.ico" />
    <meta name="viewport" content="width=device-width" />
    <script type="text/javascript">
        function toggleItem() {
            document.all['levelPanel'].style.visibility = 'hidden';
            document.all['durationPanel'].style.visibility = 'hidden';
            document.all['sendingPanel'].style.visibility = 'visible';
        }
        function toggleLevel(itemName) {
            document.all['levelPanel'].style.visibility = 'visible';
            document.all['levelItem'].value = itemName;
            return false;
        }
        function cancelLevel() {
            document.all['levelPanel'].style.visibility = 'hidden';
            return false;
        }
        function toggleDuration() {
            document.all['durationPanel'].style.visibility = 'visible';
            return false;
        }
        function cancelDuration() {
            document.all['durationPanel'].style.visibility = 'hidden';
            return false;
        }
        function cancelDrip() {
            document.all['durationPanel'].style.visibility = 'hidden';
            return false;
        }
        function toggleGroup(groupName) {

            document.all['lights'].style.display = 'none';
            document.all['doors'].style.display = 'none';
            document.all['commands'].style.display = 'none';

            document.all[groupName].style.display = 'inline';
        }
    </script>
    <style>
        A:link {
            text-decoration: none;
            color: white;
        }
        a:visited {
            text-decoration: none;
            color: white;
        }
        itemGroup {
            margin: 5px;
            padding: 5px;
        }
    </style>
</head>
<body style="background-color: black;" onload="toggleGroup('lights')">
    <form id="form1" runat="server" style="height: 100%">
        <asp:Panel runat="server" ID="sendingPanel" CssClass="popupPanel">
            <br />
            <br />
            <br />
            <br />
            <p>sending...</p>
        </asp:Panel>
        <asp:Panel runat="server" ID="levelPanel" CssClass="popupPanel">
            <br />
            <br />
            <br />
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
            <br />
            <asp:Button runat="server" ID="levelCancel" Text=" cancel " CssClass="popupButton" OnClientClick="return cancelLevel();" />
            <asp:HiddenField runat="server" ID="levelItem" />
        </asp:Panel>
        <asp:Panel runat="server" ID="durationPanel" CssClass="popupPanel">
            <br />
            <br />
            <br />
            <asp:Button runat="server" ID="duration1" OnClick="duration1_Click" Text=" 1 min " CssClass="popupButton" OnClientClick="toggleItem();" />
            <br />
            <asp:Button runat="server" ID="duration5" OnClick="duration1_Click" Text=" 5 mins " CssClass="popupButton" OnClientClick="toggleItem();" />
            <br />
            <asp:Button runat="server" ID="duration10" OnClick="duration1_Click" Text=" 10 mins " CssClass="popupButton" OnClientClick="toggleItem();" />
            <br />
            <asp:Button runat="server" ID="duration15" OnClick="duration1_Click" Text=" 15 mins " CssClass="popupButton" OnClientClick="toggleItem();" />
            <br />
            <asp:Button runat="server" ID="Button1" Text=" cancel " CssClass="popupButton" OnClientClick="return cancelDrip();" />
        </asp:Panel>
        <div class="headerLink">
            <a href="http://www.msn2.net/">MSN2.NET</a>: <a href="http://home.msn2.net">HOME</a> | <a href="http://cams.msn2.net/">CAMS</a> |  <a href="http://control.msn2.net/">CONTROL</a> 
            <br />
        </div>

        <div style="font-weight: bolder; padding: 5px">
            <a href="javascript:toggleGroup('lights')">SWITCHES</a> | 
            <a href="javascript:toggleGroup('doors')">DOORS</a> | 
            <a href="javascript:toggleGroup('commands')">COMMANDS</a> | 
            <a href="Sonos.aspx">SONOS</a>
        </div>
        
        <div id="lights">

        <div class="itemGroup" style="padding: 5px">
            <div style="float: right; margin-top: 10px">
                <asp:Button runat="server" ID="livingRoomOn" Text=" on " OnClick="livingRoomOn_Click" OnClientClick="javascript:toggleItem();" CssClass="onOffButton" />
                <asp:Button runat="server" ID="livingRoomOff" Text=" off " OnClick="livingRoomOff_Click" OnClientClick="javascript:toggleItem();" CssClass="onOffButton" />
            </div>
            <div class="mainItem">living room</div>
            <div><asp:Label runat="server" ID="livingRoomStatus" CssClass="smallNote" /></div>
        </div>

        <div class="itemGroup" style="padding: 5px">
            <div style="float: right; margin-top: 10px">
                <asp:Button runat="server" ID="mediaRoomOn" Text=" on " OnClick="mediaRoomOn_Click" OnClientClick="javascript:toggleItem();" CssClass="onOffButton" />
                <asp:Button runat="server" ID="mediaRoomOff" Text=" off " OnClick="mediaRoomOff_Click" OnClientClick="javascript:toggleItem();" CssClass="onOffButton" />
            </div>
            <div class="mainItem">media room</div>
            <div><asp:Label runat="server" ID="mediaRoomStatus" CssClass="smallNote" /></div>
        </div>

        <div class="itemGroup" style="padding: 5px">
            <div style="float: right; margin-top: 10px">
                <asp:Button runat="server" ID="upstairsHallOn" Text=" on " OnClientClick="return toggleLevel('upstairs hall light');" CssClass="onOffButton" />
                <asp:Button runat="server" ID="upstairsHallOff" Text=" off " OnClick="upstairsHallOff_Click" OnClientClick="javascript:toggleItem();" CssClass="onOffButton" />
            </div>
            <div class="mainItem">upstairs hall</div>
            <div><asp:Label runat="server" ID="upstairsHallStatus" CssClass="smallNote" /></div>
        </div>

        <div class="itemGroup" style="padding: 5px">
            <div style="float: right; margin-top: 10px">
                <asp:Button runat="server" ID="kitchenOn" Text=" on " OnClientClick="return toggleLevel('kitchen light');" CssClass="onOffButton" />
                <asp:Button runat="server" ID="kitchenOff" Text=" off " OnClick="kitchenOff_Click" OnClientClick="javascript:toggleItem();" CssClass="onOffButton" />
            </div>
            <div class="mainItem">kitchen</div>
            <div><asp:Label runat="server" ID="kitchenStatus" CssClass="smallNote" /></div>
        </div>

        <div class="itemGroup" style="padding: 5px">
            <div style="float: right; margin-top: 10px">
                <asp:Button runat="server" ID="masterSinkLightOn" OnClientClick="return toggleLevel('master sink light');" Text=" on " CssClass="onOffButton" />
                <asp:Button runat="server" ID="masterSinkLightOff" Text=" off " OnClick="masterSinkLightOff_Click" OnClientClick="javascript:toggleItem();" CssClass="onOffButton" />
            </div>
            <div class="mainItem">master sink light</div>
            <div><asp:Label runat="server" ID="masterSinkLightStatus" CssClass="smallNote" /></div>
        </div>

        <div class="itemGroup" style="padding: 5px">
            <div style="float: right; margin-top: 10px">
                <asp:Button runat="server" ID="masterBathFanOn" Text=" on " OnClick="masterBathFanOn_Click" OnClientClick="javascript:toggleItem();" CssClass="onOffButton" />
                <asp:Button runat="server" ID="masterBathFanOff" Text=" off " OnClick="masterBathFanOff_Click" OnClientClick="javascript:toggleItem();" CssClass="onOffButton" />
            </div>
            <div class="mainItem">master bath fan</div>
            <div><asp:Label runat="server" ID="masterBathFanStatus" CssClass="smallNote" /></div>
        </div>

        <div class="itemGroup" style="padding: 5px">
            <div style="float: right; margin-top: 10px">
                <asp:Button runat="server" ID="neilsRoomLightOn" OnClientClick="return toggleLevel('neils room lights');" Text=" on " CssClass="onOffButton" />
                <asp:Button runat="server" ID="neilsRoomLightOff" Text=" off " OnClick="neilsRoomLightOff_Click" OnClientClick="javascript:toggleItem();" CssClass="onOffButton" />
            </div>
            <div class="mainItem">neils room lights</div>
            <div><asp:Label runat="server" ID="neilsRoomLightStatus" CssClass="smallNote" /></div>
        </div>

        </div>

        <div id="doors">

        <div class="itemGroup" style="padding: 5px">
            <div style="float: right; margin-top: 10px">
                <asp:Button runat="server" ID="toggleGarage1" Text="toggle" OnClick="toggleGarage1_Click" OnClientClick="javascript:toggleItem();" CssClass="toggleButton" />
            </div>
            <div class="mainItem">garage 1</div>
            <div><asp:Label runat="server" ID="garage1Status" CssClass="smallNote" /></div>
        </div>

        <div class="itemGroup" style="padding: 5px">
            <div style="float: right; margin-top: 10px">
                <asp:Button runat="server" ID="toggleGarage2" Text="toggle" OnClick="toggleGarage2_Click" OnClientClick="javascript:toggleItem();" CssClass="toggleButton" />
            </div>
            <div class="mainItem">garage 2</div>
            <div><asp:Label runat="server" ID="garage2Status" CssClass="smallNote" /></div>
        </div>

        <div class="itemGroup" style="padding: 5px">
            <div style="float: right; margin-top: 10px">
                <asp:Button runat="server" ID="garageEntryLock" Text=" lock " OnClick="garageEntryLock_Click" OnClientClick="javascript:toggleItem();" CssClass="onOffButton" />
                <asp:Button runat="server" ID="garageEntryUnlock" Text=" unlock " OnClick="garageEntryUnlock_Click" OnClientClick="javascript:toggleItem();" CssClass="onOffButton" />
            </div>
            <div class="mainItem">garage entry door</div>
            <div><asp:Label runat="server" ID="garageEntryStatus" CssClass="smallNote" /></div>
        </div>

        <div class="itemGroup" style="padding: 5px">
            <div style="float: right; margin-top: 10px">
                <asp:Button runat="server" ID="frontDoorLock" Text=" lock " OnClick="frontDoorLock_Click" OnClientClick="javascript:toggleItem();" CssClass="onOffButton" />
                <asp:Button runat="server" ID="frontDoorUnlock" Text=" unlock " OnClick="frontDoorUnlock_Click" OnClientClick="javascript:toggleItem();" CssClass="onOffButton" />
            </div>
            <div class="mainItem">front door</div>
            <div><asp:Label runat="server" ID="frontDoorStatus" CssClass="smallNote" /></div>
        </div>

        <div class="itemGroup" style="padding: 5px">
            <div style="float: right; margin-top: 10px">
            </div>
            <div class="mainItem">coop door</div>
            <div><asp:Label runat="server" ID="coopDoorStatus" CssClass="smallNote" /></div>
        </div>
        </div>

        <div id="commands">

        <div class="itemGroup" style="padding: 5px">
            <div style="float: right; margin-top: 10px">
                <asp:Button runat="server" ID="gardenDripOn" OnClientClick="return toggleDuration('garden drip');" Text=" on " CssClass="onOffButton" />
                <asp:Button runat="server" ID="gardenDripOff" Text=" off " OnClick="gardenDripOff_Click" OnClientClick="javascript:toggleItem();" CssClass="onOffButton" />
            </div>
            <div class="mainItem">garden drip</div>
            <div><asp:Label runat="server" ID="gardenDripStatus" CssClass="smallNote" /></div>
        </div>


        <div class="itemGroup" style="padding: 5px">
            <div style="float: right; margin-top: 10px">
                <asp:Button runat="server" ID="mainRoomsAudioMute" Text=" on " OnClick="mainRoomsAudioMute_Click" OnClientClick="javascript:toggleItem();" CssClass="onOffButton" />
                <asp:Button runat="server" ID="mainRoomsAudioUnmute" Text=" off " OnClick="mainRoomsAudioUnmute_Click" OnClientClick="javascript:toggleItem();" CssClass="onOffButton" />
            </div>
            <div class="mainItem">napping</div>
            <div>&nbsp;</div>
        </div>

        <div class="itemGroup" style="padding: 5px">
            <div style="float: right; margin-top: 10px">
                <asp:Button runat="server" ID="tvAudioOn" Text=" on " OnClick="tvAudioOn_Click" OnClientClick="javascript:toggleItem();" CssClass="onOffButton" />
                <asp:Button runat="server" ID="tvAudioOff" Text=" off " OnClick="tvAudioOff_Click" OnClientClick="javascript:toggleItem();" CssClass="onOffButton" />
            </div>
            <div class="mainItem">tv audio</div>
            <div>&nbsp;</div>
        </div>
        </div>       

    </form>
</body>
</html>
