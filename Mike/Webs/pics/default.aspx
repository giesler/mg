<%@ Page language="c#" Codebehind="default.aspx.cs" AutoEventWireup="false" Inherits="pics.Cdefault" %>
<%@ Register TagPrefix="pics" TagName="header" Src="Controls/_header.ascx" %>
<%@ Register TagPrefix="pics" TagName="sidebar" Src="Controls/_sidebar.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>msn2.net</title>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="msn2.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body leftMargin="0" topMargin="0">
		<!-- top table with MSN2 logo -->
		<pics:header id="ctlHeader" header="Pictures" size="small" runat="server"></pics:header>
		<table cellSpacing="0" cellPadding="0" border="0" width="100%" align="left" height="100%">
			<tr>
				<td height="3" class="msn2headerfade" colspan="3"><img src="images/blank.gif" height="3"></td>
			</tr>
			<tr>
				<td width="125" class="msn2sidebar">
					<pics:sidebar runat="server" id="Sidebar1"></pics:sidebar>
				</td>
				<td width="4" class="msn2sidebarfade"></td>
				<td class="msn2contentwindow" valign="top">
					<!-- Main content -->
					<table width="100%">
						<tr>
							<td align="left">
								<p></p>
								<p><b>Welcome to the MSN2 Pictures site.</b>
								</p>
								<P><B>Tip</B>: Press F11 to switch Internet Explorer to full screen mode - you 
									won't have to scroll as much then. (Switch back to normal mode by hitting F11 
									again.)
								</P>
								<P>
									<HR color="gainsboro" SIZE="1">
								</P>
								<p><STRONG>You can view pictures in one of two ways:</STRONG>
								</p>
								<UL>
									<li>
										<A href="categories.aspx">View by Category</A>
									- show all pictures sorted by category - similiar to an 'Album' type view.
									<LI>
										<A href="SearchCriteria.aspx">Search</A> - allows you to search for pictures by 
										date, description, or people in the picture
									</LI>
								</UL>
								<P></P>
								<HR color="gainsboro" SIZE="1">
								<table width="95%" align="center" cellpadding="8">
									<tr>
										<td vAlign="top" width="30%">
											<p><strong>Random Picture</strong>
											</p>
											<asp:panel id="randomPicture" Runat="server"></asp:panel></td>
										<td style="BORDER-RIGHT: gainsboro thin solid" width="10%">&nbsp;</td>
										<td vAlign="top" width="60%">
											<asp:datalist id="dlRecent" Runat="server" RepeatLayout="Flow">
												<HeaderTemplate>
													<p>
														<STRONG>Recently added/updated categories... </STRONG>
														<ul>
												</HeaderTemplate>
												<FooterTemplate>
													</ul> </p>
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
	</body>
</HTML>
