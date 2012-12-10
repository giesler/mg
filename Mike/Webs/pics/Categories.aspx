<%@ Register TagPrefix="pics" TagName="header" Src="Controls/_header.ascx" %>
<%@ Page language="c#" Codebehind="Categories.aspx.cs" AutoEventWireup="false" Inherits="pics.Categories" smartNavigation="True" %>
<%@ Register TagPrefix="picctls" Namespace="pics.Controls" Assembly="pics" %>
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
		<style>.areaPanel { PADDING-RIGHT: 2px; PADDING-LEFT: 2px; FILTER: progid:DXImageTransform.Microsoft.Gradient(GradientType=0, StartColorStr='#ffffff', EndColorStr='#DAD8D8'); PADDING-BOTTOM: 2px; PADDING-TOP: 2px; BORDER-BOTTOM: black 1px solid }
		</style>
	</HEAD>
	<body leftMargin="0" topMargin="0">
		<form id="Form1" method="post" runat="server">
			<pics:header id="ctlHeader" runat="server" header="Pictures" size="small"></pics:header>
			<table height="100%" cellSpacing="0" cellPadding="0" width="100%" align="left" border="0">
				<tr>
					<td class="msn2headerfade" colSpan="4" height="3"><IMG height="3" src="images/blank.gif"></td>
				</tr>
				<tr>
					<td class="msn2sidebar" vAlign="top" width="125" rowSpan="3">
						<picctls:sidebar id="Sidebar1" runat="server">
						</picctls:sidebar>
					</td>
				</tr>
				<tr height="25">
					<td>
						<table class="areaPanel" cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td>
									<asp:panel id="youAreHerePanel" Runat="server" CssClass="note" Width="100%"></asp:panel>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="msn2contentwindow" vAlign="top">
						<asp:panel id="childCategoryList" Runat="server" CssClass="note"></asp:panel>
						<asp:panel id="pnlthumbs" Runat="server"></asp:panel>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
