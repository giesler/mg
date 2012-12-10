<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="pics.Controls.Mobile.Search" %>

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
                    <img src="../images/mobileheadtitle.gif" border="0" />
                </a>
            </td>
        </tr>
    </table>
    <form id="form1" runat="server">
    <div class="title">
        Date
    </div>
    <div>
        <table>
            <tr>
                <td>
                    From:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="fromMonth" AutoPostBack="true" OnSelectedIndexChanged="OnFromMonthChanged" />
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="fromDay" />
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="fromYear" AutoPostBack="true" OnSelectedIndexChanged="OnFromYearChanged" />
                </td>
            </tr>
            <tr>
                <td>
                    To:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="toMonth" AutoPostBack="true" OnSelectedIndexChanged="OnToMonthChanged" />
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="toDay" />
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="toYear" AutoPostBack="true" OnSelectedIndexChanged="OnToYearChanged" />
                </td>
            </tr>
        </table>
    </div>
    <asp:LinkButton runat="server" ID="search" OnClick="OnSearchClick" Text="Search >>" />
    </form>
    <div class="footer">
        <hr />
        <a href="default.aspx">Home</a> |
        <asp:HyperLink runat="server" ID="categoryLink" NavigateUrl="javascript: history.go(-1);"
            Text="Back" />
    </div>
</body>
</html>
