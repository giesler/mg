<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SLExpress._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Web.Configuration" %>
<%@ Import Namespace="Microsoft.Live" %>
<%@ Register TagPrefix="wl" Namespace="Microsoft.Live" Assembly="Microsoft.Live.AuthHandler, Version=15.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" %>
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:wl="http://apis.live.net/js/2010">
<head id="Head1" runat="server">
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Expires" content="-1" />
    <meta content="user-scalable=no, width=device-width" name="viewport" />
    <title>Lists</title>
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
            font-family: Segoe UI, Arial, Tahoma;
        }
        .main
        {
            font-size: 12pt;
        }
        .rightTop
        {
            text-align: right;
            float: right;
            margin: 3px;
        }
        .left
        {
            text-align: left;
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
<body>
    <form id="form1" runat="server">
    Lists
    <a href="#" onclick="onSignInOut();" disabled="true" id="signInOut" class="rightTop">Sign In</a>
    <hr width="50%" noshade align="left" />
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
        function onWLSignedIn(e) {
        }
        function onWLSignedOut(e) {
        }
        function onWLLoad() {
            signInOut.disabled = false;
        }

        function onAsyncSignInComplete() {
            window.location.href = '/';
        }

        function onSignInOut() {
            signInOut.disabled = true;

            if ("<%=UserId %>" != "") {
                Microsoft.Live.App.signOut();
                window.location.href = 'liveauth.aspx?action=logout';
            } else {
                Microsoft.Live.App.signIn(onAsyncSignInComplete)
            }

            return false;
        }

        Microsoft.Live.Core.Loader.load(["microsoft.live"], function () {
            Microsoft.Live.App.initialize(
                {
                    channelurl: "<%=WebConfigurationManager.AppSettings["wl_wrap_channel_url"]%>",
                    callbackurl: "<%=WebConfigurationManager.AppSettings["wl_wrap_client_callback"]%>?wl_session_id=<%=SessionId%>",
                    clientid: "<%=WebConfigurationManager.AppSettings["wl_wrap_client_id"]%>",
                    scope: "WL_Profiles.View, WL_Contacts.View",
                    onload: "onWLLoad"
                }
            );
        });

        if ("<%=UserId %>" != "") {
            signInOut.innerHTML = "Sign Out";
        } else {
            signInOut.innerHTML = "Sign In";
        }
                
    // --></script>
</body>
</html>
