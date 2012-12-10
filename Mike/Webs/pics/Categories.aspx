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
		<style>
			.areaPanel
			{
				filter:progid:DXImageTransform.Microsoft.Gradient(GradientType=0, StartColorStr='#DAD8D8', EndColorStr='#ffffff');
				border-bottom: black 1px solid;		
				padding-right: 2px;
				padding-left: 2px;
				padding-bottom: 2px;
				padding-top: 2px;
			}
		</style>
	</HEAD>
	<body leftMargin="0" topMargin="0">
		<form id="Form1" method="post" runat="server">
			<pics:header id="ctlHeader" runat="server" size="small" header="Pictures"></pics:header>
			<table height="100%" cellSpacing="0" cellPadding="0" width="100%" align="left" border="0">
				<tr>
					<td class="msn2headerfade" colSpan="4" height="3"><IMG height="3" src="images/blank.gif"></td>
				</tr>
				<tr>
					<td class="msn2sidebar" width="125" valign="top" rowspan="3">
						<picctls:Sidebar id="Sidebar1" runat="server">
							<asp:panel id="childCategoryList" CssClass="note" Runat="server">
								<I>
									<asp:Label id="categoriesInLabel" Runat="server">Categories in<br></asp:Label>
									<asp:Label id="currentCategory" Runat="server">[current category]</asp:Label>: </I><br>
							</asp:panel>
						</picctls:Sidebar>
					</td>
					<td class="fadeExtension" colspan="3" height="10">
						<asp:panel id="youAreHerePanel" CssClass="note" Runat="server" Width="100%">You are here: </asp:panel>
					</td>
				</tr>
				<tr height="25">
					<td>
						<table cellSpacing="0" cellPadding="0" width="100%" border="0" class="areaPanel">
							<tr>
								<td><asp:label id="lblCurrentCategory" Runat="server" Font-Size="12" Font-Bold="true">Current Category Title</asp:label></td>
								<td align="right"><asp:hyperlink id="lnkSlideshow" Runat="server" Visible="False">Slideshow</asp:hyperlink></td>
							</tr>
							<tr>
								<td colSpan="2">
									<asp:label id="lblCategoryDesc" Runat="server"></asp:label>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td class="msn2contentwindow" valign="top">
						<asp:panel id="pnlthumbs" Runat="server"></asp:panel>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
