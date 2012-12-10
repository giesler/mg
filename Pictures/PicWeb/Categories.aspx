<%@ Page language="c#" smartNavigation="False" Inherits="pics.Categories" Codebehind="Categories.aspx.cs" %>
<%@ Register TagPrefix="picctls" Namespace="pics.Controls" Assembly="PicWeb" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>MSN2 Pictures</title>
		<meta content="False" name="vs_showGrid">
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript (ECMAScript)" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="msn2.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body leftMargin="0" topMargin="0">
		<form id="Form1" method="post" runat="server">
			<picctls:header id="header" runat="server" Text="Picture Folders" size="small"></picctls:header>
			<table height="100%" cellSpacing="0" cellPadding="0" width="100%" align="left" border="0">
				<tr>
					<td class="msn2headerfade" colSpan="4" height="3"><IMG height="3" src="images/blank.gif"></td>
				</tr>
				<tr>
					<td class="msn2sidebar" vAlign="top" width="125" rowSpan="3"><picctls:sidebar id="Sidebar1" runat="server">
							<picctls:ContentPanel id="picTasks" title="Picture Tasks" runat="server" visible="false" Width="100%">
								<picctls:PictureTasks id="picTaskList" runat="server" visible="true"></picctls:PictureTasks>
							</picctls:ContentPanel>
						</picctls:sidebar></td>
				</tr>
				<tr height="25">
					<td>
						<table class="areaPanel" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td><asp:panel id="youAreHerePanel" Width="100%" CssClass="note" Runat="server"></asp:panel></td>
							</tr>
							<tr>
							    <td>
							        <asp:Panel ID="toolbarPanel" Width="100%" CssClass="toolbar" Runat="server">
							        </asp:Panel>
							    </td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="msn2contentwindow" vAlign="top"><asp:panel id="childCategoryList" CssClass="note" Runat="server"></asp:panel><asp:panel id="pnlthumbs" Runat="server"></asp:panel></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
