

<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html dir="ltr" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
    <meta http-equiv="cache-control" content="no-cache" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta name="viewport" content="width=device-width" />
    <link href="Styles.css" rel="stylesheet" type="text/css" />
    <title>home.giesler.org</title>
    <link rel="SHORTCUT ICON" href="favicon.ico" />
    <script language="javascript" type="text/javascript">
        function OnReloadPage() {
            try {
                window.location.reload()
                this.all.refreshError.innerText = ''
            }
            catch {
                this.all.refreshError.innerText = $_
                setTimeout(OnReloadPage, 10000)
            }
        }

        setTimeout(OnReloadPage, 300000)
    </script>
</head>

<body class="bodyStyle">
    <div class="headerLink" style="padding: 4px; border-bottom-color: silver;">
        <a href="/" style="color: deepskyblue">home</a> |
        <a href="/cams/">cams</a> | 
        <a href="/services.aspx">services</a>
    </div>
    <div style="clear: both; background-color: black;">
        <a class="smallthumb" href="/cams/?c=gdw" target="_blank">
            <img class="smallthumb" src="https://svcs.giesler.org:8443/getimg.aspx?c=gdw&amp;h=128&amp;id=th" alt="cam1" id="cam1img" runat="server"/></a>
        <a class="smallthumb" href="/cams/?c=dwety" target="_blank">
            <img class="smallthumb" src="https://svcs.giesler.org:8443/getimg.aspx?c=dwety&amp;h=128&amp;id=th" alt="cam2" id="cam2img" runat="server" /></a>
        <a class="smallthumb" href="https://www.wsdot.wa.gov/aviation/WebCam/Packwood.htm" target="_blank">
            <img class="smallthumb" src="https://images.wsdot.wa.gov/airports/packwood5.jpg" style="width: 223px;" alt="packwood" id="cam3img" runat="server" /></a>
    </div>
    <div style="clear: both; background-color: black; height: 135px; padding-left: 6px">
        <iframe width="100%" height="100%" marginheight="0" marginwidth="0" frameborder="0" src="devices.aspx" scrolling="auto">
        </iframe>
    </div>
    <div style="clear: both; background-color: black; height: 215px">
        <iframe width="100%" height="100%" marginheight="0" marginwidth="0" frameborder="0" src="weather.aspx" scrolling="auto">
        </iframe>
    </div>
    <div style="vertical-align: bottom; color: darkred">
        <div id="refreshError"></div>
    </div>
</body>
</html>
