<%@ Page language="c#" Codebehind="picview.aspx.cs" AutoEventWireup="false" Inherits="pics.picview" %>
<%@ Register TagPrefix="pics" TagName="header" Src="Controls/_header.ascx" %>
<%@ Register TagPrefix="picctls" Namespace="pics.Controls" Assembly="pics" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript (ECMAScript)" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<meta http-equiv="refresh" content="<%= HttpRefreshURL %>">
		<LINK href="msn2.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body topmargin="0" leftmargin="0" class="pictureMode">
		<form id="picview" method="post" runat="server">
			<div class="infoPanel">
				<table cellspacing="0" class="infoPanelTable">
					<tr>
						<td class="infoPanelCategoryBar" height="10">
							<asp:Label ID="lblCategory" Runat="server" Width="100%" Font-Bold="True">[category]</asp:Label>
						</td>
						<td class="infoPanelCategoryBar" align="right" height="10">
							<asp:Label id="lblPicture" Runat="server">[Picture]</asp:Label>&nbsp;/&nbsp;<asp:Label id="lblPictures" runat="server">[Pictures]</asp:Label>
						</td>
					</tr>
					<tr>
						<td class="infoPanelCategoryBarFade" colspan="2" height="3"><img src="Images/trans.gif" height="3"></td>
					</tr>
					<tr>
						<td colspan="2" valign="top">
							<asp:Label ID="lblTitle" Runat="server" CssClass="infoPanelTitle">[Title]</asp:Label>
							<asp:Label ID="lblPictureDate" Runat="server" CssClass="infoPanelDate">[Date]</asp:Label>
							<asp:DataList id="dlPerson" Runat="server" Width="100%" RepeatLayout="Flow" RepeatDirection="Horizontal" CssClass="infoPanelText">
								<ItemTemplate>
									<asp:Label ID="lblPersonFullName" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FullName") %>'>
									</asp:Label>
								</ItemTemplate>
								<SeparatorTemplate>
									,
								</SeparatorTemplate>
							</asp:DataList>
							<hr size="1" noshade>
							<asp:Label id="lblPictureDesc" Runat="server" CssClass="infoPanelText">[Description]</asp:Label>
						</td>
					</tr>
					<tr>
						<td colspan="2" class="infoPanelLinkBarFade" height="3"><img src="Images/trans.gif" height="3"></td>
					</tr>
					<tr>
						<td class="infoPanelLinkBar" colspan="2" height="18">
							<asp:panel id="pnlPageControls" runat="server" Width="100%" BackColor="Transparent" CssClass="infoPanelText">
								<TABLE width="100%">
									<TR>
										<TD width="33%">
											<asp:HyperLink id="lnkPrevious" CssClass="infoPanelLink" Runat="server" Visible="False">
												<img src="Images/button_left.gif" alt="Previous Picture" border="0">
											</asp:HyperLink></TD>
										<TD align="middle" width="34%">
											<asp:hyperlink id="lnkReturn" CssClass="infoPanelLink" Runat="server">
												<img src="Images/button_return.gif" alt="Return to list" border="0">
											</asp:hyperlink></TD>
										<TD align="right" width="33%">
											<asp:HyperLink id="lnkNext" CssClass="infoPanelLink" Runat="server" Visible="False">
												<img src="Images/button_right.gif" alt="Next Picture" border="0">
											</asp:HyperLink></TD>
									</TR>
								</TABLE>
							</asp:panel>
						</td>
					</tr>
				</table>
			</div>
			<table border="0" cellpadding="2" cellspacing="0">
				<tr>
					<td runat="server" id="tdPicture" class="picviewPictureBorder">
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
