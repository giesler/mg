<%@ Register TagPrefix="pics" TagName="header" Src="../Controls/_header.ascx" %>
<%@ Page language="c#" Codebehind="ResetPassword.aspx.cs" AutoEventWireup="false" Inherits="pics.Auth.ResetPassword" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>msn2.net</title>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK rel="stylesheet" type="text/css" href="../msn2.css">
	</HEAD>
	<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" bgcolor="#650d00" text="#ffffff" link="#ffff00" vlink="#ffff99" alink="#ffcc99">
		<!-- top table with MSN2 logo -->
		<pics:header id="ctlHeader" runat="server" size="small" header="Pictures - Reset Password" ShowUserInfo="false">
		</pics:header>
		<form runat="server" id="Login" method="post">
			<table align="left" cellpadding="0" cellspacing="0" width="100%">
				<tr>
					<td>
						<!-- Main content -->
						<table width="100%">
							<tr>
								<td width="50">
									&nbsp;
								</td>
								<td align="left">
									<asp:Panel Runat="server" ID="pnlPassword">
										<P>
											You can reset your password by entering your new password below.
										</P>
										<asp:Label id="lblError" Runat="server" CssClass="err"></asp:Label>
										<TABLE class="logintable" cellSpacing="0" cellPadding="5">
											<TR>
												<TD bgColor="gray" colSpan="2">
													<B>Reset Password</B>
												</TD>
											</TR>
											<TR style="COLOR: black" bgColor="white">
												<TD>
													Email:
												</TD>
												<TD>
													<asp:Label id="lblEmail" Runat="server"></asp:Label>
												</TD>
											</TR>
											<TR style="COLOR: black" bgColor="white">
												<TD>
													New password:
												</TD>
												<TD>
													<asp:TextBox id="txtNewPassword" Runat="server" Width="175px" TextMode="Password"></asp:TextBox>
												</TD>
											</TR>
											<TR style="COLOR: black" bgColor="white">
												<TD>
													Confirm new password:
												</TD>
												<TD>
													<asp:TextBox id="txtConfirmNewPassword" Runat="server" Width="175px" TextMode="Password"></asp:TextBox>
												</TD>
											</TR>
											<TR style="COLOR: black" bgColor="white">
												<TD align="right" colSpan="2">
													<asp:Button id="btnOK" Runat="server" CssClass="btn" Text=" OK "></asp:Button>
												</TD>
											</TR>
										</TABLE>
									</asp:Panel>
									<asp:Panel Runat="server" ID="pnlChanged" Visible="False">
										<P>
											Your password has been changed.
										</P>
										<P>
											To login, click <A href="../">here</A>
										</P>
									</asp:Panel>
								</td>
							</tr>
						</table>
						<!-- Begin footer -->
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
