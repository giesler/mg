<%@ Register TagPrefix="picctls" Namespace="pics.Controls" %>
<%@ Page language="c#" smartNavigation="False" Inherits="pics.SearchCriteria" CodeFile="SearchCriteria.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>pics.msn2.net: Advanced Search</title>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="msn2.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body leftMargin="0" topMargin="0">
		<form method="post" runat="server">
			<picctls:Header id="header" runat="server" size="small" Text="Picture Folders"></picctls:Header>
			<table height="100%" cellSpacing="0" cellPadding="0" width="100%" align="left" border="0">
				<tr>
					<td class="msn2headerfade" colSpan="4" height="3"><IMG height="3" src="images/blank.gif"></td>
				</tr>
				<tr>
					<td class="msn2sidebar" vAlign="top" width="125" rowSpan="3">
						<picctls:sidebar id="Sidebar1" runat="server">
							<picctls:ContentPanel id="picTasks" title="Picture Tasks" runat="server" Width="100%" visible="false">
								<picctls:PictureTasks id="picTaskList" runat="server" visible="true"></picctls:PictureTasks>
							</picctls:ContentPanel>
						</picctls:sidebar>
					</td>
				</tr>
				<tr height="25">
					<td>
						<table class="areaPanel" cellSpacing="0" cellPadding="4" width="100%" border="0">
							<tr>
								<td valign="top" width="16"><img src="Images/search.gif" width="16" height="16" /></td>
								<td valign="top">
									<b>Advanced Search</b>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="msn2contentwindow" vAlign="top">
						<p>
							You can specify how you would like to search for pictures below. Fill in any of 
							the fields below. For Name and Description you can enter a part of a name.
						</p>
						<P>
							<asp:Label ID="noResults" Runat="server" Visible="False" CssClass="err">There were no results found for your search below.  Please change your search and try again.<br><br></asp:Label>
						</P>
						<p>
							<b>Picture Description</b>: You can search for pictures based on the picture 
							description.
						</p>
						<blockquote>
							<p>
								Description:
								<asp:textbox id="description" Runat="server" Width="200px"></asp:textbox>
							</p>
						</blockquote>
						<hr noshade>
						<p>
							<b>Picture Date</b>: You can search for pictures based on the date of the 
							picture.
						</p>
						<blockquote>
								<table>
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
						</blockquote>
						<hr noshade>
						<p>
							<b>People</b>: You can search for pictures based on the people in the picture. 
							First, enter any part of the person's name. Then click 'Find'. This will show 
							anyone with similiar names. Select the person or persons you would like to 
							include, and click 'Add'. You can repeat the process to add as many names as 
							you wish.
						</p>
						<picctls:PeopleSelector id="peopleSelector" runat="server" ForeColor="Black"></picctls:PeopleSelector>
						<p>
							<asp:RadioButtonList ID="personSearchOption" Runat="server" CssClass="blacktext" Visible="True">
								<asp:ListItem value="0" Text="Search for pictures with one or more of the people listed above in the picture"
									Selected="True" />
								<asp:ListItem Value="1" Text="Search for pictures with all of the people listed above in the picture" />
								<asp:ListItem Value="2" Text="Search for pictures with only the people listed above in the picture" />
							</asp:RadioButtonList>
						</p>
						<hr noshade>
						<asp:Button ID="search" Runat="server" Text="Search" CssClass="btn" onclick="search_Click"></asp:Button>
						<asp:Button ID="reset" Runat="server" Text="Reset" CssClass="btn" onclick="reset_Click"></asp:Button>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
