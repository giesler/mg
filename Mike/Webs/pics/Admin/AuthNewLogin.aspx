<%@ Register TagPrefix="picctls" Namespace="pics.Controls" Assembly="pics" %>
<%@ Register TagPrefix="pics" TagName="header" Src="../Controls/_header.ascx" %>
<%@ Page language="c#" Codebehind="AuthNewLogin.aspx.cs" AutoEventWireup="false" Inherits="pics.Admin.AuthNewLogin" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>msn2.net</title>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK rel="stylesheet" type="text/css" href="../msn2.css">
	</HEAD>
	<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0" bgcolor="#650d00" text="#ffffff" link="#ffff00" vlink="#ffff99" alink="#ffcc99">
		<!-- top table with MSN2 logo -->
		<pics:header id="ctlHeader" runat="server" size="small" header="Pictures - Authorize New Login">
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
									<asp:Panel ID="pnlNewLoginInfo" Runat="server" Width="100%">
										<TABLE class="logintable" cellSpacing="0" cellPadding="5">
											<TR>
												<TD bgColor="gray" colSpan="2">
													<B>New Login</B>
												</TD>
											</TR>
											<TR style="COLOR: black" bgColor="white">
												<TD>
													Name:
												</TD>
												<TD>
													<asp:Label id="lblName" Width="175px" Runat="server"></asp:Label>
												</TD>
											</TR>
											<TR style="COLOR: black" bgColor="white">
												<TD>
													Email:
												</TD>
												<TD>
													<asp:Label id="lblEmail" Width="175px" Runat="server"></asp:Label>
												</TD>
											</TR>
										</TABLE>
									</asp:Panel>
									<p>
									</p>
									<asp:Panel ID="pnlPerson" Runat="server" Width="100%">
										<P>
											If you think the person above may already be in the database, you can search 
											for them here. If you think the person is a new user, click
											<asp:HyperLink id="lnkNewLogin" Runat="server">here</asp:HyperLink>
											.
										</P>
										<P>
											<picctls:PersonPicker id="PersonPicker" runat="server">
											</picctls:PersonPicker>
											<asp:Label id="lblError" Runat="server" CssClass="err"></asp:Label>
										</P>
										<P>
											After you select the person that matches click continue.
											<asp:Button id="btnContinue" Runat="server" Text=" Continue " CssClass="btn"></asp:Button>
										</P>
									</asp:Panel>
									<asp:Panel ID="pnlDone" Runat="server" Visible="False">
										The selected person's login has been activated.  An email has also been sent.
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
