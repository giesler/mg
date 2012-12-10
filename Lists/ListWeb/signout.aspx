<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="signout.aspx.cs" Inherits="SLExpress.signout" %>
<%@ Import Namespace="System.Web.Configuration" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ListGo - Signing out</title>
    <script src="https://js.live.net/v5.0/wl.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="listgo.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Signing out...
        <script type="text/javascript">

            WL.init({
                client_id: "<%=WebConfigurationManager.AppSettings["Live_Client_Id"]%>",
                redirect_uri: "http://listgo.mobi/signin.aspx"
            });
            
            function onLogOut(response) {
                window.location.href = "signin.aspx";
            }

            WL.logout(onLogOut);
        </script>

    </div>
    </form>
</body>
</html>
