<%@ Register TagPrefix="picctls" Namespace="pics.Controls" Assembly="pics" %>
<%@ Register TagPrefix="pics" TagName="header" Src="Controls/_header.ascx" %>
<%@ Page language="c#" Codebehind="SearchCriteria1.aspx.cs" AutoEventWireup="false" Inherits="pics.SearchCriteria1" smartNavigation="True" %>
<%@ Register TagPrefix="iewc" Namespace="Microsoft.Web.UI.WebControls" Assembly="Microsoft.Web.UI.WebControls" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>msn2.net</title>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="msn2.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body text="#ffffff" vLink="#ffff99" aLink="#ffcc99" link="#ffff00" bgColor="#650d00" leftMargin="0" topMargin="0" marginwidth="0" marginheight="0">
		<!-- top table with MSN2 logo -->
		<pics:header id="ctlHeader" runat="server" header="Pictures" size="small">
		</pics:header>
		<form id="SearchCriteria" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%" align="left">
				<tr>
					<td>
						<!-- Main content -->
						<table width="100%">
							<tr>
								<td width="50">
									&nbsp;
								</td>
								<td align="left">
									<p>
									</p>
									<p>
										You can specify how you would like to search for pictures below. Fill in any of 
										the fields below. For Name and Description you can enter a part of a name.
									</p>
									<P>
										<asp:Label ID="noResults" Runat="server" Visible="False" CssClass="err">There were no results found for your search below.  Please change your search and try again.<br><br></asp:Label>
									</P>
									<iewc:TabStrip id="SearchTabs" runat="server" Style="FONT-WEIGHT:bold" TabDefaultStyle="border:solid 1px black;background:#dddddd;padding-left:5px;padding-right:5px;" TabHoverStyle="color:red" TabSelectedStyle="border:solid 1px black;border-bottom:none;background:white;padding-left:5px;padding-right:5px;" SepDefaultStyle="border-bottom:solid 1px #000000;" TargetID="SearchPager">
										<iewc:Tab Text="Description" />
										<iewc:TabSeparator />
										<iewc:Tab Text="Picture Date" />
										<iewc:TabSeparator />
										<iewc:Tab Text="People" />
									</iewc:TabStrip>
									<iewc:MultiPage id="SearchPager" runat="server" CssClass="tabBody">
										<iewc:PageView>
											<p>
												You can search for pictures based on the picture description.
											</p>
											<blockquote>
												<p>
													Description:
													<asp:textbox id="description" Runat="server" Width="200px"></asp:textbox>
												</p>
											</blockquote>
										</iewc:PageView>
										<iewc:PageView>
											<p>
												You can search for pictures based on the date of the picture.
											</p>
											<blockquote>
												<p>
													<table style="COLOR: black">
														<tr>
															<td align="right">
																Between
															</td>
															<td>
																<asp:textbox id="pictureDateStart" Runat="server" Width="100px"></asp:textbox>
															</td>
															<td>
																<asp:Label ID="pictureDateStartBad" Runat="server" Visible="False" CssClass="err">The start date entered must be in the format mm/dd/yy.</asp:Label>
															</td>
														</tr>
														<tr>
															<td align="right">
																and
															</td>
															<td>
																<asp:textbox id="pictureDateEnd" Runat="server" Width="100px"></asp:textbox>
															</td>
															<td>
																<asp:Label ID="pictureDateEndBad" Runat="server" Visible="False" CssClass="err">The end date entered must be in the format mm/dd/yy.</asp:Label>
															</td>
														</tr>
													</table>
												</p>
											</blockquote>
										</iewc:PageView>
										<iewc:PageView>
											<p>
												You can search for pictures based on the people in the picture. First, enter 
												any part of the person's name. Then click 'Find'. This will show anyone with 
												similiar names. Select the person or persons you would like to include, and 
												click 'Add'. You can repeat the process to add as many names as you wish.
											</p>
											<picctls:PeopleSelector id="peopleSelector" runat="server">
											</picctls:PeopleSelector>
											<p>
												<asp:RadioButtonList ID="personSearchOption" Runat="server" CssClass="blacktext">
													<asp:ListItem Selected value="0" Text="Search for pictures with one or more of the people listed above in the picture"></asp:ListItem>
													<asp:ListItem Value="1" Text="Search for pictures with all of the people listed above in the picture"></asp:ListItem>
												</asp:RadioButtonList>
											</p>
										</iewc:PageView>
									</iewc:MultiPage>
									<asp:Button ID="search" Runat="server" Text="Search" CssClass="btn"></asp:Button>
									<asp:Button ID="reset" Runat="server" Text="Reset" CssClass="btn"></asp:Button>
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