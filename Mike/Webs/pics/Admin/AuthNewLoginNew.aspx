<%@ Register TagPrefix="picctls" Namespace="pics.Controls" Assembly="pics" %>
<%@ Page language="c#" Codebehind="AuthNewLoginNew.aspx.cs" AutoEventWireup="false" Inherits="pics.Admin.AuthNewLoginNew" %>
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
						<asp:Panel ID="pnlNewUser" Runat="server" Width="100%">
							<P>Enter the details of the new person here.
							</P>
							<TABLE>
								<TR>
									<TD>
										<TABLE class="logintable" cellSpacing="0" cellPadding="5">
											<TR class="loginTableContent">
												<TD class="loginTableTitle" colSpan="2">New Login
												</TD>
											</TR>
											<TR class="loginTableContent">
												<TD class="loginTableText">First Name:
													<asp:RequiredFieldValidator id="Requiredfieldvalidator1" Runat="server" Display="Dynamic" ControlToValidate="txtFirstName" ErrorMessage="First name is required." CssClass="err">*</asp:RequiredFieldValidator></TD>
												<TD class="loginTableText">
													<asp:TextBox id="txtFirstName" Width="175px" Runat="server"></asp:TextBox></TD>
											</TR>
											<TR class="loginTableContent">
												<TD class="loginTableText">Last Name:
													<asp:RequiredFieldValidator id="Requiredfieldvalidator2" Runat="server" Display="Dynamic" ControlToValidate="txtLastName" ErrorMessage="Last name is required." CssClass="err">*</asp:RequiredFieldValidator></TD>
												<TD class="loginTableText">
													<asp:TextBox id="txtLastName" Width="175px" Runat="server"></asp:TextBox></TD>
											</TR>
											<TR class="loginTableContent">
												<TD class="loginTableText">Full Name:
													<asp:RequiredFieldValidator id="NameValidator" Runat="server" Display="Dynamic" ControlToValidate="txtFullName" ErrorMessage="Name is required!" CssClass="err">*</asp:RequiredFieldValidator></TD>
												<TD class="loginTableText">
													<asp:TextBox id="txtFullName" Width="175px" Runat="server"></asp:TextBox></TD>
											</TR>
											<TR class="loginTableContent">
												<TD class="loginTableText" align="right" colSpan="2">
													<asp:Button id="btnOK" Text=" OK " Runat="server" CssClass="btn"></asp:Button></TD>
											</TR>
										</TABLE>
									</TD>
									<TD>
										<asp:ValidationSummary id="ValidSummary" Runat="server" CssClass="err" HeaderText="You must enter valid values for the following fields:" DisplayMode="BulletList"></asp:ValidationSummary></TD>
								</TR>
							</TABLE>
						</asp:Panel>
						<asp:Panel ID="pnlDone" Runat="server" Visible="False">
							<P>The user above has been added. An email was sent telling them they can now use 
								the website.
							</P>
						</asp:Panel>
						<!-- Begin footer -->
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
