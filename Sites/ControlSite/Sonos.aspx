<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sonos.aspx.cs" Inherits="Sonos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MSN2 Control</title>
    <link rel="SHORTCUT ICON" href="favicon.ico" />
    <meta name="viewport" content="width=device-width" />
    <link href="http://home.ms2n.net/Styles.css" rel="stylesheet" type="text/css" />
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
            <a href="http://www.ms2n.net/">msn2.net</a>: <a href="http://home.ms2n.net">home</a> | <a href="http://cams.ms2n.net/">cams</a> |  <a href="/" style="color:deepskyblue">control</a>
        </div>
        <div style="font-weight: bolder; padding: 6px; margin-top: 5px; margin-bottom: 5px">
            <a href="/?g=switches" style="padding: 4px">switches</a>&nbsp; | 
            &nbsp;<a href="/?g=doors" style="padding: 4px">doors</a>&nbsp; | 
            &nbsp;<a href="/?g=commands" style="padding: 4px">commands</a>&nbsp; | 
            &nbsp;<a href="Sonos.aspx" style="color: deepskyblue">sonos</a>
        </div>

        <asp:Panel runat="server">
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
            <asp:LinkButton runat="server" ID="rebootAll" Text="reboot all" OnClick="rebootAll_Click"  CssClass="sonosGrid" />
        </asp:Panel>
    </form>
</body>
</html>
