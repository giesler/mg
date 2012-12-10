<%@ Register TagPrefix="uc1" TagName="AutoTimer" Src="Controls/AutoTimer.ascx" %>
<%@ Page language="c#" Inherits="pics.picviewss" CodeFile="picviewss.aspx.cs" %>
<%@ Register TagPrefix="picctls" Namespace="pics.Controls" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript (ECMAScript)" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<meta http-equiv=refresh content="<%= HttpRefreshURL %>">
		<LINK href="msn2.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body class="pictureMode" leftMargin="0" topMargin="0">
		<form id="picview" method="post" runat="server">
			<div id="picture" style="LEFT: 0px; POSITION: absolute; TOP: 0px">
				<table cellSpacing="0" cellPadding="2" border="0">
					<tr>
						<td class="picviewPictureBorder" id="tdPicture" runat="server"></td>
					</tr>
				</table>
			</div>
			<div class="infoPanel">
				<table class="infoPanelTable" cellSpacing="0">
					<tr>
						<td class="infoPanelCategoryBar" height="10"><asp:label id="lblCategory" Font-Bold="True" Width="100%" Runat="server">[category]</asp:label></td>
						<td class="infoPanelCategoryBar" align="right" height="10"><asp:panel CssClass="infoPanelCategoryBar" id="pictureLocation" Runat="server">
<asp:label id="lblPicture" Runat="server">[Picture]</asp:label>&nbsp;/&nbsp; 
<asp:label id="lblPictures" runat="server">[Pictures]</asp:label> 
      </asp:panel></td>
					</tr>
					<tr>
						<td class="infoPanelCategoryBarFade" colSpan="2" height="3"><IMG height="3" src="Images/trans.gif"></td>
					</tr>
					<tr>
						<td vAlign="top" colSpan="2"><asp:label id="lblTitle" Runat="server" CssClass="infoPanelTitle">[Title]</asp:label><asp:label id="lblPictureDate" Runat="server" CssClass="infoPanelDate">[Date]</asp:label><asp:datalist id="dlPerson" Width="100%" Runat="server" CssClass="infoPanelText" RepeatDirection="Horizontal" RepeatLayout="Flow">
								<ItemTemplate>
									<asp:Label ID="lblPersonFullName" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FullName") %>'>
									</asp:Label>
								</ItemTemplate>
								<SeparatorTemplate>
									,
								</SeparatorTemplate>
							</asp:datalist>
							<hr noShade SIZE="1">
							<asp:label id="lblPictureDesc" Runat="server" CssClass="infoPanelText">[Description]</asp:label></td>
					</tr>
					<tr>
						<td class="infoPanelLinkBarFade" colSpan="2" height="3"><IMG height="3" src="Images/trans.gif"></td>
					</tr>
					<tr>
						<td class="infoPanelLinkBar" colSpan="2" height="18">
							<TABLE id="Table1" width="100%" border="0">
								<TR>
									<TD width="33%"><asp:hyperlink id="lnkPrevious" Runat="server" CssClass="infoPanelLink" Visible="False">
											<img src="Images/button_left.gif" alt="Previous Picture" border="0">
										</asp:hyperlink></TD>
									<TD align="middle" width="34%">
										<asp:hyperlink id="lnkReturn" Runat="server" CssClass="infoPanelLink">
											<img src="Images/button_return.gif" alt="Return to list" border="0">
										</asp:hyperlink>
									</TD>
									<td>
										<asp:Panel ID="editLinkPanel" Runat="server"></asp:Panel>
									</td>
									<TD align="right" width="33%" valign="center">
										<table cellpadding="0" cellspacing="0" border="0">
											<tr>
												<td height="7">
													<asp:Label ID="nextBarNote" Runat="server" style="FONT-SIZE: 6pt" Visible="False">
														Next in:
													</asp:Label>
												</td>
												<td rowspan="2">
													<asp:HyperLink id="lnkNext" Runat="server" CssClass="infoPanelLink" Visible="False">
														<img src="Images/button_right.gif" alt="Next Picture" border="0">
													</asp:HyperLink>
												</td>
											</tr>
											<tr>
												<td height="5">
													<asp:Panel ID="panelNext" runat="server" CssClass="position: absolute"></asp:Panel>
												</td>
											</tr>
										</table>
									</TD>
								</TR>
							</TABLE>
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
</HTML>
