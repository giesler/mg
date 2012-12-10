﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Picture.aspx.cs" Inherits="pics.Controls.Mobile.PicturePage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>pics.msn2.net</title>
    <link rel="Stylesheet" href="mobile.css" />
</head>
<body style="margin: 0px 0px 0px 0px; padding: 0px 0px 0px 0px">
    <form id="form1" runat="server">
    <div style="padding: 0px">
        <asp:Panel runat="server" ID="content" />
    </div>
    <div class="footer">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td>
                    <a href="default.aspx">Home</a> |
                    <asp:HyperLink runat="server" ID="detailsLink" Text="Details" />
                    |
                    <asp:HyperLink runat="server" ID="categoryLink" NavigateUrl="javascript: history.go(-1);"
                        Text="Back" />
                    <asp:LinkButton runat="server" ID="nextRandom" Text="Next..." OnClick="nextRandom_Click" />
                </td>
                <td align="right">
                    <asp:DropDownList runat="server" ID="size" AutoPostBack="true" OnSelectedIndexChanged="size_SelectedIndexChanged">
                        <asp:ListItem Text="Small" />
                        <asp:ListItem Text="Medium" Selected="True" />
                        <asp:ListItem Text="Large" />
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>