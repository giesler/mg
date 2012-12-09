<%@ Page language="c#" Codebehind="Categories.aspx.cs" AutoEventWireup="false" Inherits="pics.Categories" smartNavigation="True" %>
<%@ Register TagPrefix="pics" TagName="header" Src="Controls/_header.ascx" %>
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
	<body topmargin="0" leftmargin="0" bgcolor="#650d00" text="#ffffff" link="#ffff00" vlink="#ffff99" alink="#ffcc99">
		<pics:header id="ctlHeader" runat="server" size="small" header="Pictures - By Category">
		</pics:header>
		<form id="Form1" method="post" runat="server">
			<table cellSpacing="2" cellPadding="0" width="100%" border="0">
				<tr>
					<td width="10%" rowspan="3" valign="top">
						<asp:Panel ID="backPanel" Runat="server" CssClass="note">
							<I>Go to</I>
							<BR>
							<asp:HyperLink id="parentCategory" Runat="server">Categories</asp:HyperLink>
						</asp:Panel>
						<br>
						<br>
						<asp:Panel ID="childCategoryList" Runat="server" CssClass="note">
							<I>
								<asp:Label id="categoriesInLabel" Runat="server">Categories in<br></asp:Label>
								<asp:Label id="currentCategory" Runat="server">[current category]</asp:Label>
								:</I>
							<br>
							<br>
						</asp:Panel>
					</td>
					<td rowspan="4">
						&nbsp;
					</td>
					<td vAlign="top" height="10%">
						<table width="100%" cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td>
									<asp:label id="lblCurrentCategory" Runat="server" Font-Bold="true" Font-Size="12">Current Category Title</asp:label>
								</td>
								<td align="right">
									<asp:HyperLink ID="lnkSlideshow" Runat="server" Visible="False">Slideshow</asp:HyperLink>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td>
						<asp:label id="lblCategoryDesc" Runat="server"></asp:label>
						<hr color="gainsboro" size="1">
					</td>
				</tr>
				<tr>
					<td vAlign="top" height="90%">
						<asp:Panel Runat="server" ID="pnlthumbs"></asp:Panel>
					</td>
				</tr>
			</table>
			<p>
			</p>
		</form>
	</body>
</HTML>
