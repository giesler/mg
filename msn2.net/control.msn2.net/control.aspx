<%@ Page Language="C#" AutoEventWireup="true" CodeFile="control.aspx.cs" Inherits="control" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MS2N Control</title>
    <link href="http://home.msn2.net/Styles.css" rel="stylesheet" type="text/css" />
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
        <div style="padding: 6px">
            &nbsp;<asp:HyperLink runat="server" ID="back" NavigateUrl="/" Text="&lt; Back to devices" /><br /> <br />
            &nbsp;<asp:Label runat="server" ID="name" /> - <asp:Label runat="server" ID="status" /><br />

            <asp:ListView ID="items" runat="server">
                <ItemTemplate>
                    <div style="width: 100%; padding: 6px">
                        <asp:LinkButton runat="server" ID="controlLink" Text='<%# Eval("Label")%>' ToolTip='<%# Eval("ControlValue") %>' CommandArgument='<%# Eval("ControlValue") %>' OnClick="controlLink_Click" />
                    </div>
                </ItemTemplate>                
            </asp:ListView>

        </div>
    </form>
</body>
</html>
