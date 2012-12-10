<%@ Page language="c#" Codebehind="SearchResults.aspx.cs" AutoEventWireup="false" Inherits="pics.SearchResults" %>
<%@ Register TagPrefix="picctls" Namespace="pics.Controls" Assembly="pics" %>
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
	<body topmargin="0" leftmargin="0">
		<picctls:Header id="header" runat="server" size="small" Text="Pictures Search Results"></picctls:Header>
		<form id="Form1" method="post" runat="server">
			<asp:panel id="youAreHerePanel" CssClass="note" Runat="server" Width="100%">Below are the results of your search.</asp:panel>
			<hr color="gainsboro" SIZE="1">
			<table cellSpacing="2" cellPadding="0" width="100%" border="0">
				<tr>
					<td style="BORDER-RIGHT: gainsboro thin solid" width="10%" rowspan="3" valign="top">
						<p class="note">
							Search:<br>
							<br>
							<asp:Label ID="searchDescription" Runat="server"></asp:Label>
							<br>
							<asp:HyperLink ID="ReturnToCriteria" Runat="server">(change)</asp:HyperLink>
						</p>
					</td>
					<td rowspan="4">
						&nbsp;
					</td>
					<td vAlign="top" height="10%">
						<table width="100%" cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td align="right">
									<asp:HyperLink ID="lnkSlideshow" Runat="server" Visible="False">Slideshow</asp:HyperLink>
								</td>
							</tr>
						</table>
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
