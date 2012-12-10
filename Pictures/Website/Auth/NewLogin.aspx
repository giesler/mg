<%@ Register TagPrefix="picctls" Namespace="pics.Controls" %>
<%@ Page language="c#" Inherits="pics.Auth.NewLogin" CodeFile="NewLogin.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>msn2.net</title>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../msn2.css" type="text/css" rel="stylesheet">
		<LINK href="AuthStyles.css" type="text/css" rel="stylesheet">
		<script language="javascript">
			function setLoginFocus() { 
				if (document.all['txtName'])
					document.all['txtName'].focus();
				if (document.all['txtLookupEmail'])
					document.all['txtLookupEmail'].focus();
			}
		</script>
	</HEAD>
	<body leftMargin="0" topMargin="0" onload="setLoginFocus();">
		<!-- top table with MSN2 logo -->
		<form id="Login" method="post" runat="server">
			<picctls:Header id="header" runat="server" size="small" Text="Pictures - New Login"></picctls:Header>
			<table cellSpacing="0" cellPadding="0" border="0" width="100%" align="left" height="100%">
				<tr>
					<td height="3" class="msn2headerfade" colspan="3"><img src="../images/blank.gif" height="3"></td>
				</tr>
				<tr>
					<td class="msn2sidebar" width="125" valign="top">
						<picctls:Sidebar id="Sidebar1" runat="server"></picctls:Sidebar>
					</td>
					<td width="4" class="msn2sidebarfade"></td>
					<td class="msn2contentwindow" valign="top">
						<!-- Main content -->
						<asp:panel id="pnlEmailLookup" Runat="server" Width="100%">
							<P>Please enter your email address below. We may have already entered your name and 
								email address in the picture system, and you'll be able to simply pick a 
								password.
							</P>
							<TABLE>
								<TR>
									<TD>
										<TABLE class="logintable" cellSpacing="0" cellPadding="5">
											<TR>
												<TD class="loginTableTitle" colSpan="2">New Login</TD>
											</TR>
											<TR class="loginTableContent">
												<TD class="loginTableText">Email:
													<asp:RequiredFieldValidator id="Requiredfieldvalidator2" Runat="server" Display="Dynamic" ControlToValidate="txtLookupEmail" ErrorMessage="Email is required!" CssClass="err">*</asp:RequiredFieldValidator>
													<asp:RegularExpressionValidator id="Regularexpressionvalidator1" runat="server" Display="Static" ControlToValidate="txtLookupEmail" ErrorMessage="Email is not a valid email address.<br>Must follow name@host.domain format." ValidationExpression="^[\w-]+@[\w-]+\.(com|net|org|edu|mil)$" Font-Name="Arial" Font-Size="11">*</asp:RegularExpressionValidator></TD>
												<TD class="loginTableText">
													<asp:TextBox id="txtLookupEmail" Width="175px" Runat="server"></asp:TextBox></TD>
											</TR>
											<TR class="loginTableContent">
												<TD class="loginTableText" align="right" colSpan="2">
													<asp:Button id="btnEmailLookup" Text=" Lookup " Runat="server" CssClass="btn" onclick="btnEmailLookup_Click"></asp:Button></TD>
											</TR>
										</TABLE>
									</TD>
									<TD>
										<asp:ValidationSummary id="Validationsummary1" Runat="server" CssClass="err" HeaderText="You must enter valid values for the following fields:" DisplayMode="BulletList"></asp:ValidationSummary></TD>
								</TR>
							</TABLE>
						</asp:panel>
						<asp:panel id="pnlEmailFound" Runat="server" Visible="False" Width="100%">
							<P>Your email address -
								<asp:Label id="foundEmail" Runat="server">someone@somewhere.com</asp:Label>&nbsp;- 
								was found in the MSN2 system. An email has been sent to you with a link you can 
								use to set your password.
							</P>
							<P>When you receive the email, simply click the link in it to set your password, 
								then you'll be able to login.</P>
						</asp:panel>
						<asp:panel id="pnlNewLogin" Runat="server" Visible="False" Width="100%">
							<P>In order to view the pictures on this site you need to create a login.&nbsp; (<A href="why.aspx">Why?</A>)&nbsp; 
								Please fill out the form below. When you click 'Send', a request will be sent 
								to the person you select to activate your login. This way, we can confirm we 
								know the people using this site. We apologize for the invonvenience, but this 
								will protect the privacy of people on this site.
							</P>
							<P>When you fill out the below form, you can choose who to send the request to, if 
								you'd like. If only one of us knows you, select the person that knows you 
								below.
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
												<TD class="loginTableText">Your Full Name:
													<asp:RequiredFieldValidator id="NameValidator" Runat="server" Display="Dynamic" ControlToValidate="txtName" ErrorMessage="Name is required!" CssClass="err">*</asp:RequiredFieldValidator></TD>
												<TD class="loginTableText">
													<asp:TextBox id="txtName" Width="175px" Runat="server"></asp:TextBox></TD>
											</TR>
											<TR class="loginTableContent">
												<TD class="loginTableText">Email: (<A style="COLOR: black" href="NewLogin.aspx">change</A>)
												</TD>
												<TD class="loginTableText">
													<asp:Label id="lblEmail" Runat="server"></asp:Label></TD>
											</TR>
											<TR class="loginTableContent">
												<TD class="loginTableText">Password:
													<asp:RequiredFieldValidator id="PasswordValidator" Runat="server" Display="Dynamic" ControlToValidate="txtPassword" ErrorMessage="Password is required!" CssClass="err">*</asp:RequiredFieldValidator></TD>
												<TD class="loginTableText">
													<asp:TextBox id="txtPassword" Width="175px" Runat="server" TextMode="Password"></asp:TextBox></TD>
											</TR>
											<TR class="loginTableContent">
												<TD class="loginTableText">Confirm Password:
													<asp:RequiredFieldValidator id="ConfirmPasswordValidator" Runat="server" Display="Dynamic" ControlToValidate="txtConfirmPassword" ErrorMessage="Confirmation Password is required!" CssClass="err">*</asp:RequiredFieldValidator>
													<asp:CompareValidator id="CompareValidator1" Runat="server" Display="Dynamic" ControlToValidate="txtConfirmPassword" ErrorMessage="The confirmation password must match the password." CssClass="err" Operator="Equal" ControlToCompare="txtPassword">*</asp:CompareValidator></TD>
												<TD class="loginTableText">
													<asp:TextBox id="txtConfirmPassword" Width="175px" Runat="server" TextMode="Password"></asp:TextBox></TD>
											</TR>
											<TR class="loginTableContent">
												<TD class="loginTableText">Send request to:
												</TD>
												<TD class="loginTableText">
													<asp:DropDownList id="lstRequest" Width="175px" Runat="server">
														<asp:ListItem Text="Mike" Selected="true"></asp:ListItem>
														<asp:ListItem Text="Sara"></asp:ListItem>
														<asp:ListItem Text="Neil"></asp:ListItem>
														<asp:ListItem Text="Nick"></asp:ListItem>
													</asp:DropDownList></TD>
											</TR>
											<TR class="loginTableContent">
												<TD class="loginTableText" align="right" colSpan="2">
													<asp:Button id="btnSend" Text=" Send " Runat="server" CssClass="btn" onclick="btnSend_Click"></asp:Button></TD>
											</TR>
										</TABLE>
									</TD>
									<TD>
										<asp:ValidationSummary id="ValidSummary" Runat="server" CssClass="err" DisplayMode="List"></asp:ValidationSummary></TD>
								</TR>
							</TABLE>
						</asp:panel>
						<asp:panel id="pnlInfo" Runat="server" Visible="False" Width="100%">
							<P>Your new login request has been sent. You will receive an email when your 
								account has been activated.
							</P>
							<P>You can expect an email within a day or two.</P>
							<P>&nbsp;</P>
							<P>In the meantime - go <A href="http://www.google.com">surf the web</A>.</P>
						</asp:panel>
					</td>
				</tr>
			</table>
			<!-- Begin footer -->
		</form>
	</body>
</HTML>
