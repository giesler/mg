<%@ Register TagPrefix="pics" TagName="header" Src="../Controls/_header.ascx" %>
<%@ Page language="c#" Codebehind="NewLogin.aspx.cs" AutoEventWireup="false" Inherits="pics.Auth.NewLogin" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>msn2.net</title>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../msn2.css" type="text/css" rel="stylesheet">
		<script language="javascript">
			function setLoginFocus() {
				if (document.Login.txtName)
					document.Login.txtName.focus();
				if (document.Login.txtLookupEmail)
					document.Login.txtLookupEmail.focus();
			}
		</script>
	</HEAD>
	<body text="#ffffff" vLink="#ffff99" aLink="#ffcc99" link="#ffff00" bgColor="#650d00" leftMargin="0" topMargin="0" onload="setLoginFocus();" marginwidth="0" marginheight="0">
		<!-- top table with MSN2 logo --><pics:header id="ctlHeader" header="Pictures - New Login" size="small" runat="server">
		</pics:header>
		<form id="Login" method="post" runat="server">
			<table cellSpacing="0" cellPadding="0" width="100%" align="left">
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
									<asp:panel id="pnlEmailLookup" Runat="server" Width="100%">
										<P>
											Please enter your email address below. We may have already entered your name 
											and email address in the picture system, and you'll be able to simply pick a 
											password.
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
																Email:
																<asp:RequiredFieldValidator id="Requiredfieldvalidator2" Runat="server" CssClass="err" ErrorMessage="Email is required!" ControlToValidate="txtLookupEmail" Display="Dynamic">*</asp:RequiredFieldValidator>
																<asp:RegularExpressionValidator id="Regularexpressionvalidator1" runat="server" ErrorMessage="Email is not a valid email address.<br>Must follow name@host.domain format." ControlToValidate="txtLookupEmail" Display="Static" Font-Size="11" Font-Name="Arial" ValidationExpression="^[\w-]+@[\w-]+\.(com|net|org|edu|mil)$">*</asp:RegularExpressionValidator>
															</TD>
															<TD>
																<asp:TextBox id="txtLookupEmail" Width="175px" Runat="server"></asp:TextBox>
															</TD>
														</TR>
														<TR style="COLOR: black" bgColor="white">
															<TD align="right" colSpan="2">
																<asp:Button id="btnEmailLookup" Runat="server" CssClass="btn" Text=" Lookup "></asp:Button>
															</TD>
														</TR>
													</TABLE>
												</TD>
												<TD>
													<asp:ValidationSummary id="Validationsummary1" Runat="server" CssClass="err" DisplayMode="BulletList" HeaderText="You must enter valid values for the following fields:"></asp:ValidationSummary>
												</TD>
											</TR>
										</TABLE>
									</asp:panel>
									<asp:panel id="pnlEmailFound" Runat="server" Visible="False" Width="100%">
										<P>
											Your email address was found. An email has been sent to you with a link you can 
											use to set your password.
										</P>
									</asp:panel>
									<asp:panel id="pnlNewLogin" Runat="server" Visible="False" Width="100%">
										<P>
											Your email address was not found. Please fill out the form below. When you 
											click 'Send', a request will be sent to the person you select to activate your 
											login. This way, we can confirm we know the people using this site. We 
											apologize for the invonvenience, but this will protect the privacy of people on 
											this site.
										</P>
										<P>
											If you have more than one email address, we may have entered a different email 
											address for you already. Click <A href="NewLogin.aspx">here</A> to try another 
											email address.
										</P>
										<P>
											You can choose who to send the request to, if you'd like. If only one of us 
											knows you, select the person that knows you below.
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
																Name:
																<asp:RequiredFieldValidator id="NameValidator" Runat="server" CssClass="err" ErrorMessage="Name is required!" ControlToValidate="txtName" Display="Dynamic">*</asp:RequiredFieldValidator>
															</TD>
															<TD>
																<asp:TextBox id="txtName" Width="175px" Runat="server"></asp:TextBox>
															</TD>
														</TR>
														<TR style="COLOR: black" bgColor="white">
															<TD>
																Email: (<A href="NewLogin.aspx" style="COLOR: black">change</A>)
															</TD>
															<TD>
																<asp:Label id="lblEmail" Runat="server"></asp:Label>
															</TD>
														</TR>
														<TR style="COLOR: black" bgColor="white">
															<TD>
																Password:
																<asp:RequiredFieldValidator id="PasswordValidator" Runat="server" CssClass="err" ErrorMessage="Password is required!" ControlToValidate="txtPassword" Display="Dynamic">*</asp:RequiredFieldValidator>
															</TD>
															<TD>
																<asp:TextBox id="txtPassword" Width="175px" Runat="server" TextMode="Password"></asp:TextBox>
															</TD>
														</TR>
														<TR style="COLOR: black" bgColor="white">
															<TD>
																Confirm Password:
																<asp:RequiredFieldValidator id="ConfirmPasswordValidator" Runat="server" CssClass="err" ErrorMessage="Confirmation Password is required!" ControlToValidate="txtConfirmPassword" Display="Dynamic">*</asp:RequiredFieldValidator>
																<asp:CompareValidator id="CompareValidator1" Runat="server" CssClass="err" ErrorMessage="The confirmation password must match the password." ControlToValidate="txtConfirmPassword" Display="Dynamic" ControlToCompare="txtPassword" Operator="Equal">*</asp:CompareValidator>
															</TD>
															<TD>
																<asp:TextBox id="txtConfirmPassword" Width="175px" Runat="server" TextMode="Password"></asp:TextBox>
															</TD>
														</TR>
														<TR style="COLOR: black" bgColor="white">
															<TD>
																Send request to:
															</TD>
															<TD>
																<asp:DropDownList id="lstRequest" Width="175px" Runat="server">
																	<asp:ListItem Text="Mike" Selected></asp:ListItem>
																	<asp:ListItem Text="Sara"></asp:ListItem>
																	<asp:ListItem Text="Neil"></asp:ListItem>
																	<asp:ListItem Text="Nick"></asp:ListItem>
																</asp:DropDownList>
															</TD>
														</TR>
														<TR style="COLOR: black" bgColor="white">
															<TD align="right" colSpan="2">
																<asp:Button id="btnSend" Runat="server" CssClass="btn" Text=" Send "></asp:Button>
															</TD>
														</TR>
													</TABLE>
												</TD>
												<TD>
													<asp:ValidationSummary id="ValidSummary" Runat="server" CssClass="err" DisplayMode="BulletList" HeaderText="You must enter valid values for the following fields:"></asp:ValidationSummary>
												</TD>
											</TR>
										</TABLE>
									</asp:panel>
									<asp:panel id="pnlInfo" Runat="server" Visible="False" Width="100%">
										<P>
											Your new login request has been sent. You will receive an email when your 
											account has been activated.
										</P>
									</asp:panel>
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
