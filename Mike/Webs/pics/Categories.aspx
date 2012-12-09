<%@ Page language="c#" Codebehind="Categories.aspx.cs" AutoEventWireup="false" Inherits="pics.Categories" smartNavigation="True" %>
<%@ Register TagPrefix="pics" TagName="header" Src="Controls/_header.ascx" %>
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
	<body text="#ffffff" vLink="#ffff99" aLink="#ffcc99" link="#ffff00" bgColor="#650d00" leftMargin="0" topMargin="0">
		<pics:header id="ctlHeader" header="Pictures - By Category" size="small" runat="server"></pics:header>
		<form id="Form1" method="post" runat="server">
			<asp:panel id="youAreHerePanel" CssClass="note" Runat="server" Width="100%">You are here: </asp:panel>
			<hr color="gainsboro" SIZE="1">
			<table cellSpacing="2" cellPadding="0" width="100%" border="0" height="100%">
				<tr>
					<td style="BORDER-RIGHT: gainsboro thin solid" vAlign="top" width="100" rowSpan="3"><asp:panel id="childCategoryList" CssClass="note" Runat="server"><I>
								<asp:Label id="categoriesInLabel" Runat="server">Categories in<br></asp:Label>
								<asp:Label id="currentCategory" Runat="server">[current category]</asp:Label>:</I>
							<BR>
							<BR>
						</asp:panel></td>
					<td rowSpan="4">&nbsp;
					</td>
					<td vAlign="top" height="10%">
						<table cellSpacing="0" cellPadding="0" width="100%" border="0">
							<tr>
								<td><asp:label id="lblCurrentCategory" Runat="server" Font-Size="12" Font-Bold="true">Current Category Title</asp:label></td>
								<td align="right"><asp:hyperlink id="lnkSlideshow" Runat="server" Visible="False">Slideshow</asp:hyperlink></td>
							</tr>
							<tr>
								<td colSpan="2"><asp:label id="lblCategoryDesc" Runat="server"></asp:label>
									<hr color="gainsboro" SIZE="1">
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td vAlign="top" height="90%"><asp:panel id="pnlthumbs" Runat="server"></asp:panel></td>
				</tr>
			</table>
			<p></p>
		</form>
	</body>
</HTML>
