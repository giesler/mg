
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" EnableViewState="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MSN2 Cams</title>
    <link href="Styles.css" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="width=device-width" /> 
    <link rel="SHORTCUT ICON" href="favicon.ico" />
</head>
<body>
    <form id="form1" runat="server" style="height: 100%">
    <asp:Repeater runat="server" ID="thumbs" OnItemDataBound="thumbs_ItemDataBound">
        <ItemTemplate>
            <asp:HyperLink runat="server" ID="thumbALink">
                <asp:Image runat="server" ID="thumbAImage" Height="64" BorderWidth="0" />
                <asp:Image runat="server" ID="thumbBImage" Height="64" BorderWidth="0" />
            </asp:HyperLink>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Panel runat="server" ID="topLinks" CssClass="linkPanel">
        <asp:HyperLink runat="server" ID="homeLink1" NavigateUrl="http://home.msn2.net/" Visible="false" Text="HOME" />
        <asp:Label runat="server" ID="logLinkSeperator0" Text=" | " Visible="false" />
        <asp:HyperLink runat="server" ID="controlLink1" NavigateUrl="http://control.msn2.net/" Text="CONTROL" Visible="false" /> 
        <asp:Label runat="server" ID="logLinkSeperator3" Text=" | " Visible="false" />
        <asp:HyperLink runat="server" ID="logLink1" NavigateUrl="http://ddns.msn2.net:8081/jpegpull.htm" Text="LOG" Visible="false" /> 
        <asp:Label runat="server" ID="logLinkSeperator1" Text=" | " Visible="false" />
        <asp:HyperLink runat="server" ID="signInOutLink1" NavigateUrl="http://login.msn2.net/logout.aspx?r=http://cams.msn2.net" Text="SIGN IN" />
    </asp:Panel>
    <div style="height: 100%">
        <asp:Panel ID="rightPanel" runat="server" Visible="false" CssClass="rightFloat">
            <asp:Image ID="main2" runat="server" ClientIDMode="Static" Visible="false" CssClass="fullImage" />
        </asp:Panel>
        <asp:Panel ID="leftPanel" runat="server" CssClass="leftFloat">
            <asp:Image ID="main" runat="server" ClientIDMode="Static" CssClass="fullImage" />
        </asp:Panel>
    </div>
    <asp:Panel runat="server" ID="bottomLinks" CssClass="linkPanel" Visible="false">
        <asp:HyperLink runat="server" ID="homeLink2" NavigateUrl="http://home.msn2.net/" Visible="false" Text="HOME" />
        <asp:Label runat="server" ID="logLinkSeperator5" Text=" | " Visible="false" />
        <asp:HyperLink runat="server" ID="controlLink2" NavigateUrl="http://control.msn2.net/" Text="CONTROL" Visible="false" /> 
        <asp:Label runat="server" ID="logLinkSeperator4" Text=" | " Visible="false" />
        <asp:HyperLink runat="server" ID="logLink2" NavigateUrl="http://ddns.msn2.net:8081/jpegpull.htm" Text="LOG" Visible="false" /> 
        <asp:Label runat="server" ID="logLinkSeperator2" Text=" | " Visible="false" />        
        <asp:HyperLink runat="server" ID="signInOutLink2" NavigateUrl="http://login.msn2.net/logout.aspx?r=http://cams.msn2.net" Text="SIGN IN" />
    </asp:Panel>

    </form>
    <script language="javascript" type="text/javascript"><!--

        var count = 0;
        var cam = '<%=GetCam() %>';
        var mainRefreshInterval = <%=GetRefreshInterval("main") %>;
        var thumbRefreshInterval = <%=GetRefreshInterval("thumb") %>;
        var basePrefix = '<%=GetBasePrefix() %>';
        var thumbHeight = <%=GetThumbHeight() %>;

        function RefreshMain() {
            var tmp = new Date();
            //tmp = "http://" + basePrefix + ".msn2.net:8808/getimg.aspx?" + tmp.getTime() + "&c=" + cam.toString();

            tmp = "http://localhost:8808/getimg.aspx?" + tmp.getTime() + "&c=" + cam.toString();

            var tmp2 = new Date();
            if (cam == "1")
            {
              //  tmp2 = "http://cam2.msn2.net:8808/getimg.aspx?" + tmp2.getTime() + "&c=2";
                tmp2 = "http://localhost:8808/getimg.aspx?" + tmp2.getTime() + "&c=2";
            }
            
            window.status = "Auto refresh enabled";
            try {
                document.images["main"].src = tmp;
                if (cam == "1")
                {
                    document.images["main2"].src = tmp2;
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

        setTimeout("RefreshMain()", 250);
        setTimeout("RefresThumbs()", 500);
            // -->
    </script>
</body>
</html>
