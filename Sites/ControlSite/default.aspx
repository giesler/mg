<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MS2N Control</title>
    <link href="http://home.ms2n.net/Styles.css" rel="stylesheet" type="text/css" />
    <link rel="SHORTCUT ICON" href="favicon.ico" />
    <meta name="viewport" content="width=device-width" /></head>
    <style>
        A:link {
            color: white;
        }
        a:visited {
            color: white;
        }
        .itemGroup {
            margin: 5px;
        }
    </style>
<body style="background-color: black;">
    <form id="form1" runat="server">
        <div class="headerLink" style="padding: 4px">
            <a href="http://www.ms2n.net/">ms2n.net</a>: <a href="http://home.ms2n.net">home</a> | <a href="http://cams.ms2n.net/">cams</a> |  <a href="/" style="color:deepskyblue">control</a>
        </div>
        <div style="width:100%; padding: 6px">
            <asp:DropDownList ID="location2" runat="server" AutoPostBack="true" /> <asp:DropDownList ID="location1" runat="server" AutoPostBack="true" />
        </div>
        <div>
            <asp:ListView ID="items" runat="server">
                <ItemTemplate>
                    <div style="width: 100%; padding: 6px">
                        <asp:LinkButton runat="server" ID="controlLink" Text='<%# Eval("Name")%>' ToolTip='<%# Eval("Device_Type_String") %>' OnClick="controlLink_Click" CommandArgument='<%# Eval("Ref") %>' />
                        <asp:Label runat="server" ID="lbl"  /> - <asp:Label runat="server" ID="status" Text='<%# Eval("Status") %>' />
                    </div>
                </ItemTemplate>                
            </asp:ListView>
        </div>
    </form>
</body>
</html>
