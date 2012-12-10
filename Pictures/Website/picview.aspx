<%@ Register TagPrefix="picctls" Namespace="pics.Controls" %>
<%@ Page Language="c#" ClassName="pics.picview" CodeFile="picview.aspx.cs" Debug="true" %>
<%@ Register TagPrefix="uc1" TagName="AutoTimer" Src="Controls/AutoTimer.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
    <meta content="C#" name="CODE_LANGUAGE">
    <meta content="JavaScript (ECMAScript)" name="vs_defaultClientScript">
    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    <meta http-equiv="refresh" content="<%= HttpRefreshURL %>">
    <link href="msn2.css" type="text/css" rel="stylesheet">
    <title>MSN2 Picture Viewer</title>
</head>
<body class="pictureMode" leftmargin="0" topmargin="0">
    <form method="post" runat="server">
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
                            <asp:Label ID="lblPicture" runat="server">[Picture]</asp:Label>
                            &nbsp;/&nbsp;
                            <asp:Label ID="lblPictures" runat="server">[Pictures]</asp:Label>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="infoPanelCategoryBarFade" colspan="2" height="3">
                        <img height="3" src="Images/trans.gif"></td>
                </tr>
                <tr>
                    <td valign="top" colspan="2">
                        <asp:Label ID="lblTitle" runat="server" CssClass="infoPanelTitle">[Title]</asp:Label>
                        <asp:Label ID="lblPictureDate" runat="server" CssClass="infoPanelDate">[Date]</asp:Label>
                        <asp:Label ID="pictureBy" runat="server" CssClass="infoPanelDate">[By]</asp:Label>
                        <asp:DataList ID="dlPerson" Width="100%" runat="server" CssClass="infoPanelText"
                            RepeatDirection="Horizontal" RepeatLayout="Flow">
                            <ItemTemplate>
                                <asp:Label ID="lblPersonFullName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FullName") %>'>
                                </asp:Label>
                            </ItemTemplate>
                            <SeparatorTemplate>
                                ,
                            </SeparatorTemplate>
                        </asp:DataList>
                        <hr noshade size="1" />
                        <asp:Label ID="lblPictureDesc" runat="server" CssClass="infoPanelText">[Description]</asp:Label>
                        <hr runat="server" id="descriptonBottomSplitter" noshade size="1" />
                    </td>
                </tr>
                <tr>
                    <td class="infoPanelText" colspan="2">
                        Rate it:
                        <asp:Image runat="server" ID="star1" AlternateText="1 star" ImageUrl="Images/starf.gif" />
                        <asp:Image runat="server" ID="star2" AlternateText="2 stars" ImageUrl="Images/starf.gif" />
                        <asp:Image runat="server" ID="star3" AlternateText="3 stars" ImageUrl="Images/starf.gif" />
                        <asp:Image runat="server" ID="star4" AlternateText="4 stars" ImageUrl="Images/starf.gif" />
                        <asp:Image runat="server" ID="star5" AlternateText="5 stars" ImageUrl="Images/starf.gif" />
                        &nbsp;<asp:Panel CssClass="inlinePanel" runat="server" ID="averageRating" />
                    </td>
                </tr>
                <tr>
                    <td class="infoPanelLinkBarFade" colspan="2" height="3">
                        <img height="3" src="Images/trans.gif"></td>
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
                                <td align="center" width="34%">
                                    <asp:HyperLink ID="lnkReturn" runat="server" CssClass="infoPanelLink">
											<img src="Images/button_return.gif" alt="Return to list" border="0">
                                    </asp:HyperLink>
                                </td>
                                <td align="right" width="33%" valign="middle">
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
        <asp:Panel runat="server" ID="leftPanel" Visible="False">
            <div class="topInfoPanel">
                <table class="infoPanelTable" cellspacing="0">
                    <tr>
                        <td class="infoPanelCategoryBar" height="10">
                            Details</td>
                        <td class="infoPanelCategoryBar" align="right" height="10">
                        </td>
                    </tr>
                    <tr>
                        <td class="infoPanelCategoryBarFade" colspan="2" height="3">
                            <img height="3" src="Images/trans.gif"></td>
                    </tr>
                    <tr>
                        <td class="infoPanelText" valign="top" colspan="2">
                            <b>Categories</b><br>
                            <asp:DataList ID="categoryList" runat="server" Width="100%" CssClass="infoPanelText"
                                RepeatLayout="Flow" RepeatDirection="Horizontal">
                                <ItemTemplate>
                                    <asp:Label ID="Label6" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CategoryName") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <SeparatorTemplate>
                                    ,
                                </SeparatorTemplate>
                            </asp:DataList>
                            <hr noshade size="1">
                            <b>Groups</b><br>
                            <asp:DataList ID="securityList" runat="server" Width="100%" CssClass="infoPanelText"
                                RepeatLayout="Flow" RepeatDirection="Horizontal">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "GroupName") %>'>
                                    </asp:Label>
                                </ItemTemplate>
                                <SeparatorTemplate>
                                    ,
                                </SeparatorTemplate>
                            </asp:DataList>
                            <hr noshade size="1">
                            <asp:Panel ID="taskList" runat="server">
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td class="infoPanelLinkBarFade" colspan="2" height="3">
                            <img height="3" src="Images/trans.gif"></td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="editLinkPanel">
        </asp:Panel>
    </form>
    <script language="javascript"><!--

			// Move the picture div off the border if we have room
			if (screen.height > 1000) {	picture.style.top = 20; picture.style.left = 20; }

            var star    = 'Images/star.gif';
            var starf   = 'Images/starf.gif';
            var setRating = 0;

            var vStar1  = document.getElementById('star1'); var vStar2  = document.getElementById('star2');
            var vStar3  = document.getElementById('star3'); var vStar4  = document.getElementById('star4');
            var vStar5  = document.getElementById('star5');

            vStar1.onmouseover = OnMouseOver1; vStar2.onmouseover = OnMouseOver2;
            vStar3.onmouseover = OnMouseOver3; vStar4.onmouseover = OnMouseOver4;
            vStar5.onmouseover = OnMouseOver5;

            vStar1.onmouseout = OnMouseOut; vStar2.onmouseout = OnMouseOut;
            vStar3.onmouseout = OnMouseOut; vStar4.onmouseout = OnMouseOut;
            vStar5.onmouseout = OnMouseOut;

            vStar1.onclick      = OnClick1; vStar2.onclick      = OnClick2;
            vStar3.onclick      = OnClick3; vStar4.onclick      = OnClick4;
            vStar5.onclick      = OnClick5;

            var curRating   = document.getElementById('ratingValue');
            setRating       = parseInt(curRating.value);
            SetStars(setRating);

            function SetStars(count)
            {
	            if (count == 0)	{
		            vStar5.src = starf;		vStar4.src = starf;
		            vStar3.src = starf;		vStar2.src = starf;
		            vStar1.src = starf;
	            } else if (count == 1) {
		            vStar5.src = starf;		vStar4.src = starf;
		            vStar3.src = starf;		vStar2.src = starf;
		            vStar1.src = star;
	            } else if (count == 2) {
		            vStar5.src = starf;		vStar4.src = starf;
		            vStar3.src = starf;		vStar2.src = star;
		            vStar1.src = star;
	            } else if (count == 3) {
		            vStar5.src = starf;		vStar4.src = starf;
		            vStar3.src = star;		vStar2.src = star;
		            vStar1.src = star;
	            } else if (count == 4) {
		            vStar5.src = starf;		vStar4.src = star;
		            vStar3.src = star;		vStar2.src = star;
		            vStar1.src = star;
	            } else if (count == 5) {
		            vStar5.src = star;		vStar4.src = star;
		            vStar3.src = star;		vStar2.src = star;
		            vStar1.src = star;
	            }
            }

            function OnMouseOver1() { SetStars(1); }
            function OnMouseOver2() { SetStars(2); }
            function OnMouseOver3() { SetStars(3); }
            function OnMouseOver4() { SetStars(4); }
            function OnMouseOver5() { SetStars(5); }
            function OnMouseOut()   { SetStars(setRating); }

            function OnClick1() { SaveRating(1); }
            function OnClick2() { SaveRating(2); }
            function OnClick3() { SaveRating(3); }
            function OnClick4() { SaveRating(4); }
            function OnClick5() { SaveRating(5); }

			function SaveRating(rating)
			{
				setRating = rating;
				curRating.Value = rating;
				<%= RatingServerCallbackFunction %>
			}

            function OnRatingSaved(returnText)
            {
	            var panel = document.getElementById('averageRating');
	            panel.innerText = returnText;
            }

		// --> </script>
</body>
</html>
