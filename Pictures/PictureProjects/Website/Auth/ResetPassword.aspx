<%@ Page Language="c#" Inherits="pics.Auth.ResetPassword" CodeBehind="ResetPassword.aspx.cs" %>

<%@ Register TagPrefix="picctls" Namespace="pics.Controls" Assembly="WebControlLibrary" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>msn2.net</title>
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
    <link rel="stylesheet" type="text/css" href="../msn2.css" />
    <link href="AuthStyles.css" type="text/css" rel="stylesheet" />
</head>
<body style="padding: 2px">
    <!-- top table with MSN2 logo -->
    <form runat="server" id="Login" method="post">
    <picctls:Header ID="header" runat="server" Size="small" Text="Pictures - New Login">
    </picctls:Header>
    <table cellspacing="0" cellpadding="0" border="0" width="100%" align="left" style="height: 100%">
        <tr>
            <td height="3" class="msn2headerfade" colspan="3">
                <img src="../images/blank.gif" height="3" />
            </td>
        </tr>
        <tr>
            <td class="msn2sidebar" width="125" valign="top">
                <picctls:Sidebar ID="Sidebar1" runat="server"></picctls:Sidebar>
            </td>
            <td width="4" class="msn2sidebarfade">
            </td>
            <td class="msn2contentwindow" valign="top">
                <!-- Main content -->
                <asp:Panel runat="server" ID="pnlPassword">
                    <p>
                        You can reset your password by entering your new password below.
                    </p>
                    <asp:Label ID="lblError" runat="server" CssClass="err"></asp:Label>
                    <table class="logintable" cellspacing="0" cellpadding="5">
                        <tr>
                            <td class="loginTableTitle" colspan="2">
                                <b>Reset Password</b>
                            </td>
                            Mike Giesler - Core Technical Competencies + Big Challenges - Dealing with Ambiguity,
                            Creativity
                        </tr>
                        <tr class="loginTableContent">
                            <td class="loginTableText">
                                Email:
                            </td>
                            <td class="loginTableText">
                                <asp:Label ID="lblEmail" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr class="loginTableContent">
                            <td class="loginTableText">
                                New password:
                            </td>
                            <td class="loginTableText">
                                <asp:TextBox ID="txtNewPassword" runat="server" Width="175px" TextMode="Password"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="loginTableContent">
                            <td class="loginTableText">
                                Confirm new password:
                            </td>
                            <td class="loginTableText">
                                <asp:TextBox ID="txtConfirmNewPassword" runat="server" Width="175px" TextMode="Password"></asp:TextBox>
                            </td>
                        </tr>
                        <tr class="loginTableContent">
                            <td class="loginTableText" align="right" colspan="2">
                                <asp:Button ID="btnOK" Text=" OK " runat="server" CssClass="btn" Width="100px" OnClick="btnOK_Click">
                                </asp:Button>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlChanged" Visible="False">
                    <p>
                        Your password has been changed.
                    </p>
                    <p>
                        To login, click
                        <asp:HyperLink ID="loginLink" runat="server" Target="_top">here</asp:HyperLink></p>
                </asp:Panel>
                <!-- Begin footer -->
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
