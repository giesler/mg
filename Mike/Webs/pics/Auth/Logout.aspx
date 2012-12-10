<%@ Register TagPrefix="pics" TagName="header" Src="../Controls/_header.ascx" %>
<%@ Page language="c#" Codebehind="Logout.aspx.cs" AutoEventWireup="false" Inherits="pics.Auth.Logout" %>
<%@ Register TagPrefix="pics" TagName="sidebar" Src="../Controls/_sidebar.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>msn2.net</title>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK rel="stylesheet" type="text/css" href="../msn2.css">
	</HEAD>
	<body topmargin="0" leftmargin="0">
		<!-- top table with MSN2 logo -->
		<form runat="server" id="Login" method="post">
			<pics:header id="ctlHeader" runat="server" size="small" header="Pictures - Signout" ShowUserInfo="false"></pics:header>
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
						<blockquote>
						<p>
						</p>
						<p>
							&nbsp;
						</p>
						<p>
							You have been signed out.
						</p>
						<p>
							To sign in again, click <a href="../">here</a>.
						</p>
						</blockquote>
						<!-- Begin footer -->
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
