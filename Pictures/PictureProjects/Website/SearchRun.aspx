<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Page language="c#" debug="true" Inherits="pics.SearchRun" Codebehind="SearchRun.aspx.cs" %>
<%@ Register TagPrefix="picctls" Namespace="pics.Controls" Assembly="WebControlLibrary" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<%=RedirectHeader%>
		<meta content="False" name="vs_showGrid">
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript (ECMAScript)" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="msn2.css" type="text/css" rel="stylesheet">
	</head>
	<body style="margin: 0px">
		<form id="Form1" method="post" runat="server">

			<picctls:Header id="header" runat="server" size="small" Text="Picture Folders"></picctls:Header>
			<table height="100%" cellspacing="0" cellpadding="0" width="100%" align="left" border="0">
				<tr>
					<td class="msn2headerfade" colspan="4" height="3"><img height="3" src="images/blank.gif" /></td>
				</tr>
				<tr>
					<td class="msn2sidebar" vAlign="top" width="125" rowSpan="3">
						<picctls:sidebar id="Sidebar1" runat="server">
							<picctls:ContentPanel id="picTasks" title="Picture Tasks" runat="server" visible="false" Width="100%">
								<picctls:PictureTasks id="picTaskList" runat="server" visible="true"></picctls:PictureTasks>
							</picctls:ContentPanel>
						</picctls:sidebar>
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

									<picctls:contentpanel id="welcomeMessage" runat="server" title="Please wait...">
										<P>
											Please wait while we look through pictures...
										</P>
									</picctls:contentpanel>

					</td>
				</tr>
			</table>

		</form>
	</body>
</html>
