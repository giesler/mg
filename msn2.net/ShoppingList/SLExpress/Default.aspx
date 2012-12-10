<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SLExpress._Default" %>

<%@ Register TagPrefix="wl" Namespace="Microsoft.Live" Assembly="Microsoft.Live.AuthHandler, Version=15.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="height: 100%;">
<head runat="server">
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Expires" content="-1" />
    <title>Shopping List</title>
    <style type="text/css">
        html, body
        {
            height: 100%;
            overflow: auto;
        }
        body
        {
            padding: 0;
            margin: 0;
        }
        #silverlightControlHost
        {
            height: 100%;
            text-align: center;
        }
    </style>
</head>
<body style="height: 100%; margin: 0px">
    <script type="text/javascript" src="Silverlight.js"></script>
    <script type="text/javascript">
        //if (window.location.href.indexOf("nosl", 0) < 0 && (Silverlight.isInstalled("4.0") || Silverlight.isInstalled("3.0") || Silverlight.isInstalled("2.0"))) {
        //window.location.href = "sl.aspx";
        //}
    </script>
    <!--
    <iframe id="WebAuthControl" name="WebAuthControl" src="http://login.live.com/controls/WebAuth.htm?appid=<%=AppId%>&style=font-size%3A+10pt%3B+font-family%3A+verdana%3B+background%3A+white%3B"
        width="80px" height="20px" marginwidth="0" marginheight="0" align="middle" frameborder="0"
        scrolling="no"></iframe> -->
    <p>
        <% if (UserId == null)
           { %>
        This application does not know who you are! Click the <b>Sign in</b> link above.
        <% }
           else
           { %>
        Now this application knows that you are the user with ID = "<b><%=UserId%></b>".
        <% } %>
    </p>
    <form id="form1" runat="server" style="height: 100%">
    <asp:Panel runat="server" ID="topPanel">
        <asp:DropDownList ID="list" runat="server" AutoPostBack="true" />
    </asp:Panel>
    <asp:Panel runat="server" ID="itemPanel">
        <asp:ListBox ID="items" runat="server" AutoPostBack="true" />
    </asp:Panel>
    <asp:Panel runat="server" ID="addPanel">
        <asp:TextBox ID="add" runat="server" MaxLength="50" />
        <br />
        <asp:Button ID="addButton" runat="server" Text="Add" />
        <asp:Button ID="cancelButton" runat="server" Text="Cancel" />
    </asp:Panel>
    </form>
</body>
</html>
