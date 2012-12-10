﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="pics.Controls.Mobile._default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>pics.msn2.net Home</title>
    <link rel="Stylesheet" href="mobile.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="title">
            Recent Pictures</div>
        <asp:Panel runat="server" ID="recent" />
        <hr />
        <div class="title">
            Browse Pictures</div>
        <table>
            <tr>
                <td align="center">
                    <a href="Categories.aspx?c=1">
                        <img src="../Images/folder40.gif" height="40" width="40" border="0" /></a>
                </td>
                <td>
                    <a href="Categories.aspx?c=1">All Categories</a>
                </td>
            </tr>
        </table>
        <hr />
        <div class="title">
            Random Picture</div>
        <table>
            <tr>
                <td>
                    <asp:HyperLink runat="server" ID="randomImageLink" />
                </td>
                <td>
                    <asp:HyperLink runat="server" ID="randomTitle" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
