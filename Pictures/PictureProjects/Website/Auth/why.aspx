<%@ Page language="c#" Inherits="pics.Auth.why" Codebehind="why.aspx.cs" %>
<%@ Register TagPrefix="picctls" Namespace="pics.Controls" Assembly="WebControlLibrary" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>why require logging in?</title>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../msn2.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body leftMargin="0" topMargin="0">
		<!-- top table with MSN2 logo -->
		<picctls:Header id="header" runat="server" size="small" Text="Why require logging in?"></picctls:Header>
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
					<p></p>
					<p>
						<b>Security</b>
					</p>
					<p>
						Any content on the internet is accessible to just about anyone if it doesn't 
						have password protection.
					</p>
					<p>
						<b>Personalization</b>
					</p>
					<p>By knowing who you are, this site can show you appropriate things.&nbsp; For 
						example, people in my family will see family pictures on the main page.</p>
					<p>&nbsp;</p>
					<p><STRONG>And a bit on privacy...</STRONG></p>
					<p>The information stored about you that you enter on this site, including your 
						email address, are stored on a server at my house.&nbsp; I keep the server up 
						to date and secure.</p>
					<p>
						<br />
						<br />
						<a href="Login.aspx">Return to login page</a> 
						<!-- Begin footer --></p>
				</td>
			</tr>
		</table>
	</body>
</HTML>
