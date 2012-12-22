<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AlertItem.aspx.cs" Inherits="AlertItem" EnableViewState="false" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>cam alert</title>
    <link href="Styles.css" rel="stylesheet" type="text/css" />
    <meta name="viewport" content="width=device-width" />
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 640px;">
            <div style="float: right; padding-right: 3px">
                <asp:HyperLink runat="server" ID="previousLink">&lt;&lt;</asp:HyperLink>
                <asp:HyperLink runat="server" ID="nextLink">&gt;&gt;</asp:HyperLink>
                &nbsp;
        <a href="Log.aspx">LOG</a>
            </div>
            <asp:Label runat="server" ID="name" />
            &nbsp;&nbsp;    
    <asp:DropDownList runat="server" ID="videos" AutoPostBack="true" Style="font-size: smaller; width: 85px" />
            <br />
            <asp:Image runat="server" ID="img" />
            <video controls="controls" autoplay="autoplay" runat="server" id="videoControl" style="visibility: hidden">
                <source src="<%=GetVideoUrl() %>" type="video/mp4" />
                Your browser does not support the video tag.
            </video>
        </div>
    </form>
    <script type="text/javascript"><!--
    <%=GetScript() %>    

        var lastSelected = null;
        function openVideo() {
            var vid = document.all.videos;
            if (vid.selectedIndex > 0) {
                var item = vid[vid.selectedIndex];
                if (item.value != lastSelected) {
                    //window.open("getvid.aspx?v=" + item.value);
                }
                lastSelected = item.value;
            }
        }
        // --></script>
</body>
</html>
