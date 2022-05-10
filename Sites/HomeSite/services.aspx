<%@ Page Language="C#" AutoEventWireup="true" CodeFile="services.aspx.cs" Inherits="services" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>home services</title>
    <meta name="viewport" content="width=device-width" />
    <link href="Styles.css" rel="stylesheet" type="text/css" />
</head>
<body class="bodyStyle">
    <form id="form1" runat="server">
        <div class="headerLink" style="padding: 4px; border-bottom-color: silver;">
            <a href="/">home</a> |
            <a href="/cams/">cams</a> | 
            <a href="/services.aspx" style="color: deepskyblue">services</a> |
            <a href="/who/">who</a>
        </div>
        <div style="clear: both; padding-left: 6px; padding-top: 6px">
            <div style="padding-right: 6px; padding-left: 6px;">
                <a href="https://mysecurity.eufylife.com/#/camera" target="_blank">eufy</a> |
                <a href="https://myhs.homeseer.com/" target="_blank">myhs</a><br />
                <font style="font-size: smaller; color: gray">cam/control</font>
            </div>
            <div style="padding-right: 6px; padding-left: 6px; padding-top: 6px">
                <a href="https://www.ecobee.com/consumerportal/" target="_blank">ecobee</a> | 
                <a href="https://my.netatmo.com/app/station" target="_blank">netatmo</a><br />
                <font style="font-size: smaller; color: gray">temp</font>
            </div>
            <div style="padding-right: 6px; padding-left: 6px; padding-top: 6px">
				<a href="https://app.rach.io/" target="_blank">rachio</a> | 
                <a href="https://nas.giesler.org:58433/" target="_blank">nas</a> | 
                <a href="https://192.168.4.2:8443/manage/account/login?redirect=%2Fmanage" target="_blank">unifi</a>
                <br />
                <font style="font-size: smaller; color: gray">network</font>
            </div>
        </div>
    </form>
</body>
</html>
