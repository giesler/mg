<%@ Register TagPrefix="pics" TagName="sidebar" Src="../Controls/_sidebar.ascx" %>
<%@ Register TagPrefix="pics" TagName="header" Src="../Controls/_header.ascx" %>
<%@ Page language="c#" Codebehind="ForgotPassword.aspx.cs" AutoEventWireup="false" Inherits="pics.Auth.ForgotPassword" %>
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
			<pics:header id="ctlHeader" header="Pictures - Sign In" size="small" runat="server"></pics:header>
			<table height="100%" cellSpacing="0" cellPadding="0" width="100%" align="left" border="0">
				<tr>
					<td height="3" class="msn2headerfade" colspan="3"><IMG height="3" src="images/blank.gif"></td>
				</tr>
				<tr>
					<td class="msn2sidebar" width="125"><pics:sidebar id="Sidebar1" runat="server"></pics:sidebar></td>
					<td class="msn2sidebarfade" width="4"></td>
					<td class="msn2contentwindow" vAlign="top">
						<!-- Main content -->
						<asp:Panel Runat="server" ID="pnlConfirm">
							<P>You can easily reset your password.
							</P>
							<UL>
								<LI>
								Confirm the email address is correct below.
								<LI>
								Click 'Confirm'
								<LI>
									After clicking confirm you will be sent an email with a link you can click to 
									set a new password.
								</LI>
							</UL>
							<TABLE class="loginTable" cellSpacing="0" cellPadding="5">
								<TR class="loginTableContent">
									<TD class="loginTableTitle"><B>Email</B>
									</TD>
								</TR>
								<TR class="loginTableContent">
									<TD class="loginTableText">
										<asp:TextBox id="txtEmail" Runat="server" Width="175px"></asp:TextBox></TD>
								</TR>
								<TR class="loginTableContent">
									<TD class="loginTableText" align="right">
										<asp:Button id="btnConfirm" Runat="server" Text="Confirm" CssClass="btn"></asp:Button></TD>
								</TR>
							</TABLE>
						</asp:Panel>
						<asp:panel id="pnlBadLogin" Width="225" Visible="False" Runat="server" Height="29px" CssClass="errorPanel">
							If you forgot your 
      password, click 
<asp:HyperLink id="lnkForgotPassword" Runat="server">here</asp:HyperLink>&nbsp;to 
							find out how to change your password.
						</asp:panel>
						<asp:Panel Runat="server" ID="pnlSent" Visible="False">
							<P>An email has been sent to <B>
									<asp:Label id="lblEmail" Runat="server"></asp:Label></B>. You should 
								receive it shortly.
							</P>
						</asp:Panel>
						<!-- Begin footer -->
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
