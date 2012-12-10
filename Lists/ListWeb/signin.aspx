<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="signin.aspx.cs" Inherits="SLExpress.signin" %>

<%@ Import Namespace="System.Web.Configuration" %>
<html xmlns="http://www.w3.org/1999/xhtml" xmlns:wl="http://apis.live.net/js/2010">
<head id="Head1" runat="server">
    <title>ListGo - sign in</title>
    <script src="https://js.live.net/v5.0/wl.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="listgo.css" />
</head>
<body>
    <form id="form1" runat="server">
    ListGo 
    <a href="#" onclick="signIn();" class="rightTop">Sign In</a>
    <hr noshade />
    <div id="message">
        Click 'Sign In' to see your lists.</div>
    <script type="text/javascript">

                    function onLogin() {
                        var session = WL.getSession();
                        if (session) {
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
                    }
                    function signIn() {
                        WL.login({ scope: "wl.signin"});
                    }

                    document.all['message'].innerText = 'Authenticating...';
                    WL.Event.subscribe("auth.login", onLogin);

                    WL.init({
                        client_id: "<%=WebConfigurationManager.AppSettings["Live_Client_Id"]%>",
                        redirect_uri: "http://listgo.mobi/signin.aspx"
                    });

                    var session = WL.getSession();
                    if (session)
                    {
                       window.location.href = "default.aspx";
                    }

    </script>
    </form>
</body>
</html>
