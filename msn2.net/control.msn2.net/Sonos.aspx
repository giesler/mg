<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sonos.aspx.cs" Inherits="Sonos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MSN2 Control</title>
    <link href="http://home.msn2.net/Styles.css" rel="stylesheet" type="text/css" />
    <link rel="SHORTCUT ICON" href="favicon.ico" />
    <meta name="viewport" content="width=device-width" />
</head>
<body style="background-color: black;">
    <form id="form1" runat="server">
        <div class="headerLink">
            <a href="http://www.msn2.net/">MSN2.NET</a>: <a href="http://home.msn2.net">HOME</a> | <a href="http://cams.msn2.net/">CAMS</a> |  <a href="http://control.msn2.net/">CONTROL</a>
            <br />
        </div>
        <asp:Panel runat="server">
            <asp:GridView runat="server" ID="players" ItemType="SonosService.ZonePlayerStatus" DataKeyNames="Name" AutoGenerateColumns="false">
                <Columns>
                    <asp:HyperLinkField DataNavigateUrlFields="Location" DataTextField="Name" />
                    <asp:DynamicField DataField="GroupNumber" />
                    <asp:DynamicField DataField="Coordinator" />
                    <asp:DynamicField DataField="WifiEnabled" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" NavigateUrl="<%# GetUrl(Item.IpAddress) %>" Text="reboot" Target="_blank" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:Panel>
    </form>
</body>
</html>
