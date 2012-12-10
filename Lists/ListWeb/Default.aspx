<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SLExpress._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Web.Configuration" %>

<html xmlns="http://www.w3.org/1999/xhtml" xmlns:wl="http://apis.live.net/js/2010">
<head id="Head1" runat="server">
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Expires" content="-1" />
    <meta content="user-scalable=no, width=device-width" name="viewport" />
    <title>ListGo</title>
    <script type="text/javascript">
        function startDelete(cbx) {

        }

        function deleteTimer() {

        }
    </script>
    <link rel="stylesheet" type="text/css" href="listgo.css" />
</head>
<body>
    <form id="form1" runat="server">
    ListGo
    <a href="#" onclick="onSignOut();" id="signOut" class="rightTop">Sign Out</a>
    <hr noshade />
    <asp:Panel runat="server" ID="main" CssClass="main">
        <div class="left">
            <asp:Panel runat="server" ID="topPanel">
                <asp:DropDownList ID="list" runat="server" AutoPostBack="true" CssClass="list" />
            </asp:Panel>
        </div>
        <div class="right">
            <asp:Panel runat="server" ID="itemPanel">
            </asp:Panel>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="addPanel" Visible="false" CssClass="add">
        <asp:TextBox ID="add" runat="server" MaxLength="50" />
        <br />
        <asp:Button ID="addButton" runat="server" Text="Add" />
        <asp:Button ID="cancelButton" runat="server" Text="Cancel" />
    </asp:Panel>
    </form>
    <script type="text/javascript">// <!--
        function onSignOut() {
            window.location.href = "signout.aspx";
        }
    // --></script>
</body>
</html>
