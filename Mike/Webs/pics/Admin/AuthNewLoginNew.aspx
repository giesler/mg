<%@ Page language="c#" Codebehind="AuthNewLoginNew.aspx.cs" AutoEventWireup="false" Inherits="pics.Admin.AuthNewLoginNew" %>
<%@ Register TagPrefix="pics" TagName="header" Src="../Controls/_header.ascx" %>
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
									<asp:Panel ID="pnlNewUser" Runat="server" Width="100%">
										<P>
											Enter the details of the new person here.
										</P>
										<TABLE>
											<TR>
												<TD>
													<TABLE class="logintable" cellSpacing="0" cellPadding="5">
														<TR>
															<TD bgColor="gray" colSpan="2">
																<B>New Login</B>
															</TD>
														</TR>
														<TR style="COLOR: black" bgColor="white">
															<TD>
																First Name:
																<asp:RequiredFieldValidator id="Requiredfieldvalidator1" Runat="server" CssClass="err" ErrorMessage="First name is required." ControlToValidate="txtFirstName" Display="Dynamic">*</asp:RequiredFieldValidator>
															</TD>
															<TD>
																<asp:TextBox id="txtFirstName" Width="175px" Runat="server"></asp:TextBox>
															</TD>
														</TR>
														<TR style="COLOR: black" bgColor="white">
															<TD>
																Last Name:
																<asp:RequiredFieldValidator id="Requiredfieldvalidator2" Runat="server" CssClass="err" ErrorMessage="Last name is required." ControlToValidate="txtLastName" Display="Dynamic">*</asp:RequiredFieldValidator>
															</TD>
															<TD>
																<asp:TextBox id="txtLastName" Width="175px" Runat="server"></asp:TextBox>
															</TD>
														</TR>
														<TR style="COLOR: black" bgColor="white">
															<TD>
																Full Name:
																<asp:RequiredFieldValidator id="NameValidator" Runat="server" CssClass="err" ErrorMessage="Name is required!" ControlToValidate="txtFullName" Display="Dynamic">*</asp:RequiredFieldValidator>
															</TD>
															<TD>
																<asp:TextBox id="txtFullName" Width="175px" Runat="server"></asp:TextBox>
															</TD>
														</TR>
														<TR style="COLOR: black" bgColor="white">
															<TD align="right" colSpan="2">
																<asp:Button id="btnOK" Runat="server" CssClass="btn" Text=" OK "></asp:Button>
															</TD>
														</TR>
													</TABLE>
												</TD>
												<TD>
													<asp:ValidationSummary id="ValidSummary" Runat="server" CssClass="err" DisplayMode="BulletList" HeaderText="You must enter valid values for the following fields:"></asp:ValidationSummary>
												</TD>
											</TR>
										</TABLE>
									</asp:Panel>
									<asp:Panel ID="pnlDone" Runat="server" Visible="False">
										<P>
											The user above has been added. An email was sent telling them they can now use 
											the website.
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
