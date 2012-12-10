<%@ Register TagPrefix="picctls" Namespace="pics.Controls" %>
<%@ Page language="c#" Classname="pics.picview" CodeFile="picview.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="AutoTimer" Src="Controls/AutoTimer.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript (ECMAScript)" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<meta http-equiv=refresh content="<%= HttpRefreshURL %>">
		<LINK href="msn2.css" type="text/css" rel="stylesheet">
		<title>MSN2 Picture Viewer</title>
	</HEAD>
	<body class="pictureMode" leftMargin="0" topMargin="0">
		<form method="post" runat="server">
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
						<td vAlign="top" colSpan="2"><asp:label id="lblTitle" Runat="server" CssClass="infoPanelTitle">[Title]</asp:label><asp:label id="lblPictureDate" Runat="server" CssClass="infoPanelDate">[Date]</asp:label><asp:datalist id="dlPerson" Width="100%" Runat="server" CssClass="infoPanelText" RepeatDirection="Horizontal"
								RepeatLayout="Flow">
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
									<TD align="center" width="34%">
										<asp:hyperlink id="lnkReturn" Runat="server" CssClass="infoPanelLink">
											<img src="Images/button_return.gif" alt="Return to list" border="0">
										</asp:hyperlink>
									</TD>
									<TD align="right" width="33%" valign="middle">
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
			<asp:Panel Runat="server" ID="leftPanel" Visible="False">
				<DIV class="topInfoPanel">
					<TABLE class="infoPanelTable" cellSpacing="0">
						<TR>
							<TD class="infoPanelCategoryBar" height="10">Details</TD>
							<TD class="infoPanelCategoryBar" align="right" height="10"></TD>
						</TR>
						<TR>
							<TD class="infoPanelCategoryBarFade" colSpan="2" height="3"><IMG height="3" src="Images/trans.gif"></TD>
						</TR>
						<TR>
							<TD class="infoPanelText" vAlign="top" colSpan="2"><B>Categories</B><BR>
								<asp:datalist id="categoryList" Runat="server" Width="100%" CssClass="infoPanelText" RepeatLayout="Flow"
									RepeatDirection="Horizontal">
									<ItemTemplate>
										<asp:Label ID="Label6" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "CategoryName") %>'>
										</asp:Label>
									</ItemTemplate>
									<SeparatorTemplate>
										,
									</SeparatorTemplate>
								</asp:datalist>
								<HR noShade SIZE="1">
								<B>Groups</B><BR>
								<asp:datalist id="securityList" Runat="server" Width="100%" CssClass="infoPanelText" RepeatLayout="Flow"
									RepeatDirection="Horizontal">
									<ItemTemplate>
										<asp:Label ID="Label1" Runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "GroupName") %>'>
										</asp:Label>
									</ItemTemplate>
									<SeparatorTemplate>
										,
									</SeparatorTemplate>
								</asp:datalist>
								<HR noShade SIZE="1">
								<asp:Panel id="taskList" Runat="server"></asp:Panel></TD>
						</TR>
						<TR>
							<TD class="infoPanelLinkBarFade" colSpan="2" height="3"><IMG height="3" src="Images/trans.gif"></TD>
						</TR>
					</TABLE>
				</DIV>
			</asp:Panel>
			<asp:Panel Runat="server" ID="editLinkPanel"></asp:Panel>
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
