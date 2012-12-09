<%@ Register TagPrefix="pics" TagName="header" Src="Controls/_header.ascx" %>
<%@ Page language="c#" Codebehind="default.aspx.cs" AutoEventWireup="false" Inherits="pics.Cdefault" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<title>msn2.net</title>
		<LINK href="msn2.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body text="#ffffff" vLink="#ffff99" aLink="#ffcc99" link="#ffff00" bgColor="#650d00" leftMargin="0" topMargin="0" marginwidth="0" marginheight="0">
		<!-- top table with MSN2 logo --><pics:header id="ctlHeader" header="Pictures" size="small" runat="server"></pics:header>
		<table cellSpacing="0" cellPadding="0" width="100%" align="left">
			<tr>
				<td>
					<!-- Main content -->
					<table width="100%">
						<tr>
							<td width="50">&nbsp;
							</td>
							<td align="left">
								<p></p>
								<p><b>Welcome to the MSN2 Pictures site.</b>
								</p>
								<p>Over the next few months, expect to see more and more pictures added to this 
									site. <i>[I plan on adding all my pictures I have... eventually... -Mike]</i> And 
									if you run into any errors or have any problems using this site, let one of us 
									know. We plan on working on improving it to make it easier to use, and 
									suggestions are welcome.
								</p>
								<p><b>Tip</b>: Press F11 to switch Internet Explorer to full screen mode - you 
									won't have to scroll as much then. (Switch back to normal mode by hitting F11 
									again.)
								</p>
								<HR color="gainsboro" SIZE="1">
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
				</tr> </table> 
				<!-- Begin footer --> </td>
			</tr>
		</table>
	</body>
</HTML>
