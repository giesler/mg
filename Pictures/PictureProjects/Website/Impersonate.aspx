<%@ Page language="c#" Inherits="pics.Impersonate" Codebehind="Impersonate.aspx.cs" %>
<%@ Register TagPrefix="picctls" Namespace="pics.Controls" Assembly="WebControlLibrary" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>pics.msn2.net: Impersonate</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="msn2.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body leftMargin="0" topMargin="0">
		<form id="Form1" method="post" runat="server">
			<table height="100%" cellSpacing="0" cellPadding="0" width="100%" align="left" border="0">
				<tr>
					<td class="msn2headerfade" colSpan="4" height="3"><IMG height="3" src="images/blank.gif"></td>
				</tr>
				<tr>
					<td class="msn2sidebar" vAlign="top" width="125" rowSpan="3">
						<picctls:sidebar id="Sidebar1" runat="server"></picctls:sidebar>
						<picctls:ContentPanel id="picTasks" title="Picture Tasks" runat="server" visible="false" Width="100%"></picctls:ContentPanel>
						<picctls:PictureTasks id="picTaskList" runat="server" visible="true"></picctls:PictureTasks>
					</td>
				</tr>
				<tr height="25">
					<td>
						<table class="areaPanel" cellSpacing="0" cellPadding="4" width="100%" border="0">
							<tr>
								<td valign="top" width="16"><img src="Images/search.gif" width="16" height="16"></td>
								<td valign="top">
									<b>Add/Remove Viewers</b>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="msn2contentwindow" vAlign="top">
						If more then one person is looking at the pictures, select the other people's 
						names.
						<<br />>
						<picctls:PeopleSelector id="peopleSelector" runat="server" ForeColor="Black"></picctls:PeopleSelector>
						<<br />>
						<<br />>
						<asp:Button ID="search" Runat="server" Text="OK" CssClass="btn" onclick="search_Click"></asp:Button>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
