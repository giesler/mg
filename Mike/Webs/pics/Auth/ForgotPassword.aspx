<%@ Register TagPrefix="pics" TagName="header" Src="../Controls/_header.ascx" %>
<%@ Page language="c#" Codebehind="ForgotPassword.aspx.cs" AutoEventWireup="false" Inherits="pics.Auth.ForgotPassword" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>msn2.net</title>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK rel="stylesheet" type="text/css" href="../msn2.css">
	</HEAD>
	<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" bgcolor="#650d00" text="#ffffff" link="#ffff00" vlink="#ffff99" alink="#ffcc99">
		<!-- top table with MSN2 logo -->
		<pics:header id="ctlHeader" runat="server" size="small" header="Pictures - Forgot Password" ShowUserInfo="false">
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
									<asp:Panel Runat="server" ID="pnlConfirm">
										<P>
											Please confirm your email address below. You will receive an email that will 
											allow you to reset your password when you click 'Confirm'.
										</P>
										<TABLE class="logintable" cellSpacing="0" cellPadding="5">
											<TR>
												<TD bgColor="gray">
													<B>Email</B>
												</TD>
											</TR>
											<TR style="COLOR: black" bgColor="white">
												<TD>
													<asp:TextBox id="txtEmail" Runat="server" Width="175px"></asp:TextBox>
												</TD>
											</TR>
											<TR style="COLOR: black" bgColor="white">
												<TD align="right">
													<asp:Button id="btnConfirm" Runat="server" CssClass="btn" Text="Confirm"></asp:Button>
												</TD>
											</TR>
										</TABLE>
									</asp:Panel>
									<asp:Panel Runat="server" ID="pnlSent" Visible="False">
										<P>
											An email has been sent to <B>
												<asp:Label id="lblEmail" Runat="server"></asp:Label>
											</B>. You should receive it shortly.
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
