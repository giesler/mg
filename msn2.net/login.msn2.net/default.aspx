<%@ Page Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="Login" EnableViewState="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>msn2.net Login</title>
    <meta name="viewport" content="width=device-width" /> 
    <link href="http://home.msn2.net/Styles.css" rel="stylesheet" type="text/css" />
</head>
<body class="bodyStyle" style="color: white; background-color: black">

    <div class="headerLink">
        <a href="http://www.msn2.net/">MSN2.NET</a>: <a href="http://home.msn2.net">HOME</a> | <a href="http://cams.msn2.net/">CAMS</a> |  <a href="http://control.msn2.net/">CONTROL</a> | 
	    <a href="http://ts.msn2.net/">TS</a>
    </div>
    <br />
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
        if (document.all.username) {
            if (document.all.username.value.length > 0) {
                document.all.password.focus();
            }
            else {
                document.all.username.focus();
            }
        }
    // --></script>
</body>
</html>
