<%@ Page Language="c#" Inherits="pics.Auth.NewLogin" CodeBehind="NewLogin.aspx.cs" %>

<%@ Register TagPrefix="picctls" Namespace="pics.Controls" Assembly="WebControlLibrary" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>msn2.net</title>
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <link href="../msn2.css" type="text/css" rel="stylesheet">
    <link href="AuthStyles.css" type="text/css" rel="stylesheet">

    <script language="javascript">
			function setLoginFocus() { 
				if (document.all['txtName'])
					document.all['txtName'].focus();
				if (document.all['txtLookupEmail'])
					document.all['txtLookupEmail'].focus();
			}
    </script>

</head>
<body leftmargin="0" topmargin="0" onload="setLoginFocus();">
    <!-- top table with MSN2 logo -->
    <form id="Login" method="post" runat="server">
    <picctls:Header ID="header" runat="server" Size="small" Text="Pictures - New Login">
    </picctls:Header>
    <table cellspacing="0" cellpadding="0" border="0" width="100%" align="left" height="100%">
        <tr>
            <td height="3" class="msn2headerfade" colspan="3">
                <img src="../images/blank.gif" height="3">
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
                <asp:Panel ID="pnlEmailLookup" runat="server" Width="100%">
                    <p>
                        Please enter your email address below. We may have already entered your name and
                        email address in the picture system, and you'll be able to simply pick a password.
                    </p>
                    <table>
                        <tr>
                            <td>
                                <table class="logintable" cellspacing="0" cellpadding="5">
                                    <tr>
                                        <td class="loginTableTitle" colspan="2">
                                            New Login
                                        </td>
                                    </tr>
                                    <tr class="loginTableContent">
                                        <td class="loginTableText">
                                            Email:
                                            <asp:RequiredFieldValidator ID="Requiredfieldvalidator2" runat="server" Display="Dynamic"
                                                ControlToValidate="txtLookupEmail" ErrorMessage="Email is required!" CssClass="err">*</asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="Regularexpressionvalidator1" runat="server" Display="Static"
                                                ControlToValidate="txtLookupEmail" ErrorMessage="Email is not a valid email address.<br />Must follow name@host.domain format."
                                                ValidationExpression="^[\w-\.]+@[\w-]+\.(com|net|org|edu|mil|us|tv|\w)$" Font-Name="Arial"
                                                Font-Size="11">*</asp:RegularExpressionValidator>
                                        </td>
                                        <td class="loginTableText">
                                            <asp:TextBox ID="txtLookupEmail" Width="175px" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="loginTableContent">
                                        <td class="loginTableText" align="right" colspan="2">
                                            <asp:Button ID="btnEmailLookup" Text=" Lookup " runat="server" CssClass="btn" OnClick="btnEmailLookup_Click">
                                            </asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <asp:ValidationSummary ID="Validationsummary1" runat="server" CssClass="err" HeaderText="You must enter valid values for the following fields:"
                                    DisplayMode="BulletList"></asp:ValidationSummary>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlEmailFound" runat="server" Visible="False" Width="100%">
                    <p>
                        Your email address -
                        <asp:Label ID="foundEmail" runat="server">someone@somewhere.com</asp:Label>&nbsp;-
                        was found in the MSN2 system. An email has been sent to you with a link you can
                        use to set your password.
                    </p>
                    <p>
                        When you receive the email, simply click the link in it to set your password, then
                        you'll be able to login.</p>
                </asp:Panel>
                <asp:Panel ID="pnlNewLogin" runat="server" Visible="False" Width="100%">
                    <p>
                        In order to view the pictures on this site you need to create a login.&nbsp; (<a
                            href="why.aspx">Why?</a>)&nbsp; Please fill out the form below. When you click
                        'Send', a request will be sent to the person you select to activate your login.
                        This way, we can confirm we know the people using this site. We apologize for the
                        invonvenience, but this will protect the privacy of people on this site.
                    </p>
                    <p>
                        When you fill out the below form, you can choose who to send the request to, if
                        you'd like. If only one of us knows you, select the person that knows you below.
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
                                            Your Full Name:
                                            <asp:RequiredFieldValidator ID="NameValidator" runat="server" Display="Dynamic" ControlToValidate="txtName"
                                                ErrorMessage="Name is required!" CssClass="err">*</asp:RequiredFieldValidator>
                                        </td>
                                        <td class="loginTableText">
                                            <asp:TextBox ID="txtName" Width="175px" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="loginTableContent">
                                        <td class="loginTableText">
                                            Email: (<a style="color: black" href="NewLogin.aspx">change</a>)
                                        </td>
                                        <td class="loginTableText">
                                            <asp:Label ID="lblEmail" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="loginTableContent">
                                        <td class="loginTableText">
                                            Password:
                                            <asp:RequiredFieldValidator ID="PasswordValidator" runat="server" Display="Dynamic"
                                                ControlToValidate="txtPassword" ErrorMessage="Password is required!" CssClass="err">*</asp:RequiredFieldValidator>
                                        </td>
                                        <td class="loginTableText">
                                            <asp:TextBox ID="txtPassword" Width="175px" runat="server" TextMode="Password"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="loginTableContent">
                                        <td class="loginTableText">
                                            Confirm Password:
                                            <asp:RequiredFieldValidator ID="ConfirmPasswordValidator" runat="server" Display="Dynamic"
                                                ControlToValidate="txtConfirmPassword" ErrorMessage="Confirmation Password is required!"
                                                CssClass="err">*</asp:RequiredFieldValidator>
                                            <asp:CompareValidator ID="CompareValidator1" runat="server" Display="Dynamic" ControlToValidate="txtConfirmPassword"
                                                ErrorMessage="The confirmation password must match the password." CssClass="err"
                                                Operator="Equal" ControlToCompare="txtPassword">*</asp:CompareValidator>
                                        </td>
                                        <td class="loginTableText">
                                            <asp:TextBox ID="txtConfirmPassword" Width="175px" runat="server" TextMode="Password"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="logintablecontent">
                                        <td class="logintabletext" valign="top">
                                            Automated login script check:
                                        </td>
                                        <td class="logintabletext">
                                            Please enter the following text: etades<br />
                                            <asp:TextBox runat="server" ID="vtxt" />
                                        </td>
                                    </tr>
                                    <tr class="loginTableContent">
                                        <td class="loginTableText">
                                            Send request to:
                                        </td>
                                        <td class="loginTableText">
                                            <asp:DropDownList ID="lstRequest" Width="175px" runat="server">
                                                <asp:ListItem Text="Mike" Selected="true"></asp:ListItem>
                                                <asp:ListItem Text="Sara"></asp:ListItem>
                                                <asp:ListItem Text="Neil"></asp:ListItem>
                                                <asp:ListItem Text="Nick"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr class="loginTableContent">
                                        <td class="loginTableText">
                                            Comments:
                                        </td>
                                        <td class="loginTableText">
                                            <asp:TextBox runat="server" ID="comments" MaxLength="255"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr class="loginTableContent">
                                        <td class="loginTableText" align="right" colspan="2">
                                            <asp:Button ID="btnSend" Text=" Send " runat="server" CssClass="btn" OnClick="btnSend_Click">
                                            </asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <asp:ValidationSummary ID="ValidSummary" runat="server" CssClass="err" DisplayMode="List">
                                </asp:ValidationSummary>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlError" Visible="false">
                    <p>
                        Invalid entry. Click back to correct the form or move along...
                    </p>
                </asp:Panel>
                <asp:Panel ID="pnlInfo" runat="server" Visible="False" Width="100%">
                    <p>
                        Your new login request has been sent. You will receive an email when your account
                        has been activated.
                    </p>
                    <p>
                        You can expect an email within a day or two.</p>
                    <p>
                        &nbsp;</p>
                    <p>
                        In the meantime - go <a href="http://www.google.com">surf the web</a>.</p>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <!-- Begin footer -->
    </form>
</body>
</html>
