﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="RandomThumbSite._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Random Thumb</title>
    <style>
        .propText
        {
        	font-size: 7pt;
        	color: Silver;
        }
    </style>
</head>
<body style="font-family: Tahoma; font-size: small; margin: 0px 0px 0px 0px; background-color: #5D646D;">
    <form id="form1" runat="server">
    <div style="height: 175px; width: 130px; overflow: hidden; text-align: center;">
        <asp:HyperLink runat="server" ID="imageLink">
            <asp:Image runat="server" ID="image" />
        </asp:HyperLink>
        <asp:Label runat="server" ID="titleLabel" CssClass="propText"></asp:Label><br />
        <asp:Label runat="server" ID="dateLabel" CssClass="propText"></asp:Label><br />
        <asp:Label runat="server" ID="categories" CssClass="propText"></asp:Label>
     </div>
    </form>
</body>
</html>
