<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RandomThumbSite._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Random Thumb</title>
    <style>
        .propText
        {
            font-size: 7pt;
            color: Silver;
        }
        .picPanel
        {
            overflow: hidden;
            text-align: center;
        }
    </style>
</head>
<body style="font-family: Tahoma; font-size: small; margin: 0px 0px 0px 0px;" runat="server"
    id="bodyTag">
    <form id="form1" runat="server">
    <asp:Panel runat="server" ID="picPanel" CssClass="picPanel">
        <asp:HyperLink runat="server" ID="imageLink">
            <asp:Image runat="server" ID="image" />
        </asp:HyperLink><br />
        <asp:Panel runat="server" ID="details">
            <asp:Label runat="server" ID="titleLabel" CssClass="propText"></asp:Label><br />
            <asp:Label runat="server" ID="dateLabel" CssClass="propText"></asp:Label><br />
            <asp:Label runat="server" ID="categories" CssClass="propText"></asp:Label>
        </asp:Panel>
    </asp:Panel>
    </form>
</body>
</html>
