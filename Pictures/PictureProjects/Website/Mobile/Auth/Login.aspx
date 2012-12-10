<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="pics.Controls.Mobile.Auth.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>pics.msn2.net Login</title>
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
    <div>
        <table>
            <tr>
                <td>
                    Email:
                </td>
                <td>
                    <asp:TextBox runat="server" AutoCompleteType="Email" ID="email" MaxLength="75" />
                </td>
            </tr>
            <tr>
                <td>
                    Password:
                </td>
                <td>
                    <asp:TextBox runat="server" ID="password" TextMode="Password" MaxLength="75" />
                </td>
            </tr>
            <tr>
                <td />
                <td>
                    <asp:CheckBox runat="server" ID="chkSave" Text="Save password" />
                </td>
            </tr>
        </table>
        <br />
        <asp:Button runat="server" ID="login" Text="Login" OnClick="login_Click" />
        <br />
        <asp:Label runat="server" ID="error" Text="Invalid email/password." Visible="false" />
    </div>
    </form>
</body>
</html>
