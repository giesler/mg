<%@ Page language="c#" Inherits="pics.SearchResults" Codebehind="SearchResults.aspx.cs" %>
<%@ Register TagPrefix="picctls" Namespace="pics.Controls" Assembly="WebControlLibrary" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<meta content="False" name="vs_showGrid">
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript (ECMAScript)" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="msn2.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body leftMargin="0" topMargin="0">
		<form id="Form1" method="post" runat="server">
			<picctls:header id="header" Text="Picture Folders" size="small" runat="server"></picctls:header>
			<table height="100%" cellSpacing="0" cellPadding="0" width="100%" align="left" border="0">
				<tr>
					<td class="msn2headerfade" colSpan="4" height="3"><IMG height="3" src="images/blank.gif"></td>
				</tr>
				<tr>
					<td class="msn2sidebar" vAlign="top" width="125" rowSpan="3"><picctls:sidebar id="Sidebar1" runat="server">
							<picctls:ContentPanel id="picTasks" title="Picture Tasks" runat="server" Width="100%" visible="false">
								<picctls:PictureTasks id="picTaskList" runat="server" visible="true"></picctls:PictureTasks>
							</picctls:ContentPanel>
							<picctls:ContentPanel id="Contentpanel1" title="Search" runat="server" Width="100%" visible="true">
								<asp:Label id="searchDescription" Runat="server"></asp:Label>
								<<br />>
								<asp:HyperLink id="ReturnToCriteria" Runat="server" NavigateUrl="SearchCriteria.aspx">New search...</asp:HyperLink>
							</picctls:ContentPanel>
						</picctls:sidebar></td>
				</tr>
				<tr height="25">
					<td>
						<table class="areaPanel" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td width="16" valign="top"><img src="Images/search.gif" width="16" height="16" /></td>
								<td valign="top">
									<b>Search Results</b>
									<<br />>
									<asp:label CssClass="categoryDesc" id="resultCount" Runat="server">[Result count]</asp:label>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="msn2contentwindow" vAlign="top"><asp:panel id="childCategoryList" Runat="server" CssClass="note"></asp:panel><asp:panel id="pnlthumbs" Runat="server"></asp:panel></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
