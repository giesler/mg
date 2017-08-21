<%@ Register TagPrefix="picctls" Namespace="pics.Controls" Assembly="PicWeb" %>

<%@ Page Language="c#" Inherits="pics.Auth.Logout" Codebehind="Logout.aspx.cs" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>msn2.net</title>
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link rel="stylesheet" type="text/css" href="../msn2.css">
</head>
<body topmargin="0" leftmargin="0">
    <!-- top table with MSN2 logo -->
    <form runat="server" id="Login" method="post">
        <picctls:Header ID="header" runat="server" Size="small" Text="Pictures - Signed Out">
        </picctls:Header>
        <table cellspacing="0" cellpadding="0" border="0" width="100%" align="left" height="100%">
            <tr>
                <td height="3" class="msn2headerfade" colspan="3">
                    <img src="../images/blank.gif" height="3"></td>
            </tr>
            <tr>
                <td class="msn2sidebar" width="125" valign="top">
                    <picctls:Sidebar ID="Sidebar1" runat="server"></picctls:Sidebar>
                </td>
                <td width="4" class="msn2sidebarfade">
                </td>
                <td class="msn2contentwindow" valign="top">
                    <!-- Main content -->
                    <blockquote>
                        <p>
                        </p>
                        <p>
                        </p>
                        <p>
                            <asp:Panel runat="server" ID="confirmSignout">
                                Are you sure you want to sign out?
                                <br />
                                <br />
                                <asp:LinkButton ID="signoutLink" runat="server" OnClick="signoutLink_Click">Sign out now.</asp:LinkButton>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="signedOutPanel" Visible="false">
                                You have been signed out.
                                <br />
                                <br />
                                To sign in again, click <a href="../">here</a>.
                            </asp:Panel>
                        </p>
                    </blockquote>
                    <!-- Begin footer -->
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
