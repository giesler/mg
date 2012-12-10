<%@ Page language="c#" Inherits="pics.Cdefault" Codebehind="default.aspx.cs" %>
<%@ Register TagPrefix="picctls" Namespace="pics.Controls" Assembly="WebControlLibrary" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>pics.msn2.net: Home</title>
		<link href="msn2.css" type="text/css" rel="stylesheet" />
	</head>
	<body style="margin: 0px"> <!-- top table with MSN2 logo -->
		<form id="Form1" runat="server">
			<picctls:header id="header" runat="server" Text="Pictures" size="small"></picctls:header>
			<table height="100%" cellspacing="0" cellpadding="0" width="100%" align="left" border="0">
				<tr>
					<td class="msn2headerfade" colspan="3" height="3"><IMG height="3" src="images/blank.gif"></td>
				</tr>
				<tr>
					<td class="msn2sidebar" valign="top" width="125"><picctls:sidebar id="Sidebar1" runat="server">
							<picctls:contentpanel id="searchPanel" title="Search" runat="server" visible="false" width="100%">
<asp:TextBox id="searchQuery" Runat="server" Width="100px"></asp:TextBox>&nbsp; 
      <br />
<asp:Button id="search" Text=" Search " Runat="server" CssClass="smallButton"></asp:Button></picctls:contentpanel>
							<br />
							<br />
							<picctls:OpenMainFormLink id="mainFormLink" runat="server" visible="false"></picctls:OpenMainFormLink>
						</picctls:sidebar></td>
					<td class="msn2sidebarfade" width="4"></td>
					<td class="msn2contentwindow" vAlign="top"><!-- Main content -->
						<table cellpadding="4" border="0" width="100%" align="center">
							<tr>
								<t vAlign="top" width="50%">
									<asp:panel id="recentPictures" Runat="server"></asp:panel>
									<picctls:contentpanel id="contentRecentPictures" title="What's New" runat="server" Width="100%">
										<asp:datalist id="dlRecent" Runat="server" RepeatLayout="Flow">
											<HeaderTemplate>
											</HeaderTemplate>
											<FooterTemplate>
											</FooterTemplate>
											<ItemTemplate>
												<picctls:CategoryListViewItem CategoryId='<%# DataBinder.Eval(Container.DataItem, "Id")%>' NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "Id", "Categories.aspx?r=1&c={0}") %>' runat="server" FolderWidth="150" FolderImage="Images/folder.png">
												</picctls:CategoryListViewItem>
											</ItemTemplate>
										</asp:datalist>
									</picctls:contentpanel>
								</td>
								<td vAlign="top" width="30%">
									<picctls:contentpanel id="welcomeMessage" title="Welcome to the MSN2 pictures website!" runat="server"
										width="100%">
										<p>To view pictures, click one of the recently updated categories in the middle or 
											search or browse the pictures below.
										</p>
									</picctls:contentpanel>
									<br />
									<picctls:contentpanel id="browsePicturesContent" runat="server" title="Browse Pictures" Width="100%">
										<picctls:CategoryListViewItem id="rootCategory" runat="server" CategoryId="1" NavigateUrl="Categories.aspx" FolderWidth="64"
											FolderImage="Images/folder64x64.png"></picctls:CategoryListViewItem>
									</picctls:contentpanel>
									<br />
									<picctls:contentpanel id="searchContent" runat="server" title="Search Pictures" Width="100%">
										<table cellPadding="4" width="100%">
											<tr>
												<td vAlign="middle" align="center"><A href="SearchCriteria.aspx"><IMG height="32" src="Images/search.gif" width="32" border="0"></A></td>
												<td>
													<p><A class="categoryLink" href="SearchCriteria.aspx">Search</A><br />
														Search for pictures by date, description, or people in the picture.
													</p>
												</td>
											</tr>
										</table>
									</picctls:contentpanel>
									<br />
									<picctls:contentpanel id="contentRandomPicture" runat="server" title="Random Picture" Width="100%" Align="Center">
										<asp:panel id="randomPicture" Runat="server"></asp:panel>
									</picctls:contentpanel>
									<asp:panel id="adminMode" runat="server" width="100%" visible="false"></asp:panel>
								</td>
							</tr>
						</table>
						<HR color="gainsboro" SIZE="1">
						<p>
							Have any comments on this site? Send them to <A href="mailto:mike@giesler.org">mike@giesler.org</A>.
						</p>
						<!-- Begin footer --></td>
				</tr>
			</table>
		</form>
	</body>
</html>
