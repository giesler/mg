<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" EnableViewState="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MS2N Cams</title>
    <link href="../Styles.css" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="width=device-width" /> 
    <link rel="SHORTCUT ICON" href="../favicon.ico" />
</head>
<body>
    <form id="form1" runat="server" style="height: 100%">
    <asp:Repeater runat="server" ID="thumbs" OnItemDataBound="thumbs_ItemDataBound">
        <ItemTemplate>
            <asp:HyperLink runat="server" ID="thumbLink">
                <asp:Image runat="server" ID="thumbImage" Height="64" BorderWidth="0" />
            </asp:HyperLink>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Panel runat="server" ID="topLinks" CssClass="linkPanel">
        <asp:HyperLink runat="server" ID="homeLink1" NavigateUrl="/" Visible="false" Text="home" />
        <asp:Label runat="server" ID="logLinkSeperator0" Text=" | " Visible="false" />
        <asp:HyperLink runat="server" ID="logLink1" NavigateUrl="https://nas.ms2n.net:58433/webman/3rdparty/SurveillanceStation/" Text="nas" Visible="false" /> 
    </asp:Panel>
    <div style="height: 100%">
        <asp:Panel ID="mainPanel" runat="server" CssClass="leftFloat">
            <asp:Image ID="main" runat="server" ClientIDMode="Static" CssClass="fullImage" />
        </asp:Panel>
    </div>
    <asp:Panel runat="server" ID="bottomLinks" CssClass="linkPanel" Visible="false">
        <asp:HyperLink runat="server" ID="homeLink2" NavigateUrl="/" Visible="false" Text="home" />
        <asp:Label runat="server" ID="logLinkSeperator5" Text=" | " Visible="false" />
        <asp:HyperLink runat="server" ID="logLink2" NavigateUrl="https://nas.ms2n.net:58433/webman/3rdparty/SurveillanceStation/" Text="nas" Visible="false" /> 
    </asp:Panel>

    </form>
    <script language="javascript" type="text/javascript"><!--

        var count = 0;
        var mainTimer = 0;
        var cam = '<%=GetCam() %>';
        var mainRefreshInterval = <%=GetRefreshInterval("main") %>;
        var thumbRefreshInterval = <%=GetRefreshInterval("thumb") %>;
        var basePrefix = '<%=GetBasePrefix() %>';
        var thumbHeight = <%=GetThumbHeight() %>;
            
        function RefreshMain() {
            var tmp = new Date();

            if (mainTimer != 0)
            {
                clearTimeout(mainTimer);
                mainTimer = 0;
            }
                
            tmp = "../getimg.aspx?ts=" + tmp.getTime() + "&c=" + cam.toString();
            
            window.status = "Auto refresh enabled";
            try {
                document.images["main"].src = tmp;
            }
            catch (e) {
            }
            count = count + 1;
            if (count < 5000) {
                mainTimer = setTimeout("RefreshMain()", mainRefreshInterval);
            }
            else {
                window.status = "Hit F5 to refresh";
            }

            inGet = false;
        }

        function RefresThumbs() {
            var tmp = new Date();
            try {
                for (var i = 0; i < document.images.length; i++) {
                    var s = document.images[i].src;
                    if (s.indexOf("id=th") > 0) {
                        document.images[i].src = s.substring(0, s.indexOf("id=th")) + "id=th" + tmp.getTime();
                    }
                }
            }
            catch (e) {
            }
            count = count + 1;
            if (count < 3600) {
                setTimeout("RefresThumbs()", thumbRefreshInterval);
            }
        }

        mainTimer = setTimeout("RefreshMain()", 250);
        setTimeout("RefresThumbs()", 500);
            // -->
    </script>
</body>
</html>
