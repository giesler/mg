<%@ Register TagPrefix="picctls" Namespace="pics.Controls" %>
<%@ Page language="c#" Classname="pics.Auth.ResetPassword" CompileWith="ResetPassword.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>msn2.net</title>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK rel="stylesheet" type="text/css" href="../msn2.css">
		<LINK href="AuthStyles.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body topmargin="0" leftmargin="0">
		<!-- top table with MSN2 logo -->
		<form runat="server" id="Login" method="post">
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
						<asp:Panel Runat="server" ID="pnlPassword">
							<P>You can reset your password by entering your new password below.
							</P>
							<asp:Label id="lblError" Runat="server" CssClass="err"></asp:Label>
							<TABLE class="logintable" cellSpacing="0" cellPadding="5">
								<TR>
									<TD class="loginTableTitle" colSpan="2"><B>Reset Password</B>
									</TD>
								</TR>
								<TR class="loginTableContent">
									<TD class="loginTableText">Email:
									</TD>
									<TD class="loginTableText">
										<asp:Label id="lblEmail" Runat="server"></asp:Label></TD>
								</TR>
								<TR class="loginTableContent">
									<TD class="loginTableText">New password:
									</TD>
									<TD class="loginTableText">
										<asp:TextBox id="txtNewPassword" Runat="server" Width="175px" TextMode="Password"></asp:TextBox></TD>
								</TR>
								<TR class="loginTableContent">
									<TD class="loginTableText">Confirm new password:
									</TD>
									<TD class="loginTableText">
										<asp:TextBox id="txtConfirmNewPassword" Runat="server" Width="175px" TextMode="Password"></asp:TextBox></TD>
								</TR>
								<TR class="loginTableContent">
									<TD class="loginTableText" align="right" colSpan="2">
										<asp:Button id="btnOK" Text=" OK " Runat="server" CssClass="btn" Width="100px" onclick="btnOK_Click"></asp:Button></TD>
								</TR>
							</TABLE>
						</asp:Panel>
						<asp:Panel Runat="server" ID="pnlChanged" Visible="False">
							<P>Your password has been changed.
							</P>
							<P>To login, click
								<asp:HyperLink id="loginLink" Runat="server" Target="_top">here</asp:HyperLink></P>
						</asp:Panel>
						<!-- Begin footer -->
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
