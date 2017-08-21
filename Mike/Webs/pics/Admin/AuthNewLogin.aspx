<%@ Page language="c#" Codebehind="AuthNewLogin.aspx.cs" AutoEventWireup="false" Inherits="pics.Admin.AuthNewLogin" %>
<%@ Register TagPrefix="picctls" Namespace="pics.Controls" Assembly="pics" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>msn2.net</title>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK rel="stylesheet" type="text/css" href="../msn2.css">
	</HEAD>
	<body leftMargin="0" topMargin="0">
		<!-- top table with MSN2 logo -->
		<form id="Login" method="post" runat="server">
			<picctls:Header id="header" runat="server" size="small" Text="Pictures - New Login"></picctls:Header>
			<table cellSpacing="0" cellPadding="0" border="0" width="100%" align="left" height="100%">
				<tr>
					<td height="3" class="msn2headerfade" colspan="3"><img src="images/blank.gif" height="3"></td>
				</tr>
				<tr>
					<td class="msn2sidebar" width="125" valign="top">
						<picctls:Sidebar id="Sidebar1" runat="server"></picctls:Sidebar>
					</td>
					<td width="4" class="msn2sidebarfade"></td>
					<td class="msn2contentwindow" valign="top">
						<!-- Main content -->
						<asp:Panel ID="pnlNewLoginInfo" Runat="server" Width="100%">
							<TABLE class="logintable" cellSpacing="0" cellPadding="5">
								<TR class="loginTableContent">
									<TD class="loginTableTitle" colSpan="2">New Login
									</TD>
								</TR>
								<TR class="loginTableContent">
									<TD class="loginTableText">Name:
									</TD>
									<TD class="loginTableText">
										<asp:Label id="lblName" Width="175px" Runat="server"></asp:Label></TD>
								</TR>
								<TR class="loginTableContent">
									<TD class="loginTableText">Email:
									</TD>
									<TD class="loginTableText">
										<asp:Label id="lblEmail" Width="175px" Runat="server"></asp:Label></TD>
								</TR>
							</TABLE>
						</asp:Panel>
						<p>
						</p>
						<asp:Panel ID="pnlPerson" Runat="server" Width="100%">
							<P>If you think the person above may already be in the database, you can search for 
								them here. If you think the person is a new user, click
								<asp:HyperLink id="lnkNewLogin" Runat="server">here</asp:HyperLink>.
							</P>
							<P>
								<picctls:PersonPicker id="PersonPicker" runat="server"></picctls:PersonPicker>
								<asp:Label id="lblError" Runat="server" CssClass="err"></asp:Label></P>
							<asp:Panel id="afterPersonSelectContent" Runat="server" Visible="False">
								<P>After you select the person that matches click continue.
									<asp:Button id="btnContinue" Runat="server" CssClass="btn" Text=" Continue "></asp:Button></P>
							</asp:Panel>
						</asp:Panel>
						<asp:Panel ID="pnlDone" Runat="server" Visible="False">
							The selected person's login has been activated.  An email has also been sent.
						</asp:Panel>
						<!-- Begin footer -->
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
