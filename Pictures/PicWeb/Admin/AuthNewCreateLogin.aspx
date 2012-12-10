<%@ Page Language="c#" Inherits="pics.Admin.AuthNewCreateLogin" Codebehind="AuthNewCreateLogin.aspx.cs" %>
<%@ Register TagPrefix="picctls" Namespace="pics.Controls" Assembly="PicWeb" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>msn2.net</title>
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link rel="stylesheet" type="text/css" href="../msn2.css">
</head>
<body leftmargin="0" topmargin="0">
    <!-- top table with MSN2 logo -->
    <form id="Login" method="post" runat="server">
        <picctls:Header id="header" runat="server" size="small" Text="Pictures - New Login">
        </picctls:Header>
        <table cellspacing="0" cellpadding="0" border="0" width="100%" align="left" height="100%">
            <tr>
                <td height="3" class="msn2headerfade" colspan="3">
                    <img src="../images/blank.gif" height="3"></td>
            </tr>
            <tr>
                <td class="msn2sidebar" width="125" valign="top">
                    <picctls:Sidebar id="Sidebar1" runat="server">
                    </picctls:Sidebar>
                </td>
                <td width="4" class="msn2sidebarfade">
                </td>
                <td class="msn2contentwindow" valign="top">
                    <!-- Main content -->
                    <asp:Panel ID="pnlNewLoginInfo" runat="server" Width="100%">
                        <table class="logintable" cellspacing="0" cellpadding="5">
                            <tr class="loginTableContent">
                                <td class="loginTableTitle" colspan="2">
                                    New Login
                                </td>
                            </tr>
                            <tr class="loginTableContent">
                                <td class="loginTableText">
                                    Name:
                                </td>
                                <td class="loginTableText">
                                    <asp:Label ID="lblName" Width="175px" runat="server"></asp:Label></td>
                            </tr>
                            <tr class="loginTableContent">
                                <td class="loginTableText">
                                    Email:
                                </td>
                                <td class="loginTableText">
                                    <asp:Label ID="lblEmail" Width="175px" runat="server"></asp:Label></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlNewUser" runat="server" Width="100%">
                        <p>
                            Enter the details of the new person here.
                        </p>
                        <table>
                            <tr>
                                <td>
                                    <table class="logintable" cellspacing="0" cellpadding="5">
                                        <tr class="loginTableContent">
                                            <td class="loginTableTitle" colspan="2">
                                                New Login
                                            </td>
                                        </tr>
                                        <tr class="loginTableContent">
                                            <td class="loginTableText">
                                                First Name:
                                                <asp:RequiredFieldValidator ID="Requiredfieldvalidator1" runat="server" Display="Dynamic"
                                                    ControlToValidate="txtFirstName" ErrorMessage="First name is required." CssClass="err">*</asp:RequiredFieldValidator></td>
                                            <td class="loginTableText">
                                                <asp:TextBox ID="txtFirstName" Width="175px" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr class="loginTableContent">
                                            <td class="loginTableText">
                                                Last Name:
                                                <asp:RequiredFieldValidator ID="Requiredfieldvalidator2" runat="server" Display="Dynamic"
                                                    ControlToValidate="txtLastName" ErrorMessage="Last name is required." CssClass="err">*</asp:RequiredFieldValidator></td>
                                            <td class="loginTableText">
                                                <asp:TextBox ID="txtLastName" Width="175px" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr class="loginTableContent">
                                            <td class="loginTableText">
                                                Full Name:
                                                <asp:RequiredFieldValidator ID="NameValidator" runat="server" Display="Dynamic" ControlToValidate="txtFullName"
                                                    ErrorMessage="Name is required!" CssClass="err">*</asp:RequiredFieldValidator></td>
                                            <td class="loginTableText">
                                                <asp:TextBox ID="txtFullName" Width="175px" runat="server"></asp:TextBox></td>
                                        </tr>
                                        <tr class="loginTableContent">
                                            <td class="loginTableText" align="right" colspan="2">
                                                <asp:Button ID="btnOK" Text=" OK " runat="server" CssClass="btn" OnClick="btnOK_Click">
                                                </asp:Button></td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <asp:ValidationSummary ID="ValidSummary" runat="server" CssClass="err" HeaderText="You must enter valid values for the following fields:"
                                        DisplayMode="BulletList"></asp:ValidationSummary>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlDone" runat="server" Visible="False">
                        <p>
                            The user above has been added. An email was sent telling them they can now use the
                            website.
                        </p>
                    </asp:Panel>
                    <!-- Begin footer -->
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
