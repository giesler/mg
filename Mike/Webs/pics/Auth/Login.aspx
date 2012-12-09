<%@ Page language="c#" Codebehind="Login.aspx.cs" AutoEventWireup="false" Inherits="pics.auth.Login" %>
<%@ Register TagPrefix="pics" TagName="header" Src="../Controls/_header.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>msn2.net</title>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK rel="stylesheet" type="text/css" href="../msn2.css">
		<script language="javascript">
			function setLoginFocus() {
				if (document.Login.txtEmail)
					document.Login.txtEmail.focus();
			}
		</script>
	</HEAD>
	<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" bgcolor="#650d00" text="#ffffff" link="#ffff00" vlink="#ffff99" alink="#ffcc99" onload="setLoginFocus();">
		<!-- top table with MSN2 logo -->
		<pics:header id="ctlHeader" runat="server" size="small" header="Pictures - Login">
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
									<p>
									</p>
									<p>
										In order to view pictures on this site, you must login. By enabling login, we 
										can show people's names and picture descriptions, and search engines and such 
										will not find them. This way, anyone not wanting their names on a public 
										website don't have to worry.
									</p>
									<p>
										<b>If you have never logged in here below, please click
											<asp:HyperLink Runat="server" ID="lnkNewLogin" NavigateUrl="NewLogin.aspx">here</asp:HyperLink>
										</b>
									</p>
									<asp:Panel Runat="server" ID="pnlBadLogin" Visible="False" Width="100%">
										<P class="err">
											The email or password entered was not correct.
											<BR>
											If you forgot your password, click
											<asp:HyperLink id="lnkForgotPassword" Runat="server">here</asp:HyperLink>
											.
											<BR>
											<BR>
										</P>
									</asp:Panel>
									<table class="logintable" cellpadding="5" cellspacing="0">
										<tr>
											<td colspan="2" bgcolor="gray">
												<b>Login</b>
											</td>
										</tr>
										<tr bgcolor="white" style="COLOR: black">
											<td>
												Email:
											</td>
											<td>
												<asp:TextBox Runat="server" ID="txtEmail" Width="175px"></asp:TextBox>
											</td>
										</tr>
										<tr bgcolor="white" style="COLOR: black">
											<td>
												Password:
											</td>
											<td>
												<asp:TextBox Runat="server" ID="txtPassword" TextMode="Password" Width="175px"></asp:TextBox>
											</td>
										</tr>
										<tr bgcolor="white" style="COLOR: black">
											<td colspan="2">
												<asp:CheckBox Runat="server" ID="chkSave" Text="Save my login on this computer"></asp:CheckBox>
											</td>
										</tr>
										<tr bgcolor="white" style="COLOR: black">
											<td colspan="2" align="right">
												<asp:Button Runat="server" ID="btnLogin" Text=" Login " CssClass="btn"></asp:Button>
											</td>
										</tr>
									</table>
									<P>
										<EM><FONT size="2"><img src="../Images/ie.jpg" align="absMiddle"> This site looks and 
												works best with Internet Explorer 6.0, and with your monitor set to at least 
												800x600 resolution.</FONT></EM>
									</P>
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
