<%@ Register TagPrefix="uc1" TagName="AutoTimer" Src="Controls/AutoTimer.ascx" %>

<%@ Page Language="c#" Inherits="pics.picviewss" CodeBehind="picviewss.aspx.cs" %>

<%@ Register TagPrefix="picctls" Namespace="pics.Controls" Assembly="WebControlLibrary" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript (ECMAScript)" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <meta http-equiv="refresh" content="<%= HttpRefreshURL %>">
    <link href="msn2.css" type="text/css" rel="stylesheet">
</head>
<body class="pictureMode" leftmargin="0" topmargin="0">
    <form id="picview" method="post" runat="server">
    <div id="picture" style="left: 0px; position: absolute; top: 0px">
        <table cellspacing="0" cellpadding="2" border="0">
            <tr>
                <td class="picviewPictureBorder" id="tdPicture" runat="server">
                </td>
            </tr>
        </table>
    </div>
    <div class="infoPanel">
        <table class="infoPanelTable" cellspacing="0">
            <tr>
                <td class="infoPanelCategoryBar" height="10">
                    <asp:Label ID="lblCategory" Font-Bold="True" Width="100%" runat="server">[category]</asp:Label>
                </td>
                <td class="infoPanelCategoryBar" align="right" height="10">
                    <asp:Panel CssClass="infoPanelCategoryBar" ID="pictureLocation" runat="server">
                        <asp:Label ID="lblPicture" runat="server">[Picture]</asp:Label>&nbsp;/&nbsp;
                        <asp:Label ID="lblPictures" runat="server">[Pictures]</asp:Label>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td class="infoPanelCategoryBarFade" colspan="2" height="3">
                    <img height="3" src="Images/trans.gif">
                </td>
            </tr>
            <tr>
                <td valign="top" colspan="2">
                    <asp:Label ID="lblTitle" runat="server" CssClass="infoPanelTitle">[Title]</asp:Label><asp:Label
                        ID="lblPictureDate" runat="server" CssClass="infoPanelDate">[Date]</asp:Label><asp:DataList
                            ID="dlPerson" Width="100%" runat="server" CssClass="infoPanelText" RepeatDirection="Horizontal"
                            RepeatLayout="Flow">
                            <ItemTemplate>
                                <asp:Label ID="lblPersonFullName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FullName") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <SeparatorTemplate>
                                ,
                            </SeparatorTemplate>
                        </asp:DataList>
                    <hr noshade size="1">
                    <asp:Label ID="lblPictureDesc" runat="server" CssClass="infoPanelText">[Description]</asp:Label>
                </td>
            </tr>
            <tr>
                <td class="infoPanelLinkBarFade" colspan="2" height="3">
                    <img height="3" src="Images/trans.gif">
                </td>
            </tr>
            <tr>
                <td class="infoPanelLinkBar" colspan="2" height="18">
                    <table id="Table1" width="100%" border="0">
                        <tr>
                            <td width="33%">
                                <asp:HyperLink ID="lnkPrevious" runat="server" CssClass="infoPanelLink" Visible="False">
											<img src="Images/button_left.gif" alt="Previous Picture" border="0">
                                </asp:HyperLink>
                            </td>
                            <td align="middle" width="34%">
                                <asp:HyperLink ID="lnkReturn" runat="server" CssClass="infoPanelLink">
											<img src="Images/button_return.gif" alt="Return to list" border="0">
                                </asp:HyperLink>
                            </td>
                            <td>
                                <asp:Panel ID="editLinkPanel" runat="server">
                                </asp:Panel>
                            </td>
                            <td align="right" width="33%" valign="center">
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td height="7">
                                            <asp:Label ID="nextBarNote" runat="server" Style="font-size: 6pt" Visible="False">
														Next in:
                                            </asp:Label>
                                        </td>
                                        <td rowspan="2">
                                            <asp:HyperLink ID="lnkNext" runat="server" CssClass="infoPanelLink" Visible="False">
														<img src="Images/button_right.gif" alt="Next Picture" border="0">
                                            </asp:HyperLink>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="5">
                                            <asp:Panel ID="panelNext" runat="server" CssClass="position: absolute">
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>

    <script language="javascript"><!--
			
			// Move the picture div off the border if we have room
			if (screen.height > 1000)
			{
				picture.style.top	= 20;
				picture.style.left	= 20;
			}

		// --> </script>

</body>
</html>
