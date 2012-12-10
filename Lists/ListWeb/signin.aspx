<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="signin.aspx.cs" Inherits="SLExpress.signin" %>

<%@ Import Namespace="System.Web.Configuration" %>
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:wl="http://apis.live.net/js/2010">
<head id="Head1" runat="server">
    <title>ListGo - sign in</title>
    <script src="https://js.live.net/v5.0/wl.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="listgo.css" />
    <meta content="user-scalable=no, width=device-width" name="viewport" />
    <meta name="MobileOptimized" content="width">
</head>
<body>
    <form id="form1" runat="server">
    ListGo <a href="#" onclick="signIn();" class="rightTop">sign in</a>
    <hr noshade />
    <div id="message" />
    <div id="session" />
    <div id="state" />
    <script type="text/javascript">
        function onLogin() {
            var session = WL.getSession();
            if (session) {
                document.all['message'].innerText = 'Getting details...';

                WL.api(
                {
                    path: "me",
                    method: "GET"
                },
                function (response) {
                    if (!response.error) {                                
                        window.location.href = "signin.aspx?auth=" + session.authentication_token + "&access=" + session.access_token + "&id=" + response.id + "&name=" + response.name;
                    }
                    else {
                        document.all['message'].innerText = "Error loading user details: " + response.error.message;
                    }                                
                }
                );
            }
            else
            {
                document.all['message'].innerText = 'No session';
            }
        }
        function signIn() {
            WL.login({ scope: "wl.signin"});
        }
        function onStatusChange() {
            var session = WL.getSession();
            document.all['session'].innerText = 'session change';
        }
        function onSessionChange() {
            var session = WL.getSession();
            document.all['state'].innerText = 'status change';
        }

        document.all['message'].innerText = 'Loading...';
        WL.Event.subscribe("auth.login", onLogin);
        WL.Event.subscribe("auth.statusChange", onStatusChange);
        WL.Event.subscribe("auth.sessionChange", onSessionChange);

        WL.init({
            client_id: "<%=WebConfigurationManager.AppSettings["Live_Client_Id"]%>",
            redirect_uri: "http://listgo.mobi/signin.aspx"
        });

        var session = WL.getSession();
        var status = WL.getLoginStatus();
        if (session)
        {
            document.all['message'].innerText = 'Loading...';                       
            onLogin();
        }
        else
        {
            document.all['message'].innerText = 'Authenticating...';
        }

    </script>
    </form>
</body>
</html>
