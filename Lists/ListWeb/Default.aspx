<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SLExpress._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Web.Configuration" %>
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:wl="http://apis.live.net/js/2010">
<head id="Head1" runat="server">
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Expires" content="-1" />
    <meta content="user-scalable=no, width=device-width" name="viewport" />
    <meta name="MobileOptimized" content="width">
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
    ListGo <a href="#" onclick="onSignOut();" id="signOut" class="rightTop">sign out</a>
    <hr noshade />
    <asp:Panel runat="server" CssClass="main">
        <asp:Panel ID="modePanel" runat="server">
            <asp:DropDownList ID="list" runat="server" AutoPostBack="true" CssClass="list" />
            <asp:LinkButton ID="addMode" Text="add" runat="server" OnClick="OnAdd" CssClass="modeLink" />
            |
            <asp:LinkButton ID="editMode" Text="edit" runat="server" OnClick="OnEdit" CssClass="modeLink" />
            |
            <asp:LinkButton ID="viewMode" Text="view" Font-Bold="true" runat="server" CssClass="modeLink"
                OnClick="OnView" />
            <hr />
        </asp:Panel>
        <asp:Panel runat="server" ID="addPanel" Visible="false" CssClass="add">
            <asp:Label ID="addLabel" runat="server" Text="Add items (one per line)" />
            <br />
            <asp:TextBox ID="add" runat="server" Rows="10" Columns="30" TextMode="MultiLine" />
            <br />
            <asp:Button ID="addButton" runat="server" Text="Add" />
            <asp:Button ID="cancelButton" runat="server" Text="Cancel" />
        </asp:Panel>
        <asp:Panel runat="server" ID="editPanel" Visible="false">
            <asp:Panel ID="editItems" runat="server" />
            <asp:Panel ID="moveItem" runat="server" Visible="false">
                <asp:HiddenField ID="moveItemId" runat="server" />
                <asp:Panel ID="moveItemList" runat="server" />
                <asp:Button ID="cancelMove" runat="server" OnClick="OnCancelMove" />
            </asp:Panel>
        </asp:Panel>
        <asp:Panel runat="server" ID="main" Visible="true">
            <asp:Panel runat="server" ID="itemPanel">
            </asp:Panel>
        </asp:Panel>
    </asp:Panel>
    </form>
    <script type="text/javascript">        // <!--
        function onSignOut() {
            window.location.href = "signout.aspx";
        }
        // --></script>
</body>
</html>
