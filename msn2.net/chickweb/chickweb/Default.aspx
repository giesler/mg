<%@ Page Title="msn2.net Chicken Cams" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="chickweb._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <table>
        <tr>
            <td valign="middle" align="center">
                <asp:HyperLink ID="refreshLink" NavigateUrl="/?c=1" runat="server">
                    <asp:Image ID="webcam1" ImageUrl="getimg.aspx" AlternateText="" CssClass="webcamImage"
                        runat="server" />
                </asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td align="center">
                Camera <asp:LinkButton ID="cam1" runat="server" Enabled="false">1</asp:LinkButton> | <asp:LinkButton ID="cam2" runat="server">2</asp:LinkButton> | <asp:LinkButton ID="cam3" runat="server">3</asp:LinkButton> 
            </td>
        </tr>
    </table>
    <script type="text/javascript" src="Silverlight.js"></script>
    <script language="javascript"><!--

        var count = 0;
        var cam = <%=GetCameraNumber() %>
        function Refresh() {
            var tmp = new Date();
            tmp = "getimg.aspx?" + tmp.getTime() + "&c=" + cam.toString();
            window.status = "Auto refresh enabled";
            try {
                document.images["ctl00_MainContent_webcam1"].src = tmp;
            }
            catch (e) {
            }
            count = count + 1;
            if (count < 10000) {
                setTimeout("Refresh()", 333);
            }
            else {
                window.status = "Hit F5 to refresh";
            }
        }

        if (document.URL.indexOf("nosl", 0) < 0 && (Silverlight.isInstalled("4.0") || Silverlight.isInstalled("3.0") || Silverlight.isInstalled("2.0"))) {
            document.URL = "sl.aspx";
        }
        else {
            setTimeout("Refresh()", 1000);
        }
            // -->
    </script>
</asp:Content>
