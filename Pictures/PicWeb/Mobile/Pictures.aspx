﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Pictures.aspx.cs" Inherits="pics.Controls.Mobile.Pictures" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>msn2.net Pictures</title>
    <link rel="Stylesheet" href="mobile.css" />
</head>
<body style="margin: 0px 0px 0px 0px; padding: 0px 0px 0px 0px">
    <table style="height: 12px; width: 100%" cellpadding="0" cellspacing="0">
        <tr style="background-color: #008000">
            <td>
                <a href="default.aspx">
                    <img src="../images/msn2summer4.gif" border="0" height=20 />
                </a>
            </td>
        </tr>
    </table>
    <form id="form1" runat="server">
    <div class="title">
        <asp:Label runat="server" ID="categoryHeading"></asp:Label>
    </div>
    <div class="description">
        <asp:Label runat="server" ID="categoryDescription" />
    </div>
    <asp:Panel runat="server" ID="content" Width="100%" />
    <div runat="server" id="pager" class="description">
        <asp:Label runat="server" ID="pictureCount" />
        pics | Page
        <asp:DropDownList runat="server" ID="page" OnSelectedIndexChanged="OnPageChanged"
            AutoPostBack="true">
        </asp:DropDownList>
        of
        <asp:Label runat="server" ID="pagesCount"></asp:Label> | 
        <asp:DropDownList runat="server" ID="pageCount" AutoPostBack="true" OnSelectedIndexChanged="OnPageChanged" />
        pics per page.
    </div>
    <div class="footer">
        <hr />
        <a href="default.aspx">Home</a> |
        <asp:HyperLink runat="server" ID="categoryLink" NavigateUrl="javascript: history.go(-1);"
            Text="Back" />
    </div>
    </form>
</body>
</html>
