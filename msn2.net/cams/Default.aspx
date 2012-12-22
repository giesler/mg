<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" EnableViewState="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>cams.msn2.net</title>
    <link href="Styles.css" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="width=device-width" /> 
</head>
<body>
    <form id="form1" runat="server" style="height: 100%">
    <div style="float: right; padding-right: 3px">
        <a href="Log.aspx">LOG</a>
    </div>
    <div>
        <asp:HyperLink runat="server" NavigateUrl="?c=dw1">
            <asp:Image ID="cam1thumb" runat="server" Height="64" ImageUrl="http://cam1.msn2.net/getimg.aspx?c=dw1&h=64"
                BorderWidth="0" />
        </asp:HyperLink>
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="?c=front">
            <asp:Image ID="cam2thumb" runat="server" Height="64" ImageUrl="http://cam1.msn2.net/getimg.aspx?c=front&h=64"
                BorderWidth="0" />
        </asp:HyperLink>
        <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="?c=side">
            <asp:Image ID="cam6thumb" runat="server" Height="64" ImageUrl="http://cam5.msn2.net/getimg.aspx?c=side&h=64"
                BorderWidth="0" />
        </asp:HyperLink>
        <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="?c=3">
            <asp:Image ID="cam5thumb" runat="server" Height="64" ImageUrl="http://cam5.msn2.net/getimg.aspx?c=3&h=64"
                BorderWidth="0" />
        </asp:HyperLink>
        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="?c=1">
            <asp:Image ID="cam3thumb" runat="server" Height="64" ImageUrl="http://cam3.msn2.net/getimg.aspx?c=1&h=64"
                BorderWidth="0" />
            <asp:Image ID="cam4thumb" runat="server" Height="64" ImageUrl="http://cam3.msn2.net/getimg.aspx?c=2&h=64"
                BorderWidth="0" />
        </asp:HyperLink>
    </div>
    <div style="height: 100%">
        <asp:Panel ID="rightPanel" runat="server" Visible="false" CssClass="rightFloat">
            <asp:Image ID="main2" runat="server" ClientIDMode="Static" Visible="false" CssClass="fullImage" />
        </asp:Panel>
        <asp:Panel ID="leftPanel" runat="server" CssClass="leftFloat">
            <asp:Image ID="main" runat="server" ClientIDMode="Static" CssClass="fullImage" />
        </asp:Panel>
    </div>
    </form>
    <script language="javascript" type="text/javascript"><!--

        var count = 0;
        var cam = "<%=GetCam() %>"
        var mainRefreshInterval = <%=GetRefreshInterval("main") %>;
        var thumbRefreshInterval = <%=GetRefreshInterval("thumb") %>;
        var basePrefix = "<%=GetBasePrefix() %>";
        var thumbHeight = <%=GetThumbHeight() %>;

        function RefreshMain() {
            var tmp = new Date();
            tmp = "http://" + basePrefix + ".msn2.net/getimg.aspx?" + tmp.getTime() + "&c=" + cam.toString();

            var tmp2 = new Date();
            if (cam == "1")
            {
                tmp2 = "http://cam3.msn2.net/getimg.aspx?" + tmp2.getTime() + "&c=2";
            }
            else if (cam == "side")
            {
                tmp2 = "http://cam5.msn2.net/getimg.aspx?" + tmp2.getTime() + "&c=side";
            }

            window.status = "Auto refresh enabled";
            try {
                document.images["main"].src = tmp;
                if (cam == "1")
                {
                    document.images["main2"].src = tmp2;
                }

                if (cam == "dw1") {
                    document.images["cam1thumb"].src = tmp;
                } else if (cam == "front") {
                    document.images["cam2thumb"].src = tmp;
                } else if (cam == "1") {
                    document.images["cam3thumb"].src = tmp;                    
                    document.images["cam4thumb"].src = tmp2;
                } else if (cam == "3") {
                    document.images["cam5thumb"].src = tmp;
                } else if (cam == "side") {
                    document.images["cam6thumb"].src = tmp;
                }
            }
            catch (e) {
            }
            count = count + 1;
            if (count < 5000) {
                setTimeout("RefreshMain()", mainRefreshInterval);
            }
            else {
                window.status = "Hit F5 to refresh";
            }
        }

        function RefresThumbs() {
            var tmp = new Date();
            try {
                document.images["cam1thumb"].src = "http://cam1.msn2.net/getimg.aspx?" + tmp.getTime() + "&c=dw1&h=" + thumbHeight;
                document.images["cam2thumb"].src = "http://cam1.msn2.net/getimg.aspx?" + tmp.getTime() + "&c=front&h=" + thumbHeight;
                document.images["cam3thumb"].src = "http://cam2.msn2.net/getimg.aspx?" + tmp.getTime() + "&c=1&h=" + thumbHeight;
                document.images["cam4thumb"].src = "http://cam2.msn2.net/getimg.aspx?" + tmp.getTime() + "&c=2&h=" + thumbHeight;
                document.images["cam5thumb"].src = "http://cam3.msn2.net/getimg.aspx?" + tmp.getTime() + "&c=3&h=" + thumbHeight;
                document.images["cam6thumb"].src = "http://cam3.msn2.net/getimg.aspx?" + tmp.getTime() + "&c=side&h=" + thumbHeight;
            }
            catch (e) {
            }
            count = count + 1;
            if (count < 5000) {
                setTimeout("RefresThumbs()", thumbRefreshInterval);
            }
        }

        setTimeout("RefreshMain()", 250);
        setTimeout("RefresThumbs()", 500);
            // -->
    </script>
</body>
</html>
