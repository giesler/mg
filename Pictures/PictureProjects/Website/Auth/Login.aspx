<%@ Page language="c#" Inherits="pics.auth.Login" Debug="true" Codebehind="Login.aspx.cs" %>
<%@ Register TagPrefix="picctls" Namespace="pics.Controls" Assembly="WebControlLibrary" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>msn2.net</title>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link href="../msn2.css" type="text/css" rel="stylesheet">
		<link href="AuthStyles.css" type="text/css" rel="stylesheet">
		<script language="javascript">
			function setLoginFocus() {
				if (document.all.email)
				{
					document.all.email.focus();
					if ( document.all.email.value != '' && document.all.password)
						document.all.password.focus();
				}
			}
		</script>
	</head>
	<body style="margin: 0px" onload="setLoginFocus();">
		<!-- top table with MSN2 logo -->
		<form method="post" runat="server">
			<picctls:Header id="header" runat="server" size="small" Text="Pictures - Sign In"></picctls:Header>
			<table height="100%" cellSpacing="0" cellPadding="0" width="100%" align="left" border="0">
				<tr>
					<td class="msn2headerfade" colSpan="3" height="3"><IMG height="3" src="../images/blank.gif"></td>
				</tr>
				<tr>
					<td class="msn2sidebar" width="125" valign="top">
						<picctls:Sidebar id="Sidebar1" runat="server"></picctls:Sidebar>
					</td>
					<td class="msn2sidebarfade" width="4"></td>
					<td class="msn2contentwindow" vAlign="top">
						<!-- Main content -->
						<table class="loginTable" id="loginTable" cellSpacing="0" cellPadding="5">
							<tr>
								<td class="loginTableTitle" colSpan="2">Sign In
								</td>
							</tr>
							<tr class="loginTableContent">
								<td class="loginTableText" colSpan="2"><asp:panel id="panelMessage" Runat="server">
										<P>In order to view pictures on this site, you must sign in.
											<<br />>
											<A href="why.aspx">Why require logging in?</A></P>
									</asp:panel></td>
							</tr>
							<tr class="loginTableContent">
								<td class="loginTableText" colSpan="2">What is your email address? <font class="loginTableNote">
										<<br />>
										If you have multiple addresses, choose the one you think we used for you. </font>
								</td>
							<tr class="loginTableContent">
								<td class="loginTableText" colSpan="2">
									<blockquote>Email:
										<asp:textbox id="email" Runat="server" Width="175px"></asp:textbox><asp:requiredfieldvalidator id="RequiredFieldValidator1" runat="server" Display="Dynamic" ControlToValidate="email" ErrorMessage="* Required"></asp:requiredfieldvalidator></blockquote>
								</td>
							</tr>
							<tr class="loginTableContent">
								<td class="loginTableText" colSpan="2">Do you have an MSN2 password?
								</td>
							<tr class="loginTableContent">
								<td class="loginTableText" colSpan="2">
									<blockquote><asp:radiobutton id="radioPassword" Runat="server" Checked="True" GroupName="havepassword" Text="Yes, my password is:"></asp:radiobutton><asp:textbox id="password" Runat="server" TextMode="Password"></asp:textbox><<br />>
										<asp:radiobutton id="radioNewLogin" Runat="server" GroupName="havepassword" Text="No, I don't"></asp:radiobutton><<br />>
										<asp:radiobutton id="radioHelpMe" Runat="server" GroupName="havepassword" Text="Maybe I do, but I don't remember it."></asp:radiobutton><<br />>
									</blockquote>
								</td>
							</tr>
							<tr class="loginTableContent">
								<td class="loginTableText" colSpan="2"><asp:checkbox id="chkSave" Runat="server" Text="Always keep me signed in on this computer"></asp:checkbox></td>
							</tr>
							<tr class="loginTableContent">
								<td class="loginTableText" align="right" colSpan="2"><asp:button id="btnLogin" CssClass="btn" Runat="server" Width="100px" Text="Continue >" onclick="btnLogin_Click"></asp:button></td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
			<picctls:ErrorMessagePanel id="pnlBadPassword" title="Incorrect Password" runat="server" visible="false"><B>
					The password you entered was not correct.</B> <<br />>If you 
forgot your password, click 
<asp:HyperLink id="lnkForgotPassword" Runat="server">here</asp:HyperLink>&nbsp;to 
find out how to change it. 
<<br />>
			</picctls:ErrorMessagePanel>
			<picctls:ErrorMessagePanel id="pnlBadEmail" title="Unrecognized Email" runat="server" visible="false"><B>
					The email address you entered was not recognized.</B> <<br />>Try another email address you may have, or if this 
					is the correct address, click 
<asp:HyperLink id="lnkNewLogin" Runat="server">here</asp:HyperLink>&nbsp;to set 
your password. <<br />>
			</picctls:ErrorMessagePanel>
			<script language="javascript"><!--
			function getPageCoords (elementId) {
				var el;
				if (document.all)
				el = document.all[elementId];
				else if (document.getElementById)
				el = document.getElementById(elementId);
				if (el) {
				var coords = {x: 0, y: 0};
				while (el) {
					coords.x += el.offsetLeft;
					coords.y += el.offsetTop;
					el = el.offsetParent;
				}
				return coords;
				}
				else
				return null;
			}
		
			var loginTableCoords	= getPageCoords('loginTable');

			// Position invalid password message	
			if (document.all.pnlBadPassword)
			{
				var pwdCoords			= getPageCoords ('password');
				
				document.all.pnlBadPassword.style.top		= pwdCoords.y + 'px';
				document.all.pnlBadPassword.style.left		= loginTableCoords.x + loginTable.clientWidth + 10;
			}
			
			// Position invalid email message
			if (document.all.pnlBadEmail)
			{
				var emailCoords			= getPageCoords ('email');
				
				document.all.pnlBadEmail.style.top			= emailCoords.y;
				document.all.pnlBadEmail.style.left			= loginTableCoords.x + loginTable.clientWidth + 10;
			}

				// --> </script>
			<!-- Begin footer --></form>
	</body>
</html>
