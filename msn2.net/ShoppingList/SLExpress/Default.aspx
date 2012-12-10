<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SLExpress._Default" %>

<%@ Import Namespace="System.Web.Configuration" %>
<%@ Import Namespace="Microsoft.Live" %>
<%@ Register TagPrefix="wl" Namespace="Microsoft.Live" Assembly="Microsoft.Live.AuthHandler, Version=15.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" %>
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:wl="http://apis.live.net/js/2010">
<head id="Head1" runat="server">
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Expires" content="-1" />
    <title>My eLists</title>
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
        .main
        {
            width: 80%;
            margin: auto;
        }
        .left
        {
            width: 20%;
            float: left;
            text-align: left;
            display: inline;
        }
        .right
        {
            width: 80%;
            float: right;
            text-align: left;
            display: inline;
        }
        .signin
        {
            height: 26px;
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
    <wl:app channel-url="<%=WebConfigurationManager.AppSettings["wl_wrap_channel_url"]%>"
        callback-url="<%=WebConfigurationManager.AppSettings["wl_wrap_client_callback"]%>?wl_session_id=<%=SessionId%>"
        client-id="<%=WebConfigurationManager.AppSettings["wl_wrap_client_id"]%>" scope="WL_Profiles.View, WL_Contacts.View">
    </wl:app>
    <div class="signin">
        <wl:signin signedouttext="Sign in" theme="white" />
    </div>
    <form id="form1" runat="server" style="height: 100%">
    <div class="main">
        <div class="left">
            <asp:Panel runat="server" ID="topPanel">
                <asp:ListBox ID="list" runat="server" AutoPostBack="true" Rows="10" />
            </asp:Panel>
        </div>
        <div class="right">
            <asp:Panel runat="server" ID="itemPanel">
                <asp:Repeater runat="server" ID="items">
                    <ItemTemplate>
                        <input name="<%# DataBinder.Eval(Container.DataItem, "UniqueId") %>" value="on" type="checkbox" disabled="disabled">
                            <%# DataBinder.Eval(Container.DataItem, "Name") %>
                        </input><br />
                    </ItemTemplate>
                </asp:Repeater>
            </asp:Panel>
        </div>
    </div>
    <asp:Panel runat="server" ID="addPanel" Visible="false">
        <asp:TextBox ID="add" runat="server" MaxLength="50" />
        <br />
        <asp:Button ID="addButton" runat="server" Text="Add" />
        <asp:Button ID="cancelButton" runat="server" Text="Cancel" />
    </asp:Panel>
    </form>
</body>
</html>
