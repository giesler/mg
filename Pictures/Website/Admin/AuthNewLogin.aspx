<%@ Register TagPrefix="picctls" Namespace="pics.Controls" %>
<%@ Page language="c#" Classname="pics.Admin.AuthNewLogin" CompileWith="AuthNewLogin.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>msn2.net</title>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../msn2.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body leftMargin="0" topMargin="0">
		<!-- top table with MSN2 logo -->
		<form id="Login" method="post" runat="server">
			<picctls:header id="header" runat="server" Text="Pictures - New Login" size="small"></picctls:header>
			<table height="100%" cellSpacing="0" cellPadding="0" width="100%" align="left" border="0">
				<tr>
					<td class="msn2headerfade" colSpan="3" height="3"><IMG height="3" src="images/blank.gif"></td>
				</tr>
				<tr>
					<td class="msn2sidebar" vAlign="top" width="125"><picctls:sidebar id="Sidebar1" runat="server"></picctls:sidebar></td>
					<td class="msn2sidebarfade" width="4"></td>
					<td class="msn2contentwindow" vAlign="top">
						<!-- Main content --><asp:panel id="pnlNewLoginInfo" Width="100%" Runat="server">
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
						</asp:panel>
						<p></p>
						<asp:panel id="pnlPerson" Width="100%" Runat="server">
							<P>If you think the person above may already be a user on this site, you can search for 
								them here. If you think the person is a new user, click
								<asp:HyperLink id="lnkNewLogin" Runat="server">here</asp:HyperLink>.
							</P>
							<P>
								<picctls:PersonPicker id="PersonPicker" runat="server" onpersonselected="PersonPicker_PersonSelected"></picctls:PersonPicker>
								<asp:Label id="lblError" Runat="server" CssClass="err"></asp:Label></P>
							<asp:Panel id="afterPersonSelectContent" Runat="server" Visible="False">
								<P>Click 'Continue' to match this person.
									<asp:Button id="btnContinue" Text=" Continue " Runat="server" CssClass="btn" onclick="btnContinue_Click"></asp:Button></P>
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
