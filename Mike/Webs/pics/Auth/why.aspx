<%@ Register TagPrefix="pics" TagName="header" Src="../Controls/_header.ascx" %>
<%@ Register TagPrefix="pics" TagName="sidebar" Src="../Controls/_sidebar.ascx" %>
<%@ Page language="c#" Codebehind="why.aspx.cs" AutoEventWireup="false" Inherits="pics.Auth.why" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>why require logging in?</title>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../msn2.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body leftMargin="0" topMargin="0">
		<!-- top table with MSN2 logo -->
		<pics:header id="ctlHeader" header="Why require logging in?" size="small" runat="server"></pics:header>
		<table cellSpacing="0" cellPadding="0" border="0" width="100%" align="left" height="100%">
			<tr>
				<td height="3" class="msn2headerfade" colspan="3"><img src="images/blank.gif" height="3"></td>
			</tr>
			<tr>
				<td width="125" class="msn2sidebar">
					<pics:sidebar runat="server" id="Sidebar1"></pics:sidebar>
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
					<P>By knowing who you are, this site can show you appropriate things.&nbsp; For 
						example, people in my family will see family pictures on the main page.</P>
					<P>&nbsp;</P>
					<P><STRONG>And a bit on privacy...</STRONG></P>
					<P>The information stored about you that you enter on this site, including your 
						email address, are stored on a server at my house.&nbsp; I keep the server up 
						to date and secure.</P>
					<P>
						<br>
						<br>
						<a href="Login.aspx">Return to login page</a> 
						<!-- Begin footer --></P>
				</td>
			</tr>
		</table>
	</body>
</HTML>
