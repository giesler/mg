<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Page language="c#" debug="true" Inherits="pics.SearchRun" Codebehind="SearchRun.aspx.cs" %>
<%@ Register TagPrefix="picctls" Namespace="pics.Controls" Assembly="PicWeb" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<%=RedirectHeader%>
		<meta content="False" name="vs_showGrid">
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript (ECMAScript)" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="msn2.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body topmargin="0" leftmargin="0">
		<form id="Form1" method="post" runat="server">

			<picctls:Header id="header" runat="server" size="small" Text="Picture Folders"></picctls:Header>
			<table height="100%" cellSpacing="0" cellPadding="0" width="100%" align="left" border="0">
				<tr>
					<td class="msn2headerfade" colSpan="4" height="3"><IMG height="3" src="images/blank.gif"></td>
				</tr>
				<tr>
					<td class="msn2sidebar" vAlign="top" width="125" rowSpan="3">
						<picctls:Sidebar id="Sidebar1" runat="server">
							<picctls:ContentPanel id="picTasks" title="Picture Tasks" runat="server" visible="false" Width="100%">
								<picctls:PictureTasks id="picTaskList" runat="server" visible="true"></picctls:PictureTasks>
							</picctls:ContentPanel>
						</picctls:Sidebar>
					</td>
				</tr>
				<tr height="25">
					<td>
						<table class="areaPanel" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td valign="top" width="16"><img src="Images/search.gif" width="16" height="16" /></td>
								<td valign="top">
									<b>Searching...</b>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="msn2contentwindow" vAlign="top" style="MARGIN-TOP: 50px; MARGIN-LEFT: 50px">

									<picctls:ContentPanel id="welcomeMessage" runat="server" title="Please wait...">
										<P>
											Please wait while we look through pictures...
										</P>
									</picctls:ContentPanel>

					</td>
				</tr>
			</table>

		</form>
	</body>
</HTML>
