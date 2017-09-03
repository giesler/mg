<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sonos.aspx.cs" Inherits="Sonos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MSN2 Control</title>
    <link rel="SHORTCUT ICON" href="favicon.ico" />
    <meta name="viewport" content="width=device-width" />
    <link href="http://home.msn2.net/Styles.css" rel="stylesheet" type="text/css" />
    <style>       
        A:link {
            text-decoration: none;
            color: white;
        }

        a:visited {
            text-decoration: none;
            color: white;
        }
        .sonosGrid {
            padding: 6px;
            margin-left: 4px;
        }
    </style>
</head>
<body style="background-color: black;">
    <form id="form1" runat="server">
        <div class="headerLink" style="padding: 4px">
            <a href="http://www.msn2.net/">msn2.net</a>: <a href="http://home.msn2.net">home</a> | <a href="http://cams.msn2.net/">cams</a> |  <a href="http://control.msn2.net/">control</a>
        </div>
        <asp:Panel runat="server">
            <div style="font-size: larger; font-weight: bold; padding:10px">sonos devices</div>
            <asp:GridView runat="server" ID="players" ItemType="SonosService.ZonePlayerStatus" DataKeyNames="Name" AutoGenerateColumns="false" CellPadding="7" BorderWidth="0" GridLines="None" OnDataBound="players_DataBound" CssClass="sonosGrid">
                <Columns>
                    <asp:DynamicField HeaderText="group" DataField="GroupNumber" ItemStyle-HorizontalAlign="Center" />
                    <asp:HyperLinkField HeaderText="name" DataNavigateUrlFields="Location" DataTextField="Name" HeaderStyle-HorizontalAlign="Left" Target="_blank" />
                    <asp:TemplateField HeaderText="settings" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label runat="server" Text="<%# GetCaps(Item) %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="actions">
                        <ItemTemplate>
<!--                            <asp:DropDownList runat="server" AutoPostBack="true" ID="action" OnSelectedIndexChanged="action_SelectedIndexChanged">
                                <asp:ListItem Text="review" />
                                <asp:ListItem Text="support" />
                                <asp:ListItem Text="reboot" />
                            </asp:DropDownList>-->
                            <asp:HyperLink runat="server" NavigateUrl="<%# GetSupportUrl(Item.IpAddress) %>" Text="review" Target="_blank" /> | 
                            <asp:HyperLink runat="server" NavigateUrl="<%# GetTopoUrl(Item.IpAddress) %>" Text="topo" Target="_blank" /> |
                            <asp:HyperLink runat="server" NavigateUrl="<%# GetRebootUrl(Item.IpAddress) %>" Text="reboot" Target="_blank" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:Button runat="server" ID="rebootAll" Text="REBOOT ALL" OnClick="rebootAll_Click" />
        </asp:Panel>
    </form>
</body>
</html>
