<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SLExpress._Default" %>

<%@ Import Namespace="System.Web.Configuration" %>
<%@ Import Namespace="Microsoft.Live" %>
<%@ Register TagPrefix="wl" Namespace="Microsoft.Live" Assembly="Microsoft.Live.AuthHandler, Version=15.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" %>
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:wl="http://apis.live.net/js/2010">
<head id="Head1" runat="server">
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Expires" content="-1" />
    <title>devlists</title>
    <script type="text/javascript" src="http://js.live.net/4.1/loader.js"></script>
    <script type="text/javascript">
        function startDelete(cbx) {

        }

        function deleteTimer() {

        }
    </script>
    <style type="text/css">
        html, body
        {
            overflow: auto;
        }
        body
        {
            padding: 0;
            margin: 0;
            font-family: Segoe UI, Arial, Tahoma;
        }
        .main
        {
            margin: 0px;
            font-size: 12pt;
        }
        .left
        {
            text-align: left;
            float: left;
            bottom: 0px;
            top: 0px;
            width: 10%;
            padding: 5px;
        }
        .right
        {
            text-align: left;
            width: 90%;
            padding: 5px;
        }
        .add
        {
            padding: 5px;
        }
        .signin
        {
            height: 26px;
        }
        .list
        {
            font-family: Segoe UI, Arial, Tahoma;
            font-size: 10pt;
        }
    </style>
</head>
<body style="margin: 0px">
    <script type="text/javascript" src="Silverlight.js"></script>
    <script type="text/javascript">
        //if (window.location.href.indexOf("nosl", 0) < 0 && (Silverlight.isInstalled("4.0") || Silverlight.isInstalled("3.0") || Silverlight.isInstalled("2.0"))) {
        //window.location.href = "sl.aspx";
        //}
        
    </script>
    <wl:app channel-url="<%=WebConfigurationManager.AppSettings["wl_wrap_channel_url"]%>"
        callback-url="<%=WebConfigurationManager.AppSettings["wl_wrap_client_callback"]%>?wl_session_id=<%=SessionId%>"
        client-id="<%=WebConfigurationManager.AppSettings["wl_wrap_client_id"]%>" scope="WL_Profiles.View, WL_Contacts.View">
    </wl:app>
    <div class="signin">
        <wl:signin signedouttext="Sign in" theme="white" />
    </div>
    <form id="form1" runat="server">
    <asp:Panel runat="server" ID="main" CssClass="main">
        <div class="left">
            <asp:Panel runat="server" ID="topPanel">
                <asp:ListBox ID="list" runat="server" AutoPostBack="true" Rows="15" CssClass="list" />
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
</body>
</html>
