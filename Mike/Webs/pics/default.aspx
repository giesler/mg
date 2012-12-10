<%@ Register TagPrefix="picctls" Namespace="pics.Controls" Assembly="pics" %>
<%@ Page language="c#" Codebehind="default.aspx.cs" AutoEventWireup="false" Inherits="pics.Cdefault" %>
<%@ Register TagPrefix="pics" TagName="header" Src="Controls/_header.ascx" %>
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
			<pics:header id="ctlHeader" runat="server" size="small" header="Pictures"></pics:header>
			<table height="100%" cellSpacing="0" cellPadding="0" width="100%" align="left" border="0">
				<tr>
					<td class="msn2headerfade" colSpan="3" height="3"><IMG height="3" src="images/blank.gif"></td>
				</tr>
				<tr>
					<td class="msn2sidebar" width="125" valign="top">
						<picctls:Sidebar id="Sidebar1" runat="server">
							<picctls:contentpanel id="searchPanel" title="Search" runat="server" width="100%" align="center">
<asp:TextBox id="searchQuery" Width="100px" Runat="server"></asp:TextBox>&nbsp; 
      <BR>
<asp:Button id="search" Runat="server" CssClass="smallButton" Text=" Search "></asp:Button></picctls:contentpanel>
						</picctls:Sidebar>
					</td>
					<td class="msn2sidebarfade" width="4"></td>
					<td class="msn2contentwindow" valign="top">
						<!-- Main content -->
						<table width="100%">
							<tr>
								<td align="left">
									<picctls:contentpanel id="welcomeMessage" runat="server" title="Welcome to the MSN2 pictures website!">
										<P>To view pictures, click one of the recently updated categories below, or enter a 
											name or description to search for on the left.
										</P>
									</picctls:contentpanel>
									<p>
										<picctls:CategoryListViewItem id="rootCategory" runat="server" CategoryId="1" NavigateUrl="Categories.aspx"></picctls:CategoryListViewItem>
									</p>
									<HR color="gainsboro" SIZE="1">
									<table cellPadding="8" width="95%" align="center">
										<tr>
											<td vAlign="top" width="30%">
												<p><strong>Random Picture</strong>
												</p>
												<asp:panel id="randomPicture" Runat="server"></asp:panel></td>
											<td style="BORDER-RIGHT: gainsboro thin solid" width="10%">&nbsp;</td>
											<td vAlign="top" width="60%"><asp:datalist id="dlRecent" Runat="server" RepeatLayout="Flow">
													<HeaderTemplate>
														<p>
															<STRONG>Recently added/updated categories... </STRONG>
														</p>
													</HeaderTemplate>
													<FooterTemplate>
													</FooterTemplate>
													<ItemTemplate>
														<li>
															<asp:HyperLink Runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "CategoryID", "Categories.aspx?r=1&c={0}") %>' ID="Hyperlink1" NAME="Hyperlink1">
																<%# DataBinder.Eval(Container.DataItem, "CategoryName") %>
															</asp:HyperLink>
															(<%# DataBinder.Eval(Container.DataItem, "RecentDate", "{0:d}") %>)
														</li>
													</ItemTemplate>
												</asp:datalist></td>
										</tr>
									</table>
									<HR color="gainsboro" SIZE="1">
									<p>Any comments on this site or pictures on the site, send to <A href="mailto:mike@giesler.org">
											mike@giesler.org</A>.
									</p>
								</td>
							</tr>
						</table>
						<!-- Begin footer -->
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
