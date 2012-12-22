<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" EnableViewState="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>cams.msn2.net Login</title>
    <meta name="viewport" content="width=device-width" /> 
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>
                    User name:
                </td>
                <td>
                    <asp:TextBox ID="username" runat="server" MaxLength="50" />
                </td>
            </tr>
            <tr>
                <td>
                    Password:
                </td>
                <td>
                    <asp:TextBox ID="password" runat="server" MaxLength="50" TextMode="Password" /><br />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:Button ID="login" runat="server" Text="Login" OnClick="OnLogin" />
                </td>
            </tr>
        </table>
    </div>
    </form>
    <script language="javascript" type="text/javascript"><!--
        if (document.all.username)
            document.all.username.focus();
    // --></script>
</body>
</html>
