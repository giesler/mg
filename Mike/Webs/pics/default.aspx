<%@ Register TagPrefix="picctls" Namespace="pics.Controls" Assembly="pics" %>
<%@ Page language="c#" Codebehind="default.aspx.cs" AutoEventWireup="false" Inherits="pics.Cdefault" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>msn2.net</title>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="msn2.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body leftMargin="0" topMargin="0">
		<!-- top table with MSN2 logo -->
		<form id="default" runat="server">
			<picctls:Header id="header" runat="server" size="small" Text="Pictures"></picctls:Header>
			<table height="100%" cellSpacing="0" cellPadding="0" width="100%" align="left" border="0">
				<tr>
					<td class="msn2headerfade" colSpan="3" height="3"><IMG height="3" src="images/blank.gif"></td>
				</tr>
				<tr>
					<td class="msn2sidebar" width="125" valign="top">
						<picctls:Sidebar id="Sidebar1" runat="server">
							<picctls:contentpanel id="searchPanel" title="Search" runat="server" visible="false" align="center" width="100%">
<asp:TextBox id="searchQuery" Runat="server" Width="100px"></asp:TextBox>&nbsp; 
      <BR>
<asp:Button id="search" Text=" Search " Runat="server" CssClass="smallButton"></asp:Button></picctls:contentpanel>
							<BR>
							<BR>
							<picctls:OpenMainFormLink id="mainFormLink" runat="server" visible="false"></picctls:OpenMainFormLink>
						</picctls:Sidebar>
					</td>
					<td class="msn2sidebarfade" width="4"></td>
					<td class="msn2contentwindow" valign="top">
						<!-- Main content -->
						<table cellPadding="4" width="95%" align="center">
							<tr>
								<td colspan="2">
									<picctls:contentpanel id="welcomeMessage" runat="server" title="Welcome to the MSN2 pictures website!" width="100%">
										<P>To view pictures, click one of the recently updated categories below, or enter a 
											name or description to search for on the left.
										</P>
									</picctls:contentpanel>
								</td>
							</tr>
							<tr>
								<td width="50%">
									<asp:Panel Runat="server" ID="recentPictures"></asp:Panel>
									<picctls:contentpanel id="contentRecentPictures" runat="server" title="What's New" Width="100%">
										<asp:datalist id="dlRecent" Runat="server" RepeatLayout="Flow">
											<HeaderTemplate>
											</HeaderTemplate>
											<FooterTemplate>
											</FooterTemplate>
											<ItemTemplate>
												<picctls:CategoryListViewItem CategoryId='<%# DataBinder.Eval(Container.DataItem, "CategoryID")%>' NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "CategoryID", "Categories.aspx?r=1&c={0}") %>' runat="server" FolderWidth="32" FolderImage="Images/folder64x64.png">
												</picctls:CategoryListViewItem>
											</ItemTemplate>
										</asp:datalist>
									</picctls:contentpanel>
								</td>
								<td vAlign="top" width="30%">
									<picctls:contentpanel id="browsePicturesContent" runat="server" title="Browse Pictures" Width="100%">
										<picctls:CategoryListViewItem id="rootCategory" runat="server" CategoryId="1" NavigateUrl="Categories.aspx" FolderWidth="64" FolderImage="Images/folder64x64.png"></picctls:CategoryListViewItem>
									</picctls:contentpanel>
									<br>
									<picctls:contentpanel id="contentRandomPicture" runat="server" title="Random Picture" Width="100%" Align="Center">
										<asp:panel id="randomPicture" Runat="server"></asp:panel>
									</picctls:contentpanel>
								</td>
							</tr>
						</table>
						<HR color="gainsboro" SIZE="1">
						<p>
							Have any comments on this site? Send them to <A href="mailto:mike@giesler.org">mike@giesler.org</A>.
						</p>
						<!-- Begin footer -->
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
