<%@ Page Language="c#" Inherits="pics.Admin.AuthNewLogin" Codebehind="AuthNewLogin.aspx.cs" %>
<%@ Register TagPrefix="picctls" Namespace="pics.Controls" Assembly="WebControlLibrary" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>msn2.net</title>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema" />
		<link href="../msn2.css" type="text/css" rel="stylesheet" />
	</HEAD>
	<body leftMargin="0" topMargin="0">
		<!-- top table with MSN2 logo -->
		<form id="Login" method="post" runat="server">
			<picctls:header id="header" runat="server" Text="Pictures - New Login" size="small"></picctls:header>
			<table height="100%" cellSpacing="0" cellPadding="0" width="100%" align="left" border="0">
				<tr>
					<td class="msn2headerfade" colSpan="3" height="3"><IMG height="3" src="../images/blank.gif"></td>
				</tr>
				<tr>
					<td class="msn2sidebar" vAlign="top" width="125"><picctls:sidebar id="Sidebar1" runat="server"></picctls:sidebar></td>
					<td class="msn2sidebarfade" width="4"></td>
					<td class="msn2contentwindow" vAlign="top">
						<!-- Main content --><asp:panel id="pnlNewLoginInfo" Width="100%" Runat="server">
							<table class="logintable" cellSpacing="0" cellPadding="5">
								<tr  class="loginTableContent">
									<td class="loginTableTitle" colSpan="2">New Login
									</td>
								</tr>
								<tr  class="loginTableContent">
									<td class="loginTableText">Name:
									</td>
									<td class="loginTableText">
										<asp:Label id="lblName" Width="175px" Runat="server"></asp:Label></td>
								</tr>
								<tr  class="loginTableContent">
									<td class="loginTableText">Email:
									</td>
									<td class="loginTableText">
										<asp:Label id="lblEmail" Width="175px" Runat="server"></asp:Label></td>
								</tr>
							</table>
						</asp:panel>
						<p></p>
						<asp:panel id="pnlPerson" Width="100%" Runat="server">
							<p>If you think the person above may already be a user on this site, you can search for 
								them here. If you think the person is a new user, click
								<asp:HyperLink id="lnkNewLogin" Runat="server">here</asp:HyperLink>.
							</p>
							<p>
								<picctls:PersonPicker id="PersonPicker" runat="server" onpersonselected="PersonPicker_PersonSelected"></picctls:PersonPicker>
								<asp:Label id="lblError" Runat="server" CssClass="err"></asp:Label></p>
							<asp:Panel id="afterPersonSelectContent" Runat="server" Visible="False">
								<p>Click 'Continue' to match this person.
									<asp:Button id="btnContinue" Text=" Continue " Runat="server" CssClass="btn" onclick="btnContinue_Click"></asp:Button></p>
							</asp:Panel>
						</asp:panel><asp:panel id="pnlDone" Runat="server" Visible="False">The 
      selected person's login has been activated. An email has also been sent. 
      </asp:panel>
						<!-- Begin footer --></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
