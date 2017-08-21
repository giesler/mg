<%@ Page Language="c#" Inherits="pics.Auth.ForgotPassword" CodeBehind="ForgotPassword.aspx.cs" %>

<%@ Register TagPrefix="picctls" Namespace="pics.Controls" Assembly="WebControlLibrary" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>msn2.net</title>
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
    <link rel="stylesheet" type="text/css" href="../msn2.css" />
    <link href="AuthStyles.css" type="text/css" rel="stylesheet" />
</head>
<body style="padding: 0px">
    <!-- top table with MSN2 logo -->
    <form runat="server" id="Login" method="post">
    <picctls:Header ID="header" runat="server" Size="small" Text="Pictures - Sign In">
    </picctls:Header>
    <table style="height: 100%" cellspacing="0" cellpadding="0" width="100%" align="left"
        border="0">
        <tr>
            <td height="3" class="msn2headerfade" colspan="3">
                <img height="3" src="../images/blank.gif" />
            </td>
        </tr>
        <tr>
            <td class="msn2sidebar" width="125">
                <picctls:Sidebar ID="Sidebar1" runat="server"></picctls:Sidebar>
            </td>
            <td class="msn2sidebarfade" width="4">
            </td>
            <td class="msn2contentwindow" valign="top">
                <!-- Main content -->
                <asp:Panel runat="server" ID="pnlConfirm">
                    <p>
                        You can easily reset your password.
                    </p>
                    <ul>
                        <li>Confirm the email address is correct below.
                            <li>Click 'Confirm'
                                <li>After clicking confirm you will be sent an email with a link you can click to set
                                    a new password. </li>
                    </ul>
                    <table class="loginTable" cellspacing="0" cellpadding="5">
                        <tr class="loginTableContent">
                            <td class="loginTableTitle">
                                <b>Email</b>
                            </td>
                        </tr>
                        <tr class="loginTableContent">
                            <td class="loginTableText">
                                <asp:TextBox ID="txtEmail" runat="server" Width="175px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="loginTableContent">
                            <td class="loginTableText" align="right">
                                <asp:Button ID="btnConfirm" Text="Confirm" runat="server" CssClass="btn" OnClick="btnConfirm_Click">
                                </asp:Button>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlBadLogin" Width="225" Visible="False" runat="server" Height="29px"
                    CssClass="errorPanel">
                    If you forgot your password, click
                    <asp:HyperLink ID="lnkForgotPassword" runat="server">here</asp:HyperLink>&nbsp;to
                    find out how to change your password.
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlSent" Visible="False">
                    <p>
                        An email has been sent to <b>
                            <asp:Label ID="lblEmail" runat="server"></asp:Label></b>. It will include a
                        link you can use to reset your password.&nbsp; You should receive it shortly.
                    </p>
                </asp:Panel>
                <!-- Begin footer -->
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
