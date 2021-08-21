<%@ Page Language="C#" AutoEventWireup="true" CodeFile="thumbs.aspx.cs" Inherits="thumbs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>cams.msn2.net</title>
    <link href="Styles.css" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="width=device-width" /> 
</head>
<body>
    <form id="form1" runat="server">
        <asp:Repeater runat="server" ID="thumbsView" OnItemDataBound="thumbs_ItemDataBound">
            <ItemTemplate>
                <asp:HyperLink runat="server" ID="thumbALink" CssClass="smallthumb">
                    <asp:Image runat="server" ID="thumbAImage" Height="64" BorderWidth="0" CssClass="smallthumb" />
                    <asp:Image runat="server" ID="thumbBImage" Height="64" BorderWidth="0" CssClass="smallthumb" />
                </asp:HyperLink>
            </ItemTemplate>
        </asp:Repeater>
    </form>
</body>
</html>
